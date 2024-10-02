using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Stage
{
    [Table("HR_EmployeeConfirmationProposal"), Index("ConfirmationDate", "EffectiveDate", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeConfirmationProposal_NonClusteredIndex")] // Comfirmation / Probation Extension
    public class EmployeeConfirmationProposal : BaseModel2
    {
        [Key]
        public long ConfirmationProposalId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ConfirmationDate { get; set; }
        [StringLength(50)]
        public string TotalRatingScore { get; set; }
        [StringLength(50)]
        public string AppraiserId { get; set; }
        [StringLength(200)]
        public string AppraiserComment { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        public bool? WithPFActivation { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
