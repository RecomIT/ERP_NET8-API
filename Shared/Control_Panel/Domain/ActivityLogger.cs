using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblActivityLogger")]
    public class ActivityLogger : BaseModel
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string EMPCode { get; set; }
        [StringLength(100)]
        public string Controller { get; set; }
        [StringLength(100)]
        public string ActionMethod { get; set; }
        [StringLength(30)]
        public string ActionName { get; set; }
        public string ActionDetails { get; set; }
        public string ImpactTables { get; set; }
        public string PreviousValue { get; set; }
        public string UpdatedValue { get; set; }
        [StringLength(100)]
        public string LogInIP { get; set; }
        [StringLength(100)]
        public string MACID { get; set; }
        public long BranchId { get; set; }
        [StringLength(100)]
        public string UserName { get; set; }
    }
}
