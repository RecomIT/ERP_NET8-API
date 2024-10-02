using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Helpers.SalaryProcess
{
    public class EmployeeSalaryReviewInSalaryProcess
    {
        public int SerialNo { get; set; }
        public long SalaryReviewInfoId { get; set; }
        public string SalaryConfigCategory { get; set; }
        public decimal CurrentSalaryAmount { get; set; }
        public decimal PreviousSalaryAmount { get; set; }
        public string BaseType { get; set; }
        public string IncrementReason { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EffectiveTo { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ActivationDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> DeactivationDate { get; set; }
        public bool? IsArrearCalculated { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ArrearCalculatedDate { get; set; }
    }
}
