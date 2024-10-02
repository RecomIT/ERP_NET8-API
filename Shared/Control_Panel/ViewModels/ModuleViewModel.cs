using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Control_Panel.ViewModels
{
    public class ModuleViewModel : BaseModel
    {
        public long ModuleId { get; set; }
        [Required, StringLength(100)]
        public string ModuleName { get; set; }
        public bool IsActive { get; set; }
        [Required, Range(1, long.MaxValue)]
        public long ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public long ComId { get; set; }
        public long BranchId { get; set; }
    }
}
