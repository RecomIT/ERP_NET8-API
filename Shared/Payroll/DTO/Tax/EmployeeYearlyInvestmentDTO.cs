using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class EmployeeYearlyInvestmentDTO
    {
        public long Id { get; set; }
        //[Range(1,long.MaxValue)]
        public long FiscalYearId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required,Column(TypeName = "decimal(18,2)")]
        public decimal InvestmentAmount { get; set; }
    }

    public class UploadEmployeeYearlyInvestmentDTO : BaseModel
    {
        [AllowedExtensions(new string[] { ".xls", ".xlsx" })]
        public IFormFile ExcelFile { get; set; }
        public long FiscalYearId { get; set; }
    }
}
