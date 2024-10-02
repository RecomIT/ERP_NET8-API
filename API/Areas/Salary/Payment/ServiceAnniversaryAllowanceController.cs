using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using BLL.Salary.Payment.Interface;
using Shared.Payroll.Filter.Payment;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Payment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class ServiceAnniversaryAllowanceController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IServiceAnniversaryAllowanceBusiness _serviceAnniversaryAllowanceBusiness;
        public ServiceAnniversaryAllowanceController(ISysLogger sysLogger, IServiceAnniversaryAllowanceBusiness serviceAnniversaryAllowanceBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _serviceAnniversaryAllowanceBusiness = serviceAnniversaryAllowanceBusiness;
        }

        [HttpGet, Route("GetServiceAnniversaryAllowances")]
        public async Task<IActionResult> GetServiceAnniversaryAllowancesAsync([FromQuery] ServiceAnniversaryAllowance_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _serviceAnniversaryAllowanceBusiness.GetServiceAnniversaryAllowancesAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "GetServiceAnniversaryAllowanceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
