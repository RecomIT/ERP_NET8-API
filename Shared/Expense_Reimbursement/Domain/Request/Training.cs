using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Expense_Reimbursement.Domain.Request
{
    [Table("Reimburse_Training")]
    [Index("TrainingId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Reimburse_Training_NonClusteredIndex")]

    public class Reimburse_Training : BaseModel3
    {
        [Key]
        public long TrainingId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string ReferenceNumber { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        [StringLength(250)]
        public string InstitutionName { get; set; }     
        [StringLength(500)]
        public string Course { get; set; }
        [StringLength(500)]        
        public string Description { get; set; }
        [Column(TypeName = "date")] 
        public Nullable<DateTime> AdmissionDate { get; set; }

        [StringLength(500)]
        public string CommentsUser { get; set; }
        [StringLength(500)]
        public string CommentsAccount { get; set; }
        [StringLength(100)]
        public string Duration { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TrainingCosts { get; set; }
        [StringLength(500)]
        public string Purpose { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        [StringLength(100)]
        public string AccountStatus { get; set; }
        public bool IsApproved { get; set; }

    }
}
