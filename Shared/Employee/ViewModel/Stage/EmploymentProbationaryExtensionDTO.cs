using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentProbationaryExtensionDTO
    {
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
        [StringLength(200)]
        public string AppraiserComment { get; set; }
    }
}
