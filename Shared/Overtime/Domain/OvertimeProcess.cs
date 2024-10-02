using System;
using System.Collections.Generic;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Overtime.Domain
{
    [Table("Payroll_OvertimeProcess")]
    public class OvertimeProcess : BaseModel1
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime SalaryMonth { get; set; }
        public DateTime ProcessDate { get; set; }
        public bool IsDisbursed { get; set; } = false;
        public List<OvertimeProcessDetails> OvertimeProcessDetails { get; set; } = new();

    }

    [Table("Payroll_OvertimeProcessDetails")]
    public class OvertimeProcessDetails : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        public long OvertimeProcessId { get; set; }
        public DateTime SalaryMonth { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalArrearAmount { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal NetPay { get; set; } = 0;
        public OvertimeProcess OvertimeProcess { get; set; }

    }

    [Table("Payroll_OvertimeAllowances")]
    public class OvertimeAllowances : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        public long OvertimeProcessId { get; set; }
        public long EmployeeId { get; set; }
        public long OvertimeId { get; set; }
        public string OvertimeName { get; set; }
        public DateTime SalaryMonth { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrearAmount { get; set; } = 0;
        public string Remarks { get; set; } = "-";
        public OvertimeProcess OvertimeProcess { get; set; }

    }

    [Table("Payroll_UploadOvertimeAllowances")]
    public class UploadOvertimeAllowances : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long OvertimeId { get; set; }
        public string OvertimeName { get; set; }
        public bool IsUnitUpload { get; set; } = true;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Unit { get; set; } = 0;
        public DateTime SalaryMonth { get; set; }
        public bool IsAmountUpload { get; set; } = false;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; } = 0;
        [Column(TypeName = "decimal(18,2)")]
        public decimal ArrearAmount { get; set; } = 0;
        public string Remarks { get; set; } = "-";

    }
}
