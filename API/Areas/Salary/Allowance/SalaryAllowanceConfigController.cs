using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using BLL.Salary.Allowance.Interface;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.DTO.Configuration;
using Microsoft.AspNetCore.Authorization;
using Shared.Payroll.DTO.Allowance;

namespace API.Areas.Salary.Allowance
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SalaryAllowanceConfigController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ISalaryAllowanceConfigBusiness _salaryAllowanceConfigBusiness;
        public SalaryAllowanceConfigController(ISysLogger sysLogger, ISalaryAllowanceConfigBusiness salaryAllowanceConfigBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _salaryAllowanceConfigBusiness = salaryAllowanceConfigBusiness;
        }

        [HttpPost, Route("SaveSalaryAllowanceConfig")]
        public async Task<IActionResult> SaveSalaryAllowanceConfigAsync(SalaryAllowanceConfigurationInfoDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _salaryAllowanceConfigBusiness.SaveSalaryAllowanceConfigAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "SaveSalaryAllowanceConfigAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryAllowanceConfigurationInfos")]
        public async Task<IActionResult> GetSalaryAllowanceConfigurationInfosAsync([FromQuery] SalaryAllowanceConfig_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryAllowanceConfigBusiness.GetSalaryAllowanceConfigurationInfosAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "GetSalaryAllowanceConfigurationInfos", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryAllowanceConfigurationDetails")]
        public async Task<IActionResult> GetSalaryAllowanceConfigurationDetailsAsync(long salaryAllowanceConfigId)
        {
            var user = AppUser();
            try
            {
                if (salaryAllowanceConfigId > 0 && user.HasBoth)
                {
                    var data = await _salaryAllowanceConfigBusiness.GetSalaryAllowanceConfigurationDetailsAsync(salaryAllowanceConfigId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "GetSalaryAllowanceConfigurationDetails", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveSalaryAllowanceConfigStatus")]
        public async Task<IActionResult> SaveSalaryAllowanceConfigStatusAsync(SalaryAllowanceConfigurationStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && Utility.StatusChecking(model.StateStatus, new string[] { "Approved", "Recheck" })
                    && user.HasBoth)
                {
                    var data = await _salaryAllowanceConfigBusiness.SaveSalaryAllowanceConfigStatusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "SaveSalaryAllowanceConfigStatus", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("Save")]
        public async Task<IActionResult> SaveAsync(SalaryAllowanceConfigurationInfoDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _salaryAllowanceConfigBusiness.SaveAsync(model, user);
                    if (data.Status)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return BadRequest(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "SaveAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] SalaryAllowanceConfig_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryAllowanceConfigBusiness.GetAllAsync(filter, user);
                    if (data.Any())
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return NotFound("No data found");
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "SaveAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetHeadsInfo")]
        public async Task<IActionResult> GetHeadsInfoAsync([FromQuery] Breakhead_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryAllowanceConfigBusiness.GetHeadsInfoAsync(filter, user);
                    if (data.Any())
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return NotFound("No data found");
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "GetHeadsInfoAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeletePendingConfig")]
        public async Task<IActionResult> DeletePendingConfigAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && id > 0)
                {
                    var status = await _salaryAllowanceConfigBusiness.DeletePendingConfigAsync(id, user);
                    if (status.Status)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "DeletePendingConfigAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("ApprovedPendingConfig")]
        public async Task<IActionResult> ApprovedPendingConfigAsync(ApprovedPendingSalaryAllowanceConfigDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var status = await _salaryAllowanceConfigBusiness.ApprovedPendingConfigAsync(model, user);
                    if (status.Status)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigController", "ApprovedPendingConfigAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
