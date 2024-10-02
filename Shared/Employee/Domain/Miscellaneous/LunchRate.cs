using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_LunchRate"), Microsoft.EntityFrameworkCore.Index("LunchRateId", "Rate", "ValidFrom", "ValidTo", "CompanyId", "OrganizationId", "BranchId", IsUnique = false, Name = "IX_HR_LunchRate_NonClusteredIndex")]
    public class LunchRate : BaseModel1
    {
        [Key]
        public long LunchRateId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Rate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ValidFrom { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ValidTo { get; set; }
    }
}
