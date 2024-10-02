using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
    public static class AttendanceStatus
    {
        public static string Present = "Present"; //P
        public static string Absent = "Absent"; //A
        public static string Late = "Late"; //L
        public static string Holiday = "Holiday"; //H
        public static string Leave = "Leave"; //LV
        public static string ShortLeave = "Short Leave"; //SLV
    }
}
