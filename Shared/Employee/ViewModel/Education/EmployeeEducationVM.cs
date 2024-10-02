using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;


namespace Shared.Employee.ViewModel.Education
{
    public class EmployeeEducationVM : BaseViewModel1
    {
        public long EmployeeEducationId { get; set; }
        [Range(1, long.MaxValue)]
        public long EmployeeId { get; set; }
        [StringLength(100)]
        public string EmployeeCode { get; set; }
        [Range(1, int.MaxValue)]
        public int LevelOfEducationId { get; set; }
        [Range(1, int.MaxValue)]
        public int DegreeId { get; set; }
        [Required, StringLength(50)]
        public string Major { get; set; }
        [Required, StringLength(200)]
        public string InstitutionName { get; set; }
        [Required, StringLength(50)]
        public string Result { get; set; }
        [StringLength(50)]
        public string ScaleDivisionClass { get; set; }
        [Required, StringLength(4)]
        public string YearOfPassing { get; set; }
        [StringLength(100)]
        public string Duration { get; set; }
        public string LevelOfEducationName { get; set; }
        public string DegreeName { get; set; }
    }
}
