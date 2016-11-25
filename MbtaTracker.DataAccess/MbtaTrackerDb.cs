namespace MbtaTracker.DataAccess
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class MbtaTrackerDb : DbContext, IMbtaTrackerDb
    {
        public MbtaTrackerDb()
            : base("name=MbtaTrackerDb")
        {
        }

        public MbtaTrackerDb(string connStr)
            : base(connStr)
        {

        }

        public virtual DbSet<Calendar> Calendars { get; set; }
        public virtual DbSet<Calendar_Dates> Calendar_Dates { get; set; }
        public virtual DbSet<Download> Downloads { get; set; }
        public virtual DbSet<Feed_Info> Feed_Info { get; set; }
        public virtual DbSet<Route> Routes { get; set; }
        public virtual DbSet<Stop_Times> Stop_Times { get; set; }
        public virtual DbSet<Stop> Stops { get; set; }
        public virtual DbSet<Trip> Trips { get; set; }
        public virtual DbSet<Prediction> Predictions { get; set; }
        public virtual DbSet<PredictionTrip> PredictionTrips { get; set; }
        public virtual DbSet<PredictionTripStop> PredictionTripStops { get; set; }
        public virtual DbSet<PredictionTripVehicle> PredictionTripVehicles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Calendar>()
                .Property(e => e.service_id)
                .IsUnicode(false);

            modelBuilder.Entity<Calendar_Dates>()
                .Property(e => e.service_id)
                .IsUnicode(false);

            modelBuilder.Entity<Download>()
                .HasMany(e => e.Calendars)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Download>()
                .HasMany(e => e.Calendar_Dates)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Download>()
                .HasMany(e => e.Feed_Info)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Download>()
                .HasMany(e => e.Routes)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Download>()
                .HasMany(e => e.Stop_Times)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Download>()
                .HasMany(e => e.Stops)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Download>()
                .HasMany(e => e.Trips)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Prediction>()
                .HasMany(e => e.PredictionTrips)
                .WithRequired(e => e.Prediction)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PredictionTrip>()
                .HasMany(e => e.PredictionTripStops)
                .WithRequired(e => e.PredictionTrip)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PredictionTrip>()
                .HasMany(e => e.PredictionTripVehicles)
                .WithRequired(e => e.PredictionTrip)
                .WillCascadeOnDelete(false);
        }
    }
}
