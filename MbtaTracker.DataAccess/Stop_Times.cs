namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GtfsStatic.Stop_Times")]
    public partial class Stop_Times
    {
        [Key]
        public int stop_time_row_id { get; set; }

        public int download_id { get; set; }

        [Required]
        [StringLength(255)]
        public string trip_id { get; set; }

        [StringLength(255)]
        public string arrival_time_txt { get; set; }

        [StringLength(255)]
        public string departure_time_txt { get; set; }

        [Required]
        [StringLength(255)]
        public string stop_id { get; set; }

        public int? stop_sequence { get; set; }

        [StringLength(255)]
        public string stop_headsign { get; set; }

        public int? pickup_type { get; set; }

        public int? drop_off_type { get; set; }

        [StringLength(255)]
        public string shape_dist_traveled_txt { get; set; }

        [StringLength(1)]
        public string timepoint { get; set; }

        public virtual Download Download { get; set; }
    }
}
