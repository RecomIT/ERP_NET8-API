using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Report;
using Microsoft.Reporting.NETCore;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Info;
using BLL.Administration.Interface;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Info;
using Shared.Employee.DTO.Info;


namespace API.Areas.Employee.Info
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class EmployeeLoggerController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IReportBase _reportBase;
        private readonly IEmployeeLoggerBusiness _employeeLoggerBusiness;
        private readonly IReportConfigBusiness _reportConfigBusiness;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IInfoBusiness _infoBusiness;
        public EmployeeLoggerController(
            ISysLogger sysLogger,
            IReportBase reportBase,
            IReportConfigBusiness reportConfigBusiness,
            IEmployeeLoggerBusiness employeeLoggerBusiness,
            IWebHostEnvironment webHostEnvironment,
             IInfoBusiness infoBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _reportBase = reportBase;
            _employeeLoggerBusiness = employeeLoggerBusiness;
            _reportConfigBusiness = reportConfigBusiness;
            _infoBusiness = infoBusiness;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetEmployeeLogReport")]
        public async Task<IActionResult> GetEmployeeLogReportAsync([FromQuery] EmployeeLogReport_Filter filter)
        {
            var user = AppUser();
            try {
                string renderFormat = "PDF";
                string mimetype = "application/pdf";
                string extension = "pdf";

                var path = string.Empty;
                user.ReportConfig = await _reportConfigBusiness.ReportConfigAsync("Employee Log History", null, user.CompanyId, user.OrganizationId);
                if (user.ReportConfig != null) {
                    path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\{user.ReportConfig.ReportPath}";
                }
                else {
                    path = $"{this._webHostEnvironment.WebRootPath}\\Reports\\Userlog\\employee_info_log_report.rdlc";
                }

                var list = await _employeeLoggerBusiness.GetEmployeeLogReportAsync(filter, user);
                DataTable dataTable = new DataTable();
                if (list.Any()) {
                    dataTable = list.ToDataTable();
                }
                else {
                    list = new List<EmployeeLoggerDTO> ();
                    list.Add(new EmployeeLoggerDTO());
                    dataTable = list.ToDataTable();
                }
                

                var log_user_info = await _employeeLoggerBusiness.GetUserLogInfoAysnc(filter.UserId, user);
                if(log_user_info != null && log_user_info.Rows.Count > 0) {
                    var dateRange = filter.DateRange.Split("~");
                    var startDate = dateRange[0];
                    var endDate = dateRange[1];
                    dataTable.Rows[0]["Username"] = log_user_info.Rows[0]["Username"];
                    dataTable.Rows[0]["DateRange"] = Convert.ToDateTime(startDate).ToString("dd MMM yyyy") + " - " + Convert.ToDateTime(endDate).ToString("dd MMM yyyy");

                    log_user_info.Rows[0]["DateRange"] = Convert.ToDateTime(startDate).ToString("dd MMM yyyy") + " - " + Convert.ToDateTime(endDate).ToString("dd MMM yyyy");
                }

                if (dataTable != null) {
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
                    reportDataSource2.Name = "EmployeeInfoLogger";
                    reportDataSource2.Value = dataTable;

                    ReportDataSource reportDataSource3 = new ReportDataSource();
                    reportDataSource3.Name = "LogUserInfo";
                    reportDataSource3.Value = log_user_info;

                    localReport.DataSources.Clear();
                    localReport.DataSources.Add(reportDataSource1);
                    localReport.DataSources.Add(reportDataSource2);
                    localReport.DataSources.Add(reportDataSource3);
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

            }
            return Ok("It is Bad request");
        }
    }
}
