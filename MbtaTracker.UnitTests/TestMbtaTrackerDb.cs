using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbtaTracker.DataAccess;

namespace MbtaTracker.UnitTests
{
    public class TestMbtaTrackerDb : IMbtaTrackerDb
    {
        #region IMbtaTrackerDb implementation
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<Calendar_Dates> Calendar_Dates { get; set; }
        public DbSet<Download> Downloads { get; set; }
        public DbSet<Feed_Info> Feed_Info { get; set; }
        public DbSet<Prediction> Predictions { get; set; }
        public DbSet<PredictionTrip> PredictionTrips { get; set; }
        public DbSet<PredictionTripStop> PredictionTripStops { get; set; }
        public DbSet<PredictionTripVehicle> PredictionTripVehicles { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Stop> Stops { get; set; }
        public DbSet<Stop_Times> Stop_Times { get; set; }
        public DbSet<Trip> Trips { get; set; }
        public int SaveChanges()
        {
            SaveChangesCallCount++;
            return 1;
        }
        #endregion IMbtaTrackerDb implementation
        #region IDisposable implementation
        public void Dispose()
        {
            // do nothing
        }
        #endregion IDisposable implementation
        #region Test support methods/members

        public int SaveChangesCallCount { get; private set; }

        public void AddDownloadAndChildren(Download dl)
        {
            this.Downloads.Add(dl);
            this.Calendars.AddRange(dl.Calendars);
            this.Calendar_Dates.AddRange(dl.Calendar_Dates);
            this.Feed_Info.AddRange(dl.Feed_Info);
            this.Routes.AddRange(dl.Routes);
            this.Stops.AddRange(dl.Stops);
            this.Stop_Times.AddRange(dl.Stop_Times);
            this.Trips.AddRange(dl.Trips);
        }

        public void AddPredictionAndChildren(Prediction p)
        {
            this.Predictions.Add(p);
            foreach(var pt in p.PredictionTrips)
            {
                this.PredictionTrips.Add(pt);
                this.PredictionTripStops.AddRange(pt.PredictionTripStops);
                this.PredictionTripVehicles.AddRange(pt.PredictionTripVehicles);
            }
        }

        #endregion Test support methods/members

        public TestMbtaTrackerDb()
        {
            this.Calendars = new TestDbSet<Calendar>();
            this.Calendar_Dates = new TestDbSet<Calendar_Dates>();
            this.Downloads = new TestDbSet<Download>();
            this.Feed_Info = new TestDbSet<Feed_Info>();
            this.Routes = new TestDbSet<Route>();
            this.Stops = new TestDbSet<Stop>();
            this.Stop_Times = new TestDbSet<Stop_Times>();
            this.Trips = new TestDbSet<Trip>();
            this.Predictions = new TestDbSet<Prediction>();
            this.PredictionTrips = new TestDbSet<PredictionTrip>();
            this.PredictionTripStops = new TestDbSet<PredictionTripStop>();
            this.PredictionTripVehicles = new TestDbSet<PredictionTripVehicle>();
        }
    }
}
