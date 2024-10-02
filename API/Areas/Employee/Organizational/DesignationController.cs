
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
using BLL.Employee.Interface.Organizational;
using Shared.Employee.Filter.Organizational;

namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class DesignationController : ApiBaseController
    {
        private readonly IDesignationBusiness _designationBusiness;
        private readonly ISysLogger _sysLogger;
        public DesignationController(ISysLogger sysLogger, IDesignationBusiness designationBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _designationBusiness = designationBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetDesignations")]
        public async Task<IActionResult> GetDesignationsAsync([FromQuery] Designation_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _designationBusiness.GetDesignationsAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationController", "GetDesignationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
        
        [HttpGet, Route("GetDesignationById")]
        public async Task<IActionResult> GetDesignationById([FromQuery] Designation_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _designationBusiness.GetDesignationsAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationController", "GetDesignationById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveDesignation")]
        public async Task<IActionResult> SaveDesignationAsync(DesignationDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _designationBusiness.ValidateDesignationAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _designationBusiness.SaveDesignationAsync(model, user);
                        return Ok(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationController", "SaveDesignationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetDesignationDropdown")]
        public async Task<IActionResult> GetDesignationDropdownAsync()
        {
            var user = AppUser();
            try {
                if (user != null && user.HasBoth) {
                    var data_list = await _designationBusiness.GetDesignationDropdownAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationController", "GetGradesAsync", user);
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
                        if (model.Key == "Designation") {
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
                                var data = await _designationBusiness.GetDesignationItemsAsync(cellValues, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "DesignationController", "GetFileValuesAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
