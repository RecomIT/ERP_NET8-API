using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Models;

namespace Shared.Attendance.Domain.Attendance.LateConsideration
{

    [Table("HR_LateReasons")]
    public class LateReason : BaseModel5
    {
        [Key]
        public long LateReasonId { get; set; }
        public string LateReasonName { get; set; }
        public bool IsActive { get; set; }
    }
}
