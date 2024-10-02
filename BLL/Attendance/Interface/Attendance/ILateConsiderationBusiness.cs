using Shared.Attendance.ViewModel.Attendance.EarlyDeparture;
using Shared.Attendance.ViewModel.Attendance.LateConsideration;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Attendance.Interface.Attendance
{
    public interface ILateConsiderationBusiness
    {

        Task<IEnumerable<Select2Dropdown>> GetLateTransactionDateAsync(AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetLateReasonsAsync(long lateReasonId, AppUser user);
        Task<IEnumerable<Select2Dropdown>> GetSupervisorAsync(AppUser user);
        Task<ExecutionStatus> SaveLateRequestAsync(List<LateRequestViewModel> groupAssignmentDTOs, AppUser user);
        Task<DBResponse<LateRequestViewModel>> GetLateConsiderationMasterAsync(LateRequestFilter query, AppUser User);
        Task<DBResponse<LateRequestsDetailViewModel>> GetLateConsiderationDetailAsync(long lateRequestsId, AppUser User);
        Task<ExecutionStatus> UpdateStatusLateRequestDetaileAsync(long lateRequestsDetailId, string comment, string flag, long attendanceId, long lateRequestsId, AppUser user);
        Task<DBResponse<LateRequestViewModel>> GetLateConsiderationMasterByIdAsync(LateRequestFilter query, AppUser User);

        Task<ExecutionStatus> feedbackEmailLateRequestAsync(List<feedbackdata> dataList, AppUser user);
        Task<ExecutionStatus> SaveEarlyDepartureAsync(EarlyDepartureViewModel earlyDeparture, AppUser user);
        Task<IEnumerable<EarlyDepartureViewModel>> GetEarlyDepartureAsync(long? earlyDepartureId, long? employeeId, string flag, AppUser user);
        Task<DBResponse<EarlyDepartureViewModel>> GetEarlyMasterAsync(LateRequestFilter query, AppUser User);
        //Task<DBResponse<EarlyDepartureViewModel>> GetEarlyDepartureByIdAsync(long? earlyDepartureId, AppUser User);
        Task<IEnumerable<EarlyDepartureViewModel>> GetEarlyDepartureByIdAsync(long earlyDepartureId, AppUser user);
        Task<ExecutionStatus> UpdateEarlyDepartureAsync(long earlyDepartureId, string comment, string flag, AppUser user);
        Task<ExecutionStatus> feedbackEmailEarlyDepartureAsync(List<EarlyDepartureFeedbackdata> dataList, AppUser user);

        Task<DBResponse<EarlyDepartureViewModel>> GetEarlyMasterByIdAsync(LateRequestFilter query, AppUser User);
    }
}
