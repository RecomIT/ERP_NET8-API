using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.External_Tables
{
    [Table("tblUsers"), Keyless]
    public class tblUsers
    {
        [StringLength(100)]
        public string Id { get; set; }
        [StringLength(100)]
        public string EmployeeId { get; set; }
        [StringLength(100)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [StringLength(256)]
        public string UserName { get; set; }
        [StringLength(256)]
        public string Email { get; set; }
        [StringLength(100)]
        public string RoleId { get; set; }
        public long? BranchId { get; set; }
        public long? DivisionId { get; set; }
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
