namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GtfsStatic.Routes")]
    public partial class Route
    {
        [Key]
        public int route_row_id { get; set; }

        public int download_id { get; set; }

        [Required]
        [StringLength(50)]
        public string route_id { get; set; }

        [StringLength(50)]
        public string agency_id { get; set; }

        [StringLength(250)]
        public string route_short_name { get; set; }

        [StringLength(250)]
        public string route_long_name { get; set; }

        [StringLength(250)]
        public string route_desc { get; set; }

        [StringLength(1)]
        public string route_type { get; set; }

        [StringLength(255)]
        public string route_url { get; set; }

        [StringLength(6)]
        public string route_color { get; set; }

        [StringLength(6)]
        public string route_text_color { get; set; }

        public virtual Download Download { get; set; }
    }
}
