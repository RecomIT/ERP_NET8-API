using API.Base;
using AutoMapper;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.DTO.Setup;
using BLL.Salary.Setup.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Setup
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class FiscalYearController : ApiBaseController
    {
        private readonly IFiscalYearBusiness _fiscalYearBusiness;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        public FiscalYearController(ISysLogger sysLogger, IMapper mapper, IFiscalYearBusiness fiscalYearBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _fiscalYearBusiness = fiscalYearBusiness;
            _mapper = mapper;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetFiscalYears")]
        public async Task<IActionResult> GetFiscalYearsAsync(long? fiscalYearId)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _fiscalYearBusiness.GetFiscalYearsAsync(fiscalYearId ?? 0, "", user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearController", "GetFiscalYearsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetFiscalYear")]
        public async Task<IActionResult> GetFiscalYearAsync(long? fiscalYearId)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _fiscalYearBusiness.GetFiscalYearAsync(fiscalYearId ?? 0, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearController", "GetFiscalYearAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveFiscalYear")]
        public async Task<IActionResult> SaveFiscalYearAsync(FiscalYearDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _fiscalYearBusiness.SaveFiscalYearAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearController", "SaveFiscalYearAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetCurrentFiscalYear")]
        public async Task<IActionResult> GetCurrentFiscalYearAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _fiscalYearBusiness.GetCurrentFiscalYearAsync(user);
                    return Ok(new { data.FiscalYearId, data.FiscalYearFrom, data.FiscalYearTo, data.AssesmentYear, data.FiscalYearRange });
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearController", "GetCurrentFiscalYear", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetFiscalYearDropdown")]
        public async Task<IActionResult> GetFiscalYearDropdownAysnc()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _fiscalYearBusiness.GetFiscalYearDropdownAysnc(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "FiscalYearController", "GetFiscalYearDropdownAysnc", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
