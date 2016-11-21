using MbtaTracker.DataAccess;
using System;
using System.Collections.Generic;
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
                //string[] routes = { "CR-Fitchburg", "CR-Newburyport", "CR-Lowell", "CR-Haverhill" };
                string url = String.Format("http://realtime.mbta.com/developer/api/v2/predictionsbyroutes?api_key={0}&routes={1}&format=json",
                     apiKey,
                     String.Join(",", routes));
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                return result;
            }
        }
    }
}
