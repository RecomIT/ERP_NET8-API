using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;

namespace Shared.Payroll.DTO.Salary
{
    public class SalaryAllowanceArrearAdjustmentDTO
    {
        public long Id { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public long EmployeeId { get; set; }
        public long AllowanceNameId { get; set; }
        public short? SalaryMonth { get; set; }
        public short? SalaryYear { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> SalaryDate { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? CalculationForDays { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        public short? ArrearAdjustmentMonth { get; set; }
        public short? ArrearAdjustmentYear { get; set; }
    }
    public class UploadSalaryAllowanceArrearAdjustmentDTO
    {
        [Required]
        public long AllowanceNameId { get; set; }
        [Required]
        public short SalaryMonth { get; set; }
        [Required]
        public short SalaryYear { get; set; }

        [Required, AllowedExtensions(new string[] { ".xlsx" })]
        public IFormFile ExcelFile { get; set; }
    }
    public class SalaryAllowanceArrearAdjustmentMasterDTO
    {
        [Required, Range(1, long.MaxValue)]
        public long AllowanceNameId { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        [Required, Range(1, short.MaxValue)]
        public short Month { get; set; }
        [Required, Range(1, short.MaxValue)]
        public short Year { get; set; }
        [Required, Range(1, short.MaxValue)]
        public short AdjustmentMonth { get; set; }
        [Required, Range(1, short.MaxValue)]
        public short AdjustmentYear { get; set; }
        public List<SalaryAllowanceArrearAdjustmentDetailDTO> Details { get; set; }
    }
    public class SalaryAllowanceArrearAdjustmentDetailDTO
    {
        [Required, Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
    }
    public class ArrearAdjustmentApprovalDTO
    {
        [Required,StringLength(100)]
        public string StateStatus { get; set; }
        public List<long> Ids { get; set; }
    }
}
