using Dapper;
using AutoMapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Salary;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using Shared.OtherModels.Response;
using BLL.Salary.Salary.Interface;
using Shared.Payroll.Filter.Salary;
using Shared.OtherModels.Pagination;
using Shared.Payroll.ViewModel.Salary;
using DAL.Context.Payroll;
using Shared.Payroll.Domain.Salary;
using Shared.Separation.Filter;

namespace BLL.Salary.Salary.Implementation
{
    public class SalaryAllowanceArrearAdjustmentBusiness : ISalaryAllowanceArrearAdjustmentBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IInfoBusiness _employeeInfoBusiness;
        private readonly PayrollDbContext _payrollDbContext;

        public SalaryAllowanceArrearAdjustmentBusiness(
            IDapperData dapper, IMapper mapper, IInfoBusiness employeeInfoBusiness, ISysLogger sysLogger, PayrollDbContext payrollDbContext)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _employeeInfoBusiness = employeeInfoBusiness;
            _payrollDbContext = payrollDbContext;
        }
        public async Task<ExecutionStatus> UploadSalaryAllowanceArrearAdjAsync(List<SalaryAllowanceArrearAdjustmentDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_UploadSalaryAllowanceArrearAdj";
                var parameters = new DynamicParameters();
                var jsonData = Utility.JsonData(model);
                parameters.Add("JsonData", jsonData);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("Organizationid", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("ExecutionFlag", "Upload");

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "UploadSalaryAllowanceArrearAdjAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<SalaryAllowanceArrearAdjustmentViewModel>> GetSalaryAllowanceArrearAdjustmentListAsync(SalaryAllowanceArrearAdjustment_Filter arrearAdjustment_Filter, AppUser user)
        {
            DBResponse<SalaryAllowanceArrearAdjustmentViewModel> data_list = new DBResponse<SalaryAllowanceArrearAdjustmentViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sqlQuery = $@"WITH Data_CTE AS(
                   SELECT alw.Id, alw.EmployeeId, emp.EmployeeCode, [EmployeeName] = emp.FullName, alw.AllowanceNameId, [AllowanceName] = an.[Name], alw.SalaryMonth,  [SalaryMonthName]= DATENAME(MONTH, DATEADD(MONTH, alw.SalaryMonth - 1, '1900-01-01')), alw.SalaryYear, alw.SalaryDate, alw.CalculationForDays, alw.Amount, alw.Flag, alw.ArrearAdjustmentMonth, alw.ArrearAdjustmentYear, alw.IsActive, alw.StateStatus, alw.IsApproved, alw.CreatedBy,alw.CreatedDate, alw.UpdatedBy,
                     alw.UpdatedDate
                     FROM [dbo].[Payroll_SalaryAllowanceArrearAdjustment] alw 
                     INNER JOIN [dbo].[HR_EmployeeInformation] emp ON alw.EmployeeId = emp.EmployeeId
                      INNER JOIN [dbo].[Payroll_AllowanceName] an ON alw.AllowanceNameId = an.AllowanceNameId
                     WHERE 1=1 
                     AND (@Id IS NULL OR @Id = 0 OR alw.Id = @Id)
                     AND (@EmployeeId IS NULL OR @EmployeeId =0  OR alw.EmployeeId  = @EmployeeId)
                     AND (@AllowanceNameId IS NULL OR @AllowanceNameId=0 OR alw.AllowanceNameId = @AllowanceNameId)   
                     AND (@SalaryMonth IS NULL OR @SalaryMonth=0 OR alw.SalaryMonth = @SalaryMonth)   
                     AND (@SalaryYear IS NULL OR @SalaryYear=0 OR alw.SalaryYear = @SalaryYear)    
                     AND (@StateStatus IS NULL OR @StateStatus = '' OR alw.StateStatus = @StateStatus)
                     AND (@Flag IS NULL OR @Flag = '' OR alw.Flag = @Flag)
                     AND alw.CompanyId = @CompanyId
                     AND alw.OrganizationId = @OrganizationId
                    ),
                    Count_CTE AS (
                    SELECT COUNT(*) AS [TotalRows]
                    FROM Data_CTE)

                    SELECT JSONData=(Select * From (SELECT *
                    FROM Data_CTE
                    ORDER BY Id	
                    OFFSET (CAST(@PageNumber AS INT)-1)*@PageSize ROWS
                    FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = DapperParam.AddParams(arrearAdjustment_Filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sqlQuery, parameters, CommandType.Text);
                data_list.ListOfObject = Utility.JsonToObject<IEnumerable<SalaryAllowanceArrearAdjustmentViewModel>>(response.JSONData) ?? new List<SalaryAllowanceArrearAdjustmentViewModel>();
                data_list.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "GetSalaryAllowanceArrearAdjustmentListAsync", user);
            }
            return data_list;
        }
        public async Task<SalaryAllowanceArrearAdjustmentViewModel> GetSalaryAllowanceArrearAdjustmentByIdAsync(long id, AppUser user)
        {
            SalaryAllowanceArrearAdjustmentViewModel data = new SalaryAllowanceArrearAdjustmentViewModel();
            try
            {
                var sqlQuery = $@"SELECT a.Id, a.EmployeeId, b.EmployeeCode, [EmployeeName] = b.FullName ,a.AllowanceNameId, [AllowanceName] = an.[Name] , a.SalaryMonth, a.SalaryYear, a.SalaryDate, a.CalculationForDays, a.Amount, a.Flag, a.IsActive, a.StateStatus, a.IsApproved, [UserId]=a.CreatedBy, a.CreatedDate
                FROM Payroll_SalaryAllowanceArrearAdjustment a
                INNER JOIN HR_EmployeeInformation b ON a.EmployeeId = b.EmployeeId
                INNER JOIN Payroll_AllowanceName an ON an.AllowanceNameId = a.AllowanceNameId AND an.IsActive = 1
                Where 1=1 AND a.Id = '" + id + "' AND a.CompanyId=@CompanyId AND a.OrganizationId=@OrganizationId";
                var parameters = DapperParam.AddParams(id, user, new string[] { }, addUserId: false);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryFirstAsync<SalaryAllowanceArrearAdjustmentViewModel>(user.Database, sqlQuery, parameters, commandType: CommandType.Text);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "GetSalaryAllowanceArrearAdjustmenteByIdAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> UpdateSalaryAllowanceArrearAdjustmentAsync(SalaryAllowanceArrearAdjustmentViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var query = "";
                        try
                        {

                            if (model.Id > 0)
                            {
                                var rawAffected = 0;
                                query = $@"UPDATE [dbo].[Payroll_SalaryAllowanceArrearAdjustment]
                                SET
                                AllowanceNameId = @AllowanceNameId, SalaryMonth = @SalaryMonth, SalaryYear = @SalaryYear,
                                Flag = @Flag, Amount = ISNULL(@Amount,0), UpdatedBy = @UserId, UpdatedDate=GETDATE()   
                                WHERE 1=1 AND Id = @Id AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    model.AllowanceNameId,
                                    model.SalaryMonth,
                                    model.SalaryYear,
                                    model.Flag,
                                    model.Amount,
                                    model.Id

                                }, user, addBranch: false);

                                rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0)
                                {
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Data has been updated";
                                    transaction.Commit();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            executionStatus = ResponseMessage.Invalid();
                            await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "UpdateSalaryAllowanceArrearAdjustmentAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "UpdateSalaryAllowanceArrearAdjustmentAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeleteSalaryAllowanceArrearAdjustmentAsync(SalaryAllowanceArrearAdjustmentViewModel model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    using (var transaction = connection.BeginTransaction())
                    {
                        var query = "";
                        try
                        {

                            if (model.Id > 0)
                            {
                                var rawAffected = 0;
                                query = $@"DELETE FROM [dbo].[Payroll_SalaryAllowanceArrearAdjustment]		
                                WHERE 1=1 AND Id = @Id AND SalaryYear = @SalaryYear AND SalaryMonth = @SalaryMonth AND
		                        CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    model.Id,
                                    model.SalaryMonth,
                                    model.SalaryYear
                                }, user, addBranch: false);

                                rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0)
                                {
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Data has been deleted";
                                    transaction.Commit();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            executionStatus = ResponseMessage.Invalid();
                            await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "DeleteSalaryAllowanceArrearAdjustmentAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "DeleteMonthlyIncentiveProcessAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<SalaryAllowanceArrearAdjustmentViewModel>> GetSalaryAllowanceArrearAdjustmentByEmployeeIdInSalaryProcessAsync(long employeeId, int year, int month, AppUser user)
        {
            IEnumerable<SalaryAllowanceArrearAdjustmentViewModel> list = new List<SalaryAllowanceArrearAdjustmentViewModel>();
            try
            {
                var query = $@"SELECT SAA.EmployeeId,SAA.AllowanceNameId,AllowanceName=ALW.[Name],SAA.Flag,SAA.Amount,SAA.SalaryMonth,SAA.SalaryYear FROM Payroll_SalaryAllowanceArrearAdjustment SAA
                INNER JOIN  Payroll_AllowanceName ALW ON SAA.AllowanceNameId = ALW.AllowanceNameId
                Where SAA.EmployeeId=@EmployeeId AND SAA.StateStatus='Approved' AND SAA.Flag IN('Arrear','Adjustment') AND SAA.SalaryMonth=@Month AND SAA.SalaryYear=@Year
                AND SAA.CompanyId=@CompanyId AND SAA.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<SalaryAllowanceArrearAdjustmentViewModel>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    Year = year,
                    Month = month,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "GetSalaryAllowanceArrearAdjustmentByEmployeeId", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveAsync(SalaryAllowanceArrearAdjustmentMasterDTO info, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        List<SalaryAllowanceArrearAdjustment> list = new List<SalaryAllowanceArrearAdjustment>();
                        foreach (var i in info.Details)
                        {
                            SalaryAllowanceArrearAdjustment item = new SalaryAllowanceArrearAdjustment();
                            item.AllowanceNameId = info.AllowanceNameId;
                            item.Flag = info.Flag;
                            item.SalaryMonth = info.Month;
                            item.SalaryYear = info.Year;
                            item.CalculationForDays = 0;
                            item.SalaryDate = DateTimeExtension.LastDateOfAMonth(info.Year, info.Month);
                            item.ArrearAdjustmentMonth = info.AdjustmentMonth;
                            item.ArrearAdjustmentYear = info.AdjustmentYear;

                            item.StateStatus = StateStatus.Pending;
                            item.EmployeeId = i.EmployeeId;
                            item.Amount = i.Amount;

                            item.CreatedBy = user.ActionUserId;
                            item.CreatedDate = DateTime.Now;

                            item.CompanyId = user.CompanyId;
                            item.OrganizationId = user.OrganizationId;

                            list.Add(item);
                        }

                        if (list.Any())
                        {
                            _payrollDbContext.AddRange(list);
                            if(await _payrollDbContext.SaveChangesAsync() > 0)
                            {
                                await transaction.CommitAsync();
                                executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                            }
                            else
                            {
                                await transaction.RollbackAsync();
                                throw new Exception("data is failed to save");
                            }
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            throw new Exception("No items found in list to save");
                        }
                    }
                    catch (Exception ex)
                    {
                        if (ex.InnerException != null)
                        {
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "SaveAsync", user);
            }
            return executionStatus ?? ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
        }
        public async Task<IEnumerable<SalaryAllowanceArrearAdjustmentViewModel>> GetPendingSalaryAllowanceArrearAdjustmentAsync(SalaryAllowanceArrearAdjustment_Filter filter, AppUser user)
        {
            IEnumerable<SalaryAllowanceArrearAdjustmentViewModel> list = new List<SalaryAllowanceArrearAdjustmentViewModel>();
            try
            {
                filter.StateStatus = StateStatus.Pending;
                var query = $@"SELECT alw.Id, alw.EmployeeId, emp.EmployeeCode, [EmployeeName] = emp.FullName, alw.AllowanceNameId, [AllowanceName]= an.[Name],
                alw.SalaryMonth,alw.SalaryYear, alw.SalaryDate, alw.CalculationForDays, alw.Amount, alw.Flag, alw.ArrearAdjustmentMonth, 
                alw.ArrearAdjustmentYear, alw.IsActive, alw.StateStatus, alw.IsApproved, alw.CreatedBy,alw.CreatedDate, alw.UpdatedBy,
                alw.UpdatedDate
                FROM Payroll_SalaryAllowanceArrearAdjustment alw 
                INNER JOIN HR_EmployeeInformation emp ON alw.EmployeeId = emp.EmployeeId
                INNER JOIN Payroll_AllowanceName an ON alw.AllowanceNameId = an.AllowanceNameId
                WHERE 1=1 
                AND (@EmployeeId IS NULL OR @EmployeeId =0  OR alw.EmployeeId  = @EmployeeId)
                AND (@AllowanceNameId IS NULL OR @AllowanceNameId=0 OR alw.AllowanceNameId = @AllowanceNameId)   
                AND (@SalaryMonth IS NULL OR @SalaryMonth=0 OR alw.SalaryMonth = @SalaryMonth)   
                AND (@SalaryYear IS NULL OR @SalaryYear=0 OR alw.SalaryYear = @SalaryYear)    
                AND (alw.StateStatus = @StateStatus)
                AND (alw.Flag = @Flag)
                AND alw.CompanyId = @CompanyId
                AND alw.OrganizationId = @OrganizationId";

                list = await _dapper.SqlQueryListAsync<SalaryAllowanceArrearAdjustmentViewModel>(user.Database, query, new
                {
                    EmployeeId = filter.EmployeeId,
                    AllowanceNameId = filter.AllowanceNameId,
                    SalaryMonth = filter.SalaryMonth,
                    SalaryYear = filter.SalaryYear,
                    StateStatus = filter.StateStatus,
                    Flag = filter.Flag,
                    CompanyId=user.CompanyId,
                    OrganizationId = user.OrganizationId,
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "GetPendingSalaryAllowanceArrearAdjustmentAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> ArrearAdjustmentApprovalAsync(ArrearAdjustmentApprovalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var list_of_data = _payrollDbContext.Payroll_SalaryAllowanceArrearAdjustment.Where(i => i.StateStatus == StateStatus.Pending && model.Ids.Contains(i.Id)).ToList();
                if(list_of_data != null && list_of_data.Any())
                {
                    using (var transaction = await _payrollDbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            list_of_data.ForEach(i =>
                            {
                                i.IsApproved = (model.StateStatus == StateStatus.Approved) ? true : false;
                                i.StateStatus = model.StateStatus;
                                i.ApprovedBy = (model.StateStatus == StateStatus.Approved) ? user.ActionUserId : i.ApprovedBy;
                                i.ApprovedDate = (model.StateStatus == StateStatus.Approved) ? DateTime.Now : i.ApprovedDate;
                                i.CancelledBy = (model.StateStatus == StateStatus.Cancelled) ? user.ActionUserId : i.CancelledBy;
                                i.CancelledDate = (model.StateStatus == StateStatus.Cancelled) ? DateTime.Now : i.CancelledDate;
                            });

                            _payrollDbContext.Payroll_SalaryAllowanceArrearAdjustment.UpdateRange(list_of_data);

                            if (await _payrollDbContext.SaveChangesAsync() > 0)
                            {
                                await transaction.CommitAsync();
                                executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                            }
                            else {
                                await transaction.RollbackAsync();
                                throw new Exception("Data has been failed to update");
                            }
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            if (ex.InnerException != null)
                            {
                                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                            }
                            else
                            {
                                executionStatus = ResponseMessage.Message(false, ex.Message);
                            }
                        }
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "No items found to approved");
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentBusiness", "ArrearAdjustmentApprovalAsync", user);
                executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus ?? ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
        }
    }
}
