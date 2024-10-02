using API.Base;
using API.Services;
using BLL.Base.Interface;
using BLL.Employee.Interface.Setup;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using Shared.Services;
using System.Data;

namespace API.Areas.Employee.Setup
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class TableConfigController : ApiBaseController
    {
        private readonly ITableConfigBusiness _tableConfigBusiness;
        private readonly ISysLogger _sysLogger;
        private ExcelGenerator _excelGenerator;
        public TableConfigController(
            ITableConfigBusiness tableConfigBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _tableConfigBusiness = tableConfigBusiness;
            _sysLogger = sysLogger;
            _excelGenerator = new ExcelGenerator();
        }

        [HttpGet, Route("GetColumns")]
        public async Task<IActionResult> GetColumnsAsync(string table, string purpose)
        {
            var user = AppUser();
            try
            {
                if (
                    user.HasBoth &&
                    Utility.IsNullEmptyOrWhiteSpace(table) == false &&
                    Utility.IsNullEmptyOrWhiteSpace(purpose) == false)
                {
                    var list = await _tableConfigBusiness.GetColumnsAsync(table, purpose, user);
                    if (list == null || !list.Any())
                    {
                        return NotFound("No data found");
                    }
                    else
                    {
                        return Ok(list);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TableConfigController", "GetUploaderInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DownloadExcelUploader")]
        public async Task<IActionResult> DownloadExcelUploaderAsync(List<string> columns)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && columns.Any())
                {
                    var fileBytes = _excelGenerator.GenerateExcel(columns, "Sheet 1");
                    string fileName = "Uploader.xlsx";
                    string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    using (var package = new ExcelPackage(new MemoryStream(fileBytes)))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        fileBytes = package.GetAsByteArray();
                    }
                    HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
                    HttpContext.Response.ContentType = contentType;
                    return File(fileBytes, contentType);
                }
                return BadRequest(ResponseMessage.InvalidParameters + "/ You haven't check any column");
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TableConfigController", "DownloadExcelUploaderAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
