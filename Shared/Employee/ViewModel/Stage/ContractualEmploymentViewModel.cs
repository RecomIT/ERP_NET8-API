using System;
using Shared.BaseModels.For_ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.ViewModel.Stage
{
    public class ContractualEmploymentViewModel : BaseViewModel3
    {
        public long ContractId { get; set; }
        [StringLength(30)]
        public string ContractCode { get; set; }
        public long? LastContractId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeName { get; set; }
        [StringLength(50)]
        public string Flag { get; set; } // Joining/Renewal
        [Column(TypeName = "date")]
        public DateTime? ContractStartDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ContractEndDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? LastContractEndDate { get; set; }
        [StringLength(150)]
        public string ServiceTenure { get; set; }
        public int? ServiceDayLength { get; set; }
        public int? ServiceMonthLength { get; set; }
        public int? ServiceYearLength { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsTerminated { get; set; }
        public bool? IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
