using API.Base;
using API.Services;
using BLL.Base.Interface;
using Shared.Helpers;
using BLL.Salary.Payment.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.Filter.Payment;
using Shared.Services;

namespace API.Areas.Salary.Payment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class MonthlyAllowanceConfigController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IMonthlyAllowanceConfigBusiness _monthlyAllowanceConfigBusiness;
        public MonthlyAllowanceConfigController(
            ISysLogger sysLogger,
            IMonthlyAllowanceConfigBusiness monthlyAllowanceConfigBusiness,
            IClientDatabase clientDatabase
        ) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _monthlyAllowanceConfigBusiness = monthlyAllowanceConfigBusiness;
        }

        [HttpGet("GetMonthlyAllowanceConfigs")]
        public async Task<IActionResult> GetMonthlyAllowanceConfigsAsync([FromQuery] MonthlyAllowanceConfig_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _monthlyAllowanceConfigBusiness.GetMonthlyAllowanceConfigsAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);    
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "MonthlyAllowanceConfigController", "GetMonthlyAllowanceConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
