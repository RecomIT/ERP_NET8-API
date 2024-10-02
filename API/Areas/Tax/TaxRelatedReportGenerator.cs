using BLL.Administration.Interface;
using BLL.Base.Implementation;
using BLL.Base.Interface;
using BLL.Employee.Interface.Info;
using BLL.Salary.Salary.Interface;
using BLL.Tax.Interface;
using Microsoft.Reporting.NETCore;
using Shared.Control_Panel.ViewModels;
using Shared.Helpers;
using Shared.OtherModels.Report;
using Shared.OtherModels.User;
using Shared.Payroll.Report;
using Shared.Services;
using System.Data;


namespace API.Areas.Tax
{
    internal class TaxRelatedReportGenerator
    {
        private ReportFile _reportFile;
        private readonly ISysLogger _sysLogger;
        private readonly IReportBase _reportBase;
        private readonly IInfoBusiness _infoBusiness;
        private readonly ITaxReportBusiness _taxReportBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly ISalaryReportBusiness _salaryReportBusiness;
        public TaxRelatedReportGenerator(
            ISysLogger sysLogger,
            IReportBase reportBase,
            IInfoBusiness infoBusiness,
            ITaxReportBusiness taxReportBusiness,
            IWebHostEnvironment webHostEnvironment,
            IReportConfigBusiness reportConfigBusiness,
            ISalaryReportBusiness salaryReportBusiness)
        {
            _webHostEnvironment = webHostEnvironment;
            _sysLogger = sysLogger;
            _reportConfigBusiness = reportConfigBusiness;
            _salaryReportBusiness = salaryReportBusiness;
            _taxReportBusiness = taxReportBusiness;
            _reportBase = reportBase;
            _infoBusiness = infoBusiness;
        }
        internal async Task<ReportFile> TaxCardReportAsync(long employeeId, long taxProcessId, long fiscalYearId, short month, short year, bool? isDisbursed, string format, string password, bool isPasswordProtected, AppUser user, ReportConfigViewModel reportConfig = null, string spName = null)
        {
            _reportFile = new ReportFile();
            byte[] bytes = null;
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";
                //payroll_tax-card_FY_23-24 payroll_tax-card
                var path = "";

                // Report Config
                var employeeInfo = await _infoBusiness.GetEmployeeInformationById(employeeId, user);

                if (reportConfig == null)
                {
                    var fiscalYearRange = ReportingHelper.FiscalYearRange(month, year);
                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("TaxCard", fiscalYearRange, user.CompanyId, user.OrganizationId);
                }
                else
                {
                    user.ReportConfig = reportConfig;
                }

                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    var terminationMonth = employeeInfo?.TerminationDate.HasValue != null && employeeInfo.TerminationStatus == StateStatus.Approved ? employeeInfo.TerminationDate.Value.Month : 0;

                    var report = ReportingHelper.FindTaxReportPath(user.ReportConfig.ReportPath, month, user, (short)terminationMonth);
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{report}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\payroll\\payroll_tax-card_FY_23-24.rdlc";
                }

                LocalReport localReport = new LocalReport();

                var taxCard = new TaxCardMaster();
                taxCard = await _taxReportBusiness.TaxCardAsync(employeeId, 0, 0, month, year, isDisbursed, user);

                DataTable taxChallan = new DataTable();
                if (taxCard.TaxCardInfo.Any())
                {
                    taxChallan = await _taxReportBusiness.TaxChallanAsync(employeeId, taxCard.TaxCardInfo.FirstOrDefault().FiscalYearId, user);
                    var reportLayers = new List<ReportLayer>();
                    //var reportLayer = await _salaryReportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, user.BranchId, 0);
                    var reportLayer = await _reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, (
                        taxCard.TaxCardInfo.FirstOrDefault().BranchId > 0 ? taxCard.TaxCardInfo.FirstOrDefault().BranchId ?? 0 : user.BranchId), 0);
                    //reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : reportLayer.ReportLogo;
                    reportLayers.Add(reportLayer);

                    ReportDataSource reportDataSource1 = new ReportDataSource();
                    reportDataSource1.Name = "ReportLayer";
                    reportDataSource1.Value = reportLayers;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "EmployeeTaxProcess";
                    reportDataSource2.Value = taxCard.TaxCardInfo;

                    ReportDataSource reportDataSource3 = new ReportDataSource();
                    reportDataSource3.Name = "EmployeeTaxProcessDetail";
                    reportDataSource3.Value = taxCard.TaxCardDetails;

                    ReportDataSource reportDataSource4 = new ReportDataSource();
                    reportDataSource4.Name = "EmployeeTaxProcessSlab";
                    reportDataSource4.Value = taxCard.TaxCardSlabs;

                    ReportDataSource reportDataSource5 = new ReportDataSource();
                    reportDataSource5.Name = "EmployeeTaxChallan";
                    reportDataSource5.Value = taxChallan;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.DataSources.Add(reportDataSource3);
                    localReport.DataSources.Add(reportDataSource4);
                    localReport.DataSources.Add(reportDataSource5);
                    localReport.Refresh();

                    localReport.ReportPath = path;
                    var pdf = localReport.Render(renderFormat);

                    _reportFile.FileBytes = pdf;
                    _reportFile.Extension = extension;
                    _reportFile.Mimetype = mimetype;

                    if (isPasswordProtected)
                    {
                        bytes = FileProtection.Protected(pdf, isPasswordProtected == true ? password : null);
                        _reportFile.FileBytes = bytes;
                        _reportFile.Mimetype = mimetype;
                        _reportFile.Extension = extension;
                    }
                }
                else
                {
                    return _reportFile;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return _reportFile;
        }
        internal async Task<ReportFile> FinalTaxCardReportAsync(long employeeId, long fiscalYearId, short year, string password, bool isPasswordProtected, AppUser user, ReportConfigViewModel reportConfig = null, string spName = null)
        {
            _reportFile = new ReportFile();
            byte[] bytes = null;
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";
                var path = "";

                // Report Config
                var employeeInfo = await _infoBusiness.GetEmployeeInformationById(employeeId, user);
                //var fiscalYear = await 

                if (reportConfig == null)
                {
                    var fiscalYearRange = ReportingHelper.FiscalYearRange(6, year);
                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("TaxCard", fiscalYearRange, user.CompanyId, user.OrganizationId);
                }
                else
                {
                    user.ReportConfig = reportConfig;
                }

                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    var terminationMonth = employeeInfo?.TerminationDate.HasValue != null && employeeInfo.TerminationStatus == StateStatus.Approved ? employeeInfo.TerminationDate.Value.Month : 0;

                    var report = ReportingHelper.FindTaxReportPath(user.ReportConfig.ReportPath, 6, user, (short)terminationMonth);
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{report}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\payroll\\payroll_tax-card_FY_23-24.rdlc";
                }

                LocalReport localReport = new LocalReport();

                
                var taxCardInfo = await _taxReportBusiness.FinalTaxCardInfoAsync(employeeId, fiscalYearId, user);
                var taxCardDetail = await _taxReportBusiness.FinalTaxCardDetailAsync(employeeId, fiscalYearId, user);
                var taxCardSlab = await _taxReportBusiness.FinalTaxCardSlabAsync(employeeId, fiscalYearId, user);

                DataTable taxChallan = new DataTable();
                if (taxCardInfo.Rows.Count > 0)
                {
                    taxCardInfo.Columns.Add("TotalTaxDeductedInWord");
                    if (taxCardInfo.Columns.Contains("InWord"))
                    {
                        decimal netTaxPayable = Utility.TryParseDecimal(taxCardInfo.Rows[0]["YearlyTax"].ToString());
                        taxCardInfo.Rows[0]["InWord"] = NumberToWords.Input(netTaxPayable);   
                    }


                    decimal totalTaxDeducted = Utility.TryParseDecimal(taxCardInfo.Rows[0]["PaidTotalTax"].ToString()) + Utility.TryParseDecimal(taxCardInfo.Rows[0]["ActualTaxDeductionAmount"].ToString());

                    taxCardInfo.Rows[0]["TotalTaxDeductedInWord"]= NumberToWords.Input(totalTaxDeducted);

                    taxChallan = await _taxReportBusiness.TaxChallanAsync(employeeId, fiscalYearId, user);
                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, employeeInfo != null? employeeInfo.BranchId??0 : 0, 0);
                    reportLayers.Add(reportLayer);

                    ReportDataSource reportDataSource1 = new ReportDataSource();
                    reportDataSource1.Name = "ReportLayer";
                    reportDataSource1.Value = reportLayers;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "EmployeeTaxProcess";
                    reportDataSource2.Value = taxCardInfo;

                    ReportDataSource reportDataSource3 = new ReportDataSource();
                    reportDataSource3.Name = "EmployeeTaxProcessDetail";
                    reportDataSource3.Value = taxCardDetail;

                    ReportDataSource reportDataSource4 = new ReportDataSource();
                    reportDataSource4.Name = "EmployeeTaxProcessSlab";
                    reportDataSource4.Value = taxCardSlab;

                    ReportDataSource reportDataSource5 = new ReportDataSource();
                    reportDataSource5.Name = "EmployeeTaxChallan";
                    reportDataSource5.Value = taxChallan;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.DataSources.Add(reportDataSource3);
                    localReport.DataSources.Add(reportDataSource4);
                    localReport.DataSources.Add(reportDataSource5);
                    localReport.Refresh();

                    localReport.ReportPath = path;
                    var pdf = localReport.Render(renderFormat);

                    _reportFile.FileBytes = pdf;
                    _reportFile.Extension = extension;
                    _reportFile.Mimetype = mimetype;

                    if (isPasswordProtected)
                    {
                        bytes = FileProtection.Protected(pdf, isPasswordProtected == true ? password : null);
                        _reportFile.FileBytes = bytes;
                        _reportFile.Mimetype = mimetype;
                        _reportFile.Extension = extension;
                    }
                }
                else
                {
                    return _reportFile;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return _reportFile;
        }
        internal async Task<ReportFile> SupplementaryTaxCardReportAsync(long employeeId, long fiscalYearId, long paymenAmountId, short year, string password, bool isPasswordProtected, AppUser user, ReportConfigViewModel reportConfig = null, string spName = null)
        {
            _reportFile = new ReportFile();
            byte[] bytes = null;
            try
            {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";
                var path = "";

                // Report Config
                var employeeInfo = await _infoBusiness.GetEmployeeInformationById(employeeId, user);

                if (reportConfig == null)
                {
                    var fiscalYearRange = ReportingHelper.FiscalYearRange(6, year);
                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("TaxCard", fiscalYearRange, user.CompanyId, user.OrganizationId);
                }
                else
                {
                    user.ReportConfig = reportConfig;
                }

                if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                {
                    var report = ReportingHelper.FindTaxReportPath(user.ReportConfig.ReportPath, 1, user, 0);
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{report}";
                }
                else
                {
                    path = $"{_webHostEnvironment.WebRootPath}\\Reports\\payroll\\payroll_tax-card_FY_23-24.rdlc";
                }

                LocalReport localReport = new LocalReport();

                var taxCardInfo = await _taxReportBusiness.SupplementaryTaxCardInfoAsync(employeeId, fiscalYearId,paymenAmountId, user);
                var taxCardDetail = await _taxReportBusiness.SupplementaryTaxCardDetailAsync(employeeId, fiscalYearId, paymenAmountId, user);
                var taxCardSlab = await _taxReportBusiness.SupplementaryTaxCardSlabAsync(employeeId, fiscalYearId, paymenAmountId, user);

                DataTable taxChallan = new DataTable();
                if (taxCardInfo.Rows.Count > 0)
                {
                    if (taxCardInfo.Columns.Contains("InWord"))
                    {
                        decimal netTaxPayable = Utility.TryParseDecimal(taxCardInfo.Rows[0]["YearlyTax"].ToString());
                        taxCardInfo.Rows[0]["InWord"] = NumberToWords.Input(netTaxPayable);
                    }
                    taxChallan = await _taxReportBusiness.TaxChallanAsync(employeeId, fiscalYearId, user);
                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, employeeInfo != null ? employeeInfo.BranchId ?? 0 : 0, 0);
                    reportLayers.Add(reportLayer);

                    ReportDataSource reportDataSource1 = new ReportDataSource();
                    reportDataSource1.Name = "ReportLayer";
                    reportDataSource1.Value = reportLayers;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "EmployeeTaxProcess";
                    reportDataSource2.Value = taxCardInfo;

                    ReportDataSource reportDataSource3 = new ReportDataSource();
                    reportDataSource3.Name = "EmployeeTaxProcessDetail";
                    reportDataSource3.Value = taxCardDetail;

                    ReportDataSource reportDataSource4 = new ReportDataSource();
                    reportDataSource4.Name = "EmployeeTaxProcessSlab";
                    reportDataSource4.Value = taxCardSlab;

                    ReportDataSource reportDataSource5 = new ReportDataSource();
                    reportDataSource5.Name = "EmployeeTaxChallan";
                    reportDataSource5.Value = taxChallan;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.DataSources.Add(reportDataSource3);
                    localReport.DataSources.Add(reportDataSource4);
                    localReport.DataSources.Add(reportDataSource5);
                    localReport.Refresh();

                    localReport.ReportPath = path;
                    var pdf = localReport.Render(renderFormat);

                    _reportFile.FileBytes = pdf;
                    _reportFile.Extension = extension;
                    _reportFile.Mimetype = mimetype;

                    if (isPasswordProtected)
                    {
                        bytes = FileProtection.Protected(pdf, isPasswordProtected == true ? password : null);
                        _reportFile.FileBytes = bytes;
                        _reportFile.Mimetype = mimetype;
                        _reportFile.Extension = extension;
                    }
                }
                else
                {
                    return _reportFile;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return _reportFile;
        }
        internal async Task<byte[]> GetTaxCertificateFY2223(DataTable emploeeInfo, DataTable challanInfo, List<ReportLayer> reportLayers, AppUser user)
        {
            byte[] bytes = null;
            try
            {
                var employeeCode = emploeeInfo.Rows[0]["EmployeeCode"].ToString();
                var taxDetails = Task.Run(() => _taxReportBusiness.TaxDetailFY22_23(employeeCode, user)).Result;

                string renderFormat = "PDF";
                var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\payroll\\payroll_tax-card_Half_PWC_FY_22-23.rdlc";

                LocalReport localReport = new LocalReport();

                ReportDataSource reportDataSource1 = new ReportDataSource();
                reportDataSource1.Name = "ReportLayer";
                reportDataSource1.Value = reportLayers;

                ReportDataSource reportDataSource2 = new ReportDataSource();
                reportDataSource2.Name = "EmployeeTaxProcess";
                reportDataSource2.Value = emploeeInfo;

                ReportDataSource reportDataSource3 = new ReportDataSource();
                reportDataSource3.Name = "EmployeeTaxProcessDetail";
                reportDataSource3.Value = taxDetails;

                ReportDataSource reportDataSource4 = new ReportDataSource();
                reportDataSource4.Name = "EmployeeTaxChallan";
                reportDataSource4.Value = challanInfo;

                localReport.DataSources.Clear();
                localReport.DataSources.Add(reportDataSource1);
                localReport.DataSources.Add(reportDataSource2);
                localReport.DataSources.Add(reportDataSource3);
                localReport.DataSources.Add(reportDataSource4);
                localReport.Refresh();

                localReport.ReportPath = path;
                var pdf = localReport.Render(renderFormat);


                bytes = FileProtection.Protected(pdf, employeeCode);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRelatedReportGenerator", "GetTaxCertificateFY2223", user);
            }
            return bytes;
        }
    }
}
