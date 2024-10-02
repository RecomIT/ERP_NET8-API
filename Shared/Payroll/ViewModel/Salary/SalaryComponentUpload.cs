using Microsoft.AspNetCore.Http;
using Shared.Models;

namespace Shared.Payroll.ViewModel.Salary
{
    public class SalaryComponentUpload : BaseViewModel2
    {
        public string SalaryComponent { get; set; }
        public long AllowanceId { get; set; }
        public long DeductionId { get; set; }
        public string SalaryMonthAndYear { get; set; }
        public IFormFile ExcelFile { get; set; }
    }
}
