using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Expense_Reimbursement.Domain.Request
{
    [Table("Reimburse_Conveyance_Details")]
    [Index("ConveyanceDetailId", "CompanyId", "OrganizationId", IsUnique = false,Name = "IX_Reimburse_Conveyance_Details_NonClusteredIndex")]

    public class Conveyance_Details : BaseModel
    {
        [Key]
        public long ConveyanceDetailId { get; set; }
        public long ConveyanceId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(250)]
        public string Destination { get; set; }
        [StringLength(250)]
        public string Mode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }
        public bool IsApproved { get; set; }

    }
}
