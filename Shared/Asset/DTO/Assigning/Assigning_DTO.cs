using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Shared.Asset.DTO.Assigning
{
    public class Assigning_DTO
    {
        public long AssigningId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }
        public long AssetId { get; set; }
        public string ProductId { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public bool Approved { get; set; }
        public bool Assigned { get; set; }

    }
}
