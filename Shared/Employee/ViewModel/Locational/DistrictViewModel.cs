using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Locational
{
    public class DistrictViewModel : BaseViewModel1
    {
        public int DistrictId { get; set; }
        [Required, StringLength(100)]
        public string DistrictName { get; set; }
        [StringLength(100)]
        public string DistrictNameInBengali { get; set; }
        [StringLength(100)]
        public string DistrictCode { get; set; }
        public int DivisionId { get; set; }
        [StringLength(100)]
        public string DivisionName { get; set; }
        public int CountryId { get; set; }
        [StringLength(100)]
        public string CountryName { get; set; }
    }
}
