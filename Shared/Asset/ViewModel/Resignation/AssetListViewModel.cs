using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Asset.ViewModel.Resignation
{
    public class AssetListViewModel
    {
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public string ProductId { get; set; }
        public string AssetName { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Brand { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> WarrantyDate { get; set; }    
        public string Number { get; set; }
        public string IMEI1 { get; set; }
        public string IMEI2 { get; set; }
        public string PIN { get; set; }
        public string PUK { get; set; }
        public string LANMAC { get; set; }
        public string LANIP { get; set; }
        public string WiFiMAC { get; set; }
        public string WiFiIP { get; set; }
        public bool Condition { get; set; }
        public long AssetId { get; set; }
        public long AssigningId { get; set; }
        public long? EmployeeId { get; set; }

    }
}
