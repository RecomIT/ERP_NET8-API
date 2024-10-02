using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.DTO.Info
{
    public class EmployeePFActivationDTO
    {
        public long PFActivationId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [Required, StringLength(200)]
        public string PFBasedAmount { get; set; }
        [Required, Column(TypeName = "decimal(18,2)")]
        public decimal? PFPercentage { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PFEffectiveDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? PFActivationDate { get; set; }
        [StringLength(500)]
        public string Remarks { get; set; }
    }
}
