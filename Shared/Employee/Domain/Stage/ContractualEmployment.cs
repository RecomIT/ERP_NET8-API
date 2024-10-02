using System;
using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Stage
{
    [Table("HR_ContractualEmployment"), Index("EmployeeId", "ContractCode", "IsActive", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_ContractualEmployment_NonClusteredIndex")]
    public class ContractualEmployment : BaseModel3
    {
        [Key]
        public long ContractId { get; set; }
        [StringLength(30)]
        public string ContractCode { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string Flag { get; set; } // Joining/Renewal
        [Column(TypeName = "date")]
        public DateTime? LastContractEndDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ContractStartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ContractEndDate { get; set; }
        [StringLength(150)]
        public string ServiceTenure { get; set; }
        public int? ServiceDayLength { get; set; }
        public int? ServiceMonthLength { get; set; }
        public int? ServiceYearLength { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTerminated { get; set; }
        public bool? IsApproved { get; set; }
        public long? LastContractId { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
