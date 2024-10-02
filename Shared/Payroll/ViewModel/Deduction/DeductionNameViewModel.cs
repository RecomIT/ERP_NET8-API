using System;
using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Deduction
{
    public class DeductionNameViewModel : BaseViewModel1
    {
        public long DeductionNameId { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(20)]
        public string GLCode { get; set; }
        [StringLength(200)]
        public string DeductionNameInBengali { get; set; }
        [StringLength(200)]
        public string DeductionClientName { get; set; }
        [StringLength(200)]
        public string DeductionClientNameInBengali { get; set; }
        [StringLength(300)]
        public string DeductionDescription { get; set; }
        [StringLength(300)]
        public string DeductionDescriptionInBengali { get; set; }
        [StringLength(50)]
        public string DeductionType { get; set; }
        public string Flag { get; set; }
        public bool? IsFixed { get; set; }
        public bool IsActive { get; set; }
        [Range(1, long.MaxValue)]
        public long DeductionHeadId { get; set; }
        [StringLength(200)]
        public string DeductionHeadName { get; set; }
    }
}
