using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Shared.Asset.DTO.Create
{
    public class Create_DTO
    {
        public long AssetId { get; set; }

        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? VendorId { get; set; }
        public long? StoreId { get; set; }
        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }
        public long? BrandId { get; set; }
        [Required, StringLength(250)]
        public string AssetName { get; set; }       
        [Column(TypeName = "date")]
        public Nullable<DateTime> WarrantyDate { get; set; }       
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }
        public bool Depreciation { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal DepreciableAmount { get; set; }
        public bool Condition { get; set; }
        public bool Approved { get; set; }
        public bool ITAccess { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }

    }
}
