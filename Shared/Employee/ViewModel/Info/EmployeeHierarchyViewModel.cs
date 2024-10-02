using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Info
{
    public class EmployeeHierarchyViewModel : BaseViewModel1
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public long? SupervisorId { get; set; }
        [StringLength(100)]
        public string SupervisorName { get; set; }
        public long? ManagerId { get; set; }
        [StringLength(100)]
        public string ManagerName { get; set; }
        public long? LineManagerId { get; set; }
        [StringLength(100)]
        public string LineManagerName { get; set; }
        public long? HeadOfDepartmentId { get; set; }
        [StringLength(100)]
        public string HeadOfDepartmentName { get; set; }
        public long? HRAuthorityId { get; set; }
        [StringLength(100)]
        public string HRAuthorityName { get; set; }
        [StringLength(500)]
        public string HierarchyNames { get; set; } // 1,2,3
        [StringLength(200)]
        public string HierarchyIds { get; set; }
        public bool IsActive { get; set; }
        public DateTime? ActivationDate { get; set; }

        // 

        public string SupervisorCode { get; set; }
        public string ManagerCode { get; set; }
        public string LineManagerCode { get; set; }
        public string HeadOfDepartmentCode { get; set; }
        public string HRAuthorityCode { get; set; }
    }
}
