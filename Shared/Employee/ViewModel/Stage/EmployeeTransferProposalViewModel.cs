using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmployeeTransferProposalViewModel : BaseViewModel3
    {
        public long TransferProposalId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string Head { get; set; } // Branch / Department / Zone / Section / Subsection / Unit
        [StringLength(50)]
        public string Flag { get; set; } // JOINING
        [StringLength(50)]
        public string ExistingValue { get; set; }
        [StringLength(200)]
        public string ExistingText { get; set; }
        [Required, StringLength(50)]
        public string ProposalValue { get; set; }
        [StringLength(200)]
        public string ProposalText { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; }
        public DateTime? InActiveDate { get; set; }
        [StringLength(100)]
        public string InActiveBy { get; set; }
        public DateTime? ActiveDate { get; set; }
        [StringLength(100)]
        public string ActiveBy { get; set; }
        // Extra Properties
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [StringLength(100)]
        public string EmployeeName { get; set; }
    }
}
