using BLL.Base.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Dapper;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.ViewModel.Tax;
using Shared.Services;
using System.Data;

namespace BLL.Tax.Implementation
{
    public class TaxZoneBusiness : ITaxZoneBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public TaxZoneBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public Task<EmployeeTaxZoneViewModel> GetEmployeeTaxZonesAsync(EmployeeTaxZone_Filter query, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<ExecutionStatus> SaveEmployeeTaxZoneAsync(List<EmployeeTaxZoneViewModel> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var jsonData = Utility.JsonData(model);
                var sp_name = "sp_Payroll_EmployeeTaxZone_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("JSONData", jsonData);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "SaveEmployeeTaxZoneAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<EmployeeTaxZoneViewModel>> GetEmployeeTaxZoneListAsync(EmployeeTaxZone_Filter query, AppUser user)
        {
            IEnumerable<EmployeeTaxZoneViewModel> data = new List<EmployeeTaxZoneViewModel>();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxZone_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeTaxZoneId", query.EmployeeTaxZoneId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("ExecutionFlag", Data.Read);

                data = await _dapper.SqlQueryListAsync<EmployeeTaxZoneViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "GetEmployeeTaxZoneListAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> UpdateTaxZoneAsync(EmployeeTaxZoneViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxZone_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", model.EmployeeId);
                parameters.Add("EmployeeTaxZoneId", model.EmployeeTaxZoneId);
                parameters.Add("TaxZone", model.TaxZone);
                parameters.Add("MinimumTaxAmount", model.MinimumTaxAmount);
                parameters.Add("EffectiveDate", model.EffectiveDate);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "UpdateTaxZoneAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> TaxZoneValidatorAsync(List<EmployeeTaxZoneViewModel> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                string duplicate = string.Empty;
                foreach (var item in model)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("EmployeeId", item.EmployeeId);
                    parameters.Add("EmployeeTaxZoneId", item.EmployeeTaxZoneId);
                    parameters.Add("TaxZone", item.TaxZone);
                    parameters.Add("MinimumTaxAmount", item.MinimumTaxAmount);
                    parameters.Add("EffectiveDate", item.EffectiveDate);
                    parameters.Add("CompanyId", model.First().CompanyId);
                    parameters.Add("Organizationid", model.First().OrganizationId);
                    parameters.Add("ExecutionFlag", Data.Read);

                    var data = await _dapper.SqlQueryFirstAsync<EmployeeTaxZoneViewModel>(user.Database, "sp_Payroll_EmployeeTaxZone_Insert_Update", parameters, commandType: CommandType.StoredProcedure);
                    if (data != null && data.EmployeeTaxZoneId > 0)
                    {
                        if (executionStatus == null)
                        {
                            executionStatus = new ExecutionStatus();
                            executionStatus.Status = false;
                            executionStatus.Msg = "Validation Error";
                            executionStatus.Errors = new Dictionary<string, string>();
                        }
                    }
                }
                if (executionStatus != null)
                {
                    executionStatus.Errors.Add("duplicate", duplicate);
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "TaxZoneValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<EmployeeTaxZoneViewModel> GetEmployeeTaxZoneByIdAsync(long id, AppUser user)
        {
            EmployeeTaxZoneViewModel data = new EmployeeTaxZoneViewModel();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxZone_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeTaxZoneId", id.ToString());
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryFirstAsync<EmployeeTaxZoneViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "GetEmployeeTaxZoneByIdAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<EmployeeTaxZoneViewModel>> GetEmployeeTaxZonesAsync(long? employeeId, string taxZone, AppUser user)
        {
            IEnumerable<EmployeeTaxZoneViewModel> data = new List<EmployeeTaxZoneViewModel>();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxZone_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeTaxZoneId", 0);
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("TaxZone", taxZone ?? "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<EmployeeTaxZoneViewModel>(Database.Payroll, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "GetEmployeeTaxZonesAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetTaxZoneNameExtensionAsync(string taxZone, AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxZone_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("TaxZone", taxZone);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Extension);
                data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "GetTaxZoneNameExtensionAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> UploadTaxZoneAsync(List<EmployeeTaxZoneViewModel> taxZones, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxZone_Insert_Update";
                var jsonData = Utility.JsonData(taxZones);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.UserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", user.BranchId);
                paramaters.Add("ExecutionFlag", "TaxZone_Upload");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "UploadTaxZoneAsync", user);
            }
            return executionStatus;
        }
    }
}
