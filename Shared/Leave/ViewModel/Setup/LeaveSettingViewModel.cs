using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Leave.ViewModel.Setup
{
    public class LeaveSettingViewModel : BaseViewModel3
    {
        public long LeaveSettingId { get; set; }
        public long EmployeeTypeId { get; set; }
        [Range(1, short.MaxValue)]


        // Added By Md. Mahbur Rahman
        public bool mandatoryNoOfDays { get; set; }
        public short NoOfDays { get; set; }


        public bool IsProratedLeaveBalanceApplicable { get; set; }


        [StringLength(10)]
        public string NoOfDaysBN { get; set; }
        public short MaxDaysLeaveAtATime { get; set; }
        [StringLength(10)]
        public string MaxDaysLeaveAtATimeBN { get; set; }
        public bool IsHolidayIncluded { get; set; }
        public bool IsDayOffIncluded { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [Range(1, long.MaxValue)]
        public long LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; }
        public string EmployeeTypeName { get; set; }
        public string ShortName { get; set; }
        public short MaxDaysCarryForward { get; set; }
        public bool IsCarryForward { get; set; }
        public string LeaveApplicableFor { get; set; }
        public short RequestDaysBeforeTakingLeave { get; set; }
        [StringLength(50)]
        public string FileAttachedOption { get; set; }

        public decimal MaximumTimesInServicePeriod { get; set; }
        public bool IsMinimumServicePeroid { get; set; }
        public decimal MinimumServicePeroid { get; set; }
        public bool IsConfirmationRequired { get; set; }

        public string CalculateBalanceBasedOn { get; set; }
        public decimal DaysPerCycle { get; set; }
        public decimal GainDaysPerCycle { get; set; }


        public bool? ShowFullCalender { get; set; }





        // Added By Md. Mahbur Rahman
        //public bool FileAttachedOptionOnOff { get; set; }

        public bool MandatoryNumberOfDays { get; set; }


        //public bool IsLeaveFileAttached { get; set; }

        public bool IsMinimumDaysRequiredForFileAttached { get; set; }
        public short RequiredDaysForFileAttached { get; set; }


        // Compasation Leave
        public bool AcquiredViaOffDayWork { get; set; }
        public short DeadlineForUtilizationLeave { get; set; }




        // Annual Leave
        // Leave Encashment
        public bool IsLeaveEncashable { get; set; }
        public short? MinEncashablePercentage { get; set; }
        public short? MaxEncashablePercentage { get; set; }


        // Maternity Leave
        //public Nullable<DateTime> EstimatedDeliveryDate { get; set; }


        public bool IsRequiredEstimatedDeliveryDate { get; set; }
        public bool IsRequiredToApplyMinimumDaysBeforeEDD { get; set; }
        public short? RequiredDaysBeforeEDD { get; set; }


    }
}
