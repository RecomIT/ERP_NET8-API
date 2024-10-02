using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Helpers.SalaryProcess
{
    public class SalaryReviewFromEmployeeUploader
    {
        public string ConfigType { get; set; } // Flat - Gross Base - Basic Base
        [Column(TypeName = "decimal(18,2)")]
        public decimal GrossSalary { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasicSalary { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal HouseRent { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Medical { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Conveyance { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal LFA { get; set; } = 0;
        public Nullable<DateTime> SalaryEffectiveDate { get; set; }
        public Nullable<DateTime> SalaryActivationDate { get; set; }
    }
}
