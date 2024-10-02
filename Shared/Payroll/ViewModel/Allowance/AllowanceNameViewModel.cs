using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Payroll.ViewModel.Allowance
{
    public class AllowanceNameViewModel : BaseViewModel1
    {
        public long AllowanceNameId { get; set; }
        [Required, StringLength(200)]
        public string Name { get; set; }
        [StringLength(20)]
        public string GLCode { get; set; }
        [StringLength(200)]
        public string AllowanceNameInBengali { get; set; }
        [StringLength(200)]
        public string AllowanceClientName { get; set; }
        [StringLength(200)]
        public string AllowanceClientNameInBengali { get; set; }
        [StringLength(300)]
        public string AllowanceDescription { get; set; }
        [StringLength(300)]
        public string AllowanceDescriptionInBengali { get; set; }
        [StringLength(50)]
        public string AllowanceType { get; set; } // General / Salary
        /// <summary>
        // If IsVariableSalary is true then IsFixed is null 
        // If IsVariableSalary is false then IsFixed is true/false
        /// </summary>
        public bool? IsFixed { get; set; }
        public bool IsActive { get; set; }
        [Range(1, long.MaxValue)]
        public long AllowanceHeadId { get; set; }
        [StringLength(200)]
        public string AllowanceHeadName { get; set; }
        public string Flag { get; set; }
    }
}
