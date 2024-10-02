using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using BLL.Employee.Interface.Account;
using Shared.OtherModels.DataService;
using DAL.DapperObject.Interface;
using Shared.Employee.ViewModel.Account;
using Shared.Employee.Filter.Account;
using Shared.Employee.DTO.Account;

namespace BLL.Employee.Implementation.Account
{
    public class BankBranchBusiness : IBankBranchBusiness
    {

        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public BankBranchBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<BankBranchViewModel>> GetBankBranchesAsync(BankBranch_Filter filter, AppUser user)
        {
            IEnumerable<BankBranchViewModel> list = new List<BankBranchViewModel>();
            try
            {
                var sp_name = "sp_HR_BanksBranches_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<BankBranchViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchBusiness", "GetBankBranchesAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveBankBranchAsync(BankBranchDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_BanksBranches_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.BankBranchId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchBusiness", "SaveBankBranchAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatorBankBranchAsync(BankBranchDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_BanksBranches_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchBusiness", "ValidatorBankBranchAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetBankBranchDropdownAsync(BankBranch_Filter filter, AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetBankBranchesAsync(filter, user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.BankBranchId,
                        Value = item.BankBranchId.ToString(),
                        Text = item.BankBranchName.ToString()
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchBusiness", "GetBankBranchDropdownAsync", user);
            }
            return dropdowns;
        }
    }
}
