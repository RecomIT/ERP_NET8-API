using Shared.OtherModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Asset.Filter.Create
{
    public class Create_Filter : Sortparam
    {
        public long AssetId { get; set; }
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? VendorId { get; set; }
        public long? StoreId { get; set; }
        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }
        public long? BrandId { get; set; }
        public string Status { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }

    }
}
