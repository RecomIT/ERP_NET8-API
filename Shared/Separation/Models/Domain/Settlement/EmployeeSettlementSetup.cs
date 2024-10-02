using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Separation.Models.Domain.Settlement
{
    [Table("Payroll_EmployeeSettlementSetup")]
    public class EmployeeSettlementSetup : BaseModel1
    {
        [Key]
        public long SettlementSetupId { get; set; }
        public long? ResignationRequestId { get; set; }
        public long? EmployeeId { get; set; }
        public bool? IsGotPunishment { get; set; }
        public bool? SeparatedWithSalary { get; set; }
        public bool? WithExtraBasic { get; set; }
        public int? DaysOfExtraBasic { get; set; }
        public bool? IsSalaryDisbursedWithSettlementProcess { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? ShortfallInNoticePeriod { get; set; }
        public string NoticePayBasedOn { get; set; }
        public string Remarks { get; set; }
        public int? DaysOfDeductedBasic { get; set; }
        public int? NoOfExtraBasic { get; set; }
        public bool? IsFinalSettlementComplete { get; set; }
        public string CancelledBy { get; set; }
        public DateTime? CancelledDate { get; set; }
        public string CancelledRemarks { get; set; }
        public string StateStatus { get; set; }
        public bool? IsApproved { get; set; }
    }
}
