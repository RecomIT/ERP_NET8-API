using System.ComponentModel.DataAnnotations;

namespace Shared.Asset.ViewModel.Setting
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        [Required, StringLength(100)]
        public string CategoryName { get; set; }
        [StringLength(100)]
        public string CategoryInBengali { get; set; }
        [StringLength(100)]
        public string CategoryRemarks { get; set; }
        public bool CategoryIsActive { get; set; }
    }
}
