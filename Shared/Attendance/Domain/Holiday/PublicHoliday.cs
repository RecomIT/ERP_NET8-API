using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.Domain.Holiday
{
    [Table("HR_PublicHolidays"), Index("Title", "ReligionName", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_PublicHolidays_NonClusteredIndex")]
    public class PublicHoliday : BaseModel2
    {
        [Key]
        public int PublicHolidayId { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(200)]
        public string TitleInBengali { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public short Month { get; set; }
        public short Date { get; set; }
        [StringLength(20)]
        public string Type { get; set; } // Religious / National
        public bool IsDepandentOnMoon { get; set; }
        [StringLength(100)]
        public string ReligionName { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(20)]
        public string MonthName { get; set; }
        [StringLength(30)]
        public string Region { get; set; }
    }
}
