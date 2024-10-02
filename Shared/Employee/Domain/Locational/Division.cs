using Microsoft.EntityFrameworkCore;
using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Locational
{
    [Table("HR_Divisions"), Index("DivisionName", "DivisionCode", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_Divisions_NonClusteredIndex")]
    public class Division : BaseModel
    {
        [Key]
        public int DivisionId { get; set; }
        [Required, StringLength(100)]
        public string DivisionName { get; set; }
        [StringLength(100)]
        public string DivisionNameInBengali { get; set; }
        [StringLength(100)]
        public string DivisionCode { get; set; }
        [ForeignKey("CountryId")]
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
