using Microsoft.AspNetCore.Http;
using Shared.Models;

namespace Shared.Payroll.ViewModel.Tax
{
    public class TaxDocumentUpload : BaseViewModel2
    {
        public IFormFile ExcelFile { get; set; }
    }
}
