namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GtfsStatic.Calendar_Dates")]
    public partial class Calendar_Dates
    {
        [Key]
        public int calendar_dates_row_id { get; set; }

        public int download_id { get; set; }

        [Required]
        [StringLength(255)]
        public string service_id { get; set; }

        public DateTime? exception_date { get; set; }

        public int? exception_type { get; set; }

        public virtual Download Download { get; set; }
    }
}
