using MbtaTracker.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MbtaTracker.WebApi.Controllers
{
    public class StationsByRouteController : MbtaTrackerWebApiControllerBase
    {
        public IEnumerable<StationListItem> GetByRouteId(string routeId)
        {
            using (var db = TrackerDb)
            {
                return db.TripsByStations
                    .Where(t => t.route_id == routeId)
                    .Select(t => new StationListItem
                    {
                        StationName = t.stop_name,
                        UrlSafeStopId = t.url_safe_stop_id
                    })
                    .Distinct()
                    .OrderBy(s => s.StationName)
                    .ToList();
            }
        }
    }
}
