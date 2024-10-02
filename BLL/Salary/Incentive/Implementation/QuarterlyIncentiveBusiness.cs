using Dapper;
using AutoMapper;
using BLL.Base.Interface;
using BLL.Salary.Incentive.Interface;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Filter.Incentive.QuarterlyIncentive;
using Shared.Payroll.ViewModel.Incentive.QuarterlyIncentive;
using Shared.Services;
using System.Data;
using DAL.DapperObject.Interface;
using BLL.Dashboard.DataService.Interface;

namespace BLL.Salary.Incentive.Implementation
{
    public class QuarterlyIncentiveBusiness : IQuarterlyIncentiveBusiness
    {
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        public QuarterlyIncentiveBusiness(IDapperData dapper, IMapper mapper,
            IDataGetService dataGetService, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
        }


        public async Task<ExecutionStatus> UploadQuarterlyIncentiveAsync(List<QuarterlyIncentiveProcessDetailsViewModel> quarterlyIncentiveProcessDetails, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_UploadQuarterlyIncentiveProcess";

                var parameters = new DynamicParameters();
                var jsonData = Utility.JsonData(quarterlyIncentiveProcessDetails);
                parameters.Add("@JsonData", jsonData);
                parameters.Add("@UserId", user.UserId);
                parameters.Add("@CompanyId", user.CompanyId);
                parameters.Add("@OrganizationId", user.OrganizationId);
                parameters.Add("@BranchId", user.BranchId);
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




        public async Task<IEnumerable<Select2Dropdown>> GetQuarterlyIncenitveYearAsync(AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT DISTINCT CAST(IncentiveYear AS NVARCHAR(50)) AS 'Id', IncentiveYear AS 'Text'
                FROM [dbo].[Payroll_QuarterlyIncentiveProcess]		
                WHERE 1=1 AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";
                var parameters = new DynamicParameters();
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



        public async Task<IEnumerable<Select2Dropdown>> GetQuarterlyIncenitveQuarterAsync(Quarter_Filter filter, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT DISTINCT CAST(IncentiveQuarterNoId AS NVARCHAR(50)) AS Id, IncentiveQuarterNumber AS Text FROM [dbo].[Payroll_QuarterlyIncentiveProcess] WHERE 1=1 AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId AND IncentiveYear = @IncentiveYear";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", filter.IncentiveYear);
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

        public async Task<IEnumerable<Select2Dropdown>> GetQuarterlyIncenitveNumberAsync(Quarter_Filter filter, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT DISTINCT CAST(IncentiveQuarterNumber AS NVARCHAR(150)) AS Id, IncentiveQuarterNumber AS Text FROM [dbo].[Payroll_QuarterlyIncentiveProcess] WHERE 1=1 AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId AND IncentiveYear = @IncentiveYear";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", filter.IncentiveYear);
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

        public async Task<IEnumerable<Select2Dropdown>> GetQuarterlyIncenitveEmployees(QuarterlyIncentiveEmployee_Filter filter, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT DISTINCT CAST(QID.EmployeeId AS NVARCHAR(50)) AS 'Id',EI.FullName +' ['+ EI.EmployeeCode +']' AS 'Text'
                FROM [dbo].[Payroll_QuarterlyIncentiveProcessDetail] QID
				INNER JOIN HR_EmployeeInformation EI ON QID.EmployeeId = EI.EmployeeId
                WHERE 1=1 AND QID.CompanyId = @CompanyId AND QID.OrganizationId = @OrganizationId AND QID.IncentiveYear = @IncentiveYear
				AND QID.IncentiveQuarterNumber = @IncentiveQuarterNumber";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", filter.IncentiveYear);
                parameters.Add("IncentiveQuarterNumber", filter.IncentiveQuarterNumber);
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

        public async Task<DBResponse<QuarterlyIncentiveProcessViewModel>> GetQuarterlyIncentiveAsync(QuarterlyIncentive_Filter filter, AppUser user)
        {
            DBResponse<QuarterlyIncentiveProcessViewModel> data_list = new DBResponse<QuarterlyIncentiveProcessViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sqlQuery = $@"WITH Data_CTE AS(
		            SELECT a.QuarterlyIncentiveProcessId,a.BatchNo,
                  a.IncentiveQuarterNoId, a.IncentiveQuarterNumber, a.IncentiveYear, a.ProcessDate,a.PaymentDate,
                  a.IsDisbursed,a.CreatedBy,a.CreatedDate, a.UpdatedBy,
                  a.UpdatedDate,a.CompanyId,a.OrganizationId, a.IsApproved, HeadCount=COUNT(b.EmployeeId)
                  FROM [dbo].[Payroll_QuarterlyIncentiveProcess] a
                  INNER JOIN [dbo].[Payroll_QuarterlyIncentiveProcessDetail] b ON a.QuarterlyIncentiveProcessId = b.QuarterlyIncentiveProcessId
                  AND a.BatchNo = b.BatchNo AND a.IncentiveYear = b.IncentiveYear
                  WHERE 1=1 
	                AND (@IncentiveQuarterNoId IS NULL OR @IncentiveQuarterNoId=0 OR a.IncentiveQuarterNoId = @IncentiveQuarterNoId)
	                AND (@BatchNo IS NULL OR @BatchNo ='' OR a.BatchNo = @BatchNo)
	                AND (@IncentiveQuarterNumber IS NULL OR @IncentiveQuarterNumber='' OR a.IncentiveQuarterNumber = @IncentiveQuarterNumber)
	                AND (@IncentiveYear IS NULL OR @IncentiveYear=0 OR a.IncentiveYear = @IncentiveYear)              
                    AND (@IsDisbursed IS NULL OR a.IsDisbursed = @IsDisbursed)
	                AND a.CompanyId = @CompanyId
	                AND a.OrganizationId = @OrganizationId
                  GROUP BY a.QuarterlyIncentiveProcessId,a.BatchNo,
                  a.IncentiveQuarterNoId, a.IncentiveQuarterNumber, a.IncentiveYear, a.ProcessDate,a.PaymentDate,
                  a.IsDisbursed,a.CreatedBy,a.CreatedDate, a.UpdatedBy,
                  a.UpdatedDate,a.CompanyId,a.OrganizationId, a.IsApproved
	                ),
	                Count_CTE AS (
	                SELECT COUNT(*) AS [TotalRows]
	                FROM Data_CTE)

	                SELECT JSONData=(Select * From (SELECT *
	                FROM Data_CTE
	                ORDER BY QuarterlyIncentiveProcessId	
	                OFFSET (CAST(@PageNumber AS INT)-1)*@PageSize ROWS
	                FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sqlQuery, parameters, CommandType.Text);
                data_list.ListOfObject = Utility.JsonToObject<IEnumerable<QuarterlyIncentiveProcessViewModel>>(response.JSONData) ?? new List<QuarterlyIncentiveProcessViewModel>();
                data_list.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IQuarterlyIncentiveBusiness", "GetQuarterlyIncentiveAsync", user);
            }
            return data_list;
        }



        public async Task<IEnumerable<QuarterlyIncentiveProcessDetailsViewModel>> GetQuarterlyIncentiveDetailAsync(QuarterlyIncentiveDetail_Filter filter, AppUser user)
        {
            IEnumerable<QuarterlyIncentiveProcessDetailsViewModel> list = new List<QuarterlyIncentiveProcessDetailsViewModel>();
            try
            {
                var query = $@"SELECT PD.*,EI.EmployeeCode, EmployeeName=EI.FullName,P.IsDisbursed, P.StateStatus, P.IsApproved
                FROM [dbo].[Payroll_QuarterlyIncentiveProcessDetail] PD
					INNER JOIN Payroll_QuarterlyIncentiveProcess P
					ON P.QuarterlyIncentiveProcessId = PD.QuarterlyIncentiveProcessId
					INNER JOIN HR_EmployeeInformation EI
					ON EI.EmployeeId = PD.EmployeeId
                WHERE 1=1 
					AND (PD.CompanyId = @CompanyId)
                    AND (PD.OrganizationId = @OrganizationId)
                    AND (@EmployeeIdForSearch IS NULL OR PD.EmployeeId = @EmployeeIdForSearch)
                    AND (@QuarterlyIncentiveProcessId IS NULL OR PD.QuarterlyIncentiveProcessId = @QuarterlyIncentiveProcessId)
                    AND (@QuarterlyIncentiveProcessDetailId IS NULL OR PD.QuarterlyIncentiveProcessDetailId = @QuarterlyIncentiveProcessDetailId)
                ORDER BY PD.EmployeeId";

                var parameters = DapperParam.AddParams(filter, user, new string[] { "Action" }, addUserId: true);
                list = await _dapper.SqlQueryListAsync<QuarterlyIncentiveProcessDetailsViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }


        public async Task<ExecutionStatus> UpdateQuarterlyIncentiveDetailAsync(QuarterlyIncentiveProcessDetailsUpdate detailsUpdate, AppUser user)
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

                            if (detailsUpdate.QuarterlyIncentiveProcessDetailId > 0)
                            {
                                var rawAffected = 0;
                                query = $@"UPDATE [dbo].[Payroll_QuarterlyIncentiveProcessDetail]
                                    SET
                                    BankAccountNumber = @BankAccountNumber,
                                    BankName = @BankName,
                                    BankTransferAmount = ISNULL(@BankTransferAmount,0),
                                    COCInWalletTransfer = ISNULL(@COCInWalletTransfer, 0),     
                                    EligibleQuarterlyIncentive = ISNULL(@EligibleQuarterlyIncentive,0),
                                    QuarterlyIncentiveTax = ISNULL(@QuarterlyIncentiveTax,0),
                                    TotalDeduction = ISNULL(@TotalDeduction,0),
                                    TotalKpiAchievementScore = ISNULL(@TotalKpiAchievementScore,0),
                                    TotalKpiCompanyScore = ISNULL(@TotalKpiCompanyScore,0),
                                    TotalKpiDivisionalIndividualScore = ISNULL(@TotalKpiDivisionalIndividualScore,0),
                                    TotalQuarterlyIncentive = ISNULL(@TotalQuarterlyIncentive,0),
                                    WalletNumber = @WalletNumber,
                                    WalletTransferAmount = ISNULL(@WalletTransferAmount,0),
                                    NetPay = ISNULL(@NetPay,0),
                                    UpdatedBy = @UserId, UpdatedDate=GETDATE()   
                                    WHERE 1=1 AND QuarterlyIncentiveProcessDetailId = @QuarterlyIncentiveProcessDetailId AND 
                                    CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    detailsUpdate.BankAccountNumber,
                                    detailsUpdate.BankName,
                                    detailsUpdate.BankTransferAmount,
                                    detailsUpdate.COCInWalletTransfer,
                                    detailsUpdate.EligibleQuarterlyIncentive,
                                    detailsUpdate.QuarterlyIncentiveTax,
                                    detailsUpdate.TotalDeduction,
                                    detailsUpdate.TotalKpiAchievementScore,
                                    detailsUpdate.TotalKpiCompanyScore,
                                    detailsUpdate.TotalKpiDivisionalIndividualScore,
                                    detailsUpdate.TotalQuarterlyIncentive,
                                    detailsUpdate.WalletNumber,
                                    detailsUpdate.WalletTransferAmount,
                                    detailsUpdate.NetPay,
                                    detailsUpdate.QuarterlyIncentiveProcessDetailId

                                }, user, addBranch: false);

                                rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0)
                                {
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Processed has been updated";
                                    transaction.Commit();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            executionStatus = ResponseMessage.Invalid();
                            await _sysLogger.SaveHRMSException(ex, user.Database, "IQuarterlyIncentiveBusiness", "UpdateQuarterlyIncentiveDetailAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IQuarterlyIncentiveBusiness", "UpdateQuarterlyIncentiveDetailAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> DeleteQuarterlyIncentiveProcessAsync(DeleteQuarterlyIncentiveProcess_Filter filter, AppUser user)
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

                            if (filter.QuarterlyIncentiveProcessDetailId.Length > 0)
                            {
                                var rawAffected = 0;
                                query = $@"DELETE FROM [dbo].[Payroll_QuarterlyIncentiveProcessDetail]		
                                        WHERE 1=1 AND QuarterlyIncentiveProcessDetailId = @QuarterlyIncentiveProcessDetailId AND 
		                                 CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    filter.QuarterlyIncentiveProcessDetailId
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
                            await _sysLogger.SaveHRMSException(ex, user.Database, "IQuarterlyIncentiveBusiness", "UpdateQuarterlyIncentiveDetailAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IQuarterlyIncentiveBusiness", "UpdateQuarterlyIncentiveDetailAsync", user);
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> UndoOrDisbursedQuarterlyIncentiveProcessAsync(UndoOrDisbursed_Filter filter, AppUser user)
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
                            if (filter.IsDisbursedOrUndo == "undo" && filter.QuarterlyIncentiveProcessId.Length > 0)
                            {
                                var rawAffected = 0;
                                query = $@"DELETE FROM [dbo].[Payroll_QuarterlyIncentiveProcessDetail]		
                                        WHERE 1=1 AND QuarterlyIncentiveProcessId = @QuarterlyIncentiveProcessId AND 
		                                 CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    filter.QuarterlyIncentiveProcessId
                                }, user, addBranch: false);

                                rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);

                                if (rawAffected > 0 && filter.QuarterlyIncentiveProcessId.Length > 0 && filter.IsDisbursedOrUndo == "undo")
                                {
                                    query = $@"DELETE FROM [dbo].[Payroll_QuarterlyIncentiveProcess]		
                                        WHERE 1=1 AND QuarterlyIncentiveProcessId = @QuarterlyIncentiveProcessId AND 
		                                 CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                    parameters = DapperParam.AddParamsInKeyValuePairs(new
                                    {
                                        filter.QuarterlyIncentiveProcessId
                                    }, user, addBranch: false);

                                    rawAffected = rawAffected + await connection.ExecuteAsync(query, parameters, transaction, commandTimeout: 0, CommandType.Text);
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Process has been undo";
                                    transaction.Commit();
                                }
                            }

                            else if (filter.IsDisbursedOrUndo == "disbursed" && filter.QuarterlyIncentiveProcessId.Length > 0)
                            {
                                var rawAffected = 0;
                                query = $@"UPDATE [dbo].[Payroll_QuarterlyIncentiveProcess]
                                    SET [IsDisbursed] = 1, StateStatus = 'Approved', IsApproved=1
                                    WHERE 1=1 AND QuarterlyIncentiveProcessId = @QuarterlyIncentiveProcessId 
			                        AND IsDisbursed = 0	AND CompanyId = @CompanyId AND OrganizationId = @OrganizationId";

                                var parameters = DapperParam.AddParamsInKeyValuePairs(new
                                {
                                    filter.QuarterlyIncentiveProcessId
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
                            await _sysLogger.SaveHRMSException(ex, user.Database, "IQuarterlyIncentiveBusiness", "UndoOrDisbursedQuarterlyIncentiveProcessAsync", user);
                        }
                        finally { connection.Close(); }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IQuarterlyIncentiveBusiness", "UpdateQuarterlyIncentiveDetailAsync", user);
            }
            return executionStatus;
        }

        public async Task<DataTable> GetQuarterlyIncentiveReportAsync(QuarterlyIncentiveReport_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var query = $@"SELECT E.EmployeeCode, E.EmployeeId,[EmployeeName]=E.FullName,[DOJ] = FORMAT(E.DateOfJoining, 'dd MMM yyyy'), E.OfficeEmail,PQ.[Division], PQ.[Designation], PQ.[Grade], PQ.[Department], PQ.[IncentiveYear], PQ.[IncentiveQuarterNumber], PQ.[CurrentBasic], PQ.[TotalKpiCompanyScore], PQ.[TotalKpiDivisionalIndividualScore], PQ.[TotalKpiAchievementScore], PQ.[EligibleQuarterlyIncentive], PQ.[TotalQuarterlyIncentive], PQ.[QuarterlyIncentiveTax], PQ.[TotalDeduction], PQ.[NetPay], PQ.[BankTransferAmount], PQ.[WalletTransferAmount], PQ.[COCInWalletTransfer], PQ.[BankAccountNumber], PQ.[BankName], PQ.[BankBranchName], PQ.[RoutingNumber], PQ.[WalletNumber], PQ.[WalletAgent], PQ.[Remarks],[PaymentDate]=FORMAT(PQP.PaymentDate, 'dd MMM yyyy'), PQ.IncentiveQuarterNoId, PQ.BatchNo
                FROM [dbo].[Payroll_QuarterlyIncentiveProcessDetail] PQ
                INNER JOIN Payroll_QuarterlyIncentiveProcess PQP ON PQP.QuarterlyIncentiveProcessId = PQ.QuarterlyIncentiveProcessId 
                INNER JOIN [dbo].[HR_EmployeeInformation] E ON PQ.EmployeeId = E.EmployeeId 
                WHERE 1=1 AND PQ.[CompanyId] = @CompanyId AND PQ.[OrganizationId] = @OrganizationId AND (@Quarter IS NULL OR @Quarter='' OR PQ.[IncentiveQuarterNumber] = @Quarter) AND (@Year IS NULL OR @Year=0 OR PQ.[IncentiveYear] = @Year) AND (@EmployeeIdForSearch IS NULL OR @EmployeeIdForSearch =0 OR PQ.EmployeeId = @EmployeeIdForSearch)";

                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("Year", filter.Year);
                keyValuePairs.Add("Quarter", filter.Quarter ?? "");
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


        public async Task<DataTable> GetQuarterlyIncenitveExcel(DownloadExcel_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sqlQuery = $@"SELECT [Employee Id] = E.EmployeeCode, [Employee Name]=E.FullName,[Joining Date] = FORMAT(E.DateOfJoining, 'dd MMM yyyy'), [Email] = E.OfficeEmail,PQ.[Division], PQ.[Designation], PQ.[Grade], PQ.[Department], [Batch No]= PQ.BatchNo,[Payment Date]=FORMAT(PQP.PaymentDate, 'dd MMM yyyy'), [Incentive Year] = PQ.[IncentiveYear], [Quarter Number] =PQ.[IncentiveQuarterNumber], [Current Basic]=PQ.[CurrentBasic], [Total Company Kpi Score] = PQ.[TotalKpiCompanyScore], [Total Kpi Divisional and Individual Score]= PQ.[TotalKpiDivisionalIndividualScore],  [Total Achievement Kpi Score] = PQ.[TotalKpiAchievementScore], [Eligible Quarterly Incentive]= PQ.[EligibleQuarterlyIncentive], [Total Quarterly Incentive] = PQ.[TotalQuarterlyIncentive], [Quarterly Incentive Tax] = PQ.[QuarterlyIncentiveTax], [Bank Transfer] = PQ.[BankTransferAmount], [Wallet Transfer] =  PQ.[WalletTransferAmount], [Cash Out Charge] = PQ.[COCInWalletTransfer],  [Bank Account Number]= PQ.[BankAccountNumber], [Bank] = PQ.[BankName], [Wallet Number]= PQ.[WalletNumber], [Wallet Agent]= PQ.[WalletAgent], [Total Deduction]= [TotalDeduction], [Net Pay]= PQ.[NetPay], PQ.[Remarks]
                FROM [dbo].[Payroll_QuarterlyIncentiveProcessDetail] PQ 
                INNER JOIN Payroll_QuarterlyIncentiveProcess PQP ON PQP.QuarterlyIncentiveProcessId = PQ.QuarterlyIncentiveProcessId 
                INNER JOIN [dbo].[HR_EmployeeInformation] E ON PQ.EmployeeId = E.EmployeeId 
                WHERE 1=1 AND PQ.[CompanyId] = @CompanyId AND PQ.[OrganizationId] = @OrganizationId AND (@Quarter IS NULL OR @Quarter = '' OR PQ.[IncentiveQuarterNumber] = @Quarter)  AND (@Year IS NULL OR @Year=0 OR PQ.[IncentiveYear] = @Year) AND (@EmployeeIdForSearch IS NULL OR @EmployeeIdForSearch =0 OR PQ.EmployeeId = @EmployeeIdForSearch)";

                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("Year", filter.Year);
                keyValuePairs.Add("Quarter", filter.Quarter ?? "");
                keyValuePairs.Add("EmployeeIdForSearch", filter.EmployeeIdForSearch ?? "0");
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, sqlQuery, keyValuePairs, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IQuarterlyIncentiveBusiness", "GetQuarterlyIncenitveExcel", user);
            }
            return dataTable;
        }

        public async Task<IEnumerable<Select2Dropdown>> GetQuarterNumberExtensionAsync(short incentiveYear, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sqlQuery = "sp_Payroll_UploadQuarterlyIncentiveProcess";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", incentiveYear);
                parameters.Add("CompanyId", appUser.CompanyId);
                parameters.Add("OrganizationId", appUser.OrganizationId);
                parameters.Add("BranchId", appUser.BranchId);
                parameters.Add("ExecutionFlag", Data.Extension);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(appUser.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }


        public async Task<IEnumerable<Select2Dropdown>> GetBatchNoExtensionAsync(short incentiveYear, long? incentiveQuarterNoId, AppUser appUser)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sqlQuery = "sp_Payroll_UploadQuarterlyIncentiveProcess";
                var parameters = new DynamicParameters();
                parameters.Add("IncentiveYear", incentiveYear);
                parameters.Add("IncentiveQuarterNoId", incentiveQuarterNoId);
                parameters.Add("CompanyId", appUser.CompanyId);
                parameters.Add("OrganizationId", appUser.OrganizationId);
                parameters.Add("BranchId", appUser.BranchId);
                parameters.Add("ExecutionFlag", "Extension_QuarterBatchNo");
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    data = await _dapper.SqlQueryListAsync<Select2Dropdown>(appUser.Database, sqlQuery, parameters, CommandType.StoredProcedure);
                }
                return data;
            }
            catch (Exception ex)
            {
                return data;
            }
        }
    }




}

