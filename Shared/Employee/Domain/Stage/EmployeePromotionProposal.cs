using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Stage
{
    [Table("HR_EmployeePromotionProposal"), Index("Head", "Flag", "IsActive", "StateStatus", "EffectiveDate", "ActiveDate", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeePromotionProposal_NonClusteredIndex")] // Promotion
    public class EmployeePromotionProposal : BaseModel2
    {
        [Key]
        public long PromotionProposalId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string Head { get; set; }
        [StringLength(50)]
        public string Flag { get; set; } // JOINING // PROMOTION
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
        [Required, Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InActiveDate { get; set; }
        [StringLength(100)]
        public string InActiveBy { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActiveDate { get; set; }
        [StringLength(100)]
        public string ActiveBy { get; set; }
    }
}
