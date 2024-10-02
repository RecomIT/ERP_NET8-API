using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.ViewModel.Holiday
{
    public class YearlyHolidayViewModel : BaseViewModel3
    {
        public long YearlyHolidayId { get; set; }
        [StringLength(200)]
        public string Title { get; set; }
        [StringLength(200)]
        public string TitleInBengali { get; set; }
        public short StartMonth { get; set; }
        public short StartYear { get; set; }
        public DateTime? StartDate { get; set; }
        public short EndMonth { get; set; }
        public short EndYear { get; set; }
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
        public string DesignationId { get; set; }
        public string DepartmentId { get; set; }
        public string SectionId { get; set; }
        public string UnitId { get; set; }
        public string DivisionId { get; set; }
        public int? PublicHolidayId { get; set; }
        public bool IsDepandentOnMoon { get; set; }
    }
}
