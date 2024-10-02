
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Shared.Asset.DTO.Support
{
    public class Replacement_DTO
    {
        public long ReplacementId { get; set; }
        public long ReceivedId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }
        public long AssetId { get; set; }     
        public string ProductId { get; set; }
        public string PreviousProductId { get; set; }
        public long PreviousAssigningId { get; set; }
        [StringLength(200)]
        public string ReplaceStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
