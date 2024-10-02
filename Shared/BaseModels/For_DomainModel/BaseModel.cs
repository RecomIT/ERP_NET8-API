using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.BaseModels.For_DomainModel
{
    /// <summary>
    /// Created By, Updated By
    /// </summary>
    public class BaseModel
    {
        [StringLength(100)]
        public virtual string CreatedBy { get; set; }
        public virtual Nullable<DateTime> CreatedDate { get; set; }
        [StringLength(100)]
        public virtual string UpdatedBy { get; set; }
        public virtual Nullable<DateTime> UpdatedDate { get; set; }
        public virtual long OrganizationId { get; set; }
        public virtual long CompanyId { get; set; }
    }
}
