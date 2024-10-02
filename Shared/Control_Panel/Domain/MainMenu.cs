using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblMainMenus")]
    public class MainMenu
    {
        [Key]
        public long MMId { get; set; }
        [StringLength(100)]
        public string MenuName { get; set; }
        [StringLength(50)]
        public string ShortName { get; set; }
        [StringLength(100)]
        public string IconClass { get; set; }
        [StringLength(100)]
        public string IconColor { get; set; }
        [ForeignKey("Module")]
        public long MId { get; set; }
        public Module Module { get; set; }
        public long ApplicationId { get; set; }
        public bool IsActive { get; set; }
        public int? SequenceNo { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<SubMenu> SubMenus { get; set; }
    }
}
