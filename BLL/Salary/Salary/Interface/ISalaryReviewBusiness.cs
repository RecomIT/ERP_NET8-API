using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using System.Data;
using Shared.Payroll.DTO.Salary;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.Helpers.SalaryProcess;
using Shared.Payroll.Domain.Salary;

namespace BLL.Salary.Salary.Interface
{
    public interface ISalaryReviewBusiness
    {
        Task<ExecutionStatus> ValidateSalaryReviewAsync(SalaryReviewInfoDTO model, AppUser user);
        Task<ExecutionStatus> SaveSalaryReviewAsync(SalaryReviewInfoDTO model, AppUser user);
        Task<IEnumerable<SalaryReviewInfoViewModel>> GetSalaryReviewInfosAsync(SalaryReview_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveSalaryReviewStatusAsync(SalaryReviewStatusDTO model, AppUser user);
        Task<IEnumerable<SalaryReviewDetailViewModel>> GetSalaryAllowanceForReviewAsync(long employeeId, AppUser user);
        Task<IEnumerable<SalaryReviewDetailViewModel>> GetSalaryReviewDetailsAsync(SalaryReview_Filter filter, AppUser user);
        Task<IEnumerable<SalaryReviewInfoViewModel>> GetEmployeeLastSalaryReviewAccordingToCutOffDate(long? employeeId, string cutOffDate, AppUser user);
        Task<SalaryReviewInfoViewModel> GetLastSalaryReviewAccordingToCutOffDate(long? employeeId, string cutOffDate, AppUser user);
        Task<IEnumerable<ExecutionStatus>> UploadFlatSalaryReviewAsync(List<UploadFlatSalaryReviewInfoDTO> model, AppUser user);
        Task<EmployeeLastApprovedSalaryReviewInfo> GetLastSalaryReviewInfoByEmployeeAsync(long employeeId, AppUser user);
        Task<IEnumerable<EmployeeSalaryReviewInSalaryProcess>> GetEmployeeSalaryReviewesInSalaryProcess(SalaryReviewInSalaryProcess_Filter filter, AppUser user);
        Task<IEnumerable<SalaryReviewDetail>> GetSalaryReviewDetailsAsync(long salaryReviewInfoId, AppUser user);
        Task<ExecutionStatus> UploadSalaryReviewExcelAsync(List<UploadSalaryReviewReadDTO> salaryReviewDTOs, AppUser user);
        Task<DataTable> GetSalaryReviewSheetAsync(SalaryReviewSheet_Filter reviewSheet_Filter, AppUser user);
        Task<IEnumerable<SalaryReviewInfoViewModel>> GetAllPendingSalaryReviewesAsync(SalaryReview_Filter filter, AppUser user);
        Task<DataTable> DownloadSalaryReviewSheetAsync(SalaryReviewSheetDownload_Filter model, AppUser user);
        Task<ExecutionStatus> DeletePendingReviewAsync(long id, AppUser user);
        Task<ExecutionStatus> DeleteApprovedReviewAsync(long id, AppUser user);

    }
}
