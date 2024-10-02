using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Asset.Filter.Create
{
    public class Product_Filter
    {
        public string Format { get; set; }
        public long AssetId { get; set; }
        public bool Approved { get; set; }
        public long AssigningId { get; set; }
        public string Type { get; set; }

    }
}
