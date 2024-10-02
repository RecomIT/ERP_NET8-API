using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Asset.ViewModel.Dashboard
{
    public class EmployeeViewModel
    {
        [Column(TypeName ="date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public string Brand { get; set; }
        public string AssetName { get; set; }
        public string ProductId { get; set; }
        public string Number { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> WarrantyDate { get; set; }
        public string Condition { get; set; }        
  

    }
}
