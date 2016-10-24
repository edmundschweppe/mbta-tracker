using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.DataAccess.PredictionsByRoutesJson
{
    public class Rootobject
    {
        public Mode[] mode { get; set; }
        public Alert_Headers[] alert_headers { get; set; }
    }

    public class Mode
    {
        public string route_type { get; set; }
        public string mode_name { get; set; }
        public Route[] route { get; set; }
    }

    public class Route
    {
        public string route_id { get; set; }
        public string route_name { get; set; }
        public Direction[] direction { get; set; }
    }

    public class Direction
    {
        public string direction_id { get; set; }
        public string direction_name { get; set; }
        public Trip[] trip { get; set; }
    }

    public class Trip
    {
        public string trip_id { get; set; }
        public string trip_name { get; set; }
        public string trip_headsign { get; set; }
        public Vehicle vehicle { get; set; }
        public Stop[] stop { get; set; }
    }

    public class Vehicle
    {
        public string vehicle_id { get; set; }
        public string vehicle_lat { get; set; }
        public string vehicle_lon { get; set; }
        public string vehicle_bearing { get; set; }
        public string vehicle_speed { get; set; }
        public string vehicle_timestamp { get; set; }
    }

    public class Stop
    {
        public string stop_sequence { get; set; }
        public string stop_id { get; set; }
        public string stop_name { get; set; }
        public string sch_arr_dt { get; set; }
        public string sch_dep_dt { get; set; }
        public string pre_dt { get; set; }
        public string pre_away { get; set; }
    }

    public class Alert_Headers
    {
        public int alert_id { get; set; }
        public string header_text { get; set; }
        public string effect_name { get; set; }
    }

}
