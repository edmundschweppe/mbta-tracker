namespace MbtaTracker.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Display.TripsByStation")]
    public partial class TripsByStation
    {
        [Key]
        public int trips_by_station_id { get; set; }

        public DateTime prediction_timestamp { get; set; }

        [Required]
        [StringLength(255)]
        public string route_id { get; set; }

        [Required]
        [StringLength(255)]
        public string route_name { get; set; }

        [Required]
        [StringLength(255)]
        public string trip_id { get; set; }

        [Required]
        [StringLength(255)]
        public string trip_shortname { get; set; }

        [Required]
        [StringLength(255)]
        public string trip_headsign { get; set; }

        public int? trip_direction { get; set; }

        [StringLength(255)]
        public string vehicle_id { get; set; }

        [Required]
        [StringLength(255)]
        public string stop_id { get; set; }

        [Required]
        [StringLength(255)]
        public string url_safe_stop_id { get; set; }

        [Required]
        [StringLength(255)]
        public string stop_name { get; set; }

        public DateTime sched_dep_dt { get; set; }

        public DateTime? pred_dt { get; set; }

        public int? pred_away { get; set; }
    }
}
