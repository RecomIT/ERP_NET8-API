using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Process.Salary
{
    public class EmployeeSalaryReviewInSalaryProcess
    {
        public int SL { get; set; }
        public long SalaryReviewInfoId { get; set; }
        [StringLength(50)]
        public string SalaryConfigCategory { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal CurrentSalaryAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PreviousSalaryAmount { get; set; }
        [StringLength(50)]
        public string BaseType { get; set; }
        [StringLength(50)]
        public string IncrementReason { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactivationDate { get; set; }
        public bool? IsArrearCalculated { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ArrearCalculatedDate { get; set; }
    }
}
