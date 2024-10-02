using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.ViewModel.Organizational;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Organizational
{
    public class SubSectionBusiness : ISubSectionBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public SubSectionBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<SubSectionViewModel>> GetSubSectionsAsync(SubSection_Filter filter, AppUser user)
        {
            IEnumerable<SubSectionViewModel> list = new List<SubSectionViewModel>();
            try
            {
                var sp_name = "sp_HR_SubSections_List";
                var parameters = Utility.DappperParams(filter, user);
                list = await _dapper.SqlQueryListAsync<SubSectionViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubSectionBusiness", "GetSubSectionsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveSubSectionAsync(SubSectionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_SubSections_Insert_Update_Delete";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.SubSectionId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubSectionBusiness", "SaveSubSectionAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateSubSectionAsync(SubSectionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = string.Format(@"sp_HR_SubSections_Insert_Update_Delete");
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidParameters);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubSectionBusiness", "ValidateSubSectionAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetSubSectionDropdownAsync(SubSection_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetSubSectionsAsync(filter, user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.SubSectionId,
                        Value = item.SubSectionId.ToString(),
                        Text = item.SubSectionName.ToString() + (Utility.IsNullEmptyOrWhiteSpace(item.SectionName) == false ? " [" + item.SectionName + "]" : ""),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubSectionBusiness", "GetSubSectionDropdownAsync", user);
            }
            return dropdowns;
        }
    }
}
