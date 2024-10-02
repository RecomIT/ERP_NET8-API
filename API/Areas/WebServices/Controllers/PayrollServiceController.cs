using API.Base;
using API.Services;
using AutoMapper;
using BLL.Base.Interface;
using BLL.Salary.Allowance.Interface;
using BLL.Salary.Bonus.Interface;
using BLL.Salary.Deduction.Interface;
using BLL.Salary.Salary.Interface;
using BLL.Salary.Setup.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.Filter.Salary;
using Shared.Services;


namespace API.Areas.WebServices.Controllers
{
    [ApiController, Area("WS"), Route("api/[area]/[controller]"), Authorize]
    public class PayrollServiceController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IAllowanceHeadBusiness _allowanceHeadBusiness;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly IDeductionHeadBusiness _deductionHeadBusiness;
        private readonly IDeductionNameBusiness _deductionNameBusiness;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;
        private readonly IBonusBusiness _bonusBusiness;
        private readonly IFiscalYearBusiness _fiscalYearBusiness;
        private readonly ITaxZoneBusiness _taxZoneBusiness;
        private readonly ISysLogger _sysLogger;
        public PayrollServiceController(IMapper mapper,
            IAllowanceHeadBusiness allowanceHeadBusiness,
            IAllowanceNameBusiness allowanceNameBusiness,
            IDeductionHeadBusiness deductionHeadBusiness,
            IDeductionNameBusiness deductionNameBusiness,
            IFiscalYearBusiness fiscalYearBusiness,
            ISalaryReviewBusiness salaryReviewBusiness,
            IClientDatabase clientDatabase,
            IBonusBusiness bonusBusiness,
            ITaxZoneBusiness taxZoneBusiness,
            ISysLogger sysLogger
           ) : base(clientDatabase)
        {
            _mapper = mapper;
            _allowanceHeadBusiness = allowanceHeadBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _deductionHeadBusiness = deductionHeadBusiness;
            _deductionNameBusiness = deductionNameBusiness;
            _fiscalYearBusiness = fiscalYearBusiness;
            _salaryReviewBusiness = salaryReviewBusiness;
            _bonusBusiness = bonusBusiness;
            _taxZoneBusiness = taxZoneBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetAllowanceHeadsExtension")]
        public async Task<IActionResult> GetAllowanceHeadAsync(long companyId, long organizationId)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _allowanceHeadBusiness.GetAllowanceHeadExtensionAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetAllowanceHeadAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetAllowanceNamesExtension")]
        public async Task<IActionResult> GetAllowanceNamesAsync(string allowanceType)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _allowanceNameBusiness.GetAllowanceNameExtensionAsync(allowanceType, 0, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetAllowanceNamesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDeductionHeadsExtension")]
        public async Task<IActionResult> GetDeductionHeadAsync()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _deductionHeadBusiness.GetDeductionHeadExtensionAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetDeductionHeadsExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDeductionNamesExtension")]
        public async Task<IActionResult> GetDeductionNamesAsync(string deductionType)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _deductionNameBusiness.GetDeductionNameExtensionAsync(deductionType, 0, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetDeductionNamesExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryReviewInfosExtension")]
        public async Task<IActionResult> GetSalaryReviewInfosAsync(long? salaryReviewInfoId, long? employeeId)
        {
            var user = AppUser();
            try {
                if (user.CompanyId > 0 && user.OrganizationId > 0 && ((employeeId ?? 0) > 0 || (salaryReviewInfoId ?? 0) > 0)) {
                    var data = await _salaryReviewBusiness.GetSalaryReviewInfosAsync(new SalaryReview_Filter() { SalaryReviewInfoId=salaryReviewInfoId.ToString(), EmployeeId=employeeId.ToString()}, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetSalaryReviewInfosExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetFiscalYearsExtension")]
        public async Task<IActionResult> GetFiscalYearsExtensionAsync()
        {
            var user = AppUser();
            try {
                if (user.CompanyId > 0 && user.OrganizationId > 0) {
                    var data = await _fiscalYearBusiness.GetFiscalYearsExtensionAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetFiscalYearsExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetCurrentFiscalYear")]
        public async Task<IActionResult> GetCurrentFiscalYearAsync()
        {
            var user = AppUser();
            try {
                var data = await _fiscalYearBusiness.GetCurrentFiscalYearAsync(user);
                return Ok(data);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetCurrentFiscalYearAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        #region Bonus
        [HttpGet, Route("GetBonusExtension")]
        public async Task<IActionResult> GetBonusExtensionAsync()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var bonus_list = await _bonusBusiness.GetBonusExtensionAsync(user);
                    if (bonus_list.Count() > 0) {
                        return Ok(bonus_list);
                    }
                    else {
                        return NoContent();
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetBonusExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBonusAndConfigInThisFiscalYearExtension")]
        public async Task<IActionResult> GetBonusAndConfigInThisFiscalYearExtensionAsync()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var bonus_list = await _bonusBusiness.GetBonusAndConfigInThisFiscalYearExtensionAsync(user);
                    if (bonus_list.Count() > 0) {
                        return Ok(bonus_list);
                    }
                    else {
                        return NoContent();
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetBonusAndConfigInThisFiscalYearExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        //Tax Zone
        [HttpGet, Route("GetTaxZoneNamesExtension")]
        public async Task<IActionResult> GetTaxZoneNameExtensionAsync(string taxZone)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _taxZoneBusiness.GetTaxZoneNameExtensionAsync(taxZone, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PayrollServiceController", "GetTaxZoneNamesExtension", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion
    }
}
