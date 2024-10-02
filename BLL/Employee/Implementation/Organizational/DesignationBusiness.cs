using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Organizational;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.ViewModel.Organizational;

namespace BLL.Employee.Implementation.Organizational
{
    public class DesignationBusiness : IDesignationBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public DesignationBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<DesignationViewModel>> GetDesignationsAsync(Designation_Filter filter, AppUser user)
        {
            IEnumerable<DesignationViewModel> list = new List<DesignationViewModel>();
            try
            {
                var sp_name = "sp_HR_Designations_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<DesignationViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationBusiness", "GetDesignationAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveDesignationAsync(DesignationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Designations_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.DesignationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationBusiness", "SaveDesignationAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateDesignationAsync(DesignationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Designations_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationBusiness", "ValidateDesignationAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetDesignationDropdownAsync(AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetDesignationsAsync(new Designation_Filter(), user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.DesignationId,
                        Value = item.DesignationId.ToString(),
                        Text = item.DesignationName.ToString() + (Utility.IsNullEmptyOrWhiteSpace(item.GradeName) == false ? " [" + item.GradeName + "]" : ""),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationBusiness", "GetDesignationDropdownAsync", user);
            }
            return dropdowns;
        }

        public async Task<IEnumerable<Dropdown>> GetDesignationItemsAsync(List<string> items, AppUser user)
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                string query = $@"SELECT [Value]=DesignationId,[Text]=DesignationName FROM HR_Designations
                Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                AND DesignationName IN(Select[Value] from fn_split_string_to_column(@DesignationName, ','))";
                dropdowns = await _dapper.SqlQueryListAsync<Dropdown>(user.Database, query, new { user.CompanyId, user.OrganizationId, DesignationName = string.Join(',', items) }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeBusiness", "GetGradeItemsAsync", user);
            }
            return dropdowns;
        }
    }
}
