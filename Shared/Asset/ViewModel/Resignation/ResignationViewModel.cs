using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Asset.ViewModel.Resignation
{
    public class ResignationViewModel
    {
        public long AssigningId { get; set; }
        public long? EmployeeId { get; set; }
        [Column(TypeName ="date")]
        public Nullable<DateTime> DateOfJoining { get; set; }    
        public string BranchName { get; set; }     
        [Column(TypeName = "date")]
        public Nullable<DateTime> TerminationDate { get; set; }       
        public string TerminationStatus { get; set; }
        public string JobType { get; set; }
        public string OfficeEmail { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsReturned { get; set; }
        //public string Status { get; set; }

    }
}
