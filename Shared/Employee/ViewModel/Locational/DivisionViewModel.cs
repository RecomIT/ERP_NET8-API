using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Locational
{
    public class DivisionViewModel : BaseViewModel1
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
