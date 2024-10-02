using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Separation.Models.Domain
{
    public class ResignationSubCategory : BaseModel1
    {
        [Key]
        public long SubCategoryId { get; set; }
        public long? CategoryId { get; set; }
        public string SubCategoryCode { get; set; }
        public string SubCategoryName { get; set; }
        public bool? IsActive { get; set; }

    }
}

