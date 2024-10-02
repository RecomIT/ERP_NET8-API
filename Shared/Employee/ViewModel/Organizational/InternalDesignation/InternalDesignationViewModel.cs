using Microsoft.AspNetCore.Http;
using Shared.BaseModels.For_ViewModel;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Organizational.InternalDesignation
{
    public class InternalDesignationViewModel : BaseViewModel3
    {
        public long InternalDesignationId { get; set; }
        [StringLength(150)]
        public string InternalDesignationName { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
    }

    public class UploadInternalDesignationViewModel : BaseViewModel2
    {
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile ExcelFile { get; set; }
    }

}
