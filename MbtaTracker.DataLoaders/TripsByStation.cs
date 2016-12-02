using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.DataLoaders
{
    // *NOT* using Entity Framework here, want to save using BulkLoad
    public class TripsByStation
    {
        /*
            [trips_by_station_id] [int] IDENTITY(1,1) NOT NULL,
            [route_id] [nvarchar](255) NOT NULL,
            [route_name] [nvarchar](255) NOT NULL,
            [trip_id] [nvarchar](255) NOT NULL,
            [trip_shortname] [nvarchar](255) NOT NULL,
            [trip_headsign] [nvarchar](255) NOT NULL,
            [trip_direction] [int] null,
            [vehicle_id] [nvarchar](255) null,
            [stop_id] [nvarchar](255) NOT NULL,
            [url_safe_stop_id] [nvarchar](255) NOT NULL,
            [stop_name] [nvarchar](255) NOT NULL,
            [sched_dep_dt] [datetime] not null,
            [pred_dt] [datetime] null,
            [pred_away] [int] null,
         */
        #region Member variables
        public int trips_by_station_id;
        public DateTime prediction_timestamp;
        public string route_id;
        public string route_name;
        public string trip_id;
        public string trip_shortname;
        public string trip_headsign;
        public int trip_direction;
        public string vehicle_id;
        public string stop_id;
        public string url_safe_stop_id;
        public string stop_name;
        public DateTime sched_dep_dt;
        public DateTime? pred_dt;
        public int? pred_away;
        #endregion Member variables

        public void AddToDataTable(DataTable dt)
        {
            DataRow r = dt.NewRow();
            r.SetField<DateTime>("prediction_timestamp", prediction_timestamp);
            r.SetField<string>("route_id", route_id);
            r.SetField<string>("route_name", route_name);
            r.SetField<string>("trip_id", trip_id);
            r.SetField<string>("trip_shortname", trip_shortname);
            r.SetField<string>("trip_headsign", trip_headsign);
            r.SetField<int>("trip_direction", trip_direction);
            r.SetField<string>("vehicle_id", vehicle_id);
            r.SetField<string>("stop_id", stop_id);
            r.SetField<string>("url_safe_stop_id", url_safe_stop_id);
            r.SetField<string>("stop_name", stop_name);
            r.SetField<DateTime>("sched_dep_dt", sched_dep_dt);
            r.SetField<DateTime?>("pred_dt", pred_dt);
            r.SetField<int?>("pred_away", pred_away);
            dt.Rows.Add(r);
        }

        public static DataTable CreateDataTable()
        {
            DataTable t = new DataTable();
            t.Columns.Add(new DataColumn("prediction_timestamp", typeof(DateTime)));
            t.Columns.Add(new DataColumn("route_id", typeof(string)));
            t.Columns.Add(new DataColumn("route_name", typeof(string)));
            t.Columns.Add(new DataColumn("trip_id", typeof(string)));
            t.Columns.Add(new DataColumn("trip_shortname", typeof(string)));
            t.Columns.Add(new DataColumn("trip_headsign", typeof(string)));
            t.Columns.Add(new DataColumn("trip_direction", typeof(int)));
            t.Columns.Add(new DataColumn("vehicle_id", typeof(string)));
            t.Columns.Add(new DataColumn("stop_id", typeof(string)));
            t.Columns.Add(new DataColumn("url_safe_stop_id", typeof(string)));
            t.Columns.Add(new DataColumn("stop_name", typeof(string)));
            t.Columns.Add(new DataColumn("sched_dep_dt", typeof(DateTime)));
            t.Columns.Add(new DataColumn("pred_dt", typeof(DateTime)));
            t.Columns.Add(new DataColumn("pred_away", typeof(int)));
            return t;
        }
    }
}
