using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Salary
{
    public class EmployeeLastApprovedSalaryReviewInfo
    {
        public long SalaryReviewInfoId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        [StringLength(50)]
        public string FullName { get; set; }
        public long SalaryAllowanceConfigId { get; set; }
        public decimal CurrentSalaryAmount { get; set; }
        public decimal PreviousSalaryAmount { get; set; }
        public decimal SalaryBaseAmount { get; set; }
        public string BaseType { get; set; }
        public string IncrementReason { get; set; }
        public DateTime? ActivationDate { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? ArrearCalculatedDate { get; set; }
        public bool? IsArrearCalculated { get; set; }
        public bool? IsAutoCalculate { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string SectionName { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public List<EmployeeLastApprovedSalaryReviewDetail> EmployeeLastApprovedSalaryReviewDetails { get; set; }
    }
}
