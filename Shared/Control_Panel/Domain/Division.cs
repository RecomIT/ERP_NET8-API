using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblDivisions")]
    public class Division
    {
        [Key]
        public long DivisionId { get; set; }
        [Required, StringLength(100)]
        public string DivisionName { get; set; }
        [Required, StringLength(5)]
        public string ShortName { get; set; }
        [StringLength(30)]
        public string DIVCode { get; set; }
        public bool IsActive { get; set; }
        [ForeignKey("CompanyId")]
        public long CompanyId { get; set; }
        public Company Company { get; set; }
        public long OrganizationId { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
