using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Shared.Services;
using System;
using BLL.Base.Interface;
using System.Linq;
using API.Services;
using Microsoft.Reporting.NETCore;
using System.Collections.Generic;
using Shared.OtherModels.Report;
using Microsoft.AspNetCore.Hosting;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Report;
using API.Base;
using Shared.Attendance.Filter.Report;

namespace API.Areas.Attendance.Report
{
    [ApiController, Area("HRMS"), Route("api/[area]/Attendance/[controller]"), Authorize]
    public class ReportController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IAttendanceReportBusiness _attendanceReportBusiness;
        private readonly IReportBase _reportBase;
        public ReportController(IReportBase reportBase, IWebHostEnvironment webHostEnvironment, IAttendanceReportBusiness attendanceReportBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _attendanceReportBusiness = attendanceReportBusiness;
            _sysLogger = sysLogger;
            _webHostEnvironment = webHostEnvironment;
            _reportBase = reportBase;
        }

        [HttpGet, Route("EmployeeAttendanceReport")]
        public async Task<IActionResult> EmployeeAttendanceReportAsync([FromQuery] EmployeeAttendanceReport_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {

                    var data = await _attendanceReportBusiness.EmployeeAttendanceReportAsync(filter, user);
                    var itemCount = data.DailyAttendances.Count().ToString();

                    if (data.DailyAttendances.Count() > 0)
                    {
                        string renderFormat = "PDF";
                        string mimetype = FileProcessor.GetFileMimetype(renderFormat.ToLower());
                        string extension = renderFormat.ToLower();
                        var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\hr\\hr_attendance.rdlc";

                        LocalReport localReport = new LocalReport();

                        var reportLayers = new List<ReportLayer>();
                        var reportLayer = await _reportBase.ReportLayerAsync(0, user);
                        reportLayers.Add(reportLayer);

                        ReportDataSource reportDataSource1 = new ReportDataSource();
                        reportDataSource1.Name = "ReportLayer";
                        reportDataSource1.Value = reportLayers;

                        ReportDataSource reportDataSource2 = new ReportDataSource();
                        reportDataSource2.Name = "DailyAttendance";
                        reportDataSource2.Value = data.DailyAttendances;

                        ReportDataSource reportDataSource3 = new ReportDataSource();
                        reportDataSource3.Name = "AttendanceSummery";
                        reportDataSource3.Value = data.AttendanceSummeries;

                        ReportDataSource reportDataSource4 = new ReportDataSource();
                        reportDataSource4.Name = "AttendanceWorkShift";
                        reportDataSource4.Value = data.AttendanceWorkShifts;

                        localReport.DataSources.Clear();
                        localReport.DataSources.Add(reportDataSource1);
                        localReport.DataSources.Add(reportDataSource2);
                        localReport.DataSources.Add(reportDataSource3);
                        localReport.DataSources.Add(reportDataSource4);
                        localReport.Refresh();

                        localReport.ReportPath = path;
                        var file = localReport.Render(renderFormat);
                        return File(file, mimetype, "Attendance." + extension);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "EmployeeAttendanceReportAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DailyAttendanceReport")]
        public async Task<IActionResult> DailyAttendanceReportAsync([FromQuery] DailyAttendanceReport_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _attendanceReportBusiness.DailyAttendancesReportAsync(filter, user);
                    if (data.Count() > 0)
                    {
                        string renderFormat = "PDF";
                        string mimetype = "application/pdf";
                        string extension = renderFormat.ToLower();
                        var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\hr\\hr_employee_attendance.rdlc";

                        LocalReport localReport = new LocalReport();

                        var reportLayers = new List<ReportLayer>();
                        var reportLayer = await _reportBase.ReportLayerAsync(0, user);

                        if (reportLayer != null && !Utility.IsNullEmptyOrWhiteSpace(reportLayer.ReportLogoPath))
                        {
                            reportLayer.ReportLogo = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, reportLayer.ReportLogoPath));
                        }
                        if (reportLayer != null && !Utility.IsNullEmptyOrWhiteSpace(reportLayer.CompanyLogoPath))
                        {
                            reportLayer.CompanyPic = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, reportLayer.CompanyLogoPath));
                        }

                        reportLayers.Add(reportLayer);

                        ReportDataSource reportDataSource1 = new ReportDataSource();
                        reportDataSource1.Name = "ReportLayer";
                        reportDataSource1.Value = reportLayers;

                        ReportDataSource reportDataSource2 = new ReportDataSource();
                        reportDataSource2.Name = "DailyAttendance";
                        reportDataSource2.Value = data;

                        localReport.DataSources.Clear();
                        localReport.DataSources.Add(reportDataSource1);
                        localReport.DataSources.Add(reportDataSource2);
                        localReport.Refresh();

                        localReport.ReportPath = path;
                        var file = localReport.Render(renderFormat);
                        return File(file, mimetype, "Employee_Attendance." + extension);
                    }
                    return NoContent();
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ReportController", "DailyAttendanceReport", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
