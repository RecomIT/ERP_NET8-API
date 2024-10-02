using Microsoft.AspNetCore.Http;
using Shared.Models;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmployeePFActivationUpload : BaseViewModel2
    {
        public IFormFile ExcelFile { get; set; }
    }
}
