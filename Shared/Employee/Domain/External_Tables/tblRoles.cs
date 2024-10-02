using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.External_Tables
{
    [Table("tblRoles"), Keyless]
    public class tblRoles
    {
        public string Id { get; set; }
        [StringLength(256)]
        public string Name { get; set; }
        public bool IsActive { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
    }
}
