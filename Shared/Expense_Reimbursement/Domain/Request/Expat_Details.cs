using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Expense_Reimbursement.Domain.Request
{
    [Table("Reimburse_Expat_Details")]
    [Index("ExpatDetailId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Reimburse_Expat_Details_NonClusteredIndex")]

    public class Expat_Details : BaseModel
    {
        [Key]
        public long ExpatDetailId { get; set; }
        public long ExpatId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(250)]
        public string CompanyName { get; set; }
        [StringLength(300)]
        public string Particular { get; set; }
        [StringLength(300)]
        public string BillType { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Costs { get; set; }
        public bool IsApproved { get; set; }

    }
}
