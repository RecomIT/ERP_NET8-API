using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Salary.Payment.Interface;
using API.Base;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.DTO.Payment;

namespace API.Areas.Salary.Payment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class ConditionalDepositController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IConditionalDepositAllowanceConfigBusiness _conditionalDepositAllowanceConfigBusiness;
        public ConditionalDepositController(ISysLogger sysLogger, IConditionalDepositAllowanceConfigBusiness conditionalDepositAllowanceConfigBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _conditionalDepositAllowanceConfigBusiness = conditionalDepositAllowanceConfigBusiness;
        }

        [HttpGet, Route("GetConditionalDepositAllowanceConfigs")]
        public async Task<IActionResult> GetConditionalDepositAllowanceConfigsAsync([FromQuery] ConditionalDepositAllowanceConfig_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _conditionalDepositAllowanceConfigBusiness.GetConditionalDepositAllowanceConfigsAsync(new ConditionalDepositAllowanceConfig_Filter(), user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "GetConditionalDepositAllowanceConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("Save")]
        public async Task<IActionResult> SaveAsync(ConditionalDepositAllowanceConfigDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var status = await _conditionalDepositAllowanceConfigBusiness.SaveAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "SaveAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }

        [HttpGet, Route("GetConditionalDepositAllowanceConfigById")]
        public async Task<IActionResult> GetConditionalDepositAllowanceConfigByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.HasBoth)
                {
                    var data = await _conditionalDepositAllowanceConfigBusiness.GetEmployeeConditionalDepositAllowanceConfigById(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "GetConditionalDepositAllowanceConfigByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeEligibleDepositPayment")]
        public async Task<IActionResult> GetEmployeeEligibleDepositPaymentAsync([FromQuery] EmployeeEligibleDepositPayment_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _conditionalDepositAllowanceConfigBusiness.GetEmployeeEligibleDepositPaymentAsync(filter, user);
                    if (data == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "GetEmployeeEligibleDepositPaymentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEligibleEmployeesByConfigId")]
        public async Task<IActionResult> GetEligibleEmployeesByConfigIdAsync(long configId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _conditionalDepositAllowanceConfigBusiness.GetEligibleEmployeesByConfigIdAsync(configId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "GetEligibleEmployeesByConfigIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
