using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Overtime.Domain
{

    [Table("Payroll_OvertimeApprover")]
    public class OvertimeApprover : BaseModel1
    {
        [Key]
        public long OvertimeApproverId { get; set; }

        [Required]
        public long EmployeeId { get; set; }
        public bool IsActive { get; set; } = true;
        public bool ProxyEnabled { get; set; } = false;
        public long ProxyApproverId { get; set; } = 0;


    }

}
