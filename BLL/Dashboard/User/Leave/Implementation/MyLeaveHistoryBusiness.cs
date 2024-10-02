using BLL.Base.Interface;
using BLL.Dashboard.User.Leave.Interface;
using DAL.DapperObject.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using System;
using System.Data;
using System.Threading.Tasks;

namespace BLL.Dashboard.User.Leave.Implementation
{
    public class MyLeaveHistoryBusiness : IMyLeaveHistoryBusiness
    {
        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public MyLeaveHistoryBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }


        public async Task<object> GetLeavePeriodMonthAsync(dynamic filter, AppUser user)
        {

            try
            {
                var sp_name = "sp_GetLeavePeriodMonth";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MyLeaveHistoryBusiness", "GetLeavePeriodMonthAsync", user);

                return null;
            }
        }


        public async Task<object> GetLeavePeriodYearAsync(AppUser user)
        {
            try
            {
                var sp_name = "sp_GetLeavePeriodYear";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MyLeaveHistoryBusiness", "GetLeavePeriodYearAsync", user);

                return null;
            }
        }



        public async Task<object> GetMyLeaveHistoryAsync(dynamic filter, AppUser user)
        {
            try
            {
                var sp_name = "sp_GetMyAvailedLeavehistory";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MyLeaveHistoryBusiness", "GetEmployeeSettlementSetupAsync", user);

                return null;
            }
        }
    }
}
