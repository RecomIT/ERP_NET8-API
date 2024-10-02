using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_ConditionalProjectedPaymentParameter"), Index("Flag", "ParameterId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_ConditionalProjectedPaymentParameter_NonClusteredIndex")]
    public class ConditionalProjectedPaymentParameter : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long FiscalYearId { get; set; }
        [Required, StringLength(100)]
        public string Flag { get; set; } // ID:  Branch / Internal Designation / Designation / Department / Section / Subsection // Employee Type
        public long ParameterId { get; set; }
        [ForeignKey("ConditionalProjectedPaymentId")]
        public long ConditionalProjectedPaymentId { get; set; }
        public ConditionalProjectedPayment ConditionalProjectedPayment { get; set; }
    }
}
