using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.WebApi.Models
{
    public interface IMbtaTrackerDb : IDisposable
    {
        DbSet<TripsByStation> TripsByStations { get; set; }
        DbSet<Download> Downloads { get; set; }
        DbSet<Feed_Info> Feed_Info { get; set; }
        DbSet<Prediction> Predictions { get; set; }
    }
}
