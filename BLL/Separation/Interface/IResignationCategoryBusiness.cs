using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Separation.Interface
{
    public interface IResignationCategoryBusiness
    {
        Task<object> GetResignationCategoryAsync(AppUser user);

        Task<object> GetResignationSubCategoryAsync(dynamic filter, AppUser user);

        Task<object> GetResignationNoticePeriodAsync(dynamic filter, AppUser user);
    }
}
