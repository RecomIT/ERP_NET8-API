using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Payroll.Domain.Payment
{
    [Table("Payroll_ConditionalProjectedPayment"), Index("FiscalYearId", "AllowanceNameId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_Payroll_ConditionalProjectedPayment_NonClusteredIndex")]
    public class ConditionalProjectedPayment : BaseModel2
    {
        [Key]
        public long Id { get; set; }
        [StringLength(100)]
        public string Code { get; set; }
        public long? FiscalYearId { get; set; }
        public short? PaymentMonth { get; set; }
        public short? PaymentYear { get; set; }
        public short? PayableMonth { get; set; }
        public short? PayableYear { get; set; }
        public long? AllowanceHeadId { get; set; }
        public long AllowanceNameId { get; set; }
        [StringLength(50)]
        public string JobType { get; set; } // N/A / Permanent / Contrutual / Trainee / Probotion
        [StringLength(50)]
        public string Religion { get; set; } // N/A / Islam /Christian
        [StringLength(50)]
        public string MaritalStatus { get; set; } // N/A / Married /Single
        [StringLength(50)]
        public string Citizen { get; set; } // N/A / YES /NO
        [StringLength(50)]
        public string Gender { get; set; } // N/A /Male/Female
        [StringLength(50)]
        public string PhysicalCondition { get; set; } // None/Disabled/Undisabled
        public bool? IsConfirmationRequired { get; set; }
        [StringLength(50)]
        public string BaseOfPayment { get; set; } // Flat(5000/=) / Basic (50%) / Gross
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Percentage { get; set; } // Basic/Gross = 100%
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; } // 5000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PayableAmount { get; set; } // Basic 50% 10000/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? DisbursedAmount { get; set; } // 8500/=
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TaxAmount { get; set; } // 8500/=
        [StringLength(200)]
        public string  Reason { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; } // Pending / Approved / Processed / Disbursed
        public bool IsApproved { get; set; }
        public bool? IsDisbursed { get; set; }
        public bool? ShowInPayslip { get; set; }
        public bool? ShowInSalarySheet { get; set; }
        public bool? HasEmployeeTypes { get; set; }
        public bool? HasEmployee { get; set; } // Employee
        public bool? HasExcludeEmployee { get; set; } // Excluded Employees
        public bool? HasBranch { get; set; } // Branch
        public bool? HasDivision { get; set; } // Division
        public bool? HasGrade { get; set; } // Grade
        public bool? HasInternalDesignation { get; set; } // Internal Designation
        public bool? HasDesignation { get; set; } // Designation
        public bool? HasDepartment { get; set; } // Department
        public bool? HasSection { get; set; } // Section 
        public bool? HasSubSection { get; set; } // Subsection
        public bool? HasUnit { get; set; } // Unit
        public ICollection<ConditionalProjectedPaymentParameter> ConditionalProjectedPaymentParameters { get; set; }
        public ICollection<ConditionalProjectedPaymentDetail> ConditionalProjectedPaymentDetails { get; set; }

    }
}
