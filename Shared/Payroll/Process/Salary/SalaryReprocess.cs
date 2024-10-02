using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.Process.Salary
{
    public class SalaryReprocess
    {
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, long.MaxValue)]
        public long SalaryProcessId { get; set; }
        [Range(1, long.MaxValue)]
        public long SalaryProcessDetailId { get; set; }
        public bool WithTaxProcess { get; set; }
        [Range(1, short.MaxValue)]
        public short Month { get; set; }
        [Range(1, short.MaxValue)]
        public short Year { get; set; }
    }
}
