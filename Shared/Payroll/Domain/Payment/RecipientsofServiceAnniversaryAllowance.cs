using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_RecipientsofServiceAnniversaryAllowance"), Index("EmployeeId", "AllowanceNameId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_RecipientsofServiceAnniversaryAllowance_NonClusteredIndex")]
    public class RecipientsofServiceAnniversaryAllowance : BaseModel2
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        public long? FiscalYearId { get; set; }
        public int PaymentMonth { get; set; }
        public int PaymentYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PayableAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DisbursedAmount { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? IsVisibleInPayslip { get; set; }
        public bool? IsVisibleInSalarySheet { get; set; }
        public long? SalaryProcessId { get; set; }
        public long? SalaryProcessDetailId { get; set; }
        public long? ServiceAnniversaryAllowanceId { get; set; }
    }
}
