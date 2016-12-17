using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MbtaTracker.WebApi.Models
{
    public class StationTrainItem
    {
        public string Train { get; set; }
        public string Direction { get; set; }
        public string Destination { get; set; }
        public string ControlCar { get; set; }
        public DateTime Scheduled { get; set; }
        public DateTime? Predicted { get; set; }
    }
}