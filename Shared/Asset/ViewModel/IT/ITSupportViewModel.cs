using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Asset.ViewModel.IT
{
    public class ITSupportViewModel
    {
        public long AssigningId { get; set; }
        public long AssetId { get; set; }
        public long? EmployeeId { get; set; }
        public string ProductId { get; set; }
        [Column(TypeName ="date")]
        public Nullable<DateTime> TransactionDate { get; set; }     
        public string AssetName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }      
        [Column(TypeName = "date")]
        public Nullable<DateTime> WarrantyDate { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DesignationName { get; set; }
        public string GradeName { get; set; }
        public string DepartmentName { get; set; }
        public string Number { get; set; }
        public string LANMAC { get; set; }
        public string LANIP { get; set; }
        public string WIFIMAC { get; set; }
        public string WIFIIP { get; set; }

    }
}
