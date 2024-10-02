using System;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.DataService;
using DAL.DapperObject.Interface;
using BLL.Leave.Interface.Report;
using Shared.Leave.Report;
using Shared.Leave.Filter.Report;
using BLL.Leave.Interface.LeaveSetting;
using BLL.Base.Implementation;
using Shared.OtherModels.Report;

namespace BLL.Leave.Implementation.Report
{
    public class LeaveReportBusiness : ILeaveReportBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IReportBase _reportBase;
        private readonly ILeaveSettingBusiness _leaveSettingBusiness;
        public LeaveReportBusiness(ISysLogger sysLogger, IDapperData dapper, IReportBase reportBase, ILeaveSettingBusiness leaveSettingBusiness)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _reportBase = reportBase;
            _leaveSettingBusiness = leaveSettingBusiness;
        }

        public async Task<IEnumerable<EmployeeInfoInLeaveBalance>> EmployeeWiseLeaveBalanceSummaryAsync(LeaveQuery_Filter filter, AppUser user)
        {
            // AND (CAST(GETDATE() AS DATE) BETWEEN LeavePeriodStart AND LeavePeriodEnd)
            IEnumerable<EmployeeInfoInLeaveBalance> list = new List<EmployeeInfoInLeaveBalance>();
            try
            {
                var query = $@"Select emp.EmployeeId,emp.EmployeeCode,EmployeeName=emp.FullName,deg.DesignationName,
		        dept.DepartmentName,sec.SectionName,subsec.SubSectionName,
		        lvbal.LeaveTypeName,lvbal.TotalLeave,lvbal.LeaveAvailed,
		        [FromDate]=@FromDate,[ToDate]=@ToDate,
		        (lvbal.TotalLeave-lvbal.LeaveAvailed) as LeaveBalance
		        From HR_EmployeeInformation emp 	
		        LEFT JOIN HR_EmployeeLeaveBalance lvbal on lvbal.EmployeeId = emp.EmployeeId
		        LEFT JOIN HR_Designations deg on emp.DesignationId = deg.DesignationId
		        LEFT JOIN HR_Departments dept on emp.DepartmentId = dept.DepartmentId
	            LEFT JOIN HR_Sections sec on emp.SectionId = sec.SectionId
	            LEFT JOIN HR_SubSections subsec on emp.SubSectionId = subsec.SubSectionId
		        Where 1=1
		        AND ((@EmployeeId IS NULL OR @EmployeeId ='' OR emp.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@EmployeeId,','))))
		        AND (@DepartmentId IS NULL OR @DepartmentId=0 OR emp.DepartmentId=@DepartmentId)
		        AND (@SectionId IS NULL OR @SectionId=0 OR emp.SectionId=@SectionId) 
		        AND (@SubSectionId IS NULL OR @SubSectionId=0 OR emp.SubSectionId=@SubSectionId) 
               AND (@FromDate BETWEEN LeavePeriodStart AND LeavePeriodEnd)
		        AND (emp.CompanyId=@CompanyId)
		        AND (emp.OrganizationId=@OrganizationId)	
		        Order By emp.EmployeeCode asc";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<EmployeeInfoInLeaveBalance>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "EmployeeInfoInLeaveBalancesAsync", user);
            }
            return list;
        }
        public async Task<DataTable> MonthlyLeaveReportAsync(LeaveQuery_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_HR_EmployeesMonthlyLeaveRecords";
                var parameters = new Dictionary<string, string>();
                parameters.Add("MonthNo", filter.monthNo);
                parameters.Add("Year", filter.monthYear);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    var columnName = dataTable.Columns[columnIndex].ColumnName;
                    int j = 0;
                    int.TryParse(columnName, out j);

                    // Iterate through rows and handle null/empty values
                    for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                    {
                        var cellValue = dataTable.Rows[rowIndex][columnIndex];

                        // Convert DBNull to null for easier handling
                        if (cellValue == DBNull.Value)
                        {
                            cellValue = null;
                        }

                        // Check if the cellValue is null or empty
                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            // Replace null or empty values with 0.00
                            dataTable.Rows[rowIndex][columnIndex] = 0.00;
                        }
                    }

                    // Modify column names for days 1 to 31
                    if (j > 0 && j <= 31)
                    {
                        dataTable.Columns[columnIndex].ColumnName = "Day" + columnName;

                        var colName = dataTable.Columns[columnIndex].ColumnName;

                        // Iterate through rows and handle null/empty values
                        foreach (DataRow row in dataTable.Rows)
                        {
                            var cellValue = row[colName].ToString();

                            // Check if the cellValue is null or empty
                            if (cellValue == "0")
                            {
                                // Replace null or empty values with "-"
                                row[colName] = "-";
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportBusiness", "MonthlyLeaveReportAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> DateRangeWiseLeaveReportAsync(LeaveQuery_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_HR_RptDateRangeWiseLeave";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", filter.employeeId);
                parameters.Add("LeaveTypeId", filter.leaveTypeId);
                parameters.Add("FromDate", filter.fromDate);
                parameters.Add("ToDate", filter.toDate);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                parameters.Add("ExecutionFlag", "R");
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportBusiness", "DateRangeWiseLeaveReportAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> IndividualYearlyStatusAsync(LeaveQuery_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_HR_GetEmployeeAttandanceStatusByYear";
                var parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", filter.employeeId);
                parameters.Add("Year", filter.monthYear);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                // Iterate through columns to modify column names and replace null/empty values
                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    var columnName = dataTable.Columns[columnIndex].ColumnName;
                    int j = 0;
                    int.TryParse(columnName, out j);

                    // Iterate through rows and handle null/empty values
                    for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                    {
                        var cellValue = dataTable.Rows[rowIndex][columnIndex];

                        // Convert DBNull to null for easier handling
                        if (cellValue == DBNull.Value)
                        {
                            cellValue = null;
                        }

                        // Check if the cellValue is null or empty
                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            // Replace null or empty values with 0.00
                            dataTable.Rows[rowIndex][columnIndex] = 0.00;
                        }
                    }
                    // Modify column names for days 1 to 31
                    if (j > 0 && j <= 31)
                    {
                        dataTable.Columns[columnIndex].ColumnName = "Day" + columnName;

                        var colName = dataTable.Columns[columnIndex].ColumnName;


                        foreach (DataRow row in dataTable.Rows)
                        {
                            var cellValue = row[colName].ToString();

                            // Check if the cellValue is null or empty
                            if (cellValue == "0")
                            {
                                // Replace null or empty values with "-"
                                row[colName] = "-";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportBusiness", "EmployeeYearlyAttandanceStatusAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> YearlyLeaveReportAsync(LeaveQuery_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_HR_Employees_InPeriod_LeaveRecords";
                var parameters = new Dictionary<string, string>();
                parameters.Add("FromDate", filter.fromDate);
                parameters.Add("ToDate", filter.toDate);
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());

                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                for (int columnIndex = 0; columnIndex < dataTable.Columns.Count; columnIndex++)
                {
                    var columnName = dataTable.Columns[columnIndex].ColumnName;
                    int j = 0;
                    int.TryParse(columnName, out j);

                    // Iterate through rows and handle null/empty values
                    for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
                    {
                        var cellValue = dataTable.Rows[rowIndex][columnIndex];

                        // Convert DBNull to null for easier handling
                        if (cellValue == DBNull.Value)
                        {
                            cellValue = null;
                        }

                        // Check if the cellValue is null or empty
                        if (cellValue == null || string.IsNullOrWhiteSpace(cellValue.ToString()))
                        {
                            // Replace null or empty values with 0.00
                            dataTable.Rows[rowIndex][columnIndex] = 0.00;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportBusiness", "YearlyLeaveReportAsync", user);
            }
            return dataTable;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetLeaveYearDropdownAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"Select distinct 0 AS [Value],0 as ID,convert(varchar,(DATEADD(yy, DATEDIFF(yy, 0, LB.LeavePeriodEnd), 0)),106) + ' ~ ' + convert(varchar,(DATEADD(yy, DATEDIFF(yy, 0, LB.LeavePeriodEnd) + 1, -1)),106) as Text From HR_EmployeeLeaveBalance LB
                  Where 1=1 
				AND (LB.CompanyId=@CompanyId)
				AND (LB.OrganizationId=@OrganizationId)";
                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, new { user.CompanyId, user.OrganizationId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetLeaveTypesDropdownAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<LeaveCardEmployeeInformation>> GetEmployeeInfoForLeaveCardAsync(LeaveCardFilter filter, AppUser user)
        {
            IEnumerable<LeaveCardEmployeeInformation> list = new List<LeaveCardEmployeeInformation>();
            try
            {
                var query = $@"SELECT EMP.EmployeeId,EMP.EmployeeCode,EMP.FullName,[Designation]=DEG.DesignationName,[Department]=DPT.DepartmentName,EMP.JobType,DTL.Gender,DTL.MaritalStatus,
JoiningDate=EMP.DateOfJoining,LastWrokingDate=(CASE WHEN EMP.TerminationStatus ='Approved' THEN EMP.TerminationDate ELSE NULL END),
LeaveStartDate=@LeaveStartDate,LeaveEndDate=@LeaveEndDate,LeaveYear=YEAR(@LeaveStartDate),EMP.BranchId,EMP.CompanyId,EMP.OrganizationId,
[ServiceLength]=dbo.fnCalculateAgeReturnText(CAST(EMP.DateOfJoining AS DATE),(CASE WHEN EMP.TerminationDate IS NOT NULL AND EMP.TerminationStatus='Approved' AND EMP.TerminationDate < CAST(GETDATE() AS DATE) THEN EMP.TerminationDate
ELSE CAST(GETDATE() AS DATE) END))
FROM HR_EmployeeInformation EMP
LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
LEFT JOIN HR_Designations DEG ON EMP.DesignationId = DEG.DesignationId
LEFT JOIN HR_Departments DPT ON EMP.DepartmentId = DPT.DepartmentId
Where EMP.EmployeeId IN (SELECT DISTINCT EmployeeId FROM HR_EmployeeLeaveBalance Where @LeaveEndDate BETWEEN LeavePeriodStart AND LeavePeriodEnd)
AND (@EmployeeId = 0 OR EMP.EmployeeId=@EmployeeId) AND EMP.CompanyId=@CompanyId AND EMP.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<LeaveCardEmployeeInformation>(user.Database, query, new { EmployeeId = filter.EmployeeId, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId, LeaveStartDate = filter.LeaveStartDate, LeaveEndDate = filter.LeaveEndDate });
                ReportLayer reportLayer = null;
                if (list.Any() && list != null)
                {
                    foreach (var item in list)
                    {
                        if (item != null)
                        {
                            if (reportLayer != null)
                            {
                                if (item.BranchId == reportLayer.BranchId)
                                {
                                    item.BranchName = reportLayer.BranchName;
                                    item.BranchAddress = reportLayer.Address;
                                    item.BranchLogo = reportLayer.BranchLogo;
                                    item.ReportLogo = reportLayer.ReportLogo;
                                }
                                else
                                {
                                    reportLayer = await _reportBase.ReportLayerAsync(item.OrganizationId, item.CompanyId, item.BranchId, 0);
                                    if (reportLayer != null)
                                    {
                                        item.BranchName = reportLayer.BranchName;
                                        item.BranchAddress = reportLayer.Address;
                                        item.BranchLogo = reportLayer.BranchLogo;
                                        item.ReportLogo = reportLayer.ReportLogo;
                                    }
                                }
                            }
                            else
                            {
                                reportLayer = await _reportBase.ReportLayerAsync(item.OrganizationId, item.CompanyId, item.BranchId, 0);
                                if (reportLayer != null)
                                {
                                    item.BranchName = reportLayer.BranchName;
                                    item.BranchAddress = reportLayer.Address;
                                    item.BranchLogo = reportLayer.BranchLogo;
                                    item.ReportLogo = reportLayer.ReportLogo;
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetEmployeeInfoForLeaveCardAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<LeaveCardLeaveBalanceSummary>> GetEmployeeLeaveBalanceSummaryForLeaveCardAsync(LeaveCardFilter filter, AppUser user)
        {
            IEnumerable<LeaveCardLeaveBalanceSummary> list = new List<LeaveCardLeaveBalanceSummary>();
            try
            {
                var query = $@"SELECT *,[Balance]=[Allocated]-[Applied] FROM (SELECT 
EMP.EmployeeId,EMP.EmployeeCode,EMP.FullName,LastWrokingDate=EMP.TerminationDate,[LeaveStartDate]=LB.LeavePeriodStart,[LeaveEndDate]=LB.LeavePeriodEnd,
LB.LeaveTypeId,[LeaveType]=LT.Title,[Allocated]=LB.TotalLeave,
[Pending]=ISNULL((SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory LH Where LH.[Status] IN('Pending','Recommended') AND LH.EmployeeId=EMP.EmployeeId AND LH.LeaveTypeId=LB.LeaveTypeId
AND CAST(LH.LeaveDate AS DATE) BETWEEN @LeaveStartDate AND @LeaveEndDate),0),
[Approved]=ISNULL((SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory LH Where LH.[Status] ='Approved' AND LH.EmployeeId=EMP.EmployeeId AND LH.LeaveTypeId=LB.LeaveTypeId AND CAST(LH.LeaveDate AS DATE) >= CAST(GETDATE() AS DATE)),0),
[Availed]=ISNULL((SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory LH Where LH.[Status] ='Approved' AND LH.EmployeeId=EMP.EmployeeId AND LH.LeaveTypeId=LB.LeaveTypeId AND CAST(LH.LeaveDate AS DATE) < CAST(GETDATE() AS DATE)),0),
[Applied]=ISNULL((SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory LH Where LH.[Status] IN('Pending','Recommended','Approved') AND LH.LeaveTypeId=LB.LeaveTypeId AND LH.EmployeeId=EMP.EmployeeId AND CAST(LH.LeaveDate AS DATE) BETWEEN @LeaveStartDate AND @LeaveEndDate),0)
FROM HR_EmployeeLeaveBalance LB
INNER JOIN HR_EmployeeInformation EMP ON LB.EmployeeId =EMP.EmployeeId
INNER JOIN HR_LeaveTypes LT ON LB.LeaveTypeId = LT.Id
Where LB.EmployeeId=@EmployeeId AND LB.CompanyId=@CompanyId AND LB.OrganizationId=@OrganizationId) tbl";

                list = await _dapper.SqlQueryListAsync<LeaveCardLeaveBalanceSummary>(user.Database, query, new { EmployeeId = filter.EmployeeId, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId, LeaveStartDate = filter.LeaveStartDate, LeaveEndDate = filter.LeaveEndDate });

                var foo = await this.GetEmployeeLeaveBalanceSummaryWithApplicableLeaveAsync(filter, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetEmployeeLeaveBalanceSummaryForLeaveCardAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<LeaveCardAppliedLeaveInformation>> GetAppliedLeaveInformationForLeaveCardAsync(LeaveCardFilter filter, AppUser user)
        {
            IEnumerable<LeaveCardAppliedLeaveInformation> list = new List<LeaveCardAppliedLeaveInformation>();
            try
            {
                var query = $@"SELECT EMP.EmployeeId,EMP.EmployeeCode,EMP.FullName,[LeaveStartDate]=@LeaveStartDate,[LeaveEndDate]=@LeaveEndDate,
[LastWrokingDate]=(CASE WHEN EMP.TerminationStatus ='Approved' THEN EMP.TerminationDate ELSE NULL END),
LH.LeaveDate,LH.LeaveTypeId,[LeaveType]=LT.Title,LR.DayLeaveType,[PartOfDay]=ISNULL(LR.HalfDayType,''),LH.[Count],
[Purpose]=LR.LeavePurpose,
[AppliedDate]=LR.CreatedDate,[LeaveDuration]=(CASE WHEN LR.DayLeaveType='Full-Day' THEN LR.DayLeaveType
WHEN LR.DayLeaveType='Half-Day' AND HalfDayType='Second Portion'  THEN 'Second Half'
WHEN LR.DayLeaveType='Half-Day' AND HalfDayType='First Portion'  THEN 'First Half' END),
[Status]=(
	CASE WHEN LH.[Status] ='Approved' AND CAST(GETDATE() AS DATE) > LH.LeaveDate  THEN 'Availed'
	WHEN LH.[Status] ='Approved' AND CAST(GETDATE() AS DATE) <= LH.LeaveDate  THEN 'Approved'
	ELSE LH.[Status] END
)
FROM HR_EmployeeLeaveHistory LH
INNER JOIN HR_EmployeeLeaveRequest LR ON LH.EmployeeId = LR.EmployeeId AND LR.EmployeeLeaveRequestId = LH.EmployeeLeaveRequestId
INNER JOIN HR_LeaveTypes LT ON LH.LeaveTypeId = LT.Id
INNER JOIN HR_EmployeeInformation EMP ON LH.EmployeeId=EMP.EmployeeId
Where LH.EmployeeId=@EmployeeId AND LH.CompanyId=@CompanyId AND LH.OrganizationId=@OrganizationId AND LH.LeaveDate BETWEEN CAST(@LeaveStartDate AS DATE) AND CAST(@LeaveEndDate AS DATE)
AND LH.[Status] IN ('Pending','Recommended','Approved')";

                list = await _dapper.SqlQueryListAsync<LeaveCardAppliedLeaveInformation>(user.Database, query, new { EmployeeId = filter.EmployeeId, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId, LeaveStartDate = filter.LeaveStartDate, LeaveEndDate = filter.LeaveEndDate });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetEmployeeLeaveBalanceSummaryForLeaveCardAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<LeaveCardLeaveBalanceSummary>> GetEmployeeLeaveBalanceSummaryWithApplicableLeaveAsync(LeaveCardFilter filter, AppUser user)
        {
            IEnumerable<LeaveCardLeaveBalanceSummary> list = new List<LeaveCardLeaveBalanceSummary>();
            try
            {
                var query = $@"SELECT *,[Balance]=[Allocated]-[Applied] FROM (SELECT 
EMP.EmployeeId,EMP.EmployeeCode,EMP.FullName,[JoiningDate]=EMP.DateOfJoining,LastWrokingDate=EMP.TerminationDate,[LeaveStartDate]=LB.LeavePeriodStart,[LeaveEndDate]=LB.LeavePeriodEnd,
LB.LeaveTypeId,[LeaveType]=LT.Title,[Allocated]=LB.TotalLeave,
[Pending]=ISNULL((SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory LH Where LH.[Status] IN('Pending','Recommended') AND LH.EmployeeId=EMP.EmployeeId AND LH.LeaveTypeId=LB.LeaveTypeId
AND CAST(LH.LeaveDate AS DATE) BETWEEN @LeaveStartDate AND @LeaveEndDate),0),
[Approved]=ISNULL((SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory LH Where LH.[Status] ='Approved' AND LH.EmployeeId=EMP.EmployeeId AND LH.LeaveTypeId=LB.LeaveTypeId AND CAST(LH.LeaveDate AS DATE) >= CAST(GETDATE() AS DATE)),0),
[Availed]=ISNULL((SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory LH Where LH.[Status] ='Approved' AND LH.EmployeeId=EMP.EmployeeId AND LH.LeaveTypeId=LB.LeaveTypeId AND CAST(LH.LeaveDate AS DATE) < CAST(GETDATE() AS DATE)),0),
[Applied]=ISNULL((SELECT SUM([Count]) FROM HR_EmployeeLeaveHistory LH Where LH.[Status] IN('Pending','Recommended','Approved') AND LH.LeaveTypeId=LB.LeaveTypeId AND LH.EmployeeId=EMP.EmployeeId AND CAST(LH.LeaveDate AS DATE) BETWEEN @LeaveStartDate AND @LeaveEndDate),0)
FROM HR_EmployeeLeaveBalance LB
INNER JOIN HR_EmployeeInformation EMP ON LB.EmployeeId =EMP.EmployeeId
INNER JOIN HR_LeaveTypes LT ON LB.LeaveTypeId = LT.Id
Where LB.EmployeeId=@EmployeeId AND LB.CompanyId=@CompanyId AND LB.OrganizationId=@OrganizationId) tbl";

                list = await _dapper.SqlQueryListAsync<LeaveCardLeaveBalanceSummary>(user.Database, query, new { EmployeeId = filter.EmployeeId, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId, LeaveStartDate = filter.LeaveStartDate, LeaveEndDate = filter.LeaveEndDate });

                foreach (var item in list)
                {
                    if (item != null)
                    {
                        var calculateToDate = DateTime.Now;

                        if (item.LeaveEndDate != null)
                        {
                            if (calculateToDate.Date > item.LeaveEndDate.Value.Date)
                            {
                                calculateToDate = item.LeaveEndDate.Value.Date;
                            }

                            if(item.LastWrokingDate != null)
                            {
                                if(item.LastWrokingDate.Value > item.JoiningDate && item.LastWrokingDate.Value < item.LeaveEndDate.Value)
                                {
                                    calculateToDate = item.LastWrokingDate.Value;
                                }
                            }
                        }
                        var calculateFromDate = DateTime.Now.Date;
                        if (item.LeaveStartDate != null)
                        {
                            if (calculateFromDate > item.LeaveStartDate.Value.Date)
                            {
                                calculateFromDate = item.LeaveStartDate.Value.Date;
                            }

                            if (item.JoiningDate != null)
                            {
                                if (item.JoiningDate.Value > calculateFromDate)
                                {
                                    calculateFromDate = item.JoiningDate.Value;
                                }
                            }

                        }
                        var monthDiffInLeaveYear = calculateFromDate.GetMonthDiffIncludingThisMonth(item.LeaveEndDate.Value);
                        var monthDiff = calculateFromDate.GetMonthDiffIncludingThisMonth(calculateToDate);
                        item.Applicable = Math.Round(((item.Allocated / monthDiffInLeaveYear) * monthDiff), MidpointRounding.AwayFromZero);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetEmployeeLeaveBalanceSummaryForLeaveCardAsync", user);
            }
            return list;
        }
    }
}