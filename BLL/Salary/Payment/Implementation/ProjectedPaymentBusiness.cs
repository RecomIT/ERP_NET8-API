using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using BLL.Salary.Allowance.Interface;
using BLL.Salary.Payment.Interface;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using Shared.Payroll.DTO.Payment;
using Shared.Employee.Filter.Info;
using Shared.Payroll.Domain.Salary;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.ViewModel.Payment;
using DAL.Payroll.Repository.Interface;
using Shared.Payroll.Process.Allowance;
using DAL.Context.Payroll;
using Microsoft.EntityFrameworkCore;
using Shared.Helpers;
using AutoMapper;

namespace BLL.Salary.Payment.Implementation
{
    public class ProjectedPaymentBusiness : IProjectedPaymentBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IInfoBusiness _employeeInfoBusiness;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly IProjectedPaymentRepository _projectedPaymentRepository;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly IMapper _mapper;

        public ProjectedPaymentBusiness(
            ISalaryReviewBusiness salaryReviewBusiness,
            IInfoBusiness employeeInfoBusiness,
            ISysLogger sysLogger,
            IDapperData dapper,
            ISalaryProcessBusiness salaryProcessBusiness,
            IAllowanceNameBusiness allowanceNameBusiness,
            IProjectedPaymentRepository projectedPaymentRepository,
            IMapper mapper,
            PayrollDbContext payrollDbContext)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _salaryReviewBusiness = salaryReviewBusiness;
            _salaryProcessBusiness = salaryProcessBusiness;
            _employeeInfoBusiness = employeeInfoBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _projectedPaymentRepository = projectedPaymentRepository;
            _payrollDbContext = payrollDbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeProjectedPaymentViewModel>> GetEmployeeProjectedPaymentInfosForProcessAsync(EmployeeProjectedPayment_Filter filter, AppUser user)
        {
            filter.EmployeeId = filter.EmployeeId.IsNullEmptyOrWhiteSpace() ? "0" : filter.EmployeeId;
            IEnumerable<EmployeeProjectedPaymentViewModel> list = new List<EmployeeProjectedPaymentViewModel>();
            try
            {
                var query = $@"SELECT EPA.ProjectedAllowanceId,EPA.ProjectedAllowanceCode,EPA.PaymentYear,EPA.PaymentMonth,
	PaymentMonthName=dbo.fnGetMonthName(EPA.PaymentMonth),EPA.AllowanceNameId,EI.EmployeeCode,EPA.EmployeeId,
	EmployeeName=EI.FullName,[AllowanceName]=ALW.[Name],EPA.BaseOfPayment,EPA.[Percentage],EPA.Amount,
	EPA.PayableAmount,EPA.DisbursedAmount,EPA.TaxAmount,EPA.StateStatus,EPA.IsDisbursed,FY.FiscalYearId,FY.FiscalYearRange
	FROM Payroll_EmployeeProjectedAllowance EPA
	INNER JOIN HR_EmployeeInformation EI ON EPA.EmployeeId=EI.EmployeeId
	LEFT JOIN HR_EmployeeDetail ED ON EI.EmployeeId=ED.EmployeeId
	INNER JOIN Payroll_AllowanceName ALW ON EPA.AllowanceNameId= ALW.AllowanceNameId
	INNER JOIN Payroll_FiscalYear FY ON EPA.FiscalYearId = FY.FiscalYearId
	Where 1=1
	AND (@ProjectedAllowanceId IS NULL OR @ProjectedAllowanceId=0 OR EPA.ProjectedAllowanceId=@ProjectedAllowanceId)
    AND (@AllowanceNameId IS NULL OR @AllowanceNameId=0 OR EPA.AllowanceNameId=@AllowanceNameId)
    AND (@Reason IS NULL OR @Reason = '' OR EPA.AllowanceReason=@Reason)
	AND (@EmployeeId IS NULL OR @EmployeeId = 0 OR EPA.EmployeeId=@EmployeeId)
    AND (@PayableYear IS NULL OR @PayableYear = 0 OR EPA.PayableYear=@PayableYear)
	AND (EPA.FiscalYearId=@FiscalYearId)
	AND (EPA.StateStatus='Approved')
	AND (EPA.CompanyId=@CompanyId)
	AND (EPA.OrganizationId=@OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user, new string[] { "PaymentMonth", "PaymentYear", "StateStatus", "ProjectedAllowanceCode" }, addUserId: false);
                list = await _dapper.SqlQueryListAsync<EmployeeProjectedPaymentViewModel>(user.Database, query, parameters, CommandType.Text);

                foreach (var item in list)
                {
                    long salaryReviewId = 0;
                    var getEmployeeLastSalaryProcessedIds = await _salaryProcessBusiness.GetEmployeeLastSalaryProcessedReviewIdsAsync(item.EmployeeId, item.FiscalYearId, user);
                    if (getEmployeeLastSalaryProcessedIds.Count > 0)
                    {
                        salaryReviewId = getEmployeeLastSalaryProcessedIds.Last();
                    }
                    else
                    {
                        var lastSalaryReviewInfo = await _salaryReviewBusiness.GetLastSalaryReviewInfoByEmployeeAsync(item.EmployeeId, user);
                        if (lastSalaryReviewInfo != null)
                        {
                            salaryReviewId = lastSalaryReviewInfo.SalaryReviewInfoId;
                        }
                    }

                    IEnumerable<SalaryReviewDetail> salaryReviewDetails = new List<SalaryReviewDetail>();
                    salaryReviewDetails = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(salaryReviewId, user);

                    if (salaryReviewDetails.Count() > 0)
                    {
                        if (item.BaseOfPayment.ToLower() == "basic")
                        {
                            var basicAllowanceInfo = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter()
                            {
                                AllowanceFlag = "Basic"
                            }, user)).FirstOrDefault();
                            if (basicAllowanceInfo != null)
                            {
                                var basicAllowanceAmountInfo = salaryReviewDetails.FirstOrDefault(i => i.AllowanceNameId == basicAllowanceInfo.AllowanceNameId);
                                if (basicAllowanceAmountInfo != null)
                                {
                                    item.PayableAmount = Math.Round(basicAllowanceAmountInfo.CurrentAmount * (item.Percentage.Value / 100));
                                    item.DisbursedAmount = item.PayableAmount;
                                }
                            }
                        }
                        else
                        {
                            if (item.BaseOfPayment.ToLower() == "gross")
                            {
                                var grossAmount = salaryReviewDetails.Sum(i => i.CurrentAmount);
                                item.PayableAmount = Math.Round(grossAmount * (item.Percentage.Value / 100));
                                item.DisbursedAmount = item.PayableAmount;
                            }
                            else
                            {
                                item.PayableAmount = item.Amount;
                                item.DisbursedAmount = item.PayableAmount;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "GetEmployeeProjectedPaymentInfosAsync", user);
            }
            return list;
        }
        public async Task<DBResponse<EmployeeProjectedPaymentViewModel>> GetEmployeeProjectedPaymentsAsync(EmployeeProjectedPayment_Filter filter, AppUser user)
        {
            DBResponse<EmployeeProjectedPaymentViewModel> dbResponse = new DBResponse<EmployeeProjectedPaymentViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = $@"WITH Data_CTE AS(
	SELECT EPA.ProjectedAllowanceId,ProjectedAllowanceCode=('ID-' + FORMAT(ProjectedAllowanceId,'0000000')),
    EPA.AllowanceReason,EPA.PayableYear,EPA.PaymentYear,EPA.PaymentMonth,
	PaymentMonthName=dbo.fnGetMonthName(EPA.PaymentMonth),EPA.AllowanceNameId,EI.EmployeeCode,
	EmployeeName=EI.FullName,[AllowanceName]=ALW.[Name],EPA.BaseOfPayment,EPA.[Percentage],EPA.Amount,
	EPA.PayableAmount,EPA.DisbursedAmount,EPA.TaxAmount,EPA.StateStatus,EPA.IsDisbursed,EPA.FiscalYearId,FY.FiscalYearRange
	FROM Payroll_EmployeeProjectedAllowance EPA
	INNER JOIN HR_EmployeeInformation EI ON EPA.EmployeeId=EI.EmployeeId
	LEFT JOIN HR_EmployeeDetail ED ON EI.EmployeeId=ED.EmployeeId
	INNER JOIN Payroll_AllowanceName ALW ON EPA.AllowanceNameId= ALW.AllowanceNameId
	INNER JOIN Payroll_FiscalYear FY ON EPA.FiscalYearId = FY.FiscalYearId
	Where 1=1
	AND (@ProjectedAllowanceId IS NULL OR @ProjectedAllowanceId='' OR EPA.ProjectedAllowanceId=@ProjectedAllowanceId)
	AND (@ProjectedAllowanceCode IS NULL OR @ProjectedAllowanceCode='' OR EPA.ProjectedAllowanceCode=@ProjectedAllowanceCode)
	AND (@Reason IS NULL OR @Reason = '' OR EPA.AllowanceReason=@Reason)
    AND (@EmployeeId IS NULL OR @EmployeeId = 0 OR EPA.EmployeeId=@EmployeeId)
	AND (@FiscalYearId IS NULL OR @FiscalYearId = 0 OR EPA.FiscalYearId=@FiscalYearId)
	AND (@PaymentMonth IS NULL OR @PaymentMonth = 0 OR EPA.PaymentMonth=@PaymentMonth)
	AND (@PaymentYear IS NULL OR @PaymentYear = 0 OR EPA.PaymentYear=@PaymentYear)
    AND (@PayableYear IS NULL OR @PayableYear = 0 OR EPA.PayableYear=@PayableYear)
	AND (@StateStatus IS NULL OR @StateStatus = 0 OR EPA.StateStatus=@StateStatus)
	AND (EPA.CompanyId=@CompanyId)
	AND (EPA.OrganizationId=@OrganizationId)
	),
	Count_CTE AS (
	SELECT COUNT(*) AS [TotalRows]
	FROM Data_CTE)

	SELECT JSONData=(Select * From (SELECT *
	FROM Data_CTE
	ORDER BY 
	CASE WHEN ISNULL(@SortingCol,'') = '' THEN Data_CTE.EmployeeCode END,
	CASE WHEN @SortingCol = 'EmployeeCode' AND @SortType ='ASC' THEN EmployeeCode END ASC,
	CASE WHEN @SortingCol = 'EmployeeCode' AND @SortType ='DESC' THEN EmployeeCode END DESC
	OFFSET (CAST(@PageNumber AS INT)-1)*@PageSize ROWS
	FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.Text);
                dbResponse.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeProjectedPaymentViewModel>>(response.JSONData) ?? new List<EmployeeProjectedPaymentViewModel>();
                dbResponse.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeProjectedAllowanceBusiness", "GetEmployeeProjectedAllowancesAsync", user);
            }
            return dbResponse;
        }
        public async Task<IEnumerable<ExecutionStatus>> SaveBlukEmployeeProjectedPaymentAsync(List<EmployeeProjectedPaymentDTO> model, AppUser user)
        {
            IEnumerable<ExecutionStatus> executionStatusList = new List<ExecutionStatus>();
            try
            {
                foreach (var item in model)
                {
                    ExecutionStatus executionStatus = new ExecutionStatus();

                    var employeeInDb = (await _employeeInfoBusiness.GetEmployeeServiceDataAsync(new EmployeeService_Filter() { EmployeeCode = item.EmployeeCode }, user)).FirstOrDefault();
                    if (employeeInDb != null)
                    {
                        item.EmployeeId = employeeInDb.EmployeeId;
                    }
                    var status = await SaveEmployeeProjectedPaymentAsync(item, user);

                    executionStatus.Status = status.Status;
                    if (status.Status == true)
                    {
                        executionStatus.Msg = employeeInDb.EmployeeCode + " ~ : has been saved successfully";
                    }
                    else if (status.Status == false)
                    {
                        executionStatus.Msg = employeeInDb.EmployeeCode + " ~ : has been failed to saved";
                    }
                    executionStatusList.AsList().Add(executionStatus);
                }
            }
            catch (Exception ex)
            {
                executionStatusList.AsList().Add(ResponseMessage.Invalid());
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeProjectedAllowanceBusiness", "SaveBlukEmployeeProjectedAllowance", user);
            }
            return executionStatusList;
        }
        public async Task<ExecutionStatus> SaveEmployeeProjectedPaymentAsync(EmployeeProjectedPaymentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_EmployeeProjectedPayment_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.ProjectedAllowanceId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeProjectedAllowanceBusiness", "SaveEmployeeProjectedAllowance", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidatePaymentAsync(List<EmployeeProjectedPaymentDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            executionStatus.Status = true;
            executionStatus.Errors = new Dictionary<string, string>();
            try
            {
                var sp_name = "sp_Payroll_EmployeeProjectedPayment_Insert_Update_Delete";
                string duplicate = string.Empty;
                int itemCount = 0;
                foreach (var item in model)
                {
                    var parameters = DapperParam.AddParams(item, user, new string[] { "EmployeeCode" });
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    parameters.Add("ExecutionFlag", Data.Validate);

                    var value = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                    if (value.Status == false)
                    {
                        executionStatus.Status = false;
                        itemCount = itemCount + 1;
                        executionStatus.Errors.Add(item.EmployeeCode, value.ErrorMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.InvalidExecution);
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeProjectedAllowanceBusiness", "ValidatePaymentAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveProjectedPaymentInProcessAsync(ProjectedPaymentProcessDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                using (var transation = await _payrollDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        bool isSuccess = true;
                        var processInfo = _mapper.Map<EmployeeProjectedAllowanceProcessInfo>(model.info);
                        var code = await GenerateProjectedAllowanceProcessCodeAsync(user);
                        if (processInfo != null)
                        {
                            processInfo.HeadCount = model.payments.Count();
                            processInfo.TotalPayableAmount = model.payments.Sum(i => i.PayableAmount);
                            processInfo.TotalDisbursedAmount = model.payments.Sum(i => i.DisbursedAmount);
                            processInfo.TotalTaxAmount = 0;
                            processInfo.StateStatus = "Pending";
                            processInfo.PaymentMonth = model.info.PaymentMonth;
                            processInfo.ProcessCode = code;

                            processInfo.CreatedBy = user.ActionUserId;
                            processInfo.CreatedDate = DateTime.Now;
                            processInfo.CompanyId = user.CompanyId;
                            processInfo.OrganizationId = user.OrganizationId;

                            _payrollDbContext.Payroll_EmployeeProjectedAllowanceProcessInfo.Add(processInfo);
                            if (await _payrollDbContext.SaveChangesAsync() > 0)
                            {
                                foreach (var item in model.payments)
                                {
                                    var projectedAllowanceInDb = _payrollDbContext.Payroll_EmployeeProjectedAllowance.FirstOrDefault(i => i.ProjectedAllowanceId == item.ProjectedAllowanceId);
                                    if (projectedAllowanceInDb != null && projectedAllowanceInDb.StateStatus == StateStatus.Approved)
                                    {
                                        projectedAllowanceInDb.ShowInPayslip = model.info.ShowInPayslip;
                                        projectedAllowanceInDb.ShowInSalarySheet = model.info.ShowInSalarySheet;
                                        projectedAllowanceInDb.WithCOC = model.info.WithCOC;
                                        projectedAllowanceInDb.PayableAmount = item.PayableAmount;
                                        projectedAllowanceInDb.DisbursedAmount = item.DisbursedAmount;
                                        projectedAllowanceInDb.StateStatus = StateStatus.Processed;
                                        projectedAllowanceInDb.PaymentMonth = item.PaymentMonth;
                                        projectedAllowanceInDb.UpdatedBy = user.ActionUserId;
                                        projectedAllowanceInDb.UpdatedDate = DateTime.Now;
                                        projectedAllowanceInDb.ProjectedAllowanceProcessInfoId = processInfo.ProjectedAllowanceProcessInfoId;
                                        _payrollDbContext.Payroll_EmployeeProjectedAllowance.Update(projectedAllowanceInDb);

                                        if (await _payrollDbContext.SaveChangesAsync() == 0)
                                        {
                                            isSuccess = false;
                                            throw new Exception($"Amount status is failed to update at employee id: {item.EmployeeCode}");
                                        }
                                    }
                                    else
                                    {
                                        isSuccess = false;
                                        throw new Exception($"Employee amount already has been in a process employee id: {item.EmployeeCode}");
                                    }
                                }
                            }
                            else
                            {
                                isSuccess = false;
                                throw new Exception($"Process has been failed to save");
                            }

                        }
                        else
                        {
                            isSuccess = false;
                        }

                        if (isSuccess)
                        {
                            await transation.CommitAsync();
                            executionStatus = ResponseMessage.Message(true, ResponseMessage.Saved);
                        }
                    }
                    catch (Exception ex)
                    {
                        await transation.RollbackAsync();
                        if (ex.InnerException != null)
                        {
                            await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "SaveProjectedPaymentInProcessAsync", user);
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(true, ex.Message);
                        }

                    }
                }

                //using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                //{
                //    connection.Open();
                //    using (var transaction = connection.BeginTransaction())
                //    {
                //        var query = "";
                //        try
                //        {
                //            model.info.HeadCount = model.payments.Count();
                //            model.info.TotalPayableAmount = model.payments.Sum(i => i.PayableAmount);
                //            model.info.TotalDisbursedAmount = model.payments.Sum(i => i.DisbursedAmount);
                //            model.info.TotalTaxAmount = 0;
                //            model.info.StateStatus = "Pending";

                //            query = $@"INSERT INTO [dbo].[Payroll_EmployeeProjectedAllowanceProcessInfo](ProcessCode, HeadCount, FiscalYearId, PaymentMonth, PaymentYear, TotalAmount, TotalPayableAmount, TotalDisbursedAmount, TotalTaxAmount, StateStatus, IsApproved, IsDisbursed, ShowInPayslip, ShowInSalarySheet, CreatedBy, CreatedDate, OrganizationId, CompanyId, BranchId)
                //            OUTPUT INSERTED.*
                //            VALUES(@ProcessCode,@HeadCount,@FiscalYearId,@PaymentMonth,@PaymentYear,@TotalAmount,@TotalPayableAmount,@TotalDisbursedAmount,@TotalTaxAmount,'Disbursed',1,1,@ShowInPayslip,@ShowInSalarySheet,@UserId,GETDATE(),@OrganizationId,@CompanyId,0)";

                //            var parameters = DapperParam.AddParamsInKeyValuePairs(model.info, user);
                //            var code = await GenerateProjectedAllowanceProcessCodeAsync(user);
                //            parameters.Add("@ProcessCode", code);
                //            var processInfo = await connection.QueryFirstAsync<EmployeeProjectedAllowanceProcessInfo>(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                //            if (processInfo != null && processInfo.ProjectedAllowanceProcessInfoId > 0)
                //            {
                //                var rawAffected = 0;
                //                foreach (var item in model.payments)
                //                {
                //                    parameters.Clear();

                //                    query = $@"Update Payroll_EmployeeProjectedAllowance
                //                    SET StateStatus='Disbursed',FiscalYearId=@FiscalYearId,PayableAmount=@PayableAmount,DisbursedAmount=@DisbursedAmount,
                //                    ShowInPayslip=@ShowInPayslip, ShowInSalarySheet=@ShowInSalarySheet,UpdatedBy=@UserId,UpdatedDate=GETDATE(),PaymentMonth=@PaymentMonth
                //                    ,PaymentYear=@PaymentYear,ProjectedAllowanceProcessInfoId=@ProjectedAllowanceProcessInfoId,IsDisbursed=1
                //                    Where ProjectedAllowanceId=@ProjectedAllowanceId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                //                    parameters = DapperParam.AddParamsInKeyValuePairs(new
                //                    {
                //                        item.FiscalYearId,
                //                        item.PayableAmount,
                //                        item.DisbursedAmount,
                //                        model.info.ShowInPayslip,
                //                        model.info.ShowInSalarySheet,
                //                        item.ProjectedAllowanceId,
                //                        model.info.PaymentMonth,
                //                        model.info.PaymentYear,
                //                        processInfo.ProjectedAllowanceProcessInfoId
                //                    }, user);

                //                    rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);
                //                }
                //                if (rawAffected == model.payments.Count)
                //                {
                //                    executionStatus.Status = true;
                //                    executionStatus.Msg = "Processed has been done";
                //                    transaction.Commit();
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //            executionStatus = ResponseMessage.Invalid();
                //            await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "SaveProjectedPaymentInProcessAsync", user);
                //        }
                //        finally { connection.Close(); }
                //    }
                //}
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "SaveProjectedPaymentInProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<EmployeeProjectedAllowanceProcessInfoViewModel>> GetEmployeeProjectedAllowanceProcessInfos(EmployeeProjectedAllowanceProcess_Filter filter, AppUser user)
        {
            DBResponse<EmployeeProjectedAllowanceProcessInfoViewModel> dbResponse = new DBResponse<EmployeeProjectedAllowanceProcessInfoViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var query = $@"WITH Data_CTE AS(SELECT EPP.*,FY.FiscalYearRange FROM Payroll_EmployeeProjectedAllowanceProcessInfo EPP
	                INNER JOIN Payroll_FiscalYear FY ON EPP.FiscalYearId= FY.FiscalYearId
	                Where 1=1
	                AND (@PaymentMonth IS NULL OR @PaymentMonth=0 OR EPP.PaymentMonth=@PaymentMonth) 
	                AND (@PaymentYear IS NULL OR @PaymentYear=0 OR EPP.PaymentYear=@PaymentYear)
	                AND (@FiscalYearId IS NULL OR @FiscalYearId=0 OR FY.FiscalYearId=@FiscalYearId)
	                AND (@StateStatus IS NULL OR @StateStatus='' OR EPP.StateStatus=@StateStatus)
	                AND (EPP.CompanyId=@CompanyId)
	                AND (EPP.OrganizationId=@OrganizationId)),Count_CTE AS (
	                SELECT COUNT(*) AS [TotalRows]
	                FROM Data_CTE)

	                SELECT JSONData=(Select * From (SELECT *
	                FROM Data_CTE
	                ORDER BY 
	                CASE WHEN ISNULL(@SortingCol,'') = '' THEN Data_CTE.ProjectedAllowanceProcessInfoId END
	                OFFSET (@PageNumber-1)*@PageSize ROWS
	                FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";

                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true, addUserId: false);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, query, parameters, CommandType.Text);
                dbResponse.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeProjectedAllowanceProcessInfoViewModel>>(response.JSONData) ?? new List<EmployeeProjectedAllowanceProcessInfoViewModel>();
                dbResponse.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "GetEmployeeProjectedAllowanceProcessInfos", user);
            }
            return dbResponse;
        }
        public async Task<string> GenerateProjectedAllowanceProcessCodeAsync(AppUser user)
        {
            try
            {
                var query = $@"(Select Case When MAX(ProcessCode) IS NULL Then 'PPI-0000000001'  ELSE ('PPI-' +RIGHT('0000000000'+
						Convert(NVarchar(50),MAX(Convert(int,SUBSTRING(ProcessCode,5,20)))+1)
					,10)) END From Payroll_EmployeeProjectedAllowanceProcessInfo
					Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                string code = await _dapper.SqlQueryFirstAsync<string>(user.Database, query, parameters, CommandType.Text);
                return code;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "GenerateProjectedAllowanceProcessCodeAsync", user);
            }
            return null;
        }
        public async Task<IEnumerable<EmployeeProjectedPayment>> GetUnProcessedProjectedAllowanceAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            IEnumerable<EmployeeProjectedPayment> list = new List<EmployeeProjectedPayment>();
            try
            {
                var query = $@"SELECT PRO.* FROM Payroll_EmployeeProjectedAllowance PRO
                INNER JOIN HR_EmployeeInformation EMP ON PRO.EmployeeId = EMP.EmployeeId
                INNER JOIN Payroll_AllowanceName ALW ON PRO.AllowanceNameId=ALW.AllowanceNameId
                Where EMP.EmployeeId=@EmployeeId AND ISNULL(PRO.IsDisbursed,0)=0 AND FiscalYearId=@FiscalYearId AND PRO.CompanyId=@CompanyId AND PRO.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<EmployeeProjectedPayment>(user.Database, query, new { EmployeeId = employeeId, FiscalYearId = fiscalYearId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return list;
        }
        public async Task<IEnumerable<AllowanceDisbursedAmount>> GetTillProcessedProjectedAllowanceAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            IEnumerable<AllowanceDisbursedAmount> list = new List<AllowanceDisbursedAmount>();
            try
            {
                var query = $@"SELECT ALW.AllowanceNameId,AllowanceName=ALW.[Name],[AllowanceFlag]=Flag,PRO.DisbursedAmount FROM Payroll_EmployeeProjectedAllowance PRO
                INNER JOIN HR_EmployeeInformation EMP ON PRO.EmployeeId = EMP.EmployeeId
                INNER JOIN Payroll_AllowanceName ALW ON PRO.AllowanceNameId=ALW.AllowanceNameId
                Where EMP.EmployeeId=@EmployeeId AND PRO.IsDisbursed=1 AND PRO.PaymentMonth<>@Month AND FiscalYearId=@FiscalYearId AND PRO.CompanyId=@CompanyId AND PRO.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<AllowanceDisbursedAmount>(user.Database, query, new { EmployeeId = employeeId, FiscalYearId = fiscalYearId, Month = month, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return list;
        }
        public async Task<IEnumerable<AllowanceDisbursedAmount>> GetThisMonthProcessedProjectedAllowanceAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            IEnumerable<AllowanceDisbursedAmount> list = new List<AllowanceDisbursedAmount>();
            try
            {
                var query = $@"SELECT ALW.AllowanceNameId,AllowanceName=ALW.[Name],[AllowanceFlag]=Flag,PRO.DisbursedAmount FROM Payroll_EmployeeProjectedAllowance PRO
                INNER JOIN HR_EmployeeInformation EMP ON PRO.EmployeeId = EMP.EmployeeId
                INNER JOIN Payroll_AllowanceName ALW ON PRO.AllowanceNameId=ALW.AllowanceNameId
                Where EMP.EmployeeId=@EmployeeId AND PRO.IsDisbursed=1 AND PRO.PaymentMonth=@Month AND FiscalYearId=@FiscalYearId AND PRO.CompanyId=@CompanyId AND PRO.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<AllowanceDisbursedAmount>(user.Database, query, new { EmployeeId = employeeId, FiscalYearId = fiscalYearId, Month = month, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return list;
        }
        public async Task<DataTable> GetProjectedPaymentReport(long projectedAllowanceProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_Payroll_ProjectedPaymentSheet";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("ProjectedAllowanceProcessInfoId", projectedAllowanceProcessInfoId.ToString());
                keyValuePairs.Add("EmployeeId", employeeId.ToString());
                keyValuePairs.Add("FiscalYearId", fiscalYearId.ToString());
                keyValuePairs.Add("PaymentMonth", paymentMonth.ToString());
                keyValuePairs.Add("PaymentYear", paymentYear.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "ProjectedPaymentBusiness", "GetProjectedPaymentReport", user.UserId, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return dataTable;
        }
        public async Task<ExecutionStatus> SaveAsync(List<EmployeeProjectedPaymentDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _projectedPaymentRepository.SaveAysnc(model, user);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                    await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "SaveAsync", user);
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, ex.Message);
                }
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeletePendingAllowanceByIdAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _projectedPaymentRepository.DeletePendingAllowanceByIdAsync(id, user);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                    await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "DeletePendingAllowanceByIdAsync", user);
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, ex.Message);
                }
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeleteApprovedAllowanceByIdAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _projectedPaymentRepository.DeleteApprovedAllowanceByIdAsync(id, user);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                    await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "DeleteApprovedAllowanceByIdAsync", user);
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, ex.Message);
                }
            }
            return executionStatus;
        }
        public async Task<EmployeeProjectedPaymentViewModel> GetProjectedAllowanceByIdAsync(long id, AppUser user)
        {
            EmployeeProjectedPaymentViewModel item = null;
            try
            {
                item = await _projectedPaymentRepository.GetProjectedAllowanceByIdAsync(id, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "DeleteApprovedAllowanceByIdAsync", user);
            }
            return item;
        }

        public async Task<ExecutionStatus> UpdateAsync(EmployeeProjectedPaymentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var itemInDb = await _payrollDbContext.Payroll_EmployeeProjectedAllowance.Where(i => i.ProjectedAllowanceId == model.ProjectedAllowanceId).FirstOrDefaultAsync();
                if (itemInDb != null)
                {
                    itemInDb.BaseOfPayment = model.BaseOfPayment;
                    itemInDb.Percentage = model.Percentage;
                    itemInDb.Amount = model.Amount;
                    itemInDb.AllowanceReason = model.AllowanceReason;
                    itemInDb.PayableYear = model.PayableYear;
                    itemInDb.UpdatedBy = user.ActionUserId;
                    itemInDb.UpdatedDate = DateTime.Now;

                    _payrollDbContext.Update(itemInDb);
                    if (await _payrollDbContext.SaveChangesAsync() > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "Item not found to update");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "UpdateAsync", user);
                executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> ApprovalAsync(ProjectedPaymentApprovalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var itemInDb = await _payrollDbContext.Payroll_EmployeeProjectedAllowance.Where(i => i.ProjectedAllowanceId == model.Id && i.StateStatus == StateStatus.Pending).FirstOrDefaultAsync();
                if (itemInDb != null)
                {
                    itemInDb.StateStatus = model.Status;
                    itemInDb.UpdatedBy = user.ActionUserId;
                    itemInDb.UpdatedDate = DateTime.Now;

                    _payrollDbContext.Update(itemInDb);
                    if (await _payrollDbContext.SaveChangesAsync() > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "Item not found");
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<string>> ListOfPaymentReasonAsync(AppUser user)
        {
            IEnumerable<string> reasons = new List<string>();
            try
            {
                reasons = await _payrollDbContext.Payroll_EmployeeProjectedAllowance
                    .Where(i => i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId && i.AllowanceReason != null && i.AllowanceReason != "")
                    .Select(i => i.AllowanceReason)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentBusiness", "ListOfPaymentReasonAsync", user);
            }
            return reasons;
        }
    }
}
