
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Setup
{
    [Table("HR_TableConfig")]
    public class TableConfig : BaseModel
    {
        public long Id { get; set; }
        [StringLength(150)]
        public string Table { get; set; }
        [StringLength(100)]
        public string Column { get; set; }
        public string Purpose { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsDisabled { get; set; } = false;
        public bool IsMandatory { get; set; } = false;
        public bool IsConstant { get; set; } = false;
        public bool IsUnique { get; set; } = false;
        public bool IsNewEntry { get; set; } = false;
        public long ParentId { get; set; } = 0;
        public long Serial { get; set; } = 0;
        [StringLength(50)]
        public string Min { get; set; }  // When data type is number
        [StringLength(50)]
        public string Max { get; set; } // When data type is number
        [StringLength(100)]
        public string DataType { get; set; }
        [StringLength(100)]
        public string DefaultValue { get; set; }
        [StringLength(50)]
        public string MaxLength { get; set; } // When data type is string
        [StringLength(50)]
        public string MinLength { get; set; } // When data type is string
        [StringLength(150)]
        public string Label { get; set; }
        [StringLength(200)]
        public string HelpText { get; set; }
        public string Group { get; set; }
    }
}
