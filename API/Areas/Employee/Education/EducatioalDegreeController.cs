using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Miscellaneous;
using DAL.DapperObject.Interface;

namespace API.Areas.Employee.Education
{

    [ApiController, Area("HRMS"), Route("api/[area]/Education/[controller]"), Authorize]
    public class EducationalDegreeController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEducationalDegreeBusiness _educationalDegreeBusiness;

        public EducationalDegreeController(IEducationalDegreeBusiness educationalDegreeBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _educationalDegreeBusiness = educationalDegreeBusiness;
            _sysLogger = sysLogger;
        }


        [HttpGet, Route("GetEducationDegreeDropdown/{id:long}")]
        public async Task<IActionResult> GetEducationDegreeDropdownAsync(long id)
        {
            var user = AppUser();
            try {
                if (id > 0) {
                    var data = await _educationalDegreeBusiness.GetEducationDegreeDropdownAsync(id, user);
                    return Ok(data);
                }
                return NotFound("Level Of eductionId is not found");
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterController", "GetEducationDegreeDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



    }
}
