using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Setup
{
    [Table("HR_EmailSendingConfiguration"), Index("ModuleName", "EmailStage", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_EmailSendingConfiguration_NonClusteredIndex")]
    public class EmailSendingConfiguration : BaseModel
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200)]
        public string ModuleName { get; set; }
        [StringLength(200)]
        public string EmailStage { get; set; }
        public string EmailCC1 { get; set; }
        [StringLength(200)]
        public string EmailCC2 { get; set; }
        [StringLength(200)]
        public string EmailCC3 { get; set; }
        [StringLength(200)]
        public string EmailBCC1 { get; set; }
        [StringLength(200)]
        public string EmailBCC2 { get; set; }
        [StringLength(200)]
        public string EmailTo { get; set; }
        public bool IsActive { get; set; }
    }
}
