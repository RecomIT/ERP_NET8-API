using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Asset.ViewModel.Support
{
    public class HandoverViewModel
    {
        public long? HandoverId { get; set; }
        public long AssetId { get; set; }
        public long? EmployeeId { get; set; }
        public string ProductId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public string AssetName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string DepartmentName { get; set; }
        public string BranchName { get; set; }
        public string JobType { get; set; }
        public string Condition { get; set; }
        public string Remarks { get; set; }

    }
}
