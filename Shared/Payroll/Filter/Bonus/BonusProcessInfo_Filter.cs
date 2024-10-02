using Shared.OtherModels.Pagination;

namespace Shared.Payroll.Filter.Bonus
{
    public class BonusProcessInfo_Filter : Pageparam
    {
        public string BonusId { get; set; }
        public string BonusConfigId { get; set; }
        public string BatchNo { get; set; }
        public string FiscalYearId { get; set; }
    }
}
