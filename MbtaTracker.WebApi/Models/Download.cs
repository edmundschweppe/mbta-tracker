namespace MbtaTracker.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GtfsStatic.Download")]
    public partial class Download
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Download()
        {
            Feed_Info = new HashSet<Feed_Info>();
            Stops = new HashSet<Stop>();
        }

        [Key]
        public int download_id { get; set; }

        [Required]
        [StringLength(100)]
        public string download_file_name { get; set; }

        public DateTime download_date { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Feed_Info> Feed_Info { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Stop> Stops { get; set; }
    }
}
