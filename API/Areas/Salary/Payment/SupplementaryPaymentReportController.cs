using API.Base;
using API.Services;
using BLL.Administration.Interface;
using BLL.Base.Interface;
using BLL.Salary.Payment.Interface;
using BLL.Salary.Salary.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using Shared.Control_Panel.ViewModels;
using Shared.Helpers;
using Shared.OtherModels.EmailService;
using Shared.OtherModels.Report;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Services;
using System.Data;

namespace API.Areas.Salary.Payment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SupplementaryPaymentReportController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILoginManager _loginManager;

        private readonly ISupplementaryPaymentReportBusiness _supplementaryPaymentReportBusiness;
        private AppUser _user;

        public SupplementaryPaymentReportController(
            ISysLogger sysLogger,
            ILoginManager loginManager,
            ISupplementaryPaymentReportBusiness supplementaryPaymentReportBusiness,
            IWebHostEnvironment webHostEnvironment,
            IReportConfigBusiness reportConfigBusiness,
            IBranchInfoBusiness branchInfoBusiness,
            ISalaryReportBusiness salaryReportBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _reportConfigBusiness = reportConfigBusiness;
            _webHostEnvironment = webHostEnvironment;
            _branchInfoBusiness = branchInfoBusiness;
            _salaryReportBusiness = salaryReportBusiness;
            _loginManager = loginManager;
            _supplementaryPaymentReportBusiness = supplementaryPaymentReportBusiness;
        }

        [HttpGet("DownloadPayslip")]
        public async Task<IActionResult> DownloadPayslip([FromHeader] SupplementaryPaymentReport_Filter filter)
        {
            var user = AppUser();
            //byte[] bytes = null;
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    _user = user;
                    string renderFormat = "PDF";
                    string mimetype = "application/pdf";
                    string extension = "pdf";
                    var path = string.Empty;
                    var reportConfig = await _reportConfigBusiness.ReportConfigAsync("Supplementary Payslip", null, user.CompanyId, user.OrganizationId);
                    if (reportConfig == null)
                    {
                        //path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Recom\\Salary\\payroll_supplementary_payslip_extension_recom.rdlc";
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\PWC\\Salary\\payroll_supplementary_payslip.rdlc";
                    }
                    else
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }

                    LocalReport localReport = new LocalReport();

                    var payslipInfo = await _supplementaryPaymentReportBusiness.PayslipInfo(filter, user);
                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, user.DivisionId);
                    var reportLayers = new List<ReportLayer>();
                    reportLayers.Add(reportLayer);
                    if (payslipInfo != null && payslipInfo.Rows.Count > 0)
                    {
                        payslipInfo.Rows[0]["InWord"] = NumberToWords.Input(Convert.ToDecimal(payslipInfo.Rows[0]["DisbursedAmount"]));
                        payslipInfo.Rows[0]["MonthName"] = Utility.GetMonthName(filter.PaymentMonth);
                        if (reportLayer != null)
                        {
                            payslipInfo.Rows[0]["CompanyName"] = reportLayer.CompanyName;

                            payslipInfo.Columns.Add("BranchLogo", typeof(byte[]));
                            payslipInfo.Columns.Add("ReportLogo", typeof(byte[]));

                            payslipInfo.Rows[0]["ReportLogo"] = reportLayer.ReportLogo;
                            payslipInfo.Rows[0]["BranchLogo"] = reportLayer.BranchLogo;

                            payslipInfo.Rows[0]["Address"] = reportLayer.Address;
                        }
                        DataColumnCollection columns = payslipInfo.Columns;
                        if (columns.Contains("BranchId"))
                        {
                            if (payslipInfo.Rows[0]["BranchId"] != null)
                            {
                                long branchId = Utility.TryParseLong(payslipInfo.Rows[0]["BranchId"].ToString());
                                var branches = await _branchInfoBusiness.GetBranchsAsync(null, user);
                                if (branches.Any() && branchId > 0)
                                {
                                    var branch = branches.FirstOrDefault(item => item.BranchId == branchId);
                                    if (branch != null)
                                    {
                                        payslipInfo.Rows[0]["BranchName"] = branch.BranchName;
                                    }
                                }
                            }
                        }

                        ReportDataSource reportDataSource1 = new ReportDataSource();
                        reportDataSource1.Name = "PayslipInfo";
                        reportDataSource1.Value = payslipInfo;

                        ReportDataSource reportDataSource2 = new ReportDataSource();
                        reportDataSource2.Name = "ReportLayer";
                        reportDataSource2.Value = reportLayers;

                        localReport.SubreportProcessing += new SubreportProcessingEventHandler(PayslipDetailSubreportProcessingEventHandler);
                        localReport.DataSources.Clear();
                        localReport.DataSources.Add(reportDataSource1);
                        localReport.DataSources.Add(reportDataSource2);
                        localReport.Refresh();

                        localReport.ReportPath = path;

                        var pdf = localReport.Render(renderFormat);

                        return File(pdf, mimetype, "payslip." + extension);
                    }
                    return BadRequest("No data found to generate payslip");

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentReport", "DownloadPayslip", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }
        internal async Task<ReportFile> GetPaySlip(DataTable payslipInfo, DataTable reportLayer, string reportPath, string password, bool isPasswordProtected, AppUser user, ReportConfigViewModel reportConfig = null)
        {
            ReportFile reportFile = new ReportFile();
            try
            {
                LocalReport localReport = new LocalReport();
                ReportDataSource reportDataSource1 = new ReportDataSource();
                reportDataSource1.Name = "PayslipInfo";
                reportDataSource1.Value = payslipInfo;

                ReportDataSource reportDataSource2 = new ReportDataSource();
                reportDataSource2.Name = "ReportLayer";
                reportDataSource2.Value = reportLayer;

                localReport.SubreportProcessing += new SubreportProcessingEventHandler(PayslipDetailSubreportProcessingEventHandler);
                localReport.DataSources.Clear();
                localReport.DataSources.Add(reportDataSource1);
                localReport.DataSources.Add(reportDataSource2);
                localReport.Refresh();

                localReport.ReportPath = reportPath;

                var pdf = localReport.Render("PDF");

                reportFile.FileBytes = pdf;
                reportFile.Extension = "pdf";
                reportFile.Mimetype = "application/pdf";

                if (isPasswordProtected)
                {
                    var bytes = FileProtection.Protected(pdf, isPasswordProtected == true ? password : null);
                    reportFile.FileBytes = bytes;
                }
            }
            catch (Exception ex)
            {
            }
            return reportFile;
        }
        void PayslipDetailSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            var processId = Utility.TryParseLong(e.Parameters["ProcessId"].Values[0].ToString());
            var paymentId = Utility.TryParseLong(e.Parameters["PaymentId"].Values[0].ToString());
            var employeeId = Utility.TryParseLong(e.Parameters["EmployeeId"].Values[0].ToString());
            var payslipDetails = Task.Run(() => _supplementaryPaymentReportBusiness.PayslipDetail(processId, paymentId, employeeId, _user)).Result;
            e.DataSources.Add(new ReportDataSource("PayslipDetail", payslipDetails));
        }

        [HttpGet("Emailing")]
        public async Task<IActionResult> EmailingAsync([FromQuery] OnceOffPaymentEmailingDTO model)
        {
            var user = AppUser();
            _user = user;
            try
            {
                List<string> unsuccessfulEmailList = new List<string>();
                List<string> successfulEmailList = new List<string>();
                List<string> emailNotFoundList = new List<string>();
                List<string> fileNotFoundList = new List<string>();
                if (user.HasBoth)
                {
                    if (ModelState.IsValid)
                    {
                        string path = null;
                        var reportConfig = await _reportConfigBusiness.ReportConfigAsync("Supplementary Payslip", null, user.CompanyId, user.OrganizationId);
                        if (reportConfig == null)
                        {
                            path = $"{_webHostEnvironment.WebRootPath}\\Reports\\PWC\\Salary\\payroll_supplementary_payslip.rdlc";
                        }
                        else
                        {
                            path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                        }

                        var payslipInfo = await _supplementaryPaymentReportBusiness.PayslipInfo(new SupplementaryPaymentReport_Filter()
                        {
                            ProcessId = model.ProcessId,
                            PaymentId = model.PaymentId,
                            EmployeeId = model.EmployeeId,
                            PaymentMonth = model.PaymentMonth,
                            PaymentYear = model.PaymentYear
                        }, user);
                        var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, user.DivisionId);
                        var reportLayers = new List<ReportLayer>();
                        reportLayers.Add(reportLayer);

                        if (payslipInfo != null && payslipInfo.Rows.Count > 0)
                        {
                            
                            var emailSetting = await _loginManager.EmailSettings("Send");
                            for (int i = 0; i < payslipInfo.Rows.Count; i++)
                            {
                                List<byte[]> files = new List<byte[]>();
                                List<string> attachmentNames = new List<string>();
                                var employeeCode = payslipInfo.Rows[i]["EmployeeCode"].ToString();
                                var employeeName = payslipInfo.Rows[i]["EmployeeName"].ToString();
                                var email = payslipInfo.Rows[i]["OfficeEmail"];
                                var monthName = Utility.GetMonthName(Convert.ToInt16(payslipInfo.Rows[i]["PaymentMonth"]));
                                var allowanceName = payslipInfo.Rows[i]["AllowanceName"].ToString();
                                var year = Convert.ToInt16(payslipInfo.Rows[i]["PaymentYear"]);
                                payslipInfo.Rows[i]["InWord"] = NumberToWords.Input(Convert.ToDecimal(payslipInfo.Rows[i]["DisbursedAmount"]));
                                payslipInfo.Rows[i]["MonthName"] = monthName;
                                DataColumnCollection columns = payslipInfo.Columns;
                                if (reportLayer != null)
                                {
                                    payslipInfo.Rows[i]["CompanyName"] = reportLayer.CompanyName;

                                    if (!columns.Contains("BranchLogo"))
                                    {
                                        payslipInfo.Columns.Add("BranchLogo", typeof(byte[]));
                                    }
                                    if (!columns.Contains("ReportLogo"))
                                    {
                                        payslipInfo.Columns.Add("ReportLogo", typeof(byte[]));
                                    }

                                    payslipInfo.Rows[i]["ReportLogo"] = reportLayer.ReportLogo;
                                    payslipInfo.Rows[i]["BranchLogo"] = reportLayer.BranchLogo;

                                    payslipInfo.Rows[i]["Address"] = reportLayer.Address;
                                }
                                
                                if (columns.Contains("BranchId"))
                                {
                                    if (payslipInfo.Rows[i]["BranchId"] != null)
                                    {
                                        long branchId = Utility.TryParseLong(payslipInfo.Rows[i]["BranchId"].ToString());
                                        var branches = await _branchInfoBusiness.GetBranchsAsync(null, user);
                                        if (branches.Any() && branchId > 0)
                                        {
                                            var branch = branches.FirstOrDefault(item => item.BranchId == branchId);
                                            if (branch != null)
                                            {
                                                payslipInfo.Rows[i]["BranchName"] = branch.BranchName;
                                            }
                                        }
                                    }
                                }

                                // New DateTable
                                //var cols = payslipInfo.Columns;
                                //List<string> strings = new List<string>();
                                //foreach (DataColumn column in cols)
                                //{
                                //    strings.Add(column.ColumnName);
                                //}
                                var info = new[] { payslipInfo.Rows[i] }.CopyToDataTable();
                                //info.AddColumns(strings.ToArray(), 1);
                                //info.
                                //info.Rows[0]= payslipInfo.Rows[i];

                                var reportFile = await GetPaySlip(info, reportLayers.ToDataTable(), path, employeeCode, true, user, reportConfig);
                                files.Add(reportFile.FileBytes);
                                attachmentNames.Add(allowanceName + "Payslip " + monthName + "-" + year.ToString() + "." + model.FileFormat.ToLower());

                                EmailReceiverObject receiver = new EmailReceiverObject();
                                receiver.RecipientName = employeeName;
                                receiver.MailAddress = "yeasin@recombd.com";
                                receiver.Files = files;
                                receiver.AttachmentNames = attachmentNames;
                                if (receiver.Files.Count > 0)
                                {
                                    receiver.Subject = string.Format(@"{0} Payslip for {1}-{2}", allowanceName, monthName, year.ToString());
                                    emailSetting.DisplayName = receiver.Subject;
                                    emailSetting.EmailHtmlBody = EmailTemplate.OnceoffPaymentPayslip(receiver.RecipientName, monthName, year.ToString(),allowanceName);

                                    if (await EmailSender.SendMail(emailSetting, receiver) == true)
                                    {
                                        successfulEmailList.Add(employeeCode + " successfully sent");
                                    }
                                    else
                                    {
                                        unsuccessfulEmailList.Add(employeeCode + " unsuccessful not have any file");
                                    }
                                }
                                else
                                {
                                    fileNotFoundList.Add(employeeCode + " does not have any file");
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
                        return BadRequest("Invalid form submission");
                    }
                    return BadRequest("User Company information not found");
                }
                return BadRequest("Company information cannot found");
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SupplementaryPaymentReport", "EmailingAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
