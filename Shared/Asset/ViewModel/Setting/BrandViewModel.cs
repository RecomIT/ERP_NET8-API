using System.ComponentModel.DataAnnotations;

namespace Shared.Asset.ViewModel.Setting
{
    public class BrandViewModel
    {
        public int BrandId { get; set; }

        public int SubCategoryId { get; set; }

        [Required, StringLength(100)]
        public string BrandName { get; set; }

        [Required, StringLength(100)]
        public string SubCategoryName { get; set; }

        public string BrandInBengali { get; set; }

        public string BrandRemarks { get; set; }

        public bool BrandIsActive { get; set; }
    }
}
