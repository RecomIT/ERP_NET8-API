
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Shared.Asset.DTO.Support
{
    public class Servicing_DTO
    {
        public long ServicingId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long VendorId { get; set; }
        public long AssetId { get; set; }
        public string ProductId { get; set; }
        public string Number { get; set; }
        public string Token { get; set; }
        public string Remarks { get; set; }
    }
}
