using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class AITSubmissionDTO
    {
        public long SubmissionId { get; set; }
        public long EmployeeId { get; set; } = 0;
        [Range(1, long.MaxValue)]
        public long FiscalYearId { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public short? NumberOfCar { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        public bool? IsAuction { get; set; }
        [RequiredIfValue("SubmissionId", new string[] { "0" }), AllowedExtensions(new string[] { ".pdf",".png",".jpg",".jpeg"})]
        public IFormFile File { get; set; }
    }
}
