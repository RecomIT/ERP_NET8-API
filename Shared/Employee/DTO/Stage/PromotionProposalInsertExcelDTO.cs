using Shared.BaseModels.For_ViewModel;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Stage
{
    public class PromotionProposalInsertExcelDTO : BaseViewModel3
    {
        public string EmployeeCode { get; set; }
        public long? EmployeeId { get; set; }
        public long? DesigId { get; set; }
        public long? GrdId { get; set; }
        [Required, StringLength(100)]
        public string Head { get; set; }
        [Required, StringLength(50)]
        public string ProposalValue { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public string ProposalText { get; set; }
        public string GradeName { get; set; }
        public long? PrevDesignationId { get; set; }
        public string PrevDesignationName { get; set; }
    }
}
