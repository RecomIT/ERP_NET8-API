using API.Base;
using System.Data;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Shared.Leave.Report;
using AspNetCore.Reporting;
using Shared.OtherModels.User;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using BLL.Leave.Interface.Report;
using Shared.Leave.Filter.Report;
using DAL.DapperObject.Interface;
using Microsoft.Reporting.NETCore;
using BLL.Administration.Interface;
using BLL.Leave.Interface.LeaveSetting;
using Microsoft.AspNetCore.Authorization;


namespace API.Areas.Leave.Report
{
    [ApiController, Area("HRMS"), Route("api/[area]/Leave/[controller]"), Authorize]
    public class LeaveReportController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ILeaveReportBusiness _leaveReportBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly IReportBase _reportBusiness;
        private readonly ILeaveSettingBusiness _leaveSettingBusiness;
        private AppUser _user;

        public LeaveReportController(ISysLogger sysLogger,
            IWebHostEnvironment webHostEnvironment,
            ILeaveReportBusiness leaveReportBusiness,
            IReportConfigBusiness reportConfigBusiness,
            ILeaveSettingBusiness leaveSettingBusiness,
            IReportBase reportBaseBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _reportBusiness = reportBaseBusiness;
            _leaveReportBusiness = leaveReportBusiness;
            _webHostEnvironment = webHostEnvironment;
            _reportConfigBusiness = reportConfigBusiness;
            _leaveSettingBusiness = leaveSettingBusiness;
        }

        [HttpGet, Route("EmployeeWiseLeaveBalanceSummary")]
        public async Task<IActionResult> EmployeeWiseLeaveBalanceSummaryAsync([FromQuery] LeaveQuery_Filter filter)
        {
            var user = AppUser();
            try
            {

                if (user.HasBoth)
                {
                    string format = "xlsx";
                    string renderFormat = format.ToUpper();
                    string mimetype = Utility.GetFileMimetype(format);
                    string extension = format.ToLower();
                    var path = "";

                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Leave", "", user.CompanyId, user.OrganizationId);
                    if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }
                    else
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\hr\\Leave\\employee_wise_leave_balance_summary.rdlc";
                    }

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _reportBusiness.ReportLayerAsync(user.BranchId, user);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);

                    var datatable = await _leaveReportBusiness.EmployeeWiseLeaveBalanceSummaryAsync(filter, user);
                    if (datatable != null)
                    {
                        localReport.AddDataSource("ReportLayer", reportLayers);
                        localReport.AddDataSource("EmployeeLeaveBalance", datatable);

                        var result = localReport.Execute(RenderType.ExcelOpenXml, 1);
                        var renderBytes = result.MainStream;
                        return File(renderBytes, mimetype);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportController", "EmployeeWiseLeaveBalanceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("MonthlyLeaveReport")]
        public async Task<IActionResult> MonthlyLeaveReportAsync([FromQuery] LeaveQuery_Filter filter)
        {
            var user = AppUser();
            try
            {

                if (user.HasBoth)
                {

                    string format = "xlsx";
                    string renderFormat = format.ToUpper();
                    string mimetype = Utility.GetFileMimetype(format);
                    string extension = format.ToLower();
                    var path = "";

                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Leave", "", user.CompanyId, user.OrganizationId);
                    if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }
                    else
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\hr\\Leave\\monthly_leave_report.rdlc";
                    }

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _reportBusiness.ReportLayerAsync(user.BranchId, user);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);

                    var datatable = await _leaveReportBusiness.MonthlyLeaveReportAsync(filter, user);
                    if (datatable != null)
                    {
                        localReport.AddDataSource("ReportLayer", reportLayers);
                        localReport.AddDataSource("MonthlyLeaveRecords", datatable);

                        var result = localReport.Execute(RenderType.ExcelOpenXml, 1);
                        var renderBytes = result.MainStream;
                        return File(renderBytes, mimetype);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportController", "EmployeeWiseLeaveBalanceAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DateRangeWiseLeaveReport")]
        public async Task<IActionResult> DateRangeWiseLeaveReportAsync([FromQuery] LeaveQuery_Filter filter)
        {
            var user = AppUser();
            try
            {

                if (user.HasBoth)
                {

                    string format = "xlsx";
                    string renderFormat = format.ToUpper();
                    string mimetype = Utility.GetFileMimetype(format);
                    string extension = format.ToLower();
                    var path = "";

                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Leave", "", user.CompanyId, user.OrganizationId);
                    if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }
                    else
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\hr\\Leave\\date_wise_leave_report.rdlc";
                    }

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _reportBusiness.ReportLayerAsync(user.BranchId, user);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);

                    var datatable = await _leaveReportBusiness.DateRangeWiseLeaveReportAsync(filter, user);

                    if (datatable != null)
                    {
                        localReport.AddDataSource("ReportLayer", reportLayers);
                        localReport.AddDataSource("DateWiseLeaveStatus", datatable);

                        var result = localReport.Execute(RenderType.ExcelOpenXml, 1);
                        var renderBytes = result.MainStream;
                        return File(renderBytes, mimetype);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportController", "DateRangeWiseLeaveReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("IndividualYearlyStatus")]
        public async Task<IActionResult> IndividualYearlyStatusAsync([FromQuery] LeaveQuery_Filter filter)
        {
            var user = AppUser();
            try
            {

                if (user.HasBoth)
                {

                    string format = "xlsx";
                    string renderFormat = format.ToUpper();
                    string mimetype = Utility.GetFileMimetype(format);
                    string extension = format.ToLower();
                    var path = "";

                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Leave", "", user.CompanyId, user.OrganizationId);
                    if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }
                    else
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\hr\\Leave\\attandance_status_year.rdlc";
                    }

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _reportBusiness.ReportLayerAsync(user.BranchId, user);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);

                    var datatable = await _leaveReportBusiness.IndividualYearlyStatusAsync(filter, user);
                    if (datatable != null)
                    {
                        localReport.AddDataSource("ReportLayer", reportLayers);
                        localReport.AddDataSource("AttandanceStatusYear", datatable);

                        var result = localReport.Execute(RenderType.ExcelOpenXml, 1);
                        var renderBytes = result.MainStream;
                        return File(renderBytes, mimetype);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportController", "AttandanceStatusByYearAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("YearlyLeaveReport")]
        public async Task<IActionResult> YearlyLeaveReportAsync([FromQuery] LeaveQuery_Filter filter)
        {
            var user = AppUser();
            try
            {

                if (user.HasBoth)
                {
                    string format = "xlsx";
                    string renderFormat = format.ToUpper();
                    string mimetype = Utility.GetFileMimetype(format);
                    string extension = format.ToLower();
                    var path = "";

                    int year = Convert.ToInt32(filter.monthYear);
                    int month = Convert.ToInt32(filter.monthNo);

                    DateTime firstDateOfYear = new DateTime(year, 1, 1);
                    DateTime lastDateOfMonth = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                    string firstDateFormatted = firstDateOfYear.ToString("dd-MMM-yyyy");
                    string lastDateFormatted = lastDateOfMonth.ToString("dd-MMM-yyyy");

                    user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Leave", "", user.CompanyId, user.OrganizationId);
                    if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                    }
                    else
                    {
                        path = $"{_webHostEnvironment.WebRootPath}\\Reports\\hr\\Leave\\yearly_leave_report.rdlc";
                    }

                    var reportLayers = new List<ReportLayer>();
                    var reportLayer = await _reportBusiness.ReportLayerAsync(user.BranchId, user);
                    reportLayer.CompanyPic = reportLayer.CompanyPic == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.CompanyLogoPath) : null;
                    reportLayer.ReportLogo = reportLayer.ReportLogo == null ? Utility.GetFileBytes(Utility.PhysicalDriver + "/" + reportLayer.ReportLogoPath) : null;
                    reportLayers.Add(reportLayer);

                    AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);


                    var datatable = await _leaveReportBusiness.YearlyLeaveReportAsync(filter, user);

                    datatable.Columns.Add("FirstDateOfYear", typeof(string));
                    datatable.Columns.Add("LastDateOfMonth", typeof(string));

                    foreach (DataRow row in datatable.Rows)
                    {
                        row["FirstDateOfYear"] = firstDateFormatted;
                        row["LastDateOfMonth"] = lastDateFormatted;
                    }

                    if (datatable != null)
                    {
                        localReport.AddDataSource("ReportLayer", reportLayers);
                        localReport.AddDataSource("YearlyLeaveRecords", datatable);

                        var result = localReport.Execute(RenderType.ExcelOpenXml, 1);
                        var renderBytes = result.MainStream;
                        return File(renderBytes, mimetype);
                    }
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportController", "YearlyLeaveReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetLeaveYearDropdown")]
        public async Task<IActionResult> GetLeaveTypesDropdownAsync()
        {
            var user = AppUser();
            try
            { 
                if (user.HasBoth)
                {
                    var data = await _leaveReportBusiness.GetLeaveYearDropdownAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportController", "GetLeaveYearDropdown", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        #region Leave Card
        [HttpGet, Route("GetEmployeeLeaveCard")]
        public async Task<IActionResult> GetEmployeeLeaveCardAsync(long employeeId, int year=0)
        {
            var user = AppUser();
            _user = user;
            try
            {
                if (user.HasBoth && employeeId > 0)
                {
                    var leaveSetting = await _leaveSettingBusiness.GetLeavePeriodDateRange(null, year, user);
                    if (leaveSetting != null)
                    {
                        var employeeInformation = await _leaveReportBusiness.GetEmployeeInfoForLeaveCardAsync(new LeaveCardFilter()
                        {
                            EmployeeId = employeeId,
                            LeaveStartDate = leaveSetting.StartDate,
                            LeaveEndDate = leaveSetting.EndDate
                        }, user);

                        if (employeeInformation != null && employeeInformation.Any())
                        {
                            var reportLayer = await _reportBusiness.ReportLayerAsync(user.OrganizationId, user.CompanyId, 0, 0);
                            List<ReportLayer> reportLayers = new List<ReportLayer>();
                            reportLayers.Add(reportLayer);
                            string path = "";
                            user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Leave Card", "", user.CompanyId, user.OrganizationId);
                            if (user.ReportConfig != null && !Utility.IsNullEmptyOrWhiteSpace(user.ReportConfig.ReportPath))
                            {
                                path = $"{_webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";

                            }
                            else
                            {
                                path = $"{_webHostEnvironment.WebRootPath}\\Reports\\hr\\Leave\\mainreport_leave_card_information.rdlc";
                            }

                            Microsoft.Reporting.NETCore.LocalReport localReport = new Microsoft.Reporting.NETCore.LocalReport();
                            localReport.ReportPath = path;
                            ReportDataSource reportDataSource1 = new ReportDataSource();
                            reportDataSource1.Name = "EmployeeInformation";
                            reportDataSource1.Value = employeeInformation;

                            ReportDataSource reportDataSource2 = new ReportDataSource();
                            reportDataSource2.Name = "ReportLayer";
                            reportDataSource2.Value = reportLayers;

                            localReport.SubreportProcessing += new SubreportProcessingEventHandler(leaveBalanceSummarySubreport);
                            localReport.SubreportProcessing += new SubreportProcessingEventHandler(appliedLeaveInformationSubreport);
                            localReport.DataSources.Clear();
                            localReport.DataSources.Add(reportDataSource1);
                            localReport.DataSources.Add(reportDataSource2);
                            localReport.Refresh();

                            localReport.ReportPath = path;
                            var file = localReport.Render("PDF");
                            return File(file, Mimetype.PDF, "LeaveCard.pdf");
                        }
                        return BadRequest("Could not find employee information");
                    }
                    return BadRequest("Could not find leave period information");
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveReportController", "GetEmployeeLeaveCardAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        void leaveBalanceSummarySubreport(object sender, SubreportProcessingEventArgs e)
        {
            var employeeId = Utility.TryParseLong(e.Parameters["EmployeeId"].Values[0].ToString());
            var leaveStartDate = e.Parameters["LeaveStartDate"].Values[0].ToString();
            var leaveEndDate = e.Parameters["LeaveEndDate"].Values[0].ToString();
            var leaveBalanceSummary = Task.Run(() => _leaveReportBusiness.GetEmployeeLeaveBalanceSummaryForLeaveCardAsync(new LeaveCardFilter()
            {
                EmployeeId = employeeId,
                LeaveStartDate = Convert.ToDateTime(leaveStartDate),
                LeaveEndDate = Convert.ToDateTime(leaveEndDate)

            }, _user)).Result;

            e.DataSources.Add(new ReportDataSource("LeaveBalanceSummary", leaveBalanceSummary));
        }
        void appliedLeaveInformationSubreport(object sender, SubreportProcessingEventArgs e)
        {
            var employeeId = Utility.TryParseLong(e.Parameters["EmployeeId"].Values[0].ToString());
            var leaveStartDate = e.Parameters["LeaveStartDate"].Values[0].ToString();
            var leaveEndDate = e.Parameters["LeaveEndDate"].Values[0].ToString();
            var appliedLeaveInformation = Task.Run(() => _leaveReportBusiness.GetAppliedLeaveInformationForLeaveCardAsync(new LeaveCardFilter()
            {
                EmployeeId = employeeId,
                LeaveStartDate = Convert.ToDateTime(leaveStartDate),
                LeaveEndDate = Convert.ToDateTime(leaveEndDate)

            }, _user)).Result;

            e.DataSources.Add(new ReportDataSource("AppliedLeaveInformation", appliedLeaveInformation));
        } 
        #endregion

    }
}
