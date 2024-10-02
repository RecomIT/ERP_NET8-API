using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Info
{
    public class EmployeeUploaderFileDTO
    {
        [Required, AllowedExtensions(new string[] { ".xlsx" })]
        public IFormFile File { get; set; }
    }
}
