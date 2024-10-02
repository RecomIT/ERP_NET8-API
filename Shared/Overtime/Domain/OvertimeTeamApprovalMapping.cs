using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Overtime.Domain
{

    [Table("Payroll_OvertimeTeamApprovalMapping")]
    public class OvertimeTeamApprovalMapping : BaseModel1
    {
        [Key]
        public long OvertimeTeamApprovalMappingId { get; set; }

        [Required]
        public long OvertimeApproverId { get; set; }

        [Required]
        public long EmployeeId { get; set; }

        [Required]
        public long ApprovalLevel { get; set; } = 1;

    }

}
