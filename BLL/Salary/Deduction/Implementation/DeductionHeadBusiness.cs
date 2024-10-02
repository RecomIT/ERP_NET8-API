using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using BLL.Salary.Deduction.Interface;
using Shared.Payroll.DTO.Deduction;
using Shared.Payroll.Filter.Deduction;
using Shared.Payroll.ViewModel.Deduction;

namespace BLL.Salary.Deduction.Implementation
{
    public class DeductionHeadBusiness : IDeductionHeadBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public DeductionHeadBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<ExecutionStatus> DeductionHeadValidatorAsync(DeductionHeadDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var list = await GetDeductionHeadsAsync(new DeductionHead_Filter(), user);

                var isDuplicateDeductionHeadName = list.FirstOrDefault(a => a.DeductionHeadId != model.DeductionHeadId &&
                a.DeductionHeadName == model.DeductionHeadName && a.CompanyId == user.CompanyId && a.OrganizationId == user.OrganizationId) != null;

                var isDuplicateDeductionHeadNameInBengali = list.FirstOrDefault(a => a.DeductionHeadId != model.DeductionHeadId &&
               a.DeductionHeadNameInBengali == model.DeductionHeadNameInBengali && a.CompanyId == user.CompanyId && a.OrganizationId == user.OrganizationId) != null;

                if (isDuplicateDeductionHeadName || isDuplicateDeductionHeadNameInBengali)
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Validation Error";
                    executionStatus.Errors = new Dictionary<string, string>();
                    if (isDuplicateDeductionHeadName)
                    {
                        executionStatus.Errors.Add("duplicateDeductionHeadName", "Duplicate Deduction Head");
                    }
                    if (isDuplicateDeductionHeadNameInBengali)
                    {
                        executionStatus.Errors.Add("duplicateDeductionHeadNameInBengali", "Duplicate Bengali Deduction Head");
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadBusiness", "DeductionHeadValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetDeductionHeadDropdownAsync(AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetDeductionHeadsAsync(new DeductionHead_Filter() { }, user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.DeductionHeadId,
                        Value = item.DeductionHeadId.ToString(),
                        Text = item.DeductionHeadName.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadBusiness", "GetDeductionHeadDropdownAsync", user);
            }
            return dropdowns;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetDeductionHeadExtensionAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = "sp_Payroll_DeductionHead";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", "Extension");
                data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadBusiness", "GetDeductionHeadExtensionAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<DeductionHeadViewModel>> GetDeductionHeadsAsync(DeductionHead_Filter filter, AppUser user)
        {
            IEnumerable<DeductionHeadViewModel> data = new List<DeductionHeadViewModel>();
            try
            {
                var sp_name = $@"Select * From Payroll_DeductionHead
			Where 1= 1
			AND (@DeductionHeadId IS NULL OR @DeductionHeadId = 0 OR DeductionHeadId = @DeductionHeadId)
			AND (@DeductionHeadName IS NULL OR @DeductionHeadName = '' OR DeductionHeadName LIKE @DeductionHeadName+'%')
			AND (CompanyId=@CompanyId)
			AND (OrganizationId=@OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<DeductionHeadViewModel>(user.Database, sp_name, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadBusiness", "GetDeductionHeadsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveDeductionHeadAsync(DeductionHeadDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_DeductionHead";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("Flag", model.DeductionHeadId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadBusiness", "SaveDeductionHeadAsync", user);
            }
            return executionStatus;
        }
    }
}
