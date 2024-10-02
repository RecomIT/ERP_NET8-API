using Shared.OtherModels.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.Filter.Stage
{
    public class EmployeePromotion_Filter : Sortparam
    {
        public long PromotionProposalId { get; set; }
        public string EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        public string Head { get; set; }
        public string EffectiveDate { get; set; }
        public string StateStatus { get; set; }
    }
}
