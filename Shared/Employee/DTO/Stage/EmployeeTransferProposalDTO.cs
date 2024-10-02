using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.DTO.Stage
{
    public class EmployeeTransferProposalDTO
    {
        public long TransferProposalId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string Head { get; set; } // Branch / Department / Zone / Section / Subsection / Unit
        [Required, StringLength(50)]
        public string ProposalValue { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
