using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Locational
{
    public class PoliceStationViewModel : BaseViewModel1
    {
        public int PoliceStationId { get; set; }
        [Required, StringLength(100)]
        public string PoliceStationName { get; set; }
        [StringLength(100)]
        public string PoliceStationNameInBengali { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [StringLength(100)]
        public string DistrictName { get; set; }
        public int DistrictId { get; set; }
        public int DivisionId { get; set; }
        [StringLength(100)]
        public string DivisionName { get; set; }
        public int CountryId { get; set; }
        [StringLength(100)]
        public string CountryName { get; set; }
    }
}
