using MbtaTracker.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MbtaTracker.WebApi.Controllers
{
    public class StationController : ApiController
    {
        private IMbtaTrackerDb _trackerDb = null;
        public IMbtaTrackerDb TrackerDb
        {
            get
            {
                if (_trackerDb == null)
                {
                    return new MbtaTrackerDb();
                }
                return _trackerDb;
            }
            set
            {
                _trackerDb = value;
            }
        }

        public StationItem GetById(string urlSafeStopId)
        {
            StationItem result = new StationItem
            {
                ApplicationVersion = this.GetType().Assembly.GetName().Version.ToString(),
                StationName = null,
                UrlSafeStopId = urlSafeStopId
            };
            using (var db = TrackerDb)
            {
                var dl = db.Downloads
                            .OrderByDescending(d => d.download_date)
                            .First();
                var pred = db.Predictions
                            .OrderByDescending(p => p.prediction_time)
                            .First();
                var trips = db.TripsByStations
                            .Where(t => t.url_safe_stop_id == urlSafeStopId)
                            .OrderBy(t => t.sched_dep_dt)
                            .ThenBy(t => t.trip_direction)
                            .ToList();

                result.ScheduleTimeStamp = dl.download_date;
                result.PredictionTimeStamp = pred.prediction_time;
                var trains = new List<StationTrainItem>();
                foreach (var trip in trips)
                {
                    if (result.StationName == null)
                    {
                        result.StationName = trip.stop_name;
                    }
                    trains.Add(new StationTrainItem
                    {
                        Train = trip.trip_shortname,
                        Direction = (trip.trip_direction == 0 ? "Inbound" : "Outbound"),
                        Destination = trip.trip_headsign,
                        ControlCar = trip.vehicle_id,
                        Scheduled = trip.sched_dep_dt,
                        Predicted = trip.pred_dt
                    });
                }
                result.Trains = trains;
            }
            return result;
        }
    }
}
