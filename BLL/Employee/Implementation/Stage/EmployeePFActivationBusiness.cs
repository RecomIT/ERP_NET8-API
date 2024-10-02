using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Info;
using Shared.Employee.DTO.Stage;
using Shared.Employee.Filter.Info;
using Shared.Employee.Filter.Stage;
using Shared.Employee.ViewModel.Stage;
using BLL.Employee.Interface.Stage;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;

namespace BLL.Employee.Implementation.Stage
{
    public class EmployeePFActivationBusiness : IEmployeePFActivationBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IEmployeePFActivationRepository _employeePFActivationRepository;
        public EmployeePFActivationBusiness(ISysLogger sysLogger, IDapperData dapper, IEmployeePFActivationRepository employeePFActivationRepository)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _employeePFActivationRepository = employeePFActivationRepository;
        }

        public async Task<IEnumerable<Select2Dropdown>> GetConfirmedEmployeesToAssignPFAsync(ConfirmedEmployeesToAssignPF_Filter filter, AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"Select Convert(Nvarchar(50),EmployeeId) 'Id' ,
				(FullName+' ['+EmployeeCode+']') 'Text', [IsActive]=ISNULL(IsActive,0) 
				From HR_EmployeeInformation 
				Where 1=1
				AND IsPFMember IS NULL OR IsPFMember= 0
				AND (@DesignationId IS NULL OR @DesignationId=0 OR DesignationId=@DesignationId) 
				AND (@DepartmentId IS NULL OR @DepartmentId=0 OR DepartmentId=@DepartmentId)
				AND (@SectionId IS NULL OR @SectionId=0 OR SectionId=@SectionId) 
				AND (@SubSectionId IS NULL OR @SubSectionId=0 OR SubSectionId=@SubSectionId) 
				AND (@NotEmployee IS NULL OR @NotEmployee=0 OR EmployeeId!=@NotEmployee)
				AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationBusiness", "GetConfirmedEmployeesToAssignPFAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SavePFActivationAsync(EmployeePFActivationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _employeePFActivationRepository.SaveAsync(model, user);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationBusiness", "SavePFActivationAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<EmployeePFActivationViewModel>> GetEmployeePFActivationListAsync(PFActivation_Filter filter, AppUser user)
        {
            IEnumerable<EmployeePFActivationViewModel> list = new List<EmployeePFActivationViewModel>();
            try
            {
                var query = $@"Select a.*, b.EmployeeCode, b.FullName EmployeeName From HR_EmployeePFActivation a, HR_EmployeeInformation b
				Where 1=1
                AND a.EmployeeId = b.EmployeeId
				AND (@PFBasedAmount IS NULL OR @PFBasedAmount = '' OR a.PFBasedAmount LIKE '%' +@PFBasedAmount + '%')		
				AND (@StateStatus IS NULL OR @StateStatus = '' OR a.StateStatus LIKE '%' +@StateStatus + '%')	
				AND (@PFActivationId IS NULL OR  @PFActivationId =0 OR a.PFActivationId = @PFActivationId)
			    AND (
						(@PFEffectiveDate IS NULL OR @PFEffectiveDate='' OR a.PFEffectiveDate = CAST(@PFEffectiveDate AS DATE) )
						OR
						((@EffectiveDateFrom <> '' AND @EffectiveDateTo <> '') 
						AND a.PFEffectiveDate BETWEEN Convert(date, @EffectiveDateFrom) AND Convert(date, @EffectiveDateTo)) 
						OR
						(@EffectiveDateFrom <> '' AND a.PFEffectiveDate = Convert(date, @EffectiveDateFrom))
						OR	
						(@EffectiveDateTo <> '' AND a.PFEffectiveDate = Convert(date, @EffectiveDateTo))
						OR
						(@EffectiveDateFrom ='' AND @EffectiveDateTo = '')						
				   )
				AND (a.CompanyId =@CompanyId)
				AND (a.OrganizationId = @OrganizationId)
				AND (@EmployeeId IS NULL OR @EmployeeId=0 OR a.EmployeeId = @EmployeeId)";

                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<EmployeePFActivationViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationBusiness", "GetEmployeePFActivationListAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetPFBasedAmountDropdownAsync(string baseAmount, AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"Select Convert(Nvarchar(20), b.PFActivationId) 'Id',b.PFBasedAmount 'Text'
				From HR_EmployeePFActivation b
				Where 1=1
				AND (b.CompanyId=@CompanyId)
				AND (b.OrganizationId=@OrganizationId)
				AND (b.BranchId=@BranchId)
				Order by b.PFActivationId asc";

                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationBusiness", "GetPFBasedAmountExtensionAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SavePFActivationApprovalAsync(EmployeePFActivationApprovalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                //var sp_name = "sp_HR_EmployeePFActivation";
                //var parameters = DapperParam.AddParams(model, user);
                //parameters.Add("Flag", Data.Checking);
                //executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
                executionStatus = await _employeePFActivationRepository.PFApprovalAsync(model, user);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationBusiness", "SavePFActivationApprovalAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadPFActivationExcelAsync(List<EmployeePFActivationViewModel> models, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_EmployeePFActivation";
                var jsonData = JsonReverseConverter.JsonData(models);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.UserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", user.BranchId);
                paramaters.Add("Flag", "PF_Activation_Upload");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationBusiness", "UploadPFActivationExcelAsync", user);
            }
            return executionStatus;
        }
        public async Task<EmployeePFActivationViewModel> EmployeePFActionInfoAysnc(long employeeId, AppUser user)
        {
            EmployeePFActivationViewModel info = new EmployeePFActivationViewModel();
            try
            {
                var query = $@"SELECT * FROM HR_EmployeePFActivation Where IsApproved=1 AND StateStatus='Approved' AND EmployeeId={employeeId} AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                info = await _dapper.SqlQueryFirstAsync<EmployeePFActivationViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePFActivationBusiness", "EmployeePFActionInfoAysnc", user);
            }
            return info;
        }
    }
}
