using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentConfirmationOrProbationVM : BaseViewModel4
    {
        public long Id { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string Type { get; set; }
        [StringLength(100)]
        public string Flag { get; set; }
        public DateTime? ConfirmationDate { get; set; }
        public DateTime? ExtensionFrom { get; set; }
        public DateTime? ExtensionTo { get; set; }
        [StringLength(50)]
        public string TotalRatingScore { get; set; }
        [StringLength(50)]
        public string AppraiserId { get; set; }
        [StringLength(200)]
        public string AppraiserComment { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public long ChangeableInfoId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        //
        public string EmployeeName { get; set; }
        public string EmployeeCode { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public DateTime? DateOfJoining { get; set; }
    }
}
