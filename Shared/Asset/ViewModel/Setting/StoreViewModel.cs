using System.ComponentModel.DataAnnotations;

namespace Shared.Asset.ViewModel.Setting
{
    public class StoreViewModel
    {
        public int StoreId { get; set; }
        [Required, StringLength(100)]
        public string StoreName { get; set; }
        [StringLength(100)]
        public string StoreInBengali { get; set; }
        [StringLength(100)]
        public string StoreRemarks { get; set; }
        public bool StoreIsActive { get; set; }
    }
}
