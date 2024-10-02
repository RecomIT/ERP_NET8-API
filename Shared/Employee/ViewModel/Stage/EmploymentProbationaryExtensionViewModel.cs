using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentProbationaryExtensionViewModel : BaseViewModel3
    {
        public long ProbationaryExtensionId { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DateOfJoining { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExtensionFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExtensionTo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        [StringLength(50)]
        public string TotalRatingScore { get; set; }
        [StringLength(50)]
        public string AppraiserId { get; set; }
        [StringLength(200)]
        public string AppraiserComment { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string GradeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
