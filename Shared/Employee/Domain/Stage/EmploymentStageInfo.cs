using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Stage
{
    [Table("HR_EmploymentStageInfo")]
    public class EmploymentStageInfo : BaseModel3
    {
        [Key]
        public long EmploymentStageInfoId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string ChangeType { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        //public long BranchId { get; set; }
        public long DivisionId { get; set; }
    }
}
