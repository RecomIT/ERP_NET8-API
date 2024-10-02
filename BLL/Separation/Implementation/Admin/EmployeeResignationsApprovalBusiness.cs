using BLL.Base.Interface;
using BLL.Separation.Interface.Admin;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Shared.OtherModels.User;
using Shared.Separation.ViewModels.User;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Separation.Implementation.Admin
{
    public class EmployeeResignationsApprovalBusiness : IEmployeeResignationsApprovalBusiness
    {
        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public EmployeeResignationsApprovalBusiness(
            IDapperData dapper,
            ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }



        public async Task<IEnumerable<object>> GetApprovedResignationRequestsBySupervisorAsync(dynamic filter, AppUser user)
        {
            IEnumerable<ResignationRequestViewModel> data = new List<ResignationRequestViewModel>();
            try
            {
                var sp_name = "sp_GetApprovedResignationRequestsBySupervisor";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false, addRoleName: true);
                data = await _dapper.SqlQueryListAsync<ResignationRequestViewModel>(user.Database, sp_name, parameters,
               CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel,
               "EmployeeResignationsApprovalBusiness", "GetApprovedResignationRequestsBySupervisorAsync", user);
            }
            return data;
        }
    }
}
