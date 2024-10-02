using System;
using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace Shared.Employee.Domain.Stage
{
    [Table("HR_EmployeePFActivation"), Index("PFBasedAmount", "PFEffectiveDate", "PFActivationDate", "StateStatus", "IsApproved", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeePFActivation_NonClusteredIndex")] // PF Activation
    public class EmployeePFActivation : BaseModel2
    {
        [Key]
        public long PFActivationId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(200)]
        public string PFBasedAmount { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal? PFPercentage { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PFAmount { get; set; } = 0;
        [StringLength(200)]
        public string EmployeeName { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PFEffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PFActivationDate { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? InActiveDate { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public long? ConfirmationProposalId { get; set; }
    }
}
