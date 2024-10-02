using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Asset.ViewModel.Create
{
    public class ProductViewModel
    {
        public string Format { get; set; }
        public long AssetId { get; set; }
        public string ProductId { get; set; }
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
        public string Assigned { get; set; }
    }
}
