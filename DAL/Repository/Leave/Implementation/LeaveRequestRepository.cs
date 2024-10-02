using Dapper;
using System;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using DAL.Logger.Interface;
using Shared.Services;
using System.Linq;
using System.Data;
using Shared.OtherModels.Pagination;
using Microsoft.VisualBasic.FileIO;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using DAL.Repository.Leave.Interface;
using Shared.Leave.DTO.Request;
using Shared.Leave.Filter.Request;
using Shared.Leave.Domain.Request;
using Shared.Leave.ViewModel.Request;

namespace DAL.Repository.Leave.Implementation
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        private readonly IDapperData _dapper;
        private readonly IDALSysLogger _sysLogger;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly IEmployeeHierarchyRepository _employeeHierarchyRepository;
        private readonly ILeaveHistoryRepository _leaveHistoryRepository;
        public LeaveRequestRepository(IEmployeeHierarchyRepository employeeHierarchyRepository, ILeaveTypeRepository leaveTypeRepository, IDapperData dapper, IDALSysLogger sysLogger, ILeaveBalanceRepository leaveBalanceRepository, ILeaveHistoryRepository leaveHistoryRepository)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _leaveTypeRepository = leaveTypeRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
            _employeeHierarchyRepository = employeeHierarchyRepository;
            _leaveHistoryRepository = leaveHistoryRepository;
        }

        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmployeeLeaveRequest>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmployeeLeaveRequest>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<EmployeeLeaveRequest> GetByIdAsync(long id, AppUser user)
        {
            EmployeeLeaveRequest employeeLeaveRequest = null;
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeLeaveRequest Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND CompanyId= @CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new { EmployeeLeaveRequestId = id, user.CompanyId, user.OrganizationId };
                employeeLeaveRequest = await _dapper.SqlQueryFirstAsync<EmployeeLeaveRequest>(user.Database, query, parameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "GetByIdAsync", user);
            }
            return employeeLeaveRequest;
        }
        public async Task<string> GetLeaveCodeAysnc(AppUser user)
        {
            string leaveCode = "";
            try
            {
                var query = $@"(Select Case When MAX(EmployeeLeaveCode) IS NULL Then 'ELRC-0000000001'  
					ELSE ('ELRC-' +RIGHT('0000000000'+ Convert(NVarchar(50),MAX(Convert(int,SUBSTRING(EmployeeLeaveCode,6,20)))+1),10)) END 
					From HR_EmployeeLeaveRequest Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId)";
                var parameters = new { user.CompanyId, user.OrganizationId };
                leaveCode = await _dapper.SqlQueryFirstAsync<string>(user.Database, query, parameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return leaveCode;
        }
        public Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        //public async Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(LeaveRequest_Filter filter, AppUser user)
        //{
        //    IEnumerable<EmployeeLeaveRequestViewModel> list = new List<EmployeeLeaveRequestViewModel>();
        //    try {
        //        var sp = "sp_HR_SubordinatesEmployeeLeaveRequest";
        //        filter.SupervisorId = user.ActionUserId;
        //        var parameters = DapperParam.AddParams(filter, user, addUserId: false);
        //        list = await _dapper.SqlQueryListAsync<EmployeeLeaveRequestViewModel>(user.Database, sp, parameters, CommandType.StoredProcedure);
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetSubordinatesEmployeeLeaveRequestAsync", user);
        //    }
        //    return list;
        //}
        public async Task<bool> IsLeavePendingAsync(long id, AppUser user)
        {
            bool status = false;
            try
            {
                var query = $@"(Select Count(*) From HR_EmployeeLeaveRequest 
				Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND StateStatus IN('Pending','Recheck') AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId)";
                var parameters = new { EmployeeLeaveRequestId = id, user.CompanyId, user.OrganizationId };
                var count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, parameters);
                if (count > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "IsLeavePendingAsync", user);
            }
            return status;
        }
        public async Task<ExecutionStatus> LeaveRequestApprovalAsync(EmployeeLeaveRequestStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var leaveRequestInDb = await GetByIdAsync(model.EmployeeLeaveRequestId, user);
                if (leaveRequestInDb != null)
                {
                    if (leaveRequestInDb.StateStatus == StateStatus.Pending || leaveRequestInDb.StateStatus == StateStatus.Recommended)
                    {
                        var thisEmployeeHierarchy = await _employeeHierarchyRepository.GetEmployeeActiveHierarchy(leaveRequestInDb.EmployeeId, user);
                        long supervisorId = 0;
                        long hodId = 0;
                        long thisUserId = Utility.TryParseLong(user.ActionUserId);
                        if (thisEmployeeHierarchy != null)
                        {
                            supervisorId = thisEmployeeHierarchy.SupervisorId ?? 0;
                            hodId = thisEmployeeHierarchy.HeadOfDepartmentId ?? 0;
                        }

                        bool isSupervisor = supervisorId != 0 && supervisorId == thisUserId;
                        bool isHOD = hodId != 0 && hodId == thisUserId;

                        // Supervisor & HOD both are same
                        if ((supervisorId != 0 && supervisorId == thisUserId)
                            == (hodId != 0 && hodId == thisUserId)
                            )
                        {
                            using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                            {
                                connection.Open();
                                {
                                    using (var transaction = connection.BeginTransaction())
                                    {
                                        try
                                        {
                                            bool isSuccessfull = false;
                                            leaveRequestInDb.SupervisorId = supervisorId;
                                            leaveRequestInDb.HODId = supervisorId;
                                            if (model.StateStatus == StateStatus.Approved || model.StateStatus == StateStatus.Recommended)
                                            {
                                                leaveRequestInDb.StateStatus = StateStatus.Approved;

                                                leaveRequestInDb.CheckedBy = user.ActionUserId;
                                                leaveRequestInDb.CheckedDate = DateTime.Now;
                                                leaveRequestInDb.CheckRemarks = model.Remarks;

                                                leaveRequestInDb.ApprovedBy = user.ActionUserId;
                                                leaveRequestInDb.ApprovedDate = DateTime.Now;
                                                leaveRequestInDb.ApprovalRemarks = model.Remarks;

                                                var parameters = DapperParam.GetKeyValuePairsDynamic(leaveRequestInDb, true);
                                                parameters.Remove("EmployeeLeaveRequestId");
                                                var query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeeLeaveRequest", paramkeys: parameters.Select(x => x.Key).ToList());

                                                query += $"WHERE EmployeeLeaveRequestId=@EmployeeLeaveRequestId";
                                                parameters.Add("EmployeeLeaveRequestId", leaveRequestInDb.EmployeeLeaveRequestId);
                                                int rawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                                if (rawAffected > 0)
                                                {
                                                    query = $@"Update HR_EmployeeLeaveHistory
						                        SET [Status]='Approved',ApprovedBy=@UserId,ApprovedDate= GETDATE()
						                        Where 1=1 AND EmployeeLeaveRequestId=@EmployeeLeaveRequestId
						                        AND CompanyId=@CompanyId
						                        AND OrganizationId=@OrganizationId";

                                                    rawAffected = await connection.ExecuteAsync(query, new { UserId = user.ActionUserId, leaveRequestInDb.EmployeeLeaveRequestId, user.CompanyId, user.OrganizationId }, transaction);

                                                    if (rawAffected > 0)
                                                    {
                                                        isSuccessfull = true;
                                                    }
                                                }
                                            }
                                            else if (model.StateStatus == StateStatus.Rejected || model.StateStatus == StateStatus.Cancelled)
                                            {
                                                leaveRequestInDb.RejectedBy = user.ActionUserId;
                                                leaveRequestInDb.RejectedDate = DateTime.Now;
                                                leaveRequestInDb.RejectedRemarks = model.Remarks;
                                                leaveRequestInDb.StateStatus = StateStatus.Rejected;

                                                var parameters = DapperParam.GetKeyValuePairsDynamic(leaveRequestInDb, true);
                                                parameters.Remove("EmployeeLeaveRequestId");
                                                var query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeeLeaveRequest", paramkeys: parameters.Select(x => x.Key).ToList());

                                                query += $"WHERE EmployeeLeaveRequestId=@EmployeeLeaveRequestId";
                                                parameters.Add("EmployeeLeaveRequestId", leaveRequestInDb.EmployeeLeaveRequestId);
                                                int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);

                                                if (rawAffected > 0)
                                                {
                                                    // Update Leave Balance
                                                    query = $@"Update HR_EmployeeLeaveBalance
						                        Set LeaveApplied = LeaveApplied - @AppliedTotalDays
						                        Where EmployeeId =@EmployeeId AND LeaveTypeId =@LeaveTypeId AND @AppliedFromDate 
                                                Between LeavePeriodStart AND LeavePeriodEnd
						                        AND CompanyId=@CompanyId
						                        AND OrganizationId=@OrganizationId";

                                                    rawAffected = await connection.ExecuteAsync(query.Trim(), new
                                                    {
                                                        leaveRequestInDb.AppliedTotalDays,
                                                        leaveRequestInDb.EmployeeId,
                                                        leaveRequestInDb.LeaveTypeId,
                                                        leaveRequestInDb.AppliedFromDate,
                                                        user.CompanyId,
                                                        user.OrganizationId
                                                    }, transaction);

                                                    if (rawAffected > 0)
                                                    {
                                                        query = $@"Update HR_EmployeeLeaveHistory
						                        SET [Status]='Rejected',RejectedBy=@UserId,RejectedDate= GETDATE()
						                        Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId
						                        AND OrganizationId=@OrganizationId";

                                                        rawAffected = await connection.ExecuteAsync(query.Trim(), new
                                                        {
                                                            UserId = user.ActionUserId,
                                                            leaveRequestInDb.EmployeeLeaveRequestId,
                                                            leaveRequestInDb.EmployeeId,
                                                            user.CompanyId,
                                                            user.OrganizationId
                                                        }, transaction);

                                                        if (rawAffected > 0)
                                                        {
                                                            isSuccessfull = true;
                                                        }
                                                    }
                                                }
                                            }

                                            if (isSuccessfull == true)
                                            {
                                                transaction.Commit();
                                                executionStatus = new ExecutionStatus();
                                                executionStatus.Status = true;
                                                executionStatus.PresentValue = JsonReverseConverter.JsonData(leaveRequestInDb);
                                                executionStatus.Msg = "Data has been updated successfully";
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                executionStatus = ResponseMessage.Invalid("Data has been failed to update");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "LeaveRequestApprovalAsync", user);
                                            executionStatus = ResponseMessage.Invalid("Data has been failed to update");

                                        }
                                        finally
                                        {
                                            connection.Close();
                                        }
                                    }
                                }
                            }
                        }

                        // Supervisor & HOD both are not same
                        if ((supervisorId != 0 && supervisorId == thisUserId)
                            != (hodId != 0 && hodId == thisUserId)
                            )
                        {

                            using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                            {
                                connection.Open();
                                {
                                    using (var transaction = connection.BeginTransaction())
                                    {
                                        try
                                        {
                                            bool isSuccessfull = false;
                                            if (isSupervisor)
                                            {
                                                if (leaveRequestInDb.StateStatus == StateStatus.Pending)
                                                {

                                                    leaveRequestInDb.SupervisorId = supervisorId;

                                                    if (model.StateStatus == StateStatus.Approved || model.StateStatus == StateStatus.Recommended)
                                                    {
                                                        leaveRequestInDb.CheckedBy = user.ActionUserId;
                                                        leaveRequestInDb.CheckedDate = DateTime.Now;
                                                        leaveRequestInDb.CheckRemarks = model.Remarks;
                                                        leaveRequestInDb.StateStatus = StateStatus.Recommended;
                                                    }
                                                    else if (model.StateStatus == StateStatus.Rejected || model.StateStatus == StateStatus.Cancelled)
                                                    {
                                                        leaveRequestInDb.RejectedBy = user.ActionUserId;
                                                        leaveRequestInDb.RejectedDate = DateTime.Now;
                                                        leaveRequestInDb.RejectedRemarks = model.Remarks;
                                                        leaveRequestInDb.StateStatus = StateStatus.Rejected;
                                                    }

                                                    var parameters = DapperParam.GetKeyValuePairsDynamic(leaveRequestInDb, true);
                                                    parameters.Remove("EmployeeLeaveRequestId");
                                                    var query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeeLeaveRequest", paramkeys: parameters.Select(x => x.Key).ToList());

                                                    query += $"WHERE EmployeeLeaveRequestId=@EmployeeLeaveRequestId";
                                                    parameters.Add("EmployeeLeaveRequestId", leaveRequestInDb.EmployeeLeaveRequestId);
                                                    int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);

                                                    if (rawAffected > 0 && model.StateStatus == StateStatus.Rejected || model.StateStatus == StateStatus.Cancelled)
                                                    {
                                                        query = $@"Update HR_EmployeeLeaveBalance
						                                    Set LeaveApplied = LeaveApplied - @AppliedTotalDays
						                                    Where EmployeeId =@EmployeeId AND LeaveTypeId =@LeaveTypeId AND @AppliedFromDate 
                                                            Between LeavePeriodStart AND LeavePeriodEnd
						                                    AND CompanyId=@CompanyId
						                                    AND OrganizationId=@OrganizationId";

                                                        rawAffected = await connection.ExecuteAsync(query.Trim(), new
                                                        {
                                                            leaveRequestInDb.AppliedTotalDays,
                                                            leaveRequestInDb.EmployeeId,
                                                            leaveRequestInDb.LeaveTypeId,
                                                            leaveRequestInDb.AppliedFromDate,
                                                            user.CompanyId,
                                                            user.OrganizationId
                                                        }, transaction);

                                                        if (rawAffected > 0)
                                                        {
                                                            query = $@"Update HR_EmployeeLeaveHistory
						                                        SET [Status]='Rejected',RejectedBy=@UserId,RejectedDate= GETDATE()
						                                        Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId
						                                        AND OrganizationId=@OrganizationId";

                                                            rawAffected = await connection.ExecuteAsync(query.Trim(), new
                                                            {
                                                                UserId = user.ActionUserId,
                                                                leaveRequestInDb.EmployeeLeaveRequestId,
                                                                leaveRequestInDb.EmployeeId,
                                                                user.CompanyId,
                                                                user.OrganizationId
                                                            }, transaction);

                                                            if (rawAffected > 0)
                                                            {
                                                                isSuccessfull = true;
                                                            }
                                                        }
                                                    }
                                                    else if (rawAffected > 0 && model.StateStatus == StateStatus.Approved || model.StateStatus == StateStatus.Recommended)
                                                    {
                                                        query = $@"Update HR_EmployeeLeaveHistory
						                                SET [Status]='Recommended',CheckedBy=@UserId,CheckedDate= GETDATE(),CheckRemarks=@Remarks
						                                Where 1=1 AND EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND EmployeeId=@EmployeeId
						                                AND CompanyId=@CompanyId
						                                AND OrganizationId=@OrganizationId";

                                                        rawAffected = await connection.ExecuteAsync(query.Trim(), new
                                                        {
                                                            UserId = user.ActionUserId,
                                                            leaveRequestInDb.EmployeeLeaveRequestId,
                                                            leaveRequestInDb.EmployeeId,
                                                            user.CompanyId,
                                                            user.OrganizationId,
                                                            model.Remarks
                                                        }, transaction);

                                                        if (rawAffected > 0)
                                                        {
                                                            isSuccessfull = true;
                                                        }
                                                    }
                                                }
                                            }
                                            else if (isHOD)
                                            {
                                                if (leaveRequestInDb.StateStatus == StateStatus.Recommended)
                                                {
                                                    leaveRequestInDb.HODId = hodId;
                                                    leaveRequestInDb.StateStatus = model.StateStatus;
                                                    if (model.StateStatus == StateStatus.Approved || model.StateStatus == StateStatus.Recommended)
                                                    {
                                                        leaveRequestInDb.ApprovedBy = user.ActionUserId;
                                                        leaveRequestInDb.ApprovedDate = DateTime.Now;
                                                        leaveRequestInDb.ApprovalRemarks = model.Remarks;
                                                    }
                                                    else if (model.StateStatus == StateStatus.Rejected || model.StateStatus == StateStatus.Cancelled)
                                                    {
                                                        leaveRequestInDb.RejectedBy = user.ActionUserId;
                                                        leaveRequestInDb.RejectedDate = DateTime.Now;
                                                        leaveRequestInDb.RejectedRemarks = model.Remarks;
                                                    }

                                                    var parameters = DapperParam.GetKeyValuePairsDynamic(leaveRequestInDb, true);
                                                    parameters.Remove("EmployeeLeaveRequestId");
                                                    var query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeeLeaveRequest", paramkeys: parameters.Select(x => x.Key).ToList());

                                                    query += $"WHERE EmployeeLeaveRequestId=@EmployeeLeaveRequestId";
                                                    parameters.Add("EmployeeLeaveRequestId", leaveRequestInDb.EmployeeLeaveRequestId);
                                                    int rawAffected = await connection.ExecuteAsync(query.Trim(), parameters, transaction);

                                                    if (rawAffected > 0 && (model.StateStatus == StateStatus.Approved || model.StateStatus == StateStatus.Recommended))
                                                    {
                                                        query = $@"Update HR_EmployeeLeaveHistory
						                                SET [Status]='Approved',ApprovedBy=@UserId,ApprovedDate= GETDATE()
						                                Where 1=1 AND EmployeeLeaveRequestId=@EmployeeLeaveRequestId
						                                AND CompanyId=@CompanyId
						                                AND OrganizationId=@OrganizationId";

                                                        rawAffected = await connection.ExecuteAsync(query, new { UserId = user.ActionUserId, leaveRequestInDb.EmployeeLeaveRequestId, user.CompanyId, user.OrganizationId }, transaction);

                                                        if (rawAffected > 0)
                                                        {
                                                            isSuccessfull = true;
                                                        }
                                                    }
                                                    else if (rawAffected > 0 && (model.StateStatus == StateStatus.Rejected || model.StateStatus == StateStatus.Cancelled))
                                                    {
                                                        query = $@"Update HR_EmployeeLeaveBalance
						                                    Set LeaveApplied = LeaveApplied - @AppliedTotalDays
						                                    Where EmployeeId =@EmployeeId AND LeaveTypeId =@LeaveTypeId AND @AppliedFromDate 
                                                            Between LeavePeriodStart AND LeavePeriodEnd
						                                    AND CompanyId=@CompanyId
						                                    AND OrganizationId=@OrganizationId";

                                                        rawAffected = await connection.ExecuteAsync(query.Trim(), new
                                                        {
                                                            leaveRequestInDb.AppliedTotalDays,
                                                            leaveRequestInDb.EmployeeId,
                                                            leaveRequestInDb.LeaveTypeId,
                                                            leaveRequestInDb.AppliedFromDate,
                                                            user.CompanyId,
                                                            user.OrganizationId
                                                        }, transaction);

                                                        if (rawAffected > 0)
                                                        {
                                                            query = $@"Update HR_EmployeeLeaveHistory
						                                        SET [Status]='Rejected',RejectedBy=@UserId,RejectedDate= GETDATE()
						                                        Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId
						                                        AND OrganizationId=@OrganizationId";

                                                            rawAffected = await connection.ExecuteAsync(query.Trim(), new
                                                            {
                                                                UserId = user.ActionUserId,
                                                                leaveRequestInDb.EmployeeLeaveRequestId,
                                                                leaveRequestInDb.EmployeeId,
                                                                user.CompanyId,
                                                                user.OrganizationId
                                                            }, transaction);

                                                            if (rawAffected > 0)
                                                            {
                                                                isSuccessfull = true;
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            if (isSuccessfull == true)
                                            {
                                                transaction.Commit();
                                                executionStatus = new ExecutionStatus();
                                                executionStatus.Status = true;
                                                executionStatus.PresentValue = JsonReverseConverter.JsonData(leaveRequestInDb);
                                                executionStatus.Msg = "Data has been updated successfully";
                                            }
                                            else
                                            {
                                                transaction.Rollback();
                                                executionStatus = ResponseMessage.Invalid("Data has been failed to update");
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "LeaveRequestApprovalAsync", user);
                                            executionStatus = ResponseMessage.Invalid("Data has been failed to update");
                                        }
                                        finally
                                        {
                                            connection.Close();
                                        };
                                    }
                                }
                            }
                        }

                        // Supervisor & HOD both are not same
                        //if ((supervisorId != 0 && supervisorId == thisUserId)
                        //    != (hodId != 0 && hodId == thisUserId)
                        //    ) {

                        //}


                    }
                    else
                    {
                        executionStatus = ResponseMessage.Invalid("Leave request is not in Pending/Recommended");
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Invalid("Leave request is not found");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "LeaveRequestApprovalAsync", user);
            }
            return executionStatus;
        }
        public Task<ExecutionStatus> SaveAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<ExecutionStatus> SaveAsync(LeaveRequestDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    {
                        string previousValue = null;
                        string presentValue = null;
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                bool saveStatus = false;
                                if (model.LeaveRequest.EmployeeLeaveRequestId == 0)
                                {
                                    var leaveType = await _leaveTypeRepository.GetByIdAsync(model.LeaveRequest.LeaveTypeId, user);
                                    #region Leave Insert
                                    var leaveBalanceInfo = await _leaveBalanceRepository.GetEmployeeLeaveBalanceOfLeaveTypeAsync(model.LeaveRequest.EmployeeId, model.LeaveRequest.LeaveTypeId, model.LeaveRequest.AppliedFromDate.Value.ToString("yyyy-MM-dd"), model.LeaveRequest.AppliedToDate.Value.ToString("yyyy-MM-dd"), user);

                                    string leaveCode = await GetLeaveCodeAysnc(user);
                                    EmployeeLeaveRequest employeeLeaveRequest = new EmployeeLeaveRequest()
                                    {
                                        AddressDuringLeave = model.LeaveRequest.AddressDuringLeave,
                                        AppliedFromDate = model.LeaveRequest.AppliedFromDate,
                                        AppliedToDate = model.LeaveRequest.AppliedToDate,
                                        AppliedTotalDays = model.LeaveRequest.AppliedTotalDays,
                                        BranchId = 0,
                                        CompanyId = user.CompanyId,
                                        OrganizationId = user.OrganizationId,
                                        DayLeaveType = model.LeaveRequest.DayLeaveType,
                                        EmergencyPhoneNo = model.LeaveRequest.EmergencyPhoneNo,
                                        CreatedBy = user.ActionUserId,
                                        CreatedDate = DateTime.Now,
                                        EmployeeId = model.LeaveRequest.EmployeeId,
                                        IsApproved = false,
                                        EmployeeLeaveBalanceId = leaveBalanceInfo.EmployeeLeaveBalanceId,
                                        LeavePurpose = model.LeaveRequest.LeavePurpose,
                                        LeaveTypeId = model.LeaveRequest.LeaveTypeId,
                                        HalfDayType = model.LeaveRequest.HalfDayType,
                                        StateStatus = "Pending",
                                        EmployeeLeaveCode = leaveCode,
                                        LeaveTypeName = leaveType.Title,
                                        SupervisorId = 0,
                                        HODId = 0,
                                        FilePath = model.FilePath,
                                        FileName = model.FileName,
                                        FileSize = model.FileSize,
                                        FileType = model.FileType,
                                        ActualFileName = model.ActualFileName,
                                        EstimatedDeliveryDate = model.LeaveRequest.EstimatedDeliveryDate

                                    };
                                    var parameters = Utility.DappperParamsInKeyValuePairs(employeeLeaveRequest, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                    parameters.Remove("EmployeeLeaveRequestId");
                                    string query = Utility.GenerateInsertQuery(tableName: "HR_EmployeeLeaveRequest", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                    var insertedEmployeeLeave = await connection.QueryFirstOrDefaultAsync<EmployeeLeaveRequest>(query, parameters, transaction);
                                    #endregion
                                    if (insertedEmployeeLeave != null && insertedEmployeeLeave.EmployeeLeaveRequestId > 0)
                                    {
                                        saveStatus = true;
                                        model.LeaveRequest.EmployeeLeaveRequestId = insertedEmployeeLeave.EmployeeLeaveRequestId;
                                        #region Update Leave Balance
                                        query = $@"Update HR_EmployeeLeaveBalance
						                    Set LeaveApplied = LeaveApplied+@AppliedTotalDays, UpdatedDate = GETDATE(), UpdatedBy=@UserId
						                    Where EmployeeId=@EmployeeId AND LeaveTypeId=@LeaveTypeId 
						                    AND LeavePeriodStart <= @AppliedFromDate and LeavePeriodEnd >= @AppliedToDate  
						                    AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                        var updateParams = new
                                        {
                                            employeeLeaveRequest.AppliedTotalDays,
                                            UserId = user.ActionUserId,
                                            model.LeaveRequest.EmployeeId,
                                            model.LeaveRequest.LeaveTypeId,
                                            employeeLeaveRequest.AppliedFromDate,
                                            employeeLeaveRequest.AppliedToDate,
                                            user.CompanyId,
                                            user.OrganizationId
                                        };
                                        var updateBalanceCount = await connection.ExecuteAsync(query, updateParams, transaction);
                                        #endregion
                                        if (updateBalanceCount > 0)
                                        {
                                            saveStatus = true;
                                            if (saveStatus == true)
                                            {
                                                List<Dictionary<string, dynamic>> parameterList = new();
                                                parameters.Clear();
                                                foreach (var item in model.LeaveDays)
                                                {
                                                    if (item.Status == "Leave")
                                                    {
                                                        query = $@"INSERT INTO HR_EmployeeLeaveHistory([EmployeeLeaveRequestId],[EmployeeId],[DesignationId],[DepartmentId],[Count],[WorkShiftId],[LeaveTypeId],[LeaveSettingId],[LeaveDate],[Status],[CreatedBy],[CreatedDate],[BranchId],[CompanyId],[OrganizationId],[EmployeeLeaveBalanceId],[ReplacementDate])";
                                                        query += $@"VALUES(@EmployeeLeaveRequestId, @EmployeeId,@DesignationId,@DepartmentId, @Count,@WorkShiftId,@LeaveTypeId,0,@Date,'Pending',@UserId,GETDATE(),0,@CompanyId, @OrganizationId,@EmployeeLeaveBalanceId,@ReplacementDate)";

                                                        var leaveHistoryParam = new
                                                        {
                                                            insertedEmployeeLeave.EmployeeLeaveRequestId,
                                                            model.LeaveRequest.EmployeeId,
                                                            DesignationId = 0,
                                                            DepartmentId = 0,
                                                            Count = model.LeaveRequest.DayLeaveType == "Full-Day" ? 1 : .5,
                                                            item.WorkShiftId,
                                                            LeaveTypeId = leaveType.Id,
                                                            item.Date,
                                                            UserId = user.ActionUserId,
                                                            user.CompanyId,
                                                            user.OrganizationId,
                                                            leaveBalanceInfo.EmployeeLeaveBalanceId,
                                                            item.ReplacementDate
                                                        };
                                                        var leaveDaysCount = await connection.ExecuteAsync(query, leaveHistoryParam, transaction);

                                                        if (leaveDaysCount == 0)
                                                        {
                                                            saveStatus = false;
                                                        }
                                                        else
                                                        {
                                                            presentValue = JsonReverseConverter.JsonData(insertedEmployeeLeave);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            saveStatus = false;
                                        }
                                    }
                                    else
                                    {
                                        saveStatus = false;
                                    }
                                }
                                else
                                {
                                    var isExist = await IsLeavePendingAsync(model.LeaveRequest.EmployeeLeaveRequestId, user);
                                    saveStatus = true;
                                    if (isExist)
                                    {
                                        var leaveType = await _leaveTypeRepository.GetByIdAsync(model.LeaveRequest.LeaveTypeId, user);
                                        // Update Employee Leave Request 
                                        var employeeLeaveRequest = await GetByIdAsync(model.LeaveRequest.EmployeeLeaveRequestId, user);
                                        var previousEmployeeLeaveRequest = employeeLeaveRequest;
                                        previousValue = JsonReverseConverter.JsonData(previousEmployeeLeaveRequest);

                                        var leaveBalanceInfo = await _leaveBalanceRepository.GetEmployeeLeaveBalanceOfLeaveTypeAsync(model.LeaveRequest.EmployeeId, model.LeaveRequest.LeaveTypeId, model.LeaveRequest.AppliedFromDate.Value.ToString("yyyy-MM-dd"), model.LeaveRequest.AppliedToDate.Value.ToString("yyyy-MM-dd"), user);

                                        var employeeLeaveRequestInDb = await GetByIdAsync(model.LeaveRequest.EmployeeLeaveRequestId, user);

                                        employeeLeaveRequestInDb.AppliedFromDate = model.LeaveRequest.AppliedFromDate;
                                        employeeLeaveRequestInDb.AppliedToDate = model.LeaveRequest.AppliedToDate;
                                        employeeLeaveRequestInDb.DayLeaveType = model.LeaveRequest.DayLeaveType;
                                        employeeLeaveRequestInDb.HalfDayType = model.LeaveRequest.HalfDayType;
                                        employeeLeaveRequestInDb.AppliedTotalDays = model.LeaveRequest.AppliedTotalDays;
                                        employeeLeaveRequestInDb.LeavePurpose = model.LeaveRequest.LeavePurpose;
                                        employeeLeaveRequestInDb.EmergencyPhoneNo = model.LeaveRequest.EmergencyPhoneNo;
                                        employeeLeaveRequestInDb.AddressDuringLeave = model.LeaveRequest.AddressDuringLeave;
                                        employeeLeaveRequestInDb.UpdatedBy = user.ActionUserId;
                                        employeeLeaveRequestInDb.UpdatedDate = DateTime.Now;
                                        employeeLeaveRequestInDb.SupervisorId = 0;
                                        employeeLeaveRequestInDb.HODId = 0;

                                        if (model.File != null && model.File.Length > 0)
                                        {
                                            employeeLeaveRequestInDb.FilePath = model.FilePath;
                                            employeeLeaveRequestInDb.FileName = model.FileName;
                                            employeeLeaveRequestInDb.FileSize = model.FileSize;
                                            employeeLeaveRequestInDb.FileType = model.FileType;
                                            employeeLeaveRequestInDb.ActualFileName = model.ActualFileName;
                                        }
                                        employeeLeaveRequestInDb.EstimatedDeliveryDate = model.LeaveRequest.EstimatedDeliveryDate;


                                        var parameters = DapperParam.GetKeyValuePairsDynamic(employeeLeaveRequestInDb, addBaseProperty: true);
                                        parameters.Remove("EmployeeLeaveRequestId");
                                        string query = Utility.GenerateUpdateQuery(tableName: "HR_EmployeeLeaveRequest", paramkeys: parameters.Select(x => x.Key).ToList());
                                        parameters.Add("EmployeeLeaveRequestId", model.LeaveRequest.EmployeeLeaveRequestId);
                                        query += $@"Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId";

                                        var updateRawAffected = await connection.ExecuteAsync(query, parameters, transaction);

                                        if (updateRawAffected > 0)
                                        {
                                            // Delete History
                                            query = $@"DELETE FROM HR_EmployeeLeaveHistory Where EmployeeId=@EmployeeId AND EmployeeLeaveRequestId =@EmployeeLeaveRequestId AND LeaveTypeId=@LeaveTypeId ";
                                            var deleteParams = new { model.LeaveRequest.EmployeeId, model.LeaveRequest.EmployeeLeaveRequestId, model.LeaveRequest.LeaveTypeId };
                                            var deleteRawAffected = await connection.ExecuteAsync(query, deleteParams, transaction);

                                            if (deleteRawAffected > 0)
                                            {
                                                // Insert Leave History
                                                List<Dictionary<string, dynamic>> parameterList = new();
                                                foreach (var item in model.LeaveDays)
                                                {
                                                    if (item.Status == "Leave")
                                                    {
                                                        query = $@"INSERT INTO HR_EmployeeLeaveHistory([EmployeeLeaveRequestId],[EmployeeId],[DesignationId],[DepartmentId],[Count],[WorkShiftId],[LeaveTypeId],[LeaveSettingId],[LeaveDate],[Status],[CreatedBy],[CreatedDate],[BranchId],[CompanyId],[OrganizationId],[EmployeeLeaveBalanceId],[ReplacementDate])";
                                                        query += $@"VALUES(@EmployeeLeaveRequestId, @EmployeeId,@DesignationId,@DepartmentId, @Count,@WorkShiftId,@LeaveTypeId,0,@Date,'Pending',@UserId,GETDATE(),0,@CompanyId, @OrganizationId,@EmployeeLeaveBalanceId,@ReplacementDate)";

                                                        var leaveHistoryParam = new
                                                        {
                                                            model.LeaveRequest.EmployeeLeaveRequestId,
                                                            model.LeaveRequest.EmployeeId,
                                                            DesignationId = 0,
                                                            DepartmentId = 0,
                                                            Count = model.LeaveRequest.DayLeaveType == "Full-Day" ? 1 : .5,
                                                            item.WorkShiftId,
                                                            model.LeaveRequest.LeaveTypeId,
                                                            item.Date,
                                                            UserId = user.ActionUserId,
                                                            leaveBalanceInfo.EmployeeLeaveBalanceId,
                                                            user.CompanyId,
                                                            user.OrganizationId,
                                                            item.ReplacementDate
                                                        };
                                                        var leaveDaysCount = await connection.ExecuteAsync(query, leaveHistoryParam, transaction);

                                                        if (leaveDaysCount == 0)
                                                        {
                                                            saveStatus = false;
                                                        }
                                                    }
                                                }

                                                if (saveStatus == true)
                                                {
                                                    query = $@"Update HR_EmployeeLeaveBalance
					                                Set LeaveApplied =  ((LeaveApplied-@PreviousAppliedTotalDays)+@AppliedTotalDays),UpdatedBy=@UserId,UpdatedDate=GETDATE()
					                                Where EmployeeId=@EmployeeId AND LeaveTypeId=@LeaveTypeId 
					                                AND EmployeeLeaveBalanceId=@EmployeeLeaveBalanceId
					                                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                                    var updateLeaveBalanceParams = new
                                                    {
                                                        PreviousAppliedTotalDays = previousEmployeeLeaveRequest.AppliedTotalDays,
                                                        model.LeaveRequest.AppliedTotalDays,
                                                        UserId = user.ActionUserId,
                                                        model.LeaveRequest.EmployeeId,
                                                        model.LeaveRequest.LeaveTypeId,
                                                        previousEmployeeLeaveRequest.EmployeeLeaveBalanceId,
                                                        user.CompanyId,
                                                        user.OrganizationId
                                                    };

                                                    updateRawAffected = await connection.ExecuteAsync(query, updateLeaveBalanceParams, transaction);

                                                    if (updateRawAffected == 0)
                                                    {
                                                        saveStatus = false;
                                                    }
                                                    else
                                                    {
                                                        presentValue = JsonReverseConverter.JsonData(employeeLeaveRequestInDb);
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            saveStatus = false;
                                        }

                                    }
                                    else
                                    {
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.Status = false;
                                        executionStatus.Msg = "This Leave already has been Approved/Rejected/Cancelled";
                                    }
                                }
                                if (saveStatus == true)
                                {
                                    transaction.Commit();
                                    executionStatus = new ExecutionStatus();
                                    executionStatus.PresentValue = presentValue;
                                    executionStatus.PreviousValue = previousValue;
                                    executionStatus.Status = true;
                                    executionStatus.ItemId = model.LeaveRequest.EmployeeLeaveRequestId;
                                    executionStatus.Action = model.LeaveRequest.EmployeeLeaveRequestId > 0 ? "Update" : "Insert";
                                    executionStatus.Msg = "Data has been Saved Successfully";
                                }
                            }
                            catch (Exception ex)
                            {
                                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "SaveEmployeeLeaveRequest2Async", user);
                                executionStatus = ResponseMessage.Invalid("Something went wrong while saving leave");
                            }
                            finally { connection.Close(); }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "SaveEmployeeLeaveRequest2Async", user);
            }
            return executionStatus;
        }
        public Task<ExecutionStatus> ValidateAsync(EmployeeLeaveRequestDTO model, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<DBResponse<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(LeaveRequest_Filter filter, AppUser user)
        {
            DBResponse<EmployeeLeaveRequestViewModel> data = new DBResponse<EmployeeLeaveRequestViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var query = "sp_HR_SubordinatesEmployeeLeaveRequest";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, query, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeLeaveRequestViewModel>>(response.JSONData) ?? new List<EmployeeLeaveRequestViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetSubordinatesEmployeeLeaveRequestAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> DeleteEmployeeLeaveRequestAsync(DeleteEmployeeLeaveRequestDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var isPending = await IsLeavePendingAsync(model.EmployeeLeaveRequestId, user);
                if (isPending)
                {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    {
                        connection.Open();
                        {
                            bool isSuccessful = false;
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var leaveRequestInDb = await GetByIdAsync(model.EmployeeLeaveRequestId, user);

                                    #region Undo Leave Balance
                                    string query = $@"Update HR_EmployeeLeaveBalance
				                    Set LeaveApplied = LeaveApplied - @AppliedTotalDays
				                    Where EmployeeId =@EmployeeId AND LeaveTypeId =@LeaveTypeId AND LeaveTypeId=@LeaveTypeId 
					                AND LeavePeriodStart <= @AppliedFromDate and LeavePeriodEnd >= @AppliedToDate  			
				                    AND CompanyId=@CompanyId
				                    AND OrganizationId=@OrganizationId";

                                    var rawAffected = await connection.ExecuteAsync(query, new
                                    {
                                        leaveRequestInDb.AppliedTotalDays,
                                        leaveRequestInDb.EmployeeId,
                                        leaveRequestInDb.LeaveTypeId,
                                        leaveRequestInDb.AppliedFromDate,
                                        leaveRequestInDb.AppliedToDate,
                                        user.CompanyId,
                                        user.OrganizationId

                                    }, transaction);

                                    if (rawAffected > 0)
                                    {
                                        query = $@"Update HR_EmployeeLeaveHistory
				                        SET [Status]='Cancelled', CancelledBy=@UserId,CancelledDate=GETDATE()
				                        Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId 
                                        AND OrganizationId=@OrganizationId";

                                        rawAffected = await connection.ExecuteAsync(query, new
                                        {
                                            UserId = user.ActionUserId,
                                            leaveRequestInDb.EmployeeLeaveRequestId,
                                            leaveRequestInDb.EmployeeId,
                                            user.CompanyId,
                                            user.OrganizationId

                                        }, transaction);

                                        if (rawAffected > 0)
                                        {

                                            query = $@"Update HR_EmployeeLeaveRequest 
				                            SET StateStatus='Cancelled', CancelledBy=@UserId,CancelledDate=GETDATE()
				                            Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                UserId = user.ActionUserId,
                                                leaveRequestInDb.EmployeeLeaveRequestId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            if (rawAffected > 0)
                                            {
                                                isSuccessful = true;
                                                leaveRequestInDb.StateStatus = StateStatus.Cancelled;
                                                leaveRequestInDb.CancelledBy = user.ActionUserId;
                                                leaveRequestInDb.CancelledDate = DateTime.Now;
                                            }
                                        }
                                    }

                                    #endregion

                                    if (isSuccessful == true)
                                    {
                                        transaction.Commit();
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.PresentValue = JsonReverseConverter.JsonData(leaveRequestInDb);
                                        executionStatus.PreviousValue = null;
                                        executionStatus.Status = true;
                                        executionStatus.ItemId = model.EmployeeLeaveRequestId;
                                        executionStatus.Action = "Delete";
                                        executionStatus.Msg = "Data has been deleted Successfully";
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        executionStatus = ResponseMessage.Invalid("Something went wrong while saving data");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    executionStatus = ResponseMessage.Invalid("Something went wrong while saving data");
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "DeleteEmployeeLeaveRequestAsync", "LeaveRequestRepository", user);
                                }
                                finally { connection.Close(); }
                            }

                        }
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Invalid("Leave is not pending");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "DeleteEmployeeLeaveRequestAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ApprovedLeaveCancellationAsync(ApprovedLeaveCancellationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var leaveRequestInDb = await GetByIdAsync(model.EmployeeLeaveRequestId, user);
                if (leaveRequestInDb.StateStatus == StateStatus.Approved || leaveRequestInDb.StateStatus == StateStatus.Availed)
                {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    {
                        connection.Open();
                        {
                            bool isSuccessful = false;
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    #region Undo Leave Balance
                                    string query = $@"Update HR_EmployeeLeaveBalance
				                    Set LeaveApplied = LeaveApplied - @AppliedTotalDays
				                    Where EmployeeId =@EmployeeId AND LeaveTypeId =@LeaveTypeId AND LeaveTypeId=@LeaveTypeId 
					                AND LeavePeriodStart <= @AppliedFromDate and LeavePeriodEnd >= @AppliedToDate  			
				                    AND CompanyId=@CompanyId
				                    AND OrganizationId=@OrganizationId";

                                    var rawAffected = await connection.ExecuteAsync(query, new
                                    {
                                        leaveRequestInDb.AppliedTotalDays,
                                        leaveRequestInDb.EmployeeId,
                                        leaveRequestInDb.LeaveTypeId,
                                        leaveRequestInDb.AppliedFromDate,
                                        leaveRequestInDb.AppliedToDate,
                                        user.CompanyId,
                                        user.OrganizationId

                                    }, transaction);

                                    if (rawAffected > 0)
                                    {
                                        query = $@"Update HR_EmployeeLeaveHistory
				                        SET [Status]='Cancelled', CancelledBy=@UserId,CancelledDate=GETDATE()
				                        Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND EmployeeId=@EmployeeId AND CompanyId=@CompanyId 
                                        AND OrganizationId=@OrganizationId";

                                        rawAffected = await connection.ExecuteAsync(query, new
                                        {
                                            UserId = user.ActionUserId,
                                            leaveRequestInDb.EmployeeLeaveRequestId,
                                            leaveRequestInDb.EmployeeId,
                                            user.CompanyId,
                                            user.OrganizationId

                                        }, transaction);

                                        if (rawAffected > 0)
                                        {

                                            query = $@"Update HR_EmployeeLeaveRequest 
				                            SET StateStatus='Cancelled', CancelledBy=@UserId,CancelledDate=GETDATE(),IsApproved=0,CancelRemarks=@CancelRemarks
				                            Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                UserId = user.ActionUserId,
                                                leaveRequestInDb.EmployeeLeaveRequestId,
                                                user.CompanyId,
                                                user.OrganizationId,
                                                CancelRemarks = model.Remarks
                                            }, transaction);

                                            if (rawAffected > 0)
                                            {
                                                isSuccessful = true;
                                                leaveRequestInDb.StateStatus = "Cancel Approved Leave";
                                                leaveRequestInDb.CancelledBy = user.ActionUserId;
                                                leaveRequestInDb.CancelledDate = DateTime.Now;
                                                leaveRequestInDb.CancelRemarks = model.Remarks;
                                            }
                                        }
                                    }

                                    #endregion

                                    if (isSuccessful == true)
                                    {
                                        transaction.Commit();
                                        executionStatus = new ExecutionStatus();
                                        executionStatus.PresentValue = JsonReverseConverter.JsonData(leaveRequestInDb);
                                        executionStatus.PreviousValue = null;
                                        executionStatus.Status = true;
                                        executionStatus.ItemId = model.EmployeeLeaveRequestId;
                                        executionStatus.Action = "Delete";
                                        executionStatus.Msg = "Leave has been cancelled Successfully";
                                    }
                                    else
                                    {
                                        transaction.Rollback();
                                        executionStatus = ResponseMessage.Invalid("Something went wrong while saving data");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    executionStatus = ResponseMessage.Invalid("Something went wrong while saving data");
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "ApprovedLeaveCancellationAsync", user);
                                }
                                finally { connection.Close(); }
                            }

                        }
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Invalid("Leave is not pending");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "ApprovedLeaveCancellationAsync", user);
            }
            return executionStatus;
        }
        public async Task<EmployeeLeaveRequestViewModel> GetEmployeeLeaveRequestInfoById(long id, AppUser user)
        {
            EmployeeLeaveRequestViewModel info = new EmployeeLeaveRequestViewModel();
            try
            {
                var query = $@"Select lr.EmployeeLeaveRequestId, lr.EmployeeId, lr.DesignationId, lr.DepartmentId, lr.SectionId, lr.UnitId, lr.LeaveTypeId,lr.DayLeaveType, lr.AppliedFromDate, lr.AppliedToDate, lr.AppliedTotalDays, lr.LeavePurpose, lr.EmergencyPhoneNo, lr.AddressDuringLeave, lr.ApprovedFromDate, lr.ApprovedToDate, lr.TotalApprovalDays, lr.Remarks, lr.StateStatus, lr.IsApproved, lr.CreatedBy, lr.CreatedDate, lr.UpdatedBy, lr.UpdatedDate, lr.OrganizationId, lr.CompanyId, lr.ApprovedBy, lr.ApprovedDate, lr.ApprovalRemarks, lr.CancelledBy, lr.CancelledDate, lr.CancelRemarks, lr.BranchId, lr.CheckedBy, lr.CheckedDate, lr.CheckRemarks, lr.GradeId, lr.SubSectionId, lr.AttachmentFileNames, lr.AttachmentFileTypes, lr.AttachmentFiles, lr.HalfDayType, lr.ReliverDesignationId, lr.ReliverId, lr.EmployeeLeaveCode, lr.EmployeeLeaveBalanceId, lr.RejectedBy, lr.RejectedDate, lr.RejectedRemarks,lt.Title 'LeaveTypeName',emp.FullName 'EmployeeName', emp.GradeName ,emp.DesignationName, emp.DepartmentName,emp.SectionName, emp.SubSectionName,
              
			    CreaterInfo =(Case When lr.CreatedBy IS NOT NULL AND lr.CreatedBy !='' AND TRY_CONVERT(bigint,lr.CreatedBy) > 0 THEN 
			    (SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.CreatedBy))
			    END),
		    UpdaterInfo =(Case When lr.UpdatedBy IS NOT NULL AND lr.UpdatedBy !='' AND TRY_CONVERT(bigint,lr.UpdatedBy) > 0 THEN 
			    (SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.UpdatedBy))
			    END),
		    ApproverInfo=(Case When lr.ApprovedBy IS NOT NULL AND lr.ApprovedBy !='' AND TRY_CONVERT(bigint,lr.ApprovedBy) > 0 THEN 
			    (SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.ApprovedBy))
			    END),
		    RejecterInfo=(Case When lr.RejectedBy IS NOT NULL AND lr.RejectedBy !='' AND TRY_CONVERT(bigint,lr.RejectedBy) > 0 THEN 
			    (SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.RejectedBy))
			    END),
		    CheckerInfo=(Case When lr.CheckedBy IS NOT NULL AND lr.CheckedBy !='' AND TRY_CONVERT(bigint,lr.CheckedBy) > 0 THEN 
			    (SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.CheckedBy))
			    END),
		    CancellerInfo=(Case When lr.CancelledBy IS NOT NULL AND lr.CancelledBy !='' AND TRY_CONVERT(bigint,lr.CancelledBy) > 0 THEN 
			    (SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.CancelledBy))
			    END),
             SupervisorId=(CASE WHEN lr.StateStatus='Pending' THEN ISNULL(eh.SupervisorId,0) ELSE ISNULL(lr.SupervisorId,0) END),
             SupervisorName=(CASE WHEN lr.StateStatus='Pending' THEN 
                (SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId=ISNULL(eh.SupervisorId,0))
                ELSE NULL END),
            HODId=(CASE WHEN lr.StateStatus='Recommended' THEN ISNULL(eh.HeadOfDepartmentId,0) ELSE ISNULL(lr.HODId,0) END),
            HODName=(CASE WHEN lr.StateStatus='Recommended' THEN 
                (SELECT EmployeeName=(FullName+' ('+EmployeeCode+')') FROM HR_EmployeeInformation WHERE EmployeeId=ISNULL(eh.HeadOfDepartmentId,0))
                ELSE NULL END)

	        From [HR_EmployeeLeaveRequest] lr
	        Inner Join HR_LeaveTypes LT on lr.LeaveTypeId =LT.Id
	        Inner Join vw_HR_EmployeeList emp on lr.EmployeeId = emp.EmployeeId AND lr.CompanyId=emp.CompanyId AND lr.OrganizationId=emp.OrganizationId
	        Left Join (SELECT DISTINCT EmployeeId,SupervisorId,HeadOfDepartmentId FROM HR_EmployeeHierarchy Where IsActive=1) eh On eh.EmployeeId = emp.EmployeeId
	        Where 1=1
				        AND lr.EmployeeLeaveRequestId=@EmployeeLeaveRequestId
			
				        AND lr.CompanyId=@CompanyId
				        AND lr.OrganizationId =@OrganizationId";
                info = await _dapper.SqlQueryFirstAsync<EmployeeLeaveRequestViewModel>(user.Database, query, new { EmployeeLeaveRequestId = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "GetEmployeeLeaveRequestInfoById", user);
            }
            return info;
        }
        public async Task<EmployeeLeaveRequestInfoAndDetailViewModel> GetEmployeeLeaveRequestInfoAndDetailById(long id, AppUser user)
        {
            EmployeeLeaveRequestInfoAndDetailViewModel data = new EmployeeLeaveRequestInfoAndDetailViewModel();
            try
            {

                data.Info = await GetEmployeeLeaveRequestInfoById(id, user);
                data.Histories = await _leaveHistoryRepository.GetLeaveHistoryByIdAsync(id, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestRepository", "GetEmployeeLeaveRequestInfoAndDetailById", user);
            }
            return data;
        }
    }
}
