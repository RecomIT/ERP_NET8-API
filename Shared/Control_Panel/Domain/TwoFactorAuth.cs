using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Shared.Models;

namespace Shared.Control_Panel.Domain
{
    [Table("tblTwoFactorAuth")]
    public class TwoFactorAuth : BaseModel
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public long EmployeeId { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        public bool Enabled { get; set; } = false;
    }
}
