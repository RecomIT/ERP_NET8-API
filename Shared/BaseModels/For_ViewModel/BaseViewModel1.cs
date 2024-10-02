using Shared.BaseModels.For_DomainModel;

namespace Shared.BaseModels.For_ViewModel
{
    public class BaseViewModel1 : BaseModel
    {
        public virtual string EntryUser { get; set; }
        public virtual string UpdateUser { get; set; }
        public virtual string UserId { get; set; }
    }
}
