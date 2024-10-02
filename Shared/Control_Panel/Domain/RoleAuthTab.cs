using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblRoleAuthTab")]
    public class RoleAuthTab : BaseModel
    {
        [Key]
        public long RATId { get; set; }
        [StringLength(256)]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public long SubmenuId { get; set; }
        public long TabId { get; set; }
        [StringLength(100)]
        public string TabName { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Detail { get; set; }
        public bool Delete { get; set; }
        public bool Approval { get; set; }
        public bool Report { get; set; }
        public bool Check { get; set; }
        public bool Accept { get; set; }
        public bool Upload { get; set; }
        public long BranchId { get; set; }
        [ForeignKey("RoleAuthorization")]
        public long? TaskId { get; set; }
        public RoleAuthorization RoleAuthorization { get; set; }
    }
}
