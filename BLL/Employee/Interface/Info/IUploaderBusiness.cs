using Shared.OtherModels.User;
using Shared.Employee.DTO.Info;
using Microsoft.AspNetCore.Http;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Info;

namespace BLL.Employee.Interface.Info
{
    public interface IUploaderBusiness
    {
        Task<IEnumerable<ExecutionStatus>> UploadEmployeeInfoAsync(List<EmployeeUploaderDTO> employees, AppUser user);
        Task<ExecutionStatus> SaveEmployeeInfoAsync(EmployeeUploaderDTO employees, AppUser user);
        Task<ExecutionStatus> SaveAsync(EmployeeUploadInformation employees, AppUser user);
        Task<ExecutionStatus> UpdateEmployeeInfoAsync(EmployeeUploaderDTO employees, AppUser user);
        Task<ExecutionStatus> UpdateAsync(EmployeeUploadInformation employees, AppUser user);
        Task<IEnumerable<DownloadEmployeeInfoViewModel>> DownloadEmployeesDataExcelAsync(AppUser user);
        Task<ReadInfoFromExcel> GetEmployeeInfoFromExcelAsync(IFormFile File, AppUser user);
        Task<List<ExecutionStatus>> SaveExcelData(List<ExcelInfoCollection> models, AppUser user);
        Task<ExecutionStatus> InsertAsync(ExcelInfoCollection model, AppUser user);
        Task<ExecutionStatus> UpdateAsync(ExcelInfoCollection model, AppUser user);
        Task<bool> IsDuplicate(long employeeId, string column, string value, List<ExcelInfoCollection> employees, AppUser user);
    }
}
