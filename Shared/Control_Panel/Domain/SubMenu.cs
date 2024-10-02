using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblSubMenus")]
    public class SubMenu
    {
        [Key]
        public long SubmenuId { get; set; }
        [StringLength(100)]
        public string SubmenuName { get; set; }
        [StringLength(100)]
        public string ControllerName { get; set; }
        [StringLength(100)]
        public string ActionName { get; set; }
        [StringLength(100)]
        public string Path { get; set; }
        [StringLength(100)]
        public string Component { get; set; }
        [StringLength(100)]
        public string IconClass { get; set; }
        [StringLength(100)]
        public string IconColor { get; set; }
        public bool IsViewable { get; set; }
        public bool IsActAsParent { get; set; }
        public bool HasTab { get; set; }
        public bool IsActive { get; set; }
        public int? MenuSequence { get; set; }
        public long? ParentSubMenuId { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey("MainMenu")]
        public long MMId { get; set; }
        public MainMenu MainMenu { get; set; }
        public long ModuleId { get; set; }
        public long ApplicationId { get; set; }
        public ICollection<PageTab> PageTabs { get; set; }
    }
}
