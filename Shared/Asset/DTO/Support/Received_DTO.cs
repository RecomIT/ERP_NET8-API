
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Shared.Asset.DTO.Support
{
    public class Received_DTO
    {
        public long ReceivedId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }
        public long AssetId { get; set; }
        public long AssigningId { get; set; }
        public string ProductId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
