using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Info
{
    public class EmployeeOfficeInfo
    {
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [Required, MinLength(3), StringLength(100)]
        public string FirstName { get; set; }
        [StringLength(30)]
        public string LastName { get; set; }
        [Range(1, int.MaxValue)]
        public int? BranchId { get; set; }
        [Range(1, int.MaxValue)]
        public int DesignationId { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectionId { get; set; }
        public int? SubSectionId { get; set; }
        [Required]
        public string DateOfJoining { get; set; }
        [Range(1, int.MaxValue)]
        public long WorkshiftId { get; set; }
        [StringLength(15)]
        public string OfficeMobile { get; set; }
        [StringLength(200)]
        public string OfficeEmail { get; set; }
        [StringLength(100)]
        public string ReferenceNo { get; set; }
        [StringLength(100)]
        public string FingerId { get; set; }
        [StringLength(30)]
        public string JobType { get; set; }
        public long? InternalDesignationId { get; set; }
        public long? CostCenterId { get; set; }
        public long? JobCategoryId { get; set; }
        public long? EmployeeTypeId { get; set; }
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }
        public long? SupervisorId { get; set; }
        public long? HODId { get; set; }
        public short ProbationMonth { get; set; }
        public bool IsPFMember { get; set; } = false;

    }
}
