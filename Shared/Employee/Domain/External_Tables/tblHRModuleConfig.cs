using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.External_Tables
{
    [Table("tblHRModuleConfig"), Keyless]
    public class tblHRModuleConfig
    {
        public long HRModuleConfigId { get; set; }
        public long ApplicationId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public long? BranchId { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(100)]
        public string AttendanceProcess { get; set; } // Upload / Machine
        public bool? EnableMaxLateWarning { get; set; }
        public short? MaxLateInMonth { get; set; }
        public bool? EnableSequenceLateWarning { get; set; }
        public short? SequenceLateInMonth { get; set; }
        public short? LeaveStartMonth { get; set; }
        public short? LeaveEndMonth { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
