using Shared.OtherModels.Pagination;

namespace Shared.Leave.Filter.Report
{
    public class LeaveQuery_Filter : Sortparam
    {
        public string monthNo { get; set; }
        public string monthYear { get; set; }

        public string employeeId { get; set; }
        public string departmentId { get; set; }
        public string sectionId { get; set; }
        public string subSectionId { get; set; }
        public string zoneId { get; set; }
        public string unitId { get; set; }

        public string leaveTypeId { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
        public string isActive { get; set; }

    }
}
