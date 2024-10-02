using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.DTO.Organizational
{
    public class SubSectionDTO
    {
        public int SubSectionId { get; set; }
        [Required, StringLength(100)]
        public string SubSectionName { get; set; }
        [StringLength(100)]
        public string SubSectionNameInBengali { get; set; }
        public bool IsActive { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [Range(1, long.MaxValue)]
        public long SectionId { get; set; }
    }
}
