using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Info
{
    [Table("HR_EmployeeRelativesInfo")]
    public class EmployeeRelativesInfo : BaseModel1
    {
        [Key]
        public long RelativeId { get; set; }
        [ForeignKey("EmployeeInformation")]
        public long EmployeeId { get; set; }
        public EmployeeInformation EmployeeInformation { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(50)]
        public string Relation { get; set; }
        [StringLength(100)]
        public string Address { get; set; }
        [StringLength(30)]
        public string MobileNumber { get; set; }
    }
}
