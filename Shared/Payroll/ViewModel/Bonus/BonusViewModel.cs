using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Bonus
{
    public class BonusViewModel : BaseViewModel2
    {
        public long BonusId { get; set; }
        [Required]
        public string BonusName { get; set; }
        [StringLength(50)]
        public string BonusState { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string ActivatedBy { get; set; }
        public DateTime? ActivationDate { get; set; }
        [StringLength(50)]
        public string DeactivatedBy { get; set; }
        public DateTime? DeactivationDate { get; set; }
        [StringLength(100)]
        public string Reason { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
