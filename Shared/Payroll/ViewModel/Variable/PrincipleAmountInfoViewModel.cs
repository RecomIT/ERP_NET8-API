using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.ViewModel.Variable
{
    public class PrincipleAmountInfoViewModel
    {
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? GradeId { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(50)]
        public string JobType { get; set; } = string.Empty;
        public short Month { get; set; }
        public short Year { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Garde { get; set; }
    }
}
