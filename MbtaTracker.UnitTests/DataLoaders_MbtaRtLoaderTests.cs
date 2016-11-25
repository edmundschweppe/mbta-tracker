using MbtaTracker.DataLoaders;
using MbtaTracker.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace MbtaTracker.UnitTests
{
    [TestClass]
    public class DataLoaders_MbtaRtLoaderTests
    {
        #region Test support classes
        class TestBulkCopyHelper : IBulkCopyHelper
        {
            #region IBulkCopyHelper implementation
            public void BulkLoadData(string connString, string targetTable, DataTable dataToLoad)
            {
                this.ConnectionString = connString;
                this.TargetTable = targetTable;
                this.DataToLoad = dataToLoad;
                this.BulkLoadDataCallCount++;
            }
            #endregion IBulkCopyHelper implementation
            #region Test support methods/members
            public int BulkLoadDataCallCount { get; private set; }
            public string ConnectionString { get; private set; }
            public string TargetTable { get; private set; }
            public DataTable DataToLoad { get; private set; }
            #endregion Test support methods/members
        }

        class TestEntitiesBuilder
        {
            // helper class to encapsulate the hundreds of lines of EF setup ...
            public Download DL1 { get; private set; }
            public Feed_Info FI1 { get; private set; }
            public Calendar C1 { get; private set; }
            public Route R1 { get; private set; }
            public Trip R1T1 { get; private set; }
            public Trip R1T2 { get; private set; }
            public Stop S1 { get; private set; }
            public Stop S2 { get; private set; }
            public Stop_Times R1T1S1 { get; private set; }
            public DateTime R1T1S1_Time_Utc { get; private set; }
            public DateTime R1T1S1_Time_Local { get; private set; }
            public Stop_Times R1T1S2 { get; private set; }
            public DateTime R1T1S2_Time_Utc { get; private set; }
            public DateTime R1T1S2_Time_Local { get; private set; }
            public Stop_Times R1T2S1 { get; private set; }
            public DateTime R1T2S1_Time_Utc { get; private set; }
            public DateTime R1T2S1_Time_Local { get; private set; }
            public Stop_Times R1T2S2 { get; private set; }
            public DateTime R1T2S2_Time_Utc { get; private set; }
            public DateTime R1T2S2_Time_Local { get; private set; }
            public Prediction P1 { get; private set; }
            public PredictionTrip PT1 { get; private set; }
            public PredictionTripStop PT1S2 { get; private set; }
            public PredictionTripVehicle PT1V1 { get; private set; }

            public TestEntitiesBuilder()
            {
                DL1 = new Download
                {
                    download_id = 1,
                    download_file_name = "dummy.zip",
                    download_date = DateTime.Today.AddMonths(-1)
                };
                FI1 = new Feed_Info
                {
                    download_id = DL1.download_id,
                    feed_start_date = DateTime.Today.AddDays(-1),
                    feed_end_date = DateTime.Today.AddYears(1)
                };
                C1 = new Calendar
                {
                    download_id = DL1.download_id,
                    service_id = "Everyday",
                    sunday = 1,
                    monday = 1,
                    tuesday = 1,
                    wednesday = 1,
                    thursday = 1,
                    friday = 1,
                    saturday = 1,
                    start_date = FI1.feed_start_date,
                    end_date = FI1.feed_end_date
                };
                R1 = new Route
                {
                    download_id = DL1.download_id,
                    route_id = "CR R1",
                    route_long_name = "CR Route One"
                };
                R1T1 = new Trip
                {
                    download_id = DL1.download_id,
                    route_id = R1.route_id,
                    service_id = C1.service_id,
                    trip_id = "R1 T1",
                    trip_shortname = "R1T1",
                    trip_headsign = "Route One Trip One",
                    direction_id = 0,
                };
                R1T2 = new Trip
                {
                    download_id = DL1.download_id,
                    route_id = R1.route_id,
                    service_id = C1.service_id,
                    trip_id = "R1 T2",
                    trip_shortname = "R1T2",
                    trip_headsign = "Route One Trip Two",
                    direction_id = 1,

                };
                S1 = new Stop
                {
                    download_id = DL1.download_id,
                    stop_id = "S1",
                    stop_name = "Stop One"
                };
                S2 = new Stop
                {
                    download_id = DL1.download_id,
                    stop_id = "S2",
                    stop_name = "Stop 2"
                };
                DateTime utcDate = UtcNowWithZeroMilliseconds.AddMinutes(-5);
                string r1t1s1Time_Text = utcDate.ToString("HH:mm:ss");
                R1T1S1_Time_Utc = utcDate;
                R1T1S1_Time_Local = utcDate.ToLocalTime();
                R1T1S1= new Stop_Times
                {
                    download_id = DL1.download_id,
                    trip_id = R1T1.trip_id,
                    stop_id = S1.stop_id,
                    stop_sequence = 1,
                    arrival_time_txt = r1t1s1Time_Text,
                    departure_time_txt = r1t1s1Time_Text
                };
                utcDate = UtcNowWithZeroMilliseconds.AddMinutes(-10);
                string r1t1s2Time_Text = utcDate.ToString("HH:mm:ss");
                R1T1S2_Time_Utc = utcDate;
                R1T1S2_Time_Local = utcDate.ToLocalTime();
                R1T1S2 = new Stop_Times
                {
                    download_id = DL1.download_id,
                    trip_id = R1T1.trip_id,
                    stop_id = S2.stop_id,
                    stop_sequence = 2,
                    arrival_time_txt = r1t1s2Time_Text,
                    departure_time_txt = r1t1s2Time_Text
                };
                utcDate = UtcNowWithZeroMilliseconds.AddHours(1);
                string r1t2s1Time_Text = utcDate.ToString("HH:mm:ss");
                R1T2S1_Time_Utc = utcDate;
                R1T2S1_Time_Local = utcDate.ToLocalTime();
                R1T2S1 = new Stop_Times
                {
                    download_id = DL1.download_id,
                    trip_id = R1T2.trip_id,
                    stop_id = S1.stop_id,
                    stop_sequence = 2,
                    arrival_time_txt = r1t2s1Time_Text,
                    departure_time_txt = r1t2s1Time_Text
                };
                utcDate =  UtcNowWithZeroMilliseconds.AddMinutes(10);
                string r1t2s2Time_Text = utcDate.ToString("HH:mm:ss");
                R1T2S2_Time_Utc = utcDate;
                R1T2S2_Time_Local = utcDate.ToLocalTime();
                R1T2S2 = new Stop_Times
                {
                    download_id = DL1.download_id,
                    trip_id = R1T2.trip_id,
                    stop_id = S2.stop_id,
                    stop_sequence = 1,
                    arrival_time_txt = r1t2s2Time_Text,
                    departure_time_txt = r1t2s2Time_Text
                };

                P1 = new Prediction
                {
                    prediction_id = 1,
                    prediction_json = "",
                    prediction_time = DateTime.Now
                };
                PT1  = new PredictionTrip
                {
                    prediction_id = P1.prediction_id,
                    route_id = R1.route_id,
                    direction_id = R1T1.direction_id.ToString(),
                    trip_id = R1T1.trip_id,
                    trip_headsign = R1T1.trip_headsign
                };
                PT1S2 = new PredictionTripStop
                {
                    stop_id = S2.stop_id,
                    stop_name = S2.stop_name,
                    stop_sequence = 1,
                    sch_arr_dt = R1T1S2_Time_Utc,
                    sch_dep_dt = R1T1S2_Time_Utc,
                    pre_dt = R1T1S2_Time_Utc,
                    pre_away = (int)((R1T1S2_Time_Utc - P1.prediction_time).TotalMinutes)
                };
                PT1V1 = new PredictionTripVehicle
                {
                    vehicle_id = "V One"
                };
            }

            /// <summary>
            /// DateTime.Now, but with .Millisecond set to zero
            /// </summary>
            public DateTime UtcNowWithZeroMilliseconds
            {
                get
                {
                    return new DateTime(
                        DateTime.Now.Year,
                        DateTime.Now.Month,
                        DateTime.Now.Day,
                        DateTime.Now.Hour,
                        DateTime.Now.Minute,
                        DateTime.Now.Second,
                        DateTimeKind.Utc);
                }
            }
        }
        #endregion Test support classes

        #region Member variables
        #endregion Member variables

        #region Tests
        [TestMethod]
        public void ReloadDenormalizedTables_OneScheduledTrip_OnePrediction()
        {
            TestEntitiesBuilder eb = new TestEntitiesBuilder();
            eb.DL1.Feed_Info.Add(eb.FI1);
            eb.DL1.Calendars.Add(eb.C1);
            eb.DL1.Routes.Add(eb.R1);
            eb.DL1.Trips.Add(eb.R1T1);
            eb.DL1.Stops.Add(eb.S1);
            eb.DL1.Stops.Add(eb.S2);
            eb.DL1.Stop_Times.Add(eb.R1T1S1);
            eb.DL1.Stop_Times.Add(eb.R1T1S2);
            eb.P1.PredictionTrips.Add(eb.PT1);
            eb.PT1.PredictionTripStops.Add(eb.PT1S2);
            eb.PT1.PredictionTripVehicles.Add(eb.PT1V1);

            TestBulkCopyHelper bch = new TestBulkCopyHelper();
            TestMbtaTrackerDb db = new TestMbtaTrackerDb();
            db.AddDownloadAndChildren(eb.DL1);
            db.AddPredictionAndChildren(eb.P1);

            string connString = "test connection string";
            string apiKey = "test api key";
            MbtaRtLoader target = new MbtaRtLoader
            {
                ApiKey = apiKey,
                ConnectionString = connString
            };
            target.BulkHelper = bch;
            target.TrackerDb = db;

            target.ReloadDenormalizedTables();

            // expecting only one result row: only one trip, and S1 stop is in past
            Assert.AreEqual(0, db.SaveChangesCallCount, "checking no calls to MbtaTrackerDb.SaveChanges()");
            Assert.AreEqual(1, bch.BulkLoadDataCallCount, "checking calls to BulkCopyHelper.BulkLoad()");
            Assert.AreEqual(connString, bch.ConnectionString, "checking ConnectionString");
            Assert.AreEqual("Display.TripsByStation", bch.TargetTable, "checking TargetTable");
            DataTable dt = bch.DataToLoad;
            Assert.AreEqual(1, dt.Rows.Count, "checking row count");
            Assert.AreEqual(eb.R1.route_id, dt.Rows[0]["route_id"], "checking row 0 route_id");
            Assert.AreEqual(eb.R1.route_long_name, dt.Rows[0]["route_name"], "checking row 0 route_name");
            Assert.AreEqual(eb.R1T1.trip_id, dt.Rows[0]["trip_id"], "checking row 0 trip_id");
            Assert.AreEqual(eb.R1T1.trip_shortname, dt.Rows[0]["trip_shortname"], "checking row 0 trip_shortname");
            Assert.AreEqual(eb.R1T1.trip_headsign, dt.Rows[0]["trip_headsign"], "checking row 0 trip_headsign");
            Assert.AreEqual(eb.R1T1.direction_id, dt.Rows[0]["trip_direction"], "checking row 0 trip_direction");
            Assert.AreEqual(eb.PT1V1.vehicle_id, dt.Rows[0]["vehicle_id"], "checking row 0 vehicle_id");
            Assert.AreEqual(eb.S2.stop_id, dt.Rows[0]["stop_id"], "checking row 0 stop_id");
            Assert.AreEqual(eb.S2.stop_name, dt.Rows[0]["stop_name"], "checking row 0 stop_name");
            Assert.AreEqual(eb.R1T1S2_Time_Utc, dt.Rows[0]["sched_dep_dt"], "checking row 0 sched_dep_dt");
            Assert.AreEqual(eb.R1T1S2_Time_Utc, dt.Rows[0]["pred_dt"], "checking row 0 pred_dt");
            Assert.AreEqual(eb.PT1S2.pre_away, dt.Rows[0]["pred_away"], "checking row 0 pred_away");
        }

        [TestMethod]
        public void ReloadDenormalizedTables_TwoScheduledTrips_OnePrediction()
        {
            TestEntitiesBuilder eb = new TestEntitiesBuilder();
            eb.DL1.Feed_Info.Add(eb.FI1);
            eb.DL1.Calendars.Add(eb.C1);
            eb.DL1.Routes.Add(eb.R1);
            eb.DL1.Trips.Add(eb.R1T1);
            eb.DL1.Trips.Add(eb.R1T2);
            eb.DL1.Stops.Add(eb.S1);
            eb.DL1.Stops.Add(eb.S2);
            eb.DL1.Stop_Times.Add(eb.R1T1S1);
            eb.DL1.Stop_Times.Add(eb.R1T1S2);
            eb.DL1.Stop_Times.Add(eb.R1T2S1);
            eb.DL1.Stop_Times.Add(eb.R1T2S2);
            eb.P1.PredictionTrips.Add(eb.PT1);
            eb.PT1.PredictionTripStops.Add(eb.PT1S2);
            eb.PT1.PredictionTripVehicles.Add(eb.PT1V1);

            TestBulkCopyHelper bch = new TestBulkCopyHelper();
            TestMbtaTrackerDb db = new TestMbtaTrackerDb();
            db.AddDownloadAndChildren(eb.DL1);
            db.AddPredictionAndChildren(eb.P1);

            string connString = "test connection string";
            string apiKey = "test api key";
            MbtaRtLoader target = new MbtaRtLoader
            {
                ApiKey = apiKey,
                ConnectionString = connString
            };
            target.BulkHelper = bch;
            target.TrackerDb = db;

            target.ReloadDenormalizedTables();

            // expecting three rows: one prediction for T1, two scheduled for T2
            Assert.AreEqual(0, db.SaveChangesCallCount, "checking no calls to MbtaTrackerDb.SaveChanges()");
            Assert.AreEqual(1, bch.BulkLoadDataCallCount, "checking calls to BulkCopyHelper.BulkLoad()");
            Assert.AreEqual(connString, bch.ConnectionString, "checking ConnectionString");
            Assert.AreEqual("Display.TripsByStation", bch.TargetTable, "checking TargetTable");
            DataTable dt = bch.DataToLoad;
            Assert.AreEqual(3, dt.Rows.Count, "checking row count");
            Assert.AreEqual(eb.R1.route_id, dt.Rows[0]["route_id"], "checking row 0 route_id");
            Assert.AreEqual(eb.R1.route_long_name, dt.Rows[0]["route_name"], "checking row 0 route_name");
            Assert.AreEqual(eb.R1T1.trip_id, dt.Rows[0]["trip_id"], "checking row 0 trip_id");
            Assert.AreEqual(eb.R1T1.trip_shortname, dt.Rows[0]["trip_shortname"], "checking row 0 trip_shortname");
            Assert.AreEqual(eb.R1T1.trip_headsign, dt.Rows[0]["trip_headsign"], "checking row 0 trip_headsign");
            Assert.AreEqual(eb.R1T1.direction_id, dt.Rows[0]["trip_direction"], "checking row 0 trip_direction");
            Assert.AreEqual(eb.PT1V1.vehicle_id, dt.Rows[0]["vehicle_id"], "checking row 0 vehicle_id");
            Assert.AreEqual(eb.S2.stop_id, dt.Rows[0]["stop_id"], "checking row 0 stop_id");
            Assert.AreEqual(eb.S2.stop_name, dt.Rows[0]["stop_name"], "checking row 0 stop_name");
            Assert.AreEqual(eb.R1T1S2_Time_Utc, dt.Rows[0]["sched_dep_dt"], "checking row 0 sched_dep_dt");
            Assert.AreEqual(eb.R1T1S2_Time_Utc, dt.Rows[0]["pred_dt"], "checking row 0 pred_dt");
            Assert.AreEqual(eb.PT1S2.pre_away, dt.Rows[0]["pred_away"], "checking row 0 pred_away");
            Assert.AreEqual(eb.R1.route_id, dt.Rows[1]["route_id"], "checking row 1 route_id");
            Assert.AreEqual(eb.R1.route_long_name, dt.Rows[1]["route_name"], "checking row 1 route_name");
            Assert.AreEqual(eb.R1T2.trip_id, dt.Rows[1]["trip_id"], "checking row 1 trip_id");
            Assert.AreEqual(eb.R1T2.trip_shortname, dt.Rows[1]["trip_shortname"], "checking row 1 trip_shortname");
            Assert.AreEqual(eb.R1T2.trip_headsign, dt.Rows[1]["trip_headsign"], "checking row 1 trip_headsign");
            Assert.AreEqual(eb.R1T2.direction_id, dt.Rows[1]["trip_direction"], "checking row 1 trip_direction");
            Assert.AreEqual(DBNull.Value, dt.Rows[1]["vehicle_id"], "checking row 1 vehicle_id");
            Assert.AreEqual(eb.S1.stop_id, dt.Rows[1]["stop_id"], "checking row 1 stop_id");
            Assert.AreEqual(eb.S1.stop_name, dt.Rows[1]["stop_name"], "checking row 1 stop_name");
            Assert.AreEqual(eb.R1T2S1_Time_Utc, dt.Rows[1]["sched_dep_dt"], "checking row 1 sched_dep_dt");
            Assert.AreEqual(DBNull.Value, dt.Rows[1]["pred_dt"], "checking row 1 pred_dt");
            Assert.AreEqual(DBNull.Value, dt.Rows[1]["pred_away"], "checking row 1 pred_away");
            Assert.AreEqual(eb.R1T2.trip_id, dt.Rows[2]["trip_id"], "checking row 2 trip_id");
            Assert.AreEqual(eb.R1T2.trip_shortname, dt.Rows[2]["trip_shortname"], "checking row 2 trip_shortname");
            Assert.AreEqual(eb.R1T2.trip_headsign, dt.Rows[2]["trip_headsign"], "checking row 2 trip_headsign");
            Assert.AreEqual(eb.R1T2.direction_id, dt.Rows[2]["trip_direction"], "checking row 2 trip_direction");
            Assert.AreEqual(DBNull.Value, dt.Rows[2]["vehicle_id"], "checking row 2 vehicle_id");
            Assert.AreEqual(eb.S2.stop_id, dt.Rows[2]["stop_id"], "checking row 2 stop_id");
            Assert.AreEqual(eb.S2.stop_name, dt.Rows[2]["stop_name"], "checking row 2 stop_name");
            Assert.AreEqual(eb.R1T2S2_Time_Utc, dt.Rows[2]["sched_dep_dt"], "checking row 2 sched_dep_dt");
            Assert.AreEqual(DBNull.Value, dt.Rows[2]["pred_dt"], "checking row 2 pred_dt");
            Assert.AreEqual(DBNull.Value, dt.Rows[2]["pred_away"], "checking row 2 pred_away");
        }
        #endregion Tests
    }
}
