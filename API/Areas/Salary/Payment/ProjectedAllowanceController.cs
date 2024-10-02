using API.Base;
using OfficeOpenXml;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.DTO.Payment;
using DAL.DapperObject.Interface;
using BLL.Salary.Payment.Interface;
using Shared.Payroll.Filter.Payment;
using Microsoft.AspNetCore.Authorization;
using Shared.OtherModels.User;

namespace API.Areas.Salary.Payment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class ProjectedPaymentController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private ExcelGenerator _excelGenerator;
        private readonly IProjectedPaymentBusiness _employeeProjectedPaymentBusiness;
        public ProjectedPaymentController(
            ISysLogger sysLogger,
            IProjectedPaymentBusiness employeeProjectedPaymentBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employeeProjectedPaymentBusiness = employeeProjectedPaymentBusiness;
            _excelGenerator = new ExcelGenerator();
        }

        [HttpGet, Route("GetEmployeeProjectedPayments")]
        public async Task<IActionResult> GetEmployeeProjectedPaymentsAsync([FromQuery] EmployeeProjectedPayment_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var list = await _employeeProjectedPaymentBusiness.GetEmployeeProjectedPaymentsAsync(filter, user);
                    Response.AddPagination(list.Pageparam.PageNumber, list.Pageparam.PageSize, list.Pageparam.TotalRows, list.Pageparam.TotalPages);
                    return Ok(list.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "GetEmployeeProjectedPayments", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetProjectedAllowanceById/{id}")]
        public async Task<IActionResult> GetProjectedAllowanceByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && id > 0)
                {
                    var data = await _employeeProjectedPaymentBusiness.GetProjectedAllowanceByIdAsync(id, user);
                    if (data != null)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return NotFound(ResponseMessage.NoDataFound);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "SaveBlukEmployeeProjectedPayment", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveBlukEmployeeProjectedPayment")]
        public async Task<IActionResult> SaveBlukEmployeeProjectedPaymentAsync(List<EmployeeProjectedPaymentDTO> model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && model.Count > 0 && user.HasBoth)
                {
                    var status = await _employeeProjectedPaymentBusiness.SaveAsync(model, user);
                    if (status.Status == true)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "SaveBlukEmployeeProjectedPayment", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeeProjectedPayment")]
        public async Task<IActionResult> SaveEmployeeProjectedPaymentAsync(EmployeeProjectedPaymentDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _employeeProjectedPaymentBusiness.SaveEmployeeProjectedPaymentAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "SaveEmployeeProjectedPaymentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadUploadFormat")]
        public async Task<IActionResult> DownloadUploadFormatAsync()
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel\\ProjectedPaymentUploader.xlsx");
            string contentType = "";
            if (System.IO.File.Exists(filepath))
            {
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contentType, "projected-payment-uploader.xlsx");
        }

        [HttpPost, Route("UploadProjectedPayment")]
        public async Task<IActionResult> UploadProjectedPaymentAsync([FromForm] UploadProjectedPaymentDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    List<EmployeeProjectedPaymentDTO> list = new List<EmployeeProjectedPaymentDTO>();
                    if (model.File?.Length > 0)
                    {
                        var stream = model.File.OpenReadStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var employeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                var baseType = worksheet.Cells[row, 2].Value?.ToString();
                                var amountOrPercentage = worksheet.Cells[row, 3].Value?.ToString();

                                if (!Utility.IsNullEmptyOrWhiteSpace(baseType) && !Utility.IsNullEmptyOrWhiteSpace(amountOrPercentage))
                                {
                                    EmployeeProjectedPaymentDTO _employeeProjectedPayment = new EmployeeProjectedPaymentDTO();
                                    _employeeProjectedPayment.EmployeeId = 0;
                                    _employeeProjectedPayment.PayableYear = model.PayableYear;
                                    _employeeProjectedPayment.AllowanceReason = model.AllowanceReason;
                                    _employeeProjectedPayment.EmployeeCode = employeeCode;
                                    _employeeProjectedPayment.AllowanceNameId = model.AllowanceNameId;
                                    _employeeProjectedPayment.BaseOfPayment = baseType;
                                    _employeeProjectedPayment.Percentage = baseType != "Flat" ? Convert.ToDecimal(amountOrPercentage) : 0;
                                    _employeeProjectedPayment.Amount = baseType == "Flat" ? Convert.ToDecimal(amountOrPercentage) : 0;
                                    list.Add(_employeeProjectedPayment);
                                }
                            }
                        }
                    }
                    var status = await _employeeProjectedPaymentBusiness.SaveAsync(list, user);
                    if (status.Status == true)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "UploadProjectedPaymentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeProjectedPaymentInfosForProcess")]
        public async Task<IActionResult> GetEmployeeProjectedPaymentInfosForProcessAsync([FromQuery] EmployeeProjectedPayment_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeProjectedPaymentBusiness.GetEmployeeProjectedPaymentInfosForProcessAsync(filter, user);
                    if (data != null)
                    {
                        return Ok(data);
                    }
                    return NoContent();
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "GetEmployeeProjectedPaymentInfosForProcess", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("SaveProjectedPaymentInProcess")]
        public async Task<IActionResult> SaveProjectedPaymentInProcessAsync(ProjectedPaymentProcessDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeProjectedPaymentBusiness.SaveProjectedPaymentInProcessAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "SaveProjectedPaymentInProcessAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }

        }

        [HttpGet, Route("GetEmployeeProjectedAllowanceProcessInfos")]
        public async Task<IActionResult> GetEmployeeProjectedAllowanceProcessInfosAsync([FromQuery] EmployeeProjectedAllowanceProcess_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeProjectedPaymentBusiness.GetEmployeeProjectedAllowanceProcessInfos(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "GetEmployeeProjectedAllowanceProcessInfosAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("DownloadProjectedPaymentReport")]
        public async Task<IActionResult> DownloadProjectedPaymentReportAsync(long projectedAllowanceProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, string format)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var data = await _employeeProjectedPaymentBusiness.GetProjectedPaymentReport(projectedAllowanceProcessInfoId, employeeId, fiscalYearId, paymentMonth, paymentYear, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "Projected-Payment-Report");
                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
                    string fileName = "data." + format;
                    string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;

                    using (var package = new ExcelPackage(new MemoryStream(excelBytes)))
                    {
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
                            return BadRequest(new { message = "Worksheet not found in the Excel package." });
                        }
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "DownloadProjectedPaymentReportAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("DeletePendingAllowanceById")]
        public async Task<IActionResult> DeletePendingAllowanceByIdAsync(DeleteProjectedAllowanceDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _employeeProjectedPaymentBusiness.DeletePendingAllowanceByIdAsync(model.Id, user);
                    if (status.Status)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "DeletePendingAllowanceById", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("DeleteApprovedAllowanceById")]
        public async Task<IActionResult> DeleteApprovedAllowanceByIdAsync(DeleteProjectedAllowanceDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _employeeProjectedPaymentBusiness.DeleteApprovedAllowanceByIdAsync(model.Id, user);
                    if (status.Status)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "DeleteApprovedAllowanceByIdAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);

            }
        }

        [HttpPost, Route("UpdateProjectedPayment")]
        public async Task<IActionResult> UpdateProjectedPaymentAsync(EmployeeProjectedPaymentDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var status = await _employeeProjectedPaymentBusiness.UpdateAsync(model, user);
                    if (status != null)
                    {
                        if (status.Status)
                        {
                            return Ok(status);
                        }
                        else
                        {
                            return BadRequest(status);
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "UpdateProjectedPaymentAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("Approval")]
        public async Task<IActionResult> ApprovalAsync(ProjectedPaymentApprovalDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var status = await _employeeProjectedPaymentBusiness.ApprovalAsync(model, user);
                    if (status != null)
                    {
                        if (status.Status)
                        {
                            return Ok(status);
                        }
                        else
                        {
                            return BadRequest(status);
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "ApprovalAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("ListOfPaymentReason")]
        public async Task<IActionResult> ListOfPaymentReasonAsync()
        {
            var user = AppUser();   
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeProjectedPaymentBusiness.ListOfPaymentReasonAsync(user);
                    return Ok(data);
                }
                return BadRequest("Could not find employee's company identity");
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentController", "ListOfPaymentReason", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
