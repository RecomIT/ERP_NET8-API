using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Control_Panel.ViewModels
{
    public class OTPRequestsViewModel
    {
        public long RequestId { get; set; }
        [StringLength(50)]
        public string RequestUniqId { get; set; }
        [Required, StringLength(200), EmailAddress]
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
    }

    public class OTPVerificationViewModel
    {
        [Required, StringLength(200)]
        public string Email { get; set; }
        [Required, StringLength(6)]
        public string OTP { get; set; }
        [Required]
        public string Token { get; set; }
    }

}
