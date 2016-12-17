using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbtaTracker.WebApi.Models;
using System.Data.Entity;

namespace MbtaTracker.UnitTests
{
    public class WebApiTestMbtaTrackerDb : IMbtaTrackerDb
    {
        #region Constructors
        public WebApiTestMbtaTrackerDb()
        {
            Downloads = new TestDbSet<Download>();
            Feed_Info = new TestDbSet<Feed_Info>();
            Predictions = new TestDbSet<Prediction>();
            TripsByStations = new TestDbSet<TripsByStation>();
        }
        #endregion Constructors
        #region IMbtaTrackerDb implementation
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Feed_Info> Feed_Info { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<TripsByStation> TripsByStations { get; set; }
        #endregion IMbtaTrackerDb implementation
        #region IDisposable implementation
        public void Dispose()
        {
            // do nothing
        }
        #endregion IDisposable implementation


        #region Test support code

        #endregion Test support code
    }
}
