using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.DTO.Account
{
    public class EmployeeAccountInfoDTO
    {
        public long AccountInfoId { get; set; }
        public long EmployeeId { get; set; }
        public int? BankId { get; set; }
        public int? BankBranchId { get; set; }
        [StringLength(50)]
        public string AgentName { get; set; } // When payment mode is mobile banking [Bkash,Nogad,Upay,UCash]
        public short Year { get; set; }
        public short Month { get; set; }
        [StringLength(30)]
        public string PaymentMode { get; set; } // Cash, Bank, Mobile Banking
        [StringLength(50)]
        public string AccountNo { get; set; } // Bank, Account No, Mobile No
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        [StringLength(300)]
        public string ActivationReason { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [Column(TypeName = "date")]
        public DateTime? EffectiveFrom { get; set; }
        [Column(TypeName = "date")]
        public DateTime? DeactivationFrom { get; set; }
    }
}
