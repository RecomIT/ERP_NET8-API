using BLL.Base.Interface;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.ViewModel.Organizational;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;

namespace BLL.Employee.Implementation.Organizational
{
    public class GradeBusiness : IGradeBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public GradeBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<GradeViewModel>> GetGradesAsync(Grade_Filter filter, AppUser user)
        {
            IEnumerable<GradeViewModel> list = new List<GradeViewModel>();
            try
            {
                var sp_name = "sp_HR_Grade_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<GradeViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeBusiness", "GetGradesAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveGradeAsync(GradeDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Grade_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.GradeId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeBusiness", "SaveGradeAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateGradeAsync(GradeDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Grade_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeBusiness", "ValidateGradeAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetGradeDropdownAsync(AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetGradesAsync(new Grade_Filter(), user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.GradeId,
                        Value = item.GradeId.ToString(),
                        Text = item.GradeName.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeBusiness", "GetGradeDropdownAsync", user);
            }
            return dropdowns;
        }
        public async Task<IEnumerable<Dropdown>> GetGradeItemsAsync(List<string> items, AppUser user)
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>();
            try
            {

                string query = $@"SELECT [Value]=GradeId,[Text]=GradeName FROM HR_Grades
                Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                AND GradeName IN(Select[Value] from fn_split_string_to_column(@GradeName, ','))";
                dropdowns = await _dapper.SqlQueryListAsync<Dropdown>(user.Database, query, new { user.CompanyId, user.OrganizationId, GradeName = string.Join(',', items) }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeBusiness", "GetGradeItemsAsync", user);
            }
            return dropdowns;
        }
    }
}
