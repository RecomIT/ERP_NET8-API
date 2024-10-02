using Shared.Models;
using System.ComponentModel.DataAnnotations;


namespace Shared.Payroll.ViewModel.Configuration
{
    public class SalaryAllowanceConfigurationInfoViewModel : BaseModel2
    {
        public long SalaryAllowanceConfigId { get; set; }
        [StringLength(100)]
        public string ConfigCategory { get; set; }
        public bool IsActive { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
        public int HeadCount { get; set; }
        public long?[] SelectedItems { get; set; }
        public string HeadDetails { get; set; }
        public string JobType { get; set; }
        public string BaseType { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public bool? IsOnProcess { get; set; } = false;
        public List<SalaryAllowanceConfigurationDetailViewModel> SalaryAllowanceConfigurationDetails { get; set; }
    }
}
