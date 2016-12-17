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

        public static string DirectionFromInt(int? trip_direction)
        {
            if (trip_direction.HasValue)
            {
                switch(trip_direction)
                {
                    case 0:
                        return "Outbound";
                    case 1:
                        return "Inbound";
                    default:
                        break;
                }
            }
            return "(unknown)";
        }
    }
}