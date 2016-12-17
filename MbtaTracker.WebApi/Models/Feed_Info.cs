namespace MbtaTracker.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GtfsStatic.Feed_Info")]
    public partial class Feed_Info
    {
        [Key]
        public int feed_info_id { get; set; }

        public int download_id { get; set; }

        [StringLength(250)]
        public string feed_publisher_name { get; set; }

        [StringLength(250)]
        public string feed_publisher_url { get; set; }

        [StringLength(50)]
        public string feed_lang { get; set; }

        [StringLength(8)]
        public string feed_start_date_txt { get; set; }

        [StringLength(8)]
        public string feed_end_date_txt { get; set; }

        public DateTime? feed_start_date { get; set; }

        public DateTime? feed_end_date { get; set; }

        [StringLength(255)]
        public string feed_version { get; set; }

        public virtual Download Download { get; set; }
    }
}
