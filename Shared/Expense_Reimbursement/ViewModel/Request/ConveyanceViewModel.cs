using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Expense_Reimbursement.ViewModel.Request
{
    public class ConveyanceViewModel
    {
        public long RequestId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long EmployeeId { get; set; }
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        public string CompanyName { get; set; }
        public string Purpose { get; set; }
        public string Transportation { get; set; }
        public string TransportationType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TransportationCosts { get; set; }
        public string Description { get; set; }  
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public string Flag { get; set; }

    }
}
