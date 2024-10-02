using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.ViewModel.Organizational;

namespace BLL.Employee.Implementation.Organizational
{
    public class DepartmentBusiness : IDepartmentBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public DepartmentBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<DepartmentViewModel>> GetDepartmentsAsync(Department_Filter filter, AppUser user)
        {
            IEnumerable<DepartmentViewModel> list = new List<DepartmentViewModel>();
            try
            {
                var sp_name = "sp_HR_Departments_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<DepartmentViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentBusiness", "GetDepartmentsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveDepartmentAsync(DepartmentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Departments_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.DepartmentId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentBusiness", "SaveDepartmentAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateDepartmentAsync(DepartmentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Departments_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentBusiness", "ValidateDepartmentAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetDepartmentDropdownAsync(AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetDepartmentsAsync(new Department_Filter(), user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.DepartmentId,
                        Value = item.DepartmentId.ToString(),
                        Text = item.DepartmentName.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentBusiness", "GetDepartmentDropdownAsync", user);
            }
            return dropdowns;
        }
        public async Task<IEnumerable<Dropdown>> GetDepartmentItemsAsync(List<string> items, AppUser user)
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                string query = $@"SELECT [Value]=DepartmentId,[Text]=DepartmentName FROM HR_Departments
                Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                AND DepartmentName IN(Select[Value] from fn_split_string_to_column(@DepartmentName, ','))";

                dropdowns = await _dapper.SqlQueryListAsync<Dropdown>(user.Database, query, new { user.CompanyId, user.OrganizationId, DepartmentName = string.Join(',', items) }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeBusiness", "GetGradeItemsAsync", user);
            }
            return dropdowns;
        }
    }
}
