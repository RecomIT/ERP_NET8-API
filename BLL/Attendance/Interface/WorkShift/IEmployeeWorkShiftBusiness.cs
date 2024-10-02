using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.Attendance.ViewModel.Workshift;
using Shared.Attendance.DTO.Workshift;
using Shared.Attendance.Filter.Shift;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;

namespace BLL.Attendance.Interface.WorkShift
{
    public interface IEmployeeWorkShiftBusiness
    {
        Task<IEnumerable<EmployeeWorkShiftViewModel>> GetEmployeeWorkShiftsAsync(EmployeeShift_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveEmployeesWorkShiftAsync(List<EmployeeWorkShiftDTO> model, AppUser user);
        Task<ExecutionStatus> EmployeeWorkShiftValidatorAsync(List<EmployeeWorkShiftDTO> model, AppUser user);
        Task<ExecutionStatus> SaveEmployeesWorkShiftCheckingAsync(List<EmployeeWorkShiftStatusDTO> model, AppUser user);
        Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeesForShiftAssignAsync(EmployeeService_Filter filter, AppUser user);
        Task<EmployeeShiftViewModel> GetEmployeeShiftByIdAysnc(EmployeeShift_Filter query, AppUser user);
    }
}
