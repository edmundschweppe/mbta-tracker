using MbtaTracker.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            Prediction p = new Prediction
            {
                prediction_time = DateTime.UtcNow,
                prediction_json = json
            };
            Trace.TraceInformation("loading json");

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
            string[] routes;
            using (var db = new MbtaTrackerDb(connStr))
            {
                routes = db.Routes.Select(r => r.route_id).ToArray();
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
