using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.DTO.Request
{
    public class EmployeeLeaveDaysDTO
    {
        public int SL { get; set; }
        [Column(TypeName = "date")]
        public DateTime? Date { get; set; }
        public string DayName { get; set; }
        public long WorkShiftId { get; set; }
        public string WorkShiftName { get; set; }
        [StringLength(100), Required]
        public string Status { get; set; }
        public string Remarks { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ReplacementDate { get; set; }
    }
}
