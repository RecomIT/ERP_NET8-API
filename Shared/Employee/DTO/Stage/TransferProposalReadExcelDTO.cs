using Microsoft.AspNetCore.Http;
using Shared.BaseModels.For_ViewModel;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.DTO.Stage
{
    public class TransferProposalReadExcelDTO : BaseViewModel3
    {
        public string EmployeeCode { get; set; }
        public long? EmployeeId { get; set; }
        public long? DepartmentId { get; set; }
        [Required, StringLength(100)]
        public string Head { get; set; } // Branch / Department / Zone / Section / Subsection / Unit
        [Required, StringLength(50)]
        public string ProposalValue { get; set; }
        [Required]
        public DateTime? EffectiveDate { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public string ProposalText { get; set; }
        public long? PrevDepartmentId { get; set; }
        public string PrevDepartmentName { get; set; }
    }
    public class UploadTransferProposal : BaseViewModel2
    {
        [Required, StringLength(100)]
        public string Head { get; set; }
        public IFormFile ExcelFile { get; set; }
    }
}
