using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblModules")]
    public class Module
    {
        [Key]
        public long ModuleId { get; set; }
        [StringLength(100)]
        public string ModuleName { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("ApplicationId")]
        public long ApplicationId { get; set; }
        public Application Application { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<MainMenu> MainMenus { get; set; }
    }
}
