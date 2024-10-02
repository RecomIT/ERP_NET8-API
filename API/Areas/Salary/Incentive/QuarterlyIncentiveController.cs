using API.Base;
using BLL.Administration.Interface;
using BLL.Base.Interface;
using BLL.Salary.Incentive.Interface;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using Microsoft.Reporting.NETCore;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Shared.Payroll.Filter.Incentive.QuarterlyIncentive;
using Shared.Payroll.ViewModel.Incentive.QuarterlyIncentive;

namespace API.Areas.Salary.Incentive
{

    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class QuarterlyIncentiveController : ApiBaseController
    {
        private readonly ExcelGenerator _excelGenerator;

        private readonly IQuarterlyIncentiveBusiness _quarterlyIncentiveBusiness;

        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly ISysLogger _sysLogger;

        public QuarterlyIncentiveController(IClientDatabase clientDatabase,
            IQuarterlyIncentiveBusiness quarterlyIncentiveBusiness,
            IWebHostEnvironment webHostEnvironment,
            ISalaryReportBusiness salaryReportBusiness,
            IReportConfigBusiness reportConfigBusiness,
            ISysLogger sysLogger
            ) : base(clientDatabase)
        {
            _webHostEnvironment = webHostEnvironment;
            _salaryReportBusiness = salaryReportBusiness;
            _reportConfigBusiness = reportConfigBusiness;
            _quarterlyIncentiveBusiness = quarterlyIncentiveBusiness;
            _excelGenerator = new ExcelGenerator();
            _sysLogger = sysLogger;
        }



        [HttpGet, Route("QuarterlyIncentiveExcelFormatDownload")]
        public async Task<IActionResult> QuarterlyIncentiveExcelFormatDownloadAsync(string fileName)
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



        [HttpPost, Route("UploadQuarterlyIncentiveExcel")]
        public async Task<IActionResult> UploadQuarterlyIncentiveExcelAsync([FromForm] UploadExcel uploadedFile)
        {
            try
            {
                var appUser = AppUser();

                if (ModelState.IsValid)
                {
                    if (uploadedFile.ExcelFile?.Length > 0)
                    {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<QuarterlyIncentiveProcessDetailsViewModel> readExcelModels = new List<QuarterlyIncentiveProcessDetailsViewModel>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            var rowCount = worksheet!.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var code = worksheet.Cells[row, 1].Value?.ToString();
                                if (code != null)
                                {
                                    QuarterlyIncentiveProcessDetailsViewModel readModel = new QuarterlyIncentiveProcessDetailsViewModel();

                                    var employeeCode = worksheet.Cells[row, 1].Value.ToString();
                                    var kpiScore = Convert.ToDecimal(worksheet.Cells[row, 2].Value?.ToString());
                                    var divisionalScore = Convert.ToDecimal(worksheet.Cells[row, 3].Value?.ToString());
                                    readModel.EmployeeCode = employeeCode;
                                    readModel.kpiScore = kpiScore;
                                    readModel.DivisionalAndIndividualScore = divisionalScore;

                                    readModel.IncentiveYear = uploadedFile.IncentiveYear;
                                    readModel.IncentiveQuarterNoId = uploadedFile.IncentiveQuarterNoId;
                                    readModel.BatchNo = uploadedFile.BatchNo;
                                    readExcelModels.Add(readModel);

                                }
                                else
                                {
                                    continue;
                                }

                            }

                        }
                        var data = await _quarterlyIncentiveBusiness.UploadQuarterlyIncentiveAsync(readExcelModels, appUser);
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



        [HttpGet("GetQuarterlyIncenitveYear")]
        public async Task<IActionResult> GetQuarterlyIncenitveYearAsync()
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _quarterlyIncentiveBusiness.GetQuarterlyIncenitveYearAsync(user);
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


        [HttpGet("GetQuarterlyIncenitveQuarter")]
        public async Task<IActionResult> GetQuarterlyIncenitveQuarterAsync([FromQuery] Quarter_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _quarterlyIncentiveBusiness.GetQuarterlyIncenitveQuarterAsync(filter, user);
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

        [HttpGet("GetQuarterlyIncenitveNumber")]
        public async Task<IActionResult> GetQuarterlyIncenitveNumberAsync([FromQuery] Quarter_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _quarterlyIncentiveBusiness.GetQuarterlyIncenitveNumberAsync(filter, user);
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

        [HttpGet("GetQuarterlyIncenitveEmployees")]
        public async Task<IActionResult> GetQuarterlyIncenitveEmployeesAsync([FromQuery] QuarterlyIncentiveEmployee_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _quarterlyIncentiveBusiness.GetQuarterlyIncenitveEmployees(filter, user);
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


        [HttpGet, Route("GetQuarterlyIncentive")]
        public async Task<IActionResult> GetQuarterlyIncentiveAsync([FromQuery] QuarterlyIncentive_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data_list = await _quarterlyIncentiveBusiness.GetQuarterlyIncentiveAsync(filter, user);
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

        [HttpGet("GetQuarterlyIncentiveDetail")]
        public async Task<IActionResult> GetQuarterlyIncentiveDetail([FromQuery] QuarterlyIncentiveDetail_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _quarterlyIncentiveBusiness.GetQuarterlyIncentiveDetailAsync(filter, user);
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


        [HttpGet("DeleteQuarterlyIncentiveProcess")]
        public async Task<IActionResult> DeleteQuarterlyIncentiveProcessAsync([FromQuery] DeleteQuarterlyIncentiveProcess_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _quarterlyIncentiveBusiness.DeleteQuarterlyIncentiveProcessAsync(filter, user);
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

        [HttpGet("UndoOrDisbursedQuarterlyIncentiveProcess")]
        public async Task<IActionResult> UndoOrDisbursedQuarterlyIncentiveProcessAsync([FromQuery] UndoOrDisbursed_Filter filter)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _quarterlyIncentiveBusiness.UndoOrDisbursedQuarterlyIncentiveProcessAsync(filter, user);
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


        [HttpGet("UpdateQuarterlyIncentiveDetail")]
        public async Task<IActionResult> UpdateQuarterlyIncentiveDetailAsync([FromQuery] QuarterlyIncentiveProcessDetailsUpdate detailsUpdate)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    var allData = await _quarterlyIncentiveBusiness.UpdateQuarterlyIncentiveDetailAsync(detailsUpdate, user);
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


        [HttpGet, Route("QuarterlyIncentiveReport")]
        public async Task<IActionResult> QuarterlyIncentiveReportAsync([FromQuery] QuarterlyIncentiveReport_Filter filter)
        {
            var user = AppUser();
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";
                var path = "";
                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("QuarterlyIncentiveSlip", "", user.CompanyId, user.OrganizationId);
                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Nagad\\incentive\\payroll_QuarterlyIncentiveSlip.rdlc";
                }

                LocalReport localReport = new LocalReport();

                var reportData = await _quarterlyIncentiveBusiness.GetQuarterlyIncentiveReportAsync(filter, user);

                if (reportData.Rows.Count > 0)
                {
                    reportData.Columns.Add("AmountToWord");
                    for (int i = 0; i < reportData.Rows.Count; i++)
                    {
                        reportData.Rows[i]["AmountToWord"] = NumberToWords.Input(Convert.ToDecimal(reportData.Rows[i]["NetPay"]));
                    }
                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, 0);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    ReportDataSource reportinfo = new ReportDataSource();
                    reportinfo.Name = "ReportLayer";
                    reportinfo.Value = reportLayers;

                    ReportDataSource quarterlyIncentive = new ReportDataSource();
                    quarterlyIncentive.Name = "QuarterlyIncentive";
                    quarterlyIncentive.Value = reportData;

                    localReport.DataSources.Clear();

                    localReport.DataSources.Add(reportinfo);
                    localReport.DataSources.Add(quarterlyIncentive);

                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    return File(pdf, mimetype, "QuarterlyIncentive." + extension);
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


        [HttpGet, Route("GetQuarterlyIncenticeExcelReport")]
        public async Task<IActionResult> GetQuarterlyIncenticeExcelReportAsync([FromQuery] DownloadExcel_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var data = await _quarterlyIncentiveBusiness.GetQuarterlyIncenitveExcel(filter, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "QuaterlyIncentive");

                    filter.Format = filter.Format == "xlsx" || filter.Format == "xls" ? filter.Format : "xlsx";
                    string fileName = "data." + filter.Format;
                    string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;

                    // Load the Excel package from the byte array
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes)))
                    {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null)
                        {
                            int lastColumnIndex = worksheet.Dimension.End.Column;

                            // Set the format of the last column to Accounting
                            var lastColumn = worksheet.Cells[worksheet.Dimension.Start.Row, lastColumnIndex, worksheet.Dimension.End.Row, lastColumnIndex];
                            lastColumn.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
                        }
                        else
                        {
                            // Handle the case where the worksheet doesn't exist
                            return BadRequest(new { message = "Worksheet not found in the Excel package." });
                        }

                        // Save the modified Excel package back to a byte array
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

        [HttpGet, Route("GetQuarterNumberExtension")]
        public async Task<IActionResult> GetQuarterNumberExtensionAsync(short incentiveYear)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _quarterlyIncentiveBusiness.GetQuarterNumberExtensionAsync(incentiveYear, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }

        }

        [HttpGet, Route("GetBatchNoExtension")]
        public async Task<IActionResult> GetBatchNoExtensionAsync(short incentiveYear, long? incentiveQuarterNoId)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _quarterlyIncentiveBusiness.GetBatchNoExtensionAsync(incentiveYear, incentiveQuarterNoId, appUser);
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
