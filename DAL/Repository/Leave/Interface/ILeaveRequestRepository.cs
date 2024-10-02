using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using DAL.Repository.Base.Interface;
using Shared.Leave.DTO.Request;
using Shared.Leave.Filter.Request;
using Shared.Leave.Domain.Request;
using Shared.Leave.ViewModel.Request;

namespace DAL.Repository.Leave.Interface
{
    public interface ILeaveRequestRepository : IDapperBaseRepository<EmployeeLeaveRequest>
    {
        Task<ExecutionStatus> SaveAsync(LeaveRequestDTO model, AppUser user);
        Task<ExecutionStatus> ValidateAsync(EmployeeLeaveRequestDTO model, AppUser user);
        Task<EmployeeLeaveRequestViewModel> GetEmployeeLeaveRequestInfoById(long id, AppUser user);
        Task<EmployeeLeaveRequestInfoAndDetailViewModel> GetEmployeeLeaveRequestInfoAndDetailById(long id, AppUser user);
        Task<bool> IsLeavePendingAsync(long id, AppUser user);
        Task<string> GetLeaveCodeAysnc(AppUser user);
        Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(object filter, AppUser user);
        //Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(LeaveRequest_Filter filter, AppUser user);
        Task<DBResponse<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(LeaveRequest_Filter filter, AppUser user);
        Task<ExecutionStatus> LeaveRequestApprovalAsync(EmployeeLeaveRequestStatusDTO filter, AppUser user);
        Task<ExecutionStatus> DeleteEmployeeLeaveRequestAsync(DeleteEmployeeLeaveRequestDTO model, AppUser user);
        Task<ExecutionStatus> ApprovedLeaveCancellationAsync(ApprovedLeaveCancellationDTO model, AppUser user);
    }
}
