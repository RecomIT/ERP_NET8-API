using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentConfirmationDTO
    {
        public long ConfirmationProposalId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(5)]
        public string TotalRatingScore { get; set; }
        [StringLength(200)]
        public string AppraiserComment { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? ConfirmationDate { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        public bool? WithPFActivation { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PFEffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PFActivationDate { get; set; }
    }
}
