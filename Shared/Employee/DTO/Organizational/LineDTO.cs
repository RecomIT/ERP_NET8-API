using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Organizational
{
    public class LineDTO
    {
        public long LineId { get; set; }
        [Required, StringLength(100)]
        public string LineName { get; set; }
        [StringLength(100)]
        public string LineNameInBengali { get; set; }
        [StringLength(100)]
        public string ShortName { get; set; }
        [StringLength(10)]
        public string LineCode { get; set; }
    }
}
