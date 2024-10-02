using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Overtime.Domain
{

    [Table("Payroll_OvertimeApprovalLevel")]
    public class OvertimeApprovalLevel : BaseModel1
    {
        [Key]
        public long OvertimeApprovalLevelId { get; set; }

        [Required]
        public int MaximumLevel { get; set; } = 1;
        public int MinimumLevel { get; set; } = 1;

    }

}
