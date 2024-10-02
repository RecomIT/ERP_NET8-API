using Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Base.Interface;
using API.Services;
using DAL.DapperObject.Interface;
using BLL.Salary.Bonus.Interface;
using API.Base;
using Shared.Payroll.Filter.Bonus;
using Shared.Payroll.ViewModel.Bonus;

namespace API.Areas.Salary.Bonus
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class BonusController : ApiBaseController
    {
        private readonly IBonusBusiness _bonusBusiness;
        private readonly ISysLogger _sysLogger;
        public BonusController(IBonusBusiness bonusBusiness, IClientDatabase clientDatabase, ISysLogger sysLogger) : base(clientDatabase)
        {
            _bonusBusiness = bonusBusiness;
            _sysLogger = sysLogger;
        }

        [HttpPost, Route("SaveBonus")]
        public async Task<IActionResult> SaveBonusAsync(BonusViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _bonusBusiness.SaveBonusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusController", "SaveBonusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBonuses")]
        public async Task<IActionResult> GetBonusesync(long? bonusId, string bonusName)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {

                    var data = await _bonusBusiness.GetBonusesAsync(bonusName ?? "", bonusId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusController", "GetBonusesync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBonusConfigs")]
        public async Task<IActionResult> GetBonusConfigsAsync([FromForm] BonusQuery filter)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _bonusBusiness.GetBonusConfigsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BonusController", "GetBonusConfigsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveBonusConfig")]
        public async Task<IActionResult> SaveBonusConfigAsync(BonusConfigViewModel bonusConfig)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _bonusBusiness.SaveBonusConfigAsync(bonusConfig, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchController", "SaveBonusConfigAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


        //Added by Monzur 20-SEP-2023

        [HttpGet, Route("GetLFAYearlyAllowanceExtension")]
        public async Task<IActionResult> GetLFAYearlyAllowanceExtensionAsync(long? allowanceNameId)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _bonusBusiness.GetLFAYearlyAllowanceExtensionAsync(allowanceNameId ?? 0, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
