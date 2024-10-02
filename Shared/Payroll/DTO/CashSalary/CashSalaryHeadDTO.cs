using Microsoft.AspNetCore.Http;
using Shared.Models;
using System.ComponentModel.DataAnnotations;


namespace Shared.Payroll.DTO.CashSalary
{
    public class CashSalaryHeadDTO : BaseViewModel3
    {
        public long? CashSalaryHeadId { get; set; }
        [Required, StringLength(200)]
        public string CashSalaryHeadName { get; set; }
        [StringLength(20)]
        public string CashSalaryHeadCode { get; set; }
        [StringLength(200)]
        public string CashSalaryHeadNameInBengali { get; set; }
        public bool? IsActive { get; set; }
    }

    public class CashSalaryHeadUpload
    {
        public IFormFile ExcelFile { get; set; }
    }
}
