using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Deduction;
using Shared.OtherModels.DataService;
using BLL.Salary.Deduction.Interface;
using Shared.Payroll.Filter.Deduction;
using Shared.Payroll.ViewModel.Deduction;

namespace BLL.Salary.Deduction.Implementation
{
    public class DeductionNameBusiness : IDeductionNameBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public DeductionNameBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> SaveDeductionNameAsync(DeductionNameDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {

                var sp_name = "sp_Payroll_DeductionName";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", model.DeductionNameId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameBusiness", "SaveDeductionNameAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<DeductionNameViewModel>> GetDeductionNamesAsync(DeductionName_Filter filter, AppUser user)
        {
            IEnumerable<DeductionNameViewModel> data = new List<DeductionNameViewModel>();
            try
            {

                var query = $@"Select dn.*,dh.DeductionHeadName From Payroll_DeductionName dn
			Inner Join Payroll_DeductionHead dh on dn.DeductionHeadId = dh.DeductionHeadId
			Where 1= 1
			AND (@DeductionNameId IS NULL OR @DeductionNameId = 0 OR dn.DeductionNameId = @DeductionNameId)
			AND (@DeductionName IS NULL OR @DeductionName = '' OR dn.[Name] LIKE @DeductionName+'%')
			AND (dn.CompanyId=@CompanyId)
			AND (dn.OrganizationId=@OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<DeductionNameViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameBusiness", "GetDeductionNamesAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetDeductionNameExtensionAsync(string deductionType, long deductionHeadId, AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = "sp_Payroll_DeductionName";

                var parameters = new DynamicParameters();
                parameters.Add("DeductionHeadId", deductionHeadId);
                parameters.Add("DeductionType", deductionType);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", "Extension");

                data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameBusiness", "GetDeductionNamesAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> DeductionNameValidatorAsync(DeductionNameDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var list = await GetDeductionNamesAsync(new DeductionName_Filter(), user);
                var isDuplicateDeductionName = list.FirstOrDefault(a => a.DeductionNameId != model.DeductionNameId &&
                a.Name == model.Name && a.CompanyId == user.CompanyId && a.OrganizationId == user.OrganizationId) != null;
                var isDuplicateDeductionNameInBengali = list.FirstOrDefault(a => a.DeductionNameId != model.DeductionNameId &&
               a.DeductionNameInBengali == model.DeductionNameInBengali && a.CompanyId == user.CompanyId && a.OrganizationId == user.OrganizationId) != null;
                var isFlagExist = list.FirstOrDefault(a => a.DeductionNameId != model.DeductionNameId
               && a.Flag == model.Flag && a.CompanyId == user.CompanyId && a.OrganizationId == user.OrganizationId) != null;

                if (isDuplicateDeductionName || isDuplicateDeductionNameInBengali)
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Validation Error";
                    executionStatus.Errors = new Dictionary<string, string>();
                    if (isDuplicateDeductionName)
                    {
                        executionStatus.Errors.Add("duplicateDeductionName", "Duplicate Deduction Name");
                    }
                    if (isDuplicateDeductionNameInBengali)
                    {
                        executionStatus.Errors.Add("duplicateDeductionNameInBengali", "Duplicate Bengali Deduction Name");
                    }
                    if (isFlagExist)
                    {
                        executionStatus.Errors.Add("duplicateFlag", "Flag exist in another deduction");
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameBusiness", "DeductionNameValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetDeductionNameDropdownAsync(AppUser user)
        {
            List<Dropdown> list = new List<Dropdown>();
            try
            {
                var data = await GetDeductionNamesAsync(new DeductionName_Filter(), user);
                if (data != null && data.AsList().Count > 0)
                {
                    foreach (var item in data)
                    {
                        Dropdown dropdown = new Dropdown()
                        {
                            Id = item.DeductionNameId,
                            Text = item.Name + " [" + item.Name + "-" + item.DeductionType + "]",
                            Value = item.DeductionNameId.ToString()
                        };
                        list.Add(dropdown);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameBusiness", "GetDeductionNameDropdownAsync", user);
            }
            return list;
        }
    }
}
