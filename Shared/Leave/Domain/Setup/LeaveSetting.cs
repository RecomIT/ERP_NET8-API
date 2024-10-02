using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Leave.Domain.Setup
{
    [Table("HR_LeaveSetting"), Index("CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_LeaveSetting_NonClusteredIndex")]
    public class LeaveSetting : BaseModel2
    {
        [Key]
        public long LeaveSettingId { get; set; }

        // ----------------------------------------
        [ForeignKey("LeaveType")]
        public long LeaveTypeId { get; set; }
        public LeaveType LeaveType { get; set; }
        // ----------------------------------------


        // New Column Added By Md. Mahbur Rahman
        // ....................................
        public bool MandatoryNumberOfDays { get; set; }


        // Set NoOfDays Nullable By Md. Mahbur Rahman
        // ....................................
        [Range(1, short.MaxValue)]
        public short? NoOfDays { get; set; }


        public bool IsProratedLeaveBalanceApplicable { get; set; }


        public short? MaxDaysLeaveAtATime { get; set; }

        public bool IsHolidayIncluded { get; set; }
        public bool IsDayOffIncluded { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }


        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveTo { get; set; }

        public bool IsCarryForward { get; set; }

        public short? MaxDaysCarryForward { get; set; }



        [Required(ErrorMessage = "LeaveApplicableFor is required.")]
        [StringLength(200, ErrorMessage = "LeaveApplicableFor cannot exceed 200 characters.")]
        public string LeaveApplicableFor { get; set; }
        public short RequestDaysBeforeTakingLeave { get; set; }



        [StringLength(50)]
        public string FileAttachedOption { get; set; } //None,Optional,Mandatory
        //public bool IsLeaveFileAttached { get; set; }
        public bool? IsMinimumDaysRequiredForFileAttached { get; set; }
        public short? RequiredDaysForFileAttached { get; set; }


        public short? MaximumTimesInServicePeriod { get; set; }

        public bool? IsMinimumServicePeroid { get; set; }
        public short? MinimumServicePeroid { get; set; }

        public bool IsConfirmationRequired { get; set; }


        // Annual Leave
        // Leave Encashment
        public bool IsLeaveEncashable { get; set; }

        public short? MinEncashablePercentage { get; set; }
        public short? MaxEncashablePercentage { get; set; }


        public string CalculateBalanceBasedOn { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DaysPerCycle { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal GainDaysPerCycle { get; set; }








        public bool? AcquiredViaOffDayWork { get; set; }
        public bool? ShowFullCalender { get; set; } // When apply a leave request


        // Compasation Leave
        public short? DeadlineForUtilizationLeave { get; set; }


        public bool? IsRequiredEstimatedDeliveryDate { get; set; }

        [Display(Name = "Is Required to Apply Minimum Days Before EDD")]
        public bool? IsRequiredToApplyMinimumDaysBeforeEDD { get; set; }

        public short? RequiredDaysBeforeEDD { get; set; }


        [StringLength(50)]
        public string StateStatus { get; set; }






        [StringLength(10)]
        public string NoOfDaysBN { get; set; }
        public long EmployeeTypeId { get; set; }

        [StringLength(10)]
        public string MaxDaysLeaveAtATimeBN { get; set; }

        [StringLength(50)]
        public string JobType { get; set; }
        [StringLength(50)]
        public string EmployeeType { get; set; }

        public short DaysPastTodayOpenForLeave { get; set; }
        public short DaysBeforeTodayOpenForLeave { get; set; }

        [StringLength(50)]
        public string UnitOfServicePeroid { get; set; }



    }
}
