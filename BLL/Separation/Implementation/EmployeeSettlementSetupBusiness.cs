using BLL.Base.Interface;
using BLL.Dashboard.DataService.Interface;
using BLL.Separation.Interface;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Separation.Settlement.ViewModel;
using Shared.Separation.ViewModels.SettlementSetup;
using Shared.Services;
using System.Data;

namespace BLL.Separation.Implementation
{
    public class EmployeeSettlementSetupBusiness : IEmployeeSettlementSetupBusiness
    {

        private readonly IDataGetService _dataGetService;
        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public EmployeeSettlementSetupBusiness(IDataGetService dataGetService, IDapperData dapper, ISysLogger sysLogger)
        {
            _dataGetService = dataGetService;
            _dapper = dapper;
            _sysLogger = sysLogger;

        }







        public async Task<IEnumerable<object>> GetPendingSettlementSetupListAsync(dynamic filter, AppUser user)
        {
            IEnumerable<PendingSettlementSetupViewModel> data = new List<PendingSettlementSetupViewModel>();
            try
            {
                var sp_name = "sp_GetEmployeePendingSettlementSetupList";
                var parameters = DapperParam.AddParams(filter, user);

                data = await _dapper.SqlQueryListAsync<PendingSettlementSetupViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeSettlementSetupBusiness", "GetPendingSettlementSetupListAsync", user);
            }
            return data;
        }






        public async Task<IEnumerable<object>> GetSettlementSetupListAsync(dynamic filter, AppUser user)
        {
            IEnumerable<EmployeeSettlementSetupViewModel> data = new List<EmployeeSettlementSetupViewModel>();
            try
            {
                var sp_name = "sp_GetEmployeeSettlementSetupList";
                var parameters = DapperParam.AddParams(filter, user);

                data = await _dapper.SqlQueryListAsync<EmployeeSettlementSetupViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeSettlementSetupBusiness", "GetSettlementSetupListAsync", user);
            }
            return data;
        }











        public async Task<object> SaveEmployeeSettlementSetupAsync(dynamic filter, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SettlementSetup";
                var parameters = DapperParam.AddParams(filter, user);

                var data = await _dapper.SqlQueryFirstAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeSettlementSetupBusiness", "SaveEmployeeSettlementSetupAsync", user);
            }
            return executionStatus;
        }






        public async Task<object> GetPendingSettlementSetupEmployeesAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_GetResignationApprovedEmployeeList";

                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }






        public async Task<object> GetResignationSetupEmployeeListAsync(AppUser user)
        {
            try
            {

                var sp_name = "sp_GetResignationSetupEmployeeList";

                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

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
