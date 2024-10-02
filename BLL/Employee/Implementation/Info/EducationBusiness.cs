using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Education;
using BLL.Employee.Interface.Education;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Info
{
    public class EducationBusiness : IEducationBusiness
    {
        private ISysLogger _sysLogger;
        private IDapperData _dapper;

        public EducationBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public async Task<ExecutionStatus> DeleteEmployeeEducationAsync(DeleteEmployeeEducationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeEducation";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeEducationBusiness", "DeleteEmployeeEducationAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<EmployeeEducationVM>> GetEmployeeEducationsAsync(Education_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeEducationVM> list = new List<EmployeeEducationVM>();
            try
            {
                var sp_name = "sp_HR_EmployeeEducation";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("Flag", Data.Read);
                list = await _dapper.SqlQueryListAsync<EmployeeEducationVM>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeEducationBusiness", "SaveEmployeeEducationAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveEmployeeEducationAsync(EmployeeEducationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeEducation";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", model.EmployeeEducationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeEducationBusiness", "SaveEmployeeEducationAsync", user);
            }
            return executionStatus;
        }
    }
}
