using API.Base;
using API.Services;
using BLL.Administration.Interface;
using BLL.Base.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Process.Tax;
using Shared.Payroll.ViewModel.Tax;
using Shared.Services;

namespace API.Areas.Tax
{

    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxProcessController : ApiBaseController
    {
        private readonly ITaxProcessBusiness _taxProcessBusiness;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly ExcelGenerator _excelGenerator;
        private readonly ISysLogger _sysLogger;
        private readonly IExecuteTaxProcess _executeTaxProcess;
        public TaxProcessController(
            IReportConfigBusiness reportConfigBusiness,
            ISysLogger sysLogger,
            ITaxProcessBusiness taxProcessBusiness,
            IExecuteTaxProcess executeTaxProcess,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _excelGenerator = new ExcelGenerator();
            _taxProcessBusiness = taxProcessBusiness;
            _reportConfigBusiness = reportConfigBusiness;
            _executeTaxProcess = executeTaxProcess;
        }

        [HttpPost("Process")]
        public async Task<IActionResult> TaxProcessAsync(TaxProcessViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var fiscalYearRange = ReportingHelper.FiscalYearRange(model.SalaryMonth, model.SalaryYear);
                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("TaxCard", fiscalYearRange, user.CompanyId, user.OrganizationId);
                    var data = await _taxProcessBusiness.TaxProcessAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessController", "TaxProcessAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("ExecuteTaxProcess")]
        public async Task<IActionResult> ExecuteTaxProcessAsync(TaxProcessExecution model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _executeTaxProcess.SalaryTaxProcessAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessController", "TaxProcessAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetTaxProcessSummeryInfos")]
        public async Task<IActionResult> GetTaxProcessSummeryInfosAsync(long? fiscalYearId, short? month, short? year)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _taxProcessBusiness.GetTaxProcessSummeryInfosAsync(fiscalYearId ?? 0, month ?? 0, year ?? 0, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessController", "GetTaxProcessSummeryInfos", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetEmployeeTaxProcessDetailInfos")]
        public async Task<IActionResult> GetEmployeeTaxProcessDetailInfosAsync(long? employeeId, long fiscalYearId, long? branchId, long? salaryProcessId, short month, short year)
        {
            var user = AppUser();
            try
            {
                if (month > 0 && year > 0 && user.HasBoth)
                {
                    var data = await _taxProcessBusiness.GetEmployeeTaxProcesDetailInfosAsync(employeeId ?? 0, fiscalYearId, branchId, salaryProcessId ?? 0, month, year, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessController", "GetEmployeeTaxProcessDetailInfos", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("DeleteTaxInfo")]
        public async Task<IActionResult> DeleteTaxInfoAsync(DeleteTaxInfoDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _taxProcessBusiness.DeleteTaxProcessAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessController", "DeleteTaxInfo", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadIncomeTaxChallanFormat")]
        public async Task<IActionResult> DownloadIncomeTaxChallanFormatAsync()
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel\\TaxChallanUploadFormat.xlsx");
            string contentType = "";
            if (System.IO.File.Exists(filepath))
            {
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contentType, "Tax-Challan-Uploader.xlsx");
        }

        
    }
}
