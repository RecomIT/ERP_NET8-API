using System.Web;
using API.Services;
using OfficeOpenXml;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using API.Base;
using BLL.Employee.Interface.Info;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using Shared.Employee.DTO.Info;
using Shared.OtherModels.Response;
using Shared.Helpers;
using Shared.OtherModels.DataService;

namespace API.Areas.Employee.Info
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class InfoController : ApiBaseController
    {
        private readonly IInfoBusiness _employeeInfoBusiness;
        private readonly ISysLogger _sysLogger;
        private ExcelGenerator _excelGenerator;
        public InfoController(ISysLogger sysLogger, IInfoBusiness employeeInfoBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _employeeInfoBusiness = employeeInfoBusiness;
            _sysLogger = sysLogger;
            _excelGenerator = new ExcelGenerator();
        }

        [HttpGet, Route("GetEmployeeServiceData")]
        public async Task<IActionResult> GetEmployeeServiceDataAsync([FromQuery] EmployeeService_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data_list = await _employeeInfoBusiness.GetEmployeeServiceDataAsync(filter, user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeeServiceData", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeDataAsync")]
        public async Task<IActionResult> GetEmployeeDataAsync([FromQuery] EmployeeService_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data_list = await _employeeInfoBusiness.GetEmployeeDataAsync(filter, user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeeDataAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeesForShiftAssign")]
        public async Task<IActionResult> GetEmployeesForShiftAssignAsync([FromQuery] EmployeeService_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data_list = await _employeeInfoBusiness.GetEmployeesForShiftAssignAsync(filter, user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeeServiceData", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeInfos")]
        public async Task<IActionResult> GetEmployeeInfosAsync([FromQuery] EmployeeQuery employeeQuery)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeInfoBusiness.GetEmployeesAsync(employeeQuery, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeeInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeeExtension")]
        public async Task<IActionResult> SaveEmployeeExtensionAsync(EmployeeInit employee)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var validator = await _employeeInfoBusiness.ValidateEmployeeAsync(employee, user);
                    if (validator != null && validator.Status == true) {
                        var data = await _employeeInfoBusiness.SaveEmployeeAsync(employee, user);
                        return Ok(data);
                    }
                    return Ok(validator);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "SaveEmployeeExtensionAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeOfficeInfoById")]
        public async Task<IActionResult> GetEmployeeOfficeInfoByIdAsync([FromQuery] EmployeeOfficeInfo_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeInfoBusiness.GetEmployeeOfficeInfoByIdAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeeOfficeInfoById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmploymentApproval")]
        public async Task<IActionResult> SaveEmploymentApprovalAsync(EmployeeApprovalDTO model, string remarks)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth && Utility.StatusChecking(model.StateStatus, new string[] { StateStatus.Approved, StateStatus.Recheck })) {
                    var data = await _employeeInfoBusiness.SaveEmploymentApprovalAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "SaveEmploymentApprovalAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveEmployeeProfessionalInfo")]
        public async Task<IActionResult> SaveEmployeeProfessionalInfoAsync(EmployeeOfficeInfo model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeInfoBusiness.SaveEmployeeProfessionalInfoAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "SaveEmployeeProfessionalInfo", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetEmployeePersonalInfoById")]
        public async Task<IActionResult> GetEmployeePersonalInfoByIdAsync([FromQuery] EmployeePersonalInfoQuery query)
        {
            var user = AppUser();
            try {
                if (query.EmployeeId != "0" && user.HasBoth) {
                    var data = await _employeeInfoBusiness.GetEmployeePersonalInfoByIdAsync(query, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeePersonalInfoById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveEmployeePersonalInfo")]
        public async Task<IActionResult> SaveEmployeePersonalInfoAsync(EmployeePersonalInfo model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeInfoBusiness.SaveEmployeePersonalInfoAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "SaveEmployeePersonalInfo", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetClientEmployees")]
        public async Task<IActionResult> GetClientEmployeesAsync(long clientCompany, long clientOrganization)
        {
            var user = AppUser();
            try {
                if (clientCompany > 0 && clientOrganization > 0) {
                    user.Database = GetDatabaseName(clientOrganization);
                    user.CompanyId = clientCompany;
                    user.OrganizationId = clientOrganization;
                    var data = await _employeeInfoBusiness.GetEmployeeServiceDataAsync(new EmployeeService_Filter(), user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetClientEmployees", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetClientEmployeeById")]
        public async Task<IActionResult> GetClientEmployeeByIdAsync(long employeeId, long clientCompany, long clientOrganization)
        {
            var user = AppUser();
            try {
                if (clientCompany > 0 && clientOrganization > 0) {
                    user.Database = GetDatabaseName(clientOrganization);
                    var data = (await _employeeInfoBusiness.GetClientEmployeeByIdAsync(employeeId, clientCompany, clientOrganization, user)).FirstOrDefault();
                    if (data != null) {
                        return Ok(data);
                    }
                    return NoContent();
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetClientEmployees", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeProfileInfo")]
        public async Task<IActionResult> GetEmployeeProfileInfoAsync(long id)
        {
            var user = AppUser();
            try {
                if (id > 0) {
                    var data = await _employeeInfoBusiness.GetEmployeeProfileInfoAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeeProfileInfo", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost("UploadProfileImage")]
        public async Task<IActionResult> UploadProfileImageAsync([FromForm] UploadProfileImage upload)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    const long maxSize = 200 * 1024;
                    if (upload.Image.Length <= maxSize) {
                        var data = await _employeeInfoBusiness.UploadProfileImageAsync(upload, user);
                        return Ok(data);
                    }
                    else {
                        return Ok(new ExecutionStatus() {
                            Status = false,
                            Msg = "File size is greater than 200 KB"
                        });
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "UploadProfileImage", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost("PostEmployeeInformationForReport")]
        public async Task<IActionResult> PostEmployeeInformationForReportAsync(EmployeeInfoReport_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    filter.BloodGroup = filter.BloodGroup != null && filter.BloodGroup != "" ? HttpUtility.UrlDecode(filter.BloodGroup) : filter.BloodGroup;
                    var data = await _employeeInfoBusiness.GetEmployeeInformationForReportAsync(filter, true, user);
                    if (data.Rows.Count > 0) {
                        var jsonObj = data.DataTableToJson();
                        return Ok(jsonObj);
                    }
                    return NotFound("No data found");
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeeInformationForReportAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost("PostDownloadEmployeeInformation")]
        public async Task<IActionResult> PostDownloadEmployeeInformationAsync(EmployeeInfoReport_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    filter.BloodGroup = filter.BloodGroup != null && filter.BloodGroup != "" ? HttpUtility.UrlDecode(filter.BloodGroup) : filter.BloodGroup;
                    var datatable = await _employeeInfoBusiness.GetEmployeeInformationForReportAsync(filter, true, user);
                    if (datatable.Rows.Count > 0) {
                        var fileBytes = _excelGenerator.GenerateExcel(datatable, "Employee Information");
                        string fileName = "Employee Information.xlsx";
                        string contentType = System.Net.Mime.MediaTypeNames.Application.Octet;
                        using (var package = new ExcelPackage(new MemoryStream(fileBytes))) {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            fileBytes = package.GetAsByteArray();
                        }
                        HttpContext.Response.Headers.Add("Content-Disposition", new[] { "attachment; filename=" + fileName });
                        HttpContext.Response.ContentType = contentType;
                        return File(fileBytes, contentType);
                    }
                    else {
                        return Ok("No data found");
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetEmployeeInformationForReportAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("GetFileValues")]
        public async Task<IActionResult> GetFileValuesAsync([FromForm] ExcelFileValue model)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    if (model.ExcelFile?.Length > 0) {
                        if (model.Key == "Employee") {
                            var stream = model.ExcelFile.OpenReadStream();
                            List<string> cellValues = new List<string>();
                            using (var package = new ExcelPackage(stream)) {
                                var worksheet = package.Workbook.Worksheets.First();
                                var rowCount = worksheet.Dimension.Rows;
                                for (var row = 2; row <= rowCount; row++) {
                                    var cellValue = worksheet.Cells[row, 1].Value?.ToString();
                                    if (!Utility.IsNullEmptyOrWhiteSpace(cellValue)) {
                                        cellValues.Add(cellValue);
                                    }
                                }
                            }
                            if (cellValues.Any()) {
                                var data = await _employeeInfoBusiness.GetEmployeeItemsAsync(cellValues, user);
                                if(data.Any()) {
                                    return Ok(data);
                                }
                                else {
                                    return NotFound("items not found");
                                }
                            }
                            else {
                                return NotFound("Excel data not found");
                            }
                        }
                        return BadRequest("Invalid Key");
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoController", "GetFileValuesAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet("IsOfficeEmailAvailable")]
        public async Task<IActionResult> IsOfficeEmailAvailableAsync(string email)
        {
            var user = AppUser();
            try {
                if(Utility.IsNullEmptyOrWhiteSpace(email) == false) {
                    var isOfficeEmailAvailable = await _employeeInfoBusiness.IsOfficeEmailAvailableAsync(0,email, user);
                    return Ok(isOfficeEmailAvailable);
                }
                return NoContent();
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoController", "IsOfficeEmailAvailableAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet("IsOfficeEmailInEditAvailable")]
        public async Task<IActionResult> IsOfficeEmailInEditAvailableAsync(long id,string email)
        {
            var user = AppUser();
            try {
                if (Utility.IsNullEmptyOrWhiteSpace(email) == false) {
                    var isOfficeEmailAvailable = await _employeeInfoBusiness.IsOfficeEmailAvailableAsync(id,email, user);
                    return Ok(isOfficeEmailAvailable);
                }
                return NoContent();
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoController", "IsOfficeEmailAvailableAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet("IsEmployeeCodeAvailable")]
        public async Task<IActionResult> IsEmployeeCodeAvailableAsync(string code)
            {
            var user = AppUser();
            try {
                if (Utility.IsNullEmptyOrWhiteSpace(code) == false) {
                    var isOfficeEmailAvailable = await _employeeInfoBusiness.IsEmployeeIdAvailableAsync(code, user);
                    return Ok(isOfficeEmailAvailable);
                }
                return NoContent();
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoController", "IsEmployeeCodeAvailableAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet("IsEmployeeCodeInEditAvailable")]
        public async Task<IActionResult> IsEmployeeCodeInEditAvailableAsync(long id, string code)
        {
            var user = AppUser();
            try {
                if (Utility.IsNullEmptyOrWhiteSpace(code) == false) {
                    var isOfficeEmailAvailable = await _employeeInfoBusiness.IsEmployeeIdInEditAvailableAsync(id, code, user);
                    return Ok(isOfficeEmailAvailable);
                }
                return NoContent();
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "InfoController", "IsEmployeeCodeAvailableAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
