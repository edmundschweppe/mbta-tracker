namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MbtaRt.Predictions")]
    public partial class Prediction
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Prediction()
        {
            PredictionTrips = new HashSet<PredictionTrip>();
        }

        [Key]
        public int prediction_id { get; set; }

        public DateTime prediction_time { get; set; }

        public string prediction_json { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PredictionTrip> PredictionTrips { get; set; }
    }
}
