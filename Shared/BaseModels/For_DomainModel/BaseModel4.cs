using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.BaseModels.For_DomainModel
{
    /// <summary>
    /// Branch, Company, Organization , Created By, Updated By, Approved By, Cancelled By, Checked By
    /// </summary>
    public class BaseModel4 : BaseModel3
    {
        //public virtual long? BranchId { get; set; }
        [StringLength(100)]
        public virtual string CheckedBy { get; set; }
        public Nullable<DateTime> CheckedDate { get; set; }
        [StringLength(200)]
        public virtual string CheckRemarks { get; set; }
    }
}
