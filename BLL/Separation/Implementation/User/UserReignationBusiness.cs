using BLL.Base.Interface;
using BLL.Separation.Interface.User;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Shared.Models.Dashboard.CommonDashboard.Attendance.ViewModel;
using Shared.OtherModels.User;
using Shared.Separation.ViewModels.User;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Separation.Implementation.User
{
    public class UserReignationBusiness : IUserResignationBusiness
    {

        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public UserReignationBusiness(
            IDapperData dapper,
            ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }



        public async Task<IEnumerable<object>> GetUserResignationRequestsAsync(dynamic filter, AppUser user)
        {
            IEnumerable<ResignationRequestViewModel> data = new List<ResignationRequestViewModel>();
            try
            {
                var sp_name = "sp_HR_GetUserResignations";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);
                data = await _dapper.SqlQueryListAsync<ResignationRequestViewModel>(user.Database, sp_name, parameters,
               CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel,
               "UserReignationBusiness", "GetUserResignationRequestsAsync", user);
            }
            return data;
        }
    }
}
