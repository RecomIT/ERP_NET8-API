using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Locational
{
    public class CountryDTO
    {
        public int CountryId { get; set; }
        [Required, StringLength(100)]
        public string CountryName { get; set; }
        [StringLength(100)]
        public string CountryNameInBengali { get; set; }
        [StringLength(20)]
        public string CountryCode { get; set; }
        [Required, StringLength(100)]
        public string Nationality { get; set; }
        [StringLength(100)]
        public string NationalityInBengali { get; set; }
        [StringLength(20)]
        public string ISOCode { get; set; }
    }
}
