using System.Data;
using Shared.OtherModels.User;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.Domain.Info;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.ViewModel.Info;

namespace BLL.Employee.Interface.Info
{
    public interface IInfoBusiness
    {
        Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeeServiceDataAsync(EmployeeService_Filter filter, AppUser user);
        Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeeDataAsync(EmployeeService_Filter filter, AppUser user);
        Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeesForShiftAssignAsync(EmployeeService_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveEmployeeAsync(EmployeeInit employee, AppUser User);
        Task<ExecutionStatus> ValidateEmployeeAsync(EmployeeInit employee, AppUser user);
        Task<EmployeeInformation> GetEmployeeInformationById(long id, AppUser user);
        Task<EmployeeInformation> GetEmployeeInformationByCode(string code, AppUser user);
        Task<EmployeeDetail> GetEmployeeDetailById(long id, AppUser user);
        Task<EmployeeOfficeInfoViewModel> GetEmployeeOfficeInfoByIdAsync(EmployeeOfficeInfo_Filter filter, AppUser user);
        Task<DBResponse<EmployeePersonalInfoList>> GetEmployeePersonalInfosAsync(EmployeePersonalInfoQuery query, AppUser User);
        Task<EmployeePersonalInfoList> GetEmployeePersonalInfoByIdAsync(EmployeePersonalInfoQuery query, AppUser User);
        Task<ExecutionStatus> SaveEmploymentApprovalAsync(EmployeeApprovalDTO model, AppUser User);
        Task<ExecutionStatus> SaveEmployeeProfessionalInfoAsync(EmployeeOfficeInfo professionalInfo, AppUser User);
        Task<ExecutionStatus> SaveEmployeePersonalInfoAsync(EmployeePersonalInfo personalInfo, AppUser User);
        Task<DBResponse<EmployeeInformationViewModel>> GetEmployeesAsync(EmployeeQuery employeeQuery, AppUser User);
        Task<IEnumerable<EmployeeInformationViewModel>> GetClientEmployeesAsync(long clientCompany, long clientOrganization, AppUser user);
        Task<IEnumerable<EmployeeInformationViewModel>> GetClientEmployeeByIdAsync(long employeeId, long clientCompany, long clientOrganization, AppUser user);
        Task<ProfileInfo> GetEmployeeProfileInfoAsync(long employeeId, AppUser user);
        Task<ExecutionStatus> UploadProfileImageAsync(UploadProfileImage model, AppUser user);
        Task<EmployeeInformationViewModel> GetEmployeeAsync(long employeeId, AppUser user);
        Task<DataTable> GetEmployeeInformationForReportAsync(EmployeeInfoReport_Filter filter, bool isForDownloadExcelFile, AppUser user);
        Task<IEnumerable<Dropdown>> GetEmployeeItemsAsync(List<string> items, AppUser user);

        #region Validator
        Task<bool> IsOfficeEmailAvailableAsync(long id, string email, AppUser user);
        //Task<bool> IsOfficeEmailInEditAvailableAsync(long id, string email, AppUser user);
        Task<bool> IsEmployeeIdAvailableAsync(string code, AppUser user);
        Task<bool> IsEmployeeIdInEditAvailableAsync(long id, string code, AppUser user);
        #endregion
    }
}
