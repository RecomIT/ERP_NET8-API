using iText.Layout.Properties;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.DTO.Termination
{
    public class DiscontinuedEmployeeDTO
    {
        public long DiscontinuedId { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "Date")]
        public DateTime? LastWorkingDate { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool? CalculateFestivalBonusTaxProratedBasis { get; set; }
        public bool? CalculateProjectionTaxProratedBasis { get; set; }
        [StringLength(50), Required]
        public string Releasetype { get; set; }
        public bool? IsFullMonthSalaryHold { get; set; }
    }
}
