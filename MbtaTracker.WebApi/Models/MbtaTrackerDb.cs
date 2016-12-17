namespace MbtaTracker.WebApi.Models
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

        public virtual DbSet<TripsByStation> TripsByStations { get; set; }
        public virtual DbSet<Download> Downloads { get; set; }
        public virtual DbSet<Feed_Info> Feed_Info { get; set; }
        public virtual DbSet<Stop> Stops { get; set; }
        public virtual DbSet<Prediction> Predictions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Download>()
                .HasMany(e => e.Feed_Info)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Download>()
                .HasMany(e => e.Stops)
                .WithRequired(e => e.Download)
                .WillCascadeOnDelete(false);
        }
    }
}
