using BLL.Base.Interface;
using BLL.Dashboard.Admin.Interface;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Shared.Models.Dashboard.SubordinatesLeave.Approval.ViewModel;
using Shared.Models.Dashboard.SubordinatesLeave.LeaveHistory.ViewModel;
using Shared.OtherModels.User;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dashboard.Admin.Implentation
{
    public class EmployeeLeaveBusines : IEmployeeLeaveBusiness
    {

        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public EmployeeLeaveBusines(
            IDapperData dapper,
            ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }


        public async Task<object> GetEmployeesAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_GetAllEmployees";


                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBusines", "GetEmployeesAsync", user);

                return null;
            }

        }



        public async Task<IEnumerable<object>> GetEmployeesLeaveAsync(dynamic filter, AppUser user)
        {
            IEnumerable<SubordinatesLeaveHistoryViewModel> data = new List<SubordinatesLeaveHistoryViewModel>();
            try
            {
                var sp_name = "sp_GetEmployeesLeaveInfo";
                var parameters = DapperParam.AddParams(filter, user);

                data = await _dapper.SqlQueryListAsync<SubordinatesLeaveHistoryViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeAttendanceDataBusiness", "GetSubordinatesLeaveAsync", user);
            }
            return data;
        }




        public async Task<IEnumerable<object>> GetEmployeesLeaveApprovalAsync(dynamic filter, AppUser user)
        {
            IEnumerable<LeaveApprovalViewModel> data = new List<LeaveApprovalViewModel>();
            try
            {
                var sp_name = "sp_GetEmployeesLeaveApproval";
                var parameters = DapperParam.AddParams(filter, user);

                data = await _dapper.SqlQueryListAsync<LeaveApprovalViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeAttendanceDataBusiness", "GetEmployeesLeaveApprovalAsync", user);
            }
            return data;
        }



    }
}
