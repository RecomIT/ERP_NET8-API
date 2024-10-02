using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Info
{
    public class EmployeeDocumentDTO
    {
        public long DocumentId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(200)]
        public string DocumentName { get; set; } // Birth Certificate // NID // Passport // TIN // Police Verification
        [Required, StringLength(100)]
        public string DocumentNumber { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(200)]
        public string ActualFileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        [StringLength(100)]
        public string FileFormat { get; set; } // Extension
        [RequiredIfValue("DocumentId", new string[] { "0" }), AllowedExtensions(new string[] { ".pdf", ".jpeg", ".jpg", ".png" })]
        public IFormFile File { get; set; }
    }
}
