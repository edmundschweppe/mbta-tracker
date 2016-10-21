namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MbtaRt.PredictionTripVehicle")]
    public partial class PredictionTripVehicle
    {
        [Key]
        public int prediction_trip_vehicle_row_id { get; set; }

        public int prediction_trip_row_id { get; set; }

        [Required]
        [StringLength(255)]
        public string vehicle_id { get; set; }

        public double vehicle_lat { get; set; }

        public double vehicle_lon { get; set; }

        public double? vehicle_bearing { get; set; }

        public double? vehicle_speed { get; set; }

        public DateTime vehicle_timestamp { get; set; }

        public virtual PredictionTrip PredictionTrip { get; set; }
    }
}
