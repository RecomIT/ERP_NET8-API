using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class ExcelFileUploaderDTO
    {
        [Required, AllowedExtensions(new string[] { ".xlsx" })]
        public IFormFile File { get; set; }
    }
}
