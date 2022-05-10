using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TobaccoCompanyWPF.ViewModels.ValidationAttributes
{
    public class DateRangeAttribute : RangeAttribute
    {
        public DateRangeAttribute(int minDate, int maxDate)
          : base(typeof(DateTime),
                DateTime.Now.AddYears(minDate).ToShortDateString(),
                DateTime.Now.AddYears(maxDate).ToShortDateString()) 
        {

        }
    }
}
