using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.ViewModel.Setup
{
    public class TableConfigViewModel
    {
        public long Id { get; set; }
        [StringLength(100)]
        public string Column { get; set; } = "";
        public string Purpose { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsDisabled { get; set; } = false;
        public bool IsMandatory { get; set; } = false;
        public bool IsConstant { get; set; } = false;
        public bool IsUnique { get; set; } = false;
        public bool IsNewEntry { get; set; } = false;
        public long ParentId { get; set; } = 0;
        public string Parent { get; set; } = "";
        public long Serial { get; set; } = 0;
        [StringLength(100)]
        public string DataType { get; set; }
        [StringLength(100)]
        public string DefaultValue { get; set; }
        [StringLength(20)]
        public string MaxLength { get; set; }
        [StringLength(150)]
        public string Label { get; set; }
        [StringLength(200)]
        public string HelpText { get; set; }
        public bool IsChecked { get; set; } = true;
        public string Group { get; set; } = "";
    }
}
