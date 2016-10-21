namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GtfsStatic.Trips")]
    public partial class Trip
    {
        [Key]
        public int trip_row_id { get; set; }

        public int download_id { get; set; }

        [Required]
        [StringLength(255)]
        public string route_id { get; set; }

        [Required]
        [StringLength(255)]
        public string service_id { get; set; }

        [Required]
        [StringLength(255)]
        public string trip_id { get; set; }

        [StringLength(255)]
        public string trip_headsign { get; set; }

        [StringLength(255)]
        public string trip_shortname { get; set; }

        public int? direction_id { get; set; }

        [StringLength(255)]
        public string block_id { get; set; }

        [StringLength(255)]
        public string shape_id { get; set; }

        public int? wheelchair_accessible { get; set; }

        public int? bikes_allowed { get; set; }

        public virtual Download Download { get; set; }
    }
}
