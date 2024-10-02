using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Separation.Models.Domain
{
    public class ResignationCategoryConfig : BaseModel1
    {
        [Key]
        public long ResignationConfigId { get; set; }
        public long? CategoryId { get; set; }
        public long? SubCategoryId { get; set; }
        public long NoticePeriod { get; set; }
        public bool? IsActive { get; set; }
    }
}
