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
    public class AllowanceHeadController : ApiBaseController
    {
        private readonly IAllowanceHeadBusiness _allowanceHeadBusiness;
        private readonly ISysLogger _sysLogger;
        public AllowanceHeadController(ISysLogger sysLogger, IAllowanceHeadBusiness allowanceHeadBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _allowanceHeadBusiness = allowanceHeadBusiness;
        }

        [HttpPost, Route("SaveAllowanceHead")]
        public async Task<ActionResult<ExecutionStatus>> SaveAllowanceHeadAsync(AllowanceHeadDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _allowanceHeadBusiness.AllowanceHeadValidatorAsync(model, user);
                    if (validator != null && !validator.Status)
                    {
                        return Ok(validator);
                    }
                    var data = await _allowanceHeadBusiness.SaveAllowanceHeadAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadController", "SaveAllowanceHead", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAllowanceHeads")]
        public async Task<IActionResult> GetAllowanceHeadAsync([FromQuery] AllowanceHead_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _allowanceHeadBusiness.GetAllowanceHeadsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadController", "GetAllowanceHeads", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAllowanceHeadById")]
        public async Task<IActionResult> GetAllowanceHeadById([FromQuery] AllowanceHead_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _allowanceHeadBusiness.GetAllowanceHeadsAsync(filter, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadController", "GetAllowanceHeadById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAllowanceHeadDropdown")]
        public async Task<IActionResult> GetAllowanceHeadDropdownAsync()
        {
            var user = AppUser();
            try
            {
                if (user != null && user.HasBoth)
                {
                    var data_list = await _allowanceHeadBusiness.GetAllowanceHeadDropdownAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceHeadController", "GetAllowanceHeadDropdown", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
