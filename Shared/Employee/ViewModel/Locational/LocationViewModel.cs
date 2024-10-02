using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Locational
{
    public class LocationViewModel : BaseViewModel1
    {
        public int LocationId { get; set; }
        [Required, StringLength(100)]
        public string LocationName { get; set; }
        [StringLength(100)]
        public string LocationNameInBengali { get; set; }
        public int PoliceStationId { get; set; }
        [StringLength(100)]
        public string PoliceStationName { get; set; }
        public int DistrictId { get; set; }
        public string DistrictName { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}
