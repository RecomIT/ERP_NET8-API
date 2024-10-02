using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblOrganizationAuthorization")]
    public class OrganizationAuthorization : BaseModel
    {
        [Key]
        public long OrgAuthId { get; set; }
        public long ApplicationId { get; set; }
        public long MainmenuId { get; set; }
        public long ModuleId { get; set; }
    }
}
