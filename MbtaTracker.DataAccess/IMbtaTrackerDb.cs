using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace MbtaTracker.DataAccess
{
    public interface IMbtaTrackerDb : IDisposable
    {
        DbSet<Calendar> Calendars { get; set; }
        DbSet<Calendar_Dates> Calendar_Dates { get; set; }
        DbSet<Download> Downloads { get; set; }
        DbSet<Feed_Info> Feed_Info { get; set; }
        DbSet<Route> Routes { get; set; }
        DbSet<Stop_Times> Stop_Times { get; set; }
        DbSet<Stop> Stops { get; set; }
        DbSet<Trip> Trips { get; set; }
        DbSet<Prediction> Predictions { get; set; }
        DbSet<PredictionTrip> PredictionTrips { get; set; }
        DbSet<PredictionTripStop> PredictionTripStops { get; set; }
        DbSet<PredictionTripVehicle> PredictionTripVehicles { get; set; }

        int SaveChanges();
    }
}
