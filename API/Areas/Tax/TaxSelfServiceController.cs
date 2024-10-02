using API.Base;
using API.Services;
using BLL.Administration.Interface;
using BLL.Base.Interface;
using BLL.Employee.Implementation.Info;
using BLL.Employee.Interface.Info;
using BLL.Salary.Salary.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using Shared.OtherModels.Response;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Services;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxSelfServiceController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private readonly ITaxReportBusiness _taxReportBusiness;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ITaxAITBusiness _taxAITBusiness;
        private readonly IReportBase _reportBase;
        private readonly ITaxRefundBusiness _taxRefundBusiness;
        private readonly IInfoBusiness _infoBusiness;
        private TaxRelatedReportGenerator _taxRelatedReportGenerator;
        private ExcelGenerator _excelGenerator;
        private ReportFile _reportFile;

        public TaxSelfServiceController(
            ISysLogger sysLogger,
            IWebHostEnvironment webHostEnvironment,
            ISalaryReportBusiness salaryReportBusiness, ITaxReportBusiness taxReportBusiness, IReportConfigBusiness reportConfigBusiness, IClientDatabase clientDatabase, ITaxAITBusiness taxAITBusiness, ITaxRefundBusiness taxRefundBusiness, IReportBase reportBase, IInfoBusiness infoBusiness) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _webHostEnvironment = webHostEnvironment;
            _salaryReportBusiness = salaryReportBusiness;
            _reportConfigBusiness = reportConfigBusiness;
            _taxReportBusiness = taxReportBusiness;
            _excelGenerator = new ExcelGenerator();
            _reportBase = reportBase;
            _infoBusiness = infoBusiness;
            _taxRelatedReportGenerator = new TaxRelatedReportGenerator(_sysLogger, _reportBase, _infoBusiness, _taxReportBusiness, _webHostEnvironment, _reportConfigBusiness, _salaryReportBusiness);
            _taxAITBusiness = taxAITBusiness;
            _taxRefundBusiness = taxRefundBusiness;

        }

        [HttpGet("ShowTaxCardInfo")]
        public async Task<IActionResult> ShowTaxCardInfoAsync(short month, short year)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && user.EmployeeId > 0)
                {
                    var taxCard = await _taxReportBusiness.TaxCardAsync(user.EmployeeId, 0, 0, month, year, true, user);
                    return Ok(taxCard);
                }
                return BadRequest(new { meassage = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReportController", "GetTaxcardInformation", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [Route("DownloadTaxCard"), HttpGet]
        public async Task<IActionResult> TaxCardReportAsync(long taxProcessId, long fiscalYearId, short month, short year, string format, string password, bool isPasswordProtected)
        {
            var user = AppUser();
            try
            {
                if (user.EmployeeId > 0 && user.HasBoth)
                {
                    var reportFile = await _taxRelatedReportGenerator.TaxCardReportAsync(user.EmployeeId, taxProcessId, fiscalYearId, month, year, true, format, password, isPasswordProtected, user);
                    if (reportFile != null && reportFile.FileBytes != null)
                    {
                        return File(reportFile.FileBytes, reportFile.Mimetype, "TaxCard." + reportFile.Extension);
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReportController", "TaxCardReport", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        #region Submission

        #region AIT
        [HttpPost("SaveAIT")]
        public async Task<IActionResult> SaveAITAsync([FromForm] AITSubmissionDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth && user.EmployeeId > 0)
                {
                    model.EmployeeId = user.EmployeeId;
                    const long maxSize = 300 * 1024;
                    if (model.File == null || model.File.Length <= maxSize)
                    {
                        var status = await _taxAITBusiness.SaveAsync(model, user);
                        return Ok(status);
                    }
                    else
                    {
                        return Ok(new ExecutionStatus()
                        {
                            Status = false,
                            Msg = "File size is greater than 300 KB"
                        });
                    }

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxSelfServiceController", "DownloadPayslipAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetAITInfos")]
        public async Task<IActionResult> GetAITInfosAsync([FromQuery] TaxDocumentQuery filter)
        {
            var user = AppUser();
            try
            {
                filter.EmployeeId = user.EmployeeId.ToString();
                if (ModelState.IsValid && user.HasBoth && user.EmployeeId > 0)
                {
                    var status = await _taxAITBusiness.GetIndividualEmployeeAsync(filter, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxSelfServiceController", "GetAITInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetAITById")]
        public async Task<IActionResult> GetAITByIdAsync([FromQuery] TaxDocumentQuery filter)
        {
            var user = AppUser();
            try
            {
                filter.EmployeeId = user.EmployeeId.ToString();
                if (ModelState.IsValid && user.HasBoth && user.EmployeeId > 0)
                {
                    var status = await _taxAITBusiness.GetSingleAsync(filter, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxSelfServiceController", "GetAITInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion

        #region Tax-Refund
        [HttpPost("SaveTaxRefund")]
        public async Task<IActionResult> SaveTaxRefundAsync([FromForm] TaxRefundSubmissionDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth && user.EmployeeId > 0)
                {
                    model.EmployeeId = user.EmployeeId;
                    const long maxSize = 300 * 1024;
                    if (model.File == null || model.File.Length <= maxSize)
                    {
                        var status = await _taxRefundBusiness.SaveAsync(model, user);
                        return Ok(status);
                    }
                    else
                    {
                        return Ok(new ExecutionStatus()
                        {
                            Status = false,
                            Msg = "File size is greater than 300 KB"
                        });
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxSelfServiceController", "SaveTaxRefundAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetTaxRefundInfos")]
        public async Task<IActionResult> GetTaxRefundInfos([FromQuery] TaxDocumentQuery filter)
        {
            var user = AppUser();
            try
            {
                filter.EmployeeId = user.EmployeeId.ToString();
                if (ModelState.IsValid && user.HasBoth && user.EmployeeId > 0)
                {
                    var status = await _taxRefundBusiness.GetIndividualEmployeeAsync(filter, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxSelfServiceController", "GetAITInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetTaxRefundById")]
        public async Task<IActionResult> GetTaxRefundByIdAsync([FromQuery] TaxDocumentQuery filter)
        {
            var user = AppUser();
            try
            {
                filter.EmployeeId = user.EmployeeId.ToString();
                if (ModelState.IsValid && user.HasBoth && user.EmployeeId > 0)
                {
                    var status = await _taxRefundBusiness.GetSingleAsync(filter, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxSelfServiceController", "GetTaxRefundByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion

        #endregion
    }
}
