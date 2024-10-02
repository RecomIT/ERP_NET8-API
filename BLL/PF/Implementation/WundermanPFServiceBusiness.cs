using System;
using DAL.DapperObject;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using BLL.Base.Interface;
using System.Data;
using DAL.DapperObject.Interface;
using BLL.PF.Interface;

namespace BLL.PF.Implementation
{
    public class WundermanPFServiceBusiness : IWundermanPFServiceBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public WundermanPFServiceBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<decimal> GetEmployeeLoanDeductionAmountAsync(string employeeCode, int salaryYear, int salaryMonth, AppUser user)
        {
            decimal amount = 0;
            try
            {
                string query = $@"Select ROUND(ISNULL(dbo.fnGetLoanDeduction (@employeeCode,@salaryYear,@salaryMonth),0),0)";
                amount = await _dapper.ExecuteScalerFunction<decimal>(Database.Wunderman_PF, query, new { employeeCode, salaryYear, salaryMonth }, CommandType.Text);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetEmployeeLoanDeductionAmountAsync", user);
            }
            return amount;
        }
    }
}
