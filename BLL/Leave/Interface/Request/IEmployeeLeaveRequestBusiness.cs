using Shared.Leave.DTO.Request;
using Shared.Leave.Filter.Request;
using Shared.Leave.ViewModel.Request;
using Shared.Leave.ViewModel.Setup;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Leave.Interface.Request
{
    public interface IEmployeeLeaveRequestBusiness
    {
        Task<DBResponse<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveRequestsAsync(LeaveRequest_Filter filter, AppUser user);
        Task<ExecutionStatus> DeleteEmployeeLeaveRequestAsync(DeleteEmployeeLeaveRequestDTO model, AppUser user);
        Task<ExecutionStatus> SaveEmployeeLeaveRequestAsync(LeaveRequestDTO model, AppUser user);
        Task<ExecutionStatus> SaveEmployeeLeaveRequest2Async(LeaveRequestDTO model, AppUser user);
        Task<ExecutionStatus> ValidatorEmployeeLeaveRequestAsync(EmployeeLeaveRequestDTO model, AppUser user);
        Task<ExecutionStatus> SaveEmployeeLeaveRequestStatusAsync(EmployeeLeaveRequestStatusDTO model, AppUser user);
        Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveRequestsForSupervisorApprovalAsync(LeaveRequest_Filter filter, AppUser user);
        Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveHistoryAsync(LeaveHistory_Filter filter, AppUser user);
        Task<IEnumerable<LeaveRequestEmailSendViewModel>> LeaveRequestEmailSendAsync(LeaveRequestEmail_Filter filter, AppUser user);
        Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveHistoryAsync(LeaveRequest_Filter filter, AppUser user);
        Task<string> GetEmployeeLeaveCodeAsync(AppUser user);
        Task<bool> IsLeavePendingAsync(long EmployeeLeaveRequestId, AppUser user);
        Task<bool> SendLeaveEmailAsync(ExecutionStatus execution, AppUser user);
        //Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(LeaveRequest_Filter filter, AppUser user);
        Task<DBResponse<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(LeaveRequest_Filter filter, AppUser user);
        Task<ExecutionStatus> LeaveRequestApprovalAsync(EmployeeLeaveRequestStatusDTO model, AppUser user);
        Task<ExecutionStatus> ApprovedLeaveCancellationAsync(ApprovedLeaveCancellationDTO model, AppUser user);
        Task<EmployeeLeaveRequestInfoAndDetailViewModel> GetEmployeeLeaveRequestInfoAndDetailById(long id, AppUser user);
    }
}
