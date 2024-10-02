using Dapper;
using System.Data;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.ViewModels;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{

    public class ReportConfigBusiness : IReportConfigBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public ReportConfigBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<ReportConfigViewModel>> GetReportConfigsAsync(string reportCategory, string fiscalYearRange, long companyId, long organizationId)
        {
            IEnumerable<ReportConfigViewModel> list = new List<ReportConfigViewModel>();
            try
            {
                var sp_name = "sp_ReportConfig";
                var parameters = new DynamicParameters();
                parameters.Add("ReportCategory", reportCategory);
                parameters.Add("FiscalYearRange", fiscalYearRange);
                parameters.Add("CompanyId", companyId);
                parameters.Add("OrganizationId", organizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                list = await _dapper.SqlQueryListAsync<ReportConfigViewModel>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ReportConfigBusiness", "GetReportConfigsAsync", null, organizationId, companyId, 0);
            }
            return list;
        }
        public async Task<ReportConfigViewModel> ReportConfigAsync(string reportCategory, string fiscalYearRange, long companyId, long organizationId)
        {
            ReportConfigViewModel info = new ReportConfigViewModel();
            try
            {
                var sp_name = "sp_ReportConfig";
                var parameters = new DynamicParameters();
                parameters.Add("ReportCategory", reportCategory);
                parameters.Add("FiscalYearRange", fiscalYearRange);
                parameters.Add("CompanyId", companyId);
                parameters.Add("OrganizationId", organizationId);
                parameters.Add("ExecutionFlag", "Config");
                info = await _dapper.SqlQueryFirstAsync<ReportConfigViewModel>(Database.ControlPanel, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ReportConfigBusiness", "ReportConfigAsync", null, organizationId, companyId, 0);
            }
            return info;
        }
    }
}
