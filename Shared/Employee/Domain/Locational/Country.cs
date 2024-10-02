using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Locational
{
    [Table("HR_Countries"), Index("CountryName", "ISOCode", "Nationality", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Countries_NonClusteredIndex")]
    public class Country : BaseModel
    {
        [Key]
        public int CountryId { get; set; }
        [Required, StringLength(100)]
        public string CountryName { get; set; }
        [StringLength(100)]
        public string CountryNameInBengali { get; set; }
        [StringLength(20)]
        public string CountryCode { get; set; }
        [StringLength(20)]
        public string ISOCode { get; set; }
        [StringLength(100)]
        public string Nationality { get; set; }
        [StringLength(100)]
        public string NationalityInBengali { get; set; }
    }
}
