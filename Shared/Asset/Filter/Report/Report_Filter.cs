using Shared.OtherModels.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Asset.Filter.Report
{
    public class Report_Filter
    {
        public string EmployeeId { get; set; }
        public string SelectedEmployees { get; set; }
        public string ProductId { get; set; }
        public string AssetId { get; set; }
        public string VendorId { get; set; }
        public string StoreId { get; set; }
        public string CategoryId { get; set; }
        public string SubCategoryId { get; set; }
        public string BrandId { get; set; }
        public string ReportName { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Status { get; set; }

    }
}
