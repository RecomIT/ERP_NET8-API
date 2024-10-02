using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    /// <summary>
    /// EntryUser, UpdateUser
    /// </summary>
    public class BaseViewModel1: BaseModel
    {
        public virtual string EntryUser { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string UserId { get; set; }
    }
    public class BaseViewModel2 : BaseModel1
    {
        public virtual string EntryUser { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string UserId { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string OrganizationName { get; set; }
    }
    public class BaseViewModel3 : BaseModel2
    {
        public virtual string EntryUser { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string UserId { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string OrganizationName { get; set; }
    } 
    public class BaseViewModel4 : BaseModel3
    {
        public virtual string EntryUser { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string UserId { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string OrganizationName { get; set; }
    }

    public class BaseViewModel5 : BaseModel4
    {
        public virtual string EntryUser { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string UserId { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string OrganizationName { get; set; }
    }

    public class BaseViewModel6: BaseModel5
    {
        public virtual string EntryUser { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string UserId { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string OrganizationName { get; set; }
    }
}
