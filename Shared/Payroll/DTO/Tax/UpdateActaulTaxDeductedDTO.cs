using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Tax
{
    public class UpdateActaulTaxDeductedDTO
    {
        public long ActualTaxDeductionId { get; set; }
        public string ActualTaxDeductionIds { get; set; }
        [Range(1,12)]
        public short SalaryMonth { get; set; }
        [Range(2020, 2050)]
        public short SalaryYear { get; set; }
        public long EmployeeId { get; set; } = 0;
        public long FiscalYearId { get; set; }
    }
}
