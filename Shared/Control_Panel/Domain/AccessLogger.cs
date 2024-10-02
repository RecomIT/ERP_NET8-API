using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblAccessLogger")]
    public class AccessLogger
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string UserId { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
        [StringLength(100)]
        public string UserSessionId { get; set; } // GUID
        public string AccessToken { get; set; }
        [StringLength(100)]
        public string PublicIP { get; set; }
        [StringLength(100)]
        public string LocalIP { get; set; } // User PC LOCAL IP
        [StringLength(100)]
        public string DeviceType { get; set; } // Mobile/Tablet/Desktop
        [StringLength(100)]
        public string DeviceModel { get; set; }
        [StringLength(100)]
        public string DeviceName { get; set; }
        [StringLength(100)]
        public string OS { get; set; }
        [StringLength(100)]
        public string OSVersion { get; set; }
        [StringLength(100)]
        public string Browser { get; set; }
        [StringLength(100)]
        public string BrowserVersion { get; set; }
        [StringLength(200)]
        public string UserAgent { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public long? BranchId { get; set; }
        public long? CompanyId { get; set; }
        public long? OrganizationId { get; set; }
    }
}
