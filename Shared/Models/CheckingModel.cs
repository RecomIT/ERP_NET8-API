using System.ComponentModel.DataAnnotations;

namespace Shared.Models
{
    public class CheckingModel
    {
        [Range(1,long.MaxValue)]
        public long Id { get; set; }
        [StringLength(50)]
        public string Status { get; set; }
        [StringLength(200)]
        public string Remarks { get; set; }
    }
}
