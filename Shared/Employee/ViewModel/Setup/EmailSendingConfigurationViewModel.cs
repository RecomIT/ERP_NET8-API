using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Setup
{
    public class EmailSendingConfigurationViewModel : BaseViewModel2
    {
        public int Id { get; set; }
        [Required, StringLength(200)]
        public string ModuleName { get; set; }
        [Required, StringLength(200)]
        public string EmailStage { get; set; }
        public string EmailCC1 { get; set; }
        [StringLength(200)]
        public string EmailCC2 { get; set; }
        [StringLength(200)]
        public string EmailCC3 { get; set; }
        [StringLength(200)]
        public string EmailBCC1 { get; set; }
        [StringLength(200)]
        public string EmailBCC2 { get; set; }
        [StringLength(200)]
        public string EmailTo { get; set; }
        public bool IsActive { get; set; }
    }
}
