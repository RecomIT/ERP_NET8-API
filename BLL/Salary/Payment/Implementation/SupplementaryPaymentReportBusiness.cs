using System.Data;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Salary.Payment.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Filter.Payment;

namespace BLL.Salary.Payment.Implementation
{
    public class SupplementaryPaymentReportBusiness : ISupplementaryPaymentReportBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public SupplementaryPaymentReportBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<DataTable> PayslipDetail(long processId, long paymentId, long employeeId, AppUser user)
        {
            DataTable dataTable = null;
            try
            {
                var query = "sp_Payroll_SupplementaryPaymentPayslipDetail";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("ProcessId", processId.ToString());
                keyValuePairs.Add("PaymentId", paymentId.ToString());
                keyValuePairs.Add("EmployeeId", employeeId.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, query, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
            }
            return dataTable;
        }
        public async Task<DataTable> PayslipInfo(SupplementaryPaymentReport_Filter filter, AppUser user)
        {
            DataTable dataTable = null;
            try
            {
                var query = "sp_Payroll_SupplementaryPaymentPayslipInfo";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("ProcessId", filter.ProcessId.ToString());
                keyValuePairs.Add("PaymentId", filter.PaymentId.ToString());
                keyValuePairs.Add("EmployeeId", filter.EmployeeId.ToString());
                keyValuePairs.Add("Month", filter.PaymentMonth.ToString());
                keyValuePairs.Add("Year", filter.PaymentYear.ToString());
                keyValuePairs.Add("IsDisbursed", filter.IsDisbursed == null ? null : filter.IsDisbursed ?? false == false ? "0" : "1");
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, query, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

            }
            return dataTable;
        }
    }
}
