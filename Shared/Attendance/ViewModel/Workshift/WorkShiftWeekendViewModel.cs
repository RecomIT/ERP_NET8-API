using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.ViewModel.Workshift
{
    public class WorkShiftWeekendViewModel : BaseViewModel2
    {
        public long ShiftWeekendId { get; set; }
        public string DayName { get; set; } // Friday, Saturday, Sunday & 4 Others ...
        [StringLength(100)]
        public string WorkShiftName { get; set; }
        public long WorkShiftId { get; set; }
        public string ShiftTitle { get; set; }
    }
}
