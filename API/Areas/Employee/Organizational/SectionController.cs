
using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Organizational;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.Filter.Organizational;

namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class SectionController : ApiBaseController
    {
        private readonly ISectionBusiness _sectionBusiness;
        private readonly ISysLogger _sysLogger;
        public SectionController(ISysLogger sysLogger, ISectionBusiness sectionBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sectionBusiness = sectionBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetSections")]
        public async Task<IActionResult> GetSectionsAsync([FromQuery] Section_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _sectionBusiness.GetSectionsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SectionController", "GetSectionsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSectionById")]
        public async Task<IActionResult> GetGradeById([FromQuery] Section_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _sectionBusiness.GetSectionsAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeController", "GetGradeById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveSection")]
        public async Task<IActionResult> SaveSectionAsync(SectionDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _sectionBusiness.ValidateSectionAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _sectionBusiness.SaveSectionAsync(model, user);
                        return Ok(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SectionController", "SaveSectionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSectionDropdown")]
        public async Task<IActionResult> GetSectionDropdownAsync([FromQuery]Section_Filter filter)
        {
            var user = AppUser();
            try {
                if (user != null && user.HasBoth) {
                    var data_list = await _sectionBusiness.GetSectionDropdownAsync(filter,user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SectionController", "GetSectionDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
