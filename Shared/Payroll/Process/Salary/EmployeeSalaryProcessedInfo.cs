using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Domain.Salary;

namespace Shared.Payroll.Process.Salary
{
    public class EmployeeSalaryProcessedInfo
    {
        public EmployeeSalaryProcessedInfo()
        {
            SalaryProcessDetail = new SalaryProcessDetail();
            SalaryAllowances = new List<SalaryAllowance>();
            SalaryAllowanceArrears = new List<SalaryAllowanceArrear>();
            SalaryAllowanceAdjustments = new List<SalaryAllowanceAdjustment>();
            SalaryDeductions = new List<SalaryDeduction>();
            SalaryDeductionAdjustments = new List<SalaryDeductionAdjustment>();
            DepositAllowancePaymentHistories = new List<DepositAllowancePaymentHistory>();
            RecipientsofServiceAnniversaryAllowances = new List<RecipientsofServiceAnniversaryAllowance>();
            SalaryComponentHistories = new List<SalaryComponentHistory>();
            MonthlyAllowanceHistories = new List<MonthlyAllowanceHistory>();

        }
        public SalaryProcessDetail SalaryProcessDetail { get; set; }
        public List<SalaryAllowance> SalaryAllowances { get; set; }
        public List<SalaryAllowanceArrear> SalaryAllowanceArrears { get; set; }
        public List<SalaryAllowanceAdjustment> SalaryAllowanceAdjustments { get; set; }
        public List<SalaryDeduction> SalaryDeductions { get; set; }
        public List<SalaryDeductionAdjustment> SalaryDeductionAdjustments { get; set; }
        public List<DepositAllowanceHistory> DepositAllowanceHistories { get; set; }// Deposit
        public List<DepositAllowancePaymentHistory> DepositAllowancePaymentHistories { get; set; }// Deposit
        public List<RecipientsofServiceAnniversaryAllowance> RecipientsofServiceAnniversaryAllowances { get; set; } // Service Allowance
        public List<SalaryComponentHistory> SalaryComponentHistories { get; set; } // Service Allowance
        public List<MonthlyAllowanceHistory> MonthlyAllowanceHistories { get; set; } // 
    }
}
