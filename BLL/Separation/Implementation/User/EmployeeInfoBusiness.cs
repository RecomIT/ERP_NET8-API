using BLL.Base.Interface;
using BLL.Separation.Interface.User;
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

namespace BLL.Separation.Implementation.User
{
    internal class EmployeeInfoBusiness : IEmployeeInfoBusiness
    {
        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public EmployeeInfoBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }


        public async Task<object> GetEmployeesInfoAsync(AppUser user)
        {
            try
            {
                var sp_name = "sp_GetEmployeesInfo";

                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var data = await _dapper.SqlQueryListAsync<EmployeeInfoViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeesInfoAsync", user);

                return null;
            }
        }




        public async Task<object> GetEmployeeDetailsAsync(dynamic filter, AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_GetEmployeeDetails";

                var parameters = DapperParam.AddParams(filter, user);

                var data = await _dapper.SqlQueryFirstAsync<EmployeeInfoViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeeResignationAsync", user);

                return null;
            }

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



    }
}
