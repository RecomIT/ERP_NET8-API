using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using Shared.Employee.Filter.Stage;
using Shared.Employee.ViewModel.Stage;
using Shared.OtherModels.DataService;
using BLL.Employee.Interface.Stage;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;

namespace BLL.Employee.Implementation.Stage
{
    public class EmploymentConfirmationBusiness : IEmploymentConfirmationBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IEmploymentConfirmationRepository _employmentConfirmationRepository;
        public EmploymentConfirmationBusiness(IDapperData dapper, ISysLogger sysLogger, IEmploymentConfirmationRepository employmentConfirmationRepository)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _employmentConfirmationRepository = employmentConfirmationRepository;
        }
        public async Task<DBResponse<EmploymentConfirmationViewModel>> GetEmploymentConfirmationsAsync(Confimation_Filter filter, AppUser user)
        {
            DBResponse<EmploymentConfirmationViewModel> data = new DBResponse<EmploymentConfirmationViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _employmentConfirmationRepository.GetEmploymentConfirmationsAsync(parameters, user);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmploymentConfirmationViewModel>>(response.JSONData) ?? new List<EmploymentConfirmationViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationBusiness", "GetEmploymentConfirmationsAsync", user);
            }
            return data;
        }

        public async Task<IEnumerable<EmploymentConfirmationViewModel>> GetEmploymentConfirmationsDropdownAsync(Confimation_Filter filter, AppUser user)
        {
            IEnumerable<EmploymentConfirmationViewModel> list = new List<EmploymentConfirmationViewModel>();
            try
            {
                var query = $@"SELECT CON.ConfirmationProposalId,CON.EmployeeId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,
	            EMP.GradeName,EMP.DesignationName,EMP.DepartmentName,EMP.DateOfJoining,CON.ConfirmationDate,
	            CON.EffectiveDate,CON.AppraiserComment,CON.StateStatus,CON.CreatedDate,CON.CreatedBy
	            FROM HR_EmployeeConfirmationProposal CON
	            INNER JOIN [dbo].[vw_HR_EmployeeList] EMP ON CON.EmployeeId = EMP.EmployeeId
	            WHERE 1=1
	            AND (@ConfirmationProposalId=0 OR ISNULL(@ConfirmationProposalId,0)=0 OR CON.ConfirmationProposalId=@ConfirmationProposalId)
	            AND (ISNULL(@EmployeeId,0) = 0  OR EMP.EmployeeId=@EmployeeId)
	            AND (ISNULL(@EmployeeCode,'') = '' OR EMP.EmployeeCode LIKE '%'+@EmployeeCode+'%')
	            AND (ISNULL(@StateStatus,'')='' OR CON.StateStatus = @StateStatus)
	            AND (
		            (@FromDate IS NULL AND @ToDate IS NULL)
		            OR
		            ((ISNULL(@FromDate,'') <> '' AND ISNULL(@ToDate,'') <> '') AND ConfirmationDate Between Convert(date,@FromDate) AND Convert(date,@ToDate)) 
		            OR
		            (@FromDate <> '' AND ConfirmationDate = Convert(date,@FromDate))
		            OR	
		            (@ToDate <> '' AND ConfirmationDate =Convert(date,@ToDate))
		            OR
		            (@FromDate ='' AND @ToDate = '')
	            )
	            AND CON.CompanyId=@CompanyId
	            AND CON.OrganizationId=@OrganizationId";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<EmploymentConfirmationViewModel>(user.Database, query, CommandType.Text);

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationBusiness", "GetEmploymentConfirmationsDropdownAsync", user);
            }
            return list;
        }

        public async Task<IEnumerable<Select2Dropdown>> GetUnconfirmedEmployeeInfosInApplyAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                list = await _employmentConfirmationRepository.GetUnconfirmedEmployeeInfosInApplyAsync(user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationBusiness", "GetUnconfirmedEmployeeInfosInUpdateAsync", user);
            }
            return list;
        }

        public async Task<IEnumerable<Select2Dropdown>> GetUnconfirmedEmployeeInfosInUpdateAsync(AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                list = await _employmentConfirmationRepository.GetUnconfirmedEmployeeInfosInUpdateAsync(user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationBusiness", "GetUnconfirmedEmployeeInfosInUpdateAsync", user);
            }
            return list;
        }

        public async Task<ExecutionStatus> SaveEmploymentConfirmationAsync(EmploymentConfirmationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                //var sp_name = "sp_HR_EmploymentConfirmation_Insert_Update";
                //var parameters = DapperParam.AddParams(model, user);
                //executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                executionStatus = await _employmentConfirmationRepository.SaveAsync(model, user);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationBusiness", "SaveEmploymentConfirmationAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveEmploymentConfirmationStatusAsync(EmploymentConfirmationStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                //var sp_name = "sp_HR_EmploymentConfirmationStatus";
                //var parameters = DapperParam.AddParams(model, user);
                //executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                executionStatus = await _employmentConfirmationRepository.ConfirmationApprovalAsync(model, user);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmploymentConfirmationBusiness", "SaveEmploymentConfirmationStatusAsync", user);
            }
            return executionStatus;
        }
    }
}
