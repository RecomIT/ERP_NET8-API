using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Salary
{
    public class PayslipTaxCardEmailing
    {
        public string SelectedEmployees { get; set; } // EmployeeCode
        [Range(1, 12)]
        public short Month { get; set; }
        [Range(2022, 2050)]
        public short Year { get; set; }
        [StringLength(100), Required]
        public string ReportFileName { get; set; } // Payslip /TaxCard / Both
        public bool WithPasswordProtected { get; set; }
        [StringLength(20), Required]
        public string FileFormat { get; set; }// PDF / EXCEL
        public string Email { get; set; }
    }
}
