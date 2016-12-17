using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MbtaTracker.WebApi.Models;
using System.Data.Entity;
using MbtaTracker.WebApi.Controllers;

namespace MbtaTracker.UnitTests
{
    [TestClass]
    public class WebApi_AllStationsControllerTests
    {
#region Test methods
        [TestMethod]
        public void Get_TwoStations()
        {
            DateTime timestamp1 = DateTime.UtcNow.AddMinutes(-2);
            DateTime timestamp2 = DateTime.UtcNow.AddMinutes(-1);
            string stopOneName = "Stop One";
            string stopOneId = "StopOneId";
            string stopTwoName = "Stop Two";
            string stopTwoId = "StopTwoId";

            var db = new WebApiTestMbtaTrackerDb();
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 1,
                prediction_timestamp = timestamp1,
                stop_name = stopOneName,
                url_safe_stop_id = stopOneId
            });
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 2,
                prediction_timestamp = timestamp1,
                stop_name = stopTwoName,
                url_safe_stop_id = stopTwoId
            });
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 3,
                prediction_timestamp = timestamp2,
                stop_name = stopOneName,
                url_safe_stop_id = stopOneId
            });
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 4,
                prediction_timestamp = timestamp2,
                stop_name = stopTwoName,
                url_safe_stop_id = stopTwoId
            });

            AllStationsController target = new AllStationsController
            {
                TrackerDb = db
            };

            IEnumerable<StationListItem> results = target.Get();

            Assert.AreEqual(2, results.Count(), "checking results count");
            var stopOne = results.Where(s => s.StationName == stopOneName).Single();
            Assert.AreEqual(stopOneName, stopOne.StationName, "checking stop one name");
            Assert.AreEqual(stopOneId, stopOne.UrlSafeStopId, "checking stop one id");
            var stopTwo = results.Where(s => s.StationName == stopTwoName).Single();
            Assert.AreEqual(stopTwoName, stopTwo.StationName, "checking stop two name");
            Assert.AreEqual(stopTwoId, stopTwo.UrlSafeStopId, "checking stop two id");
        }


#endregion Test methods
    }
}
