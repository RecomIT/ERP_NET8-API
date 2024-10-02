using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using Shared.Services;
using Shared.Helpers;
using API.Services;
using AspNetCore.Reporting;
using BLL.Base.Interface;
using Shared.OtherModels.Report;
using DAL.DapperObject.Interface;
using BLL.Salary.CashSalary.Interface;
using BLL.Salary.Salary.Interface;
using API.Base;
using Shared.Payroll.Filter.CashSalary;
using Shared.Payroll.DTO.CashSalary;
using Shared.Payroll.ViewModel.CashSalary;
using BLL.Administration.Interface;

namespace API.Areas.Salary.CashSalary
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class CashSalaryController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICashSalaryBusiness _cashSalaryBusiness;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private ExcelGenerator _excelGenerator;
        public CashSalaryController(IWebHostEnvironment webHostEnvironment, ISysLogger sysLogger, IMapper mapper, ICashSalaryBusiness cashSalaryBusiness, ISalaryReportBusiness salaryReportBusiness, IClientDatabase clientDatabase, IReportConfigBusiness reportConfigBusiness) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _mapper = mapper;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _webHostEnvironment = webHostEnvironment;
            _cashSalaryBusiness = cashSalaryBusiness;
            _reportConfigBusiness = reportConfigBusiness;
            _salaryReportBusiness = salaryReportBusiness;
            _excelGenerator = new ExcelGenerator();
        }

        [HttpGet, Route("DownloadCashSalaryHeadExcel")]
        public async Task<IActionResult> DownloadCashSalaryHeadExcelAsync(string fileName)
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

        [HttpPost, Route("UploadCashSalaryHeadExcel")]
        public async Task<IActionResult> UploadCashSalaryHeadExcelAync([FromForm] CashSalaryHeadUpload uploadedFile)
        {
            try
            {
                var appUser = AppUser();

                if (ModelState.IsValid)
                {
                    if (uploadedFile.ExcelFile?.Length > 0)
                    {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<CashSalaryHeadDTO> readExcelDTOs = new List<CashSalaryHeadDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                CashSalaryHeadDTO readDTO = new CashSalaryHeadDTO();
                                var HeadName = worksheet.Cells[row, 1].Value.ToString();
                                var HeadCode = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                                var HeadNameInBng = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                                var IsActive = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                                IsActive = IsActive == "Yes" ? "True" : "False";

                                readDTO.CashSalaryHeadName = HeadName;
                                readDTO.CashSalaryHeadCode = HeadCode;
                                readDTO.CashSalaryHeadNameInBengali = HeadNameInBng;
                                readDTO.IsActive = Convert.ToBoolean(IsActive);
                                readExcelDTOs.Add(readDTO);
                            }

                        }
                        var data = await _cashSalaryBusiness.UploadCashSalaryHeadExcelAsync(readExcelDTOs, appUser);
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

        [HttpPost, Route("SaveCashSalaryHead")]
        public async Task<IActionResult> SaveCashSalaryHeadAsync(CashSalaryHeadDTO headDTO)
        {
            var appUser = AppUser();
            try
            {
                if (ModelState.IsValid)
                {

                    var data = await _cashSalaryBusiness.SaveCashSalaryHeadAsync(headDTO, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetCashSalaryHeadList")]
        public async Task<IActionResult> GetCashSalaryHeadListAsync(long? cashSalaryHeadId, string cashSalaryHeadName, string cashSalaryHeadCode, string cashSalaryHeadNameInBengali, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var appUser = AppUser();
                pageNumber = Utility.PageNumber(pageNumber);
                pageSize = Utility.PageSize(pageSize);
                var allData = await _cashSalaryBusiness.GetCashSalaryHeadListAsync(cashSalaryHeadId, cashSalaryHeadName ?? "", cashSalaryHeadCode ?? "", cashSalaryHeadNameInBengali ?? "", appUser);
                var data = PagedList<CashSalaryHeadDTO>.ToPagedList(allData, pageNumber, pageSize);
                Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetCashSalaryHeadById")]
        public async Task<IActionResult> GetCashSalaryHeadByIdAsync(long cashSalaryHeadId)
        {
            var appUser = AppUser();
            try
            {
                var data = await _cashSalaryBusiness.GetCashSalaryHeadByIdAsync(cashSalaryHeadId, appUser);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok("Serve responed with error");
            }
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="Upload Cash Salary"></param>
        /// <returns></returns>
        [HttpGet, Route("DownloadCashSalaryExcelFile")]
        public async Task<IActionResult> DownloadCashSalaryExcelFile(string fileName)
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

        [HttpGet, Route("GetCashSalaryHeadExtension")]
        public async Task<IActionResult> GetCashSalaryHeadExtensionAsync(long? cashSalaryHeadId)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _cashSalaryBusiness.GetCashSalaryHeadExtensionAsync(cashSalaryHeadId ?? 0, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("UploadCashSalaryExcel")]
        public async Task<IActionResult> UploadCashSalaryExcelAync([FromForm] UploadCashSalaryAmountViewModel model)
        {
            try
            {
                var appUser = AppUser();

                if (ModelState.IsValid)
                {
                    if (model.ExcelFile?.Length > 0)
                    {
                        var stream = model.ExcelFile.OpenReadStream();
                        List<UploadCashSalaryDTO> readExcelDTOs = new List<UploadCashSalaryDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            var rowCount = worksheet!.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var amt = worksheet.Cells[row, 2].Value?.ToString();
                                if (amt != null)
                                {
                                    UploadCashSalaryDTO readDTO = new UploadCashSalaryDTO();
                                    var empCode = worksheet.Cells[row, 1].Value?.ToString();
                                    var amount = worksheet.Cells[row, 2].Value?.ToString();

                                    readDTO.EmployeeCode = empCode;
                                    readDTO.Amount = Convert.ToDecimal(amount);
                                    readDTO.CashSalaryHeadId = model.CashSalaryHeadId;
                                    readDTO.SalaryMonth = model.SalaryMonth;
                                    readDTO.SalaryYear = model.SalaryYear;
                                    readExcelDTOs.Add(readDTO);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        var data = await _cashSalaryBusiness.UploadCashSalaryExcelAync(readExcelDTOs, appUser);
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

        [HttpGet, Route("UploadCashSalaryList")]
        public async Task<IActionResult> UploadCashSalaryListAsync(long? uploadCashSalaryId, long? employeeId, long? cashSalaryHeadId, short salaryMonth, short salaryYear, string stateStatus, int pageNumber = 1, int pageSize = 15)
        {
            try
            {
                var user = AppUser();
                if (user.HasBoth)
                {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _cashSalaryBusiness.UploadCashSalaryListAsync(uploadCashSalaryId ?? 0, employeeId ?? 0, cashSalaryHeadId ?? 0, salaryMonth, salaryYear, stateStatus, user);
                    var data = PagedList<UploadCashSalaryDTO>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("SaveCashSalaries")]
        public async Task<IActionResult> SaveCashSalariesAync(List<UploadCashSalaryDTO> cashSalaryDTOs)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _cashSalaryBusiness.SaveCashSalariesAync(cashSalaryDTOs, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPut, Route("UpdateUploadCashSalary")]
        public async Task<IActionResult> UpdateUploadCashSalaryAsync(UploadCashSalaryDTO dTO)
        {
            try
            {
                var appUser = AppUser();
                if (ModelState.IsValid && dTO.UploadCashSalaryId > 0 && appUser.HasBoth)
                {

                    var data = await _cashSalaryBusiness.UpdateUploadCashSalaryAsync(dTO, appUser);
                    return Ok(data);

                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("SaveUploadCashSalaryApproval")]
        public async Task<IActionResult> SaveUploadCashSalaryApprovalAsync(long? uploadCashSalaryId, long? employeeId, long? cashSalaryHeadId, string stateStatus, string remarks)
        {
            var appUser = AppUser();
            try
            {
                if (uploadCashSalaryId > 0 && !Utility.IsNullEmptyOrWhiteSpace(stateStatus) && Utility.StatusChecking(stateStatus, new string[] { StateStatus.Approved, StateStatus.Recheck, StateStatus.Cancelled }) && appUser.HasBoth)
                {
                    var data = await _cashSalaryBusiness.SaveUploadCashSalaryApprovalAsync(uploadCashSalaryId, employeeId, cashSalaryHeadId, stateStatus, remarks, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception)
            {

                return BadRequest("Somthing went wrong");
            }
        }

        [HttpPost, Route("CashSalaryProcess")]
        public async Task<IActionResult> CashSalaryProcessAsync(CashSalaryProcessExecutionDTO model)
        {
            var appUser = AppUser();
            try
            {
                if (ModelState.IsValid && appUser.HasBoth)
                {
                    var data = await _cashSalaryBusiness.CashSalaryProcessAsync(model, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetCashSalaryProcessInfos")]
        public async Task<IActionResult> GetCashSalaryProcessInfosAsync(long? cashSalaryProcessId, short? salaryMonth, short? salaryYear, DateTime? salaryDate, string batchNo)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.HasBoth)
                {
                    var data = await _cashSalaryBusiness.GetCashSalaryProcessInfosAsync(cashSalaryProcessId ?? 0, salaryMonth ?? 0, salaryYear ?? 0, salaryDate, batchNo, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetCashSalaryProcessDetail")]
        public async Task<IActionResult> GetCashSalaryProcessDetailAsync(long? cashSalaryProcessId, long? cashSalaryDetailId, long? employeeId, short? salaryMonth, short? salaryYear, DateTime? salaryDate, string batchNo)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.HasBoth)
                {
                    var data = await _cashSalaryBusiness.GetCashSalaryProcessDetailAsync(cashSalaryProcessId ?? 0, cashSalaryDetailId ?? 0, employeeId ?? 0, salaryMonth ?? 0, salaryYear ?? 0, salaryDate, batchNo, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("CashSalaryProcessDisbursedOrUndo")]
        public async Task<IActionResult> CashSalaryProcessDisbursedOrUndoAsync(long cashSalaryProcessId, string actionName)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.HasBoth)
                {
                    var data = await _cashSalaryBusiness.CashSalaryProcessDisbursedOrUndoAsync(cashSalaryProcessId, actionName, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("DownloadCashSalarySheet")]
        public async Task<IActionResult> DownloadCashSalarySheetAsync([FromQuery] CashSalarySheet_Filter sheet_Filter, string format)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _cashSalaryBusiness.GetCashSalarySheetAsync(sheet_Filter, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "CashSalarySheet");
                    format = format == "xlsx" || format == "xls" ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    if (data.Rows.Count > 0)
                    {
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
                    }
                    // Load the Excel package from the byte array
                    HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
                    HttpContext.Response.ContentType = contentType;
                    return File(excelBytes, contentType);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CashSalaryController", "DownloadCashSalarySheetAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("DownloadActualCashSalarySheet")]
        public async Task<IActionResult> DownloadActualCashSalarySheetAsync([FromQuery] CashSalarySheet_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    string renderFormat = filter.Format.ToUpper();
                    string mimetype = Utility.GetFileMimetype(filter.Format);
                    string extension = filter.Format.ToLower();
                    var path = "";

                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("CashSalarySheet", "", user.CompanyId, user.OrganizationId);

                    if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }
                    else
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Nagad\\cash_salary\\cash_salary_sheet_actual.rdlc";
                    }
                    var datatable = await _cashSalaryBusiness.GetActualCashSalarySheetDetailAsync(filter, user);

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, 0);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    LocalReport localReport = new LocalReport(path);

                    if (datatable.Rows.Count > 0)
                    {

                        localReport.AddDataSource("ReportLayer", reportLayers);
                        localReport.AddDataSource("ActualCashSalarySheet", datatable);

                        var result = localReport.Execute(RenderType.ExcelOpenXml, 1);
                        var renderBytes = result.MainStream;

                        Response.Headers.Add("File-Name", "CashSalarySheet");
                        return File(renderBytes, "application/vnd.ms-excel", "CashSalarySheet." + extension);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "CashSalaryController", "DownloadActualCashSalarySheet", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}

