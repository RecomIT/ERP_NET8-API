
using API.Base;
using API.Services;
using BLL.Administration.Interface;
using BLL.Asset.Report.Interface;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Shared.Asset.Filter.Report;
using Shared.OtherModels.Report;
using Shared.Services;
using System.Drawing;


namespace API.Asset.Report
{
    [ApiController, Area("Asset"), Route("api/[area]/Report/[controller]"), Authorize]
    public class ReportController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IReportBusiness _reportBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportConfigBusiness _reportConfigBusiness;


        private ExcelGenerator _excelGenerator;
        private ReportFile _reportFile;

        public ReportController(ISysLogger sysLogger,
            IWebHostEnvironment webHostEnvironment,
            IReportBusiness reportBusiness,
            IReportConfigBusiness reportConfigBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _reportBusiness = reportBusiness;
            _webHostEnvironment = webHostEnvironment;
            _reportConfigBusiness = reportConfigBusiness;
            _excelGenerator = new ExcelGenerator();

        }

        [HttpGet, Route("DownloadAssetReport")]
        public async Task<IActionResult> DownloadAssetReportAsync([FromQuery] Report_Filter model, string reportName, string format = "xlsx")
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {

                    var data = await _reportBusiness.GetAssetReport(model, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcelAsset(data, reportName);

                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes))) {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null) {

                            worksheet.InsertRow(1, 1);
                            // Set the report title in the newly inserted row
                            worksheet.Cells["A1"].Value = reportName;
                            var titleCell = worksheet.Cells["A1"];

                            // Apply styling to the title cell
                            titleCell.Style.Font.Bold = true;
                            titleCell.Style.Font.Size = 18;
                            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            titleCell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            titleCell.Style.Fill.BackgroundColor.SetColor(Color.White);

                            // Merge all cells in the "Report Title" row
                            int columnCount = worksheet.Dimension.End.Column;
                            worksheet.Cells[1, 1, 1, columnCount].Merge = true;

                            // Align numeric value columns to center
                            foreach (var cell in worksheet.Cells) {
                                if (cell.Value is double || cell.Value is int || cell.Value is float || cell.Value is decimal) {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }

                        }
                        else {
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
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DownloadAssetReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadAssigningReport")]
        public async Task<IActionResult> DownloadAssetAssigningReportAsync([FromQuery] Report_Filter model, string reportName, string format = "xlsx")
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {

                    var data = await _reportBusiness.GetAssetAssigningReport(model, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, reportName);

                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes))) {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null) {

                            worksheet.InsertRow(1, 1);
                            // Set the report title in the newly inserted row
                            worksheet.Cells["A1"].Value = reportName;
                            var titleCell = worksheet.Cells["A1"];

                            // Apply styling to the title cell
                            titleCell.Style.Font.Bold = true;
                            titleCell.Style.Font.Size = 18;
                            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            titleCell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            titleCell.Style.Fill.BackgroundColor.SetColor(Color.White);

                            // Merge all cells in the "Report Title" row
                            int columnCount = worksheet.Dimension.End.Column;
                            worksheet.Cells[1, 1, 1, columnCount].Merge = true;

                            // Align numeric value columns to center
                            foreach (var cell in worksheet.Cells) {
                                if (cell.Value is double || cell.Value is int || cell.Value is float || cell.Value is decimal) {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }

                        }
                        else {
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
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DownloadAssetReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadServicingReport")]
        public async Task<IActionResult> DownloadServicingReportAsync([FromQuery] Report_Filter model, string reportName, string format = "xlsx")
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {

                    var data = await _reportBusiness.GetServicingReport(model, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, reportName);

                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes))) {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null) {

                            worksheet.InsertRow(1, 1);
                            // Set the report title in the newly inserted row
                            worksheet.Cells["A1"].Value = reportName;
                            var titleCell = worksheet.Cells["A1"];

                            // Apply styling to the title cell
                            titleCell.Style.Font.Bold = true;
                            titleCell.Style.Font.Size = 18;
                            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            titleCell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            titleCell.Style.Fill.BackgroundColor.SetColor(Color.White);

                            // Merge all cells in the "Report Title" row
                            int columnCount = worksheet.Dimension.End.Column;
                            worksheet.Cells[1, 1, 1, columnCount].Merge = true;

                            // Align numeric value columns to center
                            foreach (var cell in worksheet.Cells) {
                                if (cell.Value is double || cell.Value is int || cell.Value is float || cell.Value is decimal) {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }

                        }
                        else {
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
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DownloadAssetReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadReplacementReport")]
        public async Task<IActionResult> DownloadReplacementReportAsync([FromQuery] Report_Filter model, string reportName, string format = "xlsx")
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {

                    var data = await _reportBusiness.GetReplacementReport(model, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, reportName);

                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes))) {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null) {

                            worksheet.InsertRow(1, 1);
                            // Set the report title in the newly inserted row
                            worksheet.Cells["A1"].Value = reportName;
                            var titleCell = worksheet.Cells["A1"];

                            // Apply styling to the title cell
                            titleCell.Style.Font.Bold = true;
                            titleCell.Style.Font.Size = 18;
                            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            titleCell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            titleCell.Style.Fill.BackgroundColor.SetColor(Color.White);

                            // Merge all cells in the "Report Title" row
                            int columnCount = worksheet.Dimension.End.Column;
                            worksheet.Cells[1, 1, 1, columnCount].Merge = true;

                            // Align numeric value columns to center
                            foreach (var cell in worksheet.Cells) {
                                if (cell.Value is double || cell.Value is int || cell.Value is float || cell.Value is decimal) {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }

                        }
                        else {
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
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DownloadAssetReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadHandoverReport")]
        public async Task<IActionResult> DownloadHandoverReportAsync([FromQuery] Report_Filter model, string reportName, string format = "xlsx")
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {

                    var data = await _reportBusiness.GetHandoverReport(model, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, reportName);

                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes))) {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null) {

                            worksheet.InsertRow(1, 1);
                            // Set the report title in the newly inserted row
                            worksheet.Cells["A1"].Value = reportName;
                            var titleCell = worksheet.Cells["A1"];

                            // Apply styling to the title cell
                            titleCell.Style.Font.Bold = true;
                            titleCell.Style.Font.Size = 18;
                            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            titleCell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            titleCell.Style.Fill.BackgroundColor.SetColor(Color.White);

                            // Merge all cells in the "Report Title" row
                            int columnCount = worksheet.Dimension.End.Column;
                            worksheet.Cells[1, 1, 1, columnCount].Merge = true;

                            // Align numeric value columns to center
                            foreach (var cell in worksheet.Cells) {
                                if (cell.Value is double || cell.Value is int || cell.Value is float || cell.Value is decimal) {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }

                        }
                        else {
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
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DownloadAssetReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadDamageReport")]
        public async Task<IActionResult> DownloadDamageReportAsync([FromQuery] Report_Filter model, string reportName, string format = "xlsx")
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {

                    var data = await _reportBusiness.GetDamageReport(model, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, reportName);

                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes))) {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null) {
                            int lastColumnIndex = worksheet.Dimension.End.Column;
                            int totalColumns = worksheet.Dimension.Columns;

                            // Set the format of the last column to Accounting
                            var lastColumn = worksheet.Cells[worksheet.Dimension.Start.Row, lastColumnIndex, worksheet.Dimension.End.Row, lastColumnIndex];
                            lastColumn.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* \"-\"??_);_(@_)";
                        }
                        else {
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
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DownloadAssetReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadRepairedReport")]
        public async Task<IActionResult> DownloadRepairedReportAsync([FromQuery] Report_Filter model, string reportName, string format = "xlsx")
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {

                    var data = await _reportBusiness.GetRepairedReport(model, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, reportName);

                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes))) {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null) {

                            worksheet.InsertRow(1, 1);
                            // Set the report title in the newly inserted row
                            worksheet.Cells["A1"].Value = reportName;
                            var titleCell = worksheet.Cells["A1"];

                            // Apply styling to the title cell
                            titleCell.Style.Font.Bold = true;
                            titleCell.Style.Font.Size = 18;
                            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            titleCell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            titleCell.Style.Fill.BackgroundColor.SetColor(Color.White);

                            // Merge all cells in the "Report Title" row
                            int columnCount = worksheet.Dimension.End.Column;
                            worksheet.Cells[1, 1, 1, columnCount].Merge = true;

                            // Align numeric value columns to center
                            foreach (var cell in worksheet.Cells) {
                                if (cell.Value is double || cell.Value is int || cell.Value is float || cell.Value is decimal) {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }

                        }
                        else {
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
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DownloadStockReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadStockReport")]
        public async Task<IActionResult> DownloadStockReportAsync([FromQuery] Report_Filter model, string reportName, string format = "xlsx")
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {

                    var data = await _reportBusiness.GetStockReport(model, user);

                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, reportName);

                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    using (var package = new ExcelPackage(new MemoryStream(excelBytes))) {
                        // Get the first worksheet from the package
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                        if (worksheet != null) {

                            worksheet.InsertRow(1, 1);
                            // Set the report title in the newly inserted row
                            worksheet.Cells["A1"].Value = reportName;
                            var titleCell = worksheet.Cells["A1"];

                            // Apply styling to the title cell
                            titleCell.Style.Font.Bold = true;
                            titleCell.Style.Font.Size = 18;
                            titleCell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                            titleCell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            titleCell.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
                            titleCell.Style.Fill.PatternType = ExcelFillStyle.Solid;
                            titleCell.Style.Fill.BackgroundColor.SetColor(Color.White);

                            // Merge all cells in the "Report Title" row
                            int columnCount = worksheet.Dimension.End.Column;
                            worksheet.Cells[1, 1, 1, columnCount].Merge = true;

                            // Align numeric value columns to center
                            foreach (var cell in worksheet.Cells) {
                                if (cell.Value is double || cell.Value is int || cell.Value is float || cell.Value is decimal) {
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                            }                  
                            
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
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DownloadAssetReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
