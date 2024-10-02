using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.DTO.Holiday
{
    public class PublicHolidayDTO
    {
        public int PublicHolidayId { get; set; }
        [Required, StringLength(200)]
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
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(20)]
        public string MonthName { get; set; } // Optional
        [StringLength(100)]
        public string ReligionName { get; set; }
    }
}
