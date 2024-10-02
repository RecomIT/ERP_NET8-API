using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Tax
{
    public class EmployeeRamainFestivalBonusViewModal
    {
        public int? RowId { get; set; }
        public long? BonusConfigId { get; set; }
        [StringLength(50)]
        public string BasedOn { get; set; }
        public short? BonusCount { get; set; }
        public decimal? BonusAmount { get; set; }
        public decimal? BonusPercentage { get; set; }
        public decimal? MaximumAmount { get; set; }
        public bool? IsConfirmedRequired { get; set; }
    }
}
