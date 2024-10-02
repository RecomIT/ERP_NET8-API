using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Shared.Expense_Reimbursement.DTO.Request
{
    public class ConveyanceDTO
    {
        public long RequestId { get; set; }
        public long ConveyanceDetailId { get; set; }

        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public long EmployeeId { get; set; }
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        public string CompanyName { get; set; }
        public string SpendMode { get; set; }
        public string Purpose { get; set; }       
        public string Description { get; set; }
        public string Transportation { get; set; }
        public string StateStatus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdvanceAmount { get; set; }


        public string Destination { get; set; }
        public string Mode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }

        public bool IsApproved { get; set; }
        public string Flag { get; set; }



    }
}
