namespace MbtaTracker.WebApi.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MbtaRt.Predictions")]
    public partial class Prediction
    {
        [Key]
        public int prediction_id { get; set; }

        public DateTime prediction_time { get; set; }

        public string prediction_json { get; set; }
    }
}
