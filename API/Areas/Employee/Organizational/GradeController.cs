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
    public class GradeController : ApiBaseController
    {
        private readonly IGradeBusiness _gradeBusiness;
        private readonly ISysLogger _sysLogger;
        public GradeController(ISysLogger sysLogger, IGradeBusiness gradeBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _gradeBusiness = gradeBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetGrades")]
        public async Task<IActionResult> GetGradesAsync([FromQuery] Grade_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _gradeBusiness.GetGradesAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeController", "GetGradesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetGradeById")]
        public async Task<IActionResult> GetGradeById([FromQuery] Grade_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _gradeBusiness.GetGradesAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeController", "GetGradeById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveGrade")]
        public async Task<IActionResult> SaveGradeAsync(GradeDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _gradeBusiness.ValidateGradeAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _gradeBusiness.SaveGradeAsync(model, user);
                        return Ok(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeController", "SaveGradeAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetGradeDropdown")]
        public async Task<IActionResult> GetGradeDropdownAsync()
        {
            var user = AppUser();
            try {
                if (user != null && user.HasBoth) {
                    var data_list = await _gradeBusiness.GetGradeDropdownAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeController", "GetGradeDropdown", user);
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
                        if (model.Key == "Grade") {
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
                                var data = await _gradeBusiness.GetGradeItemsAsync(cellValues, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "GradeController", "GetFileValuesAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
