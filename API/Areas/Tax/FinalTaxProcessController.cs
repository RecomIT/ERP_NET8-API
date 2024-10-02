using API.Base;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using Shared.Payroll.DTO.Tax;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class FinalTaxProcessController : ApiBaseController
    {
        private readonly IFinalTaxProcessBusiness _finalTaxProcessBusiness;
        private readonly ISysLogger _sysLogger;
        private ExcelGenerator _excelGenerator;
        public FinalTaxProcessController(
            IFinalTaxProcessBusiness finalTaxProcessBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase
            ) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _finalTaxProcessBusiness = finalTaxProcessBusiness;
            _excelGenerator = new ExcelGenerator();
        }

        [HttpPost("RunProcess")]
        public async Task<IActionResult> RunProcessAsync(FinalTaxProcessDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var process = await _finalTaxProcessBusiness.FinalTaxProcessAsync(model, user);
                    if (process.Status)
                    {
                        return Ok(process);
                    }
                    else
                    {
                        return BadRequest(process);
                    }
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployeesAsync(long fiscalYear, string flag, long branchId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && fiscalYear > 0 && flag.IsNullEmptyOrWhiteSpace() == false)
                {
                    var data = await _finalTaxProcessBusiness.GetEmployeesAsync(fiscalYear, flag, branchId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetFinalTaxProcessSummary")]
        public async Task<IActionResult> GetFinalTaxProcessSummaryAsync(long fiscalYearId, long branchId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _finalTaxProcessBusiness.GetFinalTaxProcessSummaryAsync(fiscalYearId, branchId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetFinalTaxProcesSummaries")]
        public async Task<IActionResult> GetFinalTaxProcessDetails(long employeeId, long fiscalYearId, long branchId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    if (fiscalYearId == 0)
                    {
                        return BadRequest("Income is missing");
                    }
                    if (branchId == 0)
                    {
                        return BadRequest("Branch is missing");
                    }
                    var data = await _finalTaxProcessBusiness.GetFinalTaxProcesSummariesAsync(employeeId, fiscalYearId, branchId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.CompanyIdentityMissing);
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("Download108Report")]
        public async Task<IActionResult> Download108Report(long fiscalYearId, long branchId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    if(fiscalYearId == 0)
                    {
                        return BadRequest("Fiscal year is required" );
                    }
                    var data = await _finalTaxProcessBusiness.Download108Report(fiscalYearId,branchId, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "108 Report");
                    string fileName = "data.xlsx";
                    string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;

                    using (var package = new ExcelPackage(new MemoryStream(excelBytes)))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        excelBytes = package.GetAsByteArray();
                    }

                    HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
                    HttpContext.Response.ContentType = contentType;
                    return File(excelBytes, contentType);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessController", "DownloadTaxSheetDetailsAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
