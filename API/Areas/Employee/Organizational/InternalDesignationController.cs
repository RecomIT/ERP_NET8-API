using API.Base;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational.InternalDesignation;
using Shared.Employee.ViewModel.Organizational.InternalDesignation;

namespace API.Areas.Employee.Organizational
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class InternalDesignationController : ApiBaseController
    {
        private readonly IInternalDesignationBusiness _designationBusiness;
        private readonly ISysLogger _sysLogger;
        public InternalDesignationController(ISysLogger sysLogger, IInternalDesignationBusiness designationBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _designationBusiness = designationBusiness;
        }

        [HttpGet("GetInternalDesignations")]
        public async Task<IActionResult> GetInternalDesignationsAsync([FromQuery] InternalDesignation_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var list = await _designationBusiness.GetInternalDesignationsAsync(filter, user);
                    Response.AddPagination(list.Pageparam.PageNumber, list.Pageparam.PageSize, list.Pageparam.TotalRows, list.Pageparam.TotalPages);
                    return Ok(list.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HRController", "GetInternalDesignationsAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost("SaveInternalDesignation")]
        public async Task<IActionResult> SaveInternalDesignationAsync(InternalDesignationDTO designationDTO)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var dbResponse = await _designationBusiness.SaveInternalDesignationAsync(designationDTO, user);
                    return Ok(dbResponse);

                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "HRController", "SaveInternalDesignationAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetInternalDesignationById")]
        public async Task<IActionResult> GetInternalDesignationByIdAync(long internalDesignationId)
        {
            var appUser = AppUser();
            try {
                var data = await _designationBusiness.GetInternalDesignationByIdAync(internalDesignationId, appUser);
                return Ok(data);
            }
            catch (Exception ex) {
                return Ok("Serve responed with error");
            }
        }

        [HttpGet, Route("DownloadInternalDesignationExcelFile")]
        public async Task<IActionResult> DownloadInternalDesignationExcelFileAsync(string fileName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel", fileName);
            var provider = new FileExtensionContentTypeProvider();
            string contenttype = "";
            if (System.IO.File.Exists(filepath)) {
                contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, fileName);
        }

        [HttpPost, Route("UploadInternalDesignationExcel")]
        public async Task<IActionResult> UploadInternalDesignationExcelAsync([FromForm] UploadInternalDesignationViewModel uploadedFile)
        {
            try {
                var appUser = AppUser();

                if (ModelState.IsValid) {
                    if (uploadedFile.ExcelFile?.Length > 0) {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<InternalDesignationDTO> internalDesignationDTOs = new List<InternalDesignationDTO>();

                        using (var package = new ExcelPackage(stream)) {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++) {
                                var internalDesig = worksheet.Cells[row, 2].Value.ToString();
                                if (internalDesig != null) {
                                    InternalDesignationDTO internalDesignationDTO = new InternalDesignationDTO();
                                    var internalDesignationName = worksheet.Cells[row, 2].Value?.ToString();
                                    var remarks = worksheet.Cells[row, 3].Value?.ToString() ?? "";

                                    internalDesignationDTO.InternalDesignationName = internalDesignationName;
                                    internalDesignationDTO.Remarks = remarks;
                                    internalDesignationDTOs.Add(internalDesignationDTO);
                                }
                                else {
                                    continue;
                                }

                            }

                        }
                        var data = await _designationBusiness.UploadInternalDesignationExcelAsync(internalDesignationDTOs, appUser);
                        return Ok(data);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
