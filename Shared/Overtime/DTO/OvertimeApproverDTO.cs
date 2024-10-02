using System;

namespace Shared.Overtime.DTO
{
    public class OvertimeApproverDTO
    {
        public long OvertimeApproverId { get; set; } = 0;
        public long EmployeeId { get; set; } = 0;
        public string EmployeeCode { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Division { get; set; }
        public string Branch { get; set; }
        public bool IsActive { get; set; } = false;
        public bool ProxyEnabled { get; set; } = false;
        public long ProxyApproverId { get; set; } = 0;
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}