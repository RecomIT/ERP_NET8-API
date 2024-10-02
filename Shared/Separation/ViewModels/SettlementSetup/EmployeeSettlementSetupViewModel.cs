using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Separation.ViewModels.SettlementSetup
{
    public class EmployeeSettlementSetupViewModel
    {
        public long SettlementSetupId { get; set; }
        public long? ResignationRequestId { get; set; }
        public long? EmployeeId { get; set; }
        public bool? IsGotPunishment { get; set; }
        public bool? SeparatedWithSalary { get; set; }
        public bool? WithExtraBasic { get; set; }
        public int? DaysOfExtraBasic { get; set; }
        public bool? IsSalaryDisbursedWithSettlementProcess { get; set; }
        public decimal? ShortfallInNoticePeriod { get; set; }
        public string NoticePayBasedOn { get; set; }
        public string Remarks { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? CompanyId { get; set; }
        public long? OrganizationId { get; set; }
        public int? DaysOfDeductedBasic { get; set; }
        public int? NoOfExtraBasic { get; set; }
        public bool? IsFinalSettlementComplete { get; set; }

        public string Name { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }

        public string PhotoPath { get; set; }
        public string Photo { get; set; }
    }
}
