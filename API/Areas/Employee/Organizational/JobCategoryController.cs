using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;

namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class JobCategoryController : ApiBaseController
    {
        private readonly IJobCategoryBusiness _jobCategoryBusiness;
        private readonly ISysLogger _sysLogger;
        public JobCategoryController(
            ISysLogger sysLogger,
            IJobCategoryBusiness jobCategoryBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _jobCategoryBusiness = jobCategoryBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetJobCategoryDropdown")]
        public async Task<IActionResult> GetJobCategoryDropdownAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _jobCategoryBusiness.GetJobCategoryDropdownAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "JobCategoryController", "GetJobCategoryDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
