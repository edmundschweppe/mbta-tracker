using MbtaTracker.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.UnitTests
{
    [TestClass]
    public class DataAccess_DownloadTests
    {
        #region Test support classes

        class GtfsStaticFeedHelper
        {
            public string TargetFolder;
            public string TargetFileName;
            public string TargetFilePath
            {
                get
                {
                    return Path.Combine(TargetFolder, TargetFileName);
                }
            }
            public List<TestFeedInfo> FeedInfos = new List<TestFeedInfo>();
            public List<TestCalendar> Calendars = new List<TestCalendar>();
            public List<TestCalendarDate> CalendarDates = new List<TestCalendarDate>();
            public List<TestRoute> Routes = new List<TestRoute>();
            public List<TestTrip> Trips = new List<TestTrip>();
            public List<TestStopTime> StopTimes = new List<TestStopTime>();
            public List<TestStop> Stops = new List<TestStop>();

            public void MakeZipFile()
            {
                if (File.Exists(TargetFilePath))
                {
                    File.Delete(TargetFilePath);
                }
                using (var zipStream = new FileStream(TargetFilePath, FileMode.Create))
                {
                    using (var zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create))
                    {
                        if (FeedInfos.Count() > 0)
                        {
                            var feedInfoEntry = zipArchive.CreateEntry("feed_info.txt");
                            using (TextWriter tw = new StreamWriter(feedInfoEntry.Open()))
                            {
                                tw.WriteLine(TestFeedInfo.Header);
                                foreach (var feedInfo in FeedInfos)
                                {
                                    tw.WriteLine(feedInfo.DataLine);
                                }
                            }
                        }
                        if (Calendars.Count > 0)
                        {
                            var calEntry = zipArchive.CreateEntry("calendar.txt");
                            using (TextWriter tw = new StreamWriter(calEntry.Open()))
                            {
                                tw.WriteLine(TestCalendar.Header);
                                foreach(var cal in Calendars)
                                {
                                    tw.WriteLine(cal.Dataline);
                                }
                            }
                        }
                        if (CalendarDates.Count > 0)
                        {
                            var calDateEntry = zipArchive.CreateEntry("calendar_dates.txt");
                            using (TextWriter tw = new StreamWriter(calDateEntry.Open()))
                            {
                                tw.WriteLine(TestCalendarDate.Header);
                                foreach (var calDate in CalendarDates)
                                {
                                    tw.WriteLine(calDate.Dataline);
                                }
                            }
                        }
                        if (Routes.Count > 0)
                        {
                            var routeEntry = zipArchive.CreateEntry("routes.txt");
                            using (TextWriter tw = new StreamWriter(routeEntry.Open()))
                            {
                                tw.WriteLine(TestRoute.Header);
                                foreach(var route in Routes)
                                {
                                    tw.WriteLine(route.Dataline);
                                }
                            }
                        }
                        if (Stops.Count > 0)
                        {
                            var stopEntry = zipArchive.CreateEntry("stops.txt");
                            using (TextWriter tw = new StreamWriter(stopEntry.Open()))
                            {
                                tw.WriteLine(TestStop.Header);
                                foreach(var stop in Stops)
                                {
                                    tw.WriteLine(stop.Dataline);
                                }
                            }
                        }
                        if (Trips.Count > 0)
                        {
                            var tripEntry = zipArchive.CreateEntry("trips.txt");
                            using (TextWriter tw = new StreamWriter(tripEntry.Open()))
                            {
                                tw.WriteLine(TestTrip.Header);
                                foreach(var trip in Trips)
                                {
                                    tw.WriteLine(trip.Dataline);
                                }
                            }
                        }
                        if (StopTimes.Count > 0)
                        {
                            var stopTimeEntry = zipArchive.CreateEntry("stop_times.txt");
                            using (TextWriter tw = new StreamWriter(stopTimeEntry.Open()))
                            {
                                tw.WriteLine(TestStopTime.Header);
                                foreach (var stopTime in StopTimes)
                                {
                                    tw.WriteLine(stopTime.Dataline);
                                }
                            }
                        }
                    }
                }
            }
        }

        class TestFeedInfo
        {
            public string PublisherName;
            public string PublisherUrl;
            public string Lang;
            public DateTime StartDate;
            public DateTime EndDate;
            public string Version;

            public static string Header
            {
                get
                {
                    return @"""feed_publisher_name"",""feed_publisher_url"",""feed_lang"",""feed_start_date"",""feed_end_date"",""feed_version""";
                }
            }
            public string DataLine
            {
                get
                {
                    return String.Format(@"""{0}"",""{1}"",""{2}"",{3:yyyyMMdd},{4:yyyyMMdd},""{5}""",
                        PublisherName,
                        PublisherUrl,
                        Lang,
                        StartDate,
                        EndDate,
                        Version);
                }
            }

            public static TestFeedInfo Sample
            {
                get
                {
                    return new TestFeedInfo
                    {
                        PublisherName = "Sample",
                        PublisherUrl = "http://www.sample.foo",
                        Lang = "EN",
                        StartDate = DateTime.Today,
                        EndDate = DateTime.Today.AddMonths(4).AddDays(-1),
                        Version = "Sample version"
                    };
                }
            }
        }

        class TestCalendar
        {
            public string ServiceId;
            public int Monday;
            public int Tuesday;
            public int Wednesday;
            public int Thursday;
            public int Friday;
            public int Saturday;
            public int Sunday;
            public DateTime StartDate;
            public DateTime EndDate;

            public static string Header
            {
                get
                {
                    return @"""service_id"",""monday"",""tuesday"",""wednesday"",""thursday"",""friday"",""saturday"",""sunday"",""start_date"",""end_date""";
                }
            }

            public string Dataline
            {
                get
                {
                    return String.Format(@"""{0}"",{1},{2},{3},{4},{5},{6},{7},{8:yyyyMMdd},{9:yyyyMMdd}",
                        ServiceId,
                        Monday,
                        Tuesday,
                        Wednesday,
                        Thursday,
                        Friday,
                        Saturday,
                        Sunday,
                        StartDate,
                        EndDate
                        );
                }
            }
        }

        class TestCalendarDate
        {
            public string ServiceId;
            public DateTime Date;
            public int ExceptionType;

            public static string Header
            {
                get
                {
                    return @"""service_id"",""date"",""exception_type""";
                }
            }

            public string Dataline
            {
                get
                {
                    return String.Format(@"""{0}"",{1:yyyyMMdd},{2}",
                        ServiceId,
                        Date,
                        ExceptionType);
                }
            }
        }

        class TestRoute
        {
            public string RouteId;
            public string AgencyId;
            public string RouteShortName;
            public string RouteLongName;
            public string RouteDesc;
            public string RouteType;
            public string RouteUrl;
            public string RouteColor;
            public string RouteTextColor;
            // not defined in Google documentation but present in MBTA version
            public int RouteSortOrder;

            public static string Header
            {
                get
                {
                    return @"""route_id"",""agency_id"",""route_short_name"",""route_long_name"",""route_desc"",""route_type"",""route_url"",""route_color"",""route_text_color"",""route_sort_order""";
                }
            }

            public string Dataline
            {
                get
                {
                    return String.Format(@"""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",{5},""{6}"",""{7}"",""{8}"",{9}",
                        RouteId,
                        AgencyId,
                        RouteShortName,
                        RouteLongName,
                        RouteDesc,
                        RouteType,
                        RouteUrl,
                        RouteColor,
                        RouteTextColor,
                        RouteSortOrder
                        );
                }
            }

            public static TestRoute SampleCommuterRail
            {
                get
                {
                    return new TestRoute
                    {
                        RouteId = "CR-Fitchburg",
                        AgencyId = "1",
                        RouteShortName = "",
                        RouteLongName = "Fitchburg Line",
                        RouteDesc = "Commuter Rail",
                        RouteType = "2",
                        RouteUrl = "",
                        RouteColor = "8B118F",
                        RouteTextColor = "FFFFFF",
                        RouteSortOrder = 52
                    };
                }
            }

            public static TestRoute SampleBus
            {
                get
                {
                    return new TestRoute
                    {
                        RouteId = "1",
                        AgencyId = "1",
                        RouteShortName = "1",
                        RouteLongName = "",
                        RouteDesc = "Key Bus Route (Frequent Service)",
                        RouteType = "3",
                        RouteUrl = "",
                        RouteColor = "FFFF7C",
                        RouteTextColor = "000000",
                        RouteSortOrder = 100
                    };
                }
            }
        }

        class TestTrip
        {
            public string RouteId;
            public string ServiceId;
            public string TripId;
            public string TripHeadsign;
            public string TripShortName;
            public int DirectionId;
            public string BlockId;
            public string ShapeId;
            public int WheelchairAccessible;
            // defined in Google documentation but not present in MBTA feed
            public int BikesAllowed;

            public static string Header
            {
                get
                {
                    return @"""route_id"",""service_id"",""trip_id"",""trip_headsign"",""trip_short_name"",""direction_id"",""block_id"",""shape_id"",""wheelchair_accessible""";
                }
            }

            public string Dataline
            {
                get
                {
                    return String.Format(@"""{0}"",""{1}"",""{2}"",""{3}"",""{4}"",{5},""{6}"",""{7}"",{8}",
                        RouteId,
                        ServiceId,
                        TripId,
                        TripHeadsign,
                        TripShortName,
                        DirectionId,
                        BlockId,
                        ShapeId,
                        WheelchairAccessible);
                }
            }
            public static TestTrip SampleRailTrip
            {
                get
                {
                    return new TestTrip
                    {
                        RouteId = TestRoute.SampleCommuterRail.RouteId,
                        ServiceId = "cr-weekday",
                        TripId = "30991975-CR_MAY2016-hxf16011-Weekday-01",
                        TripHeadsign = "North Station",
                        TripShortName = "408",
                        DirectionId = 1,
                        BlockId = String.Empty,
                        ShapeId = "9840001",
                        WheelchairAccessible = 1,
                        BikesAllowed = 0
                    };
                }
            }
            public static TestTrip SampleBusTrip
            {
                get
                {
                    return new TestTrip
                    {
                        RouteId = TestRoute.SampleBus.RouteId,
                        ServiceId = "bus_weekday",
                        TripId = "BUS32016-hbc36011-Weekday-02",
                        TripHeadsign = "31211394",
                        TripShortName = "Dudley",
                        DirectionId = 1,
                        BlockId = "C01-10",
                        ShapeId = "010058",
                        WheelchairAccessible = 1,
                        BikesAllowed = 0
                    };
                }
            }
        }

        class TestStopTime
        {
            public string TripId;
            public string ArrivalTime;
            public string DepartureTime;
            public string StopId;
            public int StopSequence;
            public string StopHeadsign;
            public int PickupType;
            public int DropoffType;

            public static string Header
            {
                get
                {
                    return @"""trip_id"",""arrival_time"",""departure_time"",""stop_id"",""stop_sequence"",""stop_headsign"",""pickup_type"",""drop_off_type""";
                }
            }
            public string Dataline
            {
                get
                {
                    return String.Format(@"""{0}"",""{1}"",""{2}"",""{3}"",{4},""{5}"",{6},{7}",
                        TripId,
                        ArrivalTime,
                        DepartureTime,
                        StopId,
                        StopSequence,
                        StopHeadsign,
                        PickupType,
                        DropoffType
                        );
                }
            }
            public static TestStopTime SampleRailStopTime
            {
                get
                {
                    return new TestStopTime
                    {
                        TripId = TestTrip.SampleRailTrip.TripId,
                        ArrivalTime = "08:00:00",
                        DepartureTime = "08:01:00",
                        StopId = "sampleX",
                        StopSequence = 3,
                        StopHeadsign = "",
                        PickupType = 0,
                        DropoffType = 0
                    };
                }
            }
        }

        class TestStop
        {
            public string StopId;
            public string StopCode;
            public string StopName;
            public string StopDesc;
            public string StopLat;
            public string StopLon;
            public string ZoneId;
            public string StopUrl;
            public int LocationType;
            public string ParentStation;
            public int WheelchairBoarding;

            public static string Header
            {
                get
                {
                    return @"""stop_id"",""stop_code"",""stop_name"",""stop_desc"",""stop_lat"",""stop_lon"",""zone_id"",""stop_url"",""location_type"",""parent_station"",""wheelchair_boarding""";
                }
            }
            public string Dataline
            {
                get
                {
                    return String.Format(@"""{0}"",""{1}"",""{2}"",""{3}"",{4},{5},""{6}"",""{7}"",{8},""{9}"",{10}",
                        StopId,
                        StopCode,
                        StopName,
                        StopDesc,
                        StopLat,
                        StopLon,
                        ZoneId,
                        StopUrl,
                        LocationType,
                        ParentStation,
                        WheelchairBoarding);
                }
            }

        }

        #endregion Test support classes

        #region Member variables
        private GtfsStaticFeedHelper _helper;
        #endregion Member variables

        #region Setup and teardown methods
        [TestInitialize]
        public void Setup()
        {
            _helper = new GtfsStaticFeedHelper
            {
                TargetFolder = @"C:\Users\edmund\Source\Repos\MbtaTracker\testdata"
            };
        }
        #endregion Setup and teardown methods

        #region Tests

        #region Feed_Info.txt tests
        [TestMethod]
        public void LoadFromZip_FeedInfoOnly()
        {
            TestFeedInfo fi = new TestFeedInfo
            {
                PublisherName = "TestOMatic",
                PublisherUrl = "http://www.example.com",
                Lang = "EN",
                StartDate = new DateTime(2016, 9, 1),
                EndDate = new DateTime(2016, 12, 31),
                Version = "My! Version! 1/2/3/4"
            };
            _helper.TargetFileName = "feedonly.zip";
            _helper.FeedInfos.Add(fi);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Feed_Info.Count(), "checking Feed_Info count");
            var actual = target.Feed_Info.First();
            Assert.AreEqual(fi.PublisherName, actual.feed_publisher_name, "checking feed_publisher_name");
            Assert.AreEqual(fi.PublisherUrl, actual.feed_publisher_url, "checking feed_publisher_url");
            Assert.AreEqual(fi.Lang, actual.feed_lang, "checking feed_lang");
            Assert.AreEqual(fi.StartDate, actual.feed_start_date, "checking feed_start_date");
            Assert.AreEqual(fi.EndDate, actual.feed_end_date, "checking feed_end_date");
            Assert.AreEqual(fi.Version, actual.feed_version, "checking feed_version");
        }
        #endregion Feed_Info.txt tests

        #region Calendar.txt tests
        [TestMethod]
        public void LoadFromZip_OneCalendar()
        {
            TestCalendar expectedCal = new TestCalendar
            {
                ServiceId = "weekday",
                Monday = 1,
                Tuesday = 1,
                Wednesday = 1,
                Thursday = 1,
                Friday = 1,
                Saturday = 0,
                Sunday = 0,
                StartDate = TestFeedInfo.Sample.StartDate,
                EndDate = TestFeedInfo.Sample.EndDate
            };
            _helper.TargetFileName = "onecalendar.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Calendars.Add(expectedCal);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Calendars.Count(), "checking Calendars count");
            var actualCal = target.Calendars.ElementAt(0);
            Assert.AreEqual(expectedCal.ServiceId, actualCal.service_id, "checking Calendar 0 service_id");
            Assert.AreEqual(expectedCal.Monday, actualCal.monday, "checking Calendar 0 monday");
            Assert.AreEqual(expectedCal.Tuesday, actualCal.tuesday, "checking Calendar 0 tuesday");
            Assert.AreEqual(expectedCal.Wednesday, actualCal.wednesday, "checking Calendar 0 wednesday");
            Assert.AreEqual(expectedCal.Thursday, actualCal.thursday, "checking Calendar 0 thursday");
            Assert.AreEqual(expectedCal.Friday, actualCal.friday, "checking Calendar 0 friday");
            Assert.AreEqual(expectedCal.Saturday, actualCal.saturday, "checking Calendar 0 saturday");
            Assert.AreEqual(expectedCal.Sunday, actualCal.sunday, "checking Calendar 0 sunday");
            Assert.AreEqual(expectedCal.StartDate, actualCal.start_date, "checking Calendar 0 start_date");
            Assert.AreEqual(expectedCal.EndDate, actualCal.end_date, "checking Calendar 0 end_date");
        }

        [TestMethod]
        public void LoadFromZip_TwoCalendars()
        {
            TestCalendar expectedWeekday = new TestCalendar
            {
                ServiceId = "weekday",
                Monday = 1,
                Tuesday = 1,
                Wednesday = 1,
                Thursday = 1,
                Friday = 1,
                Saturday = 0,
                Sunday = 0,
                StartDate = TestFeedInfo.Sample.StartDate,
                EndDate = TestFeedInfo.Sample.EndDate
            };
            TestCalendar expectedWeekend = new TestCalendar
            {
                ServiceId = "weekend",
                Monday = 0,
                Tuesday =0,
                Wednesday = 0,
                Thursday = 0,
                Friday = 0,
                Saturday = 1,
                Sunday = 1,
                StartDate = TestFeedInfo.Sample.StartDate,
                EndDate = TestFeedInfo.Sample.EndDate
            };
            _helper.TargetFileName = "twocalendars.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Calendars.Add(expectedWeekday);
            _helper.Calendars.Add(expectedWeekend);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(2, target.Calendars.Count(), "checking Calendars count");
            var actualWeekday = target.Calendars.ElementAt(0);
            Assert.AreEqual(expectedWeekday.ServiceId, actualWeekday.service_id, "checking Calendar 0 service_id");
            Assert.AreEqual(expectedWeekday.Monday, actualWeekday.monday, "checking Calendar 0 monday");
            Assert.AreEqual(expectedWeekday.Tuesday, actualWeekday.tuesday, "checking Calendar 0 tuesday");
            Assert.AreEqual(expectedWeekday.Wednesday, actualWeekday.wednesday, "checking Calendar 0 wednesday");
            Assert.AreEqual(expectedWeekday.Thursday, actualWeekday.thursday, "checking Calendar 0 thursday");
            Assert.AreEqual(expectedWeekday.Friday, actualWeekday.friday, "checking Calendar 0 friday");
            Assert.AreEqual(expectedWeekday.Saturday, actualWeekday.saturday, "checking Calendar 0 saturday");
            Assert.AreEqual(expectedWeekday.Sunday, actualWeekday.sunday, "checking Calendar 0 sunday");
            Assert.AreEqual(expectedWeekday.StartDate, actualWeekday.start_date, "checking Calendar 0 start_date");
            Assert.AreEqual(expectedWeekday.EndDate, actualWeekday.end_date, "checking Calendar 0 end_date");
            var actualWeekend = target.Calendars.ElementAt(1);
            Assert.AreEqual(expectedWeekend.ServiceId, actualWeekend.service_id, "checking Calendar 1 service_id");
            Assert.AreEqual(expectedWeekend.Monday, actualWeekend.monday, "checking Calendar 1 monday");
            Assert.AreEqual(expectedWeekend.Tuesday, actualWeekend.tuesday, "checking Calendar 1 tuesday");
            Assert.AreEqual(expectedWeekend.Wednesday, actualWeekend.wednesday, "checking Calendar 1 wednesday");
            Assert.AreEqual(expectedWeekend.Thursday, actualWeekend.thursday, "checking Calendar 1 thursday");
            Assert.AreEqual(expectedWeekend.Friday, actualWeekend.friday, "checking Calendar 1 friday");
            Assert.AreEqual(expectedWeekend.Saturday, actualWeekend.saturday, "checking Calendar 1 saturday");
            Assert.AreEqual(expectedWeekend.Sunday, actualWeekend.sunday, "checking Calendar 1 sunday");
            Assert.AreEqual(expectedWeekend.StartDate, actualWeekend.start_date, "checking Calendar 1 start_date");
            Assert.AreEqual(expectedWeekend.EndDate, actualWeekend.end_date, "checking Calendar 1 end_date");
        }
        #endregion Calendar.txt tests

        #region Calendar_Dates.txt tests
        [TestMethod]
        public void LoadFromZip_OneCalendarDate()
        {
            TestCalendarDate expected = new TestCalendarDate
            {
                ServiceId = "weekday",
                Date = TestFeedInfo.Sample.StartDate.AddDays(30),
                ExceptionType = 1
            };
            _helper.TargetFileName = "onecalendardate.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.CalendarDates.Add(expected);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Calendar_Dates.Count(), "checking Calendar_Dates count");
            var actual = target.Calendar_Dates.ElementAt(0);
            Assert.AreEqual(expected.ServiceId, actual.service_id, "checking service_id");
            Assert.AreEqual(expected.Date, actual.exception_date, "checking exception_date");
            Assert.AreEqual(expected.ExceptionType, actual.exception_type, "checking exception_type");

        }
        #endregion Calendar_Dates.txt tests

        #region Routes.txt tests
        [TestMethod]
        public void LoadFromZip_OneCrRoute()
        {
            TestRoute expectedRoute = new TestRoute
            {
                RouteId = "CR-Foo",
                AgencyId = "1",
                RouteShortName = "Foo",
                RouteLongName = "Foo from Bar to Bletch",
                RouteDesc = "Something Fooish",
                RouteType = "2",
                RouteUrl = "",
                RouteColor = "8B118F",
                RouteTextColor = "FFFFFF",
                RouteSortOrder = 1
            };

            _helper.TargetFileName = "oneroute.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Routes.Add(expectedRoute);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Routes.Count, "checking Routes count");
            var actualRoute = target.Routes.ElementAt(0);
            Assert.AreEqual(expectedRoute.RouteId, actualRoute.route_id, "checking route_id");
            Assert.AreEqual(expectedRoute.AgencyId, actualRoute.agency_id, "checking agency_id");
            Assert.AreEqual(expectedRoute.RouteShortName, actualRoute.route_short_name, "checking route_short_name");
            Assert.AreEqual(expectedRoute.RouteLongName, actualRoute.route_long_name, "checking route_long_name");
            Assert.AreEqual(expectedRoute.RouteDesc, actualRoute.route_desc, "checking route_desc");
            Assert.AreEqual(expectedRoute.RouteUrl, actualRoute.route_url, "checking route_url");
            Assert.AreEqual(expectedRoute.RouteColor, actualRoute.route_color, "checking route_color");
            Assert.AreEqual(expectedRoute.RouteTextColor, actualRoute.route_text_color, "checking route_text_color");

        }

        [TestMethod]
        public void LoadFromZip_OneBusRoute()
        {
            TestRoute busRoute = new TestRoute
            {
                RouteId = "1",
                AgencyId = "1",
                RouteShortName = "1",
                RouteLongName = "",
                RouteDesc = "Key Bus Route (Frequent Service)",
                RouteType = "3",
                RouteUrl = "",
                RouteColor = "FFFF7C",
                RouteTextColor = "000000",
                RouteSortOrder = 100
            };
            _helper.TargetFileName = "onebusroute.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Routes.Add(busRoute);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            // we're not currently loading anything but commuter rail
            Assert.AreEqual(0, target.Routes.Count, "checking Routes count");
        }
        #endregion Routes.txt tests

        #region Trips.txt tests
        [TestMethod]
        public void LoadFromZip_OneCRTrip()
        {
            TestTrip expectedTrip = new TestTrip
            {
                RouteId = TestRoute.SampleCommuterRail.RouteId,
                ServiceId = "weekday",
                TripId = "trip one",
                TripHeadsign = "Train to Fooville",
                TripShortName = "123",
                DirectionId = 0,
                BlockId = String.Empty,
                ShapeId = "shape one",
                WheelchairAccessible = 1,
                BikesAllowed = 0
            };

            _helper.TargetFileName = "onetraintrip.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Routes.Add(TestRoute.SampleCommuterRail);
            _helper.Trips.Add(expectedTrip);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Trips.Count, "checking Trips count");
            var actualTrip = target.Trips.ElementAt(0);
            Assert.AreEqual(expectedTrip.RouteId, actualTrip.route_id, "checking route_id");
            Assert.AreEqual(expectedTrip.ServiceId, actualTrip.service_id, "checking service_id");
            Assert.AreEqual(expectedTrip.TripId, actualTrip.trip_id, "checking trip_id");
            Assert.AreEqual(expectedTrip.TripHeadsign, actualTrip.trip_headsign, "checking trip_headsign");
            Assert.AreEqual(expectedTrip.TripShortName, actualTrip.trip_shortname, "checking trip_shortname");
            Assert.AreEqual(expectedTrip.DirectionId, actualTrip.direction_id, "checking direction_id");
            Assert.AreEqual(expectedTrip.BlockId, actualTrip.block_id, "checking block_id");
            Assert.AreEqual(expectedTrip.ShapeId, actualTrip.shape_id, "checking shape_id");
            Assert.AreEqual(expectedTrip.WheelchairAccessible, actualTrip.wheelchair_accessible, "checking wheelchair_accessible");
        }

        [TestMethod]
        public void LoadFromZip_OneTrainOneBusTrip()
        {
            TestTrip expectedRailTrip = new TestTrip
            {
                RouteId = TestRoute.SampleCommuterRail.RouteId,
                ServiceId = "weekday",
                TripId = "trip one",
                TripHeadsign = "Train to Fooville",
                TripShortName = "123",
                DirectionId = 0,
                BlockId = String.Empty,
                ShapeId = "shape one",
                WheelchairAccessible = 1,
                BikesAllowed = 0
            };
            TestTrip busTrip = new TestTrip
            {
                RouteId = TestRoute.SampleBus.RouteId,
                ServiceId = "bus_weekday",
                TripId = "BUS32016-hbc36011-Weekday-02",
                TripHeadsign = "31211394",
                TripShortName = "Dudley",
                DirectionId = 1,
                BlockId = "C01-10",
                ShapeId = "010058",
                WheelchairAccessible = 1,
                BikesAllowed = 0
            };

            _helper.TargetFileName = "onetrainonebustrip.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Routes.Add(TestRoute.SampleCommuterRail);
            _helper.Routes.Add(TestRoute.SampleBus);
            _helper.Trips.Add(expectedRailTrip);
            _helper.Trips.Add(busTrip);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Trips.Count, "checking Trips count");
            var actualRailTrip = target.Trips.ElementAt(0);
            Assert.AreEqual(expectedRailTrip.RouteId, actualRailTrip.route_id, "checking route_id");
            Assert.AreEqual(expectedRailTrip.ServiceId, actualRailTrip.service_id, "checking service_id");
            Assert.AreEqual(expectedRailTrip.TripId, actualRailTrip.trip_id, "checking trip_id");
            Assert.AreEqual(expectedRailTrip.TripHeadsign, actualRailTrip.trip_headsign, "checking trip_headsign");
            Assert.AreEqual(expectedRailTrip.TripShortName, actualRailTrip.trip_shortname, "checking trip_shortname");
            Assert.AreEqual(expectedRailTrip.DirectionId, actualRailTrip.direction_id, "checking direction_id");
            Assert.AreEqual(expectedRailTrip.BlockId, actualRailTrip.block_id, "checking block_id");
            Assert.AreEqual(expectedRailTrip.ShapeId, actualRailTrip.shape_id, "checking shape_id");
            Assert.AreEqual(expectedRailTrip.WheelchairAccessible, actualRailTrip.wheelchair_accessible, "checking wheelchair_accessible");

        }
        #endregion Trips.txt tests

        #region Stop_Times.txt tests
        [TestMethod]
        public void LoadFromZip_OneRailStopTime()
        {
            TestStopTime expected = new TestStopTime
            {
                TripId = TestTrip.SampleRailTrip.TripId,
                ArrivalTime = "08:00:00",
                DepartureTime = "08:01:00",
                StopId = "foo",
                StopSequence = 1,
                StopHeadsign = "not really used but lets test it",
                PickupType = 0,
                DropoffType = 1
            };
            _helper.TargetFileName = "onetrainstoptime.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Routes.Add(TestRoute.SampleCommuterRail);
            _helper.Trips.Add(TestTrip.SampleRailTrip);
            _helper.StopTimes.Add(expected);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Stop_Times.Count, "checking Stop_Times count");
            var actual = target.Stop_Times.ElementAt(0);
            Assert.AreEqual(expected.TripId, actual.trip_id, "checking trip_id");
            Assert.AreEqual(expected.ArrivalTime, actual.arrival_time_txt, "checking arrival_time_txt");
            Assert.AreEqual(expected.DepartureTime, actual.departure_time_txt, "checking departure_time_txt");
            Assert.AreEqual(expected.StopId, actual.stop_id, "checking stop_id");
            Assert.AreEqual(expected.StopSequence, actual.stop_sequence, "checking stop_sequence");
            Assert.AreEqual(expected.StopHeadsign, actual.stop_headsign, "checking stop_headsign");
            Assert.AreEqual(expected.PickupType, actual.pickup_type, "checking pickup_type");
            Assert.AreEqual(expected.DropoffType, actual.drop_off_type, "checking drop_off_type");
        }

        [TestMethod]
        public void LoadFromZip_OneRailOneBusStopTime()
        {
            TestStopTime railStopTime = new TestStopTime
            {
                TripId = TestTrip.SampleRailTrip.TripId,
                ArrivalTime = "08:00:00",
                DepartureTime = "08:01:00",
                StopId = "foo",
                StopSequence = 1,
                StopHeadsign = "not really used but lets test it",
                PickupType = 0,
                DropoffType = 1
            };
            TestStopTime busStopTime = new TestStopTime
            {
                TripId = TestTrip.SampleBusTrip.TripId,
                ArrivalTime = "09:00:00",
                DepartureTime = "09:01:00",
                StopId = "bar",
                StopSequence = 5,
                StopHeadsign = "not really used but lets test it",
                PickupType = 0,
                DropoffType = 0
            };
            _helper.TargetFileName = "onetrainstoptime.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Routes.Add(TestRoute.SampleCommuterRail);
            _helper.Trips.Add(TestTrip.SampleRailTrip);
            _helper.StopTimes.Add(railStopTime);
            _helper.StopTimes.Add(busStopTime);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Stop_Times.Count, "checking Stop_Times count");
            var actual = target.Stop_Times.ElementAt(0);
            Assert.AreEqual(railStopTime.TripId, actual.trip_id, "checking trip_id");
            Assert.AreEqual(railStopTime.ArrivalTime, actual.arrival_time_txt, "checking arrival_time_txt");
            Assert.AreEqual(railStopTime.DepartureTime, actual.departure_time_txt, "checking departure_time_txt");
            Assert.AreEqual(railStopTime.StopId, actual.stop_id, "checking stop_id");
            Assert.AreEqual(railStopTime.StopSequence, actual.stop_sequence, "checking stop_sequence");
            Assert.AreEqual(railStopTime.StopHeadsign, actual.stop_headsign, "checking stop_headsign");
            Assert.AreEqual(railStopTime.PickupType, actual.pickup_type, "checking pickup_type");
            Assert.AreEqual(railStopTime.DropoffType, actual.drop_off_type, "checking drop_off_type");
        }

        #endregion Stop_Times.txt tests

        #region Stops.txt tests
        [TestMethod]
        public void LoadFromZip_OneRailStop()
        {
            TestStop expectedStop = new TestStop
            {
                StopId = TestStopTime.SampleRailStopTime.StopId,
                StopCode = "",
                StopName = "FooTown",
                StopDesc = "Foo Town",
                StopLat = "42.519236",
                StopLon = "-71.502643",
                ZoneId = "",
                StopUrl = "",
                LocationType = 0,
                ParentStation = "",
                WheelchairBoarding = 1
            };
            _helper.TargetFileName = "onetrainstop.zip";
            _helper.FeedInfos.Add(TestFeedInfo.Sample);
            _helper.Routes.Add(TestRoute.SampleCommuterRail);
            _helper.Trips.Add(TestTrip.SampleRailTrip);
            _helper.StopTimes.Add(TestStopTime.SampleRailStopTime);
            _helper.Stops.Add(expectedStop);
            _helper.MakeZipFile();

            Download target = new Download
            {
                download_date = DateTime.Now,
                download_file_name = _helper.TargetFilePath
            };
            target.LoadFromZip();

            Assert.AreEqual(1, target.Stops.Count, "checking Stops.Count");
            var actualStop = target.Stops.ElementAt(0);
            Assert.AreEqual(expectedStop.StopId, actualStop.stop_id, "checking stop_id");
            Assert.AreEqual(expectedStop.StopCode, actualStop.stop_code, "checking stop_code");
            Assert.AreEqual(expectedStop.StopName, actualStop.stop_name, "checking stop_name");
            Assert.AreEqual(expectedStop.StopDesc, actualStop.stop_desc, "checking stop_desc");
            Assert.AreEqual(expectedStop.StopLat, actualStop.stop_lat_txt, "checking stop_lat_txt");
            Assert.AreEqual(expectedStop.StopLon, actualStop.stop_lon_txt, "checking stop_lon_txt");
            Assert.AreEqual(expectedStop.ZoneId, actualStop.zone_id, "checking zone_id");
            Assert.AreEqual(expectedStop.LocationType, actualStop.location_type, "checking location_type");
            Assert.AreEqual(expectedStop.ParentStation, actualStop.parent_station, "checking parent_station");
            Assert.AreEqual(expectedStop.WheelchairBoarding, actualStop.wheelchair_boarding, "checking wheelchair_boarding");
        }
        #endregion Stops.txt tests

        #endregion Tests
    }
}
