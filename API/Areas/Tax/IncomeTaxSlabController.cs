using System;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Services;
using DAL.DapperObject.Interface;
using BLL.Tax.Interface;
using API.Base;
using Shared.Payroll.ViewModel.Tax;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class IncomeTaxSlabController : ApiBaseController
    {
        private readonly IIncomeTaxSlabBusiness _incomeTaxSlabBusiness;
        private readonly ISysLogger _sysLogger;

        public IncomeTaxSlabController(IIncomeTaxSlabBusiness incomeTaxSlabBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _incomeTaxSlabBusiness = incomeTaxSlabBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetIncomeTaxSlabs")]
        public async Task<ActionResult> GetIncomeTaxSlabsAsync(long? IncomeTaxSlabId, string ImpliedCondition, long? FiscalYearId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = _incomeTaxSlabBusiness.GetIncomeTaxSlabsAsync(IncomeTaxSlabId, ImpliedCondition, FiscalYearId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabController", "GetIncomeTaxSlabsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveIncomeTaxSlab")]
        public async Task<ActionResult> SaveIncomeTaxSlabAsync(TaxSlabInfo model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _incomeTaxSlabBusiness.SaveIncomeTaxSlabAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabController", "SaveIncomeTaxSlab", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetIncomeTaxSlabsData")]
        public async Task<ActionResult> GetIncomeTaxSlabsDataAsync(long? IncomeTaxSlabId, string ImpliedCondition, long? FiscalYearId)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _incomeTaxSlabBusiness.GetIncomeTaxSlabsDataAsync(IncomeTaxSlabId ?? 0, ImpliedCondition, FiscalYearId ?? 0, user);
                    return Ok(data); ;
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabController", "UpdateIncomeTaxSlab", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UpdateIncomeTaxSlab")]
        public async Task<ActionResult> UpdateIncomeTaxSlabAsync(TaxSlabUpdate model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _incomeTaxSlabBusiness.UpdateIncomeTaxSlabAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabController", "UpdateIncomeTaxSlabAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
