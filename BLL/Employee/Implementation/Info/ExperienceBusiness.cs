using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using BLL.Employee.Interface.Info;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Info
{
    public class ExperienceBusiness : IExperienceBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public ExperienceBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<ExecutionStatus> DeleteEmployeeExperienceAsync(DeleteEmployeeExperienceDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeExperience";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeExperienceId", model.EmployeeExperienceId);
                parameters.Add("EmployeeId", model.EmployeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceBusiness", "DeleteEmployeeExperienceAsync", user);
            }
            return executionStatus;
        }
        public Task<ExecutionStatus> EmployeeExperienceValidatorAsync(EmployeeExperienceDTO model, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<EmployeeExperienceVM>> GetEmployeeExperiencesAsync(EmployeeExperience_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeExperienceVM> list = new List<EmployeeExperienceVM>();
            try
            {
                var query = $@"Select Expe.*,EMP.EmployeeCode From HR_EmployeeExperience Expe
				Inner Join HR_EmployeeInformation emp on expe.EmployeeId = emp.EmployeeId
				Where 1=1
				And (@EmployeeExperienceId IS NULL OR @EmployeeExperienceId = 0 OR Expe.EmployeeExperienceId = @EmployeeExperienceId)
				And (@EmployeeId IS NULL OR @EmployeeId = 0 OR Expe.EmployeeId = @EmployeeId)
                AND (@EmployeeCode IS NULL OR @EmployeeCode='' OR EMP.EmployeeCode = @EmployeeCode) 
				And (Expe.CompanyId = @CompanyId)
				And (Expe.OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<EmployeeExperienceVM>(user.Database, query, parameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceBusiness", "GetEmployeeExperiencesAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveEmployeeExperienceAsync(EmployeeExperienceDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeExperience";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", model.EmployeeExperienceId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeExperienceBusiness", "SaveEmployeeExperienceAsync", user);
            }
            return executionStatus;
        }
    }
}
