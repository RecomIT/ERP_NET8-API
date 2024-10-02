using System;
namespace Shared.Payroll.ViewModel.Bonus
{
    public class BonusTaxProcess
    {
        public long BonusId { get; set; }
        public long BonusConfigId { get; set; }
        public string BonusName { get; set; }
        public long EmployeeId { get; set; }
        public bool? IsTaxable { get; set; }
        public bool? IsPaymentProjected { get; set; }
        public bool? IsTaxDistributed { get; set; }
        public bool? IsOnceOff { get; set; }
        public bool? IsFestivalBonus { get; set; }
        public long SalaryReviewId { get; set; }
        public decimal? Gross { get; set; }
        public decimal? Basic { get; set; }
        public decimal? BonusAmount { get; set; }
        public DateTime? ProcessDate { get; set; }
    }
}
