using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.Helpers.SalaryProcess
{
    public class FiscalYearInfoByDate
    {
        public long FiscalYearId { get; set; }
        [StringLength(100)]
        public string AssesmentYear { get; set; }
        public Nullable<DateTime> FiscalYearFrom { get; set; }
        public Nullable<DateTime> FiscalYearTo { get; set; }
    }
}
