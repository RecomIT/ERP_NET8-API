using Shared.OtherModels.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.Filter.Tax
{
    public class TaxDocumentQuery : Sortparam
    {
        public string SubmissionId { get; set; }
        public string EmployeeId { get; set; }
        public string FiscalYearId { get; set; }
        [StringLength(50)]
        public string CertificateType { get; set; }
        public string isAuction { get; set; }
    }
}
