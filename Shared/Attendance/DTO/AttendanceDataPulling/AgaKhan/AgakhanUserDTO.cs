using System.ComponentModel.DataAnnotations;

namespace Shared.Attendance.DTO.AttendanceDataPulling.AgaKhan
{
    public class AgakhanUserDTO
    {
        public int nUserIdn { get; set; }
        public string sUserName { get; set; }
        public int nDepartmentIdn { get; set; }
        [StringLength(100)]
        public string sTelNumber { get; set; }
        [StringLength(100)]
        public string sEmail { get; set; }
        [StringLength(100)]
        public string sUserID { get; set; }
        public int nStartDate { get; set; }
        public int nEndDate { get; set; }
        public int nAdminLevel { get; set; }
        public int nAuthMode { get; set; }
        public int nAuthLimitCount { get; set; }
        public int nTimedAPB { get; set; }
        public int nEncryption { get; set; }
    }
}
