using System;
using System.Data;
using System.Linq;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using Microsoft.Reporting.NETCore;
using BLL.Salary.Salary.Interface;
using BLL.Administration.Interface;
using Shared.Payroll.Filter.Salary;
using Shared.Control_Panel.ViewModels;

namespace API.Areas.Salary
{
    internal class SalaryRelatedReportGenerator
    {
        private readonly ISysLogger _sysLogger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        private AppUser _user;
        private ReportFile _reportFile;
        public SalaryRelatedReportGenerator(IWebHostEnvironment webHostEnvironment, ISysLogger sysLogger, IReportConfigBusiness reportConfigBusiness, ISalaryReportBusiness salaryReportBusiness, IBranchInfoBusiness branchInfoBusiness)
        {
            _webHostEnvironment = webHostEnvironment;
            _sysLogger = sysLogger;
            _salaryReportBusiness = salaryReportBusiness;
            _reportConfigBusiness = reportConfigBusiness;
            _branchInfoBusiness = branchInfoBusiness;
            _reportFile = new ReportFile();
        }
        internal async Task<ReportFile> GetPaySlip(Payslip_Filter filter, string password, bool isPasswordProtected, AppUser user, ReportConfigViewModel reportConfig = null, string sp_Name = null)
        {
            _user = user;
            byte[] bytes = null;
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";

                var path = string.Empty;
                if (reportConfig == null)
                {
                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Payslip", null, user.CompanyId, user.OrganizationId);
                }
                else
                {
                    user.ReportConfig = reportConfig;
                }

                var branches = await _branchInfoBusiness.GetBranchsAsync(null, user);

                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Recom\\Salary\\payroll_payslip_extension_recom.rdlc";
                }

                LocalReport localReport = new LocalReport();

                var payslipInfo = await _salaryReportBusiness.PayslipReportInfoAsync(filter, user, sp_Name);

                if (payslipInfo.Rows.Count > 0)
                {
                    for (int i = 0; i < payslipInfo.Rows.Count; i++)
                    {
                        payslipInfo.Rows[i]["AmountInWord"] = NumberToWords.Input(Convert.ToDecimal(payslipInfo.Rows[i]["NetPay"]));
                        payslipInfo.Rows[i]["MonthName"] = Utility.GetMonthName(Convert.ToInt16(payslipInfo.Rows[i]["SalaryMonth"]));

                        DataColumnCollection columns = payslipInfo.Columns;

                        if (columns.Contains("BranchId"))
                        {
                            if (payslipInfo.Rows[i]["BranchId"] != null)
                            {
                                long branchId = Utility.TryParseLong(payslipInfo.Rows[i]["BranchId"].ToString());
                                if (branches.Any() && branchId > 0)
                                {
                                    var branch = branches.FirstOrDefault(item => item.BranchId == branchId);
                                    if (branch != null)
                                    {
                                        payslipInfo.Rows[i]["Location"] = branch.BranchName;
                                    }
                                }
                            }
                        }

                        if (payslipInfo.Columns.Contains("PFOpeningBalance"))
                        {
                            payslipInfo.Rows[i]["PFOpeningBalance"] = Convert.ToDecimal(payslipInfo.Rows[i]["PFOpeningBalance"]);
                        }
                    }
                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, user.DivisionId);

                    var companyPic = reportLayer.CompanyPic;
                    var reportLogo = reportLayer.ReportLogo;

                    reportLayer.CompanyPic = Utility.IsNullEmptyOrWhiteSpace(reportLayer.CompanyLogoPath) == false ?
                        Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = Utility.IsNullEmptyOrWhiteSpace(reportLayer.ReportLogoPath) == false ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : reportLayer.ReportLogo;
                    reportLayers.Add(reportLayer);

                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? companyPic : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? reportLogo : reportLayer.ReportLogo;

                    reportLayers.Add(reportLayer);

                    ReportDataSource reportDataSource1 = new ReportDataSource();
                    reportDataSource1.Name = "PayslipInfo";
                    reportDataSource1.Value = payslipInfo;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "ReportLayer";
                    reportDataSource2.Value = reportLayers;

                    localReport.SubreportProcessing +=
                    new SubreportProcessingEventHandler(PayslipSubreportProcessingEventHandler);

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    bytes = FileProtection.Protected(pdf, isPasswordProtected == true ? password : null);
                    _reportFile.FileBytes = bytes;
                    _reportFile.Mimetype = mimetype;
                    _reportFile.Extension = extension;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryRelatedReportGenerator", "GetPaySlip", user);
            }
            return _reportFile;
        }
        void PayslipSubreportProcessingEventHandler(object sender, SubreportProcessingEventArgs e)
        {
            var employeeId = e.Parameters["EmployeeId"].Values[0].ToString();
            var employeeCode = e.Parameters["EmployeeCode"].Values[0].ToString();
            var salaryMonth = e.Parameters["SalaryMonth"].Values[0].ToString();
            var salaryYear = e.Parameters["SalaryYear"].Values[0].ToString();
            var payslipDetails = Task.Run(() => _salaryReportBusiness.PayslipReportDetailAsync(new Payslip_Filter()
            {
                EmployeeId = employeeId,
                Month = salaryMonth,
                Year = salaryYear
            }, _user)).Result;

            e.DataSources.Add(new ReportDataSource("PayslipDetail", payslipDetails));
        }
        internal async Task<ReportFile> GetReconciliationRpt([FromQuery] Reconciliation_Filter filter, string password, bool isPasswordProtected, AppUser user)
        {
            _user = user;
            byte[] bytes = null;
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";

                var path = string.Empty;
                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Reconciliation", null, user.CompanyId, user.OrganizationId);

                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Payroll\\Salary\\Nagad\\reconciliation_nagad.rdlc";
                }

                LocalReport localReport = new LocalReport();

                var data = await _salaryReportBusiness.ReconciliationRptAsync(filter, user);
                if (data.Rows.Count > 0)
                {
                    //for (int i = 0; i < data.Rows.Count; i++) {
                    //data.Rows[i]["MonthNamePreview"] = Utility.GetPreviousMonthName(Convert.ToInt16(data.Rows[i]["SalaryMonth"]));
                    //data.Rows[i]["SalaryYearPreview"] = Utility.GetPreviousYear(Convert.ToInt16(data.Rows[i]["SalaryMonth"]), Convert.ToInt16(data.Rows[i]["SalaryYear"]));
                    //data.Rows[i]["MonthName"] = Utility.GetMonthName(Convert.ToInt16(data.Rows[i]["SalaryMonth"]));

                    //data.Rows[i]["MonthNamePreview"] = Utility.GetPreviousMonthName(Convert.ToInt16(filter.SalaryMonth));
                    //data.Rows[i]["SalaryYearPreview"] = Utility.GetPreviousYear(Convert.ToInt16(filter.SalaryMonth), Convert.ToInt16(filter.SalaryYear));
                    //data.Rows[i]["MonthName"] = Utility.GetMonthName(Convert.ToInt16(filter.SalaryYear));
                    //}

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, user.DivisionId);

                    var companyPic = reportLayer.CompanyPic;
                    var reportLogo = reportLayer.ReportLogo;

                    reportLayer.CompanyPic = Utility.IsNullEmptyOrWhiteSpace(reportLayer.CompanyLogoPath) == false ?
                        Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = Utility.IsNullEmptyOrWhiteSpace(reportLayer.ReportLogoPath) == false ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : reportLayer.ReportLogo;
                    reportLayers.Add(reportLayer);

                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? companyPic : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? reportLogo : reportLayer.ReportLogo;
                    reportLayers.Add(reportLayer);

                    ReportDataSource reportDataSource1 = new ReportDataSource();
                    reportDataSource1.Name = "SalaryReconciliation";
                    reportDataSource1.Value = data;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "ReportLayer";
                    reportDataSource2.Value = reportLayers;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    bytes = FileProtection.Protected(pdf, isPasswordProtected == true ? password : null);
                    _reportFile.FileBytes = bytes;
                    _reportFile.Mimetype = mimetype;
                    _reportFile.Extension = extension;
                }
            }
            catch (Exception ex)
            {
            }
            return _reportFile;
        }
        internal async Task<ReportFile> GetSalaryBreakdownRpt([FromQuery] SalaryBreakdown_Filter filter, string password, bool isPasswordProtected, AppUser user)
        {
            _user = user;
            byte[] bytes = null;
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";

                var path = string.Empty;
                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("ReconciliationBreakdown", null, user.CompanyId, user.OrganizationId);

                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Wounderman\\Salary\\payroll_breakdown.rdlc";
                }

                LocalReport localReport = new LocalReport();

                var data = await _salaryReportBusiness.SalaryBreakdownRptAsync(filter, user);
                //var dtls = await _salaryReportBusiness.SalaryBreakdownDtlsRptAsync(filter, user);

                if (data.Rows.Count > 0)
                {

                    var reportLayers = new List<ReportLayer>();
                    var branchId = Utility.TryParseLong(filter.BranchId);

                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, branchId > 0 ? branchId : user.BranchId, 0);

                    var companyPic = reportLayer.CompanyPic;
                    var reportLogo = reportLayer.ReportLogo;

                    reportLayer.CompanyPic = Utility.IsNullEmptyOrWhiteSpace(reportLayer.CompanyLogoPath) == false ?
                        Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = Utility.IsNullEmptyOrWhiteSpace(reportLayer.ReportLogoPath) == false ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : reportLayer.ReportLogo;
                    reportLayers.Add(reportLayer);

                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? companyPic : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? reportLogo : reportLayer.ReportLogo;
                    reportLayers.Add(reportLayer);

                    ReportDataSource reportDataSource1 = new ReportDataSource();
                    reportDataSource1.Name = "SalaryBreakdown";
                    reportDataSource1.Value = data;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "ReportLayer";
                    reportDataSource2.Value = reportLayers;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);

                    localReport.DataSources.Add(reportDataSource2);
                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    bytes = FileProtection.Protected(pdf, isPasswordProtected == true ? password : null);
                    _reportFile.FileBytes = bytes;
                    _reportFile.Mimetype = mimetype;
                    _reportFile.Extension = extension;
                }
            }
            catch (Exception ex)
            {
            }
            return _reportFile;
        }
        internal async Task<ReportFile> GetSalaryReconciliationRpt([FromQuery] Reconciliation_Filter filter, string password, bool isPasswordProtected, AppUser user)
        {
            _user = user;
            byte[] bytes = null;
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";

                var path = string.Empty;
                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("SalaryReconciliation", null, user.CompanyId, user.OrganizationId);

                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Wounderman\\Salary\\payroll_reconciliation.rdlc";
                }

                LocalReport localReport = new LocalReport();

                var data = await _salaryReportBusiness.SalaryReconciliationRptAsync(filter, user);

                if (data.Rows.Count > 0)
                {

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, user.DivisionId);

                    var companyPic = reportLayer.CompanyPic;
                    var reportLogo = reportLayer.ReportLogo;

                    reportLayer.CompanyPic = Utility.IsNullEmptyOrWhiteSpace(reportLayer.CompanyLogoPath) == false ?
                        Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = Utility.IsNullEmptyOrWhiteSpace(reportLayer.ReportLogoPath) == false ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : reportLayer.ReportLogo;
                    reportLayers.Add(reportLayer);

                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? companyPic : reportLayer.CompanyPic;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? reportLogo : reportLayer.ReportLogo;
                    reportLayers.Add(reportLayer);

                    ReportDataSource reportDataSource1 = new ReportDataSource();
                    reportDataSource1.Name = "SalaryReconciliation";
                    reportDataSource1.Value = data;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "ReportLayer";
                    reportDataSource2.Value = reportLayers;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    bytes = FileProtection.Protected(pdf, isPasswordProtected == true ? password : null);
                    _reportFile.FileBytes = bytes;
                    _reportFile.Mimetype = mimetype;
                    _reportFile.Extension = extension;
                }
            }
            catch (Exception ex)
            {
            }
            return _reportFile;
        }
    }
}
