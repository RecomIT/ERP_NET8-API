using System;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using DAL.Logger.Interface;
using Shared.OtherModels.Response;
using Shared.Services;
using System.Linq;
using Dapper;
using DAL.DapperObject.Interface;
using DAL.Repository.Leave.Interface;
using Shared.Leave.DTO.Setup;
using Shared.Leave.Domain.Setup;

namespace DAL.Repository.Leave.Implementation
{
    public class LeaveSettingRepository : ILeaveSettingRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public LeaveSettingRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LeaveSetting>> GetAllAsync(AppUser user)
        {
            IEnumerable<LeaveSetting> list = new List<LeaveSetting>();
            try
            {
                var query = $@"SELECT * FROM HR_LeaveSetting Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<LeaveSetting>(user.Database, query, new { user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingRepository", "GetAllAsync", user);
            }
            return list;
        }

        public Task<IEnumerable<LeaveSetting>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveSetting> GetByIdAsync(long id, AppUser user)
        {
            LeaveSetting leaveSetting = null;
            try
            {
                var query = $@"SELECT * FROM HR_LeaveSetting Where LeaveSettingId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                leaveSetting = await _dapper.SqlQueryFirstAsync<LeaveSetting>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingRepository", "GetByIdAsync", user);
            }
            return leaveSetting;
        }

        public async Task<ExecutionStatus> SaveAsync(LeaveSettingDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    {
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                if (model.LeaveSettingId == 0)
                                {
                                    LeaveSetting leaveSetting = new LeaveSetting()
                                    {

                                        AcquiredViaOffDayWork = model.AcquiredViaOffDayWork,
                                        CalculateBalanceBasedOn = model.CalculateBalanceBasedOn,

                                        DaysPerCycle = model.DaysPerCycle,
                                        GainDaysPerCycle = model.GainDaysPerCycle,


                                        FileAttachedOption = model.FileAttachedOption,
                                        IsMinimumDaysRequiredForFileAttached = model.IsMinimumDaysRequiredForFileAttached,
                                        RequiredDaysForFileAttached = model.RequiredDaysForFileAttached,

                                        IsProratedLeaveBalanceApplicable = model.IsProratedLeaveBalanceApplicable,

                                        IsRequiredEstimatedDeliveryDate = model.IsRequiredEstimatedDeliveryDate,
                                        IsRequiredToApplyMinimumDaysBeforeEDD = model.IsRequiredToApplyMinimumDaysBeforeEDD,
                                        RequiredDaysBeforeEDD = model.RequiredDaysBeforeEDD,



                                        IsActive = model.IsActive,
                                        //IsLeaveFileAttached = model.IsLeaveFileAttached,

                                        IsCarryForward = model.IsCarryForward,
                                        IsConfirmationRequired = model.IsConfirmationRequired,
                                        IsDayOffIncluded = model.IsDayOffIncluded,
                                        IsHolidayIncluded = model.IsHolidayIncluded,

                                        IsLeaveEncashable = model.IsLeaveEncashable,
                                        MinEncashablePercentage = model.MinEncashablePercentage,
                                        MaxEncashablePercentage = model.MaxEncashablePercentage,
                                        IsMinimumServicePeroid = model.IsMinimumServicePeroid,
                                        MinimumServicePeroid = model.MinimumServicePeroid,
                                        DeadlineForUtilizationLeave = model.DeadlineForUtilizationLeave,



                                        LeaveTypeId = model.LeaveTypeId,
                                        CompanyId = user.CompanyId,
                                        OrganizationId = user.OrganizationId,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,

                                        LeaveApplicableFor = model.LeaveApplicableFor,

                                        MaxDaysCarryForward = model.MaxDaysCarryForward,
                                        MaxDaysLeaveAtATime = model.MaxDaysLeaveAtATime,
                                        MaximumTimesInServicePeriod = model.MaximumTimesInServicePeriod,

                                        MandatoryNumberOfDays = model.MandatoryNumberOfDays,
                                        NoOfDays = model.NoOfDays,
                                        RequestDaysBeforeTakingLeave = model.RequestDaysBeforeTakingLeave ?? 0,
                                        UnitOfServicePeroid = model.UnitOfServicePeroid,
                                        Remarks = model.Remarks,

                                        StateStatus = "Entered",
                                        ShowFullCalender = model.ShowFullCalender,

                                        DaysBeforeTodayOpenForLeave = 0,
                                        DaysPastTodayOpenForLeave = 0,
                                        EmployeeTypeId = 0,
                                        JobType = ""
                                    };

                                    var parameters = DapperParam.GetKeyValuePairsDynamic(leaveSetting, addBaseProperty: true);
                                    parameters.Remove("LeaveSettingId");
                                    parameters.Remove("LeaveType");

                                    string query = Utility.GenerateInsertQuery(tableName: "HR_LeaveSetting", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");

                                    var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);
                                    if (rawAffected > 0)
                                    {
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.Status = true;
                                        executionStatus.Msg = "Data has been saved successfully";
                                        transaction.Commit();
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.Status = false;
                                        executionStatus.Msg = "Data has been failed to save";
                                    }
                                }
                                else if (model.LeaveSettingId > 0)
                                {
                                    var leaveSettingInDb = await GetByIdAsync(model.LeaveSettingId, user);
                                    if (leaveSettingInDb != null)
                                    {
                                        #region Seeding data
                                        leaveSettingInDb.AcquiredViaOffDayWork = model.AcquiredViaOffDayWork;
                                        leaveSettingInDb.CalculateBalanceBasedOn = model.CalculateBalanceBasedOn;
                                        leaveSettingInDb.DaysBeforeTodayOpenForLeave = 0;
                                        leaveSettingInDb.DaysPastTodayOpenForLeave = 0;
                                        leaveSettingInDb.DaysPerCycle = model.DaysPerCycle;
                                        leaveSettingInDb.GainDaysPerCycle = model.GainDaysPerCycle;

                                        leaveSettingInDb.FileAttachedOption = model.FileAttachedOption;
                                        leaveSettingInDb.IsMinimumDaysRequiredForFileAttached = model.IsMinimumDaysRequiredForFileAttached;
                                        leaveSettingInDb.RequiredDaysForFileAttached = model.RequiredDaysForFileAttached;

                                        leaveSettingInDb.IsProratedLeaveBalanceApplicable = model.IsProratedLeaveBalanceApplicable;

                                        leaveSettingInDb.IsRequiredEstimatedDeliveryDate = model.IsRequiredEstimatedDeliveryDate;
                                        leaveSettingInDb.IsRequiredToApplyMinimumDaysBeforeEDD = model.IsRequiredToApplyMinimumDaysBeforeEDD;
                                        leaveSettingInDb.RequiredDaysBeforeEDD = model.RequiredDaysBeforeEDD;

                                        leaveSettingInDb.IsActive = model.IsActive;
                                        //leaveSettingInDb.IsLeaveFileAttached = model.IsLeaveFileAttached;
                                        leaveSettingInDb.IsCarryForward = model.IsCarryForward;
                                        leaveSettingInDb.IsDayOffIncluded = model.IsDayOffIncluded;
                                        leaveSettingInDb.IsHolidayIncluded = model.IsHolidayIncluded;
                                        leaveSettingInDb.IsConfirmationRequired = model.IsConfirmationRequired;

                                        leaveSettingInDb.EmployeeTypeId = 0;

                                        leaveSettingInDb.IsLeaveEncashable = model.IsLeaveEncashable;
                                        leaveSettingInDb.MinEncashablePercentage = model.MinEncashablePercentage;
                                        leaveSettingInDb.MaxEncashablePercentage = model.MaxEncashablePercentage;

                                        leaveSettingInDb.IsMinimumServicePeroid = model.IsMinimumServicePeroid;
                                        leaveSettingInDb.MinimumServicePeroid = model.MinimumServicePeroid;
                                        leaveSettingInDb.DeadlineForUtilizationLeave = model.DeadlineForUtilizationLeave;

                                        leaveSettingInDb.LeaveTypeId = model.LeaveTypeId;
                                        leaveSettingInDb.UpdatedBy = user.ActionUserId;
                                        leaveSettingInDb.UpdatedDate = DateTime.Now;
                                        leaveSettingInDb.JobType = "";

                                        leaveSettingInDb.LeaveApplicableFor = model.LeaveApplicableFor;
                                        leaveSettingInDb.MaxDaysCarryForward = model.MaxDaysCarryForward;
                                        leaveSettingInDb.MaxDaysLeaveAtATime = model.MaxDaysLeaveAtATime;
                                        leaveSettingInDb.MaximumTimesInServicePeriod = model.MaximumTimesInServicePeriod;

                                        leaveSettingInDb.MandatoryNumberOfDays = model.MandatoryNumberOfDays;
                                        leaveSettingInDb.NoOfDays = model.NoOfDays;
                                        leaveSettingInDb.RequestDaysBeforeTakingLeave = model.RequestDaysBeforeTakingLeave ?? 0;
                                        leaveSettingInDb.UnitOfServicePeroid = model.UnitOfServicePeroid;

                                        leaveSettingInDb.Remarks = model.Remarks;
                                        leaveSettingInDb.StateStatus = "Entered";
                                        leaveSettingInDb.ShowFullCalender = model.ShowFullCalender;



                                        #endregion
                                        var parameters = DapperParam.GetKeyValuePairsDynamic(leaveSettingInDb, addBaseProperty: true);
                                        parameters.Remove("LeaveSettingId");
                                        parameters.Remove("LeaveType");
                                        parameters.Remove("LeaveTypeId");

                                        string query = Utility.GenerateUpdateQuery(tableName: "HR_LeaveSetting", paramkeys: parameters.Select(x => x.Key).ToList());
                                        parameters.Add("LeaveSettingId", model.LeaveSettingId);
                                        query += $@"Where LeaveSettingId=@LeaveSettingId";

                                        var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);
                                        if (rawAffected > 0)
                                        {
                                            executionStatus = new ExecutionStatus();
                                            executionStatus.Status = true;
                                            executionStatus.Msg = "Data has been updated successfully";
                                            transaction.Commit();
                                        }
                                        else
                                        {
                                            transaction.Rollback();
                                            executionStatus = new ExecutionStatus();
                                            executionStatus.Status = false;
                                            executionStatus.Msg = "Data has been failed to update";
                                        }
                                    }
                                    else
                                    {
                                        executionStatus = ResponseMessage.Invalid("Leave Setting item is not found");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                executionStatus = ResponseMessage.Invalid("Something went wrong while saving data");
                                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingRepository", "SaveAsync", user);
                            }
                            finally { connection.Close(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid("Something went wrong while connecting to database");
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveSettingRepository", "SaveAsync", user);
            }
            return executionStatus;
        }
    }
}
