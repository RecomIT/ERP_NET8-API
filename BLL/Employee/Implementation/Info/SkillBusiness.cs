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
    public class SkillBusiness : ISkillBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public SkillBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<ExecutionStatus> DeleteEmployeeSkillAsync(DeleteEmployeeSkillDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeSkill";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSkillBusiness", "DeleteEmployeeSkillAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> EmployeeSkillValidatorAsync(EmployeeSkillDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeSkill";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSkillBusiness", "EmployeeSkillValidatorAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<EmployeeSkillVM>> GetEmployeeSkillsAsync(Skill_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeSkillVM> list = new List<EmployeeSkillVM>();
            try
            {
                var query = $@"Select empSkill.*,Emp.EmployeeCode From HR_EmployeeSkill empSkill
				Inner Join HR_EmployeeInformation emp on empSkill.EmployeeId = emp.EmployeeId
				Where 1=1
				and (@EmployeeId IS NULL OR @EmployeeId =0 OR empSkill.EmployeeId =@EmployeeId)
				and (@EmployeeSkillId IS NULL OR @EmployeeSkillId= 0 OR empSkill.EmployeeSkillId = @EmployeeSkillId)
				and (empSkill.CompanyId = @CompanyId)
				and (empSkill.OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<EmployeeSkillVM>(user.Database, query, parameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSkillBusiness", "GetEmployeeSkillsAsync", user);
            }
            return list;
        }

        public async Task<ExecutionStatus> SaveEmployeeSkillAsync(EmployeeSkillDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeSkill";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", model.EmployeeSkillId > 0 ? Data.Update : Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSkillBusiness", "SaveEmployeeSkillAsync", user);
            }
            return executionStatus;
        }
    }
}
