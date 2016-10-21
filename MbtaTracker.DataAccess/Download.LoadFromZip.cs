using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.DataAccess
{
    public partial class Download
    {
        #region Support classes
        class CsvTextReader
        {
            private TextFieldParser _parser;
            private Dictionary<string, int> _headers;
            private string[] _fields;

            public CsvTextReader(Stream zipStream)
            {
                _parser = new TextFieldParser(zipStream)
                {
                    TextFieldType = FieldType.Delimited,
                    Delimiters = new string[] { "," },
                    HasFieldsEnclosedInQuotes = true
                };
                _headers = new Dictionary<string, int>();
                string[] headerRow = _parser.ReadFields();
                for (int i = 0; i < headerRow.Length; i++)
                {
                    _headers.Add(headerRow[i], i);
                }
            }

            public bool EndOfData
            {
                get
                {
                    return _parser.EndOfData;
                }
            }

            public void ReadLine()
            {
                if (EndOfData)
                {
                    throw new InvalidOperationException("at EndOfData");
                }
                _fields = _parser.ReadFields();
            }

            public string this[string columnHeader]
            {
                get
                {
                    if (!_headers.ContainsKey(columnHeader))
                    {
                        throw new ArgumentException("column not found", "columnHeader");
                    }
                    return _fields[_headers[columnHeader]];
                }
            }
        }
        #endregion

        #region Public API

        /// <summary>
        /// Utility method to load a new object from the GTFS static feed file 
        /// </summary>
        public void LoadFromZip()
        {
            using (var zipStream = new FileStream(download_file_name, FileMode.Open))
            {
                using (var archive = new ZipArchive(zipStream))
                {
                    string[] files =
                    {
                        "feed_info.txt",
                        "calendar.txt",
                        "calendar_dates.txt",
                        "routes.txt",
                        "trips.txt",
                        "stop_times.txt",
                        "stops.txt"
                    };
                    foreach(string file in files)
                    {
                        var entry = archive.Entries.FirstOrDefault(e => e.Name == file);
                        if (entry != null)
                        {
                            AddEntryToDownload(entry);
                        }
                    }
                }
            }
        }

        #endregion Public API

        #region Support methods

        private void AddEntryToDownload(ZipArchiveEntry entry)
        {
            string fileName = entry.Name;
            switch (fileName)
            {
                case "feed_info.txt":
                    AddFeedInfoToDownLoad(entry);
                    break;
                case "calendar.txt":
                    AddCalendarToDownLoad(entry);
                    break;
                case "calendar_dates.txt":
                    AddCalendarDatesToDownLoad(entry);
                    break;
                case "routes.txt":
                    AddRoutesToDownLoad(entry);
                    break;
                case "stops.txt":
                    AddStopsToDownLoad(entry);
                    break;
                case "stop_times.txt":
                    AddStopTimesToDownLoad(entry);
                    break;
                case "trips.txt":
                    AddTripsToDownload(entry);
                    break;
                default:
                    throw new Exception(String.Format("unknown file {0}", fileName));

            }
        }

        private void AddFeedInfoToDownLoad(ZipArchiveEntry entry)
        {
            CsvTextReader rdr = new CsvTextReader(entry.Open());
            if (!rdr.EndOfData)
            {
                rdr.ReadLine();
                Feed_Info item = new Feed_Info
                {
                    feed_publisher_name = rdr["feed_publisher_name"],
                    feed_publisher_url = rdr["feed_publisher_url"],
                    feed_lang = rdr["feed_lang"],
                    feed_start_date_txt = rdr["feed_start_date"],
                    feed_end_date_txt = rdr["feed_end_date"],
                    feed_version = rdr["feed_version"]
                };
                item.feed_start_date = DateTime.ParseExact(
                    item.feed_start_date_txt,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture);
                item.feed_end_date = DateTime.ParseExact(
                    item.feed_end_date_txt,
                    "yyyyMMdd",
                    CultureInfo.InvariantCulture);
                this.Feed_Info.Add(item);
            }
        }

        private void AddCalendarToDownLoad(ZipArchiveEntry entry)
        {
            CsvTextReader rdr = new CsvTextReader(entry.Open());
            while (!rdr.EndOfData)
            {
                rdr.ReadLine();
                string startDateTxt = rdr["start_date"];
                string endDateTxt = rdr["end_date"];
                Calendar item = new Calendar
                {
                    service_id = rdr["service_id"],
                    monday = Int32.Parse(rdr["monday"]),
                    tuesday = Int32.Parse(rdr["tuesday"]),
                    wednesday = Int32.Parse(rdr["wednesday"]),
                    thursday = Int32.Parse(rdr["thursday"]),
                    friday = Int32.Parse(rdr["friday"]),
                    saturday = Int32.Parse(rdr["saturday"]),
                    sunday = Int32.Parse(rdr["sunday"]),
                    start_date = DateTime.ParseExact(
                        rdr["start_date"],
                        "yyyyMMdd",
                        CultureInfo.InvariantCulture),
                    end_date = DateTime.ParseExact(
                        rdr["end_date"],
                        "yyyyMMdd",
                        CultureInfo.InvariantCulture)
                };
                this.Calendars.Add(item);
            };
        }

        private void AddCalendarDatesToDownLoad(ZipArchiveEntry entry)
        {
            CsvTextReader rdr = new CsvTextReader(entry.Open());
            while (!rdr.EndOfData)
            {
                rdr.ReadLine();
                var item = new Calendar_Dates
                {
                    service_id = rdr["service_id"],
                    exception_date = DateTime.ParseExact(
                        rdr["date"],
                        "yyyyMMdd",
                        CultureInfo.InvariantCulture),
                    exception_type = Int32.Parse(rdr["exception_type"])
                };
                this.Calendar_Dates.Add(item);
            };
        }

        private void AddRoutesToDownLoad(ZipArchiveEntry entry)
        {
            CsvTextReader rdr = new CsvTextReader(entry.Open());
            while (!rdr.EndOfData)
            {
                rdr.ReadLine();
                if (rdr["route_type"] == "2") // commuter rail only for now
                {
                    var item = new Route
                    {
                        route_id = rdr["route_id"],
                        agency_id = rdr["agency_id"],
                        route_short_name = rdr["route_short_name"],
                        route_long_name = rdr["route_long_name"],
                        route_desc = rdr["route_desc"],
                        route_type = rdr["route_type"],
                        route_url = rdr["route_url"],
                        route_color = rdr["route_color"],
                        route_text_color = rdr["route_text_color"]
                    };
                    this.Routes.Add(item);
                }
            }
        }

        private void AddStopsToDownLoad(ZipArchiveEntry entry)
        {
            string[] stopIds = this.Stop_Times.Select(st => st.stop_id).Distinct().ToArray();
            CsvTextReader rdr = new CsvTextReader(entry.Open());
            while (!rdr.EndOfData)
            {
                rdr.ReadLine();
                if (stopIds.Contains(rdr["stop_id"]))
                {
                    var item = new Stop
                    {
                        stop_id = rdr["stop_id"],
                        stop_code = rdr["stop_code"],
                        stop_name = rdr["stop_name"],
                        stop_desc = rdr["stop_desc"],
                        stop_lat_txt = rdr["stop_lat"],
                        stop_lon_txt = rdr["stop_lon"],
                        zone_id = rdr["zone_id"],
                        stop_url = rdr["stop_url"],
                        location_type = Int32.Parse(rdr["location_type"]),
                        parent_station = rdr["parent_station"],
                        wheelchair_boarding = Int32.Parse(rdr["wheelchair_boarding"])
                    };
                    this.Stops.Add(item);
                }
            }
        }

        private void AddStopTimesToDownLoad(ZipArchiveEntry entry)
        {
            string[] tripIds = this.Trips.Select(t => t.trip_id).ToArray();
            CsvTextReader rdr = new CsvTextReader(entry.Open());
            while (!rdr.EndOfData)
            {
                rdr.ReadLine();
                if (tripIds.Contains(rdr["trip_id"]))
                {
                    var item = new Stop_Times
                    {
                        trip_id = rdr["trip_id"],
                        arrival_time_txt = rdr["arrival_time"],
                        departure_time_txt = rdr["departure_time"],
                        stop_id = rdr["stop_id"],
                        stop_sequence = Int32.Parse(rdr["stop_sequence"]),
                        stop_headsign = rdr["stop_headsign"],
                        pickup_type = Int32.Parse(rdr["pickup_type"]),
                        drop_off_type = Int32.Parse(rdr["drop_off_type"])
                    };
                    this.Stop_Times.Add(item);
                }
            }
        }

        private void AddTripsToDownload(ZipArchiveEntry entry)
        {
            string[] routeIds = this.Routes.Select(r => r.route_id).ToArray();
            CsvTextReader rdr = new CsvTextReader(entry.Open());
            while (!rdr.EndOfData)
            {
                rdr.ReadLine();
                if (routeIds.Contains(rdr["route_id"]))
                {
                    var item = new Trip
                    {
                        route_id = rdr["route_id"],
                        service_id = rdr["service_id"],
                        trip_id = rdr["trip_id"],
                        trip_shortname = rdr["trip_short_name"],
                        trip_headsign = rdr["trip_headsign"],
                        direction_id = Int32.Parse(rdr["direction_id"]),
                        block_id = rdr["block_id"],
                        shape_id = rdr["shape_id"],
                        wheelchair_accessible = Int32.Parse(rdr["wheelchair_accessible"])
                    };
                    this.Trips.Add(item);
                };
            };
        }


        #endregion Support methods
    }
}
