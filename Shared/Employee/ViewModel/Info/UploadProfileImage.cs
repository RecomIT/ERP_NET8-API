using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Info
{
    public class UploadProfileImage
    {
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public string ImagePath { get; set; }
        [DataType(DataType.Upload)]
        [Required, AllowedExtensions(new string[] { ".jpeg", ".jpg", ".png" }, ErrorMessage = "Invalid File format")]
        public IFormFile Image { get; set; }
    }
}
