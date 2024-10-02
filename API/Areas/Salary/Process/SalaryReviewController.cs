using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.DTO.Salary;
using DAL.DapperObject.Interface;
using BLL.Salary.Salary.Interface;
using Shared.Payroll.Filter.Salary;
using BLL.Salary.Allowance.Interface;
using Shared.OtherModels.DataService;
using Shared.Payroll.ViewModel.Salary;
using Shared.Payroll.Filter.Allowance;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Process
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SalaryReviewController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ISalaryReviewBusiness _salaryReviewBusiness;
        private readonly IAllowanceNameBusiness _allowanceNameBusiness;
        private readonly ExcelGenerator _excelGenerator;
        public SalaryReviewController(ISysLogger sysLogger, IAllowanceNameBusiness allowanceNameBusiness, ISalaryReviewBusiness salaryReviewBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _salaryReviewBusiness = salaryReviewBusiness;
            _allowanceNameBusiness = allowanceNameBusiness;
            _sysLogger = sysLogger;
            _excelGenerator = new ExcelGenerator();
        }

        [HttpGet, Route("GetSalaryReviewInfos")]
        public async Task<IActionResult> GetSalaryReviewInfosAsync([FromQuery] SalaryReview_Filter filter, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _salaryReviewBusiness.GetSalaryReviewInfosAsync(filter, user);
                    var data = PagedList<SalaryReviewInfoViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(allData);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "GetSalaryReviewInfos", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveSalaryReview")]
        public async Task<IActionResult> SaveSalaryReviewAsync(SalaryReviewInfoDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    model.ActivationDate = new DateTime(model.ActivationYear, model.ActivationMonth, 1).Date;
                    model.ArrearCalculatedDate = new DateTime(model.ArrearYear, model.ArrearMonth, 1).Date;

                    var validate = await _salaryReviewBusiness.ValidateSalaryReviewAsync(model, user);
                    if (validate != null && validate.Status)
                    {
                        var data = await _salaryReviewBusiness.SaveSalaryReviewAsync(model, user);
                        return Ok(data);
                    }
                    return Ok(validate);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "SaveSalaryReview", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveSalaryReviewStatus")]
        public async Task<IActionResult> SaveSalaryReviewStatusAsync(SalaryReviewStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _salaryReviewBusiness.SaveSalaryReviewStatusAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "SaveSalaryReviewStatus", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryAllowanceForReview")]
        public async Task<IActionResult> GetSalaryAllowanceForReviewAsync(long employeeId)
        {
            var user = AppUser();
            try
            {
                if (employeeId > 0 && user.HasBoth)
                {
                    var data = await _salaryReviewBusiness.GetSalaryAllowanceForReviewAsync(employeeId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "GetSalaryAllowanceForReview", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryReviewDetails")]
        public async Task<IActionResult> GetSalaryReviewDetailsAsync([FromQuery] SalaryReview_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasUserId)
                {
                    var data = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "GetSalaryReviewDetails", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalaryReviewInfoAndDetails")]
        public async Task<IActionResult> GetSalaryReviewInfoAndDetailsAsync([FromQuery] SalaryReview_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var info = (await _salaryReviewBusiness.GetSalaryReviewInfosAsync(filter, user)).FirstOrDefault();
                    var details = await _salaryReviewBusiness.GetSalaryReviewDetailsAsync(filter, user);
                    if (info != null)
                    {
                        info.SalaryReviewDetails = details.ToList();
                    }
                    return Ok(info);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "GetSalaryReviewInfoAndDetails", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UploadFlatSalaryReview")]
        public async Task<IActionResult> UploadFlatSalaryReviewAsync([FromForm] FlatSalaryReviewUploaderFileDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.File?.Length > 0)
                    {
                        var stream = model.File.OpenReadStream();
                        List<UploadFlatSalaryReviewInfoDTO> uploadFlatSalaryReviewInfoDTO = new List<UploadFlatSalaryReviewInfoDTO>();
                        List<KeyValueIndex> allowances = new List<KeyValueIndex>();
                        List<string> columns = new List<string>();

                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            var columnCount = worksheet.Dimension.Columns;
                            for (var col = 1; col <= columnCount; col++)
                            {
                                var columnName = worksheet.Cells[1, col].Text;
                                if (!Utility.IsNullEmptyOrWhiteSpace(columnName))
                                {
                                    columns.Add(columnName.RemoveWhitespace());
                                }
                            }
                            if (columns.Count > 4)
                            {
                                columns.RemoveAt(0);
                                // Effective Date/Active Date/Arrear Date
                                columns.RemoveAt(columns.Count - 1);
                                columns.RemoveAt(columns.Count - 1);
                                columns.RemoveAt(columns.Count - 1);
                                // CTC/FB/PF
                                columns.RemoveAt(columns.Count - 1);
                                columns.RemoveAt(columns.Count - 1);
                                columns.RemoveAt(columns.Count - 1);

                                var allowance_index = 1;
                                foreach (var allowanceName in columns)
                                {
                                    KeyValueIndex allowance = new KeyValueIndex();
                                    var allowanceInDb = (await _allowanceNameBusiness.GetAllowanceNamesAsync(new AllowanceName_Filter() { Name = allowanceName }, user)).FirstOrDefault();

                                    if (allowanceInDb != null)
                                    {
                                        allowance.Key = allowanceInDb.AllowanceNameId.ToString();
                                        allowance.Value = allowanceInDb.Name;
                                        allowance.Index = allowance_index.ToString();
                                        allowances.Add(allowance);
                                    }
                                    allowance_index = allowance_index + 1;
                                }
                            }

                            for (var row = 2; row <= rowCount; row++)
                            {
                                UploadFlatSalaryReviewInfoDTO salaryReview = new UploadFlatSalaryReviewInfoDTO();
                                salaryReview.EmployeeCode = worksheet.Cells[row, 1].Value?.ToString();

                                var effectiveDate = worksheet.Cells[row, columnCount - 5].Value?.ToString();
                                if (effectiveDate.IsStringNumber() && effectiveDate.IsNullEmptyOrWhiteSpace() == false)
                                {
                                    salaryReview.EffectiveDate = DateTime.FromOADate(Convert.ToDouble(effectiveDate.RemoveWhitespace()));
                                }
                                else
                                {
                                    salaryReview.EffectiveDate = effectiveDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(effectiveDate.RemoveWhitespace()) : null;
                                }

                                var activationDate = worksheet.Cells[row, columnCount - 4].Value?.ToString();
                                if (activationDate.IsStringNumber() && activationDate.IsNullEmptyOrWhiteSpace() == false)
                                {
                                    salaryReview.ActivationDate = DateTime.FromOADate(Convert.ToDouble(activationDate.RemoveWhitespace()));
                                }
                                else
                                {
                                    salaryReview.ActivationDate = activationDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(activationDate.RemoveWhitespace()) : null;
                                }

                                var arrearDate = worksheet.Cells[row, columnCount - 3].Value?.ToString();
                                if (arrearDate.IsStringNumber() && arrearDate.IsNullEmptyOrWhiteSpace() == false)
                                {
                                    salaryReview.ArrearDate = DateTime.FromOADate(Convert.ToDouble(arrearDate.RemoveWhitespace()));
                                }
                                else
                                {
                                    salaryReview.ArrearDate = arrearDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(arrearDate.RemoveWhitespace()) : null;
                                }


                                var yearlyCTC = worksheet.Cells[row, columnCount - 2].Value?.ToString();
                                if (yearlyCTC.IsStringNumber() && yearlyCTC.IsNullEmptyOrWhiteSpace() == false)
                                {
                                    salaryReview.YearlyCTC = Convert.ToDecimal(yearlyCTC);
                                }
                                else
                                {
                                    salaryReview.YearlyCTC = 0;
                                }

                                var monthlyFB = worksheet.Cells[row, columnCount - 1].Value?.ToString();
                                if (monthlyFB.IsStringNumber() && monthlyFB.IsNullEmptyOrWhiteSpace() == false)
                                {
                                    salaryReview.MonthlyFB = Convert.ToDecimal(monthlyFB);
                                }
                                else
                                {
                                    salaryReview.MonthlyFB = 0;
                                }

                                var monthlyPF = worksheet.Cells[row, columnCount].Value?.ToString();
                                if (monthlyPF.IsStringNumber() && monthlyPF.IsNullEmptyOrWhiteSpace() == false)
                                {
                                    salaryReview.MonthlyPF = Convert.ToDecimal(monthlyPF);
                                }
                                else
                                {
                                    salaryReview.MonthlyPF = 0;
                                }

                                salaryReview.SalaryReviewDetails = new List<UploadFlatSalaryReviewDetailDTO>();

                                foreach (var allowance in allowances)
                                {
                                    UploadFlatSalaryReviewDetailDTO salaryReviewDetail = new UploadFlatSalaryReviewDetailDTO();
                                    salaryReviewDetail.AllowanceNameId = Convert.ToInt64(allowance.Key);
                                    salaryReviewDetail.AllowanceName = allowance.Value;
                                    var amount = worksheet.Cells[row, Convert.ToInt32(allowance.Index) + 1].Value?.ToString();
                                    if (amount.IsStringNumber() && amount.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        if (Convert.ToDecimal(amount) > 0)
                                        {
                                            salaryReviewDetail.Amount = Convert.ToDecimal(amount);
                                            salaryReview.SalaryReviewDetails.Add(salaryReviewDetail);
                                        }
                                    }
                                }
                                uploadFlatSalaryReviewInfoDTO.Add(salaryReview);
                            }

                            var data = await _salaryReviewBusiness.UploadFlatSalaryReviewAsync(uploadFlatSalaryReviewInfoDTO, user);

                            var totalItems = uploadFlatSalaryReviewInfoDTO.Count;
                            var totalSuccessfull = data.Where(item => item.Status == true).Count();
                            var totalUnSuccessfull = data.Where(item => item.Status == false).Count();
                            var unSuccessfull = data.Where(item => item.Status == false);

                            return Ok(new { totalItems = totalItems, totalSuccessfull = totalSuccessfull, totalUnSuccessfull = totalUnSuccessfull, unSuccessfull = unSuccessfull, status = totalUnSuccessfull > 0 ? false : true, msg = totalUnSuccessfull > 0 ? "One or more has been failed to save" : ResponseMessage.Successfull });
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "UploadFlatSalaryReviewAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetLastSalaryReviewInfoByEmployee")]
        public async Task<IActionResult> GetLastSalaryReviewInfoByEmployeeryAsync(long employeeId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && employeeId > 0)
                {
                    var data = await _salaryReviewBusiness.GetLastSalaryReviewInfoByEmployeeAsync(employeeId, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "GetLastSalaryReviewInfoByEmployeeryAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadSalaryReviewExcelFile")]
        public async Task<IActionResult> DownloadSalaryReviewExcelFileAsync(string fileName)
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

        [HttpPost, Route("UploadSalaryReviewExcel")]
        public async Task<IActionResult> UploadSalaryReviewExcelAsync([FromForm] UploadSalaryReviewViewModel uploadedFile)
        {
            var appUser = AppUser();
            try
            {


                if (ModelState.IsValid)
                {
                    if (uploadedFile.ExcelFile?.Length > 0)
                    {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<UploadSalaryReviewReadDTO> salaryReviewDTOs = new List<UploadSalaryReviewReadDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var amt = worksheet.Cells[row, 2].Value.ToString();
                                if (amt != null)
                                {
                                    UploadSalaryReviewReadDTO salaryReviewDTO = new UploadSalaryReviewReadDTO();

                                    var employeeCode = worksheet.Cells[row, 1].Value.ToString();
                                    var amount = Convert.ToDecimal(worksheet.Cells[row, 2].Value?.ToString());
                                    var incrementReason = worksheet.Cells[row, 6].Value?.ToString() ?? "";

                                    var effectiveDate = worksheet.Cells[row, 3].Value?.ToString();
                                    if (effectiveDate.IsStringNumber() && effectiveDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        salaryReviewDTO.EffectiveFrom = DateTime.FromOADate(Convert.ToDouble(effectiveDate.RemoveWhitespace()));
                                    }
                                    else
                                    {
                                        salaryReviewDTO.EffectiveFrom = effectiveDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(effectiveDate.RemoveWhitespace()) : null;
                                    }

                                    var activationDate = worksheet.Cells[row, 4].Value?.ToString();
                                    if (activationDate.IsStringNumber() && activationDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        salaryReviewDTO.ActivationDate = DateTime.FromOADate(Convert.ToDouble(activationDate.RemoveWhitespace()));
                                    }
                                    else
                                    {
                                        salaryReviewDTO.ActivationDate = activationDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(activationDate.RemoveWhitespace()) : null;
                                    }

                                    var arrearCalculatedDate = worksheet.Cells[row, 5].Value?.ToString();
                                    if (arrearCalculatedDate.IsStringNumber() && arrearCalculatedDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        salaryReviewDTO.ArrearCalculatedDate = DateTime.FromOADate(Convert.ToDouble(arrearCalculatedDate.RemoveWhitespace()));
                                    }
                                    else
                                    {
                                        salaryReviewDTO.ArrearCalculatedDate = arrearCalculatedDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(arrearCalculatedDate.RemoveWhitespace()) : null;
                                    }


                                    salaryReviewDTO.EmployeeCode = employeeCode;
                                    salaryReviewDTO.CurrentSalaryAmount = amount;
                                    salaryReviewDTO.IncrementReason = incrementReason;

                                    salaryReviewDTOs.Add(salaryReviewDTO);
                                }
                                else
                                {
                                    continue;
                                }

                            }

                        }
                        var data = await _salaryReviewBusiness.UploadSalaryReviewExcelAsync(salaryReviewDTOs, appUser);
                        return Ok(data);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, appUser.Database, "SalaryReviewController", "UploadSalaryReviewExcelAsync", appUser);

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("DownloadSalaryReviewSheetInfos")]
        public async Task<IActionResult> DownloadSalaryReviewSheetInfosAsync([FromQuery] SalaryReviewSheet_Filter reviewSheet_Filter, string format)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var data = await _salaryReviewBusiness.GetSalaryReviewSheetAsync(reviewSheet_Filter, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "Salary Review Sheet");
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
                await _sysLogger.SavePayrollException(ex, user.Database, "SalaryReviewController", "UploadSalaryReviewExcelAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("DownloadSalaryReviewFlatAmountFile")]
        public async Task<IActionResult> DownloadSalaryReviewFlatAmountFileAsync(string fileName)
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

        [HttpGet("GetAllPendingSalaryReviewes")]
        public async Task<IActionResult> GetAllPendingSalaryReviewesAsync([FromQuery] SalaryReview_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryReviewBusiness.GetAllPendingSalaryReviewesAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "GetSalaryReviewInfos", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("DownloadSalaryReviewSheet")]
        public async Task<IActionResult> DownloadSalaryReviewSheetAsync([FromQuery] SalaryReviewSheetDownload_Filter model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryReviewBusiness.DownloadSalaryReviewSheetAsync(model, user);
                    if (data != null && data.Rows.Count > 0)
                    {
                        byte[] excelBytes = _excelGenerator.GenerateExcel(data, "Salary Review Sheet");

                        string fileName = "SalaryReviewSheet.xlsx";
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
                    return NotFound("No data to found to generate excel file");
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "GetSalaryReviewInfos", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeletePendingReview")]
        public async Task<IActionResult> DeletePendingReviewAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && id > 0)
                {
                    var status = await _salaryReviewBusiness.DeletePendingReviewAsync(id, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "DeletePendingReview", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeleteApprovedReview")]
        public async Task<IActionResult> DeleteApprovedReviewAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && id > 0)
                {
                    var status = await _salaryReviewBusiness.DeleteApprovedReviewAsync(id, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReviewController", "DeleteApprovedReview", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
