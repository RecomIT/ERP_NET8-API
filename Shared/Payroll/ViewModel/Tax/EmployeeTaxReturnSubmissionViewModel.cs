using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Tax
{
    public class EmployeeTaxReturnSubmissionViewModel : BaseViewModel3
    {
        public long TaxSubmissionId { get; set; }
        public long EmployeeId { get; set; }
        public long FiscalYearId { get; set; }
        [StringLength(100)]
        public string RegistrationNo { get; set; }
        [StringLength(100)]
        public string TaxZone { get; set; }
        [StringLength(100)]
        public string TaxCircle { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PaidAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxPayable { get; set; }
        [Column(TypeName = "date")]
        public DateTime? SubmissionDate { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string FiscalYearRange { get; set; }
        public string AssesmentYear { get; set; }
    }
}
