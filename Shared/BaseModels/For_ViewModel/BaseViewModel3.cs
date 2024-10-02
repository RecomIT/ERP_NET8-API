using Shared.BaseModels.For_DomainModel;

namespace Shared.BaseModels.For_ViewModel
{
    public class BaseViewModel3 : BaseModel2
    {
        public virtual string EntryUser { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string UserId { get; set; }
        public virtual string BranchName { get; set; }
        public virtual string CompanyName { get; set; }
        public virtual string OrganizationName { get; set; }
    }
}
