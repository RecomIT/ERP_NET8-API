using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblPayrollModuleConfig")]
    public class PayrollModuleConfig
    {
        [Key]
        public long PayrollModuleConfigId { get; set; }
        public long ApplicationId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public long? BranchId { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        public string WhatDoesConsiderationForMonth { get; set; }
        public bool? IsProvidentFundactivated { get; set; }
        [StringLength(100)]
        public string BaseOfProvidentFund { get; set; }
        public string PercentageOfProvidentFund { get; set; }
        public bool CalculateTaxOnArrearAmount { get; set; }
        public string PercentageOfActualCalculatedTaxForMonthlyDeduction { get; set; }
        public bool? IsOnceOffTaxAvailable { get; set; }
        public string WhenDoesOnceOffTaxCutDown { get; set; }
        public bool? IsNonResidentTaxApplied { get; set; }
        public bool? IsFestivalBonusDisbursedbasedonReligion { get; set; }
        public string DiscontinuedEmployeesLastMonthPaymentProcess { get; set; }
        public string NoticePayBasedOn { get; set; }
        [StringLength(100)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitOfBonus { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
