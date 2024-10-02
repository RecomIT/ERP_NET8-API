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
    public class DeductionNameController : ApiBaseController
    {
        private readonly IDeductionNameBusiness _deductionNameBusiness;
        private readonly ISysLogger _sysLogger;
        public DeductionNameController(ISysLogger sysLogger, IDeductionNameBusiness deductionNameBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _deductionNameBusiness = deductionNameBusiness;
            _sysLogger = sysLogger;
        }

        [HttpPost, Route("SaveDeductionName")]
        public async Task<ActionResult<ExecutionStatus>> SaveDeductionNameAsync(DeductionNameDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var validator = await _deductionNameBusiness.DeductionNameValidatorAsync(model, user);
                    if (validator != null && !validator.Status)
                    {
                        return Ok(validator);
                    }
                    var data = await _deductionNameBusiness.SaveDeductionNameAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameController", "SaveDeductionNameAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDeductionNames")]
        public async Task<IActionResult> GetDeductionNamesAsync([FromQuery] DeductionName_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _deductionNameBusiness.GetDeductionNamesAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameController", "GetDeductionNamesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDeductionNameById")]
        public async Task<IActionResult> GetDeductionNameByIdAsync([FromQuery] DeductionName_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = (await _deductionNameBusiness.GetDeductionNamesAsync(filter, user)).FirstOrDefault();
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameController", "GetDeductionNamesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDeductionNameDropdown")]
        public async Task<IActionResult> GetDeductionNameDropdownAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _deductionNameBusiness.GetDeductionNameDropdownAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionNameController", "GetDeductionNameDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
