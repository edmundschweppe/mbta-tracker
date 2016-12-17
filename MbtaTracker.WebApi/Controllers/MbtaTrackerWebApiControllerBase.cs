using MbtaTracker.WebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace MbtaTracker.WebApi.Controllers
{
    public class MbtaTrackerWebApiControllerBase : ApiController
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
    }
}