using API.Base;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Allowance;
using BLL.Salary.Allowance.Interface;
using Shared.Payroll.Filter.Allowance;
using Microsoft.AspNetCore.Authorization;
using Shared.Payroll.ViewModel.Configuration;

namespace API.Areas.Salary.Allowance
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class AllowanceConfigController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IAllowanceConfigBusiness _allowanceConfigBusiness;

        public AllowanceConfigController(IClientDatabase clientDatabase, IAllowanceConfigBusiness allowanceConfigBusiness, ISysLogger sysLogger) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _allowanceConfigBusiness = allowanceConfigBusiness;
        }

        [HttpPost("SaveAllowanceConfig")]
        public async Task<IActionResult> SaveAllowanceConfigAsync(AllowanceConfigurationDTO allowance)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _allowanceConfigBusiness.SaveAllowanceConfigAsync(allowance, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceConfigController", "SaveAllowanceConfigAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetAllownaceConfigurations")]
        public async Task<IActionResult> GetAllownaceConfigurationsAsync([FromQuery] AllowanceConfig_Filter model, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    model.PageNumber = Utility.PageNumber(model.PageNumber);
                    model.PageSize = Utility.PageSize(model.PageSize);
                    var allData = await _allowanceConfigBusiness.GetAllownaceConfigurationsAsync(model, user);
                    var data = PagedList<AllowanceConfigurationViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceConfigController", "GetAllownaceConfigurationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetAllownaceConfigurationById")]
        public async Task<IActionResult> GetAllownaceConfigurationByIdAsync([FromQuery] AllowanceConfig_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = (await _allowanceConfigBusiness.GetAllownaceConfigurationsAsync(filter, user)).FirstOrDefault();
                    if (data != null)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return NoContent();
                    }

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceConfigController", "GetAllownaceConfigurationByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveAllowanceConfigStatus")]
        public async Task<IActionResult> SaveAllowanceConfigStatusAsync(AllowanceConfigStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && Utility.StatusChecking(model.StateStatus, new string[] { "Approved", "Recheck", "Cancelled" }) && user.HasBoth)
                {
                    var data = await _allowanceConfigBusiness.SaveAllowanceConfigStatusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceConfigController", "SaveAllowanceConfigStatus", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
