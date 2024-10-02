using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblConfigurable")]
    public class Configurable
    {
        [Key]
        public long ConfigId { get; set; }
        [StringLength(20)]
        public string ConfigCode { get; set; }
        [StringLength(300)]
        public string ConfigText { get; set; }
        public long MainmenuId { get; set; }
        public long ModuleId { get; set; }
        public long ApplicationId { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
