using BLL.Base.Interface;
using BLL.Dashboard.Admin.HR.Interface;
using DAL.DapperObject.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Dashboard.Admin.HR.Implementation
{
    public class HrDashboardBusiness : IHrDashboardBusiness
    {
        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public HrDashboardBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }


        public async Task<object> GetHrDashboardDataAsync(AppUser user)
        {
            try
            {
                var sp_name = "sp_GetHrDahboardDetails";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HrDashboardBusiness", "GetHrDashboardDataAsync", user);

                return null;
            }
        }

        public async Task<object> GetTotalEmployeeAsync(AppUser user)
        {
            try
            {
                var sp_name = "sp_GetTotalEmployees";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HrDashboardBusiness", "GetTotalEmployeeAsync", user);

                return null;
            }
        }

        public async Task<object> GetReligionsAsync(AppUser user)
        {
            try
            {
                var sp_name = "sp_GetReligions";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HrDashboardBusiness", "GetReligionsAsync", user);

                return null;
            }
        }



        public async Task<object> GetAverageEmployeeDetailsAsync(AppUser user)
        {
            try
            {
                var sp_name = "sp_GetHrEmployeeDetails";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HrDashboardBusiness", "GetAverageEmployeeDetailsAsync", user);

                return null;
            }
        }

    }
}
