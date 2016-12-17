using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MbtaTracker.WebApi.Models
{
    public class StationItem
    {
        public string ApplicationVersion { get; set; }
        public DateTime ScheduleTimeStamp { get; set; }
        public DateTime PredictionTimeStamp { get; set; }
        public string StationName { get; set; }
        public string UrlSafeStopId { get; set; }
        public IEnumerable<StationTrainItem> Trains { get; set; }
    }
}