using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.External_Tables
{
    [Table("tblDivisions"), Keyless]
    public class tblDivisions
    {
        public long DivisionId { get; set; }
        [Required, StringLength(100)]
        public string DivisionName { get; set; }
        [Required, StringLength(5)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string DIVCode { get; set; }
        public bool IsActive { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
