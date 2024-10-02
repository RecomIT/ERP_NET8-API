using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Variable
{
    [Table("Payroll_PrincipleAmountInfo")]
    public class PrincipleAmountInfo : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        public long? EmployeeId { get; set; }
        public long? DesignationId { get; set; }
        public long? GradeId { get; set; }
        public long? DepartmentId { get; set; }
        [StringLength(50)]
        public string JobType { get; set; }
        [StringLength(150)]
        public string IncomingFlag { get; set; } // Periodical Variable Allowance // Periodical Variable Deduction
        [StringLength(150)]
        public string VariableType { get; set; } // Value: Allowance/Deduction
        public short Month { get; set; }
        public short Year { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        public long IncomingId { get; set; } // Periodical Variable Allowance Primary Key// Periodical Variable Deduction Primary Key
        public long VariableId { get; set; } // Periodical Variable Allowance Id// Periodical Variable Deduction Id
        public long? ReplaceId { get; set; }
    }
}
