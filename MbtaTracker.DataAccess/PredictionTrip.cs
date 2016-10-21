namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MbtaRt.PredictionTrip")]
    public partial class PredictionTrip
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PredictionTrip()
        {
            PredictionTripStops = new HashSet<PredictionTripStop>();
            PredictionTripVehicles = new HashSet<PredictionTripVehicle>();
        }

        [Key]
        public int prediction_trip_row_id { get; set; }

        public int prediction_id { get; set; }

        [Required]
        [StringLength(255)]
        public string route_id { get; set; }

        [StringLength(1)]
        public string direction_id { get; set; }

        [Required]
        [StringLength(255)]
        public string trip_id { get; set; }

        [Required]
        [StringLength(255)]
        public string trip_name { get; set; }

        [StringLength(255)]
        public string trip_headsign { get; set; }

        public virtual Prediction Prediction { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PredictionTripStop> PredictionTripStops { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PredictionTripVehicle> PredictionTripVehicles { get; set; }
    }
}
