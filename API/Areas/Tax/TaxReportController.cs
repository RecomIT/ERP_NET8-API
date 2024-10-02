using API.Base;
using System.Data;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using System.IO.Compression;
using Shared.Payroll.DTO.Tax;
using Shared.OtherModels.User;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Info;
using BLL.Salary.Salary.Interface;
using BLL.Administration.Interface;
using Shared.OtherModels.EmailService;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.SqlServer.Server;
using BLL.Salary.Setup.Interface;
using Shared.Payroll.Domain.Setup;
using Shared.Separation.Filter;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxReportController : ApiBaseController
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly ILoginManager _loginManager;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private readonly ISysLogger _sysLogger;
        private readonly IReportBase _reportBase;
        private ExcelGenerator _excelGenerator;
        private AppUser _user;
        private TaxRelatedReportGenerator _taxRelatedReportGenerator;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly IInfoBusiness _infoBusiness;
        private readonly IFiscalYearBusiness _fiscalYearBusiness;

        private readonly ITaxReportBusiness _taxReportBusiness;
        public TaxReportController(ISysLogger sysLogger, IWebHostEnvironment webHostEnvironment,
            IReportConfigBusiness reportConfigBusiness,
            ISalaryReportBusiness salaryReportBusiness,
            ISalaryProcessBusiness salaryProcessBusiness,
             IInfoBusiness infoBusiness,
             IFiscalYearBusiness fiscalYearBusiness,
            ITaxReportBusiness taxReportBusiness,
            ILoginManager loginManager,
            IReportBase reportBase,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _webHostEnvironment = webHostEnvironment;
            _loginManager = loginManager;
            _reportConfigBusiness = reportConfigBusiness;
            _taxReportBusiness = taxReportBusiness;
            _salaryReportBusiness = salaryReportBusiness;
            _excelGenerator = new ExcelGenerator();
            _sysLogger = sysLogger;
            _reportBase = reportBase;
            _infoBusiness = infoBusiness;
            _fiscalYearBusiness = fiscalYearBusiness;
            _salaryProcessBusiness = salaryProcessBusiness;
            _taxRelatedReportGenerator = new TaxRelatedReportGenerator(_sysLogger, _reportBase, _infoBusiness, _taxReportBusiness, _webHostEnvironment, _reportConfigBusiness, _salaryReportBusiness);
        }

        [Route("TaxCardReport"), HttpGet]
        public async Task<IActionResult> TaxCardReportAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, string format, string password, bool isPasswordProtected)
        {
            _user = AppUser();
            try
            {
                var reportFile = await _taxRelatedReportGenerator.TaxCardReportAsync(employeeId, taxProcessId, fiscalYearId, month, year, null, format, password, isPasswordProtected, _user);
                if (reportFile != null && reportFile.FileBytes != null)
                {
                    return File(reportFile.FileBytes, reportFile.Mimetype, "TaxCard." + reportFile.Extension);
                }
                else
                {
                    return new EmptyResult();
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, _user.Database, "TaxReportController", "TaxCardReport", _user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetTaxcardInformation")]
        public async Task<IActionResult> GetTaxcardInformationAsync(long employeeId, short month, short year)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var taxCard = await _taxReportBusiness.TaxCardAsync(employeeId, 0, 0, month, year, null, user);
                    return Ok(taxCard);
                }
                return BadRequest(new { meassage = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReportController", "GetTaxcardInformation", _user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("PWCTaxCertificateFY2223")]
        public async Task<IActionResult> PWCTaxCertificateFY2223()
        {
            var user = AppUser();
            try
            {
                var employeeInfo = await _taxReportBusiness.TaxCardInfoFY22_23(user);
                var challanInfo = await _taxReportBusiness.TaxChallanFY22_23(user);
                var reportLayers = new List<ReportLayer>();
                var reportLayer = Task.Run(() => _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, 0)).Result;
                reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                reportLayers.Add(reportLayer);
                var emailSetting = await _loginManager.EmailSettings("Send");

                List<string> unsuccessfulEmailList = new List<string>();
                List<string> successfulEmailList = new List<string>();
                List<string> emailNotFoundList = new List<string>();
                List<string> fileNotFoundList = new List<string>();
                //employeeInfo.Rows.Count
                if (employeeInfo.Rows.Count > 0)
                {
                    for (int i = 0; i < employeeInfo.Rows.Count; i++)
                    {

                        var employeeEmail = employeeInfo.Rows[i]["Email"].ToString();
                        employeeInfo.Rows[i]["InWord"] = NumberToWords.Input(Convert.ToDecimal(employeeInfo.Rows[i]["MonthlyTax"]));

                        var employeeCode = employeeInfo.Rows[i]["EmployeeCode"].ToString();
                        if (!Utility.IsNullEmptyOrWhiteSpace(employeeEmail))
                        {

                            List<byte[]> files = new List<byte[]>();
                            List<string> attachmentNames = new List<string>();

                            var thisEmployee = new DataTable();
                            #region Single Employee Value
                            foreach (DataColumn column in employeeInfo.Columns)
                            {
                                thisEmployee.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
                            }
                            DataRow sourceRow = employeeInfo.Rows[i];
                            DataRow destinationRow = thisEmployee.NewRow();
                            foreach (DataColumn column in employeeInfo.Columns)
                            {
                                destinationRow[column.ColumnName] = sourceRow[column.ColumnName];
                            }
                            thisEmployee.Rows.Add(destinationRow);
                            #endregion

                            EmailReceiverObject emailReceiver = new EmailReceiverObject();
                            emailReceiver.RecipientName = thisEmployee.Rows[0]["EmployeeName"].ToString();
                            emailReceiver.MailAddress = "md.ahsan.uz.zaman.tpr@pwc.com"; //thisEmployee.Rows[0]["Email"].ToString();
                            emailReceiver.Subject = "Salary Tax Certificate | FY 2022-2023";

                            files.Add(await _taxRelatedReportGenerator.GetTaxCertificateFY2223(thisEmployee, challanInfo, reportLayers, user));
                            attachmentNames.Add(string.Format(@"TaxCertificate FY 22-23.pdf"));

                            emailReceiver.Files = files;
                            emailReceiver.AttachmentNames = attachmentNames;

                            if (emailReceiver.Files.Count > 0)
                            {
                                emailSetting.DisplayName = string.Format(@"Salary Tax Certificate | FY 2022-2023");
                                emailSetting.EmailHtmlBody = EmailTemplate.SendTaxCard(emailReceiver.RecipientName, "2022-2023");
                                if (await EmailSender.SendMail(emailSetting, emailReceiver) == false)
                                {
                                    unsuccessfulEmailList.Add(employeeCode + " unsuccessful not have any file");
                                }
                                else
                                {
                                    successfulEmailList.Add(employeeCode);
                                }
                            }
                            else
                            {
                                fileNotFoundList.Add(employeeCode + " does not have any file");
                            }
                        }
                        else
                        {
                            emailNotFoundList.Add(employeeCode + " email not found");
                        }
                    }
                    return Ok(new
                    {
                        totalsuccessful = successfulEmailList.Count,
                        successful = successfulEmailList,
                        totalunsuccessful = unsuccessfulEmailList.Count,
                        unsuccessful = unsuccessfulEmailList,
                        totalfileNotFound = fileNotFoundList.Count,
                        fileNotFound = fileNotFoundList,
                        totalemailNotFound = emailNotFoundList.Count,
                        emailNotFound = emailNotFoundList
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, _user.Database, "TaxReportController", "PWCTaxCertificateFY2223", _user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
            return BadRequest(ResponseMessage.SomthingWentWrong);
        }

        //Added by Monzur 05-Nov-23

        [HttpGet, Route("DownloadTaxSheetDetails")]
        public async Task<IActionResult> DownloadTaxSheetDetailsAsync(long employeeId, long fiscalYearId, short salaryMonth, short salaryYear, long salaryProcessId, long salaryProcessDetailId, string format)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var data = await _taxReportBusiness.GetTaxSheetDetailsReport(employeeId, fiscalYearId, salaryMonth, salaryYear, salaryProcessId, salaryProcessDetailId, user);
                    byte[] excelBytes = _excelGenerator.GenerateExcel(data, "108 Report");
                    format = (format == "xlsx" || format == "xls") ? format : "xlsx";
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessController", "DownloadTaxSheetDetailsAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("DownloadTaxDeductionExcel")]
        public async Task<IActionResult> DownloadTaxDeductionExcelAsync(string fileName)
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

        [HttpPost, Route("UploadActualTaxDeductionExcel")]
        public async Task<IActionResult> UploadActualTaxDeductionAmountAsync([FromForm] UploadActualTaxDeductionDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.File?.Length > 0)
                    {

                        var stream = model.File.OpenReadStream();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            List<ActualTaxDeductionDTO> list = new List<ActualTaxDeductionDTO>();
                            for (var row = 2; row <= rowCount; row++)
                            {
                                ActualTaxDeductionDTO actualTax = new ActualTaxDeductionDTO();
                                actualTax.EmployeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                actualTax.ActualTaxAmount = Convert.ToDecimal(worksheet.Cells[row, 2].Value?.ToString());
                                actualTax.SalaryMonth = model.SalaryMonth;
                                actualTax.SalaryYear = model.SalaryYear;
                                list.Add(actualTax);
                            }

                            //list = list.Where(i => i.ActualTaxAmount > 0).ToList();
                            var data = await _taxReportBusiness.UploadActaulTaxDeductionAsync(list, user);
                            return Ok(data);
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxProcessController", "SaveUploadActualTaxDeductionAmountAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("UpdateActaulTaxDeductionInSalaryAndTax")]
        public async Task<IActionResult> UpdateActaulTaxDeductionInSalaryAndTaxAsync([FromForm] UpdateActaulTaxDeductedDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _taxReportBusiness.UpdateActaulTaxDeductionInSalaryAndTaxAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReportController", "UpdateActaulTaxDeductionInSalaryAndTax", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("BulkTaxCardDownload")]
        public async Task<IActionResult> BulkTaxCardDownload([FromQuery] BulkTaxCardDownload filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var salaryDetails = (await _salaryProcessBusiness.GetSalaryProcessDetailsAsync(
                        filter.SalaryProcessId, 0, 0, 0, filter.Month, filter.Year, 0, null, user)).ToList();
                    if (salaryDetails.Any())
                    {
                        var taxReportConfig = await _reportConfigBusiness.ReportConfigAsync("TaxCard", null, user.CompanyId, user.OrganizationId);
                        using (MemoryStream ms = new MemoryStream())
                        {
                            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
                            {
                                foreach (var info in salaryDetails)
                                {
                                    var reportFile = _taxRelatedReportGenerator.TaxCardReportAsync(
                                        info.EmployeeId, 0, 0, filter.Month, filter.Year, null, ".pdf", info.EmployeeCode, false, user,
                                        taxReportConfig).Result;

                                    var entry = zip.CreateEntry(info.EmployeeCode + ".pdf");
                                    using (var fileStream = new MemoryStream(reportFile.FileBytes))
                                    {
                                        using (var entryStream = entry.Open())
                                        {
                                            fileStream.CopyTo(entryStream);
                                        }
                                    }
                                }
                                var month = Utility.GetMonthName(filter.Month) + "_" + filter.Year.ToString();
                                return File(ms.ToArray(), "application/zip", ("Bulk_TaxCard_" + month) + ".zip");
                            }
                        }
                        return BadRequest("");
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "GetSalarySheet", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("DownloadFinalTaxCard")]
        public async Task<IActionResult> DownloadFinalTaxCardAsync(long employeeId, long fiscalYearId, short year)
        {
            var user = AppUser();
            _user = user;
            try
            {
                if (user.HasBoth)
                {
                    if (employeeId == 0)
                    {
                        return BadRequest("Employee id is missing");
                    }
                    if (fiscalYearId == 0)
                    {
                        return BadRequest("Income year is missing");
                    }
                    var reportFile = await _taxRelatedReportGenerator.FinalTaxCardReportAsync(employeeId, fiscalYearId, year, null, false, _user);
                    if (reportFile != null && reportFile.FileBytes != null)
                    {
                        return File(reportFile.FileBytes, reportFile.Mimetype, "Final_Tax_Card.pdf");
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("DownloadSupplementaryTaxCard")]
        public async Task<IActionResult> DownloadSupplementaryTaxCardAsync(long employeeId, long fiscalYearId, long paymentAmountId, short year)
        {
            var user = AppUser();
            _user = user;
            try
            {
                if (user.HasBoth)
                {
                    if (employeeId == 0)
                    {
                        return BadRequest("Employee id is required");
                    }
                    if (fiscalYearId == 0)
                    {
                        return BadRequest("Income year is required");
                    }
                    if(paymentAmountId == 0)
                    {
                        return BadRequest("Payment id is required");
                    }
                    var reportFile = await _taxRelatedReportGenerator.SupplementaryTaxCardReportAsync(employeeId, fiscalYearId, paymentAmountId, year, null, false, _user);
                    if (reportFile != null && reportFile.FileBytes != null)
                    {
                        return File(reportFile.FileBytes, reportFile.Mimetype, "Supplementary_Tax_Card.pdf");
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }
                return Ok("");
            }
            catch (Exception ex)
            {
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GenerateFinalTaxCard")]
        public async Task<IActionResult> GenerateFinalTaxCardAsync(int year, int month)
        {
            var user = AppUser();
            try
            {
                if (year == 0)
                {
                    return BadRequest("Year is required");
                }
                if (month == 0)
                {
                    return BadRequest("Year is required");
                }
                var date = DateTimeExtension.FirstDateOfAMonth(year, month);
                var fiscalYear = await _fiscalYearBusiness.GetFiscalYearInfoWithinADate(date.ToString("yyyy-MM-dd"), user);
                if (fiscalYear != null)
                {
                    var reportFile = await _taxRelatedReportGenerator.FinalTaxCardReportAsync(user.EmployeeId, fiscalYear.FiscalYearId, (short)year, null, false, user);
                    if (reportFile != null && reportFile.FileBytes != null)
                    {
                        return File(reportFile.FileBytes, reportFile.Mimetype, "Final_Tax_Card.pdf");
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }
                else
                {
                    return BadRequest($"Could not find income year within {Utility.GetMonthName((short)month)} {year.ToString()}");
                }
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GenerateEmployeeFinalTaxCard")]
        public async Task<IActionResult> GenerateEmployeeFinalTaxCardAsync(long employeeId,int year, int month)
        {
            var user = AppUser();
            try
            {
                if (employeeId == 0)
                {
                    return BadRequest("Employee is required");
                }
                if (year == 0)
                {
                    return BadRequest("Year is required");
                }
                if (month == 0)
                {
                    return BadRequest("Year is required");
                }
                var date = DateTimeExtension.FirstDateOfAMonth(year, month);
                var fiscalYear = await _fiscalYearBusiness.GetFiscalYearInfoWithinADate(date.ToString("yyyy-MM-dd"), user);
                if (fiscalYear != null)
                {
                    var reportFile = await _taxRelatedReportGenerator.FinalTaxCardReportAsync(employeeId, fiscalYear.FiscalYearId, (short)year, null, false, user);
                    if (reportFile != null && reportFile.FileBytes != null)
                    {
                        return File(reportFile.FileBytes, reportFile.Mimetype, "Final_Tax_Card.pdf");
                    }
                    else
                    {
                        return new EmptyResult();
                    }
                }
                else
                {
                    return BadRequest($"Could not find income year within {Utility.GetMonthName((short)month)} {year.ToString()}");
                }
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


    }

}