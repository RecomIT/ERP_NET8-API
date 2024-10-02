using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Services;
using DAL.Payroll.Repository.Interface;
using BLL.Salary.Payment.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.Process.Allowance;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using System.Data;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.DTO.Payment;
using DAL.Context.Payroll;
using Azure;
using AutoMapper;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Shared.Control_Panel.Domain;

namespace BLL.Salary.Payment.Implementation
{
    public class ConditionalProjectedPaymentBusiness : IConditionalProjectedPaymentBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IMapper _mapper;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly IConditionalProjectedPaymentExcludeParameterRepository _conditionalProjectedPaymentExcludeParameterRepository;
        private readonly IConditionalProjectedPaymentParameterRepository _conditionalProjectedPaymentParameterRepository;

        public ConditionalProjectedPaymentBusiness(
            IDapperData dapper,
            IConditionalProjectedPaymentExcludeParameterRepository conditionalProjectedPaymentExcludeParameterRepository,
            IConditionalProjectedPaymentParameterRepository conditionalProjectedPaymentParameterRepository,
            PayrollDbContext payrollDbContext,
            IMapper mapper,
            ISysLogger sysLogger)
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
            _payrollDbContext = payrollDbContext;
            _conditionalProjectedPaymentParameterRepository = conditionalProjectedPaymentParameterRepository;
            _conditionalProjectedPaymentExcludeParameterRepository = conditionalProjectedPaymentExcludeParameterRepository;
        }

        public async Task<IEnumerable<ConditionalProjectedPayment>> GetUnProcessedConditionalProjectedPaymentsAsync(EligibilityInConditionalProjectedPayment_Filter filter, EligibleEmployeeForTaxType employee, AppUser user)
        {
            List<ConditionalProjectedPayment> list = new List<ConditionalProjectedPayment>();
            // Approved = UnProcessed // Disbursed
            try
            {
                var query = $@"SELECT Config.*,[AllowanceName]=ALW.[Name] FROM Payroll_ConditionalProjectedPayment Config
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId = ALW.AllowanceNameId
                WHERE 1=1
                AND (Config.Gender IS NULL OR Config.Gender='' OR Config.Gender=@Gender)
                AND (Config.FiscalYearId=@FiscalYearId)
                AND (Config.JobType IS NULL OR Config.JobType='' OR Config.JobType=@JobType)
                AND (Config.Religion IS NULL OR Config.Religion='' OR Config.Religion=@Religion)
                AND (Config.Citizen IS NULL OR Config.Citizen='' OR Config.Citizen=@Citizen)
                AND (Config.MaritalStatus IS NULL OR Config.MaritalStatus='' OR Config.MaritalStatus=@MaritalStatus)
                AND (Config.PhysicalCondition IS NULL OR Config.PhysicalCondition='' OR Config.PhysicalCondition=@PhysicalCondition)
                AND (Config.StateStatus='Approved')
                AND (Config.CompanyId=@CompanyId)
                AND (Config.OrganizationId=@OrganizationId)";

                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                var data = await _dapper.SqlQueryListAsync<ConditionalProjectedPayment>(user.Database, query.Trim(), parameters);
                foreach (var item in data)
                {
                    bool IsPersonExist = true;
                    if (item.HasEmployee.HasValue)
                    {
                        if (item.HasEmployee.Value == true)
                        {
                            var types = await GetConditionalProjectedPaymentsAsync(item.Id, Convert.ToInt64(filter.FiscalYearId), ConditionalParameter.Employee, user);

                            var includedItem = await _conditionalProjectedPaymentParameterRepository.ConditionalProjectedPaymentParameterById(employee.EmployeeId, ConditionalParameter.Employee, Convert.ToInt64(filter.FiscalYearId), item.Id, user);

                            if (includedItem == null)
                            {
                                IsPersonExist = false;
                            }
                            else
                            {
                                IsPersonExist = true;
                            }
                        }
                    }

                    if (item.HasEmployeeTypes.HasValue)
                    {
                        if (item.HasEmployeeTypes.Value == true)
                        {
                            var includedItem = await _conditionalProjectedPaymentParameterRepository.ConditionalProjectedPaymentParameterById(employee.EmployeeId, ConditionalParameter.EmployeeType, Convert.ToInt64(filter.FiscalYearId), item.Id, user);

                            if (includedItem == null)
                            {
                                IsPersonExist = false;
                            }
                            else
                            {
                                IsPersonExist = true;
                            }
                        }
                    }

                    if (item.HasBranch.HasValue)
                    {
                        if (item.HasGrade.Value == true)
                        {
                        }
                    }

                    if (item.HasGrade.HasValue)
                    {
                        if (item.HasGrade.Value == true)
                        {
                        }
                    }

                    if (item.HasInternalDesignation.HasValue)
                    {
                        if (item.HasEmployee.Value == true)
                        {
                        }
                    }

                    if (item.HasDesignation.HasValue)
                    {
                        if (item.HasDesignation.Value == true)
                        {
                        }
                    }

                    if (item.HasDepartment.HasValue)
                    {
                        if (item.HasDepartment.Value == true)
                        {
                        }
                    }

                    if (item.HasSection.HasValue)
                    {
                        if (item.HasSection.Value == true)
                        {
                        }
                    }

                    if (item.HasSubSection.HasValue)
                    {
                        if (item.HasSubSection.Value == true)
                        {
                        }
                    }

                    if (item.HasSubSection.HasValue)
                    {
                        if (item.HasSubSection.Value == true)
                        {
                        }
                    }

                    if (item.HasExcludeEmployee.HasValue)
                    {
                        if (item.HasExcludeEmployee.Value == true)
                        {
                            var excludedItem = await _conditionalProjectedPaymentExcludeParameterRepository.ConditionalProjectedPaymentExcludeParameterById(employee.EmployeeId, ConditionalParameter.Employee, Convert.ToInt64(filter.FiscalYearId), item.Id, user);
                            if (excludedItem != null && excludedItem.Id > 0)
                            {
                                IsPersonExist = false;
                            }
                        }
                    }

                    //if (item.HasExcludeEmployee.HasValue) {
                    //    if (item.HasExcludeEmployee.Value == true) {
                    //        var excludedItem = await _conditionalProjectedPaymentExcludeParameterRepository.ConditionalProjectedPaymentExcludeParameterById(employee.EmployeeId, ConditionalParameter.EmployeeType, Convert.ToInt64(filter.FiscalYearId), item.Id, user);

                    //        if (excludedItem != null && excludedItem.Id > 0) {
                    //            IsPersonExist = false;
                    //        }
                    //    }
                    //}

                    if (IsPersonExist == true)
                    {
                        list.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyAllowanceConfigBusiness", "GetEmployeeMonthlyAllowanceConfigsAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<ConditionalProjectedPaymentParameter>> GetConditionalProjectedPaymentsAsync(long configId, long fiscalYearId, string flag, AppUser user)
        {
            IEnumerable<ConditionalProjectedPaymentParameter> list = new List<ConditionalProjectedPaymentParameter>();
            try
            {
                var query = $@"SELECT * FROM Payroll_ConditionalProjectedPaymentParameter
                Where 1=1
                AND Flag = @Flag
                AND FiscalYearId=@FiscalYearId
                AND ConditionalProjectedPaymentId=@ConfigId
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<ConditionalProjectedPaymentParameter>(user.Database, query.Trim(), new { Flag = flag, FiscalYearId = fiscalYearId, ConfigId = configId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentBusiness", "GetConditionalProjectedPaymentsAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<AllowanceDisbursedAmount>> GetAllowanceTillDisbursedAmountsAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            IEnumerable<AllowanceDisbursedAmount> list = new List<AllowanceDisbursedAmount>();
            try
            {
                var query = $@"SELECT ALW.AllowanceNameId,AllowanceName=ALW.[Name],detail.DisbursedAmount,[AllwoanceFlag]=ALW.Flag FROM Payroll_ConditionalProjectedPaymentDetail detail
                INNER JOIN Payroll_ConditionalProjectedPayment Info ON detail.ConditionalProjectedPaymentId=Info.Id
                INNER JOIN Payroll_AllowanceName ALW ON Info.AllowanceNameId = ALW.AllowanceNameId
                Where Info.StateStatus='Disbursed' AND Info.FiscalYearId=@FiscalYearId AND detail.EmployeeId=@EmployeeId 
                AND Info.PaymentMonth<>@Month AND Info.CompanyId=@CompanyId AND Info.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<AllowanceDisbursedAmount>(user.Database, query.Trim(), new { EmployeeId = employeeId, FiscalYearId = fiscalYearId, Month = month, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentBusiness", "GetConditionalProjectedPaymentsAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<AllowanceDisbursedAmount>> GetAllowanceThisMonthDisbursedAmountsAsync(long employeeId, long fiscalYearId, int year, int month, AppUser user)
        {
            IEnumerable<AllowanceDisbursedAmount> list = new List<AllowanceDisbursedAmount>();
            try
            {
                var query = $@"SELECT ALW.AllowanceNameId,AllowanceName=ALW.[Name],detail.DisbursedAmount,[AllwoanceFlag]=ALW.Flag FROM Payroll_ConditionalProjectedPaymentDetail detail
                INNER JOIN Payroll_ConditionalProjectedPayment Info ON detail.ConditionalProjectedPaymentId=Info.Id
                INNER JOIN Payroll_AllowanceName ALW ON Info.AllowanceNameId = ALW.AllowanceNameId
                Where Info.StateStatus='Disbursed' AND Info.FiscalYearId=@FiscalYearId AND detail.EmployeeId=@EmployeeId 
                AND Info.PaymentMonth=@Month AND Info.CompanyId=@CompanyId AND Info.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<AllowanceDisbursedAmount>(user.Database, query.Trim(), new { EmployeeId = employeeId, FiscalYearId = fiscalYearId, Month = month, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentBusiness", "GetAllowanceThisMonthDisbursedAmountsAsync", user);
            }
            return list;
        }
        public async Task<DBResponse<ConditionalProjectedPaymentViewModel>> GetConditionalProjectedPaymentsAsync(ConditionalProjected_Filter filter, AppUser user)
        {
            DBResponse<ConditionalProjectedPaymentViewModel> data = new DBResponse<ConditionalProjectedPaymentViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                filter.AllowanceNameId = Utility.TryParseLong(filter.AllowanceNameId).ToString();
                filter.FiscalYearId = Utility.TryParseLong(filter.FiscalYearId).ToString();
                var sp_name = $@"WITH Data_CTE AS(
                SELECT Config.Id,Config.Code,Config.AllowanceNameId,[AllowanceName]=ALW.[Name],Config.BaseOfPayment,Config.Amount,Config.[Percentage],Config.Reason,
              FiscalYear=FY.FiscalYearRange,Config.JobType,Config.Citizen,Config.Religion,Config.Gender,Config.PaymentMonth,Config.PaymentYear,
                Config.StateStatus,PayableAmount=ISNULL(Config.PayableAmount,0),DisbursedAmount=ISNULL(Config.DisbursedAmount,0),
                Config.PayableYear,
                [Count]=(CASE 
	            WHEN Config.StateStatus ='Disbursed' THEN (SELECT Count(*) FROM Payroll_ConditionalProjectedPaymentDetail Where ConditionalProjectedPaymentId=Config.Id)
	            WHEN Config.StateStatus ='Processed' THEN (SELECT Count(*) FROM Payroll_ConditionalProjectedPaymentDetail Where ConditionalProjectedPaymentId=Config.Id)
	            ELSE 0 END)
                FROM Payroll_ConditionalProjectedPayment Config
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId = ALW.AllowanceNameId
                INNER JOIN  Payroll_FiscalYear FY ON Config.FiscalYearId = FY.FiscalYearId
                WHERE 1=1
                AND (@AllowanceNameId IS NULL OR @AllowanceNameId =0 OR Config.AllowanceNameId=@AllowanceNameId)
                AND (@FiscalYearId IS NULL OR @FiscalYearId =0 OR Config.FiscalYearId=@FiscalYearId)
                AND (@StateStatus IS NULL OR @StateStatus ='' OR Config.StateStatus=@StateStatus)
                AND Config.CompanyId =@CompanyId AND Config.OrganizationId=@OrganizationId),
                Count_CTE AS (
                SELECT COUNT(*) AS [TotalRows]
                FROM Data_CTE)
                SELECT JSONData=(Select * From (SELECT *
                FROM Data_CTE
                ORDER BY AllowanceNameId
                OFFSET (@PageNumber-1)*@PageSize ROWS
                FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),
                TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.Text);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<ConditionalProjectedPaymentViewModel>>(response.JSONData) ?? new List<ConditionalProjectedPaymentViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentBusiness", "GetConditionalProjectedPaymentsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveAsync(ConditionalProjectedPaymentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var domain = _mapper.Map<ConditionalProjectedPayment>(model);
                        domain.CompanyId = user.CompanyId;
                        domain.OrganizationId = user.OrganizationId;
                        domain.CreatedBy = user.ActionUserId;
                        domain.StateStatus = StateStatus.Pending;
                        domain.CreatedDate = DateTime.Now;
                        _payrollDbContext.Payroll_ConditionalProjectedPayment.Add(domain);
                        if (await _payrollDbContext.SaveChangesAsync() > 0)
                        {
                            executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                            await transaction.CommitAsync();
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, "Data has been failed to save");
                            await transaction.RollbackAsync();
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, ex.Message);
                        }
                        await transaction.RollbackAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentBusiness", "GetConditionalProjectedPaymentsAsync", user);
            }
            return executionStatus ?? ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
        }
        public async Task<ExecutionStatus> ApprovalAsync(ConditionalProjectedPaymentApprovalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var itemInDb = _payrollDbContext.Payroll_ConditionalProjectedPayment.FirstOrDefault(i => i.Id == model.Id && i.StateStatus == StateStatus.Pending);
                if (itemInDb != null)
                {
                    if (itemInDb.StateStatus == StateStatus.Pending)
                    {
                        using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                        {
                            try
                            {
                                itemInDb.StateStatus = model.Status;
                                itemInDb.ApprovedBy = model.Status == StateStatus.Approved ? user.ActionUserId : null;
                                itemInDb.ApprovedDate = model.Status == StateStatus.Approved ? DateTime.Now : null;
                                itemInDb.IsApproved = (model.Status == StateStatus.Approved);

                                _payrollDbContext.Payroll_ConditionalProjectedPayment.Update(itemInDb);
                                if (await _payrollDbContext.SaveChangesAsync() > 0)
                                {
                                    await transaction.CommitAsync();
                                    executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                }
                                else
                                {
                                    await transaction.RollbackAsync();
                                    executionStatus = ResponseMessage.Message(false, "Data has been failed to updated");
                                }
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                                }
                                else
                                {
                                    executionStatus = ResponseMessage.Message(false, ex.Message);
                                }
                                await transaction.RollbackAsync();
                            }
                        }
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, "Item status is not pending");
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "Item not found");
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalProjectedPaymentBusiness", "ApprovalAsync", user);
            }
            return executionStatus ?? ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
        }
        public async Task<ConditionalProjectedPaymentViewModel> GetById(long id, AppUser user)
        {
            ConditionalProjectedPaymentViewModel data = new ConditionalProjectedPaymentViewModel();
            try
            {
                var query = $@" SELECT Config.Id,Config.Code,Config.AllowanceNameId,[AllowanceName]=ALW.[Name],Config.BaseOfPayment,Config.Amount,Config.[Percentage],Config.Reason,FiscalYear=FY.FiscalYearRange,
                Config.JobType,Config.Citizen,Config.Religion,Config.Gender,Config.PaymentMonth,Config.PaymentYear,
                Config.StateStatus,PayableAmount=ISNULL(Config.PayableAmount,0),DisbursedAmount=ISNULL(Config.DisbursedAmount,0),
                Config.PayableYear,
                [Count]=(CASE 
	            WHEN Config.StateStatus ='Disbursed' THEN (SELECT Count(*) FROM Payroll_ConditionalProjectedPaymentDetail Where ConditionalProjectedPaymentId=Config.Id)
	            WHEN Config.StateStatus ='Processed' THEN (SELECT Count(*) FROM Payroll_ConditionalProjectedPaymentDetail Where ConditionalProjectedPaymentId=Config.Id)
	            ELSE 0 END)
                FROM Payroll_ConditionalProjectedPayment Config
                INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId = ALW.AllowanceNameId
                INNER JOIN  Payroll_FiscalYear FY ON Config.FiscalYearId = FY.FiscalYearId
                WHERE 1=1 AND Config.Id=@Id AND Config.CompanyId=@CompanyId AND Config.OrganizationId=@OrganizationId";

                data = await _dapper.SqlQueryFirstAsync<ConditionalProjectedPaymentViewModel>(user.Database, query, new
                {
                    Id = id,
                    CompanyId= user.CompanyId,
                    OrganizationId = user.OrganizationId,
                });
            }
            catch (Exception ex)
            {

            }

            return data;
        }
    }
}
