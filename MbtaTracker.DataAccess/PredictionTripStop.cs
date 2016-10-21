namespace MbtaTracker.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MbtaRt.PredictionTripStop")]
    public partial class PredictionTripStop
    {
        [Key]
        public int prediction_trip_stop_row_id { get; set; }

        public int prediction_trip_row_id { get; set; }

        [Required]
        [StringLength(255)]
        public string stop_id { get; set; }

        [Required]
        [StringLength(255)]
        public string stop_name { get; set; }

        public int stop_sequence { get; set; }

        public DateTime sch_arr_dt { get; set; }

        public DateTime sch_dep_dt { get; set; }

        public DateTime pre_dt { get; set; }

        public int pre_away { get; set; }

        public virtual PredictionTrip PredictionTrip { get; set; }
    }
}
