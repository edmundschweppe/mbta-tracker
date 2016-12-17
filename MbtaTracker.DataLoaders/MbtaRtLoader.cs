using MbtaTracker.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.DataLoaders
{
    public class MbtaRtLoader
    {
        public string ApiKey;
        public string ConnectionString;
        public string LatestRouteList { get; private set; }

        private IBulkCopyHelper _bulkHelper;
        private IMbtaTrackerDb _trackerDb;

        public IBulkCopyHelper BulkHelper
        {
            get
            {
                if (_bulkHelper == null)
                {
                    _bulkHelper = new BulkCopyHelper();
                }
                return _bulkHelper;
            }
            set
            {
                _bulkHelper = value;
            }
        }
        public IMbtaTrackerDb TrackerDb
        {
            get
            {
                if (_trackerDb == null)
                {
                    return new MbtaTrackerDb(ConnectionString);
                }
                return _trackerDb;
            }
            set
            {
                _trackerDb = value;
            }
        }

        public string Version
        {
            get
            {
                return Assembly.GetAssembly(this.GetType())
                    .GetName()
                    .Version
                    .ToString();
            }
        }

        public void Load()
        {
            Trace.TraceInformation("getting json");
            string json = GetPredictionsByRoutesJson(ApiKey, ConnectionString).Result;

            LoadJson(json);
        }

        public void LoadFromFile(string jsonFilePath)
        {
            string json = File.ReadAllText(jsonFilePath);
            LoadJson(json);
        }

        private void LoadJson(string json)
        {
            Trace.TraceInformation("loading json");
            Prediction p = new Prediction
            {
                prediction_time = DateTime.UtcNow,
                prediction_json = json
            };
            p.LoadFromJson();

            Trace.TraceInformation("saving to database");
            using (var db = this.TrackerDb)
            {
                db.Predictions.Add(p);
                db.SaveChanges();
            }
        }

        private async Task<string> GetPredictionsByRoutesJson(string apiKey, string connStr)
        {
            /*
             select distinct t.route_id 
from GtfsStatic.Trips t
join GtfsStatic.Calendar c on c.service_id = t.service_id
where t.download_id = 3
and c.download_id = 3
and getdate() between c.start_date and c.end_date
             
             */
            string[] routes;
            //using (var db = new MbtaTrackerDb(connStr))
            //{
            //    routes = db.Routes.Select(r => r.route_id).ToArray();
            //}
            using (var db = this.TrackerDb)
            {
                int dlId = db.Feed_Info.Where(fi => fi.feed_start_date <= DateTime.Today && DateTime.Today <= fi.feed_end_date)
                    .OrderByDescending(fi => fi.download_id)
                    .FirstOrDefault()
                    .download_id;
                routes = db.Trips.Where(t => t.download_id == dlId)
                    .Join(db.Calendars.Where( c=> c.download_id == dlId && c.start_date <= DateTime.Today && DateTime.Today <= c.end_date),
                        t => new { t.download_id, t.service_id },
                        c => new { c.download_id, c.service_id },
                        (t, c) => t.route_id
                        )
                    .Distinct()
                    .ToArray();
            }
            LatestRouteList = String.Join(",", routes);
            Trace.TraceInformation("routes: \"{0}\"", LatestRouteList);
            //string[] routes = { "CR-Fitchburg", "CR-Newburyport", "CR-Lowell", "CR-Haverhill" };
            string url = String.Format("http://realtime.mbta.com/developer/api/v2/predictionsbyroutes?api_key={0}&routes={1}&format=json",
                 apiKey,
                 //String.Join(",", routes));
                 LatestRouteList);
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }

        public void ReloadDenormalizedTables()
        {
            DataTable dt = TripsByStation.CreateDataTable();
            using (var db = this.TrackerDb)
            {
                int dlId = db.Feed_Info
                    .Where(fi => fi.feed_start_date <= DateTime.Today
                                && DateTime.Today <= fi.feed_end_date)
                    .OrderByDescending(fi => fi.download_id)
                    .First()
                    .download_id;
                var currentSched = db.Downloads.Single(d => d.download_id == dlId);

                // Start with predicted trips
                var currentPred = db.Predictions
                        .OrderByDescending(p => p.prediction_id)
                        .First();
                Trace.TraceInformation("found {0} predicted trips", currentPred.PredictionTrips.Count());
                foreach (var predTrip in currentPred.PredictionTrips)
                {
                    string route_long_name = currentSched.Routes
                        .Where(r => r.route_id == predTrip.route_id)
                        .First()
                        .route_long_name;
                    var schedTrip = currentSched.Trips
                        .Where(st => st.trip_id == predTrip.trip_id)
                        .First();
                    string vehicleId = "(not reported)";
                    if (predTrip.PredictionTripVehicles.Count() > 0)
                    {
                        vehicleId = predTrip.PredictionTripVehicles.First().vehicle_id;
                    }
                    foreach (var predTripStop in predTrip.PredictionTripStops)
                    {
                        TripsByStation t = new TripsByStation
                        {
                            prediction_timestamp = currentPred.prediction_time,
                            route_id = predTrip.route_id,
                            route_name = route_long_name,
                            trip_id = predTrip.trip_id,
                            trip_direction = (int)schedTrip.direction_id,
                            trip_headsign = schedTrip.trip_headsign,
                            trip_shortname = schedTrip.trip_shortname,
                            stop_id = predTripStop.stop_id,
                            url_safe_stop_id = UrlSafeStopId(predTripStop.stop_id),
                            stop_name = predTripStop.stop_name,
                            vehicle_id = vehicleId,
                            sched_dep_dt = predTripStop.sch_dep_dt,
                            pred_dt = predTripStop.pre_dt,
                            pred_away = predTripStop.pre_away
                        };
                        t.AddToDataTable(dt);
                    }
                }

                Trace.TraceInformation("{0} prediction rows in datatable", dt.Rows.Count);

                // For any station without predicted trips, add at least one scheduled trip
                var predStops = dt.AsEnumerable()
                    .Select(r => new { stop_id = r.Field<string>("stop_id"), direction_id = r.Field<int>("trip_direction") })
                    .Distinct()
                    .OrderBy(s => s.stop_id).ThenBy(s => s.direction_id);

                var noServiceIds = currentSched.Calendar_Dates
                    .Where(cd => cd.exception_date == DateTime.Today && cd.exception_type == 2)
                    .Select(cd => cd.service_id)
                    .ToList();
                var schedServiceIds = currentSched.Calendars
                    .Where(c => c.ServiceIsValid(DateTime.Today)
                            && !noServiceIds.Contains(c.service_id))
                    .Select(c => c.service_id)
                    .Union(
                        currentSched.Calendar_Dates
                        .Where(cd => cd.exception_date == DateTime.Today && cd.exception_type == 1)
                        .Select(cd => cd.service_id)
                        )
                    .ToList();
              
                var schedTripIds = currentSched.Trips
                    .Where(t => schedServiceIds.Contains(t.service_id))
                    .Select(t => t.trip_id)
                    .ToList();
                Dictionary<int, List<string>> tripIdsByDirection = new Dictionary<int, List<string>>();
                tripIdsByDirection[0] = currentSched.Trips
                    .Where(t => schedServiceIds.Contains(t.service_id) && t.direction_id == 0)
                    .Select(t => t.trip_id)
                    .ToList();
                tripIdsByDirection[1] = currentSched.Trips
                    .Where(t => schedServiceIds.Contains(t.service_id) && t.direction_id == 1)
                    .Select(t => t.trip_id)
                    .ToList();
                var schedStopIds = currentSched.Stops
                    .Select(s => s.stop_id)
                    .Distinct()
                    .ToList();
                var schedStops = schedStopIds
                    .Select(s => new { stop_id = s, direction_id = 0 })
                    .Union(schedStopIds.Select(s => new { stop_id = s, direction_id = 1 }))
                    .OrderBy(s => s.stop_id).ThenBy(s => s.direction_id)
                    .ToList();

                foreach (var s in schedStops)
                {
                    if (!predStops.Contains(s))
                    {
                        var schedStopTrips = currentSched.Stop_Times
                            .Where(st => st.stop_id == s.stop_id
                                    && tripIdsByDirection[s.direction_id].Contains(st.trip_id)
                                    && UtcDateFromScheduledStopTime(st.departure_time_txt) > DateTime.UtcNow)
                            .OrderBy(st => UtcDateFromScheduledStopTime(st.departure_time_txt))
                            .ToList();
                        var stopTripsToAdd = schedStopTrips.Where((st, index) => 
                            (index == 0) 
                            || (UtcDateFromScheduledStopTime(st.departure_time_txt) <= DateTime.UtcNow.AddHours(1)))
                            .ToList();
                        foreach(var addMe in stopTripsToAdd)
                        {
                            var stop = currentSched.Stops
                                .Where(cs => cs.stop_id == addMe.stop_id)
                                .First();
                            var trip = currentSched.Trips
                                .Where(t => t.trip_id == addMe.trip_id)
                                .First();
                            var route = currentSched.Routes
                                .Where(r => r.route_id == trip.route_id)
                                .First();
                            TripsByStation ts = new TripsByStation
                            {
                                prediction_timestamp = currentPred.prediction_time,
                                route_id = route.route_id,
                                route_name = route.route_long_name,
                                trip_id = trip.trip_id,
                                trip_direction = (int)trip.direction_id,
                                trip_shortname = trip.trip_shortname,
                                trip_headsign = trip.trip_headsign,
                                stop_id = stop.stop_id,
                                url_safe_stop_id = UrlSafeStopId(stop.stop_id),
                                stop_name = stop.stop_name,
                                sched_dep_dt = UtcDateFromScheduledStopTime(addMe.departure_time_txt),
                                vehicle_id = null,
                                pred_dt = null,
                                pred_away = null
                            };
                            ts.AddToDataTable(dt);
                        }
                    }
                }

                //Trace.TraceInformation("{0} stops with predictions out of {1} scheduled", 
                //    predStops.Count(), schedStops.Count());
            }

            Trace.TraceInformation("Total of {0} trips by station", dt.Rows.Count);

            // Now reload the TripsByStation table
            this.BulkHelper.BulkLoadData(ConnectionString, "Display.TripsByStation", dt);
        }

        // Future TODO: read the timezone from the agency.txt file,
        // convert from "TZ" database timezone name to .NET TimeZoneInfo TimeZoneId,
        // and use that to figure out "now" in MBTA-land
        public static string MbtaTimeZoneId
        {
            get
            {
                return "Eastern Standard Time";
            }
        }
        /// <summary>
        /// Today's date, at the specified time, in the same time zone as the MBTA, converted to UTC
        /// </summary>
        /// <param name="stopTimeText">String to convert, in hh:mm:ss format</param>
        /// <returns>The specified DateTime</returns>
        /// <remarks>If the hours specified are 24 or greater, returns tomorrow at (hh-24:mm:ss)</remarks>
        public static DateTime UtcDateFromScheduledStopTime(string stopTimeText)
        {
            var parsed = stopTimeText.Split(':');
            int hour = Convert.ToInt32(parsed[0]);
            int min = Convert.ToInt32(parsed[1]);
            int sec = Convert.ToInt32(parsed[2]);
            DateTime schedDay = DateTime.Today;
            if (hour >= 24)
            {
                hour -= 24;
                schedDay = schedDay.AddDays(1);
            }
            int year = schedDay.Year;
            int month = schedDay.Month;
            int day = schedDay.Day;
            DateTime stopTime = new DateTime(year, month, day, hour, min, sec, DateTimeKind.Unspecified);
            DateTime utcStopTime = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(stopTime, MbtaTimeZoneId, TimeZoneInfo.Utc.Id);
            return utcStopTime;
            //return new DateTime(year, month, day, hour, min, sec, DateTimeKind.Local);
        }

        /// <summary>
        /// Transforms the stop ID into an equivalent that is safe for use in MVC URLs
        /// </summary>
        /// <param name="stop_id"></param>
        /// <returns>The stop_id, with all spaces removed and any "/" converted to "Slash"</returns>
        /// <remarks>We need this because ASP.NET MVC routing gets confused by embedded slashes,
        /// and the MBTA uses things like "Littleton / Rte 495' as stop_ids.</remarks>
        public static string UrlSafeStopId(string stop_id)
        {
            return stop_id.Replace("/", "Slash").Replace(" ", "");
        }
    }
}
