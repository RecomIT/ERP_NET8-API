using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational;

namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class SubSectionController : ApiBaseController
    {
        private readonly ISubSectionBusiness _subSectionBusiness;
        private readonly ISysLogger _sysLogger;
        public SubSectionController(ISysLogger sysLogger, ISubSectionBusiness subSectionBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _subSectionBusiness = subSectionBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetSubSections")]
        public async Task<IActionResult> GetSubSectionsAsync([FromQuery] SubSection_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _subSectionBusiness.GetSubSectionsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubSectionController", "GetSubSectionsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSubSectionById")]
        public async Task<IActionResult> GetSubSectionByIdAsync([FromQuery] SubSection_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _subSectionBusiness.GetSubSectionsAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubSectionController", "GetSubSectionByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveSubSection")]
        public async Task<IActionResult> SaveSubSectionAsync(SubSectionDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _subSectionBusiness.ValidateSubSectionAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _subSectionBusiness.SaveSubSectionAsync(model, user);
                        return Ok(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubSectionController", "SaveSubSectionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSubSectionDropdown")]
        public async Task<IActionResult> GetSubSectionDropdownAsync([FromQuery] SubSection_Filter filter)
        {
            var user = AppUser();
            try {
                if (user != null && user.HasBoth) {
                    var data_list = await _subSectionBusiness.GetSubSectionDropdownAsync(filter,user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SubSectionController", "GetSubSectionDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
