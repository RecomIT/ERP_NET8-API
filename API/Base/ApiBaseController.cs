using API.Services;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.StaticFiles;
using Shared.Helpers;
using Shared.OtherModels.User;
using Shared.Services;

namespace API.Base
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

    public class ApiBaseController : ControllerBase
    {
        private readonly IClientDatabase _clientDatabase;

        public ApiBaseController(IClientDatabase clientDatabase)
        {
            _clientDatabase = clientDatabase;
        }


        [HttpGet("GetDatabaseName/{organizationId}")]
        public string GetDatabaseName(long organizationId)
        {
            return _clientDatabase.GetDatabaseName(organizationId);
        }

        [HttpGet, Route("DownloadFormatExcelFile")]
        public async Task<IActionResult> DownloadFormatExcelFileAysnc(string fileName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel", fileName);
            var provider = new FileExtensionContentTypeProvider();
            string contenttype = "";
            if (System.IO.File.Exists(filepath))
            {
                contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, fileName);
        }

        [HttpGet, Route("GetDocument")]
        public IActionResult GetDocument(string path)
        {
            var user = AppUser();
            try
            {
                if (!Utility.IsNullEmptyOrWhiteSpace(path) && path !="/" && user.HasBoth)
                {
                    var format = Utility.GetFileExtension(path);
                    var fileName = Utility.GetFileName(path);

                    var filebytes = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, path));

                    var mimeType = Utility.GetFileMimetype(format ?? "");
                    if (filebytes == null || filebytes.Length == 0)
                    {
                        return NotFound("File is not exist");
                    }
                    else
                    {
                        return File(filebytes, mimeType, fileName);
                    }
                }
                return NotFound("File is not exist");
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        internal AppUser AppUser()
        {
            var httpRequest = HttpContext.Request;
            var userObject = UserObjects.UserData(httpRequest);
            var clientObj = _clientDatabase.GetClientObj(userObject.OrganizationId);
            userObject.Database = clientObj.Database;
            userObject.OrgCode = clientObj.OrgCode;
            return userObject;
        }

        internal string ModelStateErrorMsg(ModelStateDictionary ModelState)
        {
            var message = string.Join(" | ", ModelState.Values
                   .SelectMany(v => v.Errors)
                   .Select(e => e.ErrorMessage));
            return message;
        }
    }
}
