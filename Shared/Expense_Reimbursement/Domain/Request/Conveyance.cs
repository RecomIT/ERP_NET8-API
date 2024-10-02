using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Expense_Reimbursement.Domain.Request
{
    [Table("Reimburse_Conveyance")]
    [Index("ConveyanceId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Reimburse_Conveyance_NonClusteredIndex")]

    public class Conveyance : BaseModel3
    {
        [Key]
        public long ConveyanceId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        [StringLength(250)]
        public string CompanyName { get; set; }
        [StringLength(500)]
        public string Purpose { get; set; }
        [StringLength(50)]
        public string SpendMode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdvanceAmount { get; set; }
        [StringLength(500)]
        public string Transportation { get; set; }
        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(500)]
        public string CommentsUser { get; set; }
        [StringLength(500)]
        public string CommentsAccount { get; set; }

        [StringLength(100)]
        public string StateStatus { get; set; }

        [StringLength(100)]
        public string AccountStatus { get; set; }
        public bool IsApproved { get; set; }


    }
}
