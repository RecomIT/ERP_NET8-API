using System.ComponentModel.DataAnnotations;

namespace Shared.Asset.DTO.Setting
{
    public class SubCategory_DTO
    {
        public int CategoryId { get; set; }

        public int SubCategoryId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string NameInBengali { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    
    }
}
