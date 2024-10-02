using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_DataLabel")]
    public class DataLabel : BaseModel1
    {
        [Key]
        public long Id { get; set; }
        [StringLength(200)]
        public string Label { get; set; } // Employee Info
        [StringLength(300)]
        public string DisplayName { get; set; } // Full Name
        [StringLength(200)]
        public string Alias { get; set; } // emp.EmployeeName
        public bool IsActive { get; set; }
        public int Serial { get; set; } = 0;
    }
}
