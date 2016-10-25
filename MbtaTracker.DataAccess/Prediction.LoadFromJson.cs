using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.DataAccess
{
    public partial class Prediction
    {
        /// <summary>
        /// Utility method to populate a Prediction entity from 
        /// a call to MBTA realtime 2.0 API predictionsbyroute
        /// </summary>
        public void LoadFromJson()
        {
            string json = prediction_json;
            var jsonPred = JsonConvert.DeserializeObject<PredictionsByRoutesJson.Rootobject>(json);
            if (jsonPred != null)
            {
                if (jsonPred.mode != null)
                {
                    foreach (var jsonMode in jsonPred.mode)
                    {
                        foreach (var jsonRoute in jsonMode.route)
                        {
                            foreach (var jsonDir in jsonRoute.direction)
                            {
                                foreach (var jsonTrip in jsonDir.trip)
                                {
                                    PredictionTrip pt = new PredictionTrip
                                    {
                                        route_id = jsonRoute.route_id,
                                        direction_id = jsonDir.direction_id,
                                        trip_id = jsonTrip.trip_id,
                                        trip_name = jsonTrip.trip_name,
                                        trip_headsign = jsonTrip.trip_headsign
                                    };
                                    if (jsonTrip.vehicle != null)
                                    {
                                        var jsonVehicle = jsonTrip.vehicle;
                                        PredictionTripVehicle ptv = new PredictionTripVehicle
                                        {
                                            vehicle_id = jsonVehicle.vehicle_id,
                                            vehicle_lat = double.Parse(jsonVehicle.vehicle_lat),
                                            vehicle_lon = double.Parse(jsonVehicle.vehicle_lon),
                                            vehicle_bearing = double.Parse(jsonVehicle.vehicle_bearing),
                                            vehicle_speed = double.Parse(jsonVehicle.vehicle_speed),
                                            vehicle_timestamp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jsonVehicle.vehicle_timestamp)).UtcDateTime
                                        };
                                        pt.PredictionTripVehicles.Add(ptv);
                                    }
                                    if (jsonTrip.stop != null)
                                    {
                                        foreach(var jsonStop in jsonTrip.stop)
                                        {
                                            PredictionTripStop pts = new PredictionTripStop
                                            {
                                                stop_id = jsonStop.stop_id,
                                                stop_name = jsonStop.stop_name,
                                                stop_sequence = int.Parse(jsonStop.stop_sequence),
                                                sch_arr_dt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jsonStop.sch_arr_dt)).UtcDateTime,
                                                sch_dep_dt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jsonStop.sch_dep_dt)).UtcDateTime,
                                                pre_dt = DateTimeOffset.FromUnixTimeSeconds(long.Parse(jsonStop.pre_dt)).UtcDateTime,
                                                pre_away = int.Parse(jsonStop.pre_away)
                                            };
                                            pt.PredictionTripStops.Add(pts);
                                        }
                                    }
                                    this.PredictionTrips.Add(pt);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
