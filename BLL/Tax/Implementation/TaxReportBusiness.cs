using Dapper;
using BLL.Base.Interface;
using BLL.Tax.Interface;
using BLL.Administration.Interface;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Report;
using Shared.Services;
using System.Data;
using DAL.Context.Payroll;
using Shared.Control_Panel.ViewModels;
using BLL.Asset.Implementation.Dashboard;
using BLL.Employee.Interface.Info;
using Shared.Payroll.Domain.Tax;

namespace BLL.Tax.Implementation
{
    public class TaxReportBusiness : ITaxReportBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly IInfoBusiness _employeeBusiness;

        public TaxReportBusiness(IDapperData dapper, IInfoBusiness employeeBusiness, PayrollDbContext payrollDbContext, ISysLogger sysLogger, IBranchInfoBusiness branchInfoBusiness)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _employeeBusiness = employeeBusiness;
            _payrollDbContext = payrollDbContext;
            _branchInfoBusiness = branchInfoBusiness;
        }
        public async Task<TaxCardMaster> TaxCardAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, bool? isDisbursed, AppUser user)
        {
            TaxCardMaster taxCardMaster = new TaxCardMaster();
            try
            {
                var sp_name = "sp_Payroll_RptTaxCard";
                var parameters1 = new DynamicParameters();
                parameters1.Add("EmployeeId", employeeId);
                parameters1.Add("TaxProcessId", 0);
                parameters1.Add("SalaryMonth", month);
                parameters1.Add("SalaryYear", year);
                parameters1.Add("IsDisbursed", isDisbursed);
                parameters1.Add("FiscalYearId", 0);
                parameters1.Add("BranchId", 0);
                parameters1.Add("CompanyId", user.CompanyId);
                parameters1.Add("OrganizationId", user.OrganizationId);
                parameters1.Add("ExecutionFlag", "TaxCardInfo");
                taxCardMaster.TaxCardInfo = await _dapper.SqlQueryListAsync<TaxCardInfo>(user.Database, sp_name, parameters1, CommandType.StoredProcedure);

                if (taxCardMaster.TaxCardInfo.FirstOrDefault() != null)
                {
                    taxProcessId = taxCardMaster.TaxCardInfo.FirstOrDefault().TaxProcessId;
                    fiscalYearId = taxCardMaster.TaxCardInfo.FirstOrDefault().TaxProcessId;
                    var branchId = taxCardMaster.TaxCardInfo.FirstOrDefault().BranchId;

                    if (branchId != null && branchId > 0)
                    {
                        var branch = await _branchInfoBusiness.GetBranchByIdAsync(branchId ?? 0, user);
                        taxCardMaster.TaxCardInfo.FirstOrDefault().BranchName = branch.BranchName;
                    }

                    if (taxProcessId > 0)
                    {
                        var parameters2 = new DynamicParameters();
                        parameters2.Add("EmployeeId", employeeId);
                        parameters2.Add("TaxProcessId", taxProcessId);
                        parameters2.Add("SalaryMonth", month);
                        parameters2.Add("SalaryYear", year);
                        parameters2.Add("FiscalYearId", fiscalYearId);
                        parameters2.Add("CompanyId", user.CompanyId);
                        parameters2.Add("OrganizationId", user.OrganizationId);
                        parameters2.Add("ExecutionFlag", "TaxCardDetail");
                        taxCardMaster.TaxCardDetails = await _dapper.SqlQueryListAsync<TaxCardDetail>(user.Database, sp_name, parameters2, CommandType.StoredProcedure);

                        var parameters3 = new DynamicParameters();
                        parameters3.Add("EmployeeId", employeeId);
                        parameters3.Add("TaxProcessId", taxProcessId);
                        parameters3.Add("SalaryMonth", month);
                        parameters3.Add("SalaryYear", year);
                        parameters3.Add("FiscalYearId", fiscalYearId);
                        parameters3.Add("BranchId", 0);
                        parameters3.Add("CompanyId", user.CompanyId);
                        parameters3.Add("OrganizationId", user.OrganizationId);
                        parameters3.Add("ExecutionFlag", "TaxCardSlab");
                        taxCardMaster.TaxCardSlabs = await _dapper.SqlQueryListAsync<TaxCardSlab>(user.Database, sp_name, parameters3, CommandType.StoredProcedure);
                    }
                }


            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReportBusiness", "TaxCardAsync", user);
            }
            return taxCardMaster;
        }
        public async Task<DataTable> TaxCardInfoAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, bool? isDisbursed, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_RptTaxCard";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("TaxProcessId", 0.ToString());
                parameters.Add("IsDisbursed", isDisbursed == null ? null : isDisbursed == true ? "1" : "0");
                parameters.Add("SalaryMonth", month.ToString());
                parameters.Add("SalaryYear", year.ToString());
                parameters.Add("FiscalYearId", 0.ToString());
                parameters.Add("BranchId", 0.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "TaxCardInfo");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReportBusiness", "TaxCardInfoAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> TaxCardDetailAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_RptTaxCard";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("TaxProcessId", taxProcessId.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("SalaryMonth", month.ToString());
                parameters.Add("SalaryYear", year.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "TaxCardDetail");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReportBusiness", "TaxCardDetailAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> TaxCardSlabAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_RptTaxCard";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("TaxProcessId", taxProcessId.ToString());
                parameters.Add("SalaryMonth", month.ToString());
                parameters.Add("SalaryYear", year.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("BranchId", 0.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "TaxCardSlab");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReportBusiness", "TaxCardSlabAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> TaxChallanAsync(long employeeId, long fiscalYearId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var query = $@"SELECT CHL.TaxChallanId,CHL.TaxMonth,CHL.TaxYear,CHL.ChallanNumber,CHL.ChallanDate,CHL.DepositeBank,CHL.DepositeBranch,
	            CHL.Amount,CHL.EmployeeId,
                TaxDeducted=(SELECT ActualTaxDeductionAmount FROM Payroll_EmployeeTaxProcess 
                Where EmployeeId=CHL.EmployeeId AND SalaryMonth=CHL.[Month] AND SalaryYear=CHL.[TaxYear] AND FiscalYearId=CHL.FiscalYearId) 
                FROM Payroll_TaxChallan CHL
	            INNER JOIN HR_EmployeeInformation EMP ON CHL.EmployeeId = EMP.EmployeeId AND CHL.CompanyId= EMP.CompanyId
	            INNER JOIN Payroll_FiscalYear FY ON CHL.FiscalYearId= FY.FiscalYearId
	            Where 1=1
	            AND EMP.EmployeeId=@EmployeeId
	            AND CHL.FiscalYearId=@FiscalYearId
	            AND CHL.CompanyId=@CompanyId
	            AND CHL.OrganizationId=@OrganizationId 
                Order By 
				Case WHEN CHL.[Month] IS NOT NULL AND CHL.[Month] > 0 THEN dbo.fnGetFirstDateOfAMonth(CHL.TaxYear,CHL.[Month]) END ASC,
				Case WHEN CHL.[Month] IS NULL OR CHL.[Month] <= 0 THEN CHL.ChallanDate END ASC";

                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReportBusiness", "TaxChallanAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> TaxCardInfoFY22_23(AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_PWCFY22_23_TaxCard_Info";
                var param = new Dictionary<string, string>();
                param.Add("ExecutionFlag", "Info");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReportBusiness", "TaxCardInfoFY22_23", user);
            }
            return dataTable;
        }
        public async Task<DataTable> TaxChallanFY22_23(AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_PWCFY22_23_TaxCard_Info";
                var parameters = new Dictionary<string, string>();
                parameters.Add("ExecutionFlag", "Challan");

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReportBusiness", "TaxChallanFY22_23", user);
            }
            return dataTable;
        }
        public async Task<DataTable> TaxDetailFY22_23(string employeeCode, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_PWCFY22_23_TaxCard_Info";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeCode", employeeCode);
                parameters.Add("ExecutionFlag", "Detail");

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReportBusiness", "TaxDetailFY22_23", user);
            }
            return dataTable;
        }

        /// <summary>
        /// GetTaxSheetDetailsReport
        /// </summary>
        /// <param name="employeeId"></param>
        /// <param name="fiscalYearId"></param>
        /// <param name="salaryMonth"></param>
        /// <param name="salaryYear"></param>
        /// <param name="salaryProcessId"></param>
        /// <param name="salaryProcessDetailId"></param>
        /// <param name="user"></param>
        /// <returns></returns>

        public async Task<DataTable> GetTaxSheetDetailsReport(long employeeId, long fiscalYearId, short salaryMonth, short salaryYear, long salaryProcessId, long salaryProcessDetailId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = string.Format(@"sp_Payroll_Income_Tax_Sheet_With_Calculation");
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("EmployeeId", employeeId.ToString());
                keyValuePairs.Add("FiscalYearId", fiscalYearId.ToString());
                keyValuePairs.Add("SalaryMonth", salaryMonth.ToString());
                keyValuePairs.Add("SalaryYear", salaryYear.ToString());
                keyValuePairs.Add("SalaryProcessId", salaryProcessId.ToString());
                keyValuePairs.Add("SalaryProcessDetailId", salaryProcessDetailId.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveSystemException(ex, user.Database, "ITaxBusiness", "GetTaxSheetDetailsReport", user.UserId, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return dataTable;
        }
        /// Added by Nur Vai 
        public async Task<ExecutionStatus> UploadActaulTaxDeductionValidatorAsync(List<ActualTaxDeductionDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            //int count = 0;
            try
            {
                List<long> salaryProcesses = new List<long>();
                using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        bool isTrue = true;
                        foreach (var item in model)
                        {
                            var employee = await _employeeBusiness.GetEmployeeInformationByCode(item.EmployeeCode, user);
                            if (employee != null)
                            {
                                var taxProcessInfoInDb = _payrollDbContext.Payroll_EmployeeTaxProcess.FirstOrDefault(i =>
                                 i.EmployeeId == employee.EmployeeId
                                 && i.SalaryMonth == item.SalaryMonth
                                 && i.SalaryYear == item.SalaryYear
                                 && i.CompanyId == user.CompanyId
                                 && i.OrganizationId == user.OrganizationId);

                                var salaryProcessDetailInDb = _payrollDbContext.Payroll_SalaryProcessDetail.FirstOrDefault(i =>
                                i.EmployeeId == employee.EmployeeId
                                && i.SalaryMonth == item.SalaryMonth
                                && i.SalaryYear == item.SalaryYear
                                && i.CompanyId == user.CompanyId
                                && i.OrganizationId == user.OrganizationId
                                );



                                if (taxProcessInfoInDb != null && salaryProcessDetailInDb != null)
                                {
                                    var salaryProcessInList = salaryProcesses.FirstOrDefault(i => i == salaryProcessDetailInDb.SalaryProcessId);
                                    if (salaryProcessInList == 0)
                                    {
                                        salaryProcesses.Add(salaryProcessDetailInDb.SalaryProcessId);
                                    }


                                    var actualTaxDeductionInDb = _payrollDbContext.Payroll_ActualTaxDeduction.FirstOrDefault(i =>
                               i.EmployeeId == employee.EmployeeId
                               && i.SalaryMonth == item.SalaryMonth
                               && i.SalaryYear == item.SalaryYear
                               && i.CompanyId == user.CompanyId
                               && i.OrganizationId == user.OrganizationId
                               );

                                    if (actualTaxDeductionInDb != null)
                                    {
                                        actualTaxDeductionInDb.ActualTaxAmount = item.ActualTaxAmount;
                                        _payrollDbContext.Payroll_ActualTaxDeduction.Update(actualTaxDeductionInDb);
                                    }
                                    else
                                    {
                                        ActualTaxDeduction actualTaxDeduction = new ActualTaxDeduction()
                                        {
                                            ActualTaxAmount = item.ActualTaxAmount,
                                            CompanyId = user.CompanyId,
                                            OrganizationId = user.OrganizationId,
                                            SalaryMonth = item.SalaryMonth,
                                            SalaryYear = item.SalaryYear,
                                            BranchId = employee.BranchId,
                                            CreatedBy = user.ActionUserId,
                                            CreatedDate = DateTime.Now,
                                            EmployeeId = employee.EmployeeId,
                                            FiscalYearId = taxProcessInfoInDb.FiscalYearId,
                                            IsApproved = true,
                                            StateStatus = StateStatus.Approved,
                                            ProjectionTax = taxProcessInfoInDb.ProjectionTax,
                                            OnceOffTax = taxProcessInfoInDb.OnceOffTax
                                        };
                                        await _payrollDbContext.Payroll_ActualTaxDeduction.AddAsync(actualTaxDeduction);
                                    }

                                    if (await _payrollDbContext.SaveChangesAsync() > 0)
                                    {
                                        taxProcessInfoInDb.ActualTaxDeductionAmount = item.ActualTaxAmount;
                                        _payrollDbContext.Payroll_EmployeeTaxProcess.Update(taxProcessInfoInDb);

                                        if (await _payrollDbContext.SaveChangesAsync() > 0)
                                        {
                                            salaryProcessDetailInDb.NetPay = ((salaryProcessDetailInDb.NetPay + salaryProcessDetailInDb.TaxDeductedAmount) - item.ActualTaxAmount);
                                            salaryProcessDetailInDb.TaxDeductedAmount = item.ActualTaxAmount;
                                            salaryProcessDetailInDb.TotalMonthlyTax = taxProcessInfoInDb.MonthlyTax;
                                            salaryProcessDetailInDb.ProjectionTax = taxProcessInfoInDb.ProjectionTax ?? 0;
                                            salaryProcessDetailInDb.OnceOffTax = taxProcessInfoInDb.OnceOffTax ?? 0;

                                            _payrollDbContext.Payroll_SalaryProcessDetail.Update(salaryProcessDetailInDb);
                                            if (await _payrollDbContext.SaveChangesAsync() == 0)
                                            {
                                                isTrue = false;
                                                throw new Exception($"Failed to update salary detail at employee id- {employee.EmployeeCode}");

                                            }
                                        }
                                        else
                                        {
                                            isTrue = false;
                                            throw new Exception($"Failed to update tax info at employee id- {employee.EmployeeCode}");

                                        }
                                    }
                                    else
                                    {
                                        isTrue = false;
                                        throw new Exception($"Failed to store actual tax amount at employee id- {employee.EmployeeCode}");
                                    }
                                }
                            }
                        }
                        if (isTrue)
                        {
                            transaction.Commit();
                        }

                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        if (ex.InnerException != null)
                        {
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, ex.Message);
                        }

                    }
                }

                if (salaryProcesses.Any() && salaryProcesses != null)
                {
                    using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            bool isTrue = true;
                            foreach (var id in salaryProcesses)
                            {
                                var salaryProcessInDb = _payrollDbContext.Payroll_SalaryProcess.FirstOrDefault(i => i.SalaryProcessId == id);
                                decimal netPay = 0;
                                decimal taxDeductedAmount = 0;
                                decimal totalMonthlyTax = 0;
                                decimal projectionTax = 0;
                                decimal onceOffTax = 0;
                                if (salaryProcessInDb != null)
                                {
                                    var salaryDetails = _payrollDbContext.Payroll_SalaryProcessDetail.Where(i => i.SalaryProcessId == id).ToList();
                                    foreach (var detail in salaryDetails)
                                    {
                                        netPay = netPay + detail.NetPay;
                                        taxDeductedAmount = taxDeductedAmount + detail.TaxDeductedAmount;
                                        totalMonthlyTax = totalMonthlyTax + detail.TotalMonthlyTax;
                                        projectionTax = projectionTax + detail.ProjectionTax;
                                        onceOffTax = onceOffTax + detail.OnceOffTax;
                                    }

                                    salaryProcessInDb.TotalNetPay = netPay;
                                    salaryProcessInDb.TotalTaxDeductedAmount = taxDeductedAmount;
                                    salaryProcessInDb.TotalMonthlyTax = totalMonthlyTax;
                                    salaryProcessInDb.TotalProjectionTax = projectionTax;
                                    salaryProcessInDb.TotalOnceOffTax = onceOffTax;

                                    _payrollDbContext.Payroll_SalaryProcess.Update(salaryProcessInDb);
                                    if (await _payrollDbContext.SaveChangesAsync() == 0)
                                    {
                                        isTrue = false;
                                    }
                                }
                            }

                            if (isTrue)
                            {
                                transaction.Commit();
                                executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            if (ex.InnerException != null)
                            {
                                executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                            }
                            else
                            {
                                executionStatus = ResponseMessage.Message(false, ex.Message);
                            }
                        }

                    }
                }

                //string sqlQuery = string.Format(@"sp_Payroll_UploadActaulTaxDeductedByExcel");
                //string duplicate = string.Empty;
                //foreach (var item in model)
                //{
                //    var parameters = new DynamicParameters();
                //    parameters.Add("EmployeeCode", item.EmployeeCode);
                //    parameters.Add("SalaryMonth", item.SalaryMonth);
                //    parameters.Add("SalaryYear", item.SalaryYear);
                //    parameters.Add("UserId", user.UserId);
                //    parameters.Add("CompanyId", user.CompanyId);
                //    parameters.Add("Organizationid", user.OrganizationId);
                //    parameters.Add("ExecutionFlag", Data.Validate);

                //    var data = await _dapper.SqlQueryFirstAsync<ActualTaxDeductionDTO>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);

                //    if (data != null && data.EmployeeId > 0)
                //    {
                //        if (executionStatus == null)
                //        {
                //            executionStatus = new ExecutionStatus();
                //            executionStatus.Status = false;
                //            executionStatus.Msg = "Validation Error";
                //            executionStatus.Errors = new Dictionary<string, string>();
                //        }
                //        duplicate +=
                //            count++.ToString() + ". " + data.EmployeeCode + " has got this " + data.ActualTaxAmount + " For the month " +
                //            Utility.GetMonthName(item.SalaryMonth) + " of " +
                //            item.SalaryYear.ToString() + "<br/>";
                //    }
                //}
                //if (executionStatus != null)
                //{
                //    executionStatus.Errors.Add("duplicate", duplicate);
                //}
            }
            catch (Exception ex)
            {
                executionStatus.Status = false;
                executionStatus.Msg = ResponseMessage.SomthingWentWrong;
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadActaulTaxDeductionAsync(List<ActualTaxDeductionDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            var employeeCode = "";
            try
            {

                foreach (var item in model)
                {
                    string sqlQuery = string.Format(@"sp_Payroll_UploadActaulTaxDeductedByExcel");
                    employeeCode = item.EmployeeCode;
                    if (!Utility.IsNullEmptyOrWhiteSpace(employeeCode))
                    {
                        var parameters = new DynamicParameters();
                        parameters.Add("EmployeeCode", item.EmployeeCode);
                        parameters.Add("SalaryMonth", item.SalaryMonth);
                        parameters.Add("SalaryYear", item.SalaryYear);
                        parameters.Add("ActualTaxAmount", item.ActualTaxAmount);
                        parameters.Add("UserId", user.ActionUserId);
                        parameters.Add("CompanyId", user.CompanyId);
                        parameters.Add("Organizationid", user.OrganizationId);
                        parameters.Add("ExecutionFlag", "Upload");
                        executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, commandType: CommandType.StoredProcedure);
                    }
                }


            }
            catch (Exception ex)
            {
                executionStatus.Msg = ResponseMessage.ServerResponsedWithError;
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdateActaulTaxDeductionInSalaryAndTaxAsync(UpdateActaulTaxDeductedDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = string.Format(@"sp_Payroll_UpdateActualTaxDeductionAmountInTaxAndSalary");
                var parameters = Utility.DappperParams(model, user);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception)
            {
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            return executionStatus;
        }
        public async Task<DataTable> FinalTaxCardInfoAsync(long employeeId, long fiscalYearId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var query = $@"SELECT IY.FiscalYearId,IY.FiscalYearRange,[AssessmentYear]=IY.AssesmentYear,TAX.BranchId,
                SalaryMonth= TAX.SalaryMonth,
                SalaryYear= TAX.SalaryYear,
                TAX.EmployeeId,
                EMP.EmployeeCode,
                DTL.Gender,
                EMP.DateOfJoining,
                InWord='',
                [EmployeeName]=EMP.FullName,
                DTL.LegalName,
                [Designation]=DEG.DesignationName,
                [Department]=DPT.DepartmentName,
                TINNo=ISNULL((SELECT ISNULL(DocumentNumber,'') FROM HR_EmployeeDocument Where DocumentName='TIN' AND EmployeeId=TAX.EmployeeId),''),
                [TerminationDate]=(CASE WHEN EMP.TerminationStatus ='Approved' THEN EMP.TerminationDate ELSE NULL END),
                YearlyTaxableIncome=ISNULL(ROUND(TAX.YearlyTaxableIncome,0),0),
                TotalTaxPayable=ISNULL(ROUND(TAX.TotalTaxPayable,0),0),
                AITAmount=ISNULL(TAX.AITAmount,0),
                TaxReturnAmount =ISNULL(TAX.TaxReturnAmount,0),
                ExcessTaxPaidRefundAmount = ISNULL(TAX.ExcessTaxPaidRefundAmount,0),
                YearlyTax = ISNULL(ROUND(TAX.YearlyTax,0),0),
                PaidTotalTax = ISNULL(TAX.PaidTotalTax,0),
                ProjectionAmount= ISNULL(ROUND(TAX.ProjectionAmount,0),0),
                ProjectionTax= ISNULL(ROUND(TAX.ProjectionTax,0),0),
                OnceOffAmount= ISNULL(ROUND(TAX.OnceOffAmount,0),0),
                OnceOffTax= ISNULL(ROUND(TAX.OnceOffTax,0),0),
                MonthlyTax = ISNULL(ROUND(TAX.MonthlyTax,0),0),
                ActualTaxDeductionAmount = ISNULL(ROUND(TAX.ActualTaxDeductionAmount,0),0),
                PFContributionBothPart = ISNULL(ROUND(TAX.PFContributionBothPart,0),0),
                OtherInvestment = ISNULL(ROUND(TAX.OtherInvestment,0),0),
                ActualInvestmentMade = ISNULL(ROUND(TAX.ActualInvestmentMade,0),0),
                InvestmentRebateAmount = ISNULL(TAX.InvestmentRebateAmount,0),
                ExemptionAmountOnAnnualIncome= ISNULL(TAX.ExemptionAmountOnAnnualIncome,0),
                TAX.DefaultInvestment,TAX.DefaultRebate,
                YTD='YTD',
                SalaryMonthYear= dbo.fnGetMonthName(TAX.SalaryMonth)+'-'+CAST(TAX.SalaryYear AS NVARCHAR(10)),
                ProjectedHead='-',
                TaxableIncome= ISNULL(ROUND(TAX.TaxableIncome,0),0),
                TotalLessExemptedAmount= ISNULL(TAX.TotalLessExemptedAmount,0),
                TotalCurrentMonthAllowanceAmount= ISNULL(TAX.TotalCurrentMonthAllowanceAmount,0),
                TotalProjectedAllowanceAmount= ISNULL(TAX.TotalProjectedAllowanceAmount,0),
                TotalTillMonthAllowanceAmount= ISNULL(TAX.TotalTillMonthAllowanceAmount,0),
                TotalGrossAnnualIncome= ISNULL(ROUND(TAX.TotalGrossAnnualIncome,0),0),
                GrossTaxableIncome= ISNULL(ROUND(TAX.TotalGrossAnnualIncome,0),0)
                From Payroll_FinalTaxProcess TAX
                INNER JOIN Payroll_FiscalYear IY ON TAX.FiscalYearId = IY.FiscalYearId
                INNER JOIN Payroll_SalaryProcessDetail SPD ON TAX.SalaryProcessDetailId = SPD.SalaryProcessDetailId
                INNER JOIN HR_EmployeeInformation EMP ON EMP.EmployeeId = TAX.EmployeeId
                LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
                LEFT JOIN HR_Designations DEG ON SPD.DesignationId = DEG.DesignationId
                LEFT JOIN HR_Departments DPT ON SPD.DepartmentId = DPT.DepartmentId
                Where 1=1
                AND TAX.EmployeeId =@EmployeeId
                AND TAX.FiscalYearId =@FiscalYearId
                AND TAX.CompanyId = @CompanyId
                AND TAX.OrganizationId = @OrganizationId";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, query, parameters,commandType:CommandType.Text);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dataTable;
        }
        public async Task<DataTable> FinalTaxCardDetailAsync(long employeeId, long fiscalYearId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var query = $@"Select TAX.TaxProcessDetailId,TAX.TaxProcessId,TAX.EmployeeId,EMP.EmployeeCode,EmployeeName=EMP.FullName,
            AllowanceHeadId=0,AllowanceHeadName='',TAX.AllowanceNameId,
            AllowanceName= (CASE WHEN ALW.[Name] IS NULL THEN TAX.AllowanceName WHEN ALW.[Name] ='' THEN TAX.AllowanceName ELSE ALW.[Name] END),
            TAX.TaxItem,TAX.TillDateIncome,TAX.CurrentMonthIncome,TAX.ProjectedIncome,TAX.GrossAnnualIncome,TAX.LessExempted,TAX.TotalTaxableIncome,
            TAX.IsPerquisite,TAX.Remarks
            From Payroll_FinalTaxProcessDetail TAX
            LEFT JOIN Payroll_AllowanceName ALW ON ALW.AllowanceNameId=TAX.AllowanceNameId
            INNER JOIN HR_EmployeeInformation EMP
            ON TAX.EmployeeId=EMP.EmployeeId
            Where 1=1
            AND TAX.FiscalYearId=@FiscalYearId
            AND TAX.EmployeeId=@EmployeeId
            AND TAX.CompanyId=@CompanyId
            AND TAX.OrganizationId=@OrganizationId";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dataTable;
        }
        public async Task<DataTable> FinalTaxCardSlabAsync(long employeeId, long fiscalYearId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var query = $@"Select Slab.EmployeeTaxProcessSlabId,Slab.EmployeeId,EmployeeName='',Slab.FiscalYearId,
                IY.FiscalYearRange,Slab.IncomeTaxSlabId,Slab.ImpliedCondition,
                Slab.SlabPercentage,Slab.ParameterName,Slab.TaxableIncome,
                Slab.TaxLiability,Slab.CompanyId,Slab.OrganizationId
                From Payroll_FinalTaxProcessSlab Slab
                INNER JOIN Payroll_FiscalYear IY ON Slab.FiscalYearId=IY.FiscalYearId
                Where 1=1
                AND Slab.FiscalYearId=@FiscalYearId
                AND Slab.EmployeeId=@EmployeeId
                AND Slab.CompanyId=@CompanyId
                AND Slab.OrganizationId=@OrganizationId";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, query, parameters, CommandType.Text);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());   
            }
            return dataTable;
        }

        public async Task<DataTable> SupplementaryTaxCardInfoAsync(long employeeId, long fiscalYearId, long paymenAmountId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var query = $@"SELECT IY.FiscalYearId,IY.FiscalYearRange,[AssessmentYear]=IY.AssesmentYear,TAX.BranchId,
                SalaryMonth= TAX.PaymentMonth,
                SalaryYear= TAX.PaymentYear,
                TAX.EmployeeId,
                EMP.EmployeeCode,
                DTL.Gender,
                EMP.DateOfJoining,
                InWord='',
                [EmployeeName]=EMP.FullName,
                [Designation]=DEG.DesignationName,
                [Department]=DPT.DepartmentName,
                TINNo=ISNULL((SELECT ISNULL(DocumentNumber,'') FROM HR_EmployeeDocument Where DocumentName='TIN' AND EmployeeId=TAX.EmployeeId),''),
                YearlyTaxableIncome=ISNULL(ROUND(TAX.YearlyTaxableIncome,0),0),
                TotalTaxPayable=ISNULL(ROUND(TAX.TotalTaxPayable,0),0),
                AITAmount=ISNULL(TAX.AITAmount,0),
                TaxReturnAmount =ISNULL(TAX.TaxReturnAmount,0),
                ExcessTaxPaidRefundAmount = ISNULL(TAX.ExcessTaxPaidRefundAmount,0),
                YearlyTax = ISNULL(ROUND(TAX.YearlyTax,0),0),
                PaidTotalTax = ISNULL(TAX.PaidTotalTax,0),
                ProjectionAmount= ISNULL(ROUND(TAX.ProjectionAmount,0),0),
                ProjectionTax= ISNULL(ROUND(TAX.ProjectionTax,0),0),
                OnceOffAmount= ISNULL(ROUND(TAX.OnceOffAmount,0),0),
                OnceOffTax= ISNULL(ROUND(TAX.OnceOffTax,0),0),
                MonthlyTax = ISNULL(ROUND(TAX.MonthlyTax,0),0),
                ActualTaxDeductionAmount = ISNULL(ROUND(TAX.ActualTaxDeductionAmount,0),0),
                PFContributionBothPart = ISNULL(ROUND(TAX.PFContributionBothPart,0),0),
                OtherInvestment = ISNULL(ROUND(TAX.OtherInvestment,0),0),
                ActualInvestmentMade = ISNULL(ROUND(TAX.ActualInvestmentMade,0),0),
                InvestmentRebateAmount = ISNULL(TAX.InvestmentRebateAmount,0),
                ExemptionAmountOnAnnualIncome= ISNULL(TAX.ExemptionAmountOnAnnualIncome,0),
                DefaultInvestment =0,DefaultRebate=0,
                YTD='YTD',
                SalaryMonthYear= dbo.fnGetMonthName(TAX.PaymentMonth)+'-'+CAST(TAX.PaymentYear AS NVARCHAR(10)),
                ProjectedHead='-',
                TaxableIncome= ISNULL(ROUND(TAX.TaxableIncome,0),0),
                TotalLessExemptedAmount= ISNULL(TAX.TotalLessExemptedAmount,0),
                TotalCurrentMonthAllowanceAmount= ISNULL(TAX.TotalCurrentMonthAllowanceAmount,0),
                TotalProjectedAllowanceAmount= ISNULL(TAX.TotalProjectedAllowanceAmount,0),
                TotalTillMonthAllowanceAmount= ISNULL(TAX.TotalTillMonthAllowanceAmount,0),
                TotalGrossAnnualIncome= ISNULL(ROUND(TAX.TotalGrossAnnualIncome,0),0),
                GrossTaxableIncome= ISNULL(ROUND(TAX.TotalGrossAnnualIncome,0),0)
                From Payroll_SupplementaryPaymentTaxInfo TAX
                INNER JOIN Payroll_FiscalYear IY ON TAX.FiscalYearId = IY.FiscalYearId
                INNER JOIN Payroll_SupplementaryPaymentAmount SPA ON TAX.PaymentAmountId = SPA.PaymentAmountId
                --INNER JOIN Payroll_SalaryProcessDetail SPD ON TAX.SalaryProcessDetailId = SPD.SalaryProcessDetailId
                INNER JOIN HR_EmployeeInformation EMP ON EMP.EmployeeId = TAX.EmployeeId
                LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
                LEFT JOIN HR_Designations DEG ON SPA.DesignationId = DEG.DesignationId
                LEFT JOIN HR_Departments DPT ON SPA.DepartmentId = DPT.DepartmentId
                Where 1=1
                AND TAX.EmployeeId =@EmployeeId
                AND TAX.PaymentAmountId =@PaymentAmountId
                AND TAX.FiscalYearId =@FiscalYearId
                AND TAX.CompanyId = @CompanyId
                AND TAX.OrganizationId = @OrganizationId";

                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("PaymentAmountId", paymenAmountId.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return dataTable;
        }
        public async Task<DataTable> SupplementaryTaxCardDetailAsync(long employeeId, long fiscalYearId, long paymentAmountId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var query = $@"Select TaxProcessDetailId=TAX.SupplementaryPaymentTaxDetailId,TaxProcessId=TAX.PaymentTaxInfoId,TAX.EmployeeId,
                EMP.EmployeeCode,EmployeeName=EMP.FullName,AllowanceHeadId=0,AllowanceHeadName='',TAX.AllowanceNameId,
                AllowanceName= (CASE WHEN ALW.[Name] IS NULL THEN TAX.AllowanceName WHEN ALW.[Name] ='' THEN TAX.AllowanceName ELSE ALW.[Name] END),
                TAX.TaxItem,TAX.TillDateIncome,TAX.CurrentMonthIncome,TAX.ProjectedIncome,TAX.GrossAnnualIncome,TAX.LessExempted,TAX.TotalTaxableIncome,
                TAX.IsPerquisite,TAX.Remarks
                From Payroll_SupplementaryPaymentTaxDetail TAX
                LEFT JOIN Payroll_AllowanceName ALW ON ALW.AllowanceNameId=TAX.AllowanceNameId
                INNER JOIN HR_EmployeeInformation EMP
                ON TAX.EmployeeId=EMP.EmployeeId
                Where 1=1
                AND TAX.PaymentAmountId =@PaymentAmountId
                AND TAX.FiscalYearId=@FiscalYearId
                AND TAX.EmployeeId=@EmployeeId
                AND TAX.CompanyId=@CompanyId
                AND TAX.OrganizationId=@OrganizationId";

                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("PaymentAmountId", paymentAmountId.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return dataTable;
        }
        public async Task<DataTable> SupplementaryTaxCardSlabAsync(long employeeId, long fiscalYearId, long paymentAmountId, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var query = $@"Select EmployeeTaxProcessSlabId=Slab.SupplementaryPaymentTaxSlabId,Slab.EmployeeId,EmployeeName='',
                Slab.FiscalYearId,IY.FiscalYearRange,Slab.IncomeTaxSlabId,Slab.ImpliedCondition,
                Slab.SlabPercentage,Slab.ParameterName,Slab.TaxableIncome,
                Slab.TaxLiability,Slab.CompanyId,Slab.OrganizationId
                From Payroll_SupplementaryPaymentTaxSlab Slab
                INNER JOIN Payroll_FiscalYear IY ON Slab.FiscalYearId=IY.FiscalYearId
                Where 1=1
                AND Slab.PaymentAmountId=@PaymentAmountId
                AND Slab.FiscalYearId=@FiscalYearId
                AND Slab.EmployeeId=@EmployeeId
                AND Slab.CompanyId=@CompanyId
                AND Slab.OrganizationId=@OrganizationId";

                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", employeeId.ToString());
                parameters.Add("FiscalYearId", fiscalYearId.ToString());
                parameters.Add("PaymentAmountId", paymentAmountId.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return dataTable;
        }
    }
}

