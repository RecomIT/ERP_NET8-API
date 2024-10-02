using Shared.OtherModels.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.Filter.Stage
{
    public class Confimation_Filter : Sortparam
    {
        public string ConfirmationProposalId { get; set; }
        public string EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
