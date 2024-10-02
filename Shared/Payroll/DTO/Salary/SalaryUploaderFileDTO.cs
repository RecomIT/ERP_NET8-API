using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.DTO.Salary
{
    public class FlatSalaryReviewUploaderFileDTO
    {
        [Required, AllowedExtensions(new string[] { ".xlsx" })]
        public IFormFile File { get; set; }
    }
}
