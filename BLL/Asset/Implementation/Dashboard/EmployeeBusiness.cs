
using BLL.Base.Interface;
using Shared.OtherModels.User;
using System.Data;
using Dapper;
using Shared.Asset.ViewModel.Dashboard;
using DAL.DapperObject.Interface;
using BLL.Asset.Interface.Dashboard;


namespace BLL.Asset.Implementation.Dashboard
{

    public class EmployeeBusiness : IEmployeeBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public EmployeeBusiness(
            IDapperData dapper,
            ISysLogger sysLogger)            
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<EmployeeViewModel>> GetAssetAsync(AppUser user)
        {
            IEnumerable<EmployeeViewModel> list = new List<EmployeeViewModel>();
            try {
                var sp_name = "sp_Asset_Employee_Assigning_List";          
                var parameters = new DynamicParameters();
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<EmployeeViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeBusiness", "GetAssetAsync", user);
            }
            return list;
        }
   

    }
}
