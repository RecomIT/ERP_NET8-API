using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Control_Panel.ViewModels
{
    public class MainMenuViewModel : BaseModel
    {
        public long MMId { get; set; }
        [StringLength(100)]
        public string MenuName { get; set; }
        [StringLength(50)]
        public string ShortName { get; set; }
        [StringLength(100)]
        public string IconClass { get; set; }
        [StringLength(100)]
        public string IconColor { get; set; }
        public long MId { get; set; }
        public string ModuleName { get; set; }
        public long ApplicationId { get; set; }
        public string ApplicationName { get; set; }
        public bool IsActive { get; set; }
        public int? SequenceNo { get; set; }
        public long ComId { get; set; }
        public long BranchId { get; set; }
    }
}
