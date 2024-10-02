using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Deduction;
using BLL.Salary.Deduction.Interface;
using Shared.Payroll.Filter.Deduction;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Deduction
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class DeductionHeadController : ApiBaseController
    {
        private readonly IDeductionHeadBusiness _deductionHeadBusiness;
        private readonly ISysLogger _sysLogger;
        public DeductionHeadController(ISysLogger sysLogger, IDeductionHeadBusiness deductionHeadBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _deductionHeadBusiness = deductionHeadBusiness;
        }

        [HttpPost, Route("SaveDeductionHead")]
        public async Task<ActionResult<ExecutionStatus>> SaveDeductionHeadAsync(DeductionHeadDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _deductionHeadBusiness.DeductionHeadValidatorAsync(model, user);
                    if (validator != null && !validator.Status)
                    {
                        return Ok(validator);
                    }
                    var data = await _deductionHeadBusiness.SaveDeductionHeadAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadController", "SaveDeductionHeadAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDeductionHeads")]
        public async Task<IActionResult> GetDeductionHeadAsync([FromQuery] DeductionHead_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _deductionHeadBusiness.GetDeductionHeadsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadController", "GetDeductionHeadAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDeductionHeadById")]
        public async Task<IActionResult> GetDeductionHeadByIdAsync([FromQuery] DeductionHead_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = (await _deductionHeadBusiness.GetDeductionHeadsAsync(filter, user)).FirstOrDefault();
                    if (data != null)
                    {
                        return Ok(data);
                    }
                    return NoContent();
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadController", "GetDeductionHeadAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDeductionHeadDropdown")]
        public async Task<IActionResult> GetDeductionHeadDropdownAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _deductionHeadBusiness.GetDeductionHeadDropdownAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionHeadController", "GetDeductionHeadDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
