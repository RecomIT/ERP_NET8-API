using Microsoft.AspNetCore.Http;
using Shared.BaseModels.For_ViewModel;
using Shared.Helpers.ValidationFilters;

namespace Shared.Employee.ViewModel.Account
{
    public class UploadAccountInfoViewModel : BaseViewModel2
    {
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile ExcelFile { get; set; }
    }
}
