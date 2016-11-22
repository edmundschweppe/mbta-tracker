using MbtaTracker.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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
            using (var db = new MbtaTrackerDb(ConnectionString))
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
            using (var db = new MbtaTrackerDb(connStr))
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
            using (var db = new MbtaTrackerDb(ConnectionString))
            {
                int dlId = db.Feed_Info
                    .Where(fi => fi.feed_start_date <= DateTime.Today
                                && DateTime.Today <= fi.feed_end_date)
                    .OrderByDescending(fi => fi.download_id)
                    .First()
                    .download_id;
                var currentSched = db.Downloads.First(d => d.download_id == dlId);

                // Start with predicted trips
                var currentPred = db.Predictions
                        .OrderByDescending(p => p.prediction_id)
                        .First();

                var predTrips =
                    from trip in currentPred.PredictionTrips
                    from stop in trip.PredictionTripStops
                    from vehicle in trip.PredictionTripVehicles
                    select new
                    {
                        trip,
                        stop,
                        vehicle
                    };
                Trace.TraceInformation("found {0} predicted trips", predTrips.Count());
                foreach (var predTrip in predTrips)
                {
                    TripsByStation t = new TripsByStation
                    {
                        route_id = predTrip.trip.route_id,
                        trip_id = predTrip.trip.trip_id,
                        stop_id = predTrip.stop.stop_id,
                        stop_name = predTrip.stop.stop_name,
                        vehicle_id = (predTrip.vehicle == null ? "(not reported)" : predTrip.vehicle.vehicle_id ),
                        sched_dep_dt = predTrip.stop.sch_dep_dt.ToLocalTime(),
                        pred_dt = predTrip.stop.pre_dt.ToLocalTime(),
                        pred_away = predTrip.stop.pre_away
                    };
                    t.route_name = currentSched.Routes
                        .Where(r => r.route_id == t.route_id)
                        .First()
                        .route_long_name;
                    var schedTrip = currentSched.Trips
                        .Where(st => st.trip_id == t.trip_id)
                        .First();
                    t.trip_direction = (int)schedTrip.direction_id;
                    t.trip_headsign = schedTrip.trip_headsign;
                    t.trip_shortname = schedTrip.trip_shortname;
                    t.AddToDataTable(dt);
                }

                Trace.TraceInformation("{0} predicted rows in datatable", dt.Rows.Count);

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
                    .Distinct();
                var schedStops = schedStopIds
                    .Select(s => new { stop_id = s, direction_id = 0 })
                    .Union(schedStopIds.Select(s => new { stop_id = s, direction_id = 1 }))
                    .OrderBy(s => s.stop_id).ThenBy(s => s.direction_id);

                foreach (var s in schedStops)
                {
                    if (!predStops.Contains(s))
                    {
                        var schedStopTrips = currentSched.Stop_Times
                            .Where(st => st.stop_id == s.stop_id
                                    && tripIdsByDirection[s.direction_id].Contains(st.trip_id)
                                    && DateFromSchedStopTime(st.departure_time_txt) > DateTime.Now)
                            .OrderBy(st => DateFromSchedStopTime(st.departure_time_txt));
                        var stopTripsToAdd = schedStopTrips.Where((st, index) => 
                            (index == 0) 
                            || (DateFromSchedStopTime(st.departure_time_txt) <= DateTime.Now.AddHours(1)));
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
                                route_id = route.route_id,
                                route_name = route.route_long_name,
                                trip_id = trip.trip_id,
                                trip_direction = (int)trip.direction_id,
                                trip_shortname = trip.trip_shortname,
                                trip_headsign = trip.trip_headsign,
                                stop_id = stop.stop_id,
                                stop_name = stop.stop_name,
                                sched_dep_dt = DateFromSchedStopTime(addMe.departure_time_txt),
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

        private DateTime DateFromSchedStopTime(string stopTimeText)
        {
            var parsed = stopTimeText.Split(':');
            int hour = Convert.ToInt32(parsed[0]);
            int min = Convert.ToInt32(parsed[1]);
            int sec = Convert.ToInt32(parsed[2]);
            return DateTime.Today.AddHours(hour).AddMinutes(min).AddSeconds(sec);

        }
    }
}
