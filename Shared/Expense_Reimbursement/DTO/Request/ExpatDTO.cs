using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.Expense_Reimbursement.DTO.Request
{
    public class ExpatDTO
    {
        public long ExpatDetailId { get; set; }
        public long RequestId { get; set; }        
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public long EmployeeId { get; set; }
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        [StringLength(250)]
        public string CompanyName { get; set; }
        [StringLength(500)]
        public string Particular { get; set; }
        [StringLength(300)]
        public string BillType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }
        [StringLength(500)]
        public string Expats { get; set; }
        [StringLength(250)]
        public string SpendMode { get; set; }
        [StringLength(350)]
        public string Description { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public string Flag { get; set; }

    }
}
