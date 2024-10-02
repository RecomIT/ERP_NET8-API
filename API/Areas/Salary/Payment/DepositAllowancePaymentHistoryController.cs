using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Payment;
using DAL.Payroll.Repository.Interface;
using Microsoft.AspNetCore.Authorization;


namespace API.Areas.Salary.Payment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class DepositAllowancePaymentHistoryController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDepositAllowancePaymentHistoryRepository _depositAllowancePaymentHistoryRepository;
        public DepositAllowancePaymentHistoryController(ISysLogger sysLogger, IDepositAllowancePaymentHistoryRepository depositAllowancePaymentHistoryRepository, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _depositAllowancePaymentHistoryRepository = depositAllowancePaymentHistoryRepository;
        }

        [HttpPost, Route("SavePaymentOfDepositAmount")]
        public async Task<IActionResult> SavePaymentOfDepositAmountAsync(PaymentOfDepositAmountByConfig model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _depositAllowancePaymentHistoryRepository.SavePaymentOfDepositAmountAsync(model.items, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "GetEligibleEmployeesByConfigIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
