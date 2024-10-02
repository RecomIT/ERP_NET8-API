using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Termination;
using Shared.Employee.Filter.Termination;
using Shared.Employee.ViewModel.Termination;
using BLL.Employee.Interface.Termination;
using Shared.Employee.Domain.Termination;
using Shared.Separation.Filter;
using Shared.Control_Panel.Domain;

namespace BLL.Employee.Implementation.Termination
{
    public class DiscontinuedEmployeeBusiness : IDiscontinuedEmployeeBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public DiscontinuedEmployeeBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<DiscontinuedEmployeeViewModel>> GetDiscontinuedEmployeesAsync(Termination_Filter filter, AppUser user)
        {
            IEnumerable<DiscontinuedEmployeeViewModel> list = new List<DiscontinuedEmployeeViewModel>();
            try
            {
                var sp_name = $@"SELECT DE.*,EMP.EmployeeCode,EmployeeName=EMP.FullName FROM HR_DiscontinuedEmployee DE
	INNER JOIN HR_EmployeeInformation EMP ON DE.EmployeeId =EMP.EmployeeId AND DE.CompanyId = EMP.CompanyId
	AND DE.OrganizationId = EMP.OrganizationId
	Where 1=1
	AND (@DiscontinuedId IS NULL OR @DiscontinuedId =0 OR DE.DiscontinuedId=@DiscontinuedId)
	AND (@EmployeeId IS NULL OR @EmployeeId =0 OR DE.EmployeeId=@EmployeeId)
	AND (@EmployeeCode IS NULL OR @EmployeeCode ='' OR EMP.EmployeeCode=@EmployeeCode)
	AND (@StateStatus  IS NULL OR @StateStatus  ='' OR DE.[StateStatus] =@StateStatus)
    AND (@Releasetype IS NULL OR @Releasetype='' OR DE.Releasetype =@Releasetype)
	AND (DE.CompanyId =@CompanyId)
	AND (DE.OrganizationId =@OrganizationId)
	Order By CAST(DE.LastWorkingDate AS DATE) desc";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<DiscontinuedEmployeeViewModel>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeBusiness", "GetDiscontinuedEmployeesAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> DeleteDiscontinuedEmployeeAsync(Termination_Filter filter, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_DiscontinuedEmployee_Insert_Update_Delete";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", filter.EmployeeId);
                parameters.Add("DiscontinuedId", filter.DiscontinuedId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeBusiness", "RollDiscontinuedEmployeeAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveDiscontinuedEmployeeAsync(DiscontinuedEmployeeDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_DiscontinuedEmployee_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.DiscontinuedId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeBusiness", "SaveDiscontinuedEmployeeAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateDiscontinuedEmployeeAsync(DiscontinuedEmployeeDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_DiscontinuedEmployee_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeBusiness", "SaveDiscontinuedEmployeeAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ApprovalDiscontinuedEmployeeAsync(DiscontinuedEmployeeApprovalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_DiscontinuedEmployee_Approval";
                var parameters = DapperParam.AddParams(model, user);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "DiscontinuedEmployeeBusiness", "ApprovalDiscontinuedEmployeeAsync", user);
            }

            return executionStatus;
        }
        public async Task<DiscontinuedEmployee> GetDiscontinuedEmployeeById(long employeeId, AppUser user)
        {
            DiscontinuedEmployee discontinuedEmployee = null;
            try
            {
                var query = @"SELECT * FROM HR_DiscontinuedEmployee Where EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND StateStatus IN ('Pending','Approved')";
                discontinuedEmployee = await _dapper.SqlQueryFirstAsync<DiscontinuedEmployee>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId,
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return discontinuedEmployee;
        }
    }
}
