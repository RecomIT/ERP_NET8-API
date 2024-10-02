using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Control_Panel.ViewModels
{
    public class DivisionViewModel : BaseModel
    {
        public long DivisionId { get; set; }
        [Required, StringLength(100)]
        public string DivisionName { get; set; }
        [Required, StringLength(5)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string DIVCode { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string CompanyName { get; set; }
        [StringLength(100)]
        public string OrganizationName { get; set; }
    }
}
