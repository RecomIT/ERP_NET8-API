using API.Base;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using DAL.DapperObject.Interface;
using Microsoft.Reporting.NETCore;
using BLL.Salary.Salary.Interface;
using BLL.Administration.Interface;
using BLL.Salary.Incentive.Interface;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Shared.Payroll.DTO.Incentive.MonthlyIncentive;
using Shared.Payroll.Filter.Incentive.MonthlyIncentive;
using Shared.Payroll.ViewModel.Incentive.MonthlyIncentive;

namespace API.Areas.Salary.Incentive
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class MonthlyIncentiveController : ApiBaseController
    {
        private readonly ExcelGenerator _excelGenerator;
        private readonly IMonthlyIncentiveBusiness _monthlyIncentiveBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly ISysLogger _sysLogger;

        public MonthlyIncentiveController(IClientDatabase clientDatabase,
            IMonthlyIncentiveBusiness monthlyIncentiveBusiness,
            IWebHostEnvironment webHostEnvironment,
            ISalaryReportBusiness salaryReportBusiness,
            IReportConfigBusiness reportConfigBusiness,
            ISysLogger sysLogger
            ) : base(clientDatabase)
        {
            _webHostEnvironment = webHostEnvironment;
            _salaryReportBusiness = salaryReportBusiness;
            _reportConfigBusiness = reportConfigBusiness;
            _monthlyIncentiveBusiness = monthlyIncentiveBusiness;
            _excelGenerator = new ExcelGenerator();
            _sysLogger = sysLogger;
        }


        [HttpGet, Route("DownloadMonthlyIncentiveExcel")]
        public async Task<IActionResult> DownloadMonthlyIncentiveExcelAsync(string fileName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel", fileName);
            var provider = new FileExtensionContentTypeProvider();
            string contenttype = "";
            if (System.IO.File.Exists(filepath))
            {
                contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, fileName);
        }

        [HttpPost, Route("UploadMonthlyIncentiveExcel")]
        public async Task<IActionResult> UploadMonthlyIncentiveExcelAsync([FromForm] UploadMonthlyIncentiveProcessDTO uploadedFile)
        {
            try
            {
                var appUser = AppUser();
                if (ModelState.IsValid)
                {
                    if (uploadedFile.ExcelFile?.Length > 0)
                    {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<MonthlyIncentiveProcessDetailViewModel> list = new List<MonthlyIncentiveProcessDetailViewModel>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var code = worksheet.Cells[row, 1].Value.ToString();
                                if (code != null)
                                {
                                    MonthlyIncentiveProcessDetailViewModel model = new MonthlyIncentiveProcessDetailViewModel();
                                    var employeeCode = worksheet.Cells[row, 1].Value.ToString();
                                    var adjustedKpiPerformanceScore = worksheet.Cells[row, 2].Value?.ToString();
                                    var essauRating = worksheet.Cells[row, 3].Value?.ToString();
                                    var attendanceAdherenceQualityScore = worksheet.Cells[row, 4].Value?.ToString();
                                    var adjustment = worksheet.Cells[row, 5].Value?.ToString();

                                    model.EmployeeCode = employeeCode;
                                    model.AdjustedKpiPerformanceScore = Convert.ToDecimal(adjustedKpiPerformanceScore); ;
                                    model.ESSAURating = essauRating;
                                    model.AttendanceAdherenceQualityScore = Convert.ToDecimal(attendanceAdherenceQualityScore);
                                    model.Adjustment = Convert.ToDecimal(adjustment);
                                    model.IncentiveMonth = uploadedFile.IncentiveMonth;
                                    model.IncentiveYear = uploadedFile.IncentiveYear;
                                    model.BatchNo = uploadedFile.BatchNo;
                                    list.Add(model);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        var data = await _monthlyIncentiveBusiness.UploadMonthlyIncentiveExcelAsync(list, appUser);
                        return Ok(data);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetBatchNoExtension")]
        public async Task<IActionResult> GetBatchNoExtensionAsync(short incentiveYear, long incentiveMonth)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _monthlyIncentiveBusiness.GetBatchNoExtensionAsync(incentiveYear, incentiveMonth, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet("GetMonthlyIncentiveYearExtension")]
        public async Task<IActionResult> GetMonthlyIncentiveYearExtensionAsync(short incentiveYear)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _monthlyIncentiveBusiness.GetMonthlyIncentiveYearExtensionAsync(incentiveYear, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetMonthlyIncentiveMonthExtension")]
        public async Task<IActionResult> GetMonthlyIncentiveMonthExtensionAsync(short incentiveYear, short incentiveMonth)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _monthlyIncentiveBusiness.GetMonthlyIncentiveMonthExtensionAsync(incentiveYear, incentiveMonth, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetMonthlyIncentiveList")]
        public async Task<IActionResult> GetQuarterlyIncentiveAsync([FromQuery] MonthlyIncentiveProcess_Filter incentiveProcess_Filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data_list = await _monthlyIncentiveBusiness.GetMonthlyIncentiveListAsync(incentiveProcess_Filter, user);
                    Response.AddPagination(data_list.Pageparam.PageNumber, data_list.Pageparam.PageSize, data_list.Pageparam.TotalRows, data_list.Pageparam.TotalPages);
                    return Ok(data_list.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "QuarterlyIncentiveController", "GetQuarterlyIncentiveAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet("GetMonthlyIncentiveDetail")]
        public async Task<IActionResult> GetMonthlyIncentiveDetailAsync([FromQuery] MonthlyIncentiveDetail_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _monthlyIncentiveBusiness.GetMonthlyIncentiveDetailAsync(filter, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("UpdateMonthlyIncentiveDetail")]
        public async Task<IActionResult> UpdateMonthlyIncentiveDetailAsync([FromQuery] UpdateMonthlyIncentiveProcessDetails_Filter processDetails_Filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _monthlyIncentiveBusiness.UpdateMonthlyIncentiveDetailAsync(processDetails_Filter, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("DeleteMonthlyIncentiveDetail")]
        public async Task<IActionResult> DeleteMonthlyIncentiveDetailAsync([FromQuery] DeleteMonthlyIncentiveProcess_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _monthlyIncentiveBusiness.DeleteMonthlyIncentiveDetailAsync(filter, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("UndoOrDisbursedMonthlyIncentiveProcess")]
        public async Task<IActionResult> UndoOrDisbursedMonthlyIncentiveProcessAsync([FromQuery] MonthlyIncentiveUndoOrDisbursed_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _monthlyIncentiveBusiness.UndoOrDisbursedMonthlyIncentiveProcessAsync(filter, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }

        }


        [HttpGet("GetMonthlyIncentiveEmployeesExtension")]
        public async Task<IActionResult> GetMonthlyIncentiveEmployeesExtensionAsync(short incentiveYear, short incentiveMonth, long? employeeIdForSearch)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _monthlyIncentiveBusiness.GetMonthlyIncentiveEmployeesExtensionAsync(incentiveYear, incentiveMonth, employeeIdForSearch, user);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet, Route("GetMonthlyIncentiveExcelReport")]
        public async Task<IActionResult> GetMonthlyIncentiveExcelReportAsync([FromQuery] MonthlyIncentiveDownloadExcel_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _monthlyIncentiveBusiness.GetMonthlyIncenitveExcelAsync(filter, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "MonthlyIncentive");
                    filter.Format = filter.Format == "xlsx" || filter.Format == "xls" ? filter.Format : "xlsx";
                    string fileName = "data." + filter.Format;
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
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetMonthlyIncentiveReport")]
        public async Task<IActionResult> GetMonthlyIncentiveReportAsync([FromQuery] MonthlyIncentiveReport_Filter filter)
        {
            var user = AppUser();
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";
                var path = "";

                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("MonthlyIncentiveSlip", "", user.CompanyId, user.OrganizationId);

                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Nagad\\incentive\\payroll_MonthlyIncentiveSlip.rdlc";
                }

                LocalReport localReport = new LocalReport();

                var reportData = await _monthlyIncentiveBusiness.GetMonthlyIncentiveReportAsync(filter, user);

                if (reportData.Rows.Count > 0)
                {
                    reportData.Columns.Add("AmountToWord");
                    reportData.Columns.Add("MonthName");
                    for (int i = 0; i < reportData.Rows.Count; i++)
                    {
                        reportData.Rows[i]["AmountToWord"] = NumberToWords.Input(Convert.ToDecimal(reportData.Rows[i]["NetPay"]));
                        reportData.Rows[i]["MonthName"] = Utility.GetMonthName(Convert.ToInt16(reportData.Rows[i]["IncentiveMonth"]));
                    }
                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, 0);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    ReportDataSource reportinfo = new ReportDataSource();
                    reportinfo.Name = "ReportLayer";
                    reportinfo.Value = reportLayers;

                    ReportDataSource monthlyIncentive = new ReportDataSource();
                    monthlyIncentive.Name = "MonthlyIncentive";
                    monthlyIncentive.Value = reportData;

                    localReport.DataSources.Clear();

                    localReport.DataSources.Add(reportinfo);
                    localReport.DataSources.Add(monthlyIncentive);

                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    return File(pdf, mimetype, "MonthlyIncentive." + extension);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Somthing went wrong");
            }
        }

    }
}
