namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GtfsStatic.Calendar")]
    public partial class Calendar
    {
        [Key]
        public int calendar_row_id { get; set; }

        public int download_id { get; set; }

        [Required]
        [StringLength(255)]
        public string service_id { get; set; }

        public int? monday { get; set; }

        public int? tuesday { get; set; }

        public int? wednesday { get; set; }

        public int? thursday { get; set; }

        public int? friday { get; set; }

        public int? saturday { get; set; }

        public int? sunday { get; set; }

        public DateTime? start_date { get; set; }

        public DateTime? end_date { get; set; }

        public virtual Download Download { get; set; }
    }
}
