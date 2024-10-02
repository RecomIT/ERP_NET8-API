using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Separation.Models.DTO.Resignation
{
    public class EmployeeResignationDTO
    {
        public long ResignationId { get; set; }
        [StringLength(50)]
        public string ResignCode { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        public long? GradeId { get; set; }
        public long? DesignationId { get; set; }
        public long? DepartmentId { get; set; }
        [Required, StringLength(100)]
        public string ResignationReason { get; set; }
        [StringLength(100)]
        public string SecondaryReason { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? NoticeDate { get; set; }
        [Required, Column(TypeName = "date")]
        public DateTime? RequestLastWorkingDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? AcceptedLastWorkingDate { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ActualLastWorkingDate { get; set; }
        [StringLength(200)]
        public string EmployeeComment { get; set; }
        [Column(TypeName = "date")]
        public DateTime? ExitInterviewDate { get; set; }
        public bool? IsInterviewDone { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsPullBack { get; set; }
        [StringLength(50)]
        public string StateStatus { get; set; }
    }
}
