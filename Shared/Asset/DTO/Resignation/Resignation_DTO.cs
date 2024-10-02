using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace Shared.Asset.DTO.Resignation
{
    public class Resignation_DTO
    {
        public long ResignationId { get; set; }
        public long AssetId { get; set; }
        public long AssigningId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public long? EmployeeId { get; set; }
        public string ProductId { get; set; }

        [StringLength(200)]
        public string Condition { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
        public string IsReturned { get; set; }

    }
}
