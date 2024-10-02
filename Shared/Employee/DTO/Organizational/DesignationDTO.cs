using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Organizational
{
    public class DesignationDTO
    {
        public int DesignationId { get; set; }
        [StringLength(100)]
        public string DesignationName { get; set; }
        [StringLength(20)]
        public string ShortName { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        [Range(1, long.MaxValue)]
        public int GradeId { get; set; }
    }
}
