using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.BaseModels.For_DomainModel
{
    /// <summary>
    /// Created By, Updated By, Approved By, Cancelled By
    /// </summary>
    public class BaseModel3 : BaseModel2
    {
        [StringLength(100)]
        public virtual string CancelledBy { get; set; }
        public virtual Nullable<DateTime> CancelledDate { get; set; }
        [StringLength(200)]
        public virtual string CancelRemarks { get; set; }
        [StringLength(100)]
        public virtual string RejectedBy { get; set; }
        public virtual Nullable<DateTime> RejectedDate { get; set; }
        [StringLength(200)]
        public virtual string RejectedRemarks { get; set; }
    }
}
