using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblPageTabs")]
    public class PageTab : BaseModel
    {
        [Key]
        public long TabId { get; set; }
        [StringLength(100)]
        public string TabName { get; set; }
        [StringLength(30)]
        public string IconClass { get; set; }
        [StringLength(20)]
        public string IconColor { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("SubmenuId")]
        public long? SubmenuId { get; set; }
        public SubMenu SubMenu { get; set; }
        public long MMId { get; set; }
        public long ComId { get; set; }
        public long BranchId { get; set; }
    }
}
