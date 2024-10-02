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
    public class SectionBusiness : ISectionBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public SectionBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<SectionViewModel>> GetSectionsAsync(Section_Filter filter, AppUser user)
        {
            IEnumerable<SectionViewModel> list = new List<SectionViewModel>();
            try
            {
                var sp_name = "sp_HR_Sections_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<SectionViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SectionBusiness", "GetSectionsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveSectionAsync(SectionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Sections_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.SectionId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SectionBusiness", "SaveSectionAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateSectionAsync(SectionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Sections_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SectionBusiness", "ValidateSectionAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetSectionDropdownAsync(Section_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetSectionsAsync(filter, user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.SectionId,
                        Value = item.SectionId.ToString(),
                        Text = item.SectionName.ToString() + (Utility.IsNullEmptyOrWhiteSpace(item.DepartmentName) == false ? " [" + item.DepartmentName + "]" : ""),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SectionBusiness", "GetSectionDropdownAsync", user);
            }
            return dropdowns;
        }
    }
}
