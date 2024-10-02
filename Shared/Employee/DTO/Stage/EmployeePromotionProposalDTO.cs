using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.DTO.Stage
{
    public class EmployeePromotionProposalDTO
    {
        public long PromotionProposalId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string Head { get; set; }
        [Required, StringLength(50)]
        public string ProposalValue { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
