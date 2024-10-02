using Dapper;
using System.Data;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using BLL.Administration.Interface;
using Shared.OtherModels.DataService;
using BLL.Employee.Interface.Info;
using DAL.DapperObject.Interface;
using Shared.Employee.Domain.Info;
using DAL.Context.Employee;
using Microsoft.EntityFrameworkCore;

namespace BLL.Employee.Implementation.Info
{
    public class InfoBusiness : IInfoBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IClientDatabase _clientDatabase;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        public InfoBusiness(
            IClientDatabase clientDatabase, IBranchInfoBusiness branchInfoBusiness, IDapperData dapper, ISysLogger sysLogger,
            EmployeeModuleDbContext employeeModuleDbContext)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _clientDatabase = clientDatabase;
            _employeeModuleDbContext = employeeModuleDbContext;
            _branchInfoBusiness = branchInfoBusiness;
        }

        /// <summary>
        /// To use in dropdown, select2, typehead, autocomplete
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeeServiceDataAsync(EmployeeService_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeServiceDataViewModel> list = new List<EmployeeServiceDataViewModel>();
            try
            {
                var sp_name = @"SELECT EMP.EmployeeId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,EMP.BranchId,DTL.LegalName,'' AS BranchName,GRD.GradeId,GRD.GradeName,EMP.DesignationId,DEG.DesignationName,
	EMP.DepartmentId,DPT.DepartmentName,SEC.SectionName,EMP.SectionId,SUB.SubSectionName,EMP.SubSectionId,EMP.IsActive,EMP.TerminationDate,EMP.TerminationStatus,EMP.OfficeEmail, EMP.OfficeMobile, DTL.PresentAddress,
	DTL.MaritalStatus,PreviousReviewId=0,EMP.JobType,DTL.Religion,DTL.IsResidential,EMP.IsPFMember
	FROM HR_EmployeeInformation EMP
	LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
	LEFT JOIN HR_Designations DEG ON EMP.DesignationId = DEG.DesignationId
	LEFT JOIN HR_Grades GRD ON DEG.GradeId = GRD.GradeId
	LEFT JOIN HR_Departments DPT ON EMP.DepartmentId = DPT.DepartmentId
	LEFT JOIN HR_Sections SEC ON EMP.SectionId = SEC.SectionId
	LEFT JOIN HR_SubSections SUB ON EMP.SubSectionId = SUB.SubSectionId
	Where 1=1
	 AND (@EmployeeId IS NULL OR @EmployeeId=0 OR EMP.EmployeeId=@EmployeeId)
	 AND (@EmployeeCode IS NULL OR @EmployeeCode='' OR EMP.EmployeeCode=@EmployeeCode)
	 AND (@EmployeeName IS NULL OR @EmployeeName='' OR EMP.FullName=@EmployeeName)
	 AND (@Gender IS NULL OR @Gender='' OR DTL.Gender=@Gender)
	 AND (@BranchId IS NULL OR @BranchId=0 OR EMP.BranchId=@BranchId)
	 AND (@GradeId IS NULL OR @GradeId=0 OR EMP.GradeId=@GradeId)
	 AND (@DesignationId IS NULL OR @DesignationId=0 OR EMP.DesignationId=@DesignationId)
	 AND (@ZoneId IS NULL OR @ZoneId=0)
	 AND (@UnitId IS NULL OR @UnitId=0)
     AND (@JobType IS NULL OR @JobType ='' OR EMP.JobType=@JobType)
	 AND (@DepartmentId IS NULL OR @DepartmentId=0 OR EMP.DepartmentId=@DepartmentId)
	 AND (@SectionId IS NULL OR @SectionId=0 OR EMP.SectionId=@SectionId)
	 AND (@SubSectionId IS NULL OR @SubSectionId=0 OR EMP.SubSectionId=@SubSectionId)
	 AND (@IsActive IS NULL OR @IsActive ='' OR EMP.IsActive=@IsActive)
	 AND (@IncludedEmployeeCode IS NULL OR @IncludedEmployeeCode ='' OR EMP.EmployeeCode IN (Select [Value] from fn_split_string_to_column(@IncludedEmployeeCode,',')))
	 AND (@IncludedEmployeeId IS NULL OR @IncludedEmployeeId ='' OR EMP.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@IncludedEmployeeId,',')))
	 AND (@ExcludedEmployeeCode IS NULL OR @ExcludedEmployeeCode ='' OR ((@ExcludedEmployeeCode ='' OR EMP.EmployeeCode NOT IN (Select [Value] from fn_split_string_to_column(@ExcludedEmployeeCode,',')))))
	 AND (@ExcludedEmployeeId IS NULL OR @ExcludedEmployeeId ='' OR ((@ExcludedEmployeeId ='' OR EMP.EmployeeId NOT IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@ExcludedEmployeeId,',')))))
	 AND (@MartialStatus IS NULL OR @MartialStatus ='' OR DTL.MaritalStatus = @MartialStatus)
	 AND (@TerminationStatus IS NULL OR @TerminationStatus ='' OR EMP.TerminationStatus = @TerminationStatus)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId
	Order By EMP.EmployeeId";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<EmployeeServiceDataViewModel>(user.Database, sp_name.Trim(), parameters, CommandType.Text);

                if (list.Any())
                {
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", user);
                    foreach (var item in list)
                    {
                        var thisBranch = branchs.FirstOrDefault(i => item.BranchId == i.BranchId);
                        if (thisBranch != null)
                        {
                            item.BranchName = thisBranch.BranchName;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeServiceDataAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveEmployeeAsync(EmployeeInit employee, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_Insert_EmployeeInformation_Extension";
                employee.PaymentInfo.PaymentMode = employee.PaymentInfo.PaymentMode == "Bank" ? "Bank" : employee.PaymentInfo.PaymentMode == "bKash" || employee.PaymentInfo.PaymentMode == "Nagad" || employee.PaymentInfo.PaymentMode == "Rocket" ? "Mobile Banking" : "";

                employee.PaymentInfo.AgentName = employee.PaymentInfo.PaymentMode == "Bank" ? null : employee.PaymentInfo.PaymentMode;
                var parameters = DapperParam.AddParams(employee.ProfessionalInfo, user);
                DapperParam.AddDappperParams(employee.PersonalInfo, new string[] { "EmployeeId", "EmployeeDetailId" }, ref parameters);
                DapperParam.AddDappperParams(employee.PaymentInfo, ref parameters);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeServiceDataAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateEmployeeAsync(EmployeeInit employee, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sqlQuery = "sp_HR_Validate_EmployeeInformation_Extension";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employee.ProfessionalInfo.EmployeeId);
                parameters.Add("EmployeeCode", employee.ProfessionalInfo.EmployeeCode);
                parameters.Add("OfficeEmail", employee.ProfessionalInfo.OfficeEmail);
                parameters.Add("OfficeMobile", employee.ProfessionalInfo.OfficeMobile);
                parameters.Add("FingerId", employee.ProfessionalInfo.FingerId == null ? "" : employee.ProfessionalInfo.FingerId.Trim());
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "ValidateEmployeeAsync", user);
            }
            return executionStatus;
        }
        public async Task<EmployeeOfficeInfoViewModel> GetEmployeeOfficeInfoByIdAsync(EmployeeOfficeInfo_Filter filter, AppUser user)
        {
            EmployeeOfficeInfoViewModel employeeOffice = new EmployeeOfficeInfoViewModel();
            try
            {
                var query = $@"SELECT EMP.EmployeeId,EMP.EmployeeCode,FirstName=ISNULL(EMP.FirstName,EMP.FullName),EMP.LastName,EMP.FullName,DESIG.DesignationId,
	DESIG.DesignationName,DEPT.DepartmentId,DEPT.DepartmentName,EMP.BranchId,'' AS BranchName,EMP.SectionId,
	EMP.SubSectionId,DTL.Gender,EMP.StateStatus,EMP.IsActive,DateOfJoining=CONVERT(NVARCHAR(20),EMP.DateOfJoining,106),EMP.JobType,EMP.WorkShiftId,
	[WorkShiftName]=SFT.[Name],EMP.OfficeMobile,EMP.OfficeEmail,EMP.ReferenceNo,EMP.FingerId,EMP.JobType,
	EMP.Taxzone,EMP.MinimumTaxAmount,EMP.StateStatus,EMP.SectionId,SEC.SectionName,EMP.CostCenterId,EMP.EmployeeTypeId,EMP.JobCategoryId,EMP.ProbationMonth,
    SupervisorId=Hierarchy.SupervisorId,HODId=Hierarchy.HeadOfDepartmentId
	,PreviousReviewId=ISNULL((Select TOP 1 SalaryReviewInfoId From Payroll_SalaryReviewInfo Where EmployeeId=emp.EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND IsApproved=1 Order By SalaryReviewInfoId desc),0),EMP.IsPFMember
	FROM HR_EmployeeInformation EMP
	LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId=DTL.EmployeeId
	LEFT JOIN HR_Designations DESIG ON EMP.DesignationId = DESIG.DesignationId
	LEFT JOIN HR_Departments DEPT ON EMP.DepartmentId = DEPT.DepartmentId
	LEFT JOIN HR_WorkShifts SFT ON EMP.WorkShiftId = SFT.WorkShiftId
	LEFT JOIN HR_Sections SEC ON EMP.SectionId = SEC.SectionId
	LEFT JOIN (SELECT DISTINCT EmployeeId,SupervisorId,HeadOfDepartmentId FROM HR_EmployeeHierarchy EH Where EH.IsActive=1) Hierarchy ON Hierarchy.EmployeeId= EMP.EmployeeId
	WHERE 1=1
	AND (@EmployeeId IS NULL OR @EmployeeId ='' OR @EmployeeId='0' OR EMP.EmployeeId=@EmployeeId)
	AND (@EmployeeCode IS NULL OR @EmployeeCode='' OR @EmployeeCode='0' OR EMP.EmployeeCode=@EmployeeCode)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId";
                var parameters = DapperParam.AddParams(filter, user, new string[] { "UserId" });
                employeeOffice = await _dapper.SqlQueryFirstAsync<EmployeeOfficeInfoViewModel>(user.Database, query, new
                {
                    filter.EmployeeId,
                    filter.EmployeeCode,
                    user.CompanyId,
                    user.OrganizationId
                }, CommandType.Text);

                if (employeeOffice != null && employeeOffice.EmployeeId > 0)
                {
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", user);
                    var thisBranch = branchs.FirstOrDefault(i => employeeOffice.BranchId == i.BranchId);
                    if (thisBranch != null)
                    {
                        employeeOffice.BranchName = thisBranch.BranchName;
                    }

                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeOfficeInfoByIdAsync", user);
            }
            return employeeOffice;
        }
        public async Task<DBResponse<EmployeePersonalInfoList>> GetEmployeePersonalInfosAsync(EmployeePersonalInfoQuery filters, AppUser user)
        {
            DBResponse<EmployeePersonalInfoList> data = new DBResponse<EmployeePersonalInfoList>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = $@"WITH Data_CTE AS(
	SELECT EMP.EmployeeId,ED.EmployeeDetailId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,ED.FatherName,ED.MotherName,ED.MaritalStatus,ED.SpouseName,
	ED.Gender,ED.PersonalEmailAddress,ED.PersonalMobileNo,ED.DateOfBirth,ED.Feet,ED.Inch,ED.PresentAddress,ED.PermanentAddress,
	ED.EmergencyContactPerson,ED.EmergencyContactAddress,ED.RelationWithEmergencyContactPerson,ED.IsResidential,
	ED.EmergencyContactNo,ED.BloodGroup,ED.Religion
	FROM HR_EmployeeDetail ED
	INNER JOIN HR_EmployeeInformation EMP ON ED.EmployeeId = EMP.EmployeeId
	WHERE 1=1
	AND (ISNULL(@EmployeeId,0) = 0  OR EMP.EmployeeId=@EmployeeId)
	AND (ISNULL(@EmployeeCode,'') = '' OR EMP.EmployeeCode LIKE '%'+@EmployeeCode+'%')
	AND (ISNULL(@FullName,'') = '' OR EMP.FullName LIKE '%'+@FullName+'%')
	AND (ISNULL(@Gender,'') = '' OR ED.Gender=@Gender)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId),
	Count_CTE AS (
	SELECT COUNT(*) AS [TotalRows]
	FROM Data_CTE)

	SELECT JSONData=(Select * From (SELECT *
	FROM Data_CTE
	ORDER BY EmployeeId
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = Utility.DappperParams(filters, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.Text);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeePersonalInfoList>>(response.JSONData) ?? new List<EmployeePersonalInfoList>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeePersonalInfosAsync", user);
            }
            return data;
        }
        public async Task<EmployeePersonalInfoList> GetEmployeePersonalInfoByIdAsync(EmployeePersonalInfoQuery query, AppUser user)
        {
            var data = new EmployeePersonalInfoList();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = $@"WITH Data_CTE AS(
	SELECT EMP.EmployeeId,ED.EmployeeDetailId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,ED.LegalName,ED.FatherName,ED.MotherName,ED.MaritalStatus,ED.SpouseName,
ED.Gender,ED.PersonalEmailAddress,ED.PersonalMobileNo,ED.DateOfBirth,ED.Feet,ED.Inch,ED.PresentAddress,ED.PermanentAddress,
ED.IsResidential,
ED.BloodGroup,ED.Religion,ED.NumberOfChild,
ED.EmergencyContactPerson,ED.EmergencyContactAddress,ED.RelationWithEmergencyContactPerson,ED.EmergencyContactNo,ED.EmergencyContactEmailAddress,
ED.EmergencyContactPerson2,ED.EmergencyContactAddress2,ED.RelationWithEmergencyContactPerson2,ED.EmergencyContactNo2,ED.EmergencyContactEmailAddress2
FROM HR_EmployeeDetail ED
INNER JOIN HR_EmployeeInformation EMP ON ED.EmployeeId = EMP.EmployeeId
WHERE 1=1
	AND (ISNULL(@EmployeeId,0) = 0  OR EMP.EmployeeId=@EmployeeId)
	AND (ISNULL(@EmployeeCode,'') = '' OR EMP.EmployeeCode LIKE '%'+@EmployeeCode+'%')
	AND (ISNULL(@FullName,'') = '' OR EMP.FullName LIKE '%'+@FullName+'%')
	AND (ISNULL(@Gender,'') = '' OR ED.Gender=@Gender)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId),
	Count_CTE AS (
	SELECT COUNT(*) AS [TotalRows]
	FROM Data_CTE)

	SELECT JSONData=(Select * From (SELECT *
	FROM Data_CTE
	ORDER BY EmployeeId
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = DapperParam.AddParams(query, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.Text);
                data = Utility.JsonToObject<IEnumerable<EmployeePersonalInfoList>>(response.JSONData).FirstOrDefault() ?? new EmployeePersonalInfoList();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeePersonalInfoByIdAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveEmploymentApprovalAsync(EmployeeApprovalDTO model, AppUser user)
        {
            ExecutionStatus execution = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmploymentApproval";
                var parameters = DapperParam.AddParams(model, user);
                execution = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                execution = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "SaveEmploymentApprovalAsync", user);
            }
            return execution;
        }
        public async Task<ExecutionStatus> SaveEmployeeProfessionalInfoAsync(EmployeeOfficeInfo professionalInfo, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeOfficeInfo_Insert_Update";
                var parameters = Utility.DappperParams(professionalInfo, user, new string[] { "FullName" });
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "SaveEmployeeProfessionalInfoAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveEmployeePersonalInfoAsync(EmployeePersonalInfo personalInfo, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeePersonalInfo_Insert_Update";
                var parameters = Utility.DappperParams(personalInfo, user);
                parameters.Add("ExecutionFlag", personalInfo.EmployeeDetailId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "SaveEmployeePersonalInfoAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<EmployeeInformationViewModel>> GetEmployeesAsync(EmployeeQuery employeeQuery, AppUser User)
        {
            DBResponse<EmployeeInformationViewModel> data = new DBResponse<EmployeeInformationViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = $@"WITH Data_CTE AS(
	SELECT EMP.EmployeeId,EMP.EmployeeCode,EMP.FullName,DESIG.DesignationId,DESIG.DesignationName,
	DEPT.DepartmentId,DEPT.DepartmentName,EMP.BranchId,'' AS BranchName,DTL.Gender,EMP.StateStatus,
    [IsActive]=(
	CASE 
    WHEN EMP.TerminationDate IS NOT NULL AND EMP.TerminationStatus ='Approved' AND CAST(EMP.TerminationDate AS DATE) < CAST(GETDATE() AS DATE) THEN 'False'
    WHEN EMP.IsActive IS NULL THEN 'False' 
	ELSE EMP.IsActive  END),
	EMP.DateOfJoining,EMP.JobType,EMP.WorkShiftId,[WorkShiftName]=( CASE 
	WHEN EMP.WorkShiftId > 0 THEN SFT.[Name] + ' '+ SUBSTRING(CAST(SFT.StartTime AS NVARCHAR(50)),1,5) + '-'+ SUBSTRING(CAST(SFT.EndTime AS NVARCHAR(50)),1,5) ELSE '' END),
    EMP.OfficeEmail,
    Supervisor=EH.SupervisorId,
    SupervisorName = (SELECT (FullName+ ' ['+ EmployeeCode+']') FROM HR_EmployeeInformation Where EmployeeId=EH.SupervisorId), 
    HeadOfDepartment=EH.HeadOfDepartmentId,
    HeadOfDepartmentName=(SELECT (FullName+ ' ['+ EmployeeCode+']') FROM HR_EmployeeInformation Where EmployeeId=EH.HeadOfDepartmentId)
	FROM HR_EmployeeInformation EMP
	LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId=DTL.EmployeeId
	LEFT JOIN HR_Designations DESIG ON EMP.DesignationId = DESIG.DesignationId
	LEFT JOIN HR_Departments DEPT ON EMP.DepartmentId = DEPT.DepartmentId
	LEFT JOIN HR_WorkShifts SFT ON EMP.WorkShiftId = SFT.WorkShiftId
    LEFT JOIN (SELECT EmployeeId,SupervisorId,HeadOfDepartmentId 
    FROM HR_EmployeeHierarchy
    Where IsActive=1) EH ON EMP.EmployeeId = EH.EmployeeId
	WHERE 1=1
	AND (ISNULL(@EmployeeId,0) = 0  OR EMP.EmployeeId=@EmployeeId)
	AND (ISNULL(@EmployeeCode,'') = '' OR EMP.EmployeeCode =@EmployeeCode)
	AND (ISNULL(@FullName,'') = '' OR EMP.FullName LIKE '%'+@FullName+'%')
	AND (ISNULL(@StateStatus,'')='' OR EMP.StateStatus = @StateStatus)
	AND (ISNULL(@DesignationId,0) = 0 OR EMP.DesignationId =@DesignationId)
    AND (ISNULL(@DepartmentId,0) = 0 OR EMP.DepartmentId=@DepartmentId)
	AND (ISNULL(@DateOfJoining,'') = '' OR EMP.DateOfJoining=TRY_CAST(@DateOfJoining AS DATE))
	AND (ISNULL(@BranchId,0) = 0 OR EMP.BranchId=@BranchId)
	AND (ISNULL(@Gender,'') = '' OR DTL.Gender=@Gender)
    AND (ISNULL(@JobStatus,'') = '' OR (
	CASE WHEN EMP.IsActive IS NULL THEN 1 
	WHEN EMP.TerminationDate IS NOT NULL AND EMP.TerminationStatus ='Approved' AND CAST(EMP.TerminationDate AS DATE) < CAST(GETDATE() AS DATE) THEN 0
	ELSE EMP.IsActive END)=@JobStatus)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId
	),
	Count_CTE AS (
	SELECT COUNT(*) AS [TotalRows]
	FROM Data_CTE)

	--SELECT * FROM Data_CTE
	SELECT JSONData=(Select * From (SELECT *
	FROM Data_CTE
	ORDER BY 
	CASE WHEN ISNULL(@SortingCol,'') = '' THEN TRY_CAST(EmployeeCode AS BIGINT) END,
	CASE WHEN @SortingCol = 'EmployeeCode' AND @SortType ='ASC' THEN EmployeeCode END ASC,
	CASE WHEN @SortingCol = 'EmployeeCode' AND @SortType ='DESC' THEN EmployeeCode END DESC,
	CASE WHEN @SortingCol = 'FullName' AND @SortType ='ASC' THEN FullName END ,
	CASE WHEN @SortingCol = 'FullName' AND @SortType ='DESC' THEN FullName END DESC
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber";
                var parameters = DapperParam.AddParams(employeeQuery, User, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(User.Database, sp_name, parameters, CommandType.Text);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeInformationViewModel>>(response.JSONData) ?? new List<EmployeeInformationViewModel>();
                if (data.ListOfObject.Any())
                {
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", User);
                    foreach (var item in data.ListOfObject)
                    {
                        var thisBranch = branchs.FirstOrDefault(i => item.BranchId == i.BranchId);
                        if (thisBranch != null)
                        {
                            item.BranchName = thisBranch.BranchName;
                        }
                    }
                }
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "EmployeeBusiness", "SaveEmployeeAsync", User.Username, User.OrganizationId, User.CompanyId, User.BranchId);
            }
            return data;
        }
        public async Task<IEnumerable<EmployeeInformationViewModel>> GetClientEmployeesAsync(long clientCompany, long clientOrganization, AppUser user)
        {
            IEnumerable<EmployeeInformationViewModel> list = new List<EmployeeInformationViewModel>();
            var clientDatabase = _clientDatabase.GetDatabaseName(clientOrganization);
            try
            {
                var sp_name = $@"Select emp.*,grade.GradeName,desig.DesignationName,--div.DivisionName,
	'' AS BranchName,dept.DepartmentName,
	sec.SectionName,subsec.SubSectionName,JobStatusName=(CASE WHEN ISNULL(emp.IsActive,0)=0 THEN 'Inactive' 
	WHEN ISNULL(emp.IsActive,0)=1 THEN 'Active' ELSE 'Inactive' END),'' as 'JobLocationName',emp.JobType,
	ISNULL(DTL.Gender,'N/A') 'Gender',
	ISNULL(DTL.Feet,'N/A')'Feet',ISNULL(DTL.Inch,'N/A')'Inch',0 as 'BloodGroupId','',0 as 'ReligionId',ReligionName=dtl.Religion,0 'NationalityId','' 'NationalityName',
	ISNULL(DTL.MaritalStatus,'N/A') as 'MaritalStatus',ISNULL(DTL.RelationWithEmergencyContactPerson,'N/A') as 'RelationWithEmergencyContactPerson',
	DTL.PersonalMobileNo,emp.DateOfConfirmation,emp.DateOfJoining,emp.OfficeEmail,emp.OfficeMobile,emp.TINNo,emp.StateStatus,ws.WorkShiftId, (ws.Title+'#'+ws.[Name]) 'WorkShiftName',ws.WorkShiftId as 'CurrentWorkShiftId',
	PreviousReviewId=ISNULL((Select TOP 1 SalaryReviewInfoId From Payroll_SalaryReviewInfo Where EmployeeId=emp.EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND IsApproved=1 Order By SalaryReviewInfoId desc),0)
	From HR_EmployeeInformation emp
	LEFT Join HR_Designations Desig on emp.DesignationId = desig.DesignationId
	LEFT Join HR_Grades grade on Desig.GradeId = grade.GradeId
	Left Join HR_Departments dept on emp.DepartmentId = dept.DepartmentId
	Left Join HR_Sections sec on emp.SectionId = sec.SectionId
	Left Join HR_SubSections subsec on emp.SubSectionId = subsec.SubSectionId
	Left Join HR_EmployeeDetail DTL ON emp.EmployeeId = dtl.EmployeeId
	Left Join (Select * From fnEmployeeCurrentShift(0,@CompanyId,@OrganizationId)) ws on emp.EmployeeId = ws.EmployeeId
	Where 1=1
	AND (
			(@Id > 0 AND emp.EmployeeId = @Id) OR 
			(@EmployeeIds !='' AND emp.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@EmployeeIds,','))) OR
			(@Id = 0 AND @EmployeeIds='')
		)
	AND (@DesignationId =0 OR emp.DesignationId = @DesignationId)
	AND (@DepartmentId =0 OR emp.DepartmentId = @DepartmentId)
	AND (@SectionId =0 OR emp.SectionId = @SectionId)
	AND (@SubSectionId =0 OR emp.SubSectionId = @SubSectionId)
	AND (@BranchId =0 OR emp.BranchId =@BranchId)
	AND (@JobStatus IS NULL OR emp.IsActive=@JobStatus)
	AND emp.CompanyId = @CompanyId
	AND emp.OrganizationId = @OrganizationId
	Order By EmployeeId desc";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", clientCompany);
                parameters.Add("OrganizationId", clientOrganization);

                list = await _dapper.SqlQueryListAsync<EmployeeInformationViewModel>(clientDatabase, sp_name, parameters, commandType: CommandType.Text);

                if (list.Any())
                {
                    user.CompanyId = clientCompany;
                    user.OrganizationId = clientOrganization;
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", user);
                    foreach (var item in list)
                    {
                        var thisBranch = branchs.FirstOrDefault(i => item.BranchId == i.BranchId);
                        if (thisBranch != null)
                        {
                            item.BranchName = thisBranch.BranchName;
                            item.JobLocationName = thisBranch.BranchName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetClientEmployeesAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<EmployeeInformationViewModel>> GetClientEmployeeByIdAsync(long employeeId, long clientCompany, long clientOrganization, AppUser user)
        {
            IEnumerable<EmployeeInformationViewModel> list = new List<EmployeeInformationViewModel>();
            var clientDatabase = _clientDatabase.GetDatabaseName(clientOrganization);
            try
            {
                var sp_name = $@"Select emp.*,grade.GradeName,desig.DesignationName,--div.DivisionName,
	'' AS BranchName,dept.DepartmentName,
	sec.SectionName,subsec.SubSectionName,JobStatusName=(CASE WHEN ISNULL(emp.IsActive,0)=0 THEN 'Inactive' 
	WHEN ISNULL(emp.IsActive,0)=1 THEN 'Active' ELSE 'Inactive' END),'' as 'JobLocationName',emp.JobType,
	ISNULL(DTL.Gender,'N/A') 'Gender',
	ISNULL(DTL.Feet,'N/A')'Feet',ISNULL(DTL.Inch,'N/A')'Inch',0 as 'BloodGroupId','',0 as 'ReligionId',ReligionName=dtl.Religion,0 'NationalityId','' 'NationalityName',
	ISNULL(DTL.MaritalStatus,'N/A') as 'MaritalStatus',ISNULL(DTL.RelationWithEmergencyContactPerson,'N/A') as 'RelationWithEmergencyContactPerson',
	DTL.PersonalMobileNo,emp.DateOfConfirmation,emp.DateOfJoining,emp.OfficeEmail,emp.OfficeMobile,emp.TINNo,emp.StateStatus,ws.WorkShiftId, (ws.Title+'#'+ws.[Name]) 'WorkShiftName',ws.WorkShiftId as 'CurrentWorkShiftId',
	PreviousReviewId=ISNULL((Select TOP 1 SalaryReviewInfoId From Payroll_SalaryReviewInfo Where EmployeeId=emp.EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND IsApproved=1 Order By SalaryReviewInfoId desc),0)
	From HR_EmployeeInformation emp
	LEFT Join HR_Designations Desig on emp.DesignationId = desig.DesignationId
	LEFT Join HR_Grades grade on Desig.GradeId = grade.GradeId
	Left Join HR_Departments dept on emp.DepartmentId = dept.DepartmentId
	Left Join HR_Sections sec on emp.SectionId = sec.SectionId
	Left Join HR_SubSections subsec on emp.SubSectionId = subsec.SubSectionId
	Left Join HR_EmployeeDetail DTL ON emp.EmployeeId = dtl.EmployeeId
	Left Join (Select * From fnEmployeeCurrentShift(0,@CompanyId,@OrganizationId)) ws on emp.EmployeeId = ws.EmployeeId
	Where 1=1

	AND (@Id > 0 AND emp.EmployeeId = @Id) 
	--AND (@DesignationId =0 OR emp.DesignationId = @DesignationId)
	--AND (@DepartmentId =0 OR emp.DepartmentId = @DepartmentId)
	--AND (@SectionId =0 OR emp.SectionId = @SectionId)
	--AND (@SubSectionId =0 OR emp.SubSectionId = @SubSectionId)
	--AND (@BranchId =0 OR emp.BranchId =@BranchId)
	--AND (@JobStatus IS NULL OR emp.IsActive=@JobStatus)
	AND emp.CompanyId = @CompanyId
	AND emp.OrganizationId = @OrganizationId
	Order By EmployeeId desc";
                var parameters = new DynamicParameters();
                parameters.Add("Id", employeeId);
                parameters.Add("CompanyId", clientCompany);
                parameters.Add("OrganizationId", clientOrganization);

                list = await _dapper.SqlQueryListAsync<EmployeeInformationViewModel>(clientDatabase, sp_name, parameters, commandType: CommandType.Text);

                if (list.Any())
                {
                    user.CompanyId = clientCompany;
                    user.OrganizationId = clientOrganization;
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", user);
                    foreach (var item in list)
                    {
                        var thisBranch = branchs.FirstOrDefault(i => item.BranchId == i.BranchId);
                        if (thisBranch != null)
                        {
                            item.BranchName = thisBranch.BranchName;
                            item.JobLocationName = thisBranch.BranchName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetClientEmployeesAsync", user);
            }
            return list;
        }
        public async Task<ProfileInfo> GetEmployeeProfileInfoAsync(long employeeId, AppUser user)
        {
            ProfileInfo profileInfo = new ProfileInfo();
            try
            {
                var query = $@"Select EI.EmployeeId,EI.EmployeeCode,EmployeeName=EI.FullName,'' AS BranchName,DESIG.DesignationName,
	DEPT.DepartmentName,SEC.SectionName,SUB.SubSectionName,EI.DateOfJoining,EI.DateOfConfirmation,ED.DateOfBirth,EI.OfficeEmail,
	EI.OfficeMobile,EI.ReferenceNo,EI.FingerId,EI.JobType,EI.IsActive,EI.StateStatus,ED.Gender,
	PhotoPath= SUBSTRING((ED.PhotoPath+'/'+ED.Photo),CHARINDEX('/',(ED.PhotoPath+'/'+ED.Photo))+1,200),EI.BranchId
	From HR_EmployeeInformation EI
	LEFT Join HR_EmployeeDetail  ED ON EI.EmployeeId = ED.EmployeeId
	LEFT JOIN HR_Designations DESIG ON EI.DesignationId = DESIG.DesignationId
	LEFT JOIN HR_Departments DEPT ON EI.DepartmentId = DEPT.DepartmentId
	LEFT JOIN HR_Sections SEC ON EI.SectionId = SEC.SectionId
	LEFT JOIN HR_SubSections SUB ON EI.SubSectionId = SUB.SubSectionId
	WHERE 1=1
	AND EI.EmployeeId =@EmployeeId
	AND EI.CompanyId =@CompanyId
	AND EI.OrganizationId =@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                profileInfo = await _dapper.SqlQueryFirstAsync<ProfileInfo>(user.Database, query, parameters, commandType: CommandType.Text);

                if (profileInfo != null && profileInfo.EmployeeId > 0)
                {
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", user);
                    var thisBranch = branchs.FirstOrDefault(i => profileInfo.BranchId == i.BranchId);
                    if (thisBranch != null)
                    {
                        profileInfo.BranchName = thisBranch.BranchName;
                    }

                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeProfileInfoAsync", user);
            }
            return profileInfo;
        }
        public async Task<ExecutionStatus> UploadProfileImageAsync(UploadProfileImage model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            string file = null;
            string filePath = null;
            string fileName = null;
            string extenstion = null;
            string fileSize = null;
            string actualFileName = null;
            try
            {
                if (model.EmployeeId > 0 && model.Image == null)
                {
                    // Persist Existing 
                }
                else if (model.EmployeeId > 0 && model.Image != null)
                {
                    Utility.DeleteFile(string.Format(@"{0}/{1}", Utility.PhysicalDriver, model.ImagePath));
                    file = await Utility.SaveFileAsync(model.Image, string.Format(@"{0}", user.OrgCode));
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(model.Image.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = model.Image.FileName;
                }
                else
                {
                    file = await Utility.SaveFileAsync(model.Image, string.Format(@"{0}", user.OrgCode));
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(model.Image.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = model.Image.FileName;
                }

                var query = "sp_HR_UploadProfileImage";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", model.EmployeeId);
                parameters.Add("Photo", fileName ?? "");
                parameters.Add("PhotoPath", filePath ?? "");
                parameters.Add("ActualPhotoName", actualFileName ?? "");
                parameters.Add("PhotoSize", fileSize ?? "");
                parameters.Add("PhotoFormat", extenstion ?? "");
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, query, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "UploadProfileImageAsync", user);
            }
            return executionStatus;
        }
        public async Task<EmployeeInformationViewModel> GetEmployeeAsync(long employeeId, AppUser user)
        {
            try
            {
                var sp_name = string.Format(@"Exec sp_HR_Employee_List @Id={0},@CompanyId={1},@OrganizationId={2}", employeeId, user.CompanyId, user.OrganizationId);
                sp_name = Utility.ParamChecker(sp_name);
                var data = await _dapper.SqlQueryFirstAsync<EmployeeInformationViewModel>(user.Database, sp_name);
                return data;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeesForShiftAssignAsync(EmployeeService_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeServiceDataViewModel> list = new List<EmployeeServiceDataViewModel>();
            try
            {
                var sp_name = @"SELECT EMP.EmployeeId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,EMP.BranchId,'' AS BranchName,GRD.GradeId,GRD.GradeName,EMP.DesignationId,DEG.DesignationName,
	EMP.DepartmentId,DPT.DepartmentName,SEC.SectionName,EMP.SectionId,SUB.SubSectionName,EMP.SubSectionId,EMP.IsActive,EMP.TerminationDate,EMP.TerminationStatus,EMP.OfficeEmail, EMP.OfficeMobile, DTL.PresentAddress,
	DTL.MaritalStatus,PreviousReviewId=ISNULL((Select TOP 1 SalaryReviewInfoId From Payroll_SalaryReviewInfo Where EmployeeId=emp.EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND IsApproved=1 Order By SalaryReviewInfoId desc),0),ws.WorkShiftId, (ws.Title+'#'+ws.[Name]) 'WorkShiftName'
	FROM HR_EmployeeInformation EMP
	LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
	LEFT JOIN HR_Designations DEG ON EMP.DesignationId = DEG.DesignationId
	LEFT JOIN HR_Grades GRD ON DEG.GradeId = GRD.GradeId
	LEFT JOIN HR_Departments DPT ON EMP.DepartmentId = DPT.DepartmentId
	LEFT JOIN HR_Sections SEC ON EMP.SectionId = SEC.SectionId
	LEFT JOIN HR_SubSections SUB ON EMP.SubSectionId = SUB.SubSectionId
	LEFT JOIN (Select employee.EmployeeId,WorkShiftId=ISNULL(ws.WorkShiftId,0),ews.EmployeeWorkShiftId,ws.[Name] 'WorkShiftName',ws.[Name],ws.[Title],ws.[NameDetail],ws.StartTime,ws.EndTime,ws.MaxInTime,
ws.LunchStartTime,ws.LunchEndTime,StartHour= SUBSTRING(CAST(ws.StartTime AS NVARCHAR(50)),1,2),StartMin= SUBSTRING(CAST(ws.StartTime AS NVARCHAR(50)),4,2),
EndHour= SUBSTRING(CAST(ws.EndTime AS NVARCHAR(50)),1,2),EndMin= SUBSTRING(CAST(ws.EndTime AS NVARCHAR(50)),4,2)
From HR_EmployeeInformation employee
Inner Join HR_EmployeeWorkShifts ews on employee.EmployeeId = ews.EmployeeId
Inner Join HR_WorkShifts ws on ews.WorkShiftId = ws.WorkShiftId
Where 1 = 1
AND ews.StateStatus = 'Approved' AND ews.IsApproved=1
AND (Cast(GetDate() as date) >= Cast(ews.ActiveDate as date)  
	AND Cast(GetDate() as date) < ISNULL(Cast(ews.InActiveDate as date),DATEADD(Day,1,Cast(GETDATE() as date))))
AND (employee.CompanyId =@CompanyId)
AND (employee.OrganizationId =@OrganizationId)) ws ON ws.EmployeeId= EMP.EmployeeId
	Where 1=1
	 AND (@EmployeeId IS NULL OR @EmployeeId=0 OR EMP.EmployeeId=@EmployeeId)
	 AND (@EmployeeCode IS NULL OR @EmployeeCode='' OR EMP.EmployeeCode=@EmployeeCode)
	 AND (@EmployeeName IS NULL OR @EmployeeName='' OR EMP.FullName=@EmployeeName)
	 AND (@Gender IS NULL OR @Gender='' OR DTL.Gender=@Gender)
	 AND (@BranchId IS NULL OR @BranchId=0 OR EMP.BranchId=@BranchId)
	 AND (@GradeId IS NULL OR @GradeId=0 OR EMP.GradeId=@GradeId)
	 AND (@DesignationId IS NULL OR @DesignationId=0 OR EMP.DesignationId=@DesignationId)
	 AND (@ZoneId IS NULL OR @ZoneId=0)
	 AND (@UnitId IS NULL OR @UnitId=0)
     AND (@JobType IS NULL OR @JobType ='' OR EMP.JobType=@JobType)
	 AND (@DepartmentId IS NULL OR @DepartmentId=0 OR EMP.DepartmentId=@DepartmentId)
	 AND (@SectionId IS NULL OR @SectionId=0 OR EMP.SectionId=@SectionId)
	 AND (@SubSectionId IS NULL OR @SubSectionId=0 OR EMP.SubSectionId=@SubSectionId)
	 AND (@IsActive IS NULL OR @IsActive ='' OR EMP.IsActive=@IsActive)
	 AND (@IncludedEmployeeCode IS NULL OR @IncludedEmployeeCode ='' OR EMP.EmployeeCode IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@IncludedEmployeeCode,',')))
	 AND (@IncludedEmployeeId IS NULL OR @IncludedEmployeeId ='' OR EMP.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@IncludedEmployeeId,',')))
	 AND (@ExcludedEmployeeCode IS NULL OR @ExcludedEmployeeCode ='' OR ((@ExcludedEmployeeCode ='' OR EMP.EmployeeCode NOT IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@ExcludedEmployeeCode,',')))))
	 AND (@ExcludedEmployeeId IS NULL OR @ExcludedEmployeeId ='' OR ((@ExcludedEmployeeId ='' OR EMP.EmployeeId NOT IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@ExcludedEmployeeId,',')))))
	 AND (@MartialStatus IS NULL OR @MartialStatus ='' OR DTL.MaritalStatus = @MartialStatus)
	 AND (@TerminationStatus IS NULL OR @TerminationStatus ='' OR EMP.TerminationStatus = @TerminationStatus)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId
	Order By EMP.EmployeeId";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<EmployeeServiceDataViewModel>(user.Database, sp_name.Trim(), parameters, CommandType.Text);

                if (list.Any())
                {
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", user);
                    foreach (var item in list)
                    {
                        var thisBranch = branchs.FirstOrDefault(i => item.BranchId == i.BranchId);
                        if (thisBranch != null)
                        {
                            item.BranchName = thisBranch.BranchName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeServiceDataAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeeDataAsync(EmployeeService_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeServiceDataViewModel> list = new List<EmployeeServiceDataViewModel>();
            try
            {
                var sp_name = @"SELECT EMP.EmployeeId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,EMP.BranchId,'' AS BranchName,GRD.GradeId,GRD.GradeName,EMP.DesignationId,DEG.DesignationName,
	EMP.DepartmentId,DPT.DepartmentName,SEC.SectionName,EMP.SectionId,SUB.SubSectionName,EMP.SubSectionId,EMP.IsActive,EMP.TerminationDate,EMP.TerminationStatus,EMP.OfficeEmail, EMP.OfficeMobile, DTL.PresentAddress,
	DTL.MaritalStatus,PreviousReviewId=0,EMP.OfficeMobile
	FROM HR_EmployeeInformation EMP
	LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
	LEFT JOIN HR_Designations DEG ON EMP.DesignationId = DEG.DesignationId
	LEFT JOIN HR_Grades GRD ON DEG.GradeId = GRD.GradeId
	LEFT JOIN HR_Departments DPT ON EMP.DepartmentId = DPT.DepartmentId
	LEFT JOIN HR_Sections SEC ON EMP.SectionId = SEC.SectionId
	LEFT JOIN HR_SubSections SUB ON EMP.SubSectionId = SUB.SubSectionId
	Where 1=1
	 AND (@EmployeeId IS NULL OR @EmployeeId=0 OR EMP.EmployeeId=@EmployeeId)
	 AND (@EmployeeCode IS NULL OR @EmployeeCode='' OR EMP.EmployeeCode=@EmployeeCode)
	 AND (@EmployeeName IS NULL OR @EmployeeName='' OR EMP.FullName=@EmployeeName)
	 AND (@Gender IS NULL OR @Gender='' OR DTL.Gender=@Gender)
	 AND (@BranchId IS NULL OR @BranchId=0 OR EMP.BranchId=@BranchId)
	 AND (@GradeId IS NULL OR @GradeId=0 OR EMP.GradeId=@GradeId)
	 AND (@DesignationId IS NULL OR @DesignationId=0 OR EMP.DesignationId=@DesignationId)
	 AND (@ZoneId IS NULL OR @ZoneId=0)
	 AND (@UnitId IS NULL OR @UnitId=0)
     AND (@JobType IS NULL OR @JobType ='' OR EMP.JobType=@JobType)
	 AND (@DepartmentId IS NULL OR @DepartmentId=0 OR EMP.DepartmentId=@DepartmentId)
	 AND (@SectionId IS NULL OR @SectionId=0 OR EMP.SectionId=@SectionId)
	 AND (@SubSectionId IS NULL OR @SubSectionId=0 OR EMP.SubSectionId=@SubSectionId)
	 AND (@IsActive IS NULL OR @IsActive ='' OR EMP.IsActive=@IsActive)
	 AND (@IncludedEmployeeCode IS NULL OR @IncludedEmployeeCode ='' OR EMP.EmployeeCode IN (Select [Value] from fn_split_string_to_column(@IncludedEmployeeCode,',')))
	 AND (@IncludedEmployeeId IS NULL OR @IncludedEmployeeId ='' OR EMP.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@IncludedEmployeeId,',')))
	 AND (@ExcludedEmployeeCode IS NULL OR @ExcludedEmployeeCode ='' OR ((@ExcludedEmployeeCode ='' OR EMP.EmployeeCode NOT IN (Select [Value] from fn_split_string_to_column(@ExcludedEmployeeCode,',')))))
	 AND (@ExcludedEmployeeId IS NULL OR @ExcludedEmployeeId ='' OR ((@ExcludedEmployeeId ='' OR EMP.EmployeeId NOT IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@ExcludedEmployeeId,',')))))
	 AND (@MartialStatus IS NULL OR @MartialStatus ='' OR DTL.MaritalStatus = @MartialStatus)
	 AND (@TerminationStatus IS NULL OR @TerminationStatus ='' OR EMP.TerminationStatus = @TerminationStatus)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId
	Order By EMP.EmployeeId";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<EmployeeServiceDataViewModel>(user.Database, sp_name.Trim(), parameters, CommandType.Text);

                if (list.Any())
                {
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", user);
                    foreach (var item in list)
                    {
                        var thisBranch = branchs.FirstOrDefault(i => item.BranchId == i.BranchId);
                        if (thisBranch != null)
                        {
                            item.BranchName = thisBranch.BranchName;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeDataAsync", user);
            }
            return list;
        }
        public async Task<DataTable> GetEmployeeInformationForReportAsync(EmployeeInfoReport_Filter filter, bool isForDownloadExcelFile, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                string query = "sp_HR_EmployeeInformation_For_Report";
                filter.Columns = Utility.IsNullEmptyOrWhiteSpace(filter.ColumnsJson) == false ?
                    JsonReverseConverter.JsonToObject<KeyValue>(filter.ColumnsJson, null).ToList() : new List<KeyValue>();
                var parameters = Utility.GetKeyValuePairs(filter, user, new string[] { "Columns", "ColumnsJson" }, addUserId: false);
                dataTable = await _dapper.SqlDataTable(user.Database, query, parameters, CommandType.StoredProcedure);
                if (dataTable.Rows.Count > 0)
                {
                    var branchs = await _branchInfoBusiness.GetBranchsAsync("", user);
                    if (branchs.Any())
                    {
                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            var branchId = Utility.TryParseLong(dataTable.Rows[i]["BranchId"].ToString());
                            if (branchId > 0)
                            {
                                var branchInfo = branchs.FirstOrDefault(item => item.BranchId == branchId);
                                if (branchInfo != null)
                                {
                                    dataTable.Rows[i]["Branch"] = branchInfo.BranchName;
                                }
                            }
                        }
                    }
                    branchs = null;
                    dataTable.RemoveUnwantedColumns(filter.Columns, isForDownloadExcelFile);
                }
            }
            catch (Exception ex)
            {
            }
            return dataTable;
        }
        public async Task<IEnumerable<Dropdown>> GetEmployeeItemsAsync(List<string> items, AppUser user)
        {
            IEnumerable<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                string employeeCode = string.Join(',', items);
                string query = $@"SELECT [Value]=EmployeeId,[Text]=FullName+' ['+EmployeeCode+']' FROM HR_EmployeeInformation
                Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                AND EmployeeCode IN (Select[Value] from fn_split_string_to_column(@EmployeeCode, ','))";


                dropdowns = await _dapper.SqlQueryListAsync<Dropdown>(user.Database, query, new { user.CompanyId, user.OrganizationId, EmployeeCode = employeeCode }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoBusiness", "GetEmployeeItemsAsync", user);
            }
            return dropdowns;
        }
        public async Task<bool> IsOfficeEmailAvailableAsync(long id, string email, AppUser user)
        {
            bool isExist = false;
            try
            {
                isExist = await _employeeModuleDbContext.HR_EmployeeInformation.FirstOrDefaultAsync(i=>i.EmployeeId != id && i.OfficeEmail == email && i.OrganizationId == user.OrganizationId && i.CompanyId  == user.CompanyId) != null;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoBusiness", "IsOfficeEmailAvailableAsync", user);
            }
            return isExist;
        }
        public async Task<bool> IsEmployeeIdAvailableAsync(string code, AppUser user)
        {
            bool isExist = false;
            try
            {
                var query = $@"SELECT TOP 1 EmployeeCode FROM HR_EmployeeInformation Where EmployeeCode = @Code AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var data = await _dapper.SqlQueryFirstAsync<EmployeeServiceDataViewModel>(user.Database, query, new
                {
                    Code = code,
                    user.CompanyId,
                    user.OrganizationId
                }, CommandType.Text);

                if (data != null && data.EmployeeCode == code)
                {
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoBusiness", "IsEmployeeIdAvailableAsync", user);
            }
            return isExist;
        }
        //public async Task<bool> IsOfficeEmailInEditAvailableAsync(long id, string email, AppUser user)
        //{
        //    bool isExist = false;
        //    try
        //    {
        //        var query = $@"SELECT TOP 1 EmployeeId,OfficeEmail FROM HR_EmployeeInformation Where EmployeeId<>@Id AND OfficeEmail = @OfficeEmail AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
        //        var data = await _dapper.SqlQueryFirstAsync<EmployeeServiceDataViewModel>(user.Database, query, new
        //        {
        //            Id = id.ToString(),
        //            OfficeEmail = email,
        //            user.CompanyId,
        //            user.OrganizationId
        //        }, CommandType.Text);

        //        if (data != null && data.OfficeEmail == email)
        //        {
        //            isExist = true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "InfoBusiness", "IsOfficeEmailAvailableAsync", user);
        //    }
        //    return isExist;
        //}
        public async Task<bool> IsEmployeeIdInEditAvailableAsync(long id, string code, AppUser user)
        {
            bool isExist = false;
            try
            {
                var query = $@"SELECT TOP 1 EmployeeCode FROM HR_EmployeeInformation Where EmployeeId<>@Id AND EmployeeCode = @Code AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var data = await _dapper.SqlQueryFirstAsync<EmployeeServiceDataViewModel>(user.Database, query, new
                {
                    Id = id.ToString(),
                    Code = code,
                    user.CompanyId,
                    user.OrganizationId
                }, CommandType.Text);

                if (data != null && data.EmployeeCode == code)
                {
                    isExist = true;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoBusiness", "IsEmployeeIdAvailableAsync", user);
            }
            return isExist;
        }
        public async Task<EmployeeInformation> GetEmployeeInformationById(long id, AppUser user)
        {
            EmployeeInformation employeeInformation = new EmployeeInformation();
            try
            {
                employeeInformation = await _employeeModuleDbContext.HR_EmployeeInformation.FirstOrDefaultAsync(item =>
                    item.EmployeeId == id
                    && item.CompanyId == user.CompanyId
                    && item.OrganizationId == user.OrganizationId
                );
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoBusiness", "GetEmployeeInformationById", user);
            }
            return employeeInformation;
        }
        public async Task<EmployeeDetail> GetEmployeeDetailById(long id, AppUser user)
        {
            EmployeeDetail employeeDetail = new EmployeeDetail();
            try
            {
                employeeDetail = await _employeeModuleDbContext.HR_EmployeeDetail.FirstOrDefaultAsync(item =>
                    item.EmployeeId == id
                    && item.CompanyId == user.CompanyId
                    && item.OrganizationId == user.OrganizationId
                );
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoBusiness", "GetEmployeeDetailById", user);
            }
            return employeeDetail;
        }
        public async Task<EmployeeInformation> GetEmployeeInformationByCode(string code, AppUser user)
        {
            EmployeeInformation employeeInformation = null;
            try
            {
                employeeInformation = await _employeeModuleDbContext.HR_EmployeeInformation.FirstOrDefaultAsync(item =>
                    item.EmployeeCode == code
                    && item.CompanyId == user.CompanyId
                    && item.OrganizationId == user.OrganizationId
                );
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoBusiness", "GetEmployeeInformationByCode", user);
            }
            return employeeInformation;
        }

    }
}
