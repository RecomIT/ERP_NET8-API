using Shared.Leave.ViewModel.History;
using System.Collections.Generic;

namespace Shared.Leave.ViewModel.Request
{
    public class EmployeeLeaveRequestInfoAndDetailViewModel
    {
        public EmployeeLeaveRequestInfoAndDetailViewModel()
        {
            Info = new EmployeeLeaveRequestViewModel();
            Histories = new List<EmployeeLeaveHistoryInfoViewModel>();
        }
        public EmployeeLeaveRequestViewModel Info { get; set; }
        public IEnumerable<EmployeeLeaveHistoryInfoViewModel> Histories { get; set; }
    }
}
