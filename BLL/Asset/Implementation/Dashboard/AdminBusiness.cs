
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;
using Dapper;
using DAL.DapperObject.Interface;
using Shared.Asset.ViewModel.Dashboard;
using BLL.Asset.Interface.Dashboard;
using Shared.Asset.Filter.Report;



namespace BLL.Asset.Implementation.Dashboard
{

    public class AdminBusiness : IAdminBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public AdminBusiness(
            IDapperData dapper,
            ISysLogger sysLogger)            
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
 

        public async Task<object> GetAssetCreationDataAsync(AppUser user)
        {
            try {
                var sp_name = $@"Select Count(AssetId) as AssetId From Asset_Create
                Where 1=1 AND Approved=1 AND (CompanyId = @CompanyId) AND (OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(user);
                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.Text);
                return data;
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdminBusiness", "GetAssetCreationDataAsync", user);
                return null;
            }

        }

        public async Task<object> GetAssetAssigningDataAsync(AppUser user)
        {
            try {
                var sp_name = $@"Select Count(AssetId) as AssetId From Asset_Assigning
                Where 1=1 AND Approved=1 AND (CompanyId = @CompanyId) AND (OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(user);
                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.Text);
                return data;
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AdminBusiness", "GetAssetAssigningDataAsync", user);
                return null;
            }
        }

        public async Task<IEnumerable<AdminViewModel>> GetAssetAsync(Report_Filter filter, AppUser user)
        {
            IEnumerable<AdminViewModel> list = new List<AdminViewModel>();
            try {
                var sp_name = "sp_Asset_Stock_Dashboard";
                var parameters = new DynamicParameters();
                //parameters.Add("AssetId", filter.AssetId.ToString());
                //parameters.Add("CategoryId", filter.CategoryId.ToString());
                //parameters.Add("SubCategoryId", filter.SubCategoryId.ToString());
                //parameters.Add("BrandId", filter.BrandId.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                list = await _dapper.SqlQueryListAsync<AdminViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);              

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateBusiness", "AssetDropdownAsync", user);
            }
            return list;
        }

    }
}
