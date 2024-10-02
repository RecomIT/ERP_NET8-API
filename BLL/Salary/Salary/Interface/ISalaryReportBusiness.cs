using System.Data;
using Shared.OtherModels.Report;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Report;

namespace BLL.Salary.Salary.Interface
{
    public interface ISalaryReportBusiness
    {
        Task<ReportLayer> ReportLayerAsync(long organizationId, long companyId, long branchId, long divisionId);
        Task<DataTable> GetActualSalarySheetInfoAsync(ActualSalarySheetInfo_Filter filter, AppUser user);
        Task<DataTable> GetActualSalarySheetDetailAsync(ActualSalarySheetDetail_Filter filter, AppUser user);
        Task<ExecutionStatus> GetSalarySheetAsync(long SalaryProcessId, string SalaryBatch, short SalaryMonth, short SalaryYear, long EmployeeId, AppUser user);
        Task<ExecutionStatus> GetSalarySheetAsync(SalarySheet_Filter filter, AppUser user);
        Task<DataTable> GetSalarySheetReport(long SalaryProcessId, string SalaryBatch, short SalaryMonth, short SalaryYear, long EmployeeId, AppUser user);
        Task<DataTable> GetSalarySheetReport(SalarySheet_Filter filter, AppUser user);
        Task<DataTable> PayslipReportInfoAsync(long employeeId, short month, short year, AppUser user);
        Task<PayslipMaster> PayslipExtensionAsync(long employeeId, short month, short year, AppUser user);
        Task<DataTable> PayslipExtensionAsync(Payslip_Filter filter, AppUser user);
        Task<DataTable> PayslipReportInfoAsync(Payslip_Filter filter, AppUser user, string spName = null);
        Task<DataTable> PayslipReportDetailAsync(Payslip_Filter filter, AppUser user);
        Task<DataTable> GetDateRangeSalarySheetReportAsync(SalarySheet_Filter sheet_Filter, AppUser user);
        Task<ExecutionStatus> UpdateAggregateSumInSalaryProcessAsync(Reconciliation_Filter filter, AppUser user);
        Task<DataTable> ReconciliationRptAsync(Reconciliation_Filter filter, AppUser user);
        Task<DataTable> SalaryBreakdownRptAsync(SalaryBreakdown_Filter filter, AppUser user);
        Task<DataTable> SalaryBreakdownDtlsRptAsync(SalaryBreakdown_Filter filter, AppUser user);
        Task<DataTable> SalaryReconciliationRptAsync(Reconciliation_Filter filter, AppUser user);
        Task<DataTable> GetBankStatementAsync(BankStatement_Filter filter, AppUser user);

        #region Self Service Part
        Task<PayslipMaster> SelfPayslipExtensionAsync(long employeeId, short month, short year, AppUser user);
        #endregion
    }
}
