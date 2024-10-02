using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeHistory"), Index("EmployeeId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeHistory_NonClusteredIndex")]
    public class EmployeeHistory : BaseModel1
    {
        [Key]
        public long EmployeeHistoryId { get; set; }
        public long EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public long? SectionId { get; set; }
        public long? SubsectionId { get; set; }
        public long? UnitId { get; set; }
        //public long? BranchId { get; set; }
        [StringLength(50)]
        public string TableFlag { get; set; }  // Office/Personal/Family/Nominee/Relatives/Experience/Skill
        public string EmployeeOfficeInfos { get; set; } // Data is stored in JSON format
        public string EmployeePersonalInfos { get; set; } // Data is stored in JSON format
        public string EmployeeFamilyInfos { get; set; } // Data is stored in JSON format
        public string EmployeeNomineeInfos { get; set; } // Data is stored in JSON format
        public string EmployeeRelativesInfos { get; set; } // Data is stored in JSON format
        public string EmployeeExperienceInfos { get; set; } // Data is stored in JSON format
        public string EmployeeSkillInfos { get; set; } // Data is stored in JSON format
    }
}
