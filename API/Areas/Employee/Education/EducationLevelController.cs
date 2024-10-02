using API.Base;
using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Miscellaneous;

namespace API.Areas.Employee.Education
{
    [ApiController, Area("HRMS"), Route("api/[area]/Education/[controller]"), Authorize]
    public class EducationLevelController : ApiBaseController
    {
        private readonly ILevelOfEducationBusiness _levelOfEducationBusiness;
        private readonly ISysLogger _sysLogger;
        public EducationLevelController(ILevelOfEducationBusiness levelOfEducationBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _levelOfEducationBusiness = levelOfEducationBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetLevelOfEducationsDropdown")]
        public async Task<IActionResult> GetLevelOfEducationsDropdownAsync()
        {
            var user = AppUser();
            try {
                var list = await _levelOfEducationBusiness.GetLevelOfEducationsDropdownAsync(user);
                return Ok(list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EducationLevelController", "GetLevelOfEducationsDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
