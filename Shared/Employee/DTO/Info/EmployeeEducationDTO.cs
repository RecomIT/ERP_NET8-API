using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Info
{
    public class EmployeeEducationDTO
    {
        public long EmployeeEducationId { get; set; }
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        public int LevelOfEducationId { get; set; }
        public int DegreeId { get; set; }
        [StringLength(50)]
        public string Major { get; set; }
        [StringLength(200)]
        public string InstitutionName { get; set; }
        [StringLength(50)]
        public string Result { get; set; }
        [StringLength(50)]
        public string ScaleDivisionClass { get; set; }
        [StringLength(4)]
        public string YearOfPassing { get; set; }
        [StringLength(100)]
        public string Duration { get; set; }
    }
}
