using BLL.Base.Interface;
using BLL.Employee.Interface.Account;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Account;
using Shared.Employee.Filter.Account;
using Shared.Employee.ViewModel.Account;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;

namespace BLL.Employee.Implementation.Account
{
    public class BankBusiness : IBankBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public BankBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<BankViewModel>> GetBanksAsync(Bank_Filter filter, AppUser user)
        {
            IEnumerable<BankViewModel> list = new List<BankViewModel>();
            try
            {
                var sp_name = "sp_HR_Banks_List";
                var parameters = Utility.DappperParams(filter, user);
                list = await _dapper.SqlQueryListAsync<BankViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBusiness", "GetBanksAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveBankAsync(BankDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Banks_Insert_Update_Delete";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.BankId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBusiness", "SaveBankAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorBankAsync(BankDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Banks_Insert_Update_Delete";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBusiness", "ValidatorBankAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetBankDropdownAsync(AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetBanksAsync(new Bank_Filter(), user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.BankId,
                        Value = item.BankId.ToString(),
                        Text = item.BankName.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBusiness", "GetBankDropdownAsync", user);
            }
            return dropdowns;
        }
    }
}
