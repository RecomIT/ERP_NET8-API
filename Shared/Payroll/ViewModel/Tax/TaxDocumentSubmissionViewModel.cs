using System;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Shared.Models;

namespace Shared.Payroll.ViewModel.Tax
{
    //Employee AIT Submission
    public class TaxDocumentSubmissionViewModel : BaseViewModel1
    {
        public long SubmissionId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Range(1, long.MaxValue)]
        public long FiscalYearId { get; set; }
        public string FiscalYearRange { get; set; }
        [StringLength(200)]
        public string CertificateType { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public bool IsAuction { get; set; }
        [StringLength(200)]
        public string FileName { get; set; }
        [StringLength(200)]
        public string ActualFileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        [StringLength(100)]
        public string FileFormat { get; set; }
        //[RequiredIfValue("SubmissionId", new string[] { "0" })]
        public IFormFile File { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        public string StateStatus { get; set; }
    }
}
