using Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Configuration.FreeCar
{
    [Table("Payroll_EmployeeFreeCar"), Index("EmployeeId", "FiscalYearId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_EmployeeFreeCar_NonClusteredIndex")]
    public class EmployeeFreeCar : BaseModel1
    {
        [Key]
        public long FreeCarId { get; set; }
        public long EmployeeId { get; set; }
        public long FiscalYearId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsProjected { get; set; }
        [StringLength(50)]
        public string CarModelNo { get; set; }
        [StringLength(50)]
        public int CCOfCar { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> StartDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> EndDate { get; set; }
    }
}
