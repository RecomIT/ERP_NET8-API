using API.Base;
using API.Services;
using BLL.Employee.Interface.Miscellaneous;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Shared.Employee.DTO.Lunch;
using Shared.Employee.Filter.Miscellaneous;
using Shared.Helpers;
using Shared.Services;

namespace API.Areas.Employee.Miscellaneous
{
    [ApiController, Area("hrms"), Route("api/[area]/[controller]"), Authorize]

    public class LunchRequestController : ApiBaseController
    {
        private readonly ILunchRequestService _lunchRequestService;
        private ExcelGenerator _excelGenerator;

        public LunchRequestController(
            ILunchRequestService lunchRequestService,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _lunchRequestService = lunchRequestService;
            _excelGenerator = new ExcelGenerator();
        }

        [HttpPost("create")]
        public IActionResult CreateLunchRequest([FromBody] LunchRequestDTO lunchRequestDto)
        {
            var user = AppUser();
            if(_lunchRequestService.IsLunchExist(lunchRequestDto.LunchDate, user) == false)
            {
                if (_lunchRequestService.CreateLunchRequest(lunchRequestDto, user))
                {
                    return Ok(new { message = "Lunch request created successfully", status = true });
                }
                return BadRequest(new { message = "Unable to create lunch request", status = false });
            }
            return BadRequest(new { message = "Already requested for lunch", status = false });

        }

        [HttpGet("request-exist")]
        public IActionResult RrequestExist(string date)
        {
            var user = AppUser();
            var converteredDate = Convert.ToDateTime(date);
            if (_lunchRequestService.IsLunchExist(converteredDate, user) == false)
            {
                return Ok(false);
            }
            return Ok(true);

        }

        [HttpPost("cancel/{id}")]
        public IActionResult CancelLunchRequest(long id)
        {
            if (_lunchRequestService.CancelLunchRequest(id))
            {
                return Ok(new { message = "Lunch request canceled successfully" });
            }
            return BadRequest(new { message = "Unable to cancel lunch request" });
        }

        [HttpGet("date/{date}")]
        public IActionResult GetLunchRequestsForDate(DateTime date)
        {
            var requests = _lunchRequestService.GetLunchRequestsForDate(date);
            return Ok(requests);
        }

        [HttpGet("employee/{id}/last5")]
        public IActionResult GetLastFiveRequestsForEmployee(long id)
        {
            var requests = _lunchRequestService.GetLastFiveRequestsForEmployee(id);
            return Ok(requests);
        }

        //[HttpGet("employee/{id}/month/{month}")]
        //public IActionResult GetMonthlyRequestCounts(long id, DateTime month)
        //{
        //    var (yesCount, noCount) = _lunchRequestService.GetMonthlyRequestCounts(id, month);
        //    return Ok(new { yesCount, noCount });
        //}

        //[HttpGet("{employeeId}/monthly-lunch-requests")]
        //public ActionResult GetMonthlyRequestCounts(long employeeId, int year, int month)
        //{
        //    var requestCounts = _lunchRequestService.GetMonthlyRequestCounts(employeeId, new DateTime(year, month, 1));

        //    // Return the counts in the response
        //    return Ok(new { YesCount = requestCounts.YesCount, NoCount = requestCounts.NoCount });
        //}


        //[HttpGet("{employeeId}/monthly-lunch-cost")]
        //public ActionResult<decimal> GetMonthlyLunchCost(long employeeId, int year, int month)
        //{
        //    var monthDate = new DateTime(year, month, 1);
        //    var cost = _lunchRequestService.GetMonthlyLunchCost(employeeId, monthDate);
        //    return Ok(new { totalCost = cost });
        //}

        [HttpPost("add-or-update")]
        public ActionResult AddOrUpdateLunchRate([FromBody] LunchRateDTO rateDto)
        {
            if (rateDto.Rate == null || rateDto.ValidFrom == null)
            {
                return BadRequest("Rate and ValidFrom are required.");
            }

            var success = _lunchRequestService.AddOrUpdateLunchRate(rateDto);
            if (success)
            {
                return Ok("Rate updated successfully.");
            }

            return StatusCode(500, "An error occurred while updating the rate.");
        }

        [HttpGet("current-rate/{date}")]
        public ActionResult<LunchRateDTO> GetLunchRate(DateTime date)
        {
            var rateDto = _lunchRequestService.GetLunchRateForDate(date);
            return Ok(rateDto);
        }

        [HttpGet("total-lunches")]
        public ActionResult<int> GetTotalLunchRequestsForDate(string date)
        {
            var ConverteredDate = Convert.ToDateTime(date);
            var totalRequests = _lunchRequestService.GetTotalLunchRequestsForDate(ConverteredDate);
            return Ok(new { totalLunches = totalRequests });
        }

        [HttpGet("GetLunchDetails")]
        public async Task<IActionResult> GetLunchDetailsAsync(string date)
        {
            var user = AppUser();
            if(date.IsNullEmptyOrWhiteSpace() == false && user.HasBoth)
            {
                var data = await _lunchRequestService.GetLunchDetailsAsync(date, user);
                return Ok(data);
            }
            return BadRequest(ResponseMessage.InvalidParameters);
            
        }

        [HttpGet, Route("DownloadLunchRequestSheet")]
        public async Task<IActionResult> DownloadLunchRequestSheetAsync([FromQuery] LunchRequestSheet_Filter model, string format = "xlsx")
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var data = await _lunchRequestService.LunchRequestReport(model, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "LunchRequestSheet");
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
                //await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "DownloadSalarySheetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
