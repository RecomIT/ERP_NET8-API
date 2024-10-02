using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Expense_Reimbursement.ViewModel.Request
{
    public class TrainingViewModel
    {
        public long TrainingId { get; set; }
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
        [StringLength(350)]
        public string Description { get; set; }
        [Column(TypeName = "date")]
        public string admissionDate { get; set; }
        [StringLength(350)]
        public string duration { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TrainingCosts { get; set; }
        [StringLength(250)]
        public string Purpose { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        public string Flag { get; set; }

    }
}
