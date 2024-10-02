using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblApplications")]
    public class Application
    {
        [Key]
        public long ApplicationId { get; set; }
        [StringLength(100)]
        public string ApplicationName { get; set; }
        [StringLength(50)]
        public string ApplicationType { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<Module> Modules { get; set; }
    }
}
