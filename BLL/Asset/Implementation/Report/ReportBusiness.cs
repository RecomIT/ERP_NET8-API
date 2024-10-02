using BLL.Asset.Report.Interface;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using Shared.Asset.Filter.Report;
using Shared.OtherModels.User;
using System.Data;


namespace BLL.Asset.Report.Implementation
{
    public class ReportBusiness : IReportBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public ReportBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<DataTable> GetAssetReport(Report_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_Asset_Asset_Report";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("AssetId", filter.AssetId ?? "0");
                keyValuePairs.Add("ProductId", filter.ProductId);
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);
                keyValuePairs.Add("VendorId", filter.VendorId ?? "0");
                keyValuePairs.Add("CategoryId", filter.CategoryId ?? "0");
                keyValuePairs.Add("SubCategoryId", filter.SubCategoryId ?? "0");
                keyValuePairs.Add("BrandId", filter.BrandId ?? "0");
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetAssetReport", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetAssetAssigningReport(Report_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_Asset_Assigning_Report";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("SelectedEmployees", filter.SelectedEmployees);
                keyValuePairs.Add("AssetId", filter.AssetId ?? "0");
                keyValuePairs.Add("ProductId", filter.ProductId);
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetAssetAssigningReport", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetServicingReport(Report_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_Asset_Servicing_Report";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("AssetId", filter.AssetId ?? "0");
                keyValuePairs.Add("ProductId", filter.ProductId);
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);
                keyValuePairs.Add("VendorId", filter.VendorId ?? "0");
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetServicingReport", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetReplacementReport(Report_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_Asset_Replacement_Report";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("AssetId", filter.AssetId ?? "0");
                keyValuePairs.Add("ProductId", filter.ProductId);
                keyValuePairs.Add("SelectedEmployees", filter.SelectedEmployees);
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);
                keyValuePairs.Add("Status", filter.Status );
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetReplacementReport", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetHandoverReport(Report_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_Asset_Handover_Report";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("AssetId", filter.AssetId ?? "0");
                keyValuePairs.Add("ProductId", filter.ProductId);
                keyValuePairs.Add("SelectedEmployees", filter.SelectedEmployees);
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);             
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetHandoverReport", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetRepairedReport(Report_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_Asset_Repaired_Report";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("AssetId", filter.AssetId ?? "0");
                keyValuePairs.Add("ProductId", filter.ProductId);
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetDamageReport", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetDamageReport(Report_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_Asset_Damage_Report";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("AssetId", filter.AssetId ?? "0");
                keyValuePairs.Add("ProductId", filter.ProductId);
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetDamageReport", user);
            }
            return dataTable;
        }

        public async Task<DataTable> GetStockReport(Report_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try {
                var sp_name = "sp_Asset_Stock_Report";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("AssetId", filter.AssetId ?? "0");
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportBusiness", "GetStockReport", user);
            }
            return dataTable;
        }



    }

}
