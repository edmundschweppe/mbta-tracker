using MbtaTracker.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.Console
{
    class Program
    {
        private string[] _args;

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
            string folder = @"C:\Users\edmund\Downloads";
            string file = @"MBTA_GTFS_20160921.zip";
            string connStr = @"data source=localhost;initial catalog=MbtaTracker;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework";

            Download dl = new Download
            {
                download_date = DateTime.Now,
                download_file_name = Path.Combine(folder, file)
            };
            dl.LoadFromZip();
            using (var db = new MbtaTrackerDb(connStr))
            {
                db.Downloads.Add(dl);
                db.SaveChanges();
            };
            return 0;
        }
    }
}
