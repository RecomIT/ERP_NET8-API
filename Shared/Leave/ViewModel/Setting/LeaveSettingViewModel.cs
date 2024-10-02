
namespace Shared.Leave.ViewModel.Setting
{
    public class LeaveSettingViewModel
    {
        public long LeaveSettingId { get; set; }
        public long LeaveTypeId { get; set; }
        public bool MandatoryNumberOfDays { get; set; }
        public short? NoOfDays { get; set; }
        public bool IsProratedLeaveBalanceApplicable { get; set; }
        public short? MaxDaysLeaveAtATime { get; set; }
        public bool IsHolidayIncluded { get; set; }
        public bool IsDayOffIncluded { get; set; }
        public bool IsActive { get; set; }
        public string Remarks { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public bool IsCarryForward { get; set; }
        public short? MaxDaysCarryForward { get; set; }
        public string LeaveApplicableFor { get; set; }
        public short RequestDaysBeforeTakingLeave { get; set; }
        public string FileAttachedOption { get; set; }
        public bool? IsMinimumDaysRequiredForFileAttached { get; set; }
        public short? RequiredDaysForFileAttached { get; set; }
        public short? MaximumTimesInServicePeriod { get; set; }
        public bool? IsMinimumServicePeroid { get; set; }
        public short? MinimumServicePeroid { get; set; }
        public bool IsConfirmationRequired { get; set; }
        public bool IsLeaveEncashable { get; set; }
        public short? MinEncashablePercentage { get; set; }
        public short? MaxEncashablePercentage { get; set; }
        public string CalculateBalanceBasedOn { get; set; }
        public decimal DaysPerCycle { get; set; }
        public decimal GainDaysPerCycle { get; set; }
        public bool? AcquiredViaOffDayWork { get; set; }
        public bool? ShowFullCalender { get; set; }
        public short? DeadlineForUtilizationLeave { get; set; }
        public bool? IsRequiredEstimatedDeliveryDate { get; set; }
        public bool? IsRequiredToApplyMinimumDaysBeforeEDD { get; set; }
        public short? RequiredDaysBeforeEDD { get; set; }
        public string StateStatus { get; set; }
        public string NoOfDaysBN { get; set; }
        public long EmployeeTypeId { get; set; }
        public string MaxDaysLeaveAtATimeBN { get; set; }
        public string JobType { get; set; }
        public string EmployeeType { get; set; }
        public short DaysPastTodayOpenForLeave { get; set; }
        public short DaysBeforeTodayOpenForLeave { get; set; }
        public string UnitOfServicePeroid { get; set; }
    }
}
