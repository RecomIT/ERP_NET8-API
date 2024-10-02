using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Bonus
{
    public class EligibleEmployeesForBonus
    {
        public int SL { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }
        public long GradeId { get; set; }
        public string GradeName { get; set; }
        public long DesignationId { get; set; }
        public string DesignationName { get; set; }
        public long DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfJoining { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> DateOfConfirmation { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TerminationDate { get; set; }
        public string TerminationStatus { get; set; }
        public string ReligionName { get; set; }
        public bool IsDiscontinued { get; set; }
        public string Gender { get; set; }
    }
}
