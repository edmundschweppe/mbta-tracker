using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.DataLoaders
{
    public interface IBulkCopyHelper
    {
        void BulkLoadData(string connString, string targetTable, System.Data.DataTable dataToLoad);
    }

    public class BulkCopyHelper : IBulkCopyHelper
    {
        public void BulkLoadData(string connString, string targetTable, DataTable dataToLoad)
        {
            using (SqlConnection conn = new SqlConnection(connString))
            {
                string truncSql = String.Format("TRUNCATE TABLE {0}", targetTable);
                string statsSql = String.Format("UPDATE STATISTICS {0}", targetTable);
                conn.Open();
                SqlCommand trunc = new SqlCommand(truncSql, conn);
                trunc.ExecuteNonQuery();
                using (SqlBulkCopy bc = new SqlBulkCopy(conn))
                {
                    bc.DestinationTableName = targetTable;
                    foreach(DataColumn col in dataToLoad.Columns)
                    {
                        bc.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                    }
                    //bc.ColumnMappings.Add("route_id", "route_id");
                    //bc.ColumnMappings.Add("route_name", "route_name");
                    //bc.ColumnMappings.Add("trip_id", "trip_id");
                    //bc.ColumnMappings.Add("trip_shortname", "trip_shortname");
                    //bc.ColumnMappings.Add("trip_headsign", "trip_headsign");
                    //bc.ColumnMappings.Add("trip_direction", "trip_direction");
                    //bc.ColumnMappings.Add("vehicle_id", "vehicle_id");
                    //bc.ColumnMappings.Add("stop_id", "stop_id");
                    //bc.ColumnMappings.Add("stop_name", "stop_name");
                    //bc.ColumnMappings.Add("sched_dep_dt", "sched_dep_dt");
                    //bc.ColumnMappings.Add("pred_dt", "pred_dt");
                    //bc.ColumnMappings.Add("pred_away", "pred_away");
                    bc.WriteToServer(dataToLoad);
                }
                SqlCommand stats = new SqlCommand(statsSql, conn);
                stats.ExecuteNonQuery();
            }
        }
    }
}
