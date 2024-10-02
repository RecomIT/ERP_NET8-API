using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.DTO.Setup
{
    public class LeaveSettingDTO
    {
        public long LeaveSettingId { get; set; }
        [Range(1, long.MaxValue)]
        public long LeaveTypeId { get; set; }

        public bool MandatoryNumberOfDays { get; set; }

        //[Range(1, short.MaxValue)]
        public short NoOfDays { get; set; }


        public bool IsProratedLeaveBalanceApplicable { get; set; }


        public short? MaxDaysLeaveAtATime { get; set; }

        public bool IsHolidayIncluded { get; set; }
        public bool IsDayOffIncluded { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }


        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }

        public bool IsCarryForward { get; set; }
        public short? MaxDaysCarryForward { get; set; }
        public string LeaveApplicableFor { get; set; }

        public short? RequestDaysBeforeTakingLeave { get; set; }



        public string FileAttachedOption { get; set; }
        //public bool IsLeaveFileAttached { get; set; }
        public bool? IsMinimumDaysRequiredForFileAttached { get; set; }
        public short? RequiredDaysForFileAttached { get; set; }


        public short? MaximumTimesInServicePeriod { get; set; }

        public bool IsMinimumServicePeroid { get; set; }
        public short? MinimumServicePeroid { get; set; }


        public bool IsConfirmationRequired { get; set; }
        public bool IsLeaveEncashable { get; set; }

        public short? MinEncashablePercentage { get; set; }
        public short? MaxEncashablePercentage { get; set; }




        public string CalculateBalanceBasedOn { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DaysPerCycle { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal GainDaysPerCycle { get; set; }





        public bool AcquiredViaOffDayWork { get; set; }
        public bool? ShowFullCalender { get; set; } // When apply a leave request


        // Compasation Leave
        public short? DeadlineForUtilizationLeave { get; set; }


        public bool? IsRequiredEstimatedDeliveryDate { get; set; }

        [Display(Name = "Is Required to Apply Minimum Days Before EDD")]
        public bool? IsRequiredToApplyMinimumDaysBeforeEDD { get; set; }

        public short? RequiredDaysBeforeEDD { get; set; }




        [StringLength(50)]
        public string StateStatus { get; set; }





        public long EmployeeTypeId { get; set; }











        [StringLength(50)]
        public string UnitOfServicePeroid { get; set; }





        //public bool AcquiredViaOffDayWork { get; set; }
        //public bool? ShowFullCalender { get; set; }

    }
}
