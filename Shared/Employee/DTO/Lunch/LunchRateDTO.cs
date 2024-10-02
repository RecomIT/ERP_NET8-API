using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Employee.DTO.Lunch
{
    public class LunchRateDTO
    {
        public long LunchRateId { get; set; }
       
        public decimal? Rate { get; set; }
        
        public Nullable<DateTime> ValidFrom { get; set; }
        
        public Nullable<DateTime> ValidTo { get; set; }
    }
}
