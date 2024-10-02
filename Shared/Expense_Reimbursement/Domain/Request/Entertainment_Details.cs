using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Expense_Reimbursement.Domain.Request
{
    [Table("Reimburse_Entertainment_Details")]
    [Index("EntertainmentDetailId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Reimburse_Entertainment_Details_NonClusteredIndex")]

    public class Entertainment_Details : BaseModel
    {
        [Key]
        public long EntertainmentDetailId { get; set; }
        public long EntertainmentId { get; set; }
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
