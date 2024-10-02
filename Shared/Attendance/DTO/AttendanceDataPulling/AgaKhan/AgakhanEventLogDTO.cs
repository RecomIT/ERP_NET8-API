namespace Shared.Attendance.DTO.AttendanceDataPulling.AgaKhan
{
    public class AgakhanEventLogDTO
    {
        public int nEventLogIdn { get; set; }
        public int nDateTime { get; set; }
        public int nReaderIdn { get; set; }
        public int nEventIdn { get; set; }
        public int nUserID { get; set; }
        public short nIsLog { get; set; }
        public short nTNAEvent { get; set; }
        public short nIsUseTA { get; set; }
        public short nType { get; set; }
    }
}
