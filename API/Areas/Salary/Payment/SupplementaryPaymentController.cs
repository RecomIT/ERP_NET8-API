using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.DTO.Payment;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using Shared.Employee.Filter.Info;
using BLL.Salary.Payment.Interface;
using Shared.Payroll.Filter.Payment;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Shared.OtherModels.User;


namespace API.Areas.Salary.Payment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SupplementaryPaymentController : ApiBaseController
    {
        private readonly ISupplementaryPaymentAmountBusiness _supplementaryPaymentAmountBusiness;
        private readonly ISupplementaryPaymentProcessBusiness _supplementaryPaymentProcessBusiness;
        private readonly IInfoBusiness _employeeInfoBusiness;
        private readonly ISysLogger _sysLogger;
        private ExcelGenerator _excelGenerator;
        public SupplementaryPaymentController(
            ISupplementaryPaymentAmountBusiness supplementaryPaymentAmountBusiness,
            ISupplementaryPaymentProcessBusiness supplementaryPaymentProcessBusiness,
            IInfoBusiness employeeInfoBusiness,
            ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _supplementaryPaymentAmountBusiness = supplementaryPaymentAmountBusiness;
            _supplementaryPaymentProcessBusiness = supplementaryPaymentProcessBusiness;
            _sysLogger = sysLogger;
            _employeeInfoBusiness = employeeInfoBusiness;
            _excelGenerator = new ExcelGenerator();
        }


        [HttpGet("GetSupplementaryPaymentAmountInfos")]
        public async Task<IActionResult> GetSupplementaryPaymentAmountInfosAsync([FromQuery] SupplementaryPaymentAmount_Filter model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var list = await _supplementaryPaymentAmountBusiness.GetSupplementaryPaymentAmountInfosAsync(model, user);
                    Response.AddPagination(list.Pageparam.PageNumber, list.Pageparam.PageSize, list.Pageparam.TotalRows, list.Pageparam.TotalPages);
                    return Ok(list.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentController", "GetPaymentAmountInfosAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost("SaveBulkSupplementaryPaymentAmount")]
        public async Task<IActionResult> SaveBulkSupplementaryPaymentAmountAsync(List<SupplementaryPaymentAmountDTO> payments)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _supplementaryPaymentAmountBusiness.ValidatePaymentAsync(payments, user);
                    if (validator != null && validator.Status == false)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var dbResponse = await _supplementaryPaymentAmountBusiness.SaveBulkSupplementaryPaymentAmountAsync(payments, user);
                        return Ok(dbResponse);
                    }
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentController", "SaveBulkSupplementaryPaymentAmountAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost("SaveSupplementaryPaymentAmount")]
        public async Task<IActionResult> SaveSupplementaryPaymentAmountAsync(SupplementaryPaymentAmountDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _supplementaryPaymentAmountBusiness.ValidatePaymentAsync(new List<SupplementaryPaymentAmountDTO>() { model }, user);
                    if (validator != null)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var dbResponse = await _supplementaryPaymentAmountBusiness.SaveSupplementaryPaymentAmountAsync(model, user);
                        return Ok(dbResponse);
                    }
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentController", "SaveBulkSupplementaryPaymentAmountAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetSupplementaryPaymentAmountsForProcess")]
        public async Task<IActionResult> GetSupplementaryPaymentAmountsForProcessAsync([FromQuery] SupplementaryPaymentAmount_Filter model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _supplementaryPaymentAmountBusiness.GetSupplementaryPaymentAmountsForProcessAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentController", "GetSupplementaryPaymentAmountsForProcessAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SaveSupplementaryProcess")]
        public async Task<IActionResult> SaveSupplementaryProcessAsync(SupplementaryProcessDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _supplementaryPaymentAmountBusiness.SaveSupplementaryProcessAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentController", "SaveSupplementaryProcessAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetSupplementaryPaymentProcessInfos")]
        public async Task<IActionResult> GetSupplementaryPaymentProcessInfosAsync([FromQuery] SupplementaryPaymentProcessInfo_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var list = await _supplementaryPaymentAmountBusiness.GetSupplementaryPaymentProcessInfosAsync(filter, user);
                    Response.AddPagination(list.Pageparam.PageNumber, list.Pageparam.PageSize, list.Pageparam.TotalRows, list.Pageparam.TotalPages);
                    return Ok(list.ListOfObject);

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentController", "GetSupplementaryPaymentProcessInfosAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        //added by Monzur 31-Dec-2023

        [HttpPost, Route("UploadExcelFile")]
        public async Task<IActionResult> UploadExcelFileAsync([FromForm] UploadSupplementaryPaymentAmountDTO uploadedFile)
        {
            try
            {
                var appUser = AppUser();
                if (ModelState.IsValid)
                {
                    if (uploadedFile.File?.Length > 0)
                    {
                        var stream = uploadedFile.File.OpenReadStream();
                        List<SupplementaryProcessExcelReadDTO> readExcelDTOs = new List<SupplementaryProcessExcelReadDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var val = worksheet.Cells[row, 1].Value;
                                if (val != null)
                                {
                                    var employeeCode = val.ToString();
                                    if (employeeCode != null)
                                    {
                                        SupplementaryProcessExcelReadDTO readDTO = new SupplementaryProcessExcelReadDTO();
                                        var employeeInfos = await _employeeInfoBusiness.GetEmployeeDataAsync(new EmployeeService_Filter()
                                        {
                                            EmployeeCode = employeeCode
                                        }, appUser);
                                        if (employeeInfos.Any())
                                        {
                                            var employeeInfo = employeeInfos.FirstOrDefault();
                                            if (employeeInfo != null)
                                            {
                                                var amount = worksheet.Cells[row, 2].Value;
                                                if (amount != null && Utility.TryParseDecimal(amount.ToString()) > 0)
                                                {
                                                    readDTO.EmployeeCode = employeeCode;
                                                    readDTO.EmployeeName = employeeInfo.EmployeeName;
                                                    readDTO.Designation = employeeInfo.DesignationName;
                                                    readDTO.EmployeeId = employeeInfo.EmployeeId;
                                                    readDTO.Amount = Utility.TryParseDecimal(amount.ToString());
                                                    readExcelDTOs.Add(readDTO);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        return Ok(readExcelDTOs);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost("UploadSupplementaryPaymentAmount")]
        public async Task<IActionResult> UploadSupplementaryPaymentAmountAsync(List<SupplementaryProcessExcelReadDTO> upload)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var dbResponse = await _supplementaryPaymentAmountBusiness.UploadSupplementaryPaymentAmountAsync(upload, user);
                    return Ok(dbResponse);
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentController", "UploadSupplementaryPaymentAmount", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("DownloadExcelSupplementaryPaymentAmount")]
        public async Task<IActionResult> DownloadExcelSupplementaryPaymentAmountAsync(string fileName)
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

        //Added by Monzur 12-Feb-24

        [HttpGet, Route("DownloadSupplementaryTaxSheetDetails")]
        public async Task<IActionResult> DownloadSupplementaryTaxSheetDetailsAsync(long paymentProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, string format)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var data = await _supplementaryPaymentAmountBusiness.GetSupplementaryTaxSheetDetailsReport(paymentProcessInfoId, employeeId, fiscalYearId, paymentMonth, paymentYear, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "108-Once-Off-Report");
                    format = format == "xlsx" || format == "xls" ? format : "xlsx";
                    string fileName = "data." + format;
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

        [HttpGet, Route("DownloadSupplementaryPaymentReport")]
        public async Task<IActionResult> DownloadSupplementaryPaymentReportAsync(long paymentProcessInfoId, long employeeId, long fiscalYearId, short paymentMonth, short paymentYear, string format)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var data = await _supplementaryPaymentAmountBusiness.GetSupplementaryPaymentReport(paymentProcessInfoId, employeeId, fiscalYearId, paymentMonth, paymentYear, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "Incentive-Report");
                    format = format == "xlsx" || format == "xls" ? format : "xlsx";
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
                            // Handle the case where the worksheet doesn't exist
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
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        // Supplementary Tax Process //
        [HttpPost("SupplementaryPaymentProcess")]
        public async Task<IActionResult> SupplementaryPaymentProcessAsync(SupplementaryProcessDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _supplementaryPaymentProcessBusiness.ProcessAsync(model, user);
                    if (data.Status)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return BadRequest(data);
                    }

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet("UndisbursedSupplementaryPaymentInfo/{id}")]
        public async Task<IActionResult> UndisbursedSupplementaryPaymentInfoAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.HasBoth)
                {
                    var data = await _supplementaryPaymentProcessBusiness.UndisbursedSupplementaryPaymentInfoAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.InvalidParameters);
            }
        }

        [HttpPost("DisbursedOrUndoPayment")]
        public async Task<IActionResult> DisbursedOrUndoPaymentAsync(DisbursedOrUndoPaymentDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _supplementaryPaymentProcessBusiness.DisbursedOrUndoPaymentAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.InvalidParameters);
            }
        }

        [HttpPost("DeleteSupplementaryAmount")]
        public async Task<IActionResult> DeleteSupplementaryAmountAsync(DeletePaymentDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _supplementaryPaymentAmountBusiness.DeleteSupplementaryAmountAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.InvalidParameters);
            }
        }

        [HttpGet("GetSupplementaryAmountById")]
        public async Task<IActionResult> GetSupplementaryAmountByIdAsync(long paymentAmountId)
        {
            var user = AppUser();
            try
            {
                if (paymentAmountId == 0)
                {
                    return BadRequest("Payment is required, Which is missing");
                }
                if (user.HasBoth)
                {
                    var data = await _supplementaryPaymentAmountBusiness.GetSupplementaryAmountByIdAsync(paymentAmountId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("UpdatePaymentAmount")]
        public async Task<IActionResult> UpdatePaymentAmountAsync(SupplementaryPaymentAmountDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.HasBoth)
                    {
                        var status = await _supplementaryPaymentProcessBusiness.UpdatePaymentAmountAsync(model, user);
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
                return BadRequest(ModelStateErrorMsg(ModelState));
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("UploadDeductedTaxAmount")]
        public async Task<IActionResult> UploadDeductedTaxAmountAsync(UploadSupplementaryDeductedTaxAmountDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    if (user.HasBoth)
                    {
                        if (model.File?.Length > 0)
                        {
                            List<UpdateSupplementaryTaxAmountDTO> list = new List<UpdateSupplementaryTaxAmountDTO>();
                            var stream = model.File.OpenReadStream();
                            using (var package = new ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets.First();
                                var rowCount = worksheet.Dimension.Rows;

                                for (var row = 2; row <= rowCount; row++)
                                {
                                    var val = worksheet.Cells[row, 1].Value;
                                    if (val != null)
                                    {
                                        var employeeCode = val.ToString();
                                        var employeeInfos = await _employeeInfoBusiness.GetEmployeeDataAsync(new EmployeeService_Filter()
                                        {
                                            EmployeeCode = employeeCode
                                        }, user);

                                        if (employeeInfos.Any())
                                        {
                                            var employeeInfo = employeeInfos.FirstOrDefault();
                                            if (employeeInfo != null)
                                            {
                                                var amount = worksheet.Cells[row, 2].Value;
                                                if (amount != null && Utility.TryParseDecimal(amount.ToString()) > 0)
                                                {
                                                    UpdateSupplementaryTaxAmountDTO item = new UpdateSupplementaryTaxAmountDTO();
                                                    item.PaymentProcessInfoId = model.ProcessId;
                                                    item.EmployeeId = employeeInfo.EmployeeId;
                                                    item.TaxAmount = Utility.TryParseDecimal(amount.ToString());
                                                    list.Add(item);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (list.Any())
                            {
                                var status = await _supplementaryPaymentProcessBusiness.UpdateSupplementaryTaxAmountAsync(model.ProcessId, list, user);
                                if (status.Status)
                                {
                                    return Ok(status);
                                }
                                return BadRequest(status);
                            }
                        }
                    }
                    return BadRequest(ResponseMessage.InvalidParameters);
                }
                return BadRequest(ModelStateErrorMsg(ModelState));
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
