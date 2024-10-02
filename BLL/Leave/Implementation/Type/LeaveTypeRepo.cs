

using BLL.Leave.Interface.Type;
using DAL.Context.Leave;
using Microsoft.EntityFrameworkCore;
using Shared.Leave.Domain.Setup;
using Shared.Leave.Filter.Type;
using Shared.Leave.ViewModel.Setting;
using Shared.Leave.ViewModel.Type;

namespace BLL.Leave.Implementation.Type
{
    public class LeaveTypeRepo : ILeaveTypeRepo
    {
        private readonly LeaveModuleDbContext _leaveModuleContext;

        public LeaveTypeRepo(LeaveModuleDbContext leaveModuleContext)
        {
            _leaveModuleContext = leaveModuleContext;
        }


        public async Task<IEnumerable<Select2OptionViewModel>> GetSelect2LeaveTypesAsync()
        {
            try
            {
                var select2LeaveTypes = await _leaveModuleContext.HR_LeaveTypes
                    .Select(lt => new Select2OptionViewModel
                    {
                        Id = lt.Id,
                        Text = lt.Title
                    })
                    .ToListAsync();

                return select2LeaveTypes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Select2 leave types.", ex);
            }
        }


        public async Task<IEnumerable<Select2OptionViewModel>> GetSelect2LeaveTypesAsync(LeaveType_Filter filter)
        {
            try
            {
                IQueryable<LeaveType> query = _leaveModuleContext.HR_LeaveTypes;

                if (filter != null && filter.LeaveTypeId != 0)
                {
                    query = query.Where(lt => lt.Id == filter.LeaveTypeId);
                }

                var select2LeaveTypes = await query
                    .Select(lt => new Select2OptionViewModel
                    {
                        Id = lt.Id,
                        Text = lt.Title
                    })
                    .ToListAsync();

                return select2LeaveTypes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Select2 leave types.", ex);
            }
        }


        public async Task<IEnumerable<LeaveTypeViewModel>> GetLeaveTypesAsync(LeaveType_Filter filter)
        {
            try
            {
                IQueryable<LeaveTypeViewModel> query = _leaveModuleContext.HR_LeaveTypes
                    .AsNoTracking()
                    .Where(lt => filter.LeaveTypeId == 0 || lt.Id == filter.LeaveTypeId) 
                    .Select(lt => new LeaveTypeViewModel
                    {
                        Id = lt.Id,
                        Title = lt.Title,
                        TitleInBengali = lt.TitleInBengali,
                        ShortName = lt.ShortName,
                        ShortNameInBangali = lt.ShortNameInBangali,
                        Description = lt.Description,
                        IsActive = lt.IsActive,
                        SerialNo = lt.SerialNo
                    });

                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not included in this snippet)
                throw new Exception("An error occurred while fetching leave types.", ex);
            }
        }




        public async Task<IEnumerable<LeaveTypesWithSettings>> GetLeaveTypesWithSettingsAsync(LeaveType_Filter filter)
        {
            try
            {
                IQueryable<LeaveType> query = _leaveModuleContext.HR_LeaveTypes
                    .Include(lt => lt.LeaveSettings);

                if (filter.LeaveTypeId != 0)
                {
                    query = query.Where(lt => lt.Id == filter.LeaveTypeId);
                }

                var leaveTypes = await query.Select(lt => new LeaveTypesWithSettings
                {
                    Id = lt.Id,
                    Title = lt.Title,
                    TitleInBengali = lt.TitleInBengali,
                    ShortName = lt.ShortName,
                    ShortNameInBangali = lt.ShortNameInBangali,
                    Description = lt.Description,
                    IsActive = lt.IsActive,
                    SerialNo = lt.SerialNo,
                    LeaveSettings = lt.LeaveSettings.Select(ls => new LeaveSettingViewModel
                    {
                        LeaveSettingId = ls.LeaveSettingId,
                        LeaveTypeId = ls.LeaveTypeId,
                        MandatoryNumberOfDays = ls.MandatoryNumberOfDays,
                        NoOfDays = ls.NoOfDays,
                        IsProratedLeaveBalanceApplicable = ls.IsProratedLeaveBalanceApplicable,
                        MaxDaysLeaveAtATime = ls.MaxDaysLeaveAtATime,
                        IsHolidayIncluded = ls.IsHolidayIncluded,
                        IsDayOffIncluded = ls.IsDayOffIncluded,
                        IsActive = ls.IsActive,
                        Remarks = ls.Remarks,
                        EffectiveFrom = ls.EffectiveFrom,
                        EffectiveTo = ls.EffectiveTo,
                        IsCarryForward = ls.IsCarryForward,
                        MaxDaysCarryForward = ls.MaxDaysCarryForward,
                        LeaveApplicableFor = ls.LeaveApplicableFor,
                        RequestDaysBeforeTakingLeave = ls.RequestDaysBeforeTakingLeave,
                        FileAttachedOption = ls.FileAttachedOption,
                        IsMinimumDaysRequiredForFileAttached = ls.IsMinimumDaysRequiredForFileAttached,
                        RequiredDaysForFileAttached = ls.RequiredDaysForFileAttached,
                        MaximumTimesInServicePeriod = ls.MaximumTimesInServicePeriod,
                        IsMinimumServicePeroid = ls.IsMinimumServicePeroid,
                        MinimumServicePeroid = ls.MinimumServicePeroid,
                        IsConfirmationRequired = ls.IsConfirmationRequired,
                        IsLeaveEncashable = ls.IsLeaveEncashable,
                        MinEncashablePercentage = ls.MinEncashablePercentage,
                        MaxEncashablePercentage = ls.MaxEncashablePercentage,
                        CalculateBalanceBasedOn = ls.CalculateBalanceBasedOn,
                        DaysPerCycle = ls.DaysPerCycle,
                        GainDaysPerCycle = ls.GainDaysPerCycle,
                        AcquiredViaOffDayWork = ls.AcquiredViaOffDayWork,
                        ShowFullCalender = ls.ShowFullCalender,
                        DeadlineForUtilizationLeave = ls.DeadlineForUtilizationLeave,
                        IsRequiredEstimatedDeliveryDate = ls.IsRequiredEstimatedDeliveryDate,
                        IsRequiredToApplyMinimumDaysBeforeEDD = ls.IsRequiredToApplyMinimumDaysBeforeEDD,
                        RequiredDaysBeforeEDD = ls.RequiredDaysBeforeEDD,
                        StateStatus = ls.StateStatus,
                        NoOfDaysBN = ls.NoOfDaysBN,
                        EmployeeTypeId = ls.EmployeeTypeId,
                        MaxDaysLeaveAtATimeBN = ls.MaxDaysLeaveAtATimeBN,
                        JobType = ls.JobType,
                        EmployeeType = ls.EmployeeType,
                        DaysPastTodayOpenForLeave = ls.DaysPastTodayOpenForLeave,
                        DaysBeforeTodayOpenForLeave = ls.DaysBeforeTodayOpenForLeave,
                        UnitOfServicePeroid = ls.UnitOfServicePeroid
                    }).ToList()
                }).ToListAsync();

                return leaveTypes;
            }
            catch (Exception ex)
            {
                // Log the exception (logging code not included in this snippet)
                throw new Exception("An error occurred while fetching leave types with settings.", ex);
            }
        }

        public async Task<IEnumerable<Select2OptionViewModel>> GetSelect2EncashableLeaveTypesAsync()
        {
            try
            {
                var leaveTypes = await _leaveModuleContext.HR_LeaveTypes
                    .Include(lt => lt.LeaveSettings) 
                    .ToListAsync();

                var encashableLeaveTypes = leaveTypes
                    .Where(lt => lt.LeaveSettings.Any(ls => ls.IsLeaveEncashable)) 
                    .Select(lt => new Select2OptionViewModel
                    {
                        Id = lt.Id,
                        Text = lt.Title
                    })
                    .ToList();

                return encashableLeaveTypes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching Encashable Select2 leave types.", ex);
            }
        }


        public async Task<IEnumerable<EncashableLeaveTypeSettingViewModel>> GetEncashableLeaveSettingsAsync(LeaveType_Filter filter)
        {
            try
            {
                var leaveTypesQuery = _leaveModuleContext.HR_LeaveTypes
                    .Include(lt => lt.LeaveSettings)
                    .AsQueryable();

                // Apply filter criteria if filter is not null and LeaveTypeId is specified
                if (filter?.LeaveTypeId != null && filter.LeaveTypeId != 0)
                {
                    leaveTypesQuery = leaveTypesQuery.Where(lt => lt.Id == filter.LeaveTypeId);
                }

                var leaveTypes = await leaveTypesQuery.ToListAsync();

                var encashableLeaveTypes = leaveTypes
                    .Where(lt => lt.LeaveSettings.Any(ls => ls.IsLeaveEncashable))
                    .Select(lt => new EncashableLeaveTypeSettingViewModel
                    {
                        Id = lt.Id,
                        IsLeaveEncashable = lt.LeaveSettings.First(ls => ls.IsLeaveEncashable).IsLeaveEncashable,
                        MinEncashablePercentage = lt.LeaveSettings.First(ls => ls.IsLeaveEncashable).MinEncashablePercentage,
                        MaxEncashablePercentage = lt.LeaveSettings.First(ls => ls.IsLeaveEncashable).MaxEncashablePercentage,
                        TotalLeave = _leaveModuleContext.HR_EmployeeLeaveBalance
                                .FirstOrDefault(elb => elb.LeaveTypeId == lt.Id)?.TotalLeave ?? 0
                    })
                    .ToList();

                return encashableLeaveTypes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching encashable leave settings.", ex);
            }
        }



    }
}
