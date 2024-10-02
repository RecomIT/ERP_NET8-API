using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Employee.ViewModel.Account
{
    public class EmployeeAccountInfoViewModel : BaseViewModel4
    {
        public long AccountInfoId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        public int? BankId { get; set; }
        public int? BankBranchId { get; set; }
        [StringLength(50)]
        public string AgentName { get; set; } // When payment mode is mobile banking
        public short Year { get; set; }
        public short Month { get; set; }
        [StringLength(30)]
        public string PaymentMode { get; set; } // Cash, Bank, Mobile Banking
        [StringLength(50)]
        public string AccountNo { get; set; } // Bank Account No, Mobile No
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(300)]
        public string ActivationReason { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? DeactivationFrom { get; set; }

        // 
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string GradeName { get; set; }
        public string BankName { get; set; }
        public string BankBranchName { get; set; }
    }
}
