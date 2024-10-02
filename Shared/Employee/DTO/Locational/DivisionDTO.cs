using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Locational
{
    public class DivisionDTO
    {
        public int DivisionId { get; set; }
        [Required, StringLength(100)]
        public string DivisionName { get; set; }
        [StringLength(100)]
        public string DivisionNameInBengali { get; set; }
        [StringLength(100)]
        public string DivisionCode { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
