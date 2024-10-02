using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Asset.ViewModel.Create
{
    public class CreateViewModel
    {
        public long AssetId { get; set; }

        [Column(TypeName ="date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? VendorId { get; set; }
        public long? StoreId { get; set; }
        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }
        public long? BrandId { get; set; }        
        public string AssetName { get; set; }
        public string Vendor { get; set; }
        public string Store { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        public bool Depreciation { get; set; }
        public decimal DepreciableAmount { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> WarrantyDate { get; set; }
        public long? DurationDays { get; set; }
        public bool Condition { get; set; }        
        public decimal Quantity { get; set; }        
        public decimal Amount { get; set; }        
        public decimal TotalAmount { get; set; }
        public bool Approved { get; set; }
        public bool ITAccess { get; set; }        
        public string Remarks { get; set; }
        public string Status { get; set; }

    }
}
