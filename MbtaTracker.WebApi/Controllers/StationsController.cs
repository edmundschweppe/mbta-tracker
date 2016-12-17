﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MbtaTracker.WebApi.Models;

namespace MbtaTracker.WebApi.Controllers
{
    public class StationsController : ApiController
    {
        private IMbtaTrackerDb _trackerDb = null;

        public IMbtaTrackerDb TrackerDb
        {
            get
            {
                if (_trackerDb == null)
                {
                    return new MbtaTrackerDb();
                }
                return _trackerDb;
            }
            set
            {
                _trackerDb = value;
            }
        }

        public IEnumerable<StationListItem> Get()
        {
            using (var db = TrackerDb)
            {
                return db.TripsByStations
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
