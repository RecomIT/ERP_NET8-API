using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Organizational;
using Microsoft.AspNetCore.Authorization;
using Shared.Employee.Filter.Organizational;
using BLL.Employee.Interface.Organizational;

namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class DepartmentController : ApiBaseController
    {
        private readonly IDepartmentBusiness _departmentBusiness;
        private readonly ISysLogger _sysLogger;
        public DepartmentController(ISysLogger sysLogger, IDepartmentBusiness departmentBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _departmentBusiness = departmentBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetDepartments")]
        public async Task<IActionResult> GetDepartmentsAsync([FromQuery] Department_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _departmentBusiness.GetDepartmentsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "GetDepartmentsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDepartmentById")]
        public async Task<IActionResult> GetDepartmentById([FromQuery] Department_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _departmentBusiness.GetDepartmentsAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "GetDepartmentById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveDepartment")]
        public async Task<IActionResult> SaveDepartmentAsync(DepartmentDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _departmentBusiness.ValidateDepartmentAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _departmentBusiness.SaveDepartmentAsync(model, user);
                        return Ok(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "SaveDepartmentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDepartmentDropdown")]
        public async Task<IActionResult> GetDepartmentDropdownAsync()
        {
            var user = AppUser();
            try {
                if (user != null && user.HasBoth) {
                    var data_list = await _departmentBusiness.GetDepartmentDropdownAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "GetDepartmentDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("GetFileValues")]
        public async Task<IActionResult> GetFileValuesAsync([FromForm] ExcelFileValue model)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    if (model.ExcelFile?.Length > 0) {
                        if (model.Key == "Department") {
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
                                var data = await _departmentBusiness.GetDepartmentItemsAsync(cellValues, user);
                                if (data.Any()) {
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepartmentController", "GetFileValuesAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
