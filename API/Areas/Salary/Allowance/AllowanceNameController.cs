using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Allowance;
using BLL.Salary.Allowance.Interface;
using Shared.Payroll.Filter.Allowance;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Allowance
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class AllowanceNameController : ApiBaseController
    {
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly ISysLogger _sysLogger;
        public AllowanceNameController(ISysLogger sysLogger, IAllowanceNameBusiness allowanceNameBusiness, IClientDatabase _clientDatabase) : base(_clientDatabase)
        {
            _sysLogger = sysLogger;
            _allowanceNameBusiness = allowanceNameBusiness;
        }

        [HttpPost, Route("SaveAllowanceName")]
        public async Task<ActionResult<ExecutionStatus>> SaveAllowanceNameAsync(AllowanceNameDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _allowanceNameBusiness.AllowanceNameValidatorAsync(model, user);
                    if (validator != null && !validator.Status)
                    {
                        return Ok(validator);
                    }
                    var data = await _allowanceNameBusiness.SaveAllowanceNameAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameController", "SaveAllowanceName", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAllowanceNames")]
        public async Task<IActionResult> GetAllowanceNamesAsync([FromQuery] AllowanceName_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _allowanceNameBusiness.GetAllowanceNamesAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameController", "GetAllowanceNamesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAllowanceNameById")]
        public async Task<IActionResult> GetAllowanceNameByIdAsync([FromQuery] AllowanceName_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _allowanceNameBusiness.GetAllowanceNamesAsync(filter, user);
                    if (data != null)
                    {
                        return Ok(data.FirstOrDefault());
                    }
                    return NoContent();
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameController", "GetAllowanceNames", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveAllowanceNameWithConfig")]
        public async Task<ActionResult<ExecutionStatus>> SaveAllowanceNameWithConfigAsync(AllowanceNameDTO model, string userId)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _allowanceNameBusiness.SaveAllowanceNameWithConfigAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameController", "SaveAllowanceName", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAllowanceNameDropdown")]
        public async Task<IActionResult> GetAllowanceNameDropdownAsync()
        {
            var user = AppUser();
            try
            {
                var data = await _allowanceNameBusiness.GetAllowanceNameDropdownAsync(user);
                return Ok(data);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameController", "GetAllowanceHeadDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
