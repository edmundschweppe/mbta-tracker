using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MbtaTracker.WebApi.Models
{
    public class StationListItem : IEquatable<StationListItem>
    {
        /// <summary>
        /// The human-readable name of the station
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// Unique identifier for station, encoded to support ASP MVC routing
        /// </summary>
        public string UrlSafeStopId { get; set; }


        // IEquatable implementation needed for LINQ Distinct()
        #region IEquatable implementation
        public bool Equals(StationListItem other)
        {
            if (other == null) return false;
            return this.UrlSafeStopId.Equals(other.UrlSafeStopId);
        }
        #endregion IEquatable implementation
        public override bool Equals(object obj)
        {
            StationListItem other = obj as StationListItem;
            return this.Equals(other);
        }
        public override int GetHashCode()
        {
            return this.UrlSafeStopId.GetHashCode();
        }
    }
}