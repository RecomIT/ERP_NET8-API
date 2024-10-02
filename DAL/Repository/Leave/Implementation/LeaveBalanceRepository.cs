using System;
using System.Data;
using DAL.Logger.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using DAL.DapperObject.Interface;
using DAL.Repository.Leave.Interface;
using Shared.Leave.Domain.Balance;
using Shared.Leave.ViewModel.Balance;

namespace DAL.Repository.Leave.Implementation
{
    public class LeaveBalanceRepository : ILeaveBalanceRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public LeaveBalanceRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeLeaveBalance>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeLeaveBalance>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeLeaveBalance> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<LeaveBalanceViewModel>> GetLeaveBalanceAsync(long employeeId, AppUser user)
        {
            IEnumerable<LeaveBalanceViewModel> list = new List<LeaveBalanceViewModel>();
            try
            {
                var query = $@"Select emp.EmployeeId,emp.EmployeeCode,EmployeeName=emp.FullName,elb.LeaveTypeId,LeaveTypeName=lt.Title, 
                Allocated=elb.TotalLeave, 
                Applied=(elb.LeaveApplied),
                Balance=(elb.TotalLeave-ISNULL(elb.LeaveApplied,0)),
                Pending=(SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory Where [Status]='Pending' AND LeaveDate BETWEEN elb.LeavePeriodStart AND elb.LeavePeriodEnd),
                Availed=(SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory Where [Status]='Approved' AND LeaveDate BETWEEN elb.LeavePeriodStart AND elb.LeavePeriodEnd),
                Rejected=(SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory Where [Status]='Rejected' AND LeaveDate BETWEEN elb.LeavePeriodStart AND elb.LeavePeriodEnd),
                Cancelled=(SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory Where [Status]='Cancelled' AND LeaveDate BETWEEN elb.LeavePeriodStart AND elb.LeavePeriodEnd)
                From HR_EmployeeLeaveBalance elb
                INNER JOIN HR_LeaveTypes lt on elb.LeaveTypeId=lt.Id
                INNER JOIN HR_LeaveSetting ls on lt.Id =ls.LeaveTypeId 
                INNER JOIN HR_EmployeeInformation emp on elb.EmployeeId=emp.EmployeeId
                Where 1=1
                AND ( CAST(GETDATE() AS DATE) BETWEEN elb.LeavePeriodStart AND elb.LeavePeriodEnd)
                AND (elb.EmployeeId =@EmployeeId)
                AND (elb.CompanyId=@CompanyId)
                AND (elb.OrganizationId=@OrganizationId)";

                var parameters = new { EmployeeId = employeeId, user.CompanyId, user.OrganizationId };
                list = await _dapper.SqlQueryListAsync<LeaveBalanceViewModel>(user.Database, query, parameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveBalanceRepository", "GetEmployeeBalanceAsync", user);
            }
            return list;
        }

        public async Task<EmployeeLeaveBalance> GetEmployeeLeaveBalanceOfLeaveTypeAsync(long employeeId, long leaveTypeId, string appliedFromDate, string appliedToDate, AppUser user)
        {
            EmployeeLeaveBalance employeeLeaveBalance = new EmployeeLeaveBalance();
            try
            {
                var query = $@"Select * from HR_EmployeeLeaveBalance
						Where EmployeeId=@EmployeeId AND LeaveTypeId=@LeaveTypeId 
						AND LeavePeriodStart <= @AppliedFromDate and LeavePeriodEnd >= @AppliedToDate  
						AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                employeeLeaveBalance = await _dapper.SqlQueryFirstAsync<EmployeeLeaveBalance>(user.Database, query, new { EmployeeId = employeeId, LeaveTypeId = leaveTypeId, AppliedFromDate = appliedFromDate, AppliedToDate = appliedToDate, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveBalanceRepository", "GetEmployeeLeaveBalanceOfLeaveType", user);
            }
            return employeeLeaveBalance;
        }

        public async Task UpdateApprovedLeaveToAvailed(AppUser user)
        {
            try
            {
                var query = $@"Update HR_EmployeeLeaveHistory
                SET [Status]='Availed'
                Where [Status]='Approved' AND  CAST(GETDATE() AS DATE) > CAST(LeaveDate AS DATE)
                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                await _dapper.SqlExecuteNonQuery(user.Database, query, new { user.CompanyId, user.OrganizationId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveBalanceRepository", "UpdateApprovedLeaveToAvailed", user);
            }
        }
    }
}
