using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.External_Tables
{
    [Table("tblPFModuleConfig"), Keyless]
    public class tblPFModuleConfig
    {
        public long PFModuleConfigId { get; set; }
        public long ApplicationId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public long? BranchId { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        public bool? CalculateByJoiningDate { get; set; }
        public bool? CashFlow { get; set; }
        public bool? Subsidiary { get; set; }
        public bool? OnlyEmployeePartLoan { get; set; }
        public bool? IsIslamic { get; set; }
        public bool? MonthWiseIntrument { get; set; }
        public bool? PendingContribution { get; set; }
        public bool? GenerateAmortization { get; set; }
        public bool? LoanPaidandAmortization { get; set; }
        public bool? ReceivePaymentReport { get; set; }
        public bool? ContributionFromPayroll { get; set; }
        public bool? InstrumentAccruedProcess { get; set; }
        public bool? Forfeiture { get; set; }
        public bool? Monthlyprofit { get; set; }
        public bool? Chequeue { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
