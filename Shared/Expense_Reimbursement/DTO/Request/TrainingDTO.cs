using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.Expense_Reimbursement.DTO.Request
{
    public class TrainingDTO
    {
        public long RequestId { get; set; }
        public string TransactionType { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long EmployeeId { get; set; }
        [Required, StringLength(100)]
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        [StringLength(250)]
        public string InstitutionName { get; set; }
        [StringLength(250)]
        public string Course { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> AdmissionDate { get; set; }
        [StringLength(350)]
        public string Duration { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TrainingCosts { get; set; }
        public string Purpose { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }

    }
}
