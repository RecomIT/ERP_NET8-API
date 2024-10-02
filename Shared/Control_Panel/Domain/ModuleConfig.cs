using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblModuleConfig")]
    public class ModuleConfig : BaseModel
    {
        [Key]
        public long ModuleConfigId { get; set; }
        public long ConfigId { get; set; }
        [StringLength(20)]
        public string ConfigCode { get; set; }
        [StringLength(300)]
        public string ConfigText { get; set; }
        [StringLength(100)]
        public string ConfigValue { get; set; }
        public long MainmenuId { get; set; }
        public long ModuleId { get; set; }
        public long ApplicationId { get; set; }
        public long? BranchId { get; set; }
    }
}
