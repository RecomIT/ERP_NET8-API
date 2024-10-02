using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_Line"), Index("LineName", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Line_NonClusteredIndex")]
    public class Line : BaseModel
    {
        [Key]
        public long LineId { get; set; }
        [Required, StringLength(100)]
        public string LineName { get; set; }
        [StringLength(100)]
        public string LineNameInBengali { get; set; }
        [StringLength(100)]
        public string ShortName { get; set; }
        [StringLength(10)]
        public string LineCode { get; set; } //LN-0001
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int? DepartmentId { get; set; }
        public int? SectionId { get; set; }
        public long? BranchId { get; set; }
    }
}
