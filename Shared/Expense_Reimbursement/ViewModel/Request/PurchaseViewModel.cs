using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Expense_Reimbursement.ViewModel.Request
{
    public class PurchaseViewModel
    {
        public long PurchaseId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long EmployeeId { get; set; }
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        [StringLength(250)]
        public string Purpose { get; set; } 
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchaseCost { get; set; }
        [StringLength(500)]
        public string Purchases { get; set; }
        [StringLength(250)]
        public string SpendMode { get; set; }
        [StringLength(350)]
        public string Description { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
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
