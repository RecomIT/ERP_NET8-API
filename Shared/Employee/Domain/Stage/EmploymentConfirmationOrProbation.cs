using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Stage
{
    [Table("HR_EmploymentConfirmationOrProbotion"), Index("EmployeeId", "Type", "Flag", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmploymentConfirmationOrProbotion_NonClusteredIndex")]
    public class EmploymentConfirmationOrProbation : BaseModel3
    {
        [Key]
        public long Id { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string Type { get; set; }
        [StringLength(100)]
        public string Flag { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ConfirmationDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExtensionFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExtensionTo { get; set; }
        [StringLength(50)]
        public string TotalRatingScore { get; set; }
        [StringLength(50)]
        public string AppraiserId { get; set; }
        [StringLength(200)]
        public string AppraiserComment { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveDate { get; set; }
        public long EmploymentStageInfoId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
