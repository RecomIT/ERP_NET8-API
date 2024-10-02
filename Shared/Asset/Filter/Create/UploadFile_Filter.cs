using Microsoft.AspNetCore.Http;

namespace Shared.Asset.Filter.Create
{
    public class UploadFile_Filter    {
        public string Format { get; set; }
        public IFormFile File { get; set; }
    }
}
