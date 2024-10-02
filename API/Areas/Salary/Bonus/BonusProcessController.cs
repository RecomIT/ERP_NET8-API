using API.Base;
using API.Services;
using BLL.Base.Interface;
using BLL.Salary.Bonus.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using Shared.Payroll.DTO.Bonus;
using Shared.Payroll.Filter.Bonus;
using Shared.Payroll.ViewModel.Bonus;
using Shared.Services;

namespace API.Areas.Salary.Bonus
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class BonusProcessController : ApiBaseController
    {
        private readonly IBonusProcessBusiness _bonusProcessBusiness;
        private readonly IBonusBusiness _bonusBusiness;
        private readonly ISysLogger _sysLogger;

        public BonusProcessController(IBonusBusiness bonusBusiness, ISysLogger sysLogger, IBonusProcessBusiness bonusProcessBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _bonusProcessBusiness = bonusProcessBusiness;
            _sysLogger = sysLogger;
            _bonusBusiness = bonusBusiness;
        }

        #region Bonus Process Info

        [HttpPost("ExecuteBonusProcess")]
        public async Task<IActionResult> BonusProcessAsync([FromQuery] ExecuteBonusProcess process)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _bonusProcessBusiness.ExecuteBonusProcessAsync(process, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusProcessController", "ExecuteBonusProcess", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetBonusProcessInfo")]
        public async Task<IActionResult> GetBonusProcessInfoAysnc([FromQuery] BonusProcessInfo_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _bonusProcessBusiness.GetBonusProcessesInfoAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusProcessController", "GetBonusProcessInfoAysnc", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        #endregion

        #region  Bonus Process Detail
        [HttpGet("GetBonusProcessDetails")]
        public async Task<IActionResult> GetBonusProcessDetailAsync([FromQuery] BonusProcessDetail_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _bonusProcessBusiness.GetBonusProcessDetailAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusProcessController", "GetBonusProcessDetailAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion

        [HttpPost("DisbursedBonus")]
        public async Task<IActionResult> DisbursedBonusAsync(DisbursedUndoBonusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _bonusProcessBusiness.DisbursedBonusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusProcessController", "DisbursedBonusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("UndoBonus")]
        public async Task<IActionResult> UndoBonusAsync(DisbursedUndoBonusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _bonusProcessBusiness.UndoBonusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusProcessController", "UndoBonusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("UndoEmployeeBonus")]
        public async Task<IActionResult> UndoEmployeeBonusAsync(UndoEmployeeBonus model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _bonusProcessBusiness.UndoEmployeeBonusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusProcessController", "UndoEmployeeBonusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        #region Exclude Employee From Bonus
        [HttpPost, Route("SaveExcludeEmployeeFromBonus")]
        public async Task<IActionResult> SaveExcludeEmployeeFromBonusAsync(EmployeeExcludedFromBonusDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _bonusProcessBusiness.SaveExcludeEmployeeFromBonusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "BonusProcessController", "SaveExcludeEmployeeFromBonusAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetExcludedEmployeesFromBonus")]
        public async Task<IActionResult> GetExcludedEmployeesFromBonusAsync([FromQuery] ExcludeEmployeedFromBonus_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasUserId)
                {
                    var data = await _bonusProcessBusiness.GetExcludedEmployeesFromBonusAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "BonusProcessController", "SaveExcludeEmployeeFromBonus", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("DeleteEmployeeFromExcludeList")]
        public async Task<IActionResult> DeleteEmployeeFromExcludeListAsync(EmployeeExcludedFromBonusDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _bonusProcessBusiness.DeleteEmployeeFromExcludeListAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {

                await _sysLogger.SavePayrollException(ex, user.Database, "BonusProcessController", "DeleteEmployeeFromExcludeListAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }
        #endregion
    }
}
