using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Expense_Reimbursement.Domain.Request
{
    [Table("Reimburse_Travel")]
    [Index("TravelId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Reimburse_Travel_NonClusteredIndex")]

    public class Travel : BaseModel3
    {
        [Key]
        public long TravelId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> FromDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ToDate { get; set; }  
        [StringLength(100)]
        public string SpendMode { get; set; }
        [StringLength(250)]
        public string Location { get; set; }
        [StringLength(500)]
        public string Purpose { get; set; }
        [StringLength(500)]
        public string Transportation { get; set; }      
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TransportationCosts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AccommodationCosts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SubsistenceCosts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherCosts { get; set; }

        [StringLength(500)]
        public string CommentsUser { get; set; }
        [StringLength(500)]
        public string CommentsAccount { get; set; }
        [StringLength(500)]
        public string Description { get; set; } 
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string AccountStatus { get; set; }
        public bool IsApproved { get; set; }

    }
}
