using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Separation.DTO
{
    public class EmployeeSettlementSetupDTO
    {
        public long? ResignationRequestId { get; set; }
        public long SettlementSetupId { get; set; }

        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }

        public bool IsGotPunishment { get; set; }

        public bool SeparatedWithSalary { get; set; }

        public bool WithExtraBasic { get; set; }


        public long NoOfExtraBasic { get; set; }


        public string Remarks { get; set; }
        public string CancelRemarks { get; set; }

        public bool IsSalaryDisbursedWithSettlementProcess { get; set; }

        public string ExecutionFlag { get; set; }
    }
}
