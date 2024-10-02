using System.ComponentModel.DataAnnotations;

namespace Shared.Asset.DTO.Setting
{
    public class Store_DTO
    {
        public int StoreId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [StringLength(100)]
        public string NameInBengali { get; set; }

        [StringLength(100)]
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
    
    }
}
