using System;
using System.ComponentModel.DataAnnotations;
namespace Shared.Models
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

    public class BaseModel1 : BaseModel
    {
        public virtual long? BranchId { get; set; }
    }

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
    /// <summary>
    /// Organization ,Created By, Updated By
    /// </summary>
    public class BaseModel6
    {
        public virtual long OrganizationId { get; set; }
        [StringLength(100)]
        public virtual string CreatedBy { get; set; }
        public virtual Nullable<DateTime> CreatedDate { get; set; }
        [StringLength(100)]
        public virtual string UpdatedBy { get; set; }
        public virtual Nullable<DateTime> UpdatedDate { get; set; }
    }
}
