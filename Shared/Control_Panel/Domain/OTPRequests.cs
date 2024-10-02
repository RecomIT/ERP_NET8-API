using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblOTPRequests")]
    public class OTPRequests
    {
        [Key]
        public long RequestId { get; set; }
        [StringLength(50)]
        public string RequestUniqId { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(50)]
        public string PublicIP { get; set; }
        [StringLength(50)]
        public string PrivateIP { get; set; }
        [StringLength(100)]
        public string DeviceType { get; set; }
        [StringLength(100)]
        public string OS { get; set; }
        [StringLength(100)]
        public string OSVersion { get; set; }
        [StringLength(100)]
        public string Browser { get; set; }
        [StringLength(100)]
        public string BrowserVersion { get; set; }
        public bool IsVerified { get; set; }
        [StringLength(6)]
        public string OTP { get; set; }
        public DateTime? OTPLifeTime { get; set; }
        public DateTime? VerifiedTime { get; set; }
    }
}
