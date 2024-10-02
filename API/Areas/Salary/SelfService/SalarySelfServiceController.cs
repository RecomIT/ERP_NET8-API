
using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using DAL.DapperObject.Interface;
using BLL.Salary.Salary.Interface;
using Shared.Payroll.Filter.Salary;
using BLL.Administration.Interface;
using Microsoft.AspNetCore.Authorization;


namespace API.Areas.Salary.SelfService
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SalarySelfServiceController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private SalaryRelatedReportGenerator _salaryRelatedReportGenerator;
        private readonly ITaxReportBusiness _taxReportBusiness;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private ExcelGenerator _excelGenerator;
        private ReportFile _reportFile;

        public SalarySelfServiceController(ISysLogger sysLogger, IWebHostEnvironment webHostEnvironment,
            ISalaryReportBusiness salaryReportBusiness, ITaxReportBusiness taxReportBusiness, IReportConfigBusiness reportConfigBusiness, IClientDatabase clientDatabase, IBranchInfoBusiness branchInfoBusiness) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _webHostEnvironment = webHostEnvironment;
            _salaryReportBusiness = salaryReportBusiness;
            _reportConfigBusiness = reportConfigBusiness;
            _taxReportBusiness = taxReportBusiness;
            _excelGenerator = new ExcelGenerator();
            _branchInfoBusiness = branchInfoBusiness;
            _salaryRelatedReportGenerator = new SalaryRelatedReportGenerator(_webHostEnvironment, _sysLogger, _reportConfigBusiness, _salaryReportBusiness, _branchInfoBusiness);

        }

        #region Payslip
        [HttpGet("ShowPayslip")]
        public async Task<IActionResult> ShowPayslipAsync(short month, short year)
        {
            var user = AppUser();
            try
            {
                if (user.EmployeeId > 0 && month > 0 && year > 0 && user.HasBoth)
                {
                    var data = await _salaryReportBusiness.SelfPayslipExtensionAsync(user.EmployeeId, month, year, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "PayslipExtensionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadPayslip")]
        public async Task<IActionResult> DownloadPayslipAsync([FromQuery] Payslip_Filter filter)
        {
            var user = AppUser();
            filter.EmployeeId = user.EmployeeId.ToString();
            filter.IsDisbursed = "1";
            try
            {
                var payslip_file = await _salaryRelatedReportGenerator.GetPaySlip(filter, null, false, user);
                if (payslip_file != null && payslip_file.FileBytes != null)
                {
                    return File(payslip_file.FileBytes, payslip_file.Mimetype, "payslip." + payslip_file.Extension);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "DownloadPayslipAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadPayslipByDateRange")]
        public async Task<IActionResult> DownloadPayslipByDateRangeAsync([FromQuery] Payslip_Filter filter)
        {
            var user = AppUser();
            filter.EmployeeId = user.EmployeeId.ToString();
            filter.IsDisbursed = "1";
            try
            {
                var payslip_file = await _salaryRelatedReportGenerator.GetPaySlip(filter, null, false, user);
                if (payslip_file != null && payslip_file.FileBytes != null)
                {
                    return File(payslip_file.FileBytes, payslip_file.Mimetype, "payslip." + payslip_file.Extension);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "DownloadPayslipAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadSalarySheet")]
        public async Task<IActionResult> DownloadSalarySheetAsync([FromQuery] SalarySheet_Filter model, string format = "xlsx")
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && user.EmployeeId > 0)
                {
                    model.EmployeeId = user.EmployeeId.ToString();
                    model.IsDisbursed = "1";
                    var data = await _salaryReportBusiness.GetSalarySheetReport(model, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "SalarySheet");
                    format = format == "xlsx" || format == "xls" ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes)))
                    {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null)
                        {
                            int lastColumnIndex = worksheet.Dimension.End.Column;
                            int totalColumns = worksheet.Dimension.Columns;

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
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "DownloadSalarySheetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        #endregion

    }
}
