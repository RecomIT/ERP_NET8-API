using Shared.BaseModels.For_DomainModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblRoleAuthorization")]
    public class RoleAuthorization : BaseModel
    {
        [Key]
        public long TaskId { get; set; }
        public string RoleId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public long SubmenuId { get; set; }
        public long ParentSubmenuId { get; set; }
        public bool IsSubmenuPermission { get; set; }
        public bool IsPageTabPermission { get; set; }
        public bool HasTab { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Detail { get; set; }
        public bool Delete { get; set; }
        public bool Check { get; set; }
        public bool Approval { get; set; }
        public bool Accept { get; set; }
        public bool Report { get; set; }
        public bool Upload { get; set; }
        public long BranchId { get; set; }
        public ICollection<RoleAuthTab> RoleAuthTabs { get; set; }
    }
}
