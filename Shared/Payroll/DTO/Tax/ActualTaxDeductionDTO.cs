using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.DTO.Tax
{
    public class ActualTaxDeductionDTO
    {
        public long ActualTaxDeductionId { get; set; }
        [Range(1,long.MaxValue)]
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        [Range(1, 12)]
        public short SalaryMonth { get; set; }
        [Range(2020, 2050)]
        public short SalaryYear { get; set; }
        [StringLength(1000)]
        public string FilePath { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ActualTaxAmount { get; set; }
        [StringLength(500)]
        public string ActualFileName { get; set; }
        [StringLength(500)]
        public string SystemFileName { get; set; }
        [StringLength(50)]
        public string FileFormat { get; set; }
    }
}
