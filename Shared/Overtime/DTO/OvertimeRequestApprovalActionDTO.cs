using System.ComponentModel.DataAnnotations;


namespace Shared.Overtime.DTO
{
    public class OvertimeRequestApprovalActionDTO
    {
        public long OvertimeRequestId { get; set; }

        [Required]
        public string Status { get; set; }

        [Required]
        public string Remarks { get; set; }



    }
}
