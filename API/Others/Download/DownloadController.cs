using API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Download;
using BLL.Download.Interface;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using API.Base;

namespace API.Others.Download
{
    [ApiController, Area("HRMS"), Route("api/[area]/[controller]"), Authorize]
    public class DownloadController : ApiBaseController
    {
        private readonly IDownloadBusiness _downloadBusiness;
        public DownloadController(IClientDatabase clientDatabase, IDownloadBusiness downloadBusiness) : base(clientDatabase)
        {
            _downloadBusiness = downloadBusiness;
        }

        [HttpGet]
        [Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile([FromQuery] DownloadResignationLetter_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var filePath = await _downloadBusiness.DownloadAsync(filter, user);

                    if (!string.IsNullOrEmpty(filePath))
                    {
                        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
                        return File(fileBytes, "application/octet-stream", filter.FileName);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}

