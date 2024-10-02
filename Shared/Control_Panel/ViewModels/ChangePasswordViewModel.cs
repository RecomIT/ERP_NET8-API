using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Control_Panel.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string CurrentPassword { get; set; }
        [Required, DataType(DataType.Password), MinLength(6)]
        public string NewPassword { get; set; }
        [Required, Compare("NewPassword"), DataType(DataType.Password), MinLength(6)]
        public string ConfirmPassword { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
    }
}
