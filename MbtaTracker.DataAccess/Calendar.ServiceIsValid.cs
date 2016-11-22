using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MbtaTracker.DataAccess
{
    public partial class Calendar
    {
        public bool ServiceIsValid(DateTime serviceDate)
        {
            return (start_date <= serviceDate
                && serviceDate <= end_date
                &&  (
                        (monday == 1 && serviceDate.DayOfWeek == DayOfWeek.Monday)
                        || (tuesday == 1 && serviceDate.DayOfWeek == DayOfWeek.Tuesday )
                        || (wednesday == 1 && serviceDate.DayOfWeek == DayOfWeek.Wednesday)
                        || (thursday == 1 && serviceDate.DayOfWeek == DayOfWeek.Thursday)
                        || (friday == 1 && serviceDate.DayOfWeek == DayOfWeek.Friday)
                        || (saturday == 1 && serviceDate.DayOfWeek == DayOfWeek.Saturday)
                        || (sunday == 1 && serviceDate.DayOfWeek == DayOfWeek.Sunday)
                    )
                );       
        }
    }
}
