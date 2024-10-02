using System.Data;
using Shared.OtherModels.User;
using Shared.Payroll.Filter.Payment;


namespace BLL.Salary.Payment.Interface
{
    public interface ISupplementaryPaymentReportBusiness
    {
        Task<DataTable> PayslipInfo(SupplementaryPaymentReport_Filter filter, AppUser user);
        Task<DataTable> PayslipDetail(long processId, long paymentId, long employeeId, AppUser user);
    }
}
