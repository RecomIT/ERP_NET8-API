using API.Base;
using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.PF.ExternalResourse
{
    [ApiController, Area("Fund"), Route("api/[area]/[controller]"), Authorize]
    public class ExternalController : ApiBaseController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly ISysLogger _sysLogger;
        public ExternalController(ISysLogger sysLogger, IHttpClientFactory httpClientFactory, IConfiguration config, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _httpClientFactory = httpClientFactory;
            _config = config;
            _sysLogger = sysLogger;

        }

        [HttpGet, Route("DownloadPFSummary")]
        public async Task<IActionResult> GetPFSummaryPdfAsync(string employeeCode, string fromDate, string toDate)
        {
            var user = AppUser();
            try
            {
                var apiUrl = "";
                if (user.CompanyId == 19 && user.OrganizationId == 11)
                {
                    apiUrl = $"{_config.GetSection("WTSPFSummaryUri").Value.ToString()}?empId={employeeCode}&fromDate={fromDate}&toDate={toDate}";
                }
                else
                {
                    apiUrl = $"{_config.GetSection("PFSummaryUri").Value.ToString()}?empId={employeeCode}&fromDate={fromDate}&toDate={toDate}";
                }

                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var pdfStream = await response.Content.ReadAsStreamAsync();
                    return new FileStreamResult(pdfStream, "application/pdf");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExternalController", "GetPFSummaryPdfAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
            return NotFound();
        }

        [HttpGet, Route("DownloadPFLetter")]
        public async Task<IActionResult> GetPFCardLetterPdfAsync(string employeeCode, string fromDate, string toDate)
        {
            var user = AppUser();
            try
            {
                var apiUrl = "";
                if (user.CompanyId == 19 && user.OrganizationId == 11)
                { // Wunderman Thompson
                    apiUrl = $"{_config.GetSection("WTSPFCardDateWiseLetterUri").Value.ToString()}?empId={employeeCode}&fromDate={fromDate}&toDate={toDate}";
                }
                else
                {
                    apiUrl = $"{_config.GetSection("PFCardDateWiseLetterUri").Value.ToString()}?empId={employeeCode}&fromDate={fromDate}&toDate={toDate}";
                }
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var pdfStream = await response.Content.ReadAsStreamAsync();
                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExternalController", "GetPFCardLetterPdfAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet, Route("DownloadLoanCard")]
        public async Task<IActionResult> GetLoanCardAsync(string employeeCode)
        {
            var user = AppUser();
            try
            {
                var apiUrl = "";
                if (user.CompanyId == 19 && user.OrganizationId == 11)
                { // Wunderman Thompson
                    apiUrl = $"{_config.GetSection("WTSLoanCard").Value.ToString()}?empId={employeeCode}";
                }
                else
                {
                    apiUrl = $"{_config.GetSection("LoanCard").Value.ToString()}?empId={employeeCode}";
                }
                var httpClient = _httpClientFactory.CreateClient();
                var response = await httpClient.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var pdfStream = await response.Content.ReadAsStreamAsync();
                    return new FileStreamResult(pdfStream, "application/pdf");
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExternalController", "GetPFCardLetterPdfAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }
    }
}
