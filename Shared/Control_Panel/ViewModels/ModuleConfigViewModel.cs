using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.ViewModels
{
    public class ModuleConfigViewModel : BaseViewModel1
    {
        public long ModuleConfigId { get; set; }
        public long ConfigId { get; set; }
        [StringLength(20)]
        public string ConfigCode { get; set; }
        [StringLength(300)]
        public string ConfigText { get; set; }
        [StringLength(100)]
        public string ConfigValue { get; set; }
        public long MainmenuId { get; set; }
        public long ModuleId { get; set; }
        public long ApplicationId { get; set; }
        public long? BranchId { get; set; }
        [StringLength(100)]
        public string MainmenuName { get; set; }
        [StringLength(100)]
        public string ModuleName { get; set; }
        [StringLength(100)]
        public string ApplicationName { get; set; }
        [StringLength(100)]
        public string BranchName { get; set; }
    }
    public class HRModuleConfigViewModel : BaseViewModel2
    {
        public long HRModuleConfigId { get; set; }
        public long ApplicationId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public bool? EnableMaxLateWarning { get; set; }
        public short? MaxLateInMonth { get; set; }
        public bool? EnableSequenceLateWarning { get; set; }
        public short? SequenceLateInMonth { get; set; }

        // Custom Properties
        public string ApplicationName { get; set; }
        public string ModuleName { get; set; }
        public string MainmenuName { get; set; }
    }
    public class PayrollModuleConfigViewModel : BaseViewModel2
    {
        public long PayrollModuleConfigId { get; set; }
        public long ApplicationId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public string WhatDoesConsiderationForMonth { get; set; }
        public bool? IsProvidentFundactivated { get; set; }
        public string PercentageOfBasicForProvidentFund { get; set; }
        public string PercentageOfActualCalculatedTaxForMonthlyDeduction { get; set; }
        public bool? IsOnceOffTaxAvailable { get; set; }
        public string WhenDoesOnceOffTaxCutDown { get; set; }
        public bool? IsNonResidentTaxApplied { get; set; }
        public bool? IsFestivalBonusDisbursedbasedonReligion { get; set; }
        public string DiscontinuedEmployeesLastMonthPaymentProcess { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitOfBonus { get; set; }

        // Custom Properties
        public string ApplicationName { get; set; }
        public string ModuleName { get; set; }
        public string MainmenuName { get; set; }
    }
    public class PFModuleConfigViewModel : BaseViewModel2
    {
        public long PFModuleConfigId { get; set; }
        public long ApplicationId { get; set; }
        public long ModuleId { get; set; }
        public long MainmenuId { get; set; }
        public bool? CalculateByJoiningDate { get; set; }
        public bool? CashFlow { get; set; }
        public bool? Subsidiary { get; set; }
        public bool? OnlyEmployeePartLoan { get; set; }
        public bool? IsIslamic { get; set; }
        public bool? MonthWiseIntrument { get; set; }
        public bool? PendingContribution { get; set; }
        public bool? GenerateAmortization { get; set; }
        public bool? LoanPaidandAmortization { get; set; }
        public bool? ReceivePaymentReport { get; set; }
        public bool? ContributionFromPayroll { get; set; }
        public bool? InstrumentAccruedProcess { get; set; }
        public bool? Forfeiture { get; set; }
        public bool? Monthlyprofit { get; set; }
        public bool? Chequeue { get; set; }
        // Custom Properties
        public string ApplicationName { get; set; }
        public string ModuleName { get; set; }
        public string MainmenuName { get; set; }
    }
}
