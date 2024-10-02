using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Separation.Models.Filter.Settlement_Setup
{
    public class SettlementSetupForm
    {
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }

        public bool IsGotPunishment { get; set; }

        public bool SeparatedWithSalary { get; set; }

        public bool WithExtraBasic { get; set; }


        public long NoOfExtraBasic { get; set; }


        //public double? ShortfallInNoticePeriod { get; set; }

        //public string NoticePayBasedOn { get; set; }


        public string Remarks { get; set; }

        public bool IsSalaryDisbursedWithSettlementProcess { get; set; }
    }
}
