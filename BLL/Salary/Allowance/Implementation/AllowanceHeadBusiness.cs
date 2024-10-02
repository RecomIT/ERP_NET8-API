using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using BLL.Salary.Allowance.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Allowance;
using Shared.Payroll.ViewModel.Allowance;
using Shared.Payroll.Filter.Allowance;

namespace BLL.Salary.Allowance.Implementation
{
    public class AllowanceHeadBusiness : IAllowanceHeadBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public AllowanceHeadBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<ExecutionStatus> SaveAllowanceHeadAsync(AllowanceHeadDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_AllowanceHead";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", model.AllowanceHeadId > 0 ? 'U' : 'C');
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadBusiness", "SaveAllowanceHeadAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<AllowanceHeadViewModel>> GetAllowanceHeadsAsync(AllowanceHead_Filter filter, AppUser user)
        {
            IEnumerable<AllowanceHeadViewModel> data = new List<AllowanceHeadViewModel>();
            try
            {
                var query = $@"Select * From Payroll_AllowanceHead
			Where 1= 1
			AND (@AllowanceHeadId IS NULL OR @AllowanceHeadId = 0 OR AllowanceHeadId = @AllowanceHeadId)
			AND (@AllowanceHeadName IS NULL OR @AllowanceHeadName = '' OR 
			AllowanceHeadName LIKE @AllowanceHeadName+'%')
			AND (CompanyId=@CompanyId)
			AND (OrganizationId=@OrganizationId)
			Order By AllowanceHeadName";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<AllowanceHeadViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadBusiness", "GetAllowanceHeadsAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetAllowanceHeadExtensionAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = "sp_Payroll_AllowanceHead";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", Data.Read);
                data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadBusiness", "GetAllowanceHeadExtensionAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> AllowanceHeadValidatorAsync(AllowanceHeadDTO allowanceHead, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var list = await GetAllowanceHeadsAsync(new AllowanceHead_Filter() { AllowanceHeadId = "0" }, user);

                var isDuplicateAllowanceHeadName = list.FirstOrDefault(a => a.AllowanceHeadId != allowanceHead.AllowanceHeadId &&
                a.AllowanceHeadName == allowanceHead.AllowanceHeadName && a.CompanyId == user.CompanyId && a.OrganizationId == user.OrganizationId) != null;

                var isDuplicateAllowanceHeadNameInBengali = list.FirstOrDefault(a => a.AllowanceHeadId != allowanceHead.AllowanceHeadId &&
               a.AllowanceHeadNameInBengali == allowanceHead.AllowanceHeadNameInBengali && a.CompanyId == user.CompanyId && a.OrganizationId == user.OrganizationId) != null;

                if (isDuplicateAllowanceHeadName || isDuplicateAllowanceHeadNameInBengali)
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Validation Error";
                    executionStatus.Errors = new Dictionary<string, string>();
                    if (isDuplicateAllowanceHeadName)
                    {
                        executionStatus.Errors.Add("duplicateAllowanceHeadName", "Duplicate Allowance Head");
                    }
                    if (isDuplicateAllowanceHeadNameInBengali)
                    {
                        executionStatus.Errors.Add("duplicateAllowanceHeadNameInBengali", "Duplicate Bengali Allowance Head");
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadBusiness", "AllowanceHeadValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetAllowanceHeadDropdownAsync(AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetAllowanceHeadsAsync(new AllowanceHead_Filter() { }, user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.AllowanceHeadId,
                        Value = item.AllowanceHeadId.ToString(),
                        Text = item.AllowanceHeadName.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadBusiness", "GetAllowanceHeadDropdownAsync", user);
            }
            return dropdowns;
        }
    }
}
