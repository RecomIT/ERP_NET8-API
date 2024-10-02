using Microsoft.AspNetCore.Http;
using Shared.Models;

namespace Shared.Employee.ViewModel.Info
{
    public class EmployeeInfoUpload : BaseViewModel2
    {
        public IFormFile ExcelFile { get; set; }
    }
}
