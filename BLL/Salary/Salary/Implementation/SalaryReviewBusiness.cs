using Dapper;
using System.Data;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using Shared.Payroll.DTO.Salary;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.ViewModel.Salary;
using Shared.Employee.Filter.Info;
using Shared.Payroll.Helpers.SalaryProcess;
using Shared.Payroll.Domain.Salary;
using Shared.Employee.ViewModel.Info;
using DAL.Context.Payroll;
using DAL.Context.Employee;
using BLL.Administration.Interface;
using Microsoft.EntityFrameworkCore;

namespace BLL.Salary.Salary.Implementation
{
    public class SalaryReviewBusiness : ISalaryReviewBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly IInfoBusiness _employeeBusiness;
        public SalaryReviewBusiness(
            IDapperData dapper, PayrollDbContext payrollDbContext, EmployeeModuleDbContext employeeModuleDbContext, IInfoBusiness employeeBusiness, ISysLogger sysLogger, IBranchInfoBusiness branchInfoBusiness)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _employeeBusiness = employeeBusiness;
            _payrollDbContext = payrollDbContext;
            _branchInfoBusiness = branchInfoBusiness;
            _employeeModuleDbContext = employeeModuleDbContext;
        }
        public async Task<ExecutionStatus> ValidateSalaryReviewAsync(SalaryReviewInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalaryReviewInfo";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryReviewInfoId", model.SalaryReviewInfoId);
                parameters.Add("EmployeeId", model.EmployeeId);
                parameters.Add("ActivationDate", model.ActivationDate);
                parameters.Add("EffectiveFrom", model.EffectiveFrom);
                parameters.Add("ArrearCalculatedDate", model.ArrearCalculatedDate);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "ValidateSalaryReviewAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveSalaryReviewAsync(SalaryReviewInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_Payroll_SalaryReviewInfo";
                //var employee = (await _employeeBusiness.GetEmployeeServiceDataAsync(new EmployeeService_Filter() { EmployeeId = model.EmployeeId.ToString() }, user)).FirstOrDefault();

                var parameters = DapperParam.AddParams(model, user, new string[] { "FiscalYearId", "SalaryReviewDetails", "IsApproved", "ActivationMonth", "ActivationYear", "ArrearMonth", "ArrearYear" });
                parameters.Add("DetailsJsonData", JsonReverseConverter.JsonData(model.SalaryReviewDetails));
                parameters.Add("ExecutionFlag", model.SalaryReviewInfoId == 0 ? Data.Insert : Data.Update);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "SaveSalaryReviewAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<SalaryReviewInfoViewModel>> GetSalaryReviewInfosAsync(SalaryReview_Filter filter, AppUser user)
        {
            IEnumerable<SalaryReviewInfoViewModel> data = new List<SalaryReviewInfoViewModel>();
            try
            {
                var query = $@"Select sri.*,emp.EmployeeCode,emp.FullName,emp.DesignationName,emp.DepartmentName,
				emp.SectionName,emp.BranchName, 
				ActivationMonth= MONTH(ActivationDate),
				ActivationYear= Year(ActivationDate),
				ArrearMonth= MONTH(ArrearCalculatedDate),
				ArrearYear= Year(ArrearCalculatedDate)
				From Payroll_SalaryReviewInfo sri
				Inner Join vw_HR_EmployeeList emp on sri.EmployeeId = emp.EmployeeId 
				And sri.CompanyId= emp.CompanyId And sri.OrganizationId = emp.OrganizationId
				Where 1=1
				And (@SalaryReviewInfoId IS NULL OR @SalaryReviewInfoId = 0 Or sri.SalaryReviewInfoId=@SalaryReviewInfoId)
				And (@SalaryConfigCategory IS NULL OR @SalaryConfigCategory = '' OR  sri.SalaryConfigCategory=@SalaryConfigCategory)
				And (@EmployeeId IS NULL OR @EmployeeId = 0 OR sri.EmployeeId=@EmployeeId)
				And (@StateStatus IS NULL OR  @StateStatus = '' OR sri.StateStatus=@StateStatus)
				And sri.CompanyId=@CompanyId
				And sri.OrganizationId=@OrganizationId";
                var parameters = DapperParam.AddParams(filter, user);

                data = await _dapper.SqlQueryListAsync<SalaryReviewInfoViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "SaveSalaryReviewAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveSalaryReviewStatusAsync(SalaryReviewStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalaryReviewInfo";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "SaveSalaryReviewStatusAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<SalaryReviewDetailViewModel>> GetSalaryAllowanceForReviewAsync(long employeeId, AppUser user)
        {
            IEnumerable<SalaryReviewDetailViewModel> data = new List<SalaryReviewDetailViewModel>();
            var employee = (await _employeeBusiness.GetEmployeeServiceDataAsync(new EmployeeService_Filter() { EmployeeId = employeeId.ToString() }, user)).FirstOrDefault();
            try
            {
                var query = $@"SELECT tbl1.*, [PreviousAmount]=tbl2.CurrentAmount,  SalaryBaseAmount, [PreviousReviewId] = SalaryReviewInfoId
FROM (Select scd.SalaryAllowanceConfigDetailId,sci.SalaryAllowanceConfigId 'salaryConfigCategoryInfoId',AllowanceName=alw.Name,scd.AllowanceNameId,scd.AllowanceBase,
	scd.Amount 'AllowanceAmount',scd.[Percentage] 'AllowancePercentage',
	(Case 
		When scd.AllowanceBase='Flat' then scd.Amount
		When scd.AllowanceBase !='Flat' then 0 Else 0 End) 'CurrentAmount',
	sci.ConfigCategory 'SalaryConfigCategory', 
	scd.MaxAmount,scd.AdditionalAmount,AllowanceFlag=alw.Flag
	From Payroll_SalaryAllowanceConfigurationDetails scd
	Inner Join Payroll_SalaryAllowanceConfigurationInfo sci on scd.SalaryAllowanceConfigId= sci.SalaryAllowanceConfigId and sci.StateStatus='Approved'
	INNER Join Payroll_AllowanceName alw on scd.AllowanceNameId = alw.AllowanceNameId 
	Where sci.ConfigCategory=@ConfigCategory And 
	(	(@ConfigCategory='Employee Wise' And scd.EmployeeId=@EmployeeId) OR 
		(@ConfigCategory='Designation' And scd.DesignationId=@DesignationId) OR 
		(@ConfigCategory='Grade' And scd.GradeId=@GradeId) OR
        (@ConfigCategory='Job Type' And scd.JobType=@JobType) OR 
		(@ConfigCategory='All') 
	)
	And scd.IsActive=1
	And sci.CompanyId=@CompanyId 
	And sci.OrganizationId=@OrganizationId) tbl1
	LEFT JOIN (
	SELECT SRD.AllowanceNameId, SRD.CurrentAmount, SI.SalaryBaseAmount, SI.SalaryReviewInfoId FROM Payroll_SalaryReviewDetail SRD
	INNER JOIN (SELECT TOP 1 SalaryReviewInfoId, SalaryBaseAmount FROM Payroll_SalaryReviewInfo 
	Where EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId ORDER BY ActivationDate DESC) SI 
	ON SRD.SalaryReviewInfoId=SI.SalaryReviewInfoId ) tbl2 ON tbl1.AllowanceNameId=tbl2.AllowanceNameId";

                var salaryConfigCategory = new string[] { "Employee Wise", "Designation", "Grade", "Job Type", "All" };
                var parameters = new DynamicParameters();

                parameters.Add("EmployeeId", employeeId);
                parameters.Add("DesignationId", employee.DesignationId);
                parameters.Add("GradeId", employee.GradeId);
                parameters.Add("JobType", employee.JobType);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);

                for (int i = 0; i < salaryConfigCategory.Length; i++)
                {
                    parameters.Add("ConfigCategory", salaryConfigCategory[i]);
                    data = await _dapper.SqlQueryListAsync<SalaryReviewDetailViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
                    if (data.AsList().Any())
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "GetSalaryAllowanceForReviewAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<SalaryReviewDetailViewModel>> GetSalaryReviewDetailsAsync(SalaryReview_Filter filter, AppUser user)
        {
            IEnumerable<SalaryReviewDetailViewModel> data = new List<SalaryReviewDetailViewModel>();
            try
            {
                var sp_name = "sp_Payroll_SalaryReviewDetail";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryReviewInfoId", filter.SalaryReviewInfoId);
                parameters.Add("EmployeeId", filter.EmployeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapper.SqlQueryListAsync<SalaryReviewDetailViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "GetSalaryReviewDetailsAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<SalaryReviewInfoViewModel>> GetEmployeeLastSalaryReviewAccordingToCutOffDate(long? employeeId, string cutOffDate, AppUser user)
        {
            IEnumerable<SalaryReviewInfoViewModel> list = new List<SalaryReviewInfoViewModel>();
            try
            {
                var sp_name = $@"sp_Payroll_SalaryReviewInfo";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId ?? 0);
                parameters.Add("CutOffDate", cutOffDate);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "Employee_Last_Salary_Review_List");
                list = await _dapper.SqlQueryListAsync<SalaryReviewInfoViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "GetEmployeeLastSalaryReviewAccordingToCutOffDate", user);
            }
            return list;
        }
        public async Task<SalaryReviewInfoViewModel> GetLastSalaryReviewAccordingToCutOffDate(long? employeeId, string cutOffDate, AppUser user)
        {
            SalaryReviewInfoViewModel item = new SalaryReviewInfoViewModel();
            try
            {
                var query = $@"SELECT EmployeeId, EmployeeCode, FullName, SalaryReviewInfoId, CurrentSalaryAmount,BaseType, ActivationDate
                FROM (
	                SELECT E.EmployeeId, E.EmployeeCode, E.FullName, tbl.SalaryReviewInfoId, tbl.BaseType, tbl.CurrentSalaryAmount, tbl.ActivationDate,
			                ROW_NUMBER() OVER (PARTITION BY E.EmployeeId ORDER BY tbl.ActivationDate DESC) AS RowNum
	                FROM HR_EmployeeInformation E
	                INNER JOIN Payroll_SalaryReviewInfo tbl ON tbl.EmployeeId = E.EmployeeId
	                WHERE tbl.ActivationDate <= @CutOffDate
	                AND tbl.IsApproved = 1
	                AND tbl.CompanyId = @CompanyId
	                AND tbl.OrganizationId = @OrganizationId
                ) AS t
                WHERE t.RowNum = 1 
                And t.EmployeeId=@EmployeeId";
                item = await _dapper.SqlQueryFirstAsync<SalaryReviewInfoViewModel>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    CutOffDate = cutOffDate,
                    user.CompanyId,
                    user.OrganizationId
                }, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "GetEmployeeLastSalaryReviewAccordingToCutOffDate", user);
            }
            return item;
        }
        public async Task<IEnumerable<ExecutionStatus>> UploadFlatSalaryReviewAsync(List<UploadFlatSalaryReviewInfoDTO> salaryReviews, AppUser user)
        {
            IEnumerable<ExecutionStatus> execution_list = new List<ExecutionStatus>();
            try
            {
                foreach (var salaryReview in salaryReviews)
                {
                    ExecutionStatus executionStatus = new ExecutionStatus();
                    var employeeIndb = (await _employeeBusiness.GetEmployeeOfficeInfoByIdAsync(new EmployeeOfficeInfo_Filter()
                    {
                        EmployeeCode = salaryReview.EmployeeCode
                    }, user));
                    if (employeeIndb != null)
                    {
                        bool isValid = true;
                        var lastApprovedSalaryReview = await GetLastSalaryReviewInfoByEmployeeAsync(employeeIndb.EmployeeId, user);
                        if (lastApprovedSalaryReview != null)
                        {
                            // Another Salary Review
                            SalaryReviewInfoDTO info = new SalaryReviewInfoDTO();
                            info.EmployeeId = employeeIndb.EmployeeId;

                            //if (salaryReview.EffectiveDate.HasValue) {
                            //    if (salaryReview.EffectiveDate.Value.Date <= lastApprovedSalaryReview.EffectiveFrom.Value.Date) {
                            //        isValid = false;
                            //    }
                            //}
                            //else {
                            //    isValid = false;
                            //}
                            if (isValid)
                            {
                                info.EmployeeId = employeeIndb.EmployeeId;
                                info.EffectiveFrom = salaryReview.EffectiveDate;

                                if (salaryReview.ActivationDate.Value.Date <= lastApprovedSalaryReview.ActivationDate.Value.Date)
                                {
                                    info.ActivationDate = lastApprovedSalaryReview.ActivationDate.Value.Date.AddDays(1);
                                }
                                else
                                {
                                    info.ActivationDate = salaryReview.ActivationDate;
                                }
                                if (salaryReview.ArrearDate.Value.Date <= lastApprovedSalaryReview.ArrearCalculatedDate.Value.Date)
                                {
                                    info.ArrearCalculatedDate = lastApprovedSalaryReview.ArrearCalculatedDate.Value.Date.AddDays(1);
                                }
                                else
                                {
                                    info.ArrearCalculatedDate = salaryReview.ActivationDate;
                                }
                                info.ArrearYear = (short)info.ArrearCalculatedDate.Value.Year;
                                info.ArrearMonth = (short)info.ArrearCalculatedDate.Value.Month;
                                info.IncrementReason = "Others";
                                info.Description = "Salary has been uploaded";
                                info.DesignationId = employeeIndb.DesignationId;
                                info.IsAutoCalculate = false;
                                info.BaseType = "Flat";
                                info.CurrentSalaryAmount = salaryReview.SalaryReviewDetails.Sum(item => item.Amount);
                                info.PreviousSalaryAmount = lastApprovedSalaryReview.CurrentSalaryAmount;
                                info.SalaryBaseAmount = salaryReview.SalaryReviewDetails.Sum(item => item.Amount);
                                info.IsArrearCalculated = false;
                                info.PreviousReviewId = 0;
                                info.SalaryConfigCategory = "Flat";
                                info.SectionId = employeeIndb.SectionId;
                                info.PreviousReviewId = lastApprovedSalaryReview.SalaryReviewInfoId;
                                info.YearlyCTC = salaryReview.YearlyCTC ?? 0;
                                if (info.YearlyCTC > 0)
                                {
                                    info.MonthlyCTC = Math.Round(((info.YearlyCTC ?? 0) / 12), 0);
                                }
                                info.MonthlyFB = salaryReview.MonthlyFB ?? 0;
                                info.MonthlyPF = salaryReview.MonthlyPF ?? 0;
                                info.SalaryReviewDetails = new List<SalaryReviewDetailDTO>();


                                foreach (var item in salaryReview.SalaryReviewDetails)
                                {
                                    var value = lastApprovedSalaryReview.EmployeeLastApprovedSalaryReviewDetails.FirstOrDefault(i => i.AllowanceNameId == item.AllowanceNameId);
                                    SalaryReviewDetailDTO detail = new SalaryReviewDetailDTO();
                                    detail.SalaryConfigCategory = "Flat";
                                    detail.AllowanceNameId = item.AllowanceNameId;
                                    detail.AllowanceName = item.AllowanceName;
                                    detail.CurrentAmount = item.Amount;
                                    detail.PreviousAmount = value != null ? value.CurrentAmount : 0;
                                    detail.AllowanceHeadId = 0;
                                    detail.AllowanceBase = "Flat";
                                    detail.AllowancePercentage = 0;
                                    detail.MinAmount = 0;
                                    detail.MaxAmount = 0;
                                    info.SalaryReviewDetails.Add(detail);
                                }

                                executionStatus = await this.SaveSalaryReviewAsync(info, user);
                                execution_list.AsList().Add(executionStatus);

                                //if (lastApprovedSalaryReview.CurrentSalaryAmount == info.CurrentSalaryAmount && lastApprovedSalaryReview.EmployeeLastApprovedSalaryReviewDetails.Count == info.SalaryReviewDetails.Count) {
                                //    executionStatus.Status = true;
                                //    executionStatus.Msg = employeeIndb.EmployeeCode + " ~ " + employeeIndb.FullName + " is duplicate salary review";
                                //    execution_list.AsList().Add(executionStatus);
                                //}
                                //else {

                                //}
                            }
                            else
                            {
                                executionStatus.Status = false;
                                executionStatus.Msg = employeeIndb.EmployeeCode + " ~ " + employeeIndb.FullName + "Effective date is invalid";
                                execution_list.AsList().Add(executionStatus);
                            }
                        }
                        else
                        {
                            // First Salary Review
                            if (!employeeIndb.DateOfJoining.IsNullEmptyOrWhiteSpace())
                            {
                                if (salaryReview.EffectiveDate.HasValue)
                                {
                                    if (salaryReview.EffectiveDate.Value.Date < Convert.ToDateTime(employeeIndb.DateOfJoining.RemoveWhitespace()))
                                    {
                                        salaryReview.EffectiveDate = Convert.ToDateTime(employeeIndb.DateOfJoining.RemoveWhitespace());
                                    }
                                }
                            }
                            else
                            {
                                isValid = false;
                            }

                            if (salaryReview.EffectiveDate.HasValue && !salaryReview.ActivationDate.HasValue)
                            {
                                salaryReview.ActivationDate = salaryReview.EffectiveDate.Value;
                            }
                            else if (salaryReview.EffectiveDate.HasValue && salaryReview.ActivationDate.HasValue)
                            {
                                if (salaryReview.ActivationDate.Value.Date < salaryReview.EffectiveDate.Value.Date)
                                {
                                    salaryReview.ActivationDate = salaryReview.EffectiveDate.Value.Date;
                                }
                            }
                            if (salaryReview.ArrearDate.HasValue)
                            {
                                if (salaryReview.ArrearDate.Value.Date < salaryReview.ActivationDate.Value.Date)
                                {
                                    salaryReview.ArrearDate = salaryReview.ActivationDate;
                                }
                            }
                            else
                            {
                                salaryReview.ArrearDate = salaryReview.ActivationDate;
                            }

                            if (isValid == false)
                            {
                                executionStatus.Status = false;
                                executionStatus.Msg = employeeIndb.EmployeeCode + " ~ " + employeeIndb.FullName + " Date of joining not found";
                                execution_list.AsList().Add(executionStatus);
                            }
                            else
                            {
                                SalaryReviewInfoDTO info = new SalaryReviewInfoDTO();
                                info.EmployeeId = employeeIndb.EmployeeId;
                                info.EffectiveFrom = salaryReview.EffectiveDate;
                                info.ActivationDate = salaryReview.ActivationDate;
                                info.ArrearCalculatedDate = salaryReview.ArrearDate;
                                info.ArrearYear = (short)salaryReview.ArrearDate.Value.Year;
                                info.ArrearMonth = (short)salaryReview.ArrearDate.Value.Month;
                                info.IncrementReason = "Joining";
                                info.Description = "Salary has been uploaded";
                                info.DesignationId = employeeIndb.DesignationId;
                                info.IsAutoCalculate = false;
                                info.BaseType = "Flat";
                                info.CurrentSalaryAmount = salaryReview.SalaryReviewDetails.Sum(item => item.Amount);
                                info.PreviousSalaryAmount = 0;
                                info.SalaryBaseAmount = salaryReview.SalaryReviewDetails.Sum(item => item.Amount);
                                info.IsArrearCalculated = false;
                                info.PreviousReviewId = 0;
                                info.SalaryConfigCategory = "Flat";
                                info.SectionId = employeeIndb.SectionId;
                                info.YearlyCTC = salaryReview.YearlyCTC ?? 0;
                                if (info.YearlyCTC > 0)
                                {
                                    info.MonthlyCTC = Math.Round(((info.YearlyCTC ?? 0) / 12), 0);
                                }
                                info.MonthlyFB = salaryReview.MonthlyFB ?? 0;
                                info.MonthlyPF = salaryReview.MonthlyPF ?? 0;
                                info.SalaryReviewDetails = new List<SalaryReviewDetailDTO>();

                                foreach (var item in salaryReview.SalaryReviewDetails)
                                {
                                    SalaryReviewDetailDTO detail = new SalaryReviewDetailDTO();
                                    detail.SalaryConfigCategory = "Flat";
                                    detail.AllowanceNameId = item.AllowanceNameId;
                                    detail.AllowanceName = item.AllowanceName;
                                    detail.CurrentAmount = item.Amount;
                                    detail.PreviousAmount = 0;
                                    detail.AllowanceHeadId = 0;
                                    detail.AllowanceBase = "Flat";
                                    detail.AllowancePercentage = 0;
                                    detail.MinAmount = 0;
                                    detail.MaxAmount = 0;

                                    info.SalaryReviewDetails.Add(detail);
                                }
                                executionStatus = await this.SaveSalaryReviewAsync(info, user);
                                execution_list.AsList().Add(executionStatus);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "UploadFlatSalaryReviewAsync", user);
            }
            return execution_list;
        }
        public async Task<EmployeeLastApprovedSalaryReviewInfo> GetLastSalaryReviewInfoByEmployeeAsync(long employeeId, AppUser user)
        {
            EmployeeLastApprovedSalaryReviewInfo info = new EmployeeLastApprovedSalaryReviewInfo();
            try
            {
                var query1 = $@"SELECT tbl1.SalaryReviewInfoId,tbl1.EmployeeId,EMP.EmployeeCode,EMP.FullName,EMP.DateOfJoining,EMP.DesignationId,
EMP.DesignationName,EMP.DepartmentId,EMP.DepartmentName,EMP.SubSectionId,EMP.SubSectionName,tbl1.SalaryAllowanceConfigId,
tbl1.CurrentSalaryAmount,tbl1.PreviousSalaryAmount,
tbl1.IncrementReason,tbl1.ActivationDate,
tbl1.EffectiveFrom,tbl1.ArrearCalculatedDate,tbl1.IsArrearCalculated,tbl1.IsAutoCalculate 
FROM Payroll_SalaryReviewInfo tbl1
INNER JOIN (SELECT EmployeeId,ActivationDate=MAX(ActivationDate) FROM Payroll_SalaryReviewInfo
Where EmployeeId=@EmployeeId AND IsApproved=1 AND StateStatus='Approved'
AND CompanyId =@CompanyId AND OrganizationId=@OrganizationId
Group By EmployeeId) tbl2 On tbl1.EmployeeId = tbl2.EmployeeId AND tbl1.ActivationDate= tbl2.ActivationDate
INNER JOIN [dbo].[vw_HR_EmployeeList] EMP ON tbl1.EmployeeId=EMP.EmployeeId AND tbl1.CompanyId= EMP.CompanyId AND tbl1.OrganizationId = EMP.OrganizationId";

                info = await _dapper.SqlQueryFirstAsync<EmployeeLastApprovedSalaryReviewInfo>(user.Database, query1, new { EmployeeId = employeeId, user.CompanyId, user.OrganizationId }, CommandType.Text);

                if (info != null && info.SalaryReviewInfoId > 0)
                {
                    info.EmployeeLastApprovedSalaryReviewDetails = new List<EmployeeLastApprovedSalaryReviewDetail>();

                    var query2 = $@"SELECT SalaryReviewDetailId,ALW.AllowanceNameId,AllowanceName=ALW.[Name],SRD.AllowanceBase,SRD.CurrentAmount
,SRD.PreviousAmount,SRD.AdditionalAmount,SRD.SalaryAllowanceConfigDetailId,AllowanceFlag=ALW.Flag  
FROM Payroll_SalaryReviewDetail SRD
INNER JOIN Payroll_AllowanceName ALW ON SRD.AllowanceNameId=ALW.AllowanceNameId
Where 1=1
AND SRD.SalaryReviewInfoId=@SalaryReviewInfoId AND SRD.CompanyId=@CompanyId AND SRD.OrganizationId=@OrganizationId";
                    info.EmployeeLastApprovedSalaryReviewDetails = (await _dapper.SqlQueryListAsync<EmployeeLastApprovedSalaryReviewDetail>(user.Database, query2, new { info.SalaryReviewInfoId, user.CompanyId, user.OrganizationId }, CommandType.Text)).AsList();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "GetLastSalaryReviewInfoByEmployee", user);
            }
            return info;
        }
        public async Task<IEnumerable<EmployeeSalaryReviewInSalaryProcess>> GetEmployeeSalaryReviewesInSalaryProcess(SalaryReviewInSalaryProcess_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeSalaryReviewInSalaryProcess> list = new List<EmployeeSalaryReviewInSalaryProcess>();
            try
            {
                var query = $@"Select ROW_NUMBER() Over(Order By SalaryReviewInfoId) 'SerialNo',SalaryReviewInfoId,SalaryConfigCategory,CurrentSalaryAmount,PreviousSalaryAmount,BaseType,
			IncrementReason,EffectiveFrom,EffectiveTo,ActivationDate,DeactivationDate,IsArrearCalculated,ArrearCalculatedDate
			From Payroll_SalaryReviewInfo 
			Where EmployeeId=@EmployeeId AND  EffectiveFrom <= CAST(@salaryEndDate AS DATE) AND 
			(
			(ActivationDate <=CAST(@salaryStartDate AS DATE) AND DeactivationDate IS NULL )
			OR
			(ActivationDate <=CAST(@salaryStartDate AS DATE) AND DeactivationDate >= CAST(@salaryEndDate AS DATE))
			OR
			(ActivationDate <=CAST(@salaryStartDate AS DATE) AND DeactivationDate > CAST(@salaryStartDate AS DATE))
			OR
			(ActivationDate < CAST(@salaryStartDate AS DATE) AND DeactivationDate < CAST(@salaryEndDate AS DATE) AND IsArrearCalculated=0 AND MONTH(EffectiveFrom)<> MONTH(ActivationDate))
			OR 
			(MONTH(ActivationDate)= MONTH(CAST(@salaryStartDate AS DATE)) AND YEAR(ActivationDate)= YEAR(CAST(@salaryStartDate AS DATE)) AND DeactivationDate >=CAST(@salaryEndDate AS DATE))
			OR
			(MONTH(ActivationDate)= MONTH(CAST(@salaryStartDate AS DATE)) AND YEAR(ActivationDate)= YEAR(CAST(@salaryStartDate AS DATE)) AND DeactivationDate < CAST(@salaryEndDate AS DATE))
			OR
			(MONTH(ActivationDate)= MONTH(CAST(@salaryStartDate AS DATE)) AND YEAR(ActivationDate)= YEAR(CAST(@salaryStartDate AS DATE)) AND DeactivationDate IS NULL)
			)
			AND IsApproved=1 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
			Order By SalaryReviewInfoId,EmployeeId,ActivationDate";
                var parameter = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<EmployeeSalaryReviewInSalaryProcess>(user.Database, query, parameter, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "GetEmployeeSalaryReviewesInSalaryProcess", user);
            }
            return list;
        }
        public async Task<IEnumerable<SalaryReviewDetail>> GetSalaryReviewDetailsAsync(long salaryReviewInfoId, AppUser user)
        {
            IEnumerable<SalaryReviewDetail> list = new List<SalaryReviewDetail>();
            try
            {
                var query = $@"Select * From Payroll_SalaryReviewDetail Where SalaryReviewInfoId= @SalaryReviewInfoId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryReviewInfoId", salaryReviewInfoId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<SalaryReviewDetail>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "GetSalaryReviewDetailsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> UploadSalaryReviewExcelAsync(List<UploadSalaryReviewReadDTO> salaryReviewDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            var errorIds = new List<string>();
            int itemRowCount = 0;
            try
            {

                long empID = 0;

                foreach (var item in salaryReviewDTOs)
                {
                    var uploadSalaryReview = new UploadSalaryReviewInsertDTO();
                    uploadSalaryReview.EmployeeId = await GetEmployeeComponentId(item.EmployeeCode, user);
                    empID = uploadSalaryReview.EmployeeId;
                    var salaryAllowanceReview = await GetSalaryAllowanceForReviewAsync(empID, user);
                    var employee = await _employeeBusiness.GetEmployeeAsync(empID, user);

                    uploadSalaryReview.EmployeeCode = item.EmployeeCode;
                    uploadSalaryReview.EffectiveFrom = item.EffectiveFrom;
                    uploadSalaryReview.ActivationDate = item.ActivationDate;
                    uploadSalaryReview.ArrearCalculatedDate = item.ArrearCalculatedDate;
                    uploadSalaryReview.CurrentSalaryAmount = item.CurrentSalaryAmount;
                    uploadSalaryReview.IncrementReason = item.IncrementReason;


                    foreach (var detail in salaryAllowanceReview)
                    {
                        var salaryReviewDetail = new UploadSalaryReviewDetailDTO();
                        salaryReviewDetail.EmployeeId = empID;
                        salaryReviewDetail.AllowanceNameId = detail.AllowanceNameId;
                        salaryReviewDetail.AllowanceName = detail.AllowanceName;
                        salaryReviewDetail.AllowancePercentage = detail.AllowancePercentage;
                        salaryReviewDetail.AllowanceBase = detail.AllowanceBase;
                        salaryReviewDetail.SalaryAllowanceConfigDetailId = detail.SalaryAllowanceConfigDetailId;
                        salaryReviewDetail.PreviousAmount = detail.PreviousAmount;
                        uploadSalaryReview.SalaryConfigCategoryInfoId = detail.SalaryConfigCategoryInfoId;
                        uploadSalaryReview.SalaryConfigCategory = detail.SalaryConfigCategory;
                        uploadSalaryReview.PreviousSalaryAmount = detail.SalaryBaseAmount;
                        uploadSalaryReview.PreviousReviewId = detail.PreviousReviewId;
                        uploadSalaryReview.BaseType = detail.AllowanceBase;

                        if (detail.AllowanceBase != "Flat")
                        {
                            salaryReviewDetail.CurrentAmount = Math.Round(uploadSalaryReview.CurrentSalaryAmount / 100 * detail.AllowancePercentage);
                        }
                        else
                        {
                            salaryReviewDetail.CurrentAmount = uploadSalaryReview.CurrentSalaryAmount;
                        }

                        uploadSalaryReview.uploadSalaryReviewDetails.Add(salaryReviewDetail);
                    }

                    var detailsJsonData = Utility.JsonData(uploadSalaryReview.uploadSalaryReviewDetails);
                    //var parameters = Utility.DappperParams(uploadSalaryReview,user,new string[] { "uploadSalaryReviewDetails","EmployeeCode", "MonthlyCTC", "MonthlyPF", "MonthlyFB", "SalaryConfigCategoryInfoId", "CTCAmountWithoutFestivalBonus", "IsApproved" });

                    var parameters = new DynamicParameters();
                    parameters.Add("SalaryReviewInfoId", uploadSalaryReview.SalaryReviewInfoId);
                    parameters.Add("EmployeeId", uploadSalaryReview.EmployeeId);
                    parameters.Add("DesignationId", employee.DesignationId);
                    parameters.Add("InternalDesignationId", employee.InternalDesignationId ?? 0);
                    parameters.Add("DepartmentId", employee.DepartmentId);
                    parameters.Add("SectionId", employee.SectionId);
                    parameters.Add("BranchId", employee.BranchId);
                    parameters.Add("BaseType", uploadSalaryReview.BaseType);
                    parameters.Add("IsAutoCalculate", 1);
                    parameters.Add("CurrentSalaryAmount", uploadSalaryReview.CurrentSalaryAmount);
                    parameters.Add("PreviousSalaryAmount", uploadSalaryReview.PreviousSalaryAmount);
                    parameters.Add("SalaryBaseAmount", uploadSalaryReview.CurrentSalaryAmount);
                    parameters.Add("SalaryConfigCategory", uploadSalaryReview.SalaryConfigCategory);
                    parameters.Add("SalaryAllowanceConfigId", uploadSalaryReview.SalaryConfigCategoryInfoId);
                    parameters.Add("PreviousReviewId", uploadSalaryReview.PreviousReviewId ?? 0);
                    parameters.Add("IncrementReason", uploadSalaryReview.IncrementReason);
                    parameters.Add("Description", uploadSalaryReview.Description ?? "");
                    parameters.Add("StateStatus", uploadSalaryReview.StateStatus ?? "Pending");
                    parameters.Add("IsActive", uploadSalaryReview.IsActive);
                    parameters.Add("Remarks", uploadSalaryReview.Remarks ?? "");
                    parameters.Add("ActivationDate", uploadSalaryReview.ActivationDate);
                    parameters.Add("ArrearCalculatedDate", uploadSalaryReview.ArrearCalculatedDate);
                    parameters.Add("EffectiveFrom", uploadSalaryReview.EffectiveFrom);
                    parameters.Add("EffectiveTo", uploadSalaryReview.EffectiveTo);
                    parameters.Add("DetailsJsonData", detailsJsonData);
                    parameters.Add("UserId", user.UserId);
                    parameters.Add("CompanyId", user.CompanyId);
                    parameters.Add("OrganizationId", user.OrganizationId);
                    parameters.Add("ExecutionFlag", Data.Insert);

                    var sp_name = "sp_Payroll_SalaryReviewInfo";

                    if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                    {
                        executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
                        if (!executionStatus.Status)
                        {
                            executionStatus.Status = false;
                            errorIds.Add(executionStatus.Msg);
                            itemRowCount += 1;
                            executionStatus.Errors.Add(item.EmployeeCode, executionStatus.ErrorMsg);
                        }
                    }
                }

                if (errorIds.Any())
                {
                    executionStatus.Msg += $"  |  Could not process this ids{string.Join(',', errorIds)}";
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                errorIds.Add(executionStatus.Msg);
                await _sysLogger.SavePayrollException(ex, user.Database, "AllowanceBusiness", "UploadSalaryReviewExcelAsync", user);
            }

            return executionStatus;
        }
        public async Task<long> GetEmployeeComponentId(string empCode, AppUser user)
        {
            long id = 0;
            try
            {
                var sqlQuery = @"Select EmployeeId From HR_EmployeeInformation Where EmployeeCode ='" + empCode + "' AND IsActive=1 AND CompanyId = " + user.CompanyId + " AND OrganizationId = " + user.OrganizationId + " AND BranchId = " + user.BranchId + "";
                var parameters = new DynamicParameters();
                var employeeViewModel = new EmployeeInformationViewModel();
                parameters.Add("EmployeeId", employeeViewModel.EmployeeId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                if (!Utility.IsNullEmptyOrWhiteSpace(sqlQuery))
                {
                    id = await _dapper.SqlQueryFirstAsync<long>(user.Database, sqlQuery, parameters, commandType: CommandType.Text);
                    return id;
                }
            }
            catch (Exception)
            {
            }
            return id;
        }
        public async Task<DataTable> GetSalaryReviewSheetAsync(SalaryReviewSheet_Filter reviewSheet_Filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "SP_Payroll_SalaryReviewDetailSheet";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();

                keyValuePairs.Add("FromDate", reviewSheet_Filter.FromDate.ToString());
                keyValuePairs.Add("EmployeeId", reviewSheet_Filter.EmployeeId.ToString());
                keyValuePairs.Add("ExecutionFlag", "Report");
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveSystemException(ex, user.Database, "IAllowanceBusiness", "GetSalaryReviewSheetAsync", user.UserId, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return dataTable;
        }
        public async Task<IEnumerable<SalaryReviewInfoViewModel>> GetAllPendingSalaryReviewesAsync(SalaryReview_Filter filter, AppUser user)
        {
            IEnumerable<SalaryReviewInfoViewModel> data = new List<SalaryReviewInfoViewModel>();
            try
            {
                var query = $@"Select sri.*,emp.EmployeeCode,emp.FullName,emp.DesignationName,emp.DepartmentName,
				emp.SectionName,emp.BranchName, 
				ActivationMonth= MONTH(ActivationDate),
				ActivationYear= Year(ActivationDate),
				ArrearMonth= MONTH(ArrearCalculatedDate),
				ArrearYear= Year(ArrearCalculatedDate)
				From Payroll_SalaryReviewInfo sri
				Inner Join vw_HR_EmployeeList emp on sri.EmployeeId = emp.EmployeeId 
				And sri.CompanyId= emp.CompanyId And sri.OrganizationId = emp.OrganizationId
				Where 1=1
				And (@SalaryReviewInfoId IS NULL OR @SalaryReviewInfoId = 0 Or sri.SalaryReviewInfoId=@SalaryReviewInfoId)
				And (@EmployeeId IS NULL OR @EmployeeId = 0 OR sri.EmployeeId=@EmployeeId)
				And (sri.StateStatus='Pending')
				And sri.CompanyId=@CompanyId
				And sri.OrganizationId=@OrganizationId";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<SalaryReviewInfoViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "SaveSalaryReviewAsync", user);
            }
            return data;
        }
        public async Task<DataTable> DownloadSalaryReviewSheetAsync(SalaryReviewSheetDownload_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Id");
            dataTable.Columns.Add("Name");
            dataTable.Columns.Add("Branch");
            dataTable.Columns.Add("Designation");
            dataTable.Columns.Add("Joining Date");
            dataTable.Columns.Add("Effective From");
            dataTable.Columns.Add("Activation Date");
            dataTable.Columns.Add("Arrear Date");
            dataTable.Columns.Add("Status");
            dataTable.Columns.Add("Reason");
            dataTable.Columns.Add("New Gross");
            dataTable.Columns.Add("Old Gross");

            try
            {
                string query = $@"SELECT [Id]=EMP.EmployeeCode,[Name]=EMP.FullName,EMP.BranchId,[JoiningDate]=CONVERT(NVARCHAR(20),EMP.DateOfJoining,106),DEG.DesignationName,
                EffectiveFrom=CONVERT(NVARCHAR(20),RI.EffectiveFrom,106),
                ActivationDate=CONVERT(NVARCHAR(20),RI.ActivationDate,106),
                ArrearDate=CONVERT(NVARCHAR(20),RI.ArrearCalculatedDate,106),
                [Status]=RI.StateStatus,
                [Reason]=RI.IncrementReason,
                RI.CreatedDate,
                RI.ApprovedDate,
                NewGross=ISNULL((SELECT SUM(CurrentAmount) FROM Payroll_SalaryReviewDetail Where SalaryReviewInfoId=ISNULL(RI.SalaryReviewInfoId,0)),0),
                OldGross=ISNULL((SELECT SUM(CurrentAmount) FROM Payroll_SalaryReviewDetail Where SalaryReviewInfoId=ISNULL(RI.PreviousReviewId,0)),0),
                RD.AllowanceNameId,
                [Allowance]=ALW.[Name],
                [Amount]=RD.CurrentAmount,
                RI.SalaryReviewInfoId
                FROM Payroll_SalaryReviewDetail RD
                INNER JOIN Payroll_SalaryReviewInfo RI ON RD.SalaryReviewInfoId= RI.SalaryReviewInfoId
                INNER JOIN HR_EmployeeInformation EMP ON EMP.EmployeeId = RI.EmployeeId
                INNER JOIN Payroll_AllowanceName ALW ON ALW.AllowanceNameId = RD.AllowanceNameId
                LEFT JOIN HR_Designations DEG ON EMP.DesignationId = DEG.DesignationId
                Where 1=1
                AND (@EffectiveFrom IS NULL OR @EffectiveFrom='' OR CAST(RI.EffectiveFrom AS DATE) BETWEEN CAST(@EffectiveFrom AS DATE) AND CAST(@EffectiveTo AS DATE))
                AND (@JoiningDateFrom IS NULL OR @JoiningDateFrom='' OR CAST(EMP.DateOfJoining AS DATE) BETWEEN CAST(@JoiningDateFrom AS DATE) AND CAST(@JoiningDateTo AS DATE))
                AND (@EmployeeId IS NULL OR @EmployeeId='' OR @EmployeeId=0 OR @EmployeeId='0' OR EMP.EmployeeId=@EmployeeId)
                AND (@StateStatus IS NULL OR @StateStatus=''OR RI.StateStatus=@StateStatus)
                AND (@BranchId IS NULL OR @BranchId='0' OR @BranchId=0 OR EMP.BranchId=@BranchId)
                AND RI.CompanyId=@CompanyId
                AND RI.OrganizationId=@OrganizationId";
                var data = await _dapper.SqlQueryListAsync<SalaryReviewSheetInfoDTO>(user.Database, query, new
                {
                    EffectiveFrom = filter.EffectiveFrom,
                    EffectiveTo = filter.EffectiveTo,
                    JoiningDateFrom = filter.JoiningDateFrom,
                    JoiningDateTo = filter.JoiningDateTo,
                    EmployeeId = filter.EmployeeId,
                    BranchId = filter.BranchId,
                    StateStatus = filter.StateStatus,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
                if (data.Any())
                {
                    var branches = await _branchInfoBusiness.GetBranchsAsync("", user);

                    var allowances = data.Select(i => new { i.AllowanceNameId, i.Allowance }).Distinct().ToList();

                    foreach (var allowance in allowances)
                    {
                        dataTable.Columns.Add(allowance.Allowance);
                    }
                    dataTable.Columns.Add("Created Date");
                    dataTable.Columns.Add("Approved Date");


                    var salaryReviewIds = data.Select(i => i.SalaryReviewInfoId).Distinct().ToArray();

                    foreach (var id in salaryReviewIds)
                    {
                        var allAllowances = data.Where(i => i.SalaryReviewInfoId == id).ToList();
                        var firstItem = allAllowances.FirstOrDefault();

                        if (firstItem != null)
                        {
                            var branchInfo = branches.FirstOrDefault(i => i.BranchId == firstItem.BranchId);
                            var branchName = "";
                            if (branchInfo != null)
                            {
                                branchName = branchInfo.BranchName;
                            }

                            var row = dataTable.NewRow();
                            row["Id"] = firstItem.Id;
                            row["Name"] = firstItem.Name;
                            row["Branch"] = branchName;
                            row["Designation"] = firstItem.DesignationName;
                            row["Joining Date"] = firstItem.JoiningDate;
                            row["Effective From"] = firstItem.EffectiveFrom;
                            row["Activation Date"] = firstItem.ActivationDate;
                            row["Arrear Date"] = firstItem.ActivationDate;
                            row["Status"] = firstItem.Status;
                            row["Reason"] = firstItem.Reason;
                            row["New Gross"] = firstItem.NewGross;
                            row["Old Gross"] = firstItem.OldGross;
                            foreach (var item in allAllowances)
                            {
                                row[item.Allowance] = Utility.TryParseDecimal(item.Amount.ToString());
                            }

                            row["Created Date"] = firstItem.CreatedDate.HasValue ? firstItem.CreatedDate.Value.ToString("dd-MMM-yyyy") : "";
                            row["Approved Date"] = firstItem.ApprovedDate.HasValue ?
                                firstItem.ApprovedDate.Value.ToString("dd-MMM-yyyy") : null;
                            dataTable.Rows.Add(row);
                        }
                    }
                }
                #region Using EF
                //var data = await (from info in _payrollDbContext.Payroll_SalaryReviewInfo
                //                  join detail in _payrollDbContext.Payroll_SalaryReviewDetail on info.SalaryReviewInfoId equals detail.SalaryReviewInfoId
                //                  join allowance in _payrollDbContext.Paryroll_AllownaceNames on detail.AllowanceNameId equals allowance.AllowanceNameId
                //                  where
                //                  (model.EffectiveFrom.HasValue && info.EffectiveFrom.HasValue ? info.EffectiveFrom.Value.Date >= model.EffectiveFrom.Value.Date : (1 == 1))
                //                  && (model.EffectiveTo.HasValue && info.EffectiveTo.HasValue ? info.EffectiveTo.Value.Date <= model.EffectiveTo.Value.Date : (1 == 1))

                //                  && (model.StateStatus != null && model.StateStatus != "" ? info.StateStatus == model.StateStatus : (1 == 1))
                //                  && (model.EmployeeId > 0 ? info.EmployeeId == model.EmployeeId : (1 == 1))

                //                  && info.CompanyId == user.CompanyId
                //                  && info.OrganizationId == user.OrganizationId
                //                  select new
                //                  {
                //                      EmployeeId = info.EmployeeId,
                //                      Id = "",
                //                      FullName = "",
                //                      DateOfJoining = info.CreatedDate,
                //                      info.SalaryReviewInfoId,
                //                      info.EffectiveFrom,
                //                      info.EffectiveTo,
                //                      info.ActivationDate,
                //                      info.ArrearCalculatedDate,
                //                      info.StateStatus,
                //                      info.IncrementReason,
                //                      info.CreatedDate,
                //                      info.ApprovedDate,
                //                      detail.AllowanceNameId,
                //                      AllowanceName = allowance.Name,
                //                      detail.CurrentAmount,

                //                  }).ToListAsync();

                //if (data.Any())
                //{
                //    var employeeIds = data.Select(i => i.EmployeeId).Distinct().ToArray();
                //    var employeelist = (from emp in _employeeModuleDbContext.HR_EmployeeInformation
                //                        where emp.CompanyId == user.CompanyId && emp.OrganizationId == user.OrganizationId
                //                        && employeeIds.Contains(emp.EmployeeId)
                //                        && (model.JoiningDateFrom.HasValue && emp.DateOfJoining.HasValue ? emp.DateOfJoining.Value.Date >= model.JoiningDateFrom.Value.Date : (1 == 1))
                //                        && (model.JoiningDateTo.HasValue && emp.DateOfJoining.HasValue ? emp.DateOfJoining.Value.Date <= model.JoiningDateTo.Value.Date : (1 == 1))
                //                        select emp);

                //    data = (from d in data
                //            join emp in employeelist on d.EmployeeId equals emp.EmployeeId
                //            select new
                //            {
                //                EmployeeId = d.EmployeeId,
                //                Id = emp.EmployeeCode,
                //                FullName = emp.FullName,
                //                DateOfJoining = emp.DateOfJoining,
                //                d.SalaryReviewInfoId,
                //                d.EffectiveFrom,
                //                d.EffectiveTo,
                //                d.ActivationDate,
                //                d.ArrearCalculatedDate,
                //                d.StateStatus,
                //                d.IncrementReason,
                //                d.CreatedDate,
                //                d.ApprovedDate,
                //                d.AllowanceNameId,
                //                d.AllowanceName,
                //                d.CurrentAmount,
                //            }).ToList();

                //    if (data.Any())
                //    {
                //        var allowances = data.Select(i => new { i.AllowanceNameId, i.AllowanceName }).Distinct().ToList();
                //        foreach (var item in allowances)
                //        {
                //            dataTable.Columns.Add(item.AllowanceName);
                //        }
                //        dataTable.Columns.Add("Created Date");
                //        dataTable.Columns.Add("Approved Date");

                //        var salaryReviewIds = data.Select(i => i.SalaryReviewInfoId).Distinct().ToArray();

                //        foreach (var id in salaryReviewIds)
                //        {
                //            var allAllowances = data.Where(i => i.SalaryReviewInfoId == id).ToList();
                //            var firstItem = allAllowances.FirstOrDefault();
                //            if (firstItem != null)
                //            {

                //                var row = dataTable.NewRow();
                //                row["Id"] = firstItem.Id;
                //                row["Name"] = firstItem.FullName;
                //                row["Joining Date"] = firstItem.DateOfJoining.Value.ToString("dd-MMM-yyyy");
                //                row["Effective From"] = firstItem.EffectiveFrom.HasValue ? firstItem.EffectiveFrom.Value.ToString("dd-MMM-yyyy") : "";
                //                row["Activation Date"] = firstItem.ActivationDate.HasValue ? firstItem.ActivationDate.Value.ToString("dd-MMM-yyyy") : "";
                //                row["Arrear Date"] = firstItem.ArrearCalculatedDate.HasValue ? firstItem.ArrearCalculatedDate.Value.ToString("dd-MMM-yyyy") : "";
                //                row["Status"] = firstItem.StateStatus;
                //                row["Reason"] = firstItem.IncrementReason;
                //                foreach (var item in allAllowances)
                //                {
                //                    row[item.AllowanceName] = Utility.TryParseDecimal(item.CurrentAmount.ToString());
                //                }

                //                row["Created Date"] = firstItem.CreatedDate.HasValue ? firstItem.CreatedDate.Value.ToString("dd-MMM-yyyy") : "";
                //                row["Approved Date"] = firstItem.ApprovedDate.HasValue ?
                //                    firstItem.ApprovedDate.Value.ToString("dd-MMM-yyyy") : null;
                //                dataTable.Rows.Add(row);
                //            }
                //        }
                //    }

                //} 
                #endregion
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "DownloadSalaryReviewSheetAsync", user);
            }
            return dataTable;
        }
        public async Task<ExecutionStatus> DeletePendingReviewAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var itemIbDb = await _payrollDbContext.Payroll_SalaryReviewInfo.FirstOrDefaultAsync(i => i.SalaryReviewInfoId == id
                && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId);

                if (
                    itemIbDb != null &&
                    itemIbDb.SalaryReviewInfoId > 0 &&
                    (itemIbDb.StateStatus == StateStatus.Pending || itemIbDb.StateStatus == StateStatus.Recheck))
                {
                    _payrollDbContext.Payroll_SalaryReviewInfo.Remove(itemIbDb);
                    var rowCount = await _payrollDbContext.SaveChangesAsync();
                    if (rowCount > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Deleted);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "DeletePendingReviewAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeleteApprovedReviewAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var itemIbDb = await _payrollDbContext.Payroll_SalaryReviewInfo.FirstOrDefaultAsync(i => i.SalaryReviewInfoId == id
                && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId);

                if (
                    itemIbDb != null &&
                    itemIbDb.SalaryReviewInfoId > 0 &&
                    itemIbDb.StateStatus == StateStatus.Approved && itemIbDb.EffectiveFrom.HasValue)
                {

                    // Find in salary process
                    //var salaryReviewIds = (from detail in _payrollDbContext.Payroll_SalaryProcessDetail
                    //                       where
                    //                       detail.EmployeeId == itemIbDb.EmployeeId
                    //                       && detail.CompanyId == itemIbDb.CompanyId
                    //                       && detail.OrganizationId == itemIbDb.OrganizationId
                    //                       && DateTimeExtension.FirstDateOfAMonth(detail.SalaryYear, detail.SalaryMonth).Date >=
                    //                        DateTimeExtension.FirstDateOfAMonth(itemIbDb.EffectiveFrom.Value.Year, itemIbDb.EffectiveFrom.Value.Month).Date
                    //                       select detail.SalaryReviewInfoIds);

                    string query = $@"SELECT SalaryReviewInfoIds FROM Payroll_SalaryProcessDetail
                    Where EmployeeId =@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                    AND dbo.fnGetFirstDateOfAMonth(SalaryYear,SalaryMonth) >= dbo.fnGetFirstDateOfAMonth(@Year,@Month)";

                    var salaryReviewIds = await _dapper.SqlQueryListAsync<string>(user.Database, query, new
                    {
                        EmployeeId = itemIbDb.EmployeeId,
                        Year = itemIbDb.EffectiveFrom.Value.Year,
                        Month = itemIbDb.EffectiveFrom.Value.Month,
                        CompanyId = user.CompanyId,
                        OrganizationId = user.OrganizationId,
                    });


                    if (salaryReviewIds.Any())
                    {
                        // Let's check
                        List<long> longs = new List<long>();
                        foreach (var item in salaryReviewIds)
                        {
                            if (!Utility.IsNullEmptyOrWhiteSpace(item))
                            {
                                var items = item.Split(',');
                                if (items.Length > 0)
                                {
                                    foreach (var i in items)
                                    {
                                        if (Utility.TryParseLong(i) > 0)
                                        {
                                            if (!longs.Contains(Utility.TryParseLong(i)))
                                            {
                                                longs.Add(Utility.TryParseLong(i));
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        var isItemExist = longs.SingleOrDefault(i => i == itemIbDb.SalaryReviewInfoId);
                        if (isItemExist > 0)
                        {
                            executionStatus = ResponseMessage.Message(false, "This salary review already exist in salary process");
                        }
                        else
                        {
                            // Do delete
                            // Previous Salary Review Id;

                            if((itemIbDb.PreviousReviewId??0) > 0)
                            {
                                var updatePreviousReviewInDb = await _payrollDbContext.Payroll_SalaryReviewInfo.FirstOrDefaultAsync(i => i.SalaryReviewInfoId == (itemIbDb.PreviousReviewId ?? 0)&& i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId);
                                if(updatePreviousReviewInDb != null && updatePreviousReviewInDb.SalaryReviewInfoId > 0)
                                {
                                    updatePreviousReviewInDb.EffectiveTo = null;
                                    updatePreviousReviewInDb.DeactivationDate = null;
                                    _payrollDbContext.Update(updatePreviousReviewInDb);
                                    _payrollDbContext.Remove(itemIbDb);
                                    var rowCount = await _payrollDbContext.SaveChangesAsync();
                                    if (rowCount > 0)
                                    {
                                        executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Deleted);
                                    }
                                    else
                                    {
                                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                                    }
                                }
                                else
                                {
                                    executionStatus = ResponseMessage.Message(false, "Cann't find previous salary review info");
                                }
                            }
                            else
                            {
                                executionStatus = ResponseMessage.Message(false, "Cann't find previous salary review info");
                            }
                            
                        }
                    }
                    else
                    {
                        // Do delete
                        if ((itemIbDb.PreviousReviewId ?? 0) > 0)
                        {
                            var updatePreviousReviewInDb = await _payrollDbContext.Payroll_SalaryReviewInfo.FirstOrDefaultAsync(i => i.SalaryReviewInfoId == (itemIbDb.PreviousReviewId ?? 0) && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId);
                            if (updatePreviousReviewInDb != null && updatePreviousReviewInDb.SalaryReviewInfoId > 0)
                            {
                                updatePreviousReviewInDb.EffectiveTo = null;
                                updatePreviousReviewInDb.DeactivationDate = null;
                                updatePreviousReviewInDb.DeactivationDate = null;

                                _payrollDbContext.Update(updatePreviousReviewInDb);
                                _payrollDbContext.Remove(itemIbDb);
                                var rowCount = await _payrollDbContext.SaveChangesAsync();
                                if (rowCount > 0)
                                {
                                    executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Deleted);
                                }
                                else
                                {
                                    executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                                }
                            }
                            else
                            {
                                executionStatus = ResponseMessage.Message(false, "Cann't find previous salary review info");
                            }
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, "Cann't find previous salary review info");
                        }
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "Item not found/Status has been changed/Effective date is missing");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewBusiness", "DeletePendingReviewAsync", user);
            }
            return executionStatus;
        }
    }
}
