using API.Base;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Variable;
using BLL.Salary.Variable.Interface;
using Shared.Payroll.Filter.Variable;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Variable
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class PeriodicalAllowanceController : ApiBaseController
    {
        private readonly IPeriodicallyVariableAllowanceBusiness _periodicallyVariableAllowance;
        private readonly ISysLogger _sysLogger;
        public PeriodicalAllowanceController(IPeriodicallyVariableAllowanceBusiness periodicallyVariableAllowance,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _periodicallyVariableAllowance = periodicallyVariableAllowance;
        }

        [HttpPost, Route("Save")]
        public async Task<IActionResult> SaveAsync(PeriodicalAllowanceInfoDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth && model.Details.Any())
                {
                    var data = await _periodicallyVariableAllowance.SaveAsync(model, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicalAllowanceController", "SaveAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAll")]
        public async Task<IActionResult> GetAllAsync([FromQuery] PeriodicalAllowance_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _periodicallyVariableAllowance.GetAllAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicalAllowanceController", "GetAllAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetById")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _periodicallyVariableAllowance.GetByIdAsync(id, user);
                    if (data != null)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicalAllowanceController", "GetAllAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeletePendingVariable")]
        public async Task<IActionResult> DeletePendingVariableAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && id > 0)
                {
                    var status = await _periodicallyVariableAllowance.DeletePendingVariableAsync(id, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicalAllowanceController", "DeletePendingVariable", user);
                return BadRequest(ResponseMessage.InvalidForm);
            }
        }

        [HttpGet, Route("GetHeadInfos/{id}")]
        public async Task<IActionResult> GetPeriodicalHeadInfosAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.HasBoth)
                {
                    var data = await _periodicallyVariableAllowance.GetPeriodicalHeadInfosAsync(id, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicalAllowanceController", "GetPeriodicalHeadInfosAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }

        [HttpGet, Route("GetPendingPrincipleAmountInfos/{id}")]
        public async Task<IActionResult> GetPendingPrincipleAmountInfosAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.HasBoth)
                {
                    var data = await _periodicallyVariableAllowance.GetPendingPrincipleAmountInfosAsync(id, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "PeriodicalAllowanceController", "GetPendingPrincipleAmountInfosAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }
    }
}
