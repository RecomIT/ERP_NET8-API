using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblReportConfig")]
    public class ReportConfig : BaseModel
    {
        [Key]
        public long ReportConfigId { get; set; }
        [StringLength(500)]
        public string ReportCategory { get; set; } // Payslip // Tax Card
        [StringLength(500)]
        public string ReportPath { get; set; }
        [StringLength(500)]
        public string SubReport1ReportPath { get; set; }
        [StringLength(500)]
        public string SubReport1Process { get; set; }
        [StringLength(500)]
        public string SubReport2ReportPath { get; set; }
        [StringLength(500)]
        public string SubReport2Process { get; set; }
        [StringLength(500)]
        public string MinifiedReportPath { get; set; }
        [StringLength(500)]
        public string FinalReportPath { get; set; }
        public short? Month { get; set; }
        [StringLength(500)]
        public string ServiceName { get; set; }
        [StringLength(500)]
        public string ProcessName { get; set; } // SP NAME
        [StringLength(1000)]
        public string LogoPath { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActiveDate { get; set; }
        [StringLength(100)]
        public string FiscalYearRange { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactiveDate { get; set; }
        public long? AuthorizorId { get; set; }
        [StringLength(1000)]
        public string SignaturePath { get; set; }
        public long? ReportCategoryId { get; set; }
        public long? ReportSubCategoryId { get; set; }
        public long? ReportAuthorizationId { get; set; }
        public long? ReportSignatoriesId { get; set; }
    }
}
