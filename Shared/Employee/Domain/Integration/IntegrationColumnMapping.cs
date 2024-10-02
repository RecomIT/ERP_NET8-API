using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Integration
{
    [Table("HR_IntegrationColumnMapping")]
    public class IntegrationColumnMapping : BaseModel
    {
        [Key]
        public long Id { get; set; }
        [StringLength(150)]
        public string SourceColumn { get; set; }
        [StringLength(150)]
        public string SystemColumn { get; set; }
        [StringLength(150)]
        public string SystemTable { get; set; }
        [ForeignKey("IntegrationModule")]
        public long IntegrationModuleId { get; set; }
        public IntegrationModule IntegrationModule { get; set; }
    }
}
