using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeNomineeInfo"), Index("Relation", "DocumentType", "DocumentNumber", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmployeeNomineeInfo_NonClusteredIndex")]
    public class EmployeeNomineeInfo : BaseModel1
    {
        [Key]
        public long NomineeId { get; set; }
        [ForeignKey("EmployeeInformation")]
        public long EmployeeId { get; set; }
        public EmployeeInformation EmployeeInformation { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Relation { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(30)]
        public string Phone { get; set; }
        [StringLength(50)]
        public string DocumentType { get; set; }
        [StringLength(100)]
        public string DocumentNumber { get; set; }
        [StringLength(200)]
        public string PhotoPath { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Percentage { get; set; }
    }
}
