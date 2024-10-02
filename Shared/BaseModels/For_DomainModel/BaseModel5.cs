using System;
using System.ComponentModel.DataAnnotations;

namespace Shared.BaseModels.For_DomainModel
{
    /// <summary>
    /// Branch, Company, Organization , Created By, Updated By, Approved By, Cancelled By, Checked By, Accepted By
    /// </summary>
    public class BaseModel5 : BaseModel4
    {
        [StringLength(100)]
        public virtual string AcceptedBy { get; set; }
        public virtual Nullable<DateTime> AcceptedDate { get; set; }
        [StringLength(200)]
        public virtual string AcceptorRemarks { get; set; }
    }
}
