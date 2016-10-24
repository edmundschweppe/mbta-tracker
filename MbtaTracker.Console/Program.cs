using MbtaTracker.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.Console
{
    class Program
    {
        private string[] _args;
        private string _apiKey = "wX9NwuHnZU2ToO7GmGR9uw";
        private string _connStr = @"data source=localhost;initial catalog=MbtaTracker;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

        static int Main(string[] args)
        {
            Program program = new Program(args);
            return program.Run();
        }

        public Program(string[] args)
        {
            _args = args;
        }

        public int Run()
        {
            //LoadGtfsStaticData();
            LoadMbtaRtData();
            return 0;
        }

        private void LoadGtfsStaticData()
        {
            string folder = @"C:\Users\edmund\Downloads";
            string file = @"MBTA_GTFS_20160921.zip";

            Download dl = new Download
            {
                download_date = DateTime.Now,
                download_file_name = Path.Combine(folder, file)
            };
            dl.LoadFromZip();
            using (var db = new MbtaTrackerDb(_connStr))
            {
                db.Downloads.Add(dl);
                db.SaveChanges();
            };
        }

        private void LoadMbtaRtData()
        {
            Prediction p = new Prediction
            {
                prediction_time = DateTime.UtcNow
            };
            string json = GetPredictionsByRoutesJson().Result;
            p.LoadFromJson(json);
            using (var db = new MbtaTrackerDb(_connStr))
            {
                db.Predictions.Add(p);
                db.SaveChanges();
            }
        }

        private async Task<string> GetPredictionsByRoutesJson()
        {
            string[] routes;
            using (var db = new MbtaTrackerDb(_connStr))
            {
                routes = db.Routes.Select(r => r.route_id).ToArray();
            }
                //string[] routes = { "CR-Fitchburg", "CR-Newburyport", "CR-Lowell", "CR-Haverhill" };
                string url = String.Format("http://realtime.mbta.com/developer/api/v2/predictionsbyroutes?api_key={0}&routes={1}&format=json",
                     _apiKey,
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
