using API.Base;
using Spire.Pdf;
using System.Data;
using System.Text;
using API.Services;
using OfficeOpenXml;
using Shared.Services;
using BLL.Base.Interface;
using BLL.Tax.Interface;
using Spire.Pdf.Conversion;
using AspNetCore.Reporting;
using API.Areas.Tax;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using Shared.OtherModels.EmailService;
using Microsoft.AspNetCore.Authorization;
using BLL.Administration.Interface;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.DTO.Salary;
using BLL.Employee.Interface.Info;

namespace API.Areas.Salary.Process
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SalaryReportController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private readonly ITaxReportBusiness _taxReportBusiness;
        private readonly ISalaryProcessBusiness _salaryProcessBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly ILoginManager _loginManager;
        private readonly IReportBase _reportBase;
        private readonly IInfoBusiness _infoBusiness;
        private SalaryRelatedReportGenerator _salaryRelatedReportGenerator;
        private TaxRelatedReportGenerator _taxRelatedReportGenerator;
        private ExcelGenerator _excelGenerator;
        private ReportFile _reportFile;
        public SalaryReportController(ISysLogger sysLogger,
            IWebHostEnvironment webHostEnvironment,
            ISalaryReportBusiness salaryReportBusiness,
            IReportConfigBusiness reportConfigBusiness,
            ISalaryProcessBusiness salaryProcessBusiness,
            ILoginManager loginManager,
            IBranchInfoBusiness branchInfoBusiness,
            ITaxReportBusiness taxReportBusiness,
            IReportBase reportBase,
            IInfoBusiness infoBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _salaryReportBusiness = salaryReportBusiness;
            _webHostEnvironment = webHostEnvironment;
            _reportConfigBusiness = reportConfigBusiness;
            _salaryProcessBusiness = salaryProcessBusiness;
            _loginManager = loginManager;
            _reportBase = reportBase;
            _branchInfoBusiness = branchInfoBusiness;
            _taxReportBusiness = taxReportBusiness;
            _infoBusiness = infoBusiness;
            _excelGenerator = new ExcelGenerator();
            _salaryRelatedReportGenerator = new SalaryRelatedReportGenerator(_webHostEnvironment, _sysLogger, _reportConfigBusiness, _salaryReportBusiness, _branchInfoBusiness);
            _taxRelatedReportGenerator = new TaxRelatedReportGenerator(_sysLogger, _reportBase, _infoBusiness, _taxReportBusiness, _webHostEnvironment, reportConfigBusiness, _salaryReportBusiness);
        }

        [HttpGet("PayslipExtension")]
        public async Task<IActionResult> PayslipExtensionAsync(long employeeId, short month, short year)
        {
            var user = AppUser();
            try
            {
                if (employeeId > 0 && month > 0 && year > 0 && user.HasBoth)
                {
                    var data = await _salaryReportBusiness.PayslipExtensionAsync(employeeId, month, year, user);
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

        [HttpGet, Route("DownloadActualSalarySheet")]
        public async Task<IActionResult> DownloadActualSalarySheetAsync([FromQuery] ActualSalarySheetDetail_Filter filter)
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

                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("SalarySheet", "", user.CompanyId, user.OrganizationId);

                    if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }
                    else
                    {
                        if (user.CompanyId == 18 && user.OrganizationId == 11)
                        {
                            if (filter.SalaryType == "Intern")
                            {
                                path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Payroll\\Salary\\Nagad\\salary_sheet_intern_nagad.rdlc";
                            }
                            else if (filter.SalaryType == "NewJoinerAndSeparation")
                            {
                                path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Payroll\\Salary\\Nagad\\salary_sheet_new_joiner_and_separated_nagad.rdlc";
                            }
                            else if (filter.SalaryType == "InternNewJoinerAndSeparation")
                            {
                                path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Payroll\\Salary\\Nagad\\salary_sheet_intern_new_joiner_and_separated_nagad.rdlc";
                            }
                            else
                            {
                                path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Payroll\\Salary\\Nagad\\salary_sheet_nagad.rdlc";
                            }
                        }
                    }

                    var datatable = await _salaryReportBusiness.GetActualSalarySheetDetailAsync(filter, user);

                    var reportLayers = new List<ReportLayer>();
                    var branchId = Utility.TryParseLong(filter.BranchId);

                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, branchId > 0 ? branchId : user.BranchId, 0);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    LocalReport localReport = new LocalReport(path);

                    if (datatable.Rows.Count > 0)
                    {
                        localReport.AddDataSource("ReportLayer", reportLayers);
                        localReport.AddDataSource("SalarySheetInfo", datatable);

                        if (user.OrganizationId == 11 && user.CompanyId == 19 || user.OrganizationId == 5 && user.CompanyId == 3)
                        {
                            //
                            var permanent_employee_query = from r in datatable.AsEnumerable()
                                                           where r.Field<string>("JobType") == "Permanent"
                                                           select r;
                            DataTable permanent_employee_dt = new DataTable();
                            permanent_employee_dt = permanent_employee_query.CopyToDataTable();

                            var contractual_employee_query = from r in datatable.AsEnumerable()
                                                             where r.Field<string>("JobType") == "Contractual"
                                                             select r;
                            DataTable contractual_employee_dt = new DataTable();
                            contractual_employee_dt = contractual_employee_query.CopyToDataTable();

                            localReport.AddDataSource("Permanent", permanent_employee_dt);
                            localReport.AddDataSource("Contractual", contractual_employee_dt);

                            // Reconcilition Report
                            var reconciliation = await _salaryReportBusiness.ReconciliationRptAsync(new Reconciliation_Filter
                            {
                                BranchId = filter.BranchId,
                                SalaryMonth = filter.SalaryMonth,
                                SalaryYear = filter.SalaryYear,
                            }, user);

                            localReport.AddDataSource("Reconciliation", reconciliation);

                            var permanent_employee_reconciliation = await _salaryReportBusiness.SalaryBreakdownRptAsync(new SalaryBreakdown_Filter
                            {
                                BranchId = filter.BranchId,
                                SalaryMonth = filter.SalaryMonth,
                                SalaryYear = filter.SalaryYear,
                                JobType = "Permanent"
                            }, user);
                            localReport.AddDataSource("Permanent_Employee_Reconciliation", permanent_employee_reconciliation);


                            var contractual_employee_reconciliation = await _salaryReportBusiness.SalaryBreakdownRptAsync(new SalaryBreakdown_Filter
                            {
                                BranchId = filter.BranchId,
                                SalaryMonth = filter.SalaryMonth,
                                SalaryYear = filter.SalaryYear,
                                JobType = "Contractual"
                            }, user);
                            localReport.AddDataSource("Contractual_Employee_Reconciliation", contractual_employee_reconciliation);
                        }

                        var result = localReport.Execute(RenderType.ExcelOpenXml, 1);

                        var renderBytes = result.MainStream;

                        Response.Headers.Add("File-Name", "SalarySheet");
                        return File(renderBytes, "application/vnd.ms-excel", "SalarySheet." + extension);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch(Exception ex)
            {
                //await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "DownloadActualSalarySheet", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadActualSalarySheetNew")]
        public async Task<IActionResult> DownloadActualSalarySheetNewAsync([FromQuery] ActualSalarySheetDetail_Filter filter)
        {
            var user = AppUser();
            _reportFile = new ReportFile();
            try
            {
                if (user.HasBoth)
                {
                    string renderFormat = filter.Format.ToUpper();
                    string mimetype = Utility.GetFileMimetype(filter.Format);
                    string extension = filter.Format.ToLower();
                    var path = "";

                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("SalarySheet", "", user.CompanyId, user.OrganizationId);

                    if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }
                    else
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\payroll\\payroll_salary_sheet_pwc.rdlc";
                    }
                    var datatable = await _salaryReportBusiness.GetActualSalarySheetDetailAsync(filter, user);

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, 0);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    Microsoft.Reporting.NETCore.LocalReport localReport = new Microsoft.Reporting.NETCore.LocalReport();

                    if (datatable.Rows.Count > 0)
                    {

                        Microsoft.Reporting.NETCore.ReportDataSource reportDataSource1 = new Microsoft.Reporting.NETCore.ReportDataSource();
                        reportDataSource1.Name = "ReportLayer";
                        reportDataSource1.Value = reportLayers;

                        Microsoft.Reporting.NETCore.ReportDataSource reportDataSource2 = new Microsoft.Reporting.NETCore.ReportDataSource();
                        reportDataSource2.Name = "SalarySheetInfo";
                        reportDataSource2.Value = datatable;

                        localReport.DataSources.Clear();
                        localReport.DataSources.Add(reportDataSource1);
                        localReport.DataSources.Add(reportDataSource2);
                        localReport.Refresh();

                        localReport.ReportPath = path;
                        var pdf = localReport.Render("pdf");

                        _reportFile.FileBytes = pdf;
                        _reportFile.Extension = extension;
                        _reportFile.Mimetype = mimetype;

                        PdfDocument pdfDocument = new PdfDocument();
                        pdfDocument.LoadFromBytes(pdf);
                        XlsxLineLayoutOptions options = new XlsxLineLayoutOptions(false, true, true, true);
                        pdfDocument.ConvertOptions.SetPdfToXlsxOptions(options);

                        byte[] fileByte;
                        using (MemoryStream finalExcelFile = new MemoryStream())
                        {
                            pdfDocument.SaveToStream(finalExcelFile, FileFormat.XLSX);

                            fileByte = finalExcelFile.ToArray();
                        }
                        Response.Headers.Add("File-Name", "SalarySheet");
                        return File(fileByte, "application/vnd.ms-excel", "SalarySheet." + "pdf");
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "DownloadActualSalarySheet", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadSalarySheet")]
        public async Task<IActionResult> DownloadSalarySheetAsync([FromQuery] SalarySheet_Filter model, string format = "xlsx")
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
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

        [HttpGet, Route("PayslipFromExternalSource")]
        public async Task<IActionResult> PayslipFromExternalSourceAsync(string employeeCode, short month, short year)
        {
            string mimetype = "application/pdf";
            string extension = "pdf";
            var user = AppUser();
            try
            {
                var URL = $"{AppSettings.PayslipApiURL}?emp_no=" + employeeCode + "&year=" + year + "&month=" + month;
                using (var httpClient = new HttpClient())
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, URL))
                    {
                        requestMessage.Headers.Add("Client_Token", AppSettings.PayslipApiKey);
                        var response = await httpClient.SendAsync(requestMessage);
                        var file = await response.Content.ReadAsByteArrayAsync();
                        return File(file, mimetype, "payslip." + extension);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "PayslipFromExternalSource", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("TaxCardFromExternalSource")]
        public async Task<IActionResult> TaxCardFromExternalSourceAsync(string employeeCode, short month, short year)
        {
            string mimetype = "application/pdf";
            string extension = "pdf";
            var user = AppUser();
            try
            {
                var URL = $"{AppSettings.TaxCardURL}?emp_no=" + employeeCode + "&year=" + year + "&month=" + month;
                using (var httpClient = new HttpClient())
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, URL))
                    {
                        requestMessage.Headers.Add("Client_Token", AppSettings.PayslipApiKey);
                        var response = await httpClient.SendAsync(requestMessage);
                        var file = await response.Content.ReadAsByteArrayAsync();
                        return File(file, mimetype, "tax-card." + extension);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "TaxCardFromExternalSource", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("BonusPayslipFromExternalSource")]
        public async Task<IActionResult> BonusPayslipFromExternalSourceAsync(string employeeCode, short month, short year)
        {
            string mimetype = "application/pdf";
            string extension = "pdf";
            var user = AppUser();
            try
            {
                var URL = $"{AppSettings.BonusPayslipApiURL}?emp_no=" + employeeCode + "&year=" + year + "&month=" + month;
                using (var httpClient = new HttpClient())
                {
                    using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, URL))
                    {
                        requestMessage.Headers.Add("Client_Token", AppSettings.PayslipApiKey);
                        var response = await httpClient.SendAsync(requestMessage);
                        var file = await response.Content.ReadAsByteArrayAsync();
                        return File(file, mimetype, "bonus-payslip." + extension);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "BonusPayslipFromExternalSource", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetSalarySheet")]
        public async Task<IActionResult> GetSalarySheetAsync([FromQuery] SalarySheet_Filter model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryReportBusiness.GetSalarySheetAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "GetSalarySheet", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("PayslipOrTaxCardEmailing")]
        public async Task<IActionResult> PayslipOrTaxCardEmailing([FromQuery] PayslipTaxCardEmailing model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    List<string> unsuccessfulEmailList = new List<string>();
                    List<string> successfulEmailList = new List<string>();
                    List<string> emailNotFoundList = new List<string>();
                    List<string> fileNotFoundList = new List<string>();
                    var salaryDetails = (await _salaryProcessBusiness.GetSalaryProcessDetailsAsync(0, 0, 0, 0, model.Month, model.Year, 0, null, user)).ToList();

                    var emailSetting = await _loginManager.EmailSettings("Send");
                    if (salaryDetails != null && salaryDetails.Count() > 0)
                    {
                        var salaryReportConfig = await _reportConfigBusiness.ReportConfigAsync("Payslip", null, user.CompanyId, user.OrganizationId);
                        string payslip_sp_name = ReportingHelper.ReportProcess("sp_Payroll_RptPayslip_Extension", user);

                        var fiscalYearRange = ReportingHelper.FiscalYearRange(model.Month, model.Year);

                        var taxReportConfig = await _reportConfigBusiness.ReportConfigAsync("TaxCard", null, user.CompanyId, user.OrganizationId);
                        string taxCard_sp_name = ReportingHelper.ReportProcess("sp_Payroll_RptPayslip_Extension", user);

                        foreach (var info in salaryDetails)
                        {
                            EmailReceiverObject receiver = new EmailReceiverObject();
                            receiver.RecipientName = info.EmployeeName;
                            receiver.MailAddress = info.Email != null ? info.Email.Trim() : null;
                            
                            if (!Utility.IsNullEmptyOrWhiteSpace(receiver.MailAddress))
                            {
                                receiver.Subject =
                                model.ReportFileName == "Payslip" ? "Payslip " + Utility.GetMonthName(model.Month) + "-" + model.Year.ToString() : model.ReportFileName == "TaxCard" ? "TaxCertificate " + Utility.GetMonthName(model.Month) + "-" + model.Year.ToString() : "Payslip & TaxCertificate " + Utility.GetMonthName(model.Month) + "-" + model.Year.ToString();

                                List<byte[]> files = new List<byte[]>();
                                List<string> attachmentNames = new List<string>();

                                if (model.ReportFileName == "Payslip" || model.ReportFileName == "Both")
                                {
                                    var reportFile = _salaryRelatedReportGenerator.GetPaySlip(
                                        new Payslip_Filter()
                                        {
                                            EmployeeId = info.EmployeeId.ToString(),
                                            Month = info.SalaryMonth.ToString(),
                                            Year = info.SalaryYear.ToString(),
                                            Format = model.FileFormat,
                                        }
                                        , info.EmployeeCode, model.WithPasswordProtected, user, salaryReportConfig, payslip_sp_name).Result;
                                    files.Add(reportFile.FileBytes);
                                    attachmentNames.Add("Payslip " + Utility.GetMonthName(model.Month) + "-" + model.Year.ToString() + "." + model.FileFormat.ToLower());
                                }
                                if (model.ReportFileName == "TaxCard" || model.ReportFileName == "Both")
                                {
                                    var reportFile = _taxRelatedReportGenerator.TaxCardReportAsync(info.EmployeeId, 0, 0, model.Month, model.Year, null, model.FileFormat, info.EmployeeCode, model.WithPasswordProtected, user, taxReportConfig).Result;
                                    if(reportFile.FileBytes != null && reportFile.FileBytes.Length > 0) {
                                        files.Add(reportFile.FileBytes);
                                        attachmentNames.Add("TaxCertificate " + Utility.GetMonthName(model.Month) + "-" + model.Year.ToString() + "." + model.FileFormat.ToLower());
                                    }
                                }

                                receiver.Files = files;
                                receiver.AttachmentNames = attachmentNames;

                                if (receiver.Files.Count > 0)
                                {
                                    emailSetting.DisplayName = string.Format(@"Payslip for {0}-{1}", Utility.GetMonthName(model.Month), model.Year.ToString());
                                    emailSetting.EmailHtmlBody = EmailTemplate.SendPayslipAndTaxCard(receiver.RecipientName, Utility.GetMonthName(model.Month), model.Year.ToString());
                                    if (await EmailSender.SendMail(emailSetting, receiver) == true)
                                    {
                                        successfulEmailList.Add(info.EmployeeCode + " successfully sent");
                                    }
                                    else
                                    {
                                        unsuccessfulEmailList.Add(info.EmployeeCode + " unsuccessful not have any file");
                                    }
                                }
                                else
                                {
                                    fileNotFoundList.Add(info.EmployeeCode + " does not have any file");
                                }
                            }
                            else
                            {
                                emailNotFoundList.Add(info.EmployeeCode + " email not found");
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
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "PayslipOrTaxCardEmailing", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("ReconciliationRpt")]
        public async Task<IActionResult> ReconciliationRptAsync([FromQuery] Reconciliation_Filter filter)
        {
            var user = AppUser();
            try
            {
                var updateData = await _salaryReportBusiness.UpdateAggregateSumInSalaryProcessAsync(filter, user);
                if (updateData.Status == true)
                {
                    var data = await _salaryRelatedReportGenerator.GetReconciliationRpt(filter, null, false, user);
                    return File(data.FileBytes, data.Mimetype, "reconciliation." + data.Extension);
                }
                else
                {
                    return Ok(updateData);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "ReconciliationRpt", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet("SalaryBreakdownRpt")]
        public async Task<IActionResult> SalaryBreakdownRptAsync([FromQuery] SalaryBreakdown_Filter filter)
        {
            var user = AppUser();
            try
            {
                var data = await _salaryRelatedReportGenerator.GetSalaryBreakdownRpt(filter, null, false, user);
                return File(data.FileBytes, data.Mimetype, "SalaryBreakdown." + data.Extension);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "SalaryBreakdownRpt", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("SalaryReconciliationRpt")]
        public async Task<IActionResult> SalaryReconciliationRptAsync([FromQuery] Reconciliation_Filter filter)
        {
            var user = AppUser();
            try
            {
                var data = await _salaryRelatedReportGenerator.GetSalaryReconciliationRpt(filter, null, false, user);
                return File(data.FileBytes, data.Mimetype, "Reconciliation." + data.Extension);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "SalaryBreakdownRpt", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadBankStatement")]
        public async Task<IActionResult> DownloadBankStatementAsync([FromQuery] BankStatement_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _salaryReportBusiness.GetBankStatementAsync(filter, user);
                    Dictionary<string, string> dictionary = new Dictionary<string, string>();
                    dictionary.Add("Transaction Reference", "Text1");
                    dictionary.Add("Beneficiary A/C No", "Text");
                    byte[] fileBytes = _excelGenerator.GenerateExcel(data, "Bank_Statement_Sheet", null, dictionary);
                    filter.Format = filter.Format == "xlsx" || filter.Format == "xls" || filter.Format == "csv" ? filter.Format : "xlsx";
                    filter.Format = "xlsx";
                    string fileName = "data." + filter.Format;
                    string contentType = "";
                    if (filter.Format == "csv")
                    {
                        contentType = "text/csv";
                    }
                    else
                    {
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    }
                    using (var package = new ExcelPackage(new MemoryStream(fileBytes)))
                    {

                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (filter.Format == "csv")
                        {
                            var csvData = new StringBuilder();
                            for (int row = 1; row <= worksheet.Dimension.End.Row; row++)
                            {
                                for (int col = 1; col <= worksheet.Dimension.End.Column; col++)
                                {
                                    var cellValue = worksheet.Cells[row, col].Value?.ToString() ?? "";
                                    csvData.Append($"{cellValue},");
                                }
                                csvData.AppendLine();
                            }
                            fileBytes = Encoding.UTF8.GetBytes(csvData.ToString());
                        }
                        else
                        {
                            fileBytes = package.GetAsByteArray();
                        }
                    }
                    HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
                    HttpContext.Response.ContentType = contentType;
                    return File(fileBytes, contentType);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryReportController", "GetSalarySheet", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
