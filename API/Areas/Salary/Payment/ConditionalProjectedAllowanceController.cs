using API.Base;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using BLL.Salary.Payment.Interface;
using Shared.Payroll.Filter.Payment;
using Microsoft.AspNetCore.Authorization;
using Shared.Payroll.DTO.Payment;

namespace API.Areas.Salary.Payment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class ConditionalProjectedAllowanceController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IConditionalProjectedPaymentBusiness _conditionalProjectedPaymentBusiness;
        public ConditionalProjectedAllowanceController(
            ISysLogger sysLogger,
            IClientDatabase clientDatabase,
            IConditionalProjectedPaymentBusiness conditionalProjectedPaymentBusiness
            ) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _conditionalProjectedPaymentBusiness = conditionalProjectedPaymentBusiness;
        }

        [HttpGet("GetConditionalProjectedPayments")]
        public async Task<IActionResult> GetConditionalProjectedPaymentsAsync([FromQuery] ConditionalProjected_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _conditionalProjectedPaymentBusiness.GetConditionalProjectedPaymentsAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "GetConditionalDepositAllowanceConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync(ConditionalProjectedPaymentDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var status = await _conditionalProjectedPaymentBusiness.SaveAsync(model, user);
                    if (status.Status)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "SaveAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("Approval")]
        public async Task<IActionResult> ApprovalAsync(ConditionalProjectedPaymentApprovalDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _conditionalProjectedPaymentBusiness.ApprovalAsync(model, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "SaveAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(long id)
        {
            var user = AppUser();   
            try
            {
                if(user.HasBoth && id > 0)
                {
                    var data = await _conditionalProjectedPaymentBusiness.GetById(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositController", "GetById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
