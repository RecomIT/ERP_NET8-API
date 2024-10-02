using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using Shared.OtherModels.DataService;
using BLL.Employee.Interface.Info;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Info
{
    public class HierarchyBusiness : IHierarchyBusiness
    {
        private IDapperData _dapper;
        private ISysLogger _sysLogger;

        public HierarchyBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<EmployeeHierarchyViewModel>> GetEmployeeHierarchyAsync(EmployeeHierarchy_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeHierarchyViewModel> data = new List<EmployeeHierarchyViewModel>();
            try
            {
                var query = $@"Select e.EmployeeCode,e.FullName,
				SupervisorId,LineManagerId,ManagerId,HeadOfDepartmentId,HRAuthorityId,
				sup.EmployeeCode as [SupervisorCode],SupervisorName=sup.FullName ,
				lin.EmployeeCode AS [LineManagerCode],LineManagerName=lin.FullName,
				man.EmployeeCode AS [ManagerCode],ManagerName=man.FullName,
				hod.EmployeeCode AS [HeadOfDepartmentCode],HeadOfDepartmentName=hod.FullName,
				hr.EmployeeCode AS [HRAuthorityCode],HRAuthorityName=hr.FullName,eh.IsActive
				From HR_EmployeeHierarchy eh 
				LEFT JOIN HR_EmployeeInformation e on eh.EmployeeId = e.EmployeeId
				LEFT JOIN HR_EmployeeInformation sup on sup.EmployeeId = eh.SupervisorId
				LEFT JOIN HR_EmployeeInformation lin on lin.EmployeeId = eh.LineManagerId
				LEFT JOIN HR_EmployeeInformation man on man.EmployeeId = eh.ManagerId
				LEFT JOIN HR_EmployeeInformation hod on hod.EmployeeId = eh.HeadOfDepartmentId
				LEFT JOIN HR_EmployeeInformation hr on hr.EmployeeId = eh.HRAuthorityId
				Where 1=1 
				AND (@EmployeeId IS NULL OR @EmployeeId =0 OR eh.EmployeeId= @EmployeeId)  
				AND (eh.CompanyId=@CompanyId)
				AND (eh.OrganizationId=@OrganizationId)";
                data = await _dapper.SqlQueryListAsync<EmployeeHierarchyViewModel>(user.Database, query, new
                {
                    filter.EmployeeId,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyBusiness", "GetEmployeeHierarchyAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveEmployeeHierarchyAsync(EmployeeHierarchyDTO EmployeeHierarchy, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {

                var sp_name = "sp_HR_EmployeeHierarchy";
                var parameters = new DynamicParameters();
                parameters.Add("Id", EmployeeHierarchy.Id);
                parameters.Add("EmployeeId", EmployeeHierarchy.EmployeeId);
                parameters.Add("SupervisorId", EmployeeHierarchy.SupervisorId ?? 0);
                parameters.Add("SupervisorName", EmployeeHierarchy.SupervisorName);
                parameters.Add("ManagerId", EmployeeHierarchy.ManagerId ?? 0);
                parameters.Add("ManagerName", EmployeeHierarchy.ManagerName);
                parameters.Add("LineManagerId", EmployeeHierarchy.LineManagerId ?? 0);
                parameters.Add("LineManagerName", EmployeeHierarchy.LineManagerName);
                parameters.Add("HeadOfDepartmentId", EmployeeHierarchy.HeadOfDepartmentId ?? 0);
                parameters.Add("HeadOfDepartmentName", EmployeeHierarchy.HeadOfDepartmentName);
                parameters.Add("HRAuthorityId", EmployeeHierarchy.HRAuthorityId ?? 0);
                parameters.Add("HRAuthorityName", EmployeeHierarchy.HRAuthorityName);
                parameters.Add("IsActive", EmployeeHierarchy.IsActive);
                parameters.Add("ActivationDate", EmployeeHierarchy.ActivationDate);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", EmployeeHierarchy.Id > 0 ? Data.Update : Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyBusiness", "SaveEmployeeHierarchyAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> EmployeeHierarchyValidatorAsync(EmployeeHierarchyDTO EmployeeHierarchy, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeHierarchy";
                var parameters = new DynamicParameters();
                parameters.Add("Id", EmployeeHierarchy.Id);
                parameters.Add("EmployeeId", EmployeeHierarchy.EmployeeId);
                parameters.Add("SupervisorId", EmployeeHierarchy.SupervisorId);
                parameters.Add("SupervisorName", EmployeeHierarchy.SupervisorName);
                parameters.Add("ManagerId", EmployeeHierarchy.ManagerId);
                parameters.Add("ManagerName", EmployeeHierarchy.ManagerName);
                parameters.Add("LineManagerId", EmployeeHierarchy.LineManagerId);
                parameters.Add("LineManagerName", EmployeeHierarchy.LineManagerName);
                parameters.Add("HeadOfDepartmentId", EmployeeHierarchy.HeadOfDepartmentId);
                parameters.Add("HeadOfDepartmentName", EmployeeHierarchy.HeadOfDepartmentName);
                parameters.Add("IsActive", EmployeeHierarchy.IsActive);
                parameters.Add("ActivationDate", EmployeeHierarchy.ActivationDate);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", Data.Validate);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyBusiness", "EmployeeHierarchyValidatorAsync", user);
            }
            return executionStatus;
        }

        public async Task<EmployeeHierarchyViewModel> GetEmployeeActiveHierarchyAsync(long employeeId, AppUser user)
        {
            EmployeeHierarchyViewModel employee = new EmployeeHierarchyViewModel();
            try
            {
                var query = $@"Select e.EmployeeCode,e.FullName,
				SupervisorId,LineManagerId,ManagerId,HeadOfDepartmentId,HRAuthorityId,
				sup.EmployeeCode as [SupervisorCode],SupervisorName=sup.FullName ,
				lin.EmployeeCode AS [LineManagerCode],LineManagerName=lin.FullName,
				man.EmployeeCode AS [ManagerCode],ManagerName=man.FullName,
				hod.EmployeeCode AS [HeadOfDepartmentCode],HeadOfDepartmentName=hod.FullName,
				hr.EmployeeCode AS [HRAuthorityCode],HRAuthorityName=hr.FullName,eh.IsActive
				From HR_EmployeeHierarchy eh 
				LEFT JOIN HR_EmployeeInformation e on eh.EmployeeId = e.EmployeeId
				LEFT JOIN HR_EmployeeInformation sup on sup.EmployeeId = eh.SupervisorId
				LEFT JOIN HR_EmployeeInformation lin on lin.EmployeeId = eh.LineManagerId
				LEFT JOIN HR_EmployeeInformation man on man.EmployeeId = eh.ManagerId
				LEFT JOIN HR_EmployeeInformation hod on hod.EmployeeId = eh.HeadOfDepartmentId
				LEFT JOIN HR_EmployeeInformation hr on hr.EmployeeId = eh.HRAuthorityId
				Where 1=1 
				AND (eh.EmployeeId= @EmployeeId)
				AND (eh.IsActive=1)
				AND (eh.CompanyId=@CompanyId)
				AND (eh.OrganizationId=@OrganizationId)";

                employee = await _dapper.SqlQueryFirstAsync<EmployeeHierarchyViewModel>(user.Database, query, new
                {
                    EmployeeId = employeeId,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyBusiness", "GetEmployeeActiveHierarchyAsync", user);
            }
            return employee;
        }

        public async Task<IEnumerable<Select2Dropdown>> GetSubordinatesAsync(long id, AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT 
                [Id]=EMP.EmployeeId,
                [Value]=CAST(EMP.EmployeeId AS NVARCHAR(50)),
                [Text]=(FullName+' ['+EmployeeCode+']') FROM HR_EmployeeInformation EMP
                INNER JOIN (SELECT EmployeeId FROM HR_EmployeeHierarchy EH
                INNER JOIN (SELECT DISTINCT Id FROM(
                SELECT Id FROM HR_EmployeeHierarchy Where SupervisorId = @Id AND IsActive=1 AND CompanyId=@CompanyId
                Union All
                SELECT Id FROM HR_EmployeeHierarchy Where HeadOfDepartmentId = @Id AND IsActive=1 AND CompanyId=@CompanyId
                ) tbl) tbl1 ON tbl1.Id = EH.Id) Employee ON EMP.EmployeeId= Employee.EmployeeId
                Where 1=1
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeHierarchyBusiness", "GetSubordinatesAsync", user);
            }
            return list;
        }
    }
}
