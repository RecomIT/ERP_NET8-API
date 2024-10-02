using API.Base;
using System.Data;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using DAL.DapperObject.Interface;
using BLL.Administration.Interface;
using BLL.Employee.Interface.Report;
using Shared.Employee.Filter.Report;
using Shared.Employee.ViewModel.Report;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee_Module.Report
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class HRLetterController : ApiBaseController
    {
        private readonly IHRLetterBusiness hRLetterBusiness;
        private readonly ISysLogger syslogger;
        private readonly IReportBase reportBase;
        private readonly IReportConfigBusiness reportConfigBusiness;
        private readonly IWebHostEnvironment webHostEnvironment;
        public HRLetterController(
            IWebHostEnvironment _webHostEnvironment,
            IReportConfigBusiness _reportConfigBusiness,
            IReportBase _reportBase,
            IHRLetterBusiness _hRLetterBusiness,
            ISysLogger _logger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            hRLetterBusiness = _hRLetterBusiness;
            syslogger = _logger;
            reportBase = _reportBase;
            reportConfigBusiness = _reportConfigBusiness;
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet, Route("GetEmployeeInfo/{id}")]
        public async Task<IActionResult> GetEmployeeInfoAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0)
                {
                    var info = await hRLetterBusiness.GetEmployeeInfoAsync(id, user);
                    if (info != null)
                    {
                        return Ok(info);
                    }
                    else
                    {
                        return NotFound("Data not found");
                    }
                }
                return BadRequest("Employee Id is missing");
            }
            catch (Exception ex)
            {
                await syslogger.SaveHRMSException(ex, user.Database, "HRLetterController", "GetClearanceLetterAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadClearanceLetter")]
        public async Task<IActionResult> DownloadClearanceLetterAsync([FromQuery] ClearanceLetter_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (filter.Id > 0)
                {
                    var info = await hRLetterBusiness.GetEmployeeInfoAsync(filter.Id, user);
                    if (info != null)
                    {
                        info.TerminationDate = Utility.IsNullEmptyOrWhiteSpace(info.TerminationDate) == false ? Convert.ToDateTime(info.TerminationDate).ToString("dd MMM yyyy") : "";
                        var reportLayer = await reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, info.BranchId, 0);
                        reportLayer.IssueDate = Utility.IsNullEmptyOrWhiteSpace(filter.IssueDate) == false ? Convert.ToDateTime(filter.IssueDate).ToString("dd MMM yyyy") : "";
                        List<ReportLayer> reportLayers = new List<ReportLayer>();
                        reportLayers.Add(reportLayer);
                        string path = "";

                        user.ReportConfig = await reportConfigBusiness.ReportConfigAsync("Clearance Letter", "", user.CompanyId, user.OrganizationId);
                        if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                        {
                            path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                        }
                        else
                        {
                            path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\hr\\HR Latters\\ClearanceLetter.rdlc";
                        }


                        LocalReport localReport = new LocalReport(path);
                        List<HRLetterEmployeeInfoViewModel> list = new List<HRLetterEmployeeInfoViewModel>();
                        list.Add(info);
                        var datatable = list.ToDataTable();

                        if (datatable != null)
                        {
                            localReport.AddDataSource("ReportLayer", reportLayers);
                            localReport.AddDataSource("EmployeeInfo", datatable);

                            var result = localReport.Execute(RenderType.Pdf, 1);
                            var renderBytes = result.MainStream;
                            return File(renderBytes, Mimetype.PDF, "ClearanceLetter" + ".pdf");
                        }

                        return Ok(info);
                    }
                    else
                    {
                        return NotFound("Data not found");
                    }
                }
                return BadRequest("Employee Id is missing");
            }
            catch (Exception ex)
            {
                await syslogger.SaveHRMSException(ex, user.Database, "HRLetterController", "DownloadClearanceLetter", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadExperienceLetter")]
        public async Task<IActionResult> DownloadExperienceLetterAsync([FromQuery] ExperienceLetter_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (filter.Id > 0)
                {
                    var info = await hRLetterBusiness.GetEmployeeInfoAsync(filter.Id, user);
                    if (info != null)
                    {
                        info.TerminationDate = Utility.IsNullEmptyOrWhiteSpace(info.TerminationDate) == false ? Convert.ToDateTime(info.TerminationDate).ToString("dd MMM yyyy") : "";
                        var reportLayer = await reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, info.BranchId, 0);
                        reportLayer.IssueDate = Utility.IsNullEmptyOrWhiteSpace(filter.IssueDate) == false ? Convert.ToDateTime(filter.IssueDate).ToString("dd MMM yyyy") : "";
                        List<ReportLayer> reportLayers = new List<ReportLayer>();
                        reportLayers.Add(reportLayer);
                        string path = "";

                        user.ReportConfig = await reportConfigBusiness.ReportConfigAsync("Experience Letter", "", user.CompanyId, user.OrganizationId);
                        if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                        {
                            path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                        }
                        else
                        {
                            path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\Wounderman\\HR Latters\\ExperienceLetter.rdlc";
                        }

                        LocalReport localReport = new LocalReport(path);
                        List<HRLetterEmployeeInfoViewModel> list = new List<HRLetterEmployeeInfoViewModel>();
                        list.Add(info);
                        var datatable = list.ToDataTable();

                        if (datatable != null)
                        {
                            localReport.AddDataSource("ReportLayer", reportLayers);
                            localReport.AddDataSource("EmployeeInfo", datatable);

                            var result = localReport.Execute(RenderType.Pdf, 1);
                            var renderBytes = result.MainStream;
                            return File(renderBytes, Mimetype.PDF, "ExperienceLetter" + ".pdf");
                        }
                    }
                    else
                    {
                        return NotFound("Data not found");
                    }
                }
                return BadRequest("Employee Id is missing");
            }
            catch (Exception ex)
            {
                await syslogger.SaveHRMSException(ex, user.Database, "HRLetterController", "DownloadExperienceLetter", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadNOC")]
        public async Task<IActionResult> DownloadNOCAsync([FromQuery] NOC_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (filter.Id > 0)
                {
                    filter.Format = Utility.IsNullEmptyOrWhiteSpace(filter.Format) == true ? "pdf" : filter.Format;
                    var mimeType = Utility.GetFileMimetype(filter.Format);
                    var info = await hRLetterBusiness.GetEmployeeInfoAsync(filter.Id, user);
                    if (info != null)
                    {
                        info.TerminationDate = Utility.IsNullEmptyOrWhiteSpace(info.TerminationDate) == false ? Convert.ToDateTime(info.TerminationDate).ToString("dd MMM yyyy") : "";
                        var reportLayer = await reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, info.BranchId, 0);
                        reportLayer.IssueDate = Utility.IsNullEmptyOrWhiteSpace(filter.IssueDate) == false ? Convert.ToDateTime(filter.IssueDate).ToString("dd MMM yyyy") : "";
                        List<ReportLayer> reportLayers = new List<ReportLayer>();
                        reportLayers.Add(reportLayer);
                        string path = "";

                        user.ReportConfig = await reportConfigBusiness.ReportConfigAsync("NOC", "", user.CompanyId, user.OrganizationId);
                        if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                        {
                            path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                        }
                        else
                        {
                            path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\hr\\HR Latters\\NOCLetter.rdlc";
                        }

                        LocalReport localReport = new LocalReport(path);
                        List<HRLetterEmployeeInfoViewModel> list = new List<HRLetterEmployeeInfoViewModel>();
                        list.Add(info);
                        var datatable = list.ToDataTable();

                        if (datatable != null)
                        {
                            localReport.AddDataSource("ReportLayer", reportLayers);
                            localReport.AddDataSource("EmployeeInfo", datatable);


                            Response.Headers.Add("x-doc-type", mimeType);

                            if (filter.Format.ToLower() == "word" || filter.Format.ToLower() == "docx" || filter.Format.ToLower() == "doc")
                            {
                                var result = localReport.Execute(RenderType.WordOpenXml, 1);
                                var renderBytes = result.MainStream;
                                return File(renderBytes, mimeType, "NOC" + $".{filter.Format}");
                            }
                            if (filter.Format.ToLower() == "pdf")
                            {
                                var result = localReport.Execute(RenderType.Pdf, 1);
                                var renderBytes = result.MainStream;
                                return File(renderBytes, mimeType, "NOC" + $".{filter.Format}");
                            }
                        }
                    }
                    else
                    {
                        return NotFound("Data not found");
                    }
                }
                return BadRequest("Employee Id is missing");
            }
            catch (Exception ex)
            {
                await syslogger.SaveHRMSException(ex, user.Database, "HRLetterController", "DownloadNOC", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadSalaryCertificate")]
        public async Task<IActionResult> DownloadSalaryCertificateAsync([FromQuery] NOC_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (filter.Id > 0)
                {

                    var info = await hRLetterBusiness.GetEmployeeInfoAsync(filter.Id, user);
                    if (info != null)
                    {
                        info.TerminationDate = Utility.IsNullEmptyOrWhiteSpace(info.TerminationDate) == false ?
                            Convert.ToDateTime(info.TerminationDate).ToString("dd MMM yyyy") : "";
                        var reportLayer = await reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, info.BranchId, 0);
                        reportLayer.IssueDate = Utility.IsNullEmptyOrWhiteSpace(filter.IssueDate) == false ? Convert.ToDateTime(filter.IssueDate).ToString("dd MMM yyyy") : "";
                        List<ReportLayer> reportLayers = new List<ReportLayer>();
                        reportLayers.Add(reportLayer);
                        string path = "";

                        user.ReportConfig = await reportConfigBusiness.ReportConfigAsync("Salary Certificate", "", user.CompanyId, user.OrganizationId);
                        if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                        {
                            path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                        }
                        else
                        {
                            path = $"{this.webHostEnvironment.WebRootPath}\\Reports\\hr\\HR Latters\\SalaryCertificate.rdlc";
                        }

                        LocalReport localReport = new LocalReport(path);
                        // Salary Breakdown
                        var salaryBreakdown = await hRLetterBusiness.SalaryBreakdownAsync(filter.Id, user);
                        DataTable salaryAllowances = DataTableExtension.AddColumns(["HeadType", "Name", "Amount", "BaseAmount"]);
                        DataTable salaryDeductions = DataTableExtension.AddColumns(["HeadType", "Name", "Amount", "BaseAmount"]);
                        decimal grossPay = 0;
                        decimal totalDuduction = 0;
                        if (salaryBreakdown != null && salaryBreakdown.Rows.Count > 0)
                        {
                            int deductionIndex = 0;
                            for (int i = 0; i < salaryBreakdown.Rows.Count; i++)
                            {
                                if (salaryBreakdown.Rows[i]["HeadType"].ToString() == "Allowance")
                                {
                                    grossPay = grossPay + Utility.TryParseDecimal(salaryBreakdown.Rows[i]["BaseAmount"].ToString());
                                    salaryAllowances.Rows.Add(salaryAllowances.NewRow());

                                    salaryAllowances.Rows[i]["HeadType"] = salaryBreakdown.Rows[i]["HeadType"].ToString();
                                    salaryAllowances.Rows[i]["Name"] = salaryBreakdown.Rows[i]["Name"].ToString();
                                    salaryAllowances.Rows[i]["Amount"] = Utility.TryParseDecimal(salaryBreakdown.Rows[i]["Amount"].ToString()).ToString("C0").Replace("$", String.Empty);
                                    salaryAllowances.Rows[i]["BaseAmount"] = Utility.TryParseDecimal(salaryBreakdown.Rows[i]["BaseAmount"].ToString()).ToString("C0").Replace("$", String.Empty);
                                }
                                else if (salaryBreakdown.Rows[i]["HeadType"].ToString() == "Deduction")
                                {
                                    totalDuduction = totalDuduction + Utility.TryParseDecimal(salaryBreakdown.Rows[i]["Amount"].ToString());
                                    salaryDeductions.Rows.Add(salaryDeductions.NewRow());

                                    salaryDeductions.Rows[deductionIndex]["HeadType"] = salaryBreakdown.Rows[i]["HeadType"].ToString();
                                    salaryDeductions.Rows[deductionIndex]["Name"] = salaryBreakdown.Rows[i]["Name"].ToString();
                                    salaryDeductions.Rows[deductionIndex]["Amount"] = Utility.TryParseDecimal(salaryBreakdown.Rows[i]["Amount"].ToString()).ToString("C0").Replace("$", String.Empty);
                                    salaryDeductions.Rows[deductionIndex]["BaseAmount"] = Utility.TryParseDecimal(salaryBreakdown.Rows[i]["BaseAmount"].ToString()).ToString("C0").Replace("$", String.Empty);
                                    deductionIndex++;
                                }
                            }
                            if (grossPay > 0)
                            {
                                info.GrossPay = grossPay;
                                info.NetPay = Math.Round((grossPay - totalDuduction), MidpointRounding.AwayFromZero);
                                info.InWord = NumberToWords.Input(info.NetPay);
                            }
                        }


                        List<HRLetterEmployeeInfoViewModel> list = new List<HRLetterEmployeeInfoViewModel>();
                        list.Add(info);
                        var datatable = list.ToDataTable();

                        if (datatable != null)
                        {
                            localReport.AddDataSource("ReportLayer", reportLayers);
                            localReport.AddDataSource("EmployeeInfo", datatable);
                            localReport.AddDataSource("Allowance", salaryAllowances);
                            localReport.AddDataSource("Deduction", salaryDeductions);

                            var result = localReport.Execute(RenderType.Pdf, 1);
                            var renderBytes = result.MainStream;
                            return File(renderBytes, Mimetype.PDF, "SalaryCertificate" + ".=df");
                        }
                    }
                    else
                    {
                        return NotFound("Data not found");
                    }
                }
                return BadRequest("Employee Id is missing");
            }
            catch (Exception ex)
            {
                await syslogger.SaveHRMSException(ex, user.Database, "HRLetterController", "DownloadNOC", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
