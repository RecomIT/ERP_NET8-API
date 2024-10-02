using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Holiday
{
    [Table("HR_YearlyHolidays"), Index("Title", "StartDate", "EndDate", "Type", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_YearlyHolidays_NonClusteredIndex")]
    public class YearlyHoliday : BaseModel2
    {
        [Key]
        public long YearlyHolidayId { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(200)]
        public string TitleInBengali { get; set; }
        public short StartMonth { get; set; }
        public short StartYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? StartDate { get; set; }
        public short EndMonth { get; set; }
        public short EndYear { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EndDate { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(50)]
        public string SpecifiedFor { get; set; } // All / Division / Branch / Unit
        public string DesignationId { get; set; } // 1,2,3,4,5,6,7
        public string DepartmentId { get; set; } // 1,2,3,4,5,6,7
        public string SectionId { get; set; } // 1,2,3,4,5,6,7
        public string UnitId { get; set; } // 1,2,3,4,5,6,7
        //public string BranchId { get; set; } // 1,2,3,4,5,6,7
        public string DivisionId { get; set; } // 1,2,3,4,5,6,7
        public int? PublicHolidayId { get; set; }
    }
}
