
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace Shared.Asset.DTO.Support
{
    public class Handover_DTO
    {
        public long HandoverId { get; set; }        
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }
        public long AssetId { get; set; }     
        public string ProductId { get; set; }      
        public string HandoverStatus { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
