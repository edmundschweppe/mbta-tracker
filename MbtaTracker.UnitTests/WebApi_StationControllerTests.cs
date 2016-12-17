using MbtaTracker.WebApi.Controllers;
using MbtaTracker.WebApi.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.UnitTests
{
    [TestClass]
    public class WebApi_StationControllerTests
    {
        #region Test methods
        [TestMethod]
        public void GetById_NoScheduledTrains()
        {
            DateTime downloadDate = new DateTime(2016, 9, 1);
            DateTime predictionTimeStamp = downloadDate
                .AddMonths(1)
                .AddHours(12)
                .AddMinutes(13)
                .ToUniversalTime();
            string stationName = null;
            string urlSafeStopId = "StationOneId";

            var db = new WebApiTestMbtaTrackerDb();
            Download dl = new Download
            {
                download_id = 1,
                download_date = downloadDate,
                download_file_name = "foo.zip"
            };
            db.Downloads.Add(dl);
            db.Predictions.Add(new Prediction {
                prediction_id = 1,
                prediction_time = predictionTimeStamp
            });

            StationController target = new StationController
            {
                TrackerDb = db
            };
            StationItem result = target.GetById(urlSafeStopId);

            Assert.AreEqual(downloadDate, result.ScheduleTimeStamp, "checking schedule time stamp");
            Assert.AreEqual(predictionTimeStamp, result.PredictionTimeStamp, "checking prediction time stamp");
            Assert.AreEqual(stationName, result.StationName, "checking StationName");
            Assert.AreEqual(urlSafeStopId, result.UrlSafeStopId, "checking UrlSafeStopId");
            Assert.AreEqual(0, result.Trains.Count(), "checking trains count");           
        }

        [TestMethod]
        public void GetById_OneScheduledTrain_NoPredictions()
        {
            DateTime downloadDate = new DateTime(2016, 9, 1);
            DateTime predictionTimeStamp = downloadDate
                .AddMonths(1)
                .AddHours(12)
                .AddMinutes(13)
                .ToUniversalTime();
            string stationName = "Station One";
            string urlSafeStopId = "StationOneId";
            string trainOneTrain = "402";
            string trainOneDirection = "Inbound";
            string trainOneDestination = "Route One Start";
            DateTime trainOneScheduledTime = predictionTimeStamp.AddHours(5);


            var db = new WebApiTestMbtaTrackerDb();
            Download dl = new Download
            {
                download_id = 1,
                download_date = downloadDate,
                download_file_name = "foo.zip"
            };
            db.Downloads.Add(dl);
            db.Predictions.Add(new Prediction
            {
                prediction_id = 1,
                prediction_time = predictionTimeStamp
            });
            db.TripsByStations.Add(new TripsByStation {
                prediction_timestamp = predictionTimeStamp,
                stop_name = stationName,
                url_safe_stop_id = urlSafeStopId,
                trip_shortname = trainOneTrain,
                trip_direction = 1,
                trip_headsign = trainOneDestination,
                sched_dep_dt = trainOneScheduledTime,
                pred_dt = null,
                vehicle_id = null
            });

            StationController target = new StationController
            {
                TrackerDb = db
            };
            StationItem result = target.GetById(urlSafeStopId);

            Assert.AreEqual(downloadDate, result.ScheduleTimeStamp, "checking schedule time stamp");
            Assert.AreEqual(predictionTimeStamp, result.PredictionTimeStamp, "checking prediction time stamp");
            Assert.AreEqual(stationName, result.StationName, "checking StationName");
            Assert.AreEqual(urlSafeStopId, result.UrlSafeStopId, "checking UrlSafeStopId");
            Assert.AreEqual(1, result.Trains.Count(), "checking trains count");
            var actualTrainOne = result.Trains.First();
            Assert.AreEqual(trainOneTrain, actualTrainOne.Train, "checking Train 0");
            Assert.AreEqual(trainOneDirection, actualTrainOne.Direction, "checking train 0 Direction");
            Assert.AreEqual(trainOneDestination, actualTrainOne.Destination, "checking train 0 Destination");
            Assert.IsNull(actualTrainOne.ControlCar, "checking train 0 ControlCar");
            Assert.AreEqual(trainOneScheduledTime, actualTrainOne.Scheduled, "checking train 0 Scheduled");
            Assert.IsNull(actualTrainOne.Predicted, "checking train 0 predicted");
        }

        [TestMethod]
        public void GetById_OneScheduledTrain_WithPredictions()
        {
            DateTime downloadDate = new DateTime(2016, 9, 1);
            DateTime predictionTimeStamp = downloadDate
                .AddMonths(1)
                .AddHours(12)
                .AddMinutes(13)
                .ToUniversalTime();
            string stationName = "Station One";
            string urlSafeStopId = "StationOneId";
            string trainOneTrain = "401";
            string trainOneDirection = "Outbound";
            string trainOneDestination = "Route One End";
            string trainOneControlCar = "1234";
            DateTime trainOneScheduledTime = predictionTimeStamp.AddHours(5);
            DateTime trainOnePredictedTime = trainOneScheduledTime.AddSeconds(1);

            var db = new WebApiTestMbtaTrackerDb();
            Download dl = new Download
            {
                download_id = 1,
                download_date = downloadDate,
                download_file_name = "foo.zip"
            };
            db.Downloads.Add(dl);
            db.Predictions.Add(new Prediction
            {
                prediction_id = 1,
                prediction_time = predictionTimeStamp
            });
            db.TripsByStations.Add(new TripsByStation
            {
                prediction_timestamp = predictionTimeStamp,
                stop_name = stationName,
                url_safe_stop_id = urlSafeStopId,
                trip_shortname = trainOneTrain,
                trip_direction = 0,
                trip_headsign = trainOneDestination,
                sched_dep_dt = trainOneScheduledTime,
                pred_dt = trainOnePredictedTime,
                vehicle_id = trainOneControlCar
            });

            StationController target = new StationController
            {
                TrackerDb = db
            };
            StationItem result = target.GetById(urlSafeStopId);

            Assert.AreEqual(downloadDate, result.ScheduleTimeStamp, "checking schedule time stamp");
            Assert.AreEqual(predictionTimeStamp, result.PredictionTimeStamp, "checking prediction time stamp");
            Assert.AreEqual(stationName, result.StationName, "checking StationName");
            Assert.AreEqual(urlSafeStopId, result.UrlSafeStopId, "checking UrlSafeStopId");
            Assert.AreEqual(1, result.Trains.Count(), "checking trains count");
            var actualTrainOne = result.Trains.First();
            Assert.AreEqual(trainOneTrain, actualTrainOne.Train, "checking Train 0");
            Assert.AreEqual(trainOneDirection, actualTrainOne.Direction, "checking train 0 Direction");
            Assert.AreEqual(trainOneDestination, actualTrainOne.Destination, "checking train 0 Destination");
            Assert.AreEqual(trainOneControlCar ,actualTrainOne.ControlCar, "checking train 0 ControlCar");
            Assert.AreEqual(trainOneScheduledTime, actualTrainOne.Scheduled, "checking train 0 Scheduled");
            Assert.AreEqual(trainOnePredictedTime, actualTrainOne.Predicted, "checking train 0 predicted");
        }

        [TestMethod]
        public void GetById_MultipleTrains()
        {
            DateTime downloadDate = new DateTime(2016, 9, 1);
            DateTime predictionTimeStamp = downloadDate
                .AddMonths(1)
                .AddHours(12)
                .AddMinutes(13)
                .ToUniversalTime();
            string stationOneName = "Station One";
            string urlSafeStopIdOne = "StationOneId";
            string stationTwoName = "Station Two";
            string urlSafeStopIdTwo = "StationTwoId";

            var db = new WebApiTestMbtaTrackerDb();
            db.Downloads.Add(new Download
            {
                download_id = 1,
                download_date = downloadDate.AddMonths(-3),
                download_file_name = "foo.zip"
            });
            db.Downloads.Add(new Download
            {
                download_id = 2,
                download_date = downloadDate,
                download_file_name = "foo.zip"
            });
            db.Predictions.Add(new Prediction
            {
                prediction_id = 1,
                prediction_time = predictionTimeStamp.AddMinutes(-2)
            });
            db.Predictions.Add(new Prediction
            {
                prediction_id = 2,
                prediction_time = predictionTimeStamp.AddMinutes(-1)
            });
            db.Predictions.Add(new Prediction
            {
                prediction_id = 3,
                prediction_time = predictionTimeStamp
            });
            TripsByStation s1t1 = new TripsByStation
            {
                prediction_timestamp = predictionTimeStamp,
                stop_name = stationOneName,
                url_safe_stop_id = urlSafeStopIdOne,
                trip_shortname = "401",
                trip_direction = 0,
                trip_headsign = "Route One End",
                sched_dep_dt = predictionTimeStamp.AddMinutes(20),
                pred_dt = predictionTimeStamp.AddMinutes(20),
                vehicle_id = "1001"
            };
            TripsByStation s1t2 = new TripsByStation
            {
                prediction_timestamp = predictionTimeStamp,
                stop_name = stationOneName,
                url_safe_stop_id = urlSafeStopIdOne,
                trip_shortname = "403",
                trip_direction = 0,
                trip_headsign = "Route One End",
                sched_dep_dt = predictionTimeStamp.AddMinutes(50),
                pred_dt = predictionTimeStamp.AddMinutes(51),
                vehicle_id = "2001"
            };
            TripsByStation s2t1 = new TripsByStation
            {
                prediction_timestamp = predictionTimeStamp,
                stop_name = stationTwoName,
                url_safe_stop_id = urlSafeStopIdTwo,
                trip_shortname = "401",
                trip_direction = 0,
                trip_headsign = "Route One End",
                sched_dep_dt = predictionTimeStamp.AddMinutes(25),
                pred_dt = predictionTimeStamp.AddMinutes(25),
                vehicle_id = "1001"
            };
            TripsByStation s2t2 = new TripsByStation
            {
                prediction_timestamp = predictionTimeStamp,
                stop_name = stationTwoName,
                url_safe_stop_id = urlSafeStopIdTwo,
                trip_shortname = "403",
                trip_direction = 0,
                trip_headsign = "Route One End",
                sched_dep_dt = predictionTimeStamp.AddMinutes(55),
                pred_dt = predictionTimeStamp.AddMinutes(55),
                vehicle_id = "2001"
            };
            db.TripsByStations.Add(s1t2);
            db.TripsByStations.Add(s2t2);
            db.TripsByStations.Add(s1t1);
            db.TripsByStations.Add(s2t1);

            StationController target = new StationController
            {
                TrackerDb = db
            };
            StationItem result = target.GetById(urlSafeStopIdOne);

            Assert.AreEqual(downloadDate, result.ScheduleTimeStamp, "checking schedule time stamp");
            Assert.AreEqual(predictionTimeStamp, result.PredictionTimeStamp, "checking prediction time stamp");
            Assert.AreEqual(stationOneName, result.StationName, "checking StationName");
            Assert.AreEqual(urlSafeStopIdOne, result.UrlSafeStopId, "checking UrlSafeStopId");
            Assert.AreEqual(2, result.Trains.Count(), "checking trains count");
            var trainArray = result.Trains.ToArray();
            Assert.AreEqual(s1t1.trip_shortname, trainArray[0].Train, "checking Train 0");
            Assert.AreEqual("Outbound", trainArray[0].Direction, "checking train 0 Direction");
            Assert.AreEqual(s1t1.trip_headsign, trainArray[0].Destination, "checking train 0 Destination");
            Assert.AreEqual(s1t1.vehicle_id, trainArray[0].ControlCar, "checking train 0 ControlCar");
            Assert.AreEqual(s1t1.sched_dep_dt, trainArray[0].Scheduled, "checking train 0 Scheduled");
            Assert.AreEqual(s1t1.pred_dt, trainArray[0].Predicted, "checking train 0 predicted");
            Assert.AreEqual(s1t2.trip_shortname, trainArray[1].Train, "checking Train 1");
            Assert.AreEqual("Outbound", trainArray[1].Direction, "checking train 1 Direction");
            Assert.AreEqual(s1t2.trip_headsign, trainArray[1].Destination, "checking train 1 Destination");
            Assert.AreEqual(s1t2.vehicle_id, trainArray[1].ControlCar, "checking train 1 ControlCar");
            Assert.AreEqual(s1t2.sched_dep_dt, trainArray[1].Scheduled, "checking train 1 Scheduled");
            Assert.AreEqual(s1t2.pred_dt, trainArray[1].Predicted, "checking train 1 predicted");
        }


        #endregion Test methods
    }
}
