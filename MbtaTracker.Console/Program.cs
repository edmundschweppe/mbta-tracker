#define TRACE

using MbtaTracker.DataAccess;
using MbtaTracker.DataLoaders;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
            if (_args.Length > 0)
            {
                switch(_args[0].ToLowerInvariant())
                {
                    case "/loadstatic":
                        LoadGtfsStaticData();
                        break;
                    case "/loadrealtime":
                    case "/loadrt":
                        LoadMbtaRtData();
                        break;
                    default:
                        Trace.TraceError("Invalid option {0}", _args[0]);
                        return 1;
                }
            }
            else
            {
                Trace.TraceError("Either /loadstatic or /loadrt required");
                return 1;
            }

            return 0;
        }

        private void LoadGtfsStaticData()
        {
            string folder = ConfigurationManager.AppSettings["GtfsStaticFolder"];
            string file = ConfigurationManager.AppSettings["GtfsStaticZipFile"];
            string connStr = ConfigurationManager.ConnectionStrings["MbtaTracker"].ConnectionString;

            GtfsStaticLoader l = new GtfsStaticLoader
            {
                ZipFileFolder = folder,
                ZipFileName = file,
                ConnectionString = connStr
            };
            Trace.TraceInformation("GtfsStaticLoader version " + l.Version);

            l.Load();
        }

        private void LoadMbtaRtData()
        {
            string apiKey = ConfigurationManager.AppSettings["MbtaRealtimeApiKey"];
            string connStr = ConfigurationManager.ConnectionStrings["MbtaTracker"].ConnectionString;

            MbtaRtLoader l = new MbtaRtLoader
            {
                ApiKey = apiKey,
                ConnectionString = connStr
            };
            Trace.TraceInformation("MbtaRtLoader version " + l.Version);

            l.Load();
            l.ReloadDenormalizedTables();
        }
    }
}
