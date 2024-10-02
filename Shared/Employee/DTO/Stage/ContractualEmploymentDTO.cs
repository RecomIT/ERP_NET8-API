using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.DTO.Stage
{
    public class ContractualEmploymentDTO
    {
        public long ContractId { get; set; }
        public long LastContractId { get; set; }
        [StringLength(30)]
        public string ContractCode { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastContractEndDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ContractStartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ContractEndDate { get; set; }
        [StringLength(50)]
        public string Flag { get; set; }
        [StringLength(150)]
        public string ServiceTenure { get; set; }
        public int? ServiceDayLength { get; set; }
        public int? ServiceMonthLength { get; set; }
        public int? ServiceYearLength { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
