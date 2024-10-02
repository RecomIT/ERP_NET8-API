using Shared.OtherModels.User;
using System.Threading.Tasks;

namespace BLL.Separation.DownloadPDF.Interface
{
    public interface IDownloadPDF
    {
        Task<string> DownloadResignationLetterAsync(dynamic filter, AppUser user);
    }
}
