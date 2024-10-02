using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Asset.ViewModel.Assigning
{
    public class AssigningViewModel
    {
        public long AssigningId { get; set; }
        public long AssetId { get; set; }
        public long? EmployeeId { get; set; }
        public string ProductId { get; set; }
        [Column(TypeName ="date")]
        public Nullable<DateTime> TransactionDate { get; set; }     
        public string AssetName { get; set; }
        public string Vendor { get; set; }
        public string Store { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        public bool Depreciation { get; set; }        
        [Column(TypeName = "date")]
        public Nullable<DateTime> WarrantyDate { get; set; }
        public long? DurationDays { get; set; }
        public bool Condition { get; set; }      
        public bool Approved { get; set; }
        public bool ITAccess { get; set; }        
        public string Remarks { get; set; }
        public string Status { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string DepartmentName { get; set; }
        public bool Assigned { get; set; }
    }
}
