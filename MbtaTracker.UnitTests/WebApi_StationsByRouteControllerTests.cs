using MbtaTracker.WebApi.Models;
using MbtaTracker.WebApi.Controllers;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.UnitTests
{
    [TestClass]
    public class WebApi_StationsByRouteControllerTests
    {
        #region Test methods
        [TestMethod]
        public void GetByRouteId_TwoStations()
        {
            DateTime timestamp1 = DateTime.UtcNow.AddMinutes(-2);
            DateTime timestamp2 = DateTime.UtcNow.AddMinutes(-1);
            string routeOneId = "RouteOneId";
            string routeTwoId = "RouteTwoId";
            string stopOneName = "Stop One (both routes)";
            string stopOneId = "StopOneId";
            string stopTwoName = "Stop Two (route one only)";
            string stopTwoId = "StopTwoId";
            string stopThreeName = "Stop Three (route two only)";
            string stopThreeId = "StopThreeId";

            var db = new WebApiTestMbtaTrackerDb();
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 1,
                prediction_timestamp = timestamp1,
                route_id = routeOneId,
                stop_name = stopOneName,
                url_safe_stop_id = stopOneId
            });
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 2,
                prediction_timestamp = timestamp1,
                route_id = routeOneId,
                stop_name = stopTwoName,
                url_safe_stop_id = stopTwoId
            });
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 3,
                prediction_timestamp = timestamp2,
                route_id = routeOneId,
                stop_name = stopOneName,
                url_safe_stop_id = stopOneId
            });
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 4,
                prediction_timestamp = timestamp2,
                route_id = routeOneId,
                stop_name = stopTwoName,
                url_safe_stop_id = stopTwoId
            });
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 3,
                prediction_timestamp = timestamp2,
                route_id = routeTwoId,
                stop_name = stopOneName,
                url_safe_stop_id = stopOneId
            });
            db.TripsByStations.Add(new TripsByStation
            {
                trips_by_station_id = 4,
                prediction_timestamp = timestamp2,
                route_id = routeTwoId,
                stop_name = stopThreeName,
                url_safe_stop_id = stopThreeId
            });

            StationsByRouteController target = new StationsByRouteController
            {
                TrackerDb = db
            };

            IEnumerable<StationListItem> results = target.GetByRouteId(routeOneId);

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
