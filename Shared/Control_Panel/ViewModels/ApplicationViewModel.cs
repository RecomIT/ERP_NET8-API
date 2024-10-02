using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class ApplicationViewModel : BaseModel
    {
        public long ApplicationId { get; set; }
        [Required, StringLength(100)]
        public string ApplicationName { get; set; }
        [Required, StringLength(50)]
        public string ApplicationType { get; set; }
        public bool IsActive { get; set; }
        public long ComId { get; set; }
        public long BranchId { get; set; }
    }
}
