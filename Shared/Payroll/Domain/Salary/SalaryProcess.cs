using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Salary
{
    [Table("Payroll_SalaryProcess"), Index("ProcessType", "BatchNo", "SalaryMonth", "SalaryYear", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_SalaryProcess_NonClusteredIndex")]
    public class SalaryProcess : BaseModel5
    {
        [Key]
        public long SalaryProcessId { get; set; }
        [StringLength(50)]
        public string ProcessType { get; set; }
        [StringLength(50)]
        public string SalaryProcessUniqId { get; set; }
        [StringLength(30)]
        public string BatchNo { get; set; }
        [StringLength(150)]
        public string DocFileNumber { get; set; }
        public long? FiscalYearId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> SalaryDate { get; set; }
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ProcessDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> PaymentDate { get; set; }
        public long? DivisionId { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(10)]
        public string Gender { get; set; }
        public bool IsDisbursed { get; set; }
        public int TotalEmployees { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearAllowance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAllowanceAdjustment { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPFAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPFArrear { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearDeduction { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalProjectionTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalOnceOffTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalMonthlyTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTaxDeductedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalBonus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalGrossPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PayableAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalNetPay { get; set; }

        public int? TotalHoldDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalHoldAmount { get; set; }
        public int? TotalUnholdDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TotalUnholdAmount { get; set; }
        public long? CompanyAccountId { get; set; }

        //Added by Nur Vai 27-November-2023

        [Column(TypeName = "decimal(18,2)")]
        public decimal? RegularNetPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RegularNetPayLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RegularNetPayDifferenceWithLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ContractualNetPay { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ContractualNetPayLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ContractualNetPayDifferenceWithLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RegularAndContractualNetPayLastMonthTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayRegular { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayRegularLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayRegularDifferenceWithLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayContractual { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayContractualLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayContractualDifferenceWithLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayRegularAndContractualTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RegularAndContractualNetPayDifferencWithLastMonthTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ResignedNetPayRegular { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ResignedNetPayContractual { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ResignedNetPayRegularAndContractualTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RegularNetPayHeadCountLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ContractualNetPayHeadCountLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? RegularAndContractualNetPayHeadCountTotalLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayRegularHeadCount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayContractualHeadCount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayRegularAndContractualHeadCountTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ResignedNetPayRegularHeadCount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ResignedNetPayContractualHeadCount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ResignedNetPayRegularAndContractualHeadCountTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashSalary { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashSalaryLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashSalaryDifferenceWithLastMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashSalaryNewJoined { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CashSalaryResigned { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? NewJoinerNetPayRegularAndContractualDifferencWithLastMonthTotal { get; set; }
        public ICollection<SalaryProcessDetail> SalaryProcessDetails { get; set; }
    }
}
