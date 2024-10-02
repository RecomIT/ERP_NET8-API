using API.Base;
using AutoMapper;
using BLL.Administration.Interface;
using BLL.Base.Interface;
using BLL.Employee.Interface.Info;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;
using Shared.Control_Panel.Filter;
using Shared.Employee.Filter.Info;
using Shared.OtherModels.Report;
using Shared.OtherModels.User;
using Shared.Services;

namespace API.Areas.ControlPanel.Controllers
{
    [ApiController, Area("ControlPanel"), Route("api/[area]/[controller]"), Authorize]
    public class UserLogReportController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IReportBase _reportBase;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly IInfoBusiness _infoBusiness;
        private readonly IUserLogReportBusiness _userLogReportBusiness;
        private AppUser _user;
        private ReportFile _reportFile;
        public UserLogReportController(ISysLogger sysLogger, IWebHostEnvironment webHostEnvironment, IReportConfigBusiness reportConfigBusiness, IReportBase reportBase, IInfoBusiness infoBusiness, IUserLogReportBusiness userLogReportBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _reportConfigBusiness = reportConfigBusiness;
            _webHostEnvironment = webHostEnvironment;
            _userLogReportBusiness = userLogReportBusiness;
            _infoBusiness = infoBusiness;
            _reportBase = reportBase;
            _reportFile = new ReportFile();
        }

        [HttpGet("UserAccessReport")]
        public async Task<IActionResult> UserAccessReportAsync([FromQuery] UserLogReport_Filter filter)
        {
            var user = AppUser();
            try {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";

                var path = string.Empty;
                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("User Access Report", null, user.CompanyId, user.OrganizationId);
                if (user.ReportConfig != null) {
                    path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else {
                    path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Userlog\\user_access_log_report.rdlc";
                }

                var dataTable = await _userLogReportBusiness.GetUserAccessStatusInfoAsync(filter, user);
                if (dataTable != null && dataTable.Rows.Count > 0) {
                    LocalReport localReport = new LocalReport();
                    var reportLayers = new List<ReportLayer>();

                    var branchId = Utility.TryParseLong(filter.BranchId) > 0 ? Utility.TryParseLong(filter.BranchId) : user.BranchId;
                    var reportLayer = await _reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, branchId, user.DivisionId);
                    reportLayer.Username = user.Username;

                    var employeeInfo = await _infoBusiness.GetEmployeePersonalInfoByIdAsync(new EmployeePersonalInfoQuery() {
                        EmployeeId = user.EmployeeId.ToString()
                    }, user);

                    if (employeeInfo != null) {
                        reportLayer.EmployeeName = employeeInfo.EmployeeName;
                    }

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
                    reportDataSource1.Name = "ReportLayer";
                    reportDataSource1.Value = reportLayers;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "UserAccessStatusInfo";
                    reportDataSource2.Value = dataTable;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    return File(pdf, mimetype, "UserAccessReport." + extension);
                }
                else {
                    return NotFound("No data found to generate the report"); // Or return a custom message
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserAccessReportAsync", "UserAccessReportAsync", _user);
            }
            return BadRequest(ResponseMessage.ServerResponsedWithError);
        }

        [HttpGet("UserPrivilegeReport")]
        public async Task<IActionResult> UserPrivilegeReportAsync([FromQuery] UserLogReport_Filter filter)
        {
            var user = AppUser();
            try {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";

                var path = string.Empty;
                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("User Privilege Report", null, user.CompanyId, user.OrganizationId);
                if (user.ReportConfig != null) {
                    path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else {
                    path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Userlog\\user_privilege_report.rdlc";
                }

                var dataTable = await _userLogReportBusiness.GetUserPrivilegeInfoAsync(filter, user);
                if (dataTable != null && dataTable.Rows.Count > 0) {
                    LocalReport localReport = new LocalReport();
                    var reportLayers = new List<ReportLayer>();

                    var branchId = Utility.TryParseLong(filter.BranchId) > 0 ? Utility.TryParseLong(filter.BranchId) : user.BranchId;
                    var reportLayer = await _reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, branchId, user.DivisionId);

                    var employeeInfo = await _infoBusiness.GetEmployeePersonalInfoByIdAsync(new EmployeePersonalInfoQuery() {
                        EmployeeId = user.EmployeeId.ToString()
                    }, user);

                    if (employeeInfo != null) {
                        reportLayer.EmployeeName = employeeInfo.EmployeeName;
                    }

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
                    reportDataSource1.Name = "ReportLayer";
                    reportDataSource1.Value = reportLayers;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "UsePrivilegeReport";
                    reportDataSource2.Value = dataTable;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    return File(pdf, mimetype, "UsePrivilegeReport." + extension);
                }
                else {
                    return NotFound("No data found to generate the report"); // Or return a custom message
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserAccessReportAsync", "UserAccessReportAsync", _user);
            }
            return BadRequest(ResponseMessage.ServerResponsedWithError);
        }

        [HttpGet("UserRolePrivilegeReport")]
        public async Task<IActionResult> UserRolePrivilegeReportAsync([FromQuery] UserLogReport_Filter filter)
        {
            var user = AppUser();
            try {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";

                var path = string.Empty;
                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("User Role Privilege Report", null, user.CompanyId, user.OrganizationId);
                if (user.ReportConfig != null) {
                    path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else {
                    path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Userlog\\user_role_privilege_report.rdlc";
                }

                var dataTable = await _userLogReportBusiness.GetUserRolePrivilegeInfoAsync(filter, user);
                if (dataTable != null && dataTable.Rows.Count > 0) {
                    LocalReport localReport = new LocalReport();
                    var reportLayers = new List<ReportLayer>();

                    var branchId = Utility.TryParseLong(filter.BranchId) > 0 ? Utility.TryParseLong(filter.BranchId) : user.BranchId;
                    var reportLayer = await _reportBase.ReportLayerAsync(user.OrganizationId, user.CompanyId, branchId, user.DivisionId);
                    var employeeInfo = await _infoBusiness.GetEmployeePersonalInfoByIdAsync(new EmployeePersonalInfoQuery() {
                        EmployeeId = user.EmployeeId.ToString()
                    }, user);

                    if (employeeInfo != null) {
                        reportLayer.EmployeeName = employeeInfo.EmployeeName;
                    }

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
                    reportDataSource1.Name = "ReportLayer";
                    reportDataSource1.Value = reportLayers;

                    ReportDataSource reportDataSource2 = new ReportDataSource();
                    reportDataSource2.Name = "UsePrivilegeReport";
                    reportDataSource2.Value = dataTable;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.Refresh();

                    localReport.ReportPath = path;

                    var pdf = localReport.Render(renderFormat);

                    return File(pdf, mimetype, "UsePrivilegeReport." + extension);
                }
                else {
                    return NotFound("No data found to generate the report"); // Or return a custom message
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "UserAccessReportAsync", "UserAccessReportAsync", _user);
            }
            return BadRequest(ResponseMessage.ServerResponsedWithError);
        }
    }
}
