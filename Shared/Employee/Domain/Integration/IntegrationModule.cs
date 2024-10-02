using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Integration
{
    [Table("HR_IntegrationModule")]
    public class IntegrationModule : BaseModel
    {
        [Key]
        public long Id { get; set; }
        [StringLength(150)]
        public string Module { get; set; }
        [ForeignKey("IntegrationConfig")]
        public long IntegrationConfigId { get; set; }
        public IntegrationConfig IntegrationConfig { get; set; }
        public ICollection<IntegrationColumnMapping> IntegrationColumnMappings { get; set; }
    }
}
