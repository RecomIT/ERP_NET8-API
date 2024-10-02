using Microsoft.AspNetCore.Http;
using Shared.Models;

namespace Shared.Payroll.ViewModel.Tax
{
    public class EmployeeTaxZoneUpload : BaseViewModel2
    {
        public IFormFile ExcelFile { get; set; }
    }
}
