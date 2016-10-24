using MbtaTracker.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.UnitTests
{
    [TestClass]
    public class DataAccess_PredictionTests
    {
        #region Support classes
        #endregion Support classes

        #region Member variables

        private string _oneFitchburgOutbound = @"
{
  ""mode"": [
    {
      ""route_type"": ""2"",
      ""mode_name"": ""Commuter Rail"",
      ""route"": [
        {
          ""route_id"": ""CR-Fitchburg"",
          ""route_name"": ""Fitchburg Line"",
          ""direction"": [
            {
              ""direction_id"": ""0"",
              ""direction_name"": ""Outbound"",
              ""trip"": [
                {
                  ""trip_id"": ""30999151-CR_MAY2016-hxf16011-Weekday-01"",
                  ""trip_name"": ""409 (11:30 am from North Station)"",
                  ""trip_headsign"": ""Fitchburg"",
                  ""vehicle"": {
                    ""vehicle_id"": ""1636"",
                    ""vehicle_lat"": ""42.3734703063965"",
                    ""vehicle_lon"": ""-71.2393493652344"",
                    ""vehicle_bearing"": ""246"",
                    ""vehicle_speed"": ""11"",
                    ""vehicle_timestamp"": ""1477324350""
                  },
                  ""stop"": [
                    {
                      ""stop_sequence"": ""15"",
                      ""stop_id"": ""Fitchburg"",
                      ""stop_name"": ""Fitchburg"",
                      ""sch_arr_dt"": ""1477328100"",
                      ""sch_dep_dt"": ""1477328100"",
                      ""pre_dt"": ""1477327853"",
                      ""pre_away"": ""3365""
                    }
                  ]
                }
              ]
            }
          ]
        }
      ]
    }
  ],
  ""alert_headers"": []
}
";
        private string _oneOutboundFitchburg_NoVehicle = @"
{
  ""mode"": [
    {
      ""route_type"": ""2"",
      ""mode_name"": ""Commuter Rail"",
      ""route"": [
        {
          ""route_id"": ""CR-Fitchburg"",
          ""route_name"": ""Fitchburg Line"",
          ""direction"": [
            {
              ""direction_id"": ""0"",
              ""direction_name"": ""Outbound"",
              ""trip"": [
                {
                  ""trip_id"": ""30999151-CR_MAY2016-hxf16011-Weekday-01"",
                  ""trip_name"": ""409 (11:30 am from North Station)"",
                  ""trip_headsign"": ""Fitchburg"",
                  ""stop"": [
                    {
                      ""stop_sequence"": ""15"",
                      ""stop_id"": ""Fitchburg"",
                      ""stop_name"": ""Fitchburg"",
                      ""sch_arr_dt"": ""1477328100"",
                      ""sch_dep_dt"": ""1477328100"",
                      ""pre_dt"": ""1477327853"",
                      ""pre_away"": ""3365""
                    }
                  ]
                }
              ]
            }
          ]
        }
      ]
    }
  ],
  ""alert_headers"": []
}
";
        private Prediction _target;

        #endregion Member variables

        #region Setup and teardown methods

        [TestInitialize]
        public void Setup()
        {
            _target = new Prediction
            {
                prediction_time = DateTime.UtcNow
            };
        }
        #endregion Setup and teardown methods

        #region Tests
        [TestMethod]
        public void PredictionTrip_EmptyJson()
        {
            string json = String.Empty;

            _target.LoadFromJson(json);

            Assert.AreEqual(0, _target.PredictionTrips.Count, "checking PredictionTrips.Count");
        }

        [TestMethod]
        public void PredictionTrip_AlertHeaderOnly()
        {
            string json = @"{""alert_headers"":[]}";

            _target.LoadFromJson(json);

            Assert.AreEqual(0, _target.PredictionTrips.Count, "checking PredictionTrips.Count");
        }

        [TestMethod]
        public void PredictionTrip_OneOutbound_NoVehicle()
        {
            _target.LoadFromJson(_oneOutboundFitchburg_NoVehicle);

            Assert.AreEqual(1, _target.PredictionTrips.Count, "checking PredictionTrips.Count");
            var actualTrip = _target.PredictionTrips.ElementAt(0);
            Assert.AreEqual("CR-Fitchburg", actualTrip.route_id, "checking route_id");
            Assert.AreEqual("0", actualTrip.direction_id, "checking direction_id");
            Assert.AreEqual("30999151-CR_MAY2016-hxf16011-Weekday-01", actualTrip.trip_id, "checking trip_id");
            Assert.AreEqual("409 (11:30 am from North Station)", actualTrip.trip_name, "checking trip_name");
            Assert.AreEqual("Fitchburg", actualTrip.trip_headsign, "checking trip_headsign");

            Assert.AreEqual(0, actualTrip.PredictionTripVehicles.Count, "checking PredictionTripVehicles.Count");

            Assert.AreEqual(1, actualTrip.PredictionTripStops.Count, "checking PredictionTripStops.Count");
            var actualStop = actualTrip.PredictionTripStops.ElementAt(0);
            Assert.AreEqual("Fitchburg", actualStop.stop_id, "checking stop_id");
            Assert.AreEqual("Fitchburg", actualStop.stop_name, "checking stop_name");
            Assert.AreEqual(15, actualStop.stop_sequence, "checking stop_sequence");
            DateTime expectedSchedDt = DateTimeOffset.FromUnixTimeSeconds(1477328100).UtcDateTime;
            Assert.AreEqual(expectedSchedDt, actualStop.sch_arr_dt, "checking sch_arr_dt");
            Assert.AreEqual(expectedSchedDt, actualStop.sch_dep_dt, "checking sch_dep_dt");
            DateTime expectedPredDt = DateTimeOffset.FromUnixTimeSeconds(1477327853).UtcDateTime;
            Assert.AreEqual(expectedPredDt, actualStop.pre_dt, "checking pre_dt");
            Assert.AreEqual(3365, actualStop.pre_away, "checking pre_away");
        }

        [TestMethod]
        public void PredictionTrip_OneOutbound()
        {
            _target.LoadFromJson(_oneFitchburgOutbound);

            Assert.AreEqual(1, _target.PredictionTrips.Count, "checking PredictionTrips.Count");
            var actualTrip = _target.PredictionTrips.ElementAt(0);
            Assert.AreEqual("CR-Fitchburg", actualTrip.route_id, "checking route_id");
            Assert.AreEqual("0", actualTrip.direction_id, "checking direction_id");
            Assert.AreEqual("30999151-CR_MAY2016-hxf16011-Weekday-01", actualTrip.trip_id, "checking trip_id");
            Assert.AreEqual("409 (11:30 am from North Station)", actualTrip.trip_name, "checking trip_name");
            Assert.AreEqual("Fitchburg", actualTrip.trip_headsign, "checking trip_headsign");

            Assert.AreEqual(1, actualTrip.PredictionTripVehicles.Count, "checking PredictionTripVehicles.Count");
            var actualVehicle = actualTrip.PredictionTripVehicles.ElementAt(0);
            Assert.AreEqual("1636", actualVehicle.vehicle_id, "checking vehicle_id");
            Assert.AreEqual(42.3734703063965, actualVehicle.vehicle_lat, "checking vehicle_lat");
            Assert.AreEqual(-71.2393493652344, actualVehicle.vehicle_lon, "checking vehicle_lon");
            Assert.AreEqual(246, actualVehicle.vehicle_bearing, "checking vehicle_bearing");
            Assert.AreEqual(11, actualVehicle.vehicle_speed, "checking vehicle_speed");
            DateTime expectedTimeStamp = DateTimeOffset.FromUnixTimeSeconds(1477324350).UtcDateTime;
            Assert.AreEqual(expectedTimeStamp, actualVehicle.vehicle_timestamp, "checking vehicle_timestamp");

            Assert.AreEqual(1, actualTrip.PredictionTripStops.Count, "checking PredictionTripStops.Count");
            var actualStop = actualTrip.PredictionTripStops.ElementAt(0);
            Assert.AreEqual("Fitchburg", actualStop.stop_id, "checking stop_id");
            Assert.AreEqual("Fitchburg", actualStop.stop_name, "checking stop_name");
            Assert.AreEqual(15, actualStop.stop_sequence, "checking stop_sequence");
            DateTime expectedSchedDt = DateTimeOffset.FromUnixTimeSeconds(1477328100).UtcDateTime;
            Assert.AreEqual(expectedSchedDt, actualStop.sch_arr_dt, "checking sch_arr_dt");
            Assert.AreEqual(expectedSchedDt, actualStop.sch_dep_dt, "checking sch_dep_dt");
            DateTime expectedPredDt = DateTimeOffset.FromUnixTimeSeconds(1477327853).UtcDateTime;
            Assert.AreEqual(expectedPredDt, actualStop.pre_dt, "checking pre_dt");
            Assert.AreEqual(3365, actualStop.pre_away, "checking pre_away");
        }

        #endregion Tests
    }
}
