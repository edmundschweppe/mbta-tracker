namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GtfsStatic.Stops")]
    public partial class Stop
    {
        [Key]
        public int stop_row_id { get; set; }

        public int download_id { get; set; }

        [Required]
        [StringLength(255)]
        public string stop_id { get; set; }

        [StringLength(255)]
        public string stop_code { get; set; }

        [StringLength(255)]
        public string stop_name { get; set; }

        [StringLength(255)]
        public string stop_desc { get; set; }

        [StringLength(255)]
        public string stop_lat_txt { get; set; }

        [StringLength(255)]
        public string stop_lon_txt { get; set; }

        [StringLength(255)]
        public string zone_id { get; set; }

        [StringLength(255)]
        public string stop_url { get; set; }

        public int? location_type { get; set; }

        [StringLength(255)]
        public string parent_station { get; set; }

        [StringLength(255)]
        public string stop_timezone { get; set; }

        public int? wheelchair_boarding { get; set; }

        public virtual Download Download { get; set; }
    }
}
