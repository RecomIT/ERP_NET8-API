using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Setup;
using BLL.Salary.Setup.Interface;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Payroll.Domain.Setup;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Setup;
using DAL.Context.Payroll;
using Microsoft.EntityFrameworkCore;

namespace BLL.Salary.Setup.Implementation
{
    public class FiscalYearBusiness : IFiscalYearBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly PayrollDbContext _payrollDbContext;

        public FiscalYearBusiness(IDapperData dapper, ISysLogger sysLogger, PayrollDbContext payrollDbContext)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _payrollDbContext = payrollDbContext;
        }

        public async Task<IEnumerable<FiscalYearViewModel>> GetFiscalYearsAsync(long? fiscalYearId, string fiscalYearRange, AppUser user)
        {
            IEnumerable<FiscalYearViewModel> data = new List<FiscalYearViewModel>();
            try
            {
                var sp_name = "sp_Payroll_FiscalYear";
                var parameters = new DynamicParameters();
                parameters.Add("FiscalYearId", fiscalYearId ?? 0);
                parameters.Add("FiscalYearRange", fiscalYearRange ?? "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<FiscalYearViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearBusiness", "GetFiscalYearsAsync", user);
            }
            return data;
        }
        public async Task<FiscalYearViewModel> GetFiscalYearAsync(long? fiscalYearId, AppUser user)
        {
            FiscalYearViewModel data = new FiscalYearViewModel();
            try
            {
                var sp_name = "sp_Payroll_FiscalYear";
                var parameters = new DynamicParameters();
                parameters.Add("FiscalYearId", fiscalYearId ?? 0);
                parameters.Add("FiscalYearRange", "");
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryFirstAsync<FiscalYearViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearBusiness", "GetFiscalYearAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveFiscalYearAsync(FiscalYearDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var fiscalYearRanges = model.FiscalYearRange.Split('-');
                var assesmentYear = (Convert.ToInt16(fiscalYearRanges[0]) + 1).ToString() + "-" + (Convert.ToInt16(fiscalYearRanges[0]) + 2).ToString();
                model.AssesmentYear = assesmentYear;
                model.FiscalYearFrom = Convert.ToDateTime(fiscalYearRanges[0] + "-07-01");
                model.FiscalYearTo = Convert.ToDateTime(fiscalYearRanges[1] + "-06-30");
                var sp_name = "sp_Payroll_FiscalYear";
                var parameters = new DynamicParameters();
                parameters.Add("FiscalYearId", model.FiscalYearId);
                parameters.Add("FiscalYearRange", model.FiscalYearRange ?? "");
                parameters.Add("FiscalYearFrom", model.FiscalYearFrom);
                parameters.Add("FiscalYearTo", model.FiscalYearTo);
                parameters.Add("AssesmentYear", model.AssesmentYear);
                parameters.Add("StateStatus", model.StateStatus);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", model.FiscalYearId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearBusiness", "GetFiscalYearAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeleteFiscalYearAsync(long fiscalYearId, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_FiscalYear";
                var parameters = new DynamicParameters();
                parameters.Add("FiscalYearId", fiscalYearId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Delete);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearBusiness", "DeleteFiscalYearAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetFiscalYearsExtensionAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = "sp_Payroll_FiscalYear";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Extension);
                data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearBusiness", "GetFiscalYearsExtensionAsync", user);
            }
            return data;
        }
        public async Task<FiscalYearViewModel> GetCurrentFiscalYearAsync(AppUser user)
        {
            FiscalYearViewModel data = new FiscalYearViewModel();
            try
            {
                var param = new DynamicParameters();
                param.Add("CompanyId", user.CompanyId);
                param.Add("OrganizationId", user.OrganizationId);
                var sp_name = @$"SELECT FiscalYearId,FiscalYearFrom,FiscalYearTo,FiscalYearRange FROM Payroll_FiscalYear 
                Where 1=1 AND CAST(GETDATE() AS DATE) BETWEEN  FiscalYearFrom AND FiscalYearTo AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                data = await _dapper.SqlQueryFirstAsync<FiscalYearViewModel>(user.Database, sp_name, param, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearBusiness", "GetCurrentFiscalYearAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<Dropdown>> GetFiscalYearDropdownAysnc(AppUser user)
        {
            IEnumerable<Dropdown> list = new List<Dropdown>();
            try
            {
                var items = (await GetFiscalYearsAsync(0, null, user)).AsList();
                foreach (var item in items)
                {
                    Dropdown dropdown = new Dropdown();
                    dropdown.Text = item.FiscalYearRange;
                    dropdown.Value = item.FiscalYearId.ToString();
                    dropdown.Id = item.FiscalYearId;
                    dropdown.Remarks = item.FiscalYearFrom.Value.ToString("dd MMM yyyy") + "~" + item.FiscalYearTo.Value.ToString("dd MMM yyyy");
                    list.AsList().Add(dropdown);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearBusiness", "GetFiscalYearDropdownAysnc", user);
            }
            return list;
        }
        public async Task<FiscalYear> GetFiscalYearInfoWithinADate(string date, AppUser user)
        {
            FiscalYear fiscalYear = null;
            try
            {
                var query = $@"SELECT * FROM Payroll_FiscalYear 
				Where @Date BETWEEN FiscalYearFrom AND FiscalYearTo AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("Date", date);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                fiscalYear = await _dapper.SqlQueryFirstAsync<FiscalYear>(user.Database, query.Trim(), parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearBusiness", "GetFiscalYearDropdownAysnc", user);
            }
            return fiscalYear;
        }

        public async Task<FiscalYear> GetFiscalYearByIdAsync(long fiscalYearId, AppUser user)
        {
            FiscalYear fiscalYear = null;
            try
            {
                fiscalYear = await _payrollDbContext.Payroll_FiscalYear.FirstOrDefaultAsync(i => i.FiscalYearId == fiscalYearId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return fiscalYear;
        }
    }
}
