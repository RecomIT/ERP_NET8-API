using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Stage
{
    [Table("HR_EmploymentStageDetails")]
    public class EmploymentStageDetails : BaseModel3
    {
        [Key]
        public long EmploymentStageDetailId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string HeadType { get; set; }
        [Required, StringLength(100)]
        public string Head { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        [StringLength(50)]
        public string ExistingValue { get; set; }
        [StringLength(200)]
        public string ExistingText { get; set; }
        [Required, StringLength(50)]
        public string ProposalValue { get; set; }
        [StringLength(200)]
        public string ProposalText { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; }
        public DateTime? InActiveDate { get; set; }
        [StringLength(100)]
        public string InActiveBy { get; set; }
        public DateTime? ActiveDate { get; set; }
        [StringLength(100)]
        public string ActiveBy { get; set; }
        //public long BranchId { get; set; }
        public long DivisionId { get; set; }
        // Foreign Key
        public long? EmploymentStageInfoId { get; set; }
    }
}
