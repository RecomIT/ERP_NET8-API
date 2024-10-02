using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentStageInfoVM : BaseViewModel4
    {
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
        //
        public string EmployeeName { get; set; }
        public string DivisionName { get; set; }
        public int TotalHead { get; set; }
    }
}
