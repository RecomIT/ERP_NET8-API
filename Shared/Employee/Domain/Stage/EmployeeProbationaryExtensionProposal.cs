using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Stage
{
    [Table("HR_EmployeeProbationaryExtensionProposal"), Index("ExtensionFrom", "StateStatus", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeProbationaryExtensionProposal_NonClusteredIndex")]
    public class EmployeeProbationaryExtensionProposal : BaseModel2
    {
        [Key]
        public long ProbationaryExtensionId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? ExtensionFrom { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? ExtensionTo { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        [StringLength(50)]
        public string TotalRatingScore { get; set; }
        [StringLength(50)]
        public string AppraiserId { get; set; }
        [StringLength(200)]
        public string AppraiserComment { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
