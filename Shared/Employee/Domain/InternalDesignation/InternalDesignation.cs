using Shared.BaseModels.For_DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Employee.Domain.InternalDesignation
{
    [Table("HR_InternalDesignations")]
    public class InternalDesignation : BaseModel2
    {
        [Key]
        public long InternalDesignationId { get; set; }
        [StringLength(150)]
        public string InternalDesignationName { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
    }

}
