using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Asset.ViewModel.Support
{
    public class ReceivedViewModel
    {
        public long? ServicingId { get; set; }
        public long AssetId { get; set; }            
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public string AssetName { get; set; }
        public string Brand { get; set; }
        public string ProductId { get; set; }
        public string Number { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> WarrantyDate { get; set; }
        public string Remarks { get; set; }

    }
}
