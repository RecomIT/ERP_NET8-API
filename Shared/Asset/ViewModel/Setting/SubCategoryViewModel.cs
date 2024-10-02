using System.ComponentModel.DataAnnotations;

namespace Shared.Asset.ViewModel.Setting
{
    public class SubCategoryViewModel
    {
        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        [Required, StringLength(100)]
        public string CategoryName { get; set; }

        [Required, StringLength(100)]
        public string SubCategoryName { get; set; }

        public string SubCategoryInBengali { get; set; }

        public string SubCategoryRemarks { get; set; }

        public bool SubCategoryIsActive { get; set; }
    }
}
