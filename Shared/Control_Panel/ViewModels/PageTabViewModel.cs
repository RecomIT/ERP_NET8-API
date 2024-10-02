using Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace Shared.Control_Panel.ViewModels
{
    public class PageTabViewModel : BaseModel
    {
        public long TabId { get; set; }
        [Required, StringLength(100)]
        public string TabName { get; set; }
        [StringLength(30)]
        public string IconClass { get; set; }
        [StringLength(20)]
        public string IconColor { get; set; }
        [Range(1, long.MaxValue)]
        public long? SubmenuId { get; set; }
        [StringLength(100)]
        public string SubmenuName { get; set; }
        public long MMId { get; set; }
        [StringLength(100)]
        public string MenuName { get; set; }
        public bool IsActive { get; set; }
        public long ComId { get; set; }
        public long BranchId { get; set; }
    }
}
