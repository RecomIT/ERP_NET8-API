using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Expense_Reimbursement.Domain.Request
{
    [Table("Reimburse_Expat")]
    [Index("ExpatId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Reimburse_Expat_NonClusteredIndex")]

    public class Expat : BaseModel3
    {
        [Key]
        public long ExpatId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        [StringLength(500)]
        public string Expats { get; set; }

        [StringLength(500)]
        public string CommentsUser { get; set; }
        [StringLength(500)]
        public string CommentsAccount { get; set; }

        [StringLength(100)]
        public string SpendMode { get; set; }   
        [StringLength(500)]
        public string Description { get; set; } 
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string AccountStatus { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdvanceAmount { get; set; }
        public bool IsApproved { get; set; }

    }
}
