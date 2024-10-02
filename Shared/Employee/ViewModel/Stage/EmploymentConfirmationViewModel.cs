using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentConfirmationViewModel : BaseViewModel3
    {
        public long ConfirmationProposalId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(5)]
        public string TotalRatingScore { get; set; }
        [StringLength(200)]
        public string AppraiserComment { get; set; }
        [Required]
        public DateTime? DateOfJoining { get; set; }
        [Required]
        public DateTime? ConfirmationDate { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string GradeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool? WithPFActivation { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PFEffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PFActivationDate { get; set; }
    }
}
