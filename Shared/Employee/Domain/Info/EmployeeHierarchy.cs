using System;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeHierarchy"), Index("SupervisorId", "LineManagerId", "ManagerId", "HeadOfDepartmentId", "HRAuthorityId", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeHierarchy_NonClusteredIndex")]
    public class EmployeeHierarchy : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("EmployeeInformation")]
        public long EmployeeId { get; set; }
        public EmployeeInformation EmployeeInformation { get; set; }
        public long? SupervisorId { get; set; }  // LEVEL 1
        [StringLength(100)]
        public string SupervisorName { get; set; }
        public long? LineManagerId { get; set; } // LEVEL 2
        [StringLength(100)]
        public string LineManagerName { get; set; }
        public long? ManagerId { get; set; } // LEVEL 3
        [StringLength(100)]
        public string ManagerName { get; set; }
        public long? HeadOfDepartmentId { get; set; } // LEVEL 4
        [StringLength(100)]
        public string HeadOfDepartmentName { get; set; }
        public long? HRAuthorityId { get; set; } // LEVEL 5
        [StringLength(100)]
        public string HRAuthorityName { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActivationDate { get; set; }
    }
}
