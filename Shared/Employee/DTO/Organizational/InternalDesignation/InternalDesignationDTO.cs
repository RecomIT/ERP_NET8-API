using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Employee.DTO.Organizational.InternalDesignation
{
    public class InternalDesignationDTO
    {
        public long InternalDesignationId { get; set; }
        [StringLength(150)]
        public string InternalDesignationName { get; set; }
        public bool? IsActive { get; set; }
        [StringLength(150)]
        public string Remarks { get; set; }
    }
}
