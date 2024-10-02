using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Separation.Models.Domain
{
    public class ResignationCategory : BaseModel
    {
        [Key]
        public long CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public bool? IsActive { get; set; }
    }
}
