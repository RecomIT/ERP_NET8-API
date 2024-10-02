using System;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Tax.Interface;
using API.Base;
using Shared.Payroll.DTO.Tax;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxSettingController : ApiBaseController
    {
        private readonly ITaxSettingBusiness _taxSettingBusiness;
        private readonly ISysLogger _sysLogger;
        public TaxSettingController(ISysLogger sysLogger, ITaxSettingBusiness taxSettingBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _taxSettingBusiness = taxSettingBusiness;
        }

        [HttpPost, Route("SaveTaxSetting")]
        public async Task<IActionResult> SaveTaxSettingAsync(TaxSettingDTO setting)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _taxSettingBusiness.ValidateIncomeTaxSettingAsync(setting, user);
                    if (validator != null && validator.Status == false)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _taxSettingBusiness.SaveTaxSettingAsync(setting, user);
                        return Ok(data);
                    }
                }
                return Ok(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchController", "SaveTaxSettingAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetTaxSettings")]
        public async Task<IActionResult> GetTaxSettingsAsync(long? IncomeTaxSettingId, long? FiscalYearId, string ImpliedCondition)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _taxSettingBusiness.GetTaxSettingsAsync(IncomeTaxSettingId ?? 0, FiscalYearId ?? 0, ImpliedCondition ?? "", user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxController", "GetTaxSettings", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetTaxSettingById")]
        public async Task<IActionResult> GetTaxSettingByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _taxSettingBusiness.GetTaxSettingAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxController", "GetTaxSetting", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
