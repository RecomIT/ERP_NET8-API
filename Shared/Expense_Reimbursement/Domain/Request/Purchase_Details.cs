using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Expense_Reimbursement.Domain.Request
{
    [Table("Reimburse_Purchase_Details")]
    [Index("PurchaseDetailId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Reimburse_Purchase_Details_NonClusteredIndex")]

    public class Purchase_Details : BaseModel
    {
        [Key]
        public long PurchaseDetailId { get; set; }
        public long PurchaseId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(250)]
        public string Item { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public bool IsApproved { get; set; }

    }
}
