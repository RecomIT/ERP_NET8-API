using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using BLL.Employee.Interface.Miscellaneous;
using DAL.DapperObject.Interface;
using Shared.Employee.ViewModel.Education;
using Shared.Employee.Filter.Education;
using Shared.Employee.DTO.Eudcation;

namespace BLL.Employee.Implementation.Miscellaneous
{
    public class LevelOfEducationBusiness : ILevelOfEducationBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public LevelOfEducationBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public async Task<IEnumerable<LevelOfEducationViewModel>> GetLevelOfEducationsAsync(LevelOfEducation_Fiter filter, AppUser user)
        {
            IEnumerable<LevelOfEducationViewModel> list = new List<LevelOfEducationViewModel>();
            try
            {
                var query = $@"Select * From HR_LevelOfEducation
				Where 1=1
				and (@Name IS NULL OR @Name = '' OR [Name] LIKE '%' +@Name + '%')
				and (@LevelOfEducationId IS NULL OR @LevelOfEducationId = 0 OR [LevelOfEducationId]=@LevelOfEducationId)
				and (OrganizationId = @OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("LevelOfEducationId", filter.LevelOfEducationId);
                parameters.Add("Name", filter.Name);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<LevelOfEducationViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LevelOfEducationBusiness", "GetLevelOfEducations", user);
            }
            return list;
        }
        public async Task<IEnumerable<Dropdown>> GetLevelOfEducationsDropdownAsync(AppUser user)
        {
            IEnumerable<Dropdown> list = new List<Dropdown>();
            try
            {
                var data = await GetLevelOfEducationsAsync(new LevelOfEducation_Fiter()
                {
                    LevelOfEducationId = "0",
                    Name = ""
                }, user);

                foreach (var item in data)
                {
                    Dropdown dropdown = new Dropdown()
                    {
                        Id = item.LevelOfEducationId,
                        Value = item.LevelOfEducationId.ToString(),
                        Text = item.Name,
                    };
                    list.AsList().Add(dropdown);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LevelOfEducationBusiness", "GetLevelOfEducationsDropdownAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveLevelOfEducationAsync(LevelOfEducationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_LevelOfEducation";
                var parameters = new DynamicParameters();
                parameters.Add("LevelOfEducationId", model.LevelOfEducationId);
                parameters.Add("Name", model.Name);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", model.LevelOfEducationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "LevelOfEducationBusiness", "SaveLevelOfEducationAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateLevelOfEducationAsync(LevelOfEducationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_LevelOfEducation";
                var parameters = new DynamicParameters();
                parameters.Add("LevelOfEducationId", model.LevelOfEducationId);
                parameters.Add("Name", model.Name);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", model.LevelOfEducationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LevelOfEducationBusiness", "ValidateLevelOfEducationAsync", user);
            }
            return executionStatus;
        }
    }
}
