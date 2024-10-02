using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using BLL.Salary.Payment.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Process.Salary;

namespace BLL.Salary.Payment.Implementation
{
    public class DepositAllowancePaymentHistoryBusiness : IDepositAllowancePaymentHistoryBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IDepositeAllowanceHistoryBusiness _depositeAllowanceHistoryBusiness;

        public DepositAllowancePaymentHistoryBusiness(IDapperData dapper, ISysLogger sysLogger, IDepositeAllowanceHistoryBusiness depositeAllowanceHistoryBusiness)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _depositeAllowanceHistoryBusiness = depositeAllowanceHistoryBusiness;
        }

        public async Task<IEnumerable<DepositAllowancePaymentHistoryViewModel>> GetPreviousDepositAllowancePaymentProposalsAsync(FindDepositAllowancePaymentProposal_Filter filter, AppUser user)
        {
            IEnumerable<DepositAllowancePaymentHistoryViewModel> list = new List<DepositAllowancePaymentHistoryViewModel>();
            try
            {

                var query = "";
                var parameters = new DynamicParameters();
                if (filter.IncomingFlag == "Conditional")
                {
                    query = $@"SELECT PH.*,[AllowanceName]=ALW.[Name],EI.EmployeeCode,[EmployeeName]=EI.FullName 
                FROM Payroll_DepositAllowancePaymentHistory PH
                INNER JOIN Payroll_AllowanceName ALW ON PH.AllowanceNameId=ALW.AllowanceNameId
                INNER JOIN HR_EmployeeInformation EI ON PH.EmployeeId=EI.EmployeeId
                Where 1=1
                AND (PH.EmployeeId=@EmployeeId)
                AND (PH.PaymentDate <= CAST(@PaymentDate AS DATE))
                AND (PH.StateStatus='Disbursed')
                AND (PH.ConditionalDepositAllowanceConfigId=@ConditionalDepositAllowanceConfigId)
                AND (PH.CompanyId=@CompanyId)
                AND (PH.OrganizationId=@OrganizationId)";

                    parameters.Add("EmployeeId", filter.EmployeeId);
                    parameters.Add("PaymentDate", filter.PaymentDate);
                    parameters.Add("ConditionalDepositAllowanceConfigId", filter.ConditionalDepositAllowanceConfigId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                }
                else
                {
                    query = $@"SELECT PH.*,[AllowanceName]=ALW.[Name],EI.EmployeeCode,[EmployeeName]=EI.FullName 
                FROM Payroll_DepositAllowancePaymentHistory PH
                INNER JOIN Payroll_AllowanceName ALW ON PH.AllowanceNameId=ALW.AllowanceNameId
                INNER JOIN HR_EmployeeInformation EI ON PH.EmployeeId=EI.EmployeeId
                Where 1=1
                AND (PH.EmployeeId=@EmployeeId)
                AND (PH.PaymentDate <= CAST(@PaymentDate AS DATE))
                AND (PH.StateStatus='Disbursed')
                AND (PH.EmployeeDepositAllowanceConfigId=@EmployeeDepositAllowanceConfigId)
                AND (PH.CompanyId=@CompanyId)
                AND (PH.OrganizationId=@OrganizationId)";

                    parameters.Add("EmployeeId", filter.EmployeeId);
                    parameters.Add("PaymentDate", filter.PaymentDate);
                    parameters.Add("EmployeeDepositAllowanceConfigId", filter.EmployeeDepositAllowanceConfigId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                }

                if (query != "" && query != null)
                {
                    list = await _dapper.SqlQueryListAsync<DepositAllowancePaymentHistoryViewModel>(user.Database, query.Trim(), parameters, CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryBusiness", "GetThisMonthDepositAllowancePaymentProposalsAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<DepositAllowancePaymentHistory>> GetThisMonthDepositAllowancePaymentProposalsAsync(FindDepositAllowancePaymentProposal_Filter filter, AppUser user)
        {
            IEnumerable<DepositAllowancePaymentHistory> list = new List<DepositAllowancePaymentHistory>();
            try
            {
                var query = $@"SELECT PH.*,[AllowanceName]=ALW.[Name],EI.EmployeeCode,[EmployeeName]=EI.FullName 
                FROM Payroll_DepositAllowancePaymentHistory PH
                INNER JOIN Payroll_AllowanceName ALW ON PH.AllowanceNameId=ALW.AllowanceNameId
                INNER JOIN HR_EmployeeInformation EI ON PH.EmployeeId=EI.EmployeeId
                Where 1=1
                AND (PH.EmployeeId=@EmployeeId)
                AND (PH.PaymentBeMade=@PaymentBeMade)
                AND (PH.PaymentMonth=@PaymentMonth)
                AND (PH.PaymentYear=@PaymentYear)
                AND (PH.IsDisbursed IS NULL OR PH.IsDisbursed=0)
                AND (PH.StateStatus='Approved')
                AND (PH.CompanyId=@CompanyId)
                AND (PH.OrganizationId=@OrganizationId)";

                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", filter.EmployeeId);
                parameters.Add("PaymentBeMade", filter.PaymentBeMade);
                parameters.Add("PaymentMonth", filter.PaymentMonth);
                parameters.Add("PaymentYear", filter.PaymentYear);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<DepositAllowancePaymentHistory>(user.Database, query.Trim(), parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryBusiness", "GetThisMonthDepositAllowancePaymentProposalsAsync", user);
            }
            return list;
        }

        // Execute in Salary Process
        public async Task<IEnumerable<DepositAllowancePaymentHistory>> ThisMonthEmployeeDepositAllowanceCalculationInSalaryAsync(EligibleEmployeeForSalaryType employee, List<DepositAllowanceHistory> listOfNewDepositAmount, int month, int year, long fiscalYearId, AppUser user)
        {
            IEnumerable<DepositAllowancePaymentHistory> thisMonthDepositPaymentProposals = new List<DepositAllowancePaymentHistory>();
            try
            {
                var lastDateOfSalaryMonth = DateTimeExtension.LastDateOfAMonth(year, month);
                thisMonthDepositPaymentProposals = await GetThisMonthDepositAllowancePaymentProposalsAsync(new FindDepositAllowancePaymentProposal_Filter()
                {
                    EmployeeId = employee.EmployeeId.ToString(),
                    PaymentBeMade = "With Salary",
                    PaymentMonth = month.ToString(),
                    PaymentYear = year.ToString(),
                }, user);

                if (thisMonthDepositPaymentProposals != null && thisMonthDepositPaymentProposals.AsList().Count > 0)
                {
                    foreach (var paymentProposal in thisMonthDepositPaymentProposals)
                    {
                        paymentProposal.PaymentDate = lastDateOfSalaryMonth;
                        // till deposit paid
                        var previousDepositPayments = await GetPreviousDepositAllowancePaymentProposalsAsync(new FindDepositAllowancePaymentProposal_Filter()
                        {
                            ConditionalDepositAllowanceConfigId = paymentProposal.ConditionalDepositAllowanceConfigId.ToString(),
                            EmployeeDepositAllowanceConfigId = paymentProposal.EmployeeDepositAllowanceConfigId.ToString(),
                            IncomingFlag = paymentProposal.IncomingFlag,
                            AllowanceNameId = paymentProposal.AllowanceNameId.ToString(),
                            EmployeeId = paymentProposal.EmployeeId.ToString(),
                            PaymentDate = DateTimeExtension.LastDateOfAMonth(paymentProposal.PaymentYear, paymentProposal.PaymentMonth).ToString("yyyy-MM-dd")
                        }, user);

                        decimal tillDepositPaid = 0;
                        if (previousDepositPayments != null && previousDepositPayments.AsList().Count > 0)
                        {
                            tillDepositPaid = previousDepositPayments.AsList().Sum(i => i.DisbursedAmount);
                        }

                        // deposit amount except this month
                        var employeeDepositAllowanceHistoriesExceptPaymentMonth = await _depositeAllowanceHistoryBusiness.GetEmployeeDepositAllowanceHistoriesExceptPaymentMonthAsync(new FindDepositAllowanceHistory_Filter()
                        {
                            AllowanceNameId = paymentProposal.AllowanceNameId.ToString(),
                            EmployeeId = paymentProposal.EmployeeId.ToString(),
                            DepositDate = lastDateOfSalaryMonth.Date.ToString("yyyy-MM-dd"),
                            ConditionalDepositAllowanceConfigId = paymentProposal.ConditionalDepositAllowanceConfigId.ToString(),
                            EmployeeDepositAllowanceConfigId = paymentProposal.EmployeeDepositAllowanceConfigId.ToString(),
                            IncomingFlag = paymentProposal.IncomingFlag
                        }, user);

                        decimal depositAmount = 0;
                        if (employeeDepositAllowanceHistoriesExceptPaymentMonth != null && employeeDepositAllowanceHistoriesExceptPaymentMonth.AsList().Count > 0)
                        {
                            depositAmount = employeeDepositAllowanceHistoriesExceptPaymentMonth.Sum(i => i.Amount + i.Arrear);
                        }

                        // this month deposit amount
                        decimal currentMonthDeposit = 0;
                        if (listOfNewDepositAmount != null && listOfNewDepositAmount.Count > 0)
                        {
                            if (paymentProposal.IncomingFlag == "Conditional")
                            {
                                currentMonthDeposit = listOfNewDepositAmount.Where(i => i.ConditionalDepositAllowanceConfigId == paymentProposal.ConditionalDepositAllowanceConfigId).Sum(i => i.Amount);
                            }
                            else if (paymentProposal.IncomingFlag == "Individual")
                            {
                                currentMonthDeposit = listOfNewDepositAmount.Where(i => i.EmployeeDepositAllowanceConfigId == paymentProposal.EmployeeDepositAllowanceConfigId).Sum(i => i.Amount);
                            }
                        }


                        // remain
                        var remainDepositAmount = depositAmount - tillDepositPaid;
                        var totalAmountIncludingThisMonth = remainDepositAmount + currentMonthDeposit;

                        // What we need for this currest proposal
                        if (paymentProposal.PaymentApproach.ToLower() == "proposal amount")
                        {
                            // Proposal amount (Less or Equal)
                            if (paymentProposal.ProposalAmount <= totalAmountIncludingThisMonth)
                            {
                                paymentProposal.PayableAmount = paymentProposal.ProposalAmount;
                                paymentProposal.DisbursedAmount = paymentProposal.ProposalAmount;
                            }
                            else if (paymentProposal.ProposalAmount > totalAmountIncludingThisMonth)
                            {
                                paymentProposal.PayableAmount = totalAmountIncludingThisMonth;
                                paymentProposal.DisbursedAmount = totalAmountIncludingThisMonth;
                            }
                            else
                            {
                                paymentProposal.PayableAmount = 0;
                            }
                        }
                        if (paymentProposal.PaymentApproach.ToLower() == "exact proposal amount")
                        {
                            if (paymentProposal.ProposalAmount <= totalAmountIncludingThisMonth)
                            {
                                paymentProposal.PayableAmount = paymentProposal.ProposalAmount;
                                paymentProposal.DisbursedAmount = paymentProposal.ProposalAmount;
                            }
                            else
                            {
                                paymentProposal.PayableAmount = 0;
                                paymentProposal.DisbursedAmount = 0;
                            }
                        }
                        if (paymentProposal.PaymentApproach.ToLower() == "remaining deposit amount")
                        {
                            if (remainDepositAmount > 0)
                            {
                                paymentProposal.PayableAmount = remainDepositAmount;
                                paymentProposal.DisbursedAmount = remainDepositAmount;
                            }
                            else
                            {
                                paymentProposal.PayableAmount = 0;
                                paymentProposal.DisbursedAmount = 0;
                            }
                        }
                        if (paymentProposal.PaymentApproach.ToLower() == "remaining deposit amount with current month")
                        {
                            if (totalAmountIncludingThisMonth > 0)
                            {
                                paymentProposal.PayableAmount = totalAmountIncludingThisMonth;
                                paymentProposal.DisbursedAmount = totalAmountIncludingThisMonth;
                            }
                            else
                            {
                                paymentProposal.PayableAmount = 0;
                                paymentProposal.DisbursedAmount = 0;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryBusiness", "ThisMonthEmployeeDepositAllowanceCalculationInSalaryAsync", user);
            }
            return thisMonthDepositPaymentProposals;
        }
    }
}
