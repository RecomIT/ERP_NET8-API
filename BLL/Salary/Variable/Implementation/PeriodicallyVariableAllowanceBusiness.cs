using Dapper;
using AutoMapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using DAL.Context.Payroll;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Variable;
using Shared.OtherModels.Response;
using BLL.Salary.Variable.Interface;
using Shared.OtherModels.Pagination;
using Shared.Payroll.Domain.Variable;
using Shared.Payroll.Filter.Variable;
using Shared.Payroll.ViewModel.Variable;
using System.Collections.Generic;
using DAL.Context.Employee;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;
using Microsoft.EntityFrameworkCore;

namespace BLL.Salary.Variable.Implementation
{
    public class PeriodicallyVariableAllowanceBusiness : IPeriodicallyVariableAllowanceBusiness
    {
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        public PeriodicallyVariableAllowanceBusiness(PayrollDbContext payrollDbContext, EmployeeModuleDbContext employeeModuleDbContext, IDapperData dapper, ISysLogger sysLogger, IMapper mapper)
        {
            _payrollDbContext = payrollDbContext;
            _employeeModuleDbContext = employeeModuleDbContext;
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
        }

        #region Periodically Variable Allowance
        public async Task<IEnumerable<PeriodicallyVariableAllowanceInfoViewModel>> GetPeriodicallyVariableAllowanceInfosAsync(long? id, string salaryVariableFor, string amountBaseOn, long? allowanceNameId, AppUser user)
        {
            IEnumerable<PeriodicallyVariableAllowanceInfoViewModel> data = new List<PeriodicallyVariableAllowanceInfoViewModel>();
            try
            {
                var sp_name = "sp_Payroll_PeriodicallyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("PeriodicallyVariableAllowanceInfoId", id ?? 0);
                parameters.Add("SalaryVariableFor", salaryVariableFor ?? "");
                parameters.Add("AmountBaseOn", amountBaseOn ?? "");
                parameters.Add("AllowanceNameId", allowanceNameId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);
                data = await _dapper.SqlQueryListAsync<PeriodicallyVariableAllowanceInfoViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "GetPeriodicallyVariableAllowanceInfosAsync", user);
            }
            return data;
        }
        public async Task<PeriodicallyVariableAllowanceInfoViewModel> GetPeriodicallyVariableAllowanceInfoAsync(long? id, long? allowanceNameId, AppUser user)
        {
            PeriodicallyVariableAllowanceInfoViewModel data = new PeriodicallyVariableAllowanceInfoViewModel();
            try
            {
                var sp_name = "sp_Payroll_PeriodicallyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("PeriodicallyVariableAllowanceInfoId", id);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Detail);

                data = await _dapper.SqlQueryFirstAsync<PeriodicallyVariableAllowanceInfoViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                if (data != null)
                {
                    sp_name = "sp_Payroll_PeriodicallyVariableAllowanceDetail";
                    data.PeriodicalDetails = (await _dapper.SqlQueryListAsync<PeriodicalDetails>(user.Database, sp_name, parameters, CommandType.StoredProcedure)).ToList();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "GetPeriodicallyVariableAllowanceInfosAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SavePeriodicallyVariableAllowanceInfoAsync(PeriodicallyVariableAllowanceInfo info, List<PeriodicallyVariableAllowanceDetail> details, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_PeriodicallyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryVariableFor", info.SpecifyFor);
                parameters.Add("AmountBaseOn", info.AmountBaseOn);
                parameters.Add("PrincipalAmount", info.PrincipalAmount);
                parameters.Add("Amount", info.Amount);
                parameters.Add("Percentage", info.Percentage);
                parameters.Add("DurationType", info.DurationType);
                parameters.Add("FiscalYearId", info.FiscalYearId);
                parameters.Add("EffectiveFrom", info.EffectiveFrom);
                parameters.Add("EffectiveTo", info.EffectiveTo);
                parameters.Add("StateStatus", info.StateStatus);
                parameters.Add("Remarks", info.Remarks);
                parameters.Add("AllowanceNameId", info.AllowanceNameId);
                parameters.Add("JsonData", Utility.JsonData(details));
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "GetPeriodicallyVariableAllowanceInfosAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SavePeriodicallyVariableAllowanceAsync(PeriodicallyVariableAllowanceInfo info, List<PeriodicalDetails> details, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            List<PeriodicallyVariableAllowanceDetail> list = new List<PeriodicallyVariableAllowanceDetail>();
            try
            {
                if (info.SpecifyFor == "All")
                {
                    PeriodicallyVariableAllowanceDetail data = new PeriodicallyVariableAllowanceDetail()
                    {
                        AllowanceNameId = info.AllowanceNameId,
                        SpecifyFor = info.SpecifyFor,
                        DurationType = info.DurationType,
                        FiscalYearId = info.FiscalYearId,
                        EffectiveFrom = info.EffectiveFrom,
                        EffectiveTo = info.EffectiveTo,
                        AmountBaseOn = info.AmountBaseOn,
                        PrincipalAmount = info.PrincipalAmount,
                        Amount = info.Amount,
                        StateStatus = StateStatus.Pending,
                        IsApproved = false
                    };
                }
                else
                {
                    foreach (var item in details)
                    {
                        PeriodicallyVariableAllowanceDetail data = new PeriodicallyVariableAllowanceDetail();
                        //data.PeriodicallyVariableAllowanceDetailId = item.DetailId;
                        data.AllowanceNameId = info.AllowanceNameId;
                        data.SpecifyFor = info.SpecifyFor;
                        data.EmployeeId = info.SpecifyFor == "Employee" ? item.Id : 0;
                        data.GradeId = info.SpecifyFor == "Grade" ? item.Id : 0;
                        data.DesignationId = info.SpecifyFor == "Designation" ? item.Id : 0;
                        data.DurationType = info.DurationType;
                        data.FiscalYearId = info.FiscalYearId;
                        data.EffectiveFrom = info.EffectiveFrom;
                        data.EffectiveTo = info.EffectiveTo;
                        data.AmountBaseOn = info.AmountBaseOn;
                        data.PrincipalAmount = info.PrincipalAmount;
                        data.Percentage = info.Percentage;
                        data.Amount = info.Amount;
                        data.StateStatus = StateStatus.Pending;
                        data.IsApproved = false;
                        list.Add(data);
                    }
                }
                executionStatus = await SavePeriodicallyVariableAllowanceInfoAsync(info, list, user);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "SavePeriodicallyVariableAllowanceAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SavePeriodicallyVariableAllowanceStatusAsync(long periodicallyVariableAllowanceInfoId, string status, string remarks, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_PeriodicallyVariableAllowance";
                var parameters = new DynamicParameters();
                parameters.Add("PeriodicallyVariableAllowanceInfoId", periodicallyVariableAllowanceInfoId);
                parameters.Add("StateStatus", status);
                parameters.Add("Remarks", status);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("Organizationid", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Checking);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "SavePeriodicallyVariableAllowanceStatusAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveAsync(PeriodicalAllowanceInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            using var transaction = await _payrollDbContext.Database.BeginTransactionAsync();
            try
            {
                if (model.Id == 0)
                {


                    var domain = _mapper.Map<PeriodicallyVariableAllowanceInfo>(model);
                    domain.StateStatus = StateStatus.Pending;
                    domain.IsApproved = false;
                    domain.CompanyId = user.CompanyId;
                    domain.OrganizationId = user.OrganizationId;
                    domain.CreatedBy = user.ActionUserId;
                    domain.CreatedDate = DateTime.Now;

                    domain.PeriodicallyVariableAllowanceDetails = _mapper.Map<List<PeriodicallyVariableAllowanceDetail>>(model.Details);
                    domain.PeriodicallyVariableAllowanceDetails.ToList().ForEach(item =>
                    {
                        item.StateStatus = StateStatus.Pending;
                        item.IsApproved = false;
                        item.CompanyId = user.CompanyId;
                        item.OrganizationId = user.OrganizationId;
                        item.CreatedBy = user.ActionUserId;
                        item.CreatedDate = DateTime.Now;
                    });

                    List<PrincipleAmountInfo> list = _mapper.Map<List<PrincipleAmountInfo>>(model.PrincipleAmounts);
                    await _payrollDbContext.AddRangeAsync(domain);

                    if (list.Any())
                    {
                        if ((await _payrollDbContext.SaveChangesAsync()) > 0)
                        {
                            list.ForEach(item =>
                            {
                                item.IncomingFlag = "Periodical Variable Allowance";
                                item.IncomingId = domain.Id;
                                item.CompanyId = user.CompanyId;
                                item.OrganizationId = user.OrganizationId;
                                item.CreatedBy = user.ActionUserId;
                                item.CreatedDate = DateTime.Now;
                            });

                            await _payrollDbContext.AddRangeAsync(list);
                            if ((await _payrollDbContext.SaveChangesAsync()) > 0)
                            {
                                executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                await transaction.CommitAsync();
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                            }
                        }
                    }
                    else
                    {
                        var rowCount = await _payrollDbContext.SaveChangesAsync();
                        if (rowCount > 0)
                        {
                            await transaction.CommitAsync();
                            executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Successfull);
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                        }
                    }
                }
                else
                {
                    // Update
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SaveAsync", "PeriodicallyVariableAllowanceBusiness", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<PeriodicallyVariableAllowanceInfoViewModel>> GetAllAsync(PeriodicalAllowance_Filter filter, AppUser user)
        {
            DBResponse<PeriodicallyVariableAllowanceInfoViewModel> data = new DBResponse<PeriodicallyVariableAllowanceInfoViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                string query = $@"BEGIN
                WITH Data_CTE AS(SELECT Id,SpecifyFor,AmountBaseOn,
                Amount=(Case 
                When AmountBaseOn ='Gross' OR AmountBaseOn ='Basic' THEN [Percentage]
                When AmountBaseOn ='Flat' THEN [Amount]
                When AmountBaseOn ='Principal Amount' THEN [PrincipalAmount]
                ELSE 0 END),DurationType,FiscalYearId,EffectiveFrom,EffectiveTo,
                CalculateProratedAmount,JobType,Info.AllowanceNameId,AllowanceName=ALW.[Name],
                Info.CreatedDate,
                Info.StateStatus,
                HeadCount =
                (
	                CASE
	                WHEN SpecifyFor ='Employee Wise' 
	                THEN (SELECT DISTINCT COUNT(EmployeeId) FROM Payroll_PeriodicallyVariableAllowanceDetail Where InfoId=Info.Id)

	                WHEN SpecifyFor ='Designation' 
	                THEN (SELECT DISTINCT COUNT(DesignationId) FROM Payroll_PeriodicallyVariableAllowanceDetail Where InfoId=Info.Id)

	                WHEN SpecifyFor ='Department' 
	                THEN (SELECT DISTINCT COUNT(DepartmentId) FROM Payroll_PeriodicallyVariableAllowanceDetail Where InfoId=Info.Id)

	                WHEN SpecifyFor ='Grade' 
	                THEN (SELECT DISTINCT COUNT(GradeId) FROM Payroll_PeriodicallyVariableAllowanceDetail Where InfoId=Info.Id)
	                ELSE 1 END
                )
                FROM Payroll_PeriodicallyVariableAllowanceInfo Info
                INNER JOIN Payroll_AllowanceName ALW ON info.AllowanceNameId= ALW.AllowanceNameId
                Where 1=1
                AND (@SpecifyFor IS NULL OR @SpecifyFor='' OR Info.SpecifyFor=@SpecifyFor)
                AND (@StateStatus IS NULL OR @StateStatus='' OR Info.StateStatus=@StateStatus)
                AND (@AllowanceNameId IS NULL OR @AllowanceNameId=0 OR Info.AllowanceNameId=@AllowanceNameId)
                AND Info.CompanyId=@CompanyId
                AND Info.OrganizationId=@OrganizationId),
                Count_CTE AS (
	                SELECT COUNT(*) AS [TotalRows]
	                FROM Data_CTE)
	                SELECT JSONData=(Select * From (SELECT *
	                FROM Data_CTE
	                ORDER BY ID DESC
	                OFFSET (@PageNumber-1)*@PageSize ROWS
	                FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber
                END";

                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, query, parameters, CommandType.Text);
                data.ListOfObject = JsonReverseConverter.JsonToObject2<IEnumerable<PeriodicallyVariableAllowanceInfoViewModel>>(response.JSONData) ?? new List<PeriodicallyVariableAllowanceInfoViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GetAllAsync", "PeriodicallyVariableAllowanceBusiness", user);
            }
            return data;
        }
        public async Task<PeriodicallyVariableAllowanceInfoViewModel?> GetByIdAsync(long id, AppUser user)
        {
            PeriodicallyVariableAllowanceInfoViewModel model = null;
            try
            {
                string query = $@"SELECT Id,SpecifyFor,AmountBaseOn,
                Amount=(Case 
                When AmountBaseOn ='Gross' OR AmountBaseOn ='Basic' THEN [Percentage]
                When AmountBaseOn ='Flat' THEN [Amount]
                When AmountBaseOn ='Principal Amount' THEN [PrincipalAmount]
                ELSE 0 END),DurationType,FiscalYearId,EffectiveFrom,EffectiveTo,
                CalculateProratedAmount,JobType,Info.AllowanceNameId,AllowanceName=ALW.[Name],
                Info.StateStatus,
                HeadCount =
                (
	                CASE
	                WHEN SpecifyFor ='Employee Wise' 
	                THEN (SELECT DISTINCT COUNT(EmployeeId) FROM Payroll_PeriodicallyVariableAllowanceDetail Where Id=Info.Id)

	                WHEN SpecifyFor ='Designation' 
	                THEN (SELECT DISTINCT COUNT(DesignationId) FROM Payroll_PeriodicallyVariableAllowanceDetail Where Id=Info.Id)

	                WHEN SpecifyFor ='Department' 
	                THEN (SELECT DISTINCT COUNT(DepartmentId) FROM Payroll_PeriodicallyVariableAllowanceDetail Where Id=Info.Id)

	                WHEN SpecifyFor ='Grade' 
	                THEN (SELECT DISTINCT COUNT(GradeId) FROM Payroll_PeriodicallyVariableAllowanceDetail Where Id=Info.Id)
	                ELSE 1 END
                )
                FROM Payroll_PeriodicallyVariableAllowanceInfo Info
                INNER JOIN Payroll_AllowanceName ALW ON info.AllowanceNameId= ALW.AllowanceNameId
                Where 1=1
                AND Id=@Id
                AND Info.CompanyId=@CompanyId
                AND Info.OrganizationId=@OrganizationId";
                model = await _dapper.SqlQueryFirstAsync<PeriodicallyVariableAllowanceInfoViewModel>(user.Database, query, new
                {
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "GetByIdAsync", user);
            }
            return model;
        }
        public async Task<ExecutionStatus> DeletePendingVariableAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var itemInDb = _payrollDbContext.Payroll_PeriodicallyVariableAllowanceInfo.SingleOrDefault(item => item.Id == id);
                if (itemInDb != null && itemInDb.StateStatus == StateStatus.Pending)
                {
                    var detailsInDb = _payrollDbContext.Payroll_PeriodicallyVariableAllowanceDetail.Where(item => item.InfoId == id).ToList();
                    if (itemInDb.AmountBaseOn == "Principal Amount")
                    {
                        var principalInDb = _payrollDbContext.Payroll_PrincipleAmountInfo.Where(item => item.IncomingId == id
                        && item.IncomingFlag == "Periodical Variable Allowance" && item.VariableType == "Allowance").ToList();

                        _payrollDbContext.RemoveRange(principalInDb);
                    }
                    _payrollDbContext.RemoveRange(detailsInDb);
                    _payrollDbContext.RemoveRange(itemInDb);

                    var rowCount = await _payrollDbContext.SaveChangesAsync();
                    if (rowCount > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Deleted);
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "Item not found, may be status has been changed");
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "GetByIdAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<PeriodicalHeadInfoViewModel>> GetPeriodicalHeadInfosAsync(long id, AppUser user)
        {
            IEnumerable<PeriodicalHeadInfoViewModel> list = new List<PeriodicalHeadInfoViewModel>();
            try
            {
                var infoInDb = _payrollDbContext.Payroll_PeriodicallyVariableAllowanceInfo.SingleOrDefault(item => item.Id == id);
                if (infoInDb != null && infoInDb.Id > 0)
                {
                    var query = $@"SELECT *,
                    ItemText=
                    (
	                    CASE 
	                    WHEN SpecifyFor = 'Employee Wise' THEN (SELECT FullName FROM HR_EmployeeInformation Where EmployeeId=ItemId)
	                    WHEN SpecifyFor = 'Designation' THEN (SELECT DesignationName FROM HR_Designations Where DesignationId=ItemId)
	                    WHEN SpecifyFor = 'Department' THEN (SELECT DepartmentName FROM HR_Departments Where DepartmentId=ItemId)
	                    WHEN SpecifyFor = 'Grade' THEN (SELECT GradeName FROM HR_Grades Where GradeId=ItemId)
	                    ELSE '' END
                    )
                    FROM (SELECT DISTINCT InfoId,SpecifyFor,DetailId=0,
                    ItemId = (CASE 
	                    WHEN SpecifyFor = 'Employee Wise' THEN ISNULL(EmployeeId,0)
	                    WHEN SpecifyFor = 'Designation' THEN ISNULL(DesignationId,0)
	                    WHEN SpecifyFor = 'Department' THEN ISNULL(DesignationId,0)
	                    WHEN SpecifyFor = 'Grade' THEN ISNULL(GradeId,0)
	                    ELSE 0 END)

                    FROM Payroll_PeriodicallyVariableAllowanceDetail
                    Where InfoId=@InfoId 
                    AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                    )tbl";

                    list = await _dapper.SqlQueryListAsync<PeriodicalHeadInfoViewModel>(user.Database, query, new
                    {
                        InfoId = id,
                        CompanyId = user.CompanyId,
                        OrganizationId = user.OrganizationId
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "GetPeriodicalHeadInfosAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<PrincipleAmountInfoViewModel>> GetPendingPrincipleAmountInfosAsync(long id, AppUser user)
        {
            IEnumerable<PrincipleAmountInfoViewModel> list = new List<PrincipleAmountInfoViewModel>();
            try
            {
                var infoInDb = _payrollDbContext.Payroll_PeriodicallyVariableAllowanceInfo.SingleOrDefault(item => item.Id == id);
                if (infoInDb != null && infoInDb.StateStatus == StateStatus.Pending)
                {
                    var query = $@"SELECT DISTINCT [Month],[Year],[Amount] FROM Payroll_PrincipleAmountInfo PAI
                    Where IncomingId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                    list = await _dapper.SqlQueryListAsync<PrincipleAmountInfoViewModel>(user.Database, query, new
                    {
                        Id = id,
                        CompanyId = user.CompanyId,
                        OrganizationId = user.OrganizationId
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "GetPendingPrincipleAmountInfosAsync", user);
            }
            return list;
        }

        public async Task<ExecutionStatus> DeletePendingAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var itemInDb = await _payrollDbContext.Payroll_PeriodicallyVariableAllowanceInfo.SingleOrDefaultAsync(item => item.Id == id);
                if (itemInDb != null && itemInDb.StateStatus == StateStatus.Pending)
                {
                    var principleAmountInfos = _payrollDbContext.Payroll_PrincipleAmountInfo.Where(item => item.IncomingId == id).ToList();
                    _payrollDbContext.Remove(itemInDb);
                    _payrollDbContext.Remove(principleAmountInfos);
                    if ((await _payrollDbContext.SaveChangesAsync()) > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Deleted);
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "Item not found/Status has been changed");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicallyVariableAllowanceBusiness", "DeletePendingAsync", user);
            }
            return executionStatus;
        }
        #endregion
    }
}
