using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class DistrictViewModel : BaseModel
    {
        public long DistrictId { get; set; }
        [Required, StringLength(100)]
        public string DistrictName { get; set; }
        [Required, StringLength(5)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string DISCode { get; set; }
        public bool IsActive { get; set; }
        public long DivisionId { get; set; }
        [StringLength(100)]
        public string DivisionName { get; set; }
        [StringLength(100)]
        public string CompanyName { get; set; }
        [StringLength(100)]
        public string OrganizationName { get; set; }
    }
}
