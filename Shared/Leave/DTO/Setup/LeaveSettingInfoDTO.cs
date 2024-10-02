namespace Shared.Leave.DTO.Setup
{
    public class LeaveSettingInfoDTO
    {
        public string LeavePeriodStart { get; set; }
        public string LeavePeriodEnd { get; set; }
        public string LeaveTypeName { get; set; }
        public short RequestDaysBeforeTakingLeave { get; set; }
        public short MaxDaysLeaveAtATime { get; set; }
        public bool AcquiredViaOffDayWork { get; set; }
        public string FileAttachedOption { get; set; }
        //public bool IsLeaveFileAttached { get; set; }
        public bool? ShowFullCalender { get; set; }

        // File Attached

        public bool IsMinimumDaysRequiredForFileAttached { get; set; }
        public short RequiredDaysForFileAttached { get; set; }


        public short RequiredDaysBeforeEDD { get; set; }
        public bool MandatoryNumberOfDays { get; set; }

        //public string FilePath { get; set; }
        //public string FileName { get; set; }

        public decimal TotalLeave { get; set; }

        public short LeaveSettingId { get; set; }
        public short CompanyId { get; set; }
        public short OrganizationId { get; set; }


    }
}
