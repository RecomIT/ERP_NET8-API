using BLL.Base.Interface;
using BLL.Dashboard.DataService.Interface;
using DAL.DapperObject.Interface;
using NLog.Filters;
using Shared.OtherModels.User;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BLL.Dashboard.DataService.Implementation
{
    public class DataGetService : IDataGetService
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public DataGetService(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }



        // ------------------------------------------ >>> GetDataWithoutEmployeeAsync
        public async Task<object> GetDataWithoutEmployeeAsync<T>(AppUser user, string sqlQuery, string businessClassName, string methodName)
        {
            try
            {
                var data = await _dapper.SqlQueryListAsync<T>(user.Database, sqlQuery, Utility.DappperParam(user, addEmployee: false), CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _sysLogger.SaveHRMSException(ex, user.Database, businessClassName, methodName, user);
                return null;
            }
        }


        // ------------------------------------------ >>> GetDataAsync
        public async Task<object> GetDataAsync<T>(AppUser user, string sqlQuery, string businessClassName, string methodName)
        {
            try
            {
                var data = await _dapper.SqlQueryListAsync<T>(user.Database, sqlQuery, Utility.DappperParam(user), CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await _sysLogger.SaveHRMSException(ex, user.Database, businessClassName, methodName, user);
                return null;
            }
        }



        // ------------------------------------------ >>> GetDataAsync with Filter
        public Task<IEnumerable<T>> GetDataAsync<T>(AppUser user, string sqlQuery, string businessClassName, string methodName, object filter = null)
        {
            try
            {
                var parameters = Utility.DappperParams(filter, user, addEmployee: true, addUserId: false);
                var data = _dapper.SqlQueryListAsync<T>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _sysLogger.SaveHRMSException(ex, user.Database, businessClassName, methodName, user);
                return null;
            }
        }






        // ------------------------------------------ >>> GetDataAsync
        public async Task<object> GetDataAsync<T>(AppUser user, string sqlQuery)
        {
            try
            {
                var parameters = DapperParam.AddParams(user);

                var data = await _dapper.SqlQueryListAsync<T>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        // ------------------------------------------ >>> GetDataAsync with Filter
        public Task<IEnumerable<T>> GetDataAsync<T>(AppUser user, string sqlQuery, object filter = null)
        {
            try
            {
                var parameters = Utility.DappperParams(filter, user, addEmployee: true);
                var data = _dapper.SqlQueryListAsync<T>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}

