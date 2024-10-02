using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Expense_Reimbursement.ViewModel.Request
{
    public class TravelViewModel
    {
        public long RequestId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long EmployeeId { get; set; }
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> FromDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ToDate { get; set; }
        [StringLength(250)]
        public string SpendMode { get; set; }
        [StringLength(200)]
        public string Location { get; set; }
        [StringLength(250)]
        public string Purpose { get; set; }
        [StringLength(250)]
        public string Transportation { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TransportationCosts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AccommodationCosts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SubsistenceCosts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherCosts { get; set; }
        [StringLength(350)]
        public string Description { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public string Flag { get; set; }
        [StringLength(100)]
        public string AccountStatus { get; set; }

    }
}
