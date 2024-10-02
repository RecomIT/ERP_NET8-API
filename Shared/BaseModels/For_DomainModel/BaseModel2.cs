using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.BaseModels.For_DomainModel
{
    /// <summary>
    /// Created By, Updated By, Approved By
    /// </summary>
    public class BaseModel2 : BaseModel1
    {
        [StringLength(100)]
        public virtual string ApprovedBy { get; set; }
        public virtual Nullable<DateTime> ApprovedDate { get; set; }
        [StringLength(200)]
        public virtual string ApprovalRemarks { get; set; }
    }
}
