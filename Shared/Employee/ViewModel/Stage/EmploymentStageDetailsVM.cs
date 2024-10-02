using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentStageDetailsVM : BaseViewModel4
    {
        public long EmploymentStageDetailId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string Head { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        [StringLength(100)]
        public string HeadType { get; set; }
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
        public long DivisionId { get; set; }
        // Foreign Key
        public long? PromotionalInfoId { get; set; }
        //
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? DateOfJoining { get; set; }
    }
}
