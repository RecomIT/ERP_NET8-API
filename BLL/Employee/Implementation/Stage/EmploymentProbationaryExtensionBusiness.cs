using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using Shared.Employee.Filter.Stage;
using Shared.Employee.ViewModel.Stage;
using BLL.Employee.Interface.Stage;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Stage
{
    public class EmploymentProbationaryExtensionBusiness : IEmploymentProbationaryExtensionBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public EmploymentProbationaryExtensionBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<DBResponse<EmploymentProbationaryExtensionViewModel>> GetEmploymentProbationaryExtensionAsync(ProbationaryExtension_Filter model, AppUser user)
        {
            DBResponse response = new DBResponse();
            DBResponse<EmploymentProbationaryExtensionViewModel> data = new DBResponse<EmploymentProbationaryExtensionViewModel>();
            try
            {
                var sp_name = "sp_HR_EmploymentProbationaryExtension_List";
                var parameters = DapperParam.AddParams(model, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = JsonReverseConverter.JsonToObject<IEnumerable<EmploymentProbationaryExtensionViewModel>>(response.JSONData) ?? new List<EmploymentProbationaryExtensionViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentProbationaryExtensionBusiness", "GetEmploymentProbationaryExtensionAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveEmploymentProbationaryExtensionAsync(EmploymentProbationaryExtensionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmploymentProbationaryExtension_Insert_Update";
                var parameters = Utility.DappperParams(model, user);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentProbationaryExtensionBusiness", "SaveEmploymentProbationaryExtensionAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveEmploymentProbationaryExtensionStatusAsync(EmploymentProbationaryStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_EmploymentProbationaryStatus";
                var parameters = Utility.DappperParams(model, user);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentProbationaryExtensionBusiness", "SaveEmploymentProbationaryExtensionStatusAsync", user);
            }
            return executionStatus;
        }
    }
}
