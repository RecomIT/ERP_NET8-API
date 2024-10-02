using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Control_Panel.ViewModels
{
    public class ReportConfigViewModel
    {
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
        public long? FiscalYearId { get; set; }
        public bool IsActive { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactiveDate { get; set; }
        public long? AuthorizorId { get; set; }
        [StringLength(1000)]
        public string SignaturePath { get; set; }
    }
}
