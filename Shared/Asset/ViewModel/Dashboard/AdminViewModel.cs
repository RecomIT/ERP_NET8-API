using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Asset.ViewModel.Dashboard
{
    public class AdminViewModel
    {
        public long AssetId { get; set; }     
        public string AssetName { get; set; }
        public string Vendor { get; set; }
        public string Store { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        public decimal TotalQuantity { get; set; }
        public decimal AssigningQuantity { get; set; }
        public decimal HandoverQuantity { get; set; }
        public decimal ReplacementQuantity { get; set; }        
        public decimal ReceivedQuantity { get; set; }        
        public decimal ServicingQuantity { get; set; }
        public decimal RepairedQuantity { get; set; }
        public decimal Balance { get; set; } 

    }
}
