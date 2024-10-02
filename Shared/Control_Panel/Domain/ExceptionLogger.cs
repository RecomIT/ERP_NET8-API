using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblExceptionLogger")]
    public class ExceptionLogger : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public string StackTrace { get; set; }
        public string Source { get; set; }
        public string Message { get; set; }
        public string HelpLink { get; set; }
        [StringLength(50)]
        public string FileName { get; set; }
        [StringLength(50)]
        public string MethodName { get; set; }
        [StringLength(20)]
        public string LineNo { get; set; }
        public string Location { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public int? ErrorNumber { get; set; }
        public int? ErrorState { get; set; }
        public int? ErrorSeverity { get; set; }
        public DateTime? ErrorDateTime { get; set; }
        public long BranchId { get; set; }
        [StringLength(100)]
        public string LogInIP { get; set; }
        [StringLength(100)]
        public string PCName { get; set; }
        [StringLength(100)]
        public string MACID { get; set; }
        [StringLength(100)]
        public string DeviceType { get; set; }
        [StringLength(100)]
        public string DeviceModel { get; set; }
        public string UserId { get; set; }
        public string Flag { get; set; }
    }
}
