using MbtaTracker.DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.DataLoaders
{
    public class GtfsStaticLoader
    {
        private string _zipFileFolder;
        private string _zipFileName;
        private string _connectionString;

        /// <summary>
        /// Folder containing ZIP file with GTFS static data
        /// </summary>
        public string ZipFileFolder
        {
            get
            {
                return _zipFileFolder;
            }
            set
            {
                _zipFileFolder = value;
            }
        }

        /// <summary>
        /// Name of ZIP file containing GTFS static data
        /// </summary>
        public string ZipFileName
        {
            get
            {
                return _zipFileName;
            }
            set
            {
                _zipFileName = value;
            }
        }

        /// <summary>
        /// Full path to the GTFS static data ZIP file
        /// </summary>
        public string ZipFilePath
        {
            get
            {
                return Path.Combine(ZipFileFolder, ZipFileName);
            }
        }

        /// <summary>
        /// Database connection string to load GTFS data to
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        public void Load()
        {
            Trace.TraceInformation("LoadGtfsStaticData from file {0}", ZipFilePath);
            Download dl = new Download
            {
                download_date = DateTime.UtcNow,
                download_file_name = ZipFilePath
            };
            dl.LoadFromZip();
            using (var db = new MbtaTrackerDb(ConnectionString))
            {
                db.Downloads.Add(dl);
                Trace.TraceInformation("starting SaveChanges");
                db.SaveChanges();
                Trace.TraceInformation("SaveChanges completed");
            };
        }
    }
}
