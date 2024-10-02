using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Download.Interface
{
    public interface IDownloadBusiness
    {
        Task<string> DownloadAsync(dynamic filter, AppUser user);
    }
}
