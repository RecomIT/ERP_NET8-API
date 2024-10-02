using Dapper;
using System.Data;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Salary.Payment.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.Domain.Payment;

namespace BLL.Salary.Payment.Implementation
{
    public class DepositeAllowanceHistoryBusiness : IDepositeAllowanceHistoryBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public DepositeAllowanceHistoryBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<IEnumerable<DepositAllowanceHistory>> GetEmployeeDepositAllowanceHistoriesExceptPaymentMonthAsync(FindDepositAllowanceHistory_Filter filter, AppUser user)
        {
            IEnumerable<DepositAllowanceHistory> list = new List<DepositAllowanceHistory>();
            try
            {
                var query = "";
                var parameters = new DynamicParameters();
                if (filter.IncomingFlag == "Conditional")
                {
                    query = $@"SELECT * FROM Payroll_DepositAllowanceHistory
                    Where 1=1
                    AND (EmployeeId =@EmployeeId)
                    AND (AllowanceNameId =@AllowanceNameId)
                    AND (DepositDate < @DepositDate)
                    AND (ConditionalDepositAllowanceConfigId=@ConditionalDepositAllowanceConfigId)
                    AND CompanyId=@CompanyId
                    AND OrganizationId=@OrganizationId";
                    parameters.Add("EmployeeId", filter.EmployeeId);
                    parameters.Add("AllowanceNameId", filter.AllowanceNameId);
                    parameters.Add("DepositDate", filter.DepositDate);
                    parameters.Add("ConditionalDepositAllowanceConfigId", filter.ConditionalDepositAllowanceConfigId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                }
                else
                {
                    query = $@"SELECT * FROM Payroll_DepositAllowanceHistory
                    Where 1=1
                    AND (EmployeeId =@EmployeeId)
                    AND (AllowanceNameId =@AllowanceNameId)
                    AND (DepositDate < @DepositDate)
                    AND (EmployeeDepositAllowanceConfigId=@EmployeeDepositAllowanceConfigId)
                    AND CompanyId=@CompanyId
                    AND OrganizationId=@OrganizationId";
                    parameters.Add("EmployeeId", filter.EmployeeId);
                    parameters.Add("AllowanceNameId", filter.AllowanceNameId);
                    parameters.Add("PaymentMonth", filter.PaymentMonth);
                    parameters.Add("PaymentMonth", filter.PaymentYear);
                    parameters.Add("EmployeeDepositAllowanceConfigId", filter.EmployeeDepositAllowanceConfigId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                }

                if (query != "" && query != null)
                {
                    list = await _dapper.SqlQueryListAsync<DepositAllowanceHistory>(user.Database, query.Trim(), parameters, CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositeAllowanceHistoryBusiness", "GetEmployeeDepositAllowanceHistoriesExceptPaymentMonthAsync", user);
            }
            return list;
        }
    }
}
