using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Attendance.DTO.Holiday
{
    public class YearlyHolidayDTO
    {
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
        public long PublicHolidayId { get; set; }
        public string SpecifiedFor { get; set; }
    }
}
