using AutoMapper;
using BLL.Base.Interface;
using BLL.Salary.Incentive.Interface;
using Dapper;
using System.Data;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Filter.Incentive.MonthlyIncentive;
using Shared.Payroll.ViewModel.Incentive.MonthlyIncentive;
using Shared.Services;
using DAL.DapperObject.Interface;

namespace BLL.Salary.Incentive.Implementation
{
    public class MonthlyIncentiveBusiness : IMonthlyIncentiveBusiness
    {

        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        public MonthlyIncentiveBusiness(IDapperData dapper, IMapper mapper,
           ISysLogger sysLogger)
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<Select2Dropdown>> GetBatchNoExtensionAsync(short incentiveYear, long incentiveMonth, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sqlQuery = $@"Select Convert(Nvarchar(100), a.BatchNo) 'Value', a.BatchNo 'Text'
                From Payroll_SupplementaryPaymentprocessInfo a, Payroll_SupplementaryPaymentAmount b
                Where 1=1
                AND a.PaymentProcessInfoId = b.PaymentProcessInfoId AND a.PaymentYear = b.PaymentYear AND a.PaymentMonth = b.PaymentMonth
                AND a.IsApproved = 1 AND a.StateStatus = 'Disbursed'
                AND a.PaymentYear = @IncentiveYear AND a.PaymentMonth = @IncentiveMonth
                AND a.BatchNo LIKE N'%Incentive%'+ CONVERT(NVARCHAR(10), @IncentiveMonth) +'%' 
                AND b.CompanyId= @CompanyId
                AND b.OrganizationId= @OrganizationId	
                Group BY a.BatchNo
                Order by a.BatchNo";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", incentiveYear);
                parameters.Add("IncentiveMonth", incentiveMonth);
                parameters.Add("CompanyId", appUser.CompanyId);
                parameters.Add("OrganizationId", appUser.OrganizationId);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(appUser.Database, sqlQuery, parameters, CommandType.Text);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public async Task<IEnumerable<Select2Dropdown>> GetMonthlyIncentiveYearExtensionAsync(short incentiveYear, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sqlQuery = $@"Select Convert(Nvarchar(100), a.IncentiveYear) 'Id', Convert(Nvarchar(100), a.IncentiveYear) 'Text'
                From Payroll_MonthlyIncentiveProcess a
                Where 1=1 AND a.IsActive = 1
                AND (@IncentiveYear IS NULL OR @IncentiveYear = 0 OR  a.IncentiveYear = @IncentiveYear)
                AND a.CompanyId= @CompanyId
                AND a.OrganizationId= @OrganizationId	
                Group BY a.IncentiveYear Order by a.IncentiveYear";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", incentiveYear);
                parameters.Add("CompanyId", appUser.CompanyId);
                parameters.Add("OrganizationId", appUser.OrganizationId);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(appUser.Database, sqlQuery, parameters, CommandType.Text);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public async Task<IEnumerable<Select2Dropdown>> GetMonthlyIncentiveMonthExtensionAsync(short incentiveYear, short incentiveMonth, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sqlQuery = $@"Select Convert(Nvarchar(100), a.IncentiveMonth) 'Id', DATENAME(MONTH, DATEADD(MONTH, a.IncentiveMonth - 1, '1900-01-01')) 'Text'
                    From Payroll_MonthlyIncentiveProcess a
                    Where 1=1 AND a.IsActive = 1
                    AND a.IncentiveYear = @IncentiveYear AND (@IncentiveMonth IS NULL OR @IncentiveMonth = 0 OR  a.IncentiveMonth = @IncentiveMonth) AND a.CompanyId= @CompanyId AND a.OrganizationId= @OrganizationId  Group BY a.IncentiveMonth Order by a.IncentiveMonth";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", incentiveYear);
                parameters.Add("IncentiveMonth", incentiveMonth);
                parameters.Add("CompanyId", appUser.CompanyId);
                parameters.Add("OrganizationId", appUser.OrganizationId);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(appUser.Database, sqlQuery, parameters, CommandType.Text);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }

        public async Task<ExecutionStatus> UploadMonthlyIncentiveExcelAsync(List<MonthlyIncentiveProcessDetailViewModel> models, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_UploadMonthlyIncentiveProcess";
                var jsonData = Utility.JsonData(models);
                var parameters = new DynamicParameters();
                parameters.Add("JsonData", jsonData);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("@ExecutionFlag", "Upload");

                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }


        public async Task<DBResponse<MonthlyIncentiveProcessViewModel>> GetMonthlyIncentiveListAsync(MonthlyIncentiveProcess_Filter incentiveProcess_Filter, AppUser user)
        {
            DBResponse<MonthlyIncentiveProcessViewModel> data_list = new DBResponse<MonthlyIncentiveProcessViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sqlQuery = $@"WITH Data_CTE AS(
                    SELECT a.MonthlyIncentiveProcessId, a.BatchNo, a.IsActive,
                    a.MonthlyIncentiveNoId, a.MonthlyIncentiveName, a.IncentiveYear, a.IncentiveMonth, a.ProcessDate,a.PaymentDate,
                    a.IsDisbursed,a.IsApproved, a.StateStatus, a.Remarks, a.CreatedBy,a.CreatedDate, a.UpdatedBy,
                    a.UpdatedDate, [IncentiveMonthName] = DATENAME(MONTH, DATEADD(MONTH, a.IncentiveMonth - 1, '1900-01-01')),
                    HeadCount=COUNT(b.EmployeeId)
                    FROM [dbo].[Payroll_MonthlyIncentiveProcess] a INNER JOIN [dbo].[Payroll_MonthlyIncentiveProcessDetail] b
                    ON a.MonthlyIncentiveProcessId = b.MonthlyIncentiveProcessId AND a.BatchNo = b.BatchNo AND a.IncentiveMonth = b.IncentiveMonth AND a.IncentiveYear = b.IncentiveYear
                    WHERE 1=1 
                    AND (@MonthlyIncentiveNoId IS NULL OR @MonthlyIncentiveNoId = 0 OR b.MonthlyIncentiveNoId = @MonthlyIncentiveNoId)
                    AND (@BatchNo IS NULL OR @BatchNo ='' OR a.BatchNo = @BatchNo)
                    AND (@MonthlyIncentiveName IS NULL OR @MonthlyIncentiveName = '' OR a.MonthlyIncentiveName = @MonthlyIncentiveName)
                    AND (@IncentiveMonth IS NULL OR @IncentiveMonth=0 OR a.IncentiveMonth = @IncentiveMonth)   
                    AND (@IncentiveYear IS NULL OR @IncentiveYear=0 OR a.IncentiveYear = @IncentiveYear)    
                    AND (@IsDisbursed IS NULL OR IsDisbursed = @IsDisbursed)
                    AND a.CompanyId = @CompanyId
                    AND a.OrganizationId = @OrganizationId
                    GROUP BY a.MonthlyIncentiveProcessId, a.BatchNo, a.IsActive,
                    a.MonthlyIncentiveNoId, a.MonthlyIncentiveName, a.IncentiveYear, a.IncentiveMonth, a.ProcessDate,a.PaymentDate,
                    a.IsDisbursed,a.IsApproved, a.StateStatus, a.Remarks, a.CreatedBy,a.CreatedDate, a.UpdatedBy,
                    a.UpdatedDate
                    ),
                    Count_CTE AS (
                    SELECT COUNT(*) AS [TotalRows]
                    FROM Data_CTE)

                    SELECT JSONData=(Select * From (SELECT *
                    FROM Data_CTE
                    ORDER BY MonthlyIncentiveProcessId	
                    OFFSET (CAST(@PageNumber AS INT)-1)*@PageSize ROWS
                    FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = DapperParam.AddParams(incentiveProcess_Filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sqlQuery, parameters, CommandType.Text);
                data_list.ListOfObject = Utility.JsonToObject<IEnumerable<MonthlyIncentiveProcessViewModel>>(response.JSONData) ?? new List<MonthlyIncentiveProcessViewModel>();
                data_list.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyIncentiveBusiness", "GetMonthlyIncentiveListAsync", user);
            }
            return data_list;
        }

        public async Task<IEnumerable<MonthlyIncentiveProcessDetailViewModel>> GetMonthlyIncentiveDetailAsync(MonthlyIncentiveDetail_Filter filter, AppUser user)
        {
            IEnumerable<MonthlyIncentiveProcessDetailViewModel> list = new List<MonthlyIncentiveProcessDetailViewModel>();
            try
            {
                var query = $@"SELECT dtl.*, [IncentiveMonthName] = DATENAME(MONTH, DATEADD(MONTH, dtl.IncentiveMonth - 1, '1900-01-01')), EI.EmployeeCode, EmployeeName=EI.FullName, info.IsDisbursed, info.StateStatus, info.IsApproved
                    FROM [dbo].[Payroll_MonthlyIncentiveProcessDetail] dtl
                    INNER JOIN [Payroll_MonthlyIncentiveProcess] info
                    ON info.MonthlyIncentiveProcessId = dtl.MonthlyIncentiveProcessId
                    INNER JOIN HR_EmployeeInformation EI
                    ON EI.EmployeeId = dtl.EmployeeId
                    WHERE 1=1 
                    AND dtl.CompanyId = @CompanyId
                    AND dtl.OrganizationId = @OrganizationId
                    AND (@EmployeeIdForSearch IS NULL OR dtl.EmployeeId = @EmployeeIdForSearch)
                    AND (@MonthlyIncentiveProcessId IS NULL OR dtl.MonthlyIncentiveProcessId = @MonthlyIncentiveProcessId)
                    AND (@MonthlyIncentiveProcessDetailId IS NULL OR dtl.MonthlyIncentiveProcessDetailId = @MonthlyIncentiveProcessDetailId)
                   ORDER BY dtl.EmployeeId ";

                var parameters = DapperParam.AddParams(filter, user, new string[] { "Action" }, addUserId: true);
                list = await _dapper.SqlQueryListAsync<MonthlyIncentiveProcessDetailViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        public async Task<ExecutionStatus> UpdateMonthlyIncentiveDetailAsync(UpdateMonthlyIncentiveProcessDetails_Filter filter, AppUser user)
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

                            if (filter.MonthlyIncentiveProcessDetailId > 0)
                            {
                                var rawAffected = 0;
                                query = $@"UPDATE [dbo].[Payroll_MonthlyIncentiveProcessDetail]
                                SET 
                                BankAccountNumber = @BankAccountNumber,
                                BankName = @BankName,
                                BankTransferAmount = ISNULL(@BankTransferAmount,0),
                                COCInWalletTransfer =ISNULL(@COCInWalletTransfer, 0),     
                                EligibleIncentive = ISNULL(@EligibleIncentive,0),
                                IncentiveTax = ISNULL(@IncentiveTax,0),
                                TotalDeduction = ISNULL(@TotalDeduction,0),
                                AdjustedKpiPerformanceScore = ISNULL(@AdjustedKpiPerformanceScore,0),
                                ESSAURating = @ESSAURating,
                                AttendanceAdherenceQualityScore = ISNULL(@AttendanceAdherenceQualityScore,0),
                                TotalIncentive = ISNULL(@TotalIncentive,0),
                                WalletNumber = @WalletNumber,
                                WalletTransferAmount = ISNULL(@WalletTransferAmount,0),
                                NetPay = ISNULL(@NetPay,0),
                                UpdatedBy = @UserId, UpdatedDate=GETDATE()   
                                WHERE 1=1 AND MonthlyIncentiveProcessDetailId = @MonthlyIncentiveProcessDetailId AND 
	                                CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    filter.BankAccountNumber,
                                    filter.BankName,
                                    filter.BankTransferAmount,
                                    filter.COCInWalletTransfer,
                                    filter.EligibleIncentive,
                                    filter.IncentiveTax,
                                    filter.TotalDeduction,
                                    filter.AdjustedKpiPerformanceScore,
                                    filter.ESSAURating,
                                    filter.AttendanceAdherenceQualityScore,
                                    filter.TotalIncentive,
                                    filter.WalletNumber,
                                    filter.WalletTransferAmount,
                                    filter.NetPay,
                                    filter.MonthlyIncentiveProcessDetailId

                                }, user, addBranch: false);

                                rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0)
                                {
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Process has been updated";
                                    transaction.Commit();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            executionStatus = ResponseMessage.Invalid();
                            await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyIncentiveBusiness", "UpdateMonthlyIncentiveDetailAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyIncentiveBusiness", "UpdateQuarterlyIncentiveDetailAsync", user);
            }
            return executionStatus;
        }


        public async Task<ExecutionStatus> DeleteMonthlyIncentiveDetailAsync(DeleteMonthlyIncentiveProcess_Filter filter, AppUser user)
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

                            if (filter.MonthlyIncentiveProcessDetailId.Length > 0)
                            {
                                var rawAffected = 0;
                                query = $@"DELETE FROM [dbo].[Payroll_MonthlyIncentiveProcessDetail]		
                                        WHERE 1=1 AND MonthlyIncentiveProcessDetailId = @MonthlyIncentiveProcessDetailId AND 
		                                 CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    filter.MonthlyIncentiveProcessDetailId
                                }, user, addBranch: false);

                                rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0)
                                {
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Process has been deleted";
                                    transaction.Commit();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            executionStatus = ResponseMessage.Invalid();
                            await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyIncentiveBusiness", "DeleteMonthlyIncentiveProcessAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyIncentiveBusiness", "DeleteMonthlyIncentiveProcessAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> UndoOrDisbursedMonthlyIncentiveProcessAsync(MonthlyIncentiveUndoOrDisbursed_Filter filter, AppUser user)
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
                            if (filter.IsDisbursedOrUndo == "undo" && filter.MonthlyIncentiveProcessId.Length > 0)
                            {
                                var rawAffected = 0;
                                query = $@"DELETE FROM [dbo].[Payroll_MonthlyIncentiveProcessDetail]		
                                        WHERE 1=1 AND MonthlyIncentiveProcessId = @MonthlyIncentiveProcessId AND 
		                                 CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    filter.MonthlyIncentiveProcessId
                                }, user, addBranch: false);

                                rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0 && filter.MonthlyIncentiveProcessId.Length > 0 && filter.IsDisbursedOrUndo == "undo")
                                {
                                    query = $@"DELETE FROM [dbo].[Payroll_MonthlyIncentiveProcess]		
                                        WHERE 1=1 AND MonthlyIncentiveProcessId = @MonthlyIncentiveProcessId AND 
		                                 CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                    parameters = DapperParam.AddParamsInKeyValuePairs(new
                                    {
                                        filter.MonthlyIncentiveProcessId
                                    }, user, addBranch: false);

                                    rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Process has been undo";
                                    transaction.Commit();
                                }
                            }

                            else if (filter.IsDisbursedOrUndo == "disbursed" && filter.MonthlyIncentiveProcessId.Length > 0)
                            {
                                var rawAffected = 0;
                                query = $@"UPDATE [dbo].[Payroll_MonthlyIncentiveProcess]
                                    SET [IsDisbursed] = 1, StateStatus = 'Approved', IsApproved=1
                                    WHERE 1=1 AND MonthlyIncentiveProcessId = @MonthlyIncentiveProcessId 
			                        AND IsDisbursed = 0	AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    filter.MonthlyIncentiveProcessId
                                }, user, addBranch: false);

                                rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0 && filter.IsDisbursedOrUndo == "disbursed")
                                {

                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Process has been disbursed";
                                    transaction.Commit();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            executionStatus = ResponseMessage.Invalid();
                            await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyIncentiveBusiness", "UndoOrDisbursedMonthlyIncentiveProcessAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyIncentiveBusiness", "UndoOrDisbursedMonthlyIncentiveProcessAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<Select2Dropdown>> GetMonthlyIncentiveEmployeesExtensionAsync(short incentiveYear, short incentiveMonth, long? employeeIdForSearch, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT DISTINCT CAST(dtl.EmployeeId AS NVARCHAR(50)) AS 'Id', emp.FullName +' ['+ emp.EmployeeCode +']' AS 'Text'
                    FROM [dbo].[Payroll_MonthlyIncentiveProcessDetail] dtl
                    INNER JOIN HR_EmployeeInformation emp ON dtl.EmployeeId = emp.EmployeeId
                    WHERE 1=1 AND dtl.CompanyId = @CompanyId AND dtl.OrganizationId = @OrganizationId AND dtl.IncentiveYear = @IncentiveYear
                    AND dtl.IncentiveMonth = @IncentiveMonth AND (@EmployeeId IS NULL OR @EmployeeId = 0 OR dtl.EmployeeId = @EmployeeId)";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", incentiveYear);
                parameters.Add("IncentiveMonth", incentiveMonth);
                parameters.Add("EmployeeId", employeeIdForSearch);
                parameters.Add("CompanyId", appUser.CompanyId);
                parameters.Add("OrganizationId", appUser.OrganizationId);
                if (!Utility.IsNullEmptyOrWhiteSpace(query))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(appUser.Database, query, parameters, CommandType.Text);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }


        public async Task<DataTable> GetMonthlyIncentiveReportAsync(MonthlyIncentiveReport_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {

                var query = $@"SELECT [EmployeeId] = E.EmployeeCode, [EmployeeName]=E.FullName,[JoiningDate] = FORMAT(E.DateOfJoining, 'dd MMM yyyy'), [Email] = E.OfficeEmail,dtl.[Division], dtl.[Designation], dtl.[Grade], dtl.[Department], dtl.BatchNo,[PaymentDate]=FORMAT(info.PaymentDate, 'dd MMM yyyy'), dtl.[IncentiveYear], dtl.IncentiveMonth, [IncentiveMonthName]= DATENAME(MONTH, DATEADD(MONTH, dtl.IncentiveMonth - 1, '1900-01-01')), dtl.[CurrentBasic],dtl.AdjustedKpiPerformanceScore,dtl.ESSAURating, dtl.AttendanceAdherenceQualityScore, dtl.EligibleIncentive, [TotalAdjustment] =Adjustment, dtl.TotalIncentive, dtl.IncentiveTax, dtl.[BankTransferAmount], dtl.[WalletTransferAmount], dtl.[COCInWalletTransfer], dtl.[BankAccountNumber],dtl.[BankName], dtl.[WalletNumber], dtl.[WalletAgent], dtl.[TotalDeduction], dtl.[NetPay], dtl.[Remarks]
		        FROM [dbo].[Payroll_MonthlyIncentiveProcessDetail] dtl 
		        INNER JOIN [Payroll_MonthlyIncentiveProcess] info ON dtl.MonthlyIncentiveProcessId = info.MonthlyIncentiveProcessId 
		        INNER JOIN [dbo].[HR_EmployeeInformation] E ON dtl.EmployeeId = E.EmployeeId 
		        WHERE 1=1 
		        AND dtl.[CompanyId] = @CompanyId AND dtl.[OrganizationId] = @OrganizationId  AND (@IncentiveYear IS NULL OR @IncentiveYear=0 OR dtl.[IncentiveYear] = @IncentiveYear) AND (@IncentiveMonth IS NULL OR @IncentiveMonth = 0 OR dtl.[IncentiveMonth] = @IncentiveMonth) AND (@EmployeeIdForSearch IS NULL OR @EmployeeIdForSearch =0 OR dtl.EmployeeId = @EmployeeIdForSearch)";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                keyValuePairs.Add("IncentiveYear", filter.IncentiveYear);
                keyValuePairs.Add("IncentiveMonth", filter.IncentiveMonth ?? "0");
                keyValuePairs.Add("EmployeeIdForSearch", filter.EmployeeIdForSearch ?? "0");
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, query, keyValuePairs, CommandType.Text);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
            return dataTable;
        }


        public async Task<DataTable> GetMonthlyIncenitveExcelAsync(MonthlyIncentiveDownloadExcel_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sqlQuery = $@"SELECT  [Employee Id] = E.EmployeeCode, [Employee Name]=E.FullName,[Joining Date] = FORMAT(E.DateOfJoining, 'dd MMM yyyy'), [Email] = E.OfficeEmail,dtl.[Division], dtl.[Designation], dtl.[Grade], dtl.[Department], [Batch No]= dtl.BatchNo,[Payment Date]=FORMAT(info.PaymentDate, 'dd MMM yyyy'), [Incentive Year] = dtl.[IncentiveYear], [Incentive Month] =dtl.IncentiveMonth, [Current Basic]=dtl.[CurrentBasic], [Performance Score] = dtl.AdjustedKpiPerformanceScore, [ESSAU Rating]= dtl.ESSAURating,  [Attendance Score] = dtl.AttendanceAdherenceQualityScore, [Eligible Monthly Incentive]= dtl.EligibleIncentive, [Total Adjustment] =Adjustment, [Total Quarterly Incentive] = dtl.TotalIncentive, [Monthly Incentive Tax] = dtl.IncentiveTax, [Bank Transfer] = dtl.[BankTransferAmount], [Wallet Transfer] =  dtl.[WalletTransferAmount], [Cash Out Charge] = dtl.[COCInWalletTransfer],  [Bank Account Number]= dtl.[BankAccountNumber], [Bank] = dtl.[BankName], [Wallet Number]= dtl.[WalletNumber], [Wallet Agent]= dtl.[WalletAgent], [Total Deduction]= [TotalDeduction], [Net Pay]= dtl.[NetPay], dtl.[Remarks]
                FROM [dbo].[Payroll_MonthlyIncentiveProcessDetail] dtl 
                INNER JOIN [Payroll_MonthlyIncentiveProcess] info ON dtl.MonthlyIncentiveProcessId = info.MonthlyIncentiveProcessId 
                INNER JOIN [dbo].[HR_EmployeeInformation] E ON dtl.EmployeeId = E.EmployeeId 
                WHERE 1=1 
                AND dtl.[CompanyId] = @CompanyId AND dtl.[OrganizationId] = @OrganizationId  AND (@IncentiveYear IS NULL OR @IncentiveYear=0 OR dtl.[IncentiveYear] = @IncentiveYear) AND (@IncentiveMonth IS NULL OR @IncentiveMonth = 0 OR dtl.[IncentiveMonth] = @IncentiveMonth) AND (@EmployeeIdForSearch IS NULL OR @EmployeeIdForSearch =0 OR dtl.EmployeeId = @EmployeeIdForSearch)";

                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                keyValuePairs.Add("IncentiveYear", filter.IncentiveYear);
                keyValuePairs.Add("IncentiveMonth", filter.IncentiveMonth ?? "0");
                keyValuePairs.Add("EmployeeIdForSearch", filter.EmployeeIdForSearch ?? "0");
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, sqlQuery, keyValuePairs, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyIncentiveBusiness", "GetQuarterlyIncenitveExcel", user);
            }
            return dataTable;
        }


    }
}
