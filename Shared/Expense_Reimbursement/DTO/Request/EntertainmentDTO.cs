using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.Expense_Reimbursement.DTO.Request
{
    public class EntertainmentDTO
    {
        public long RequestId { get; set; }
        public long EntertainmentId { get; set; }
        public long EntertainmentDetailId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public long EmployeeId { get; set; }
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        [StringLength(250)]
        public string Purpose { get; set; }

        [StringLength(500)]
        public string Entertainments { get; set; }
        [StringLength(250)]
        public string SpendMode { get; set; }
        [StringLength(350)]
        public string Description { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdvanceAmount { get; set; }


        public string Item { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        public string Flag { get; set; }
        public string FileName { get; set; }
        [StringLength(200)]
        public string ActualFileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        [StringLength(100)]
        public string FileFormat { get; set; }
        public IFormFile File { get; set; }



    }
}
