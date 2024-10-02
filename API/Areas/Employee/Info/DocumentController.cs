using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.Response;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Info;
using DAL.Repository.Employee.Interface;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;

namespace API.Areas.Employee.Info
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class DocumentController : ApiBaseController
    {
        private readonly IDocumentBusiness _employeeDocumentBusiness;
        private readonly IDocumentRepository _documentRepository;
        private readonly ISysLogger _sysLogger;
        public DocumentController(IDocumentBusiness employeeDocumentBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase, IDocumentRepository documentRepository) : base(clientDatabase)
        {
            _employeeDocumentBusiness = employeeDocumentBusiness;
            _sysLogger = sysLogger;
            _documentRepository = documentRepository;
        }


        [HttpPost, Route("SaveEmployeeDocument")]
        public async Task<IActionResult> SaveEmployeeDocumentAsync([FromForm] EmployeeDocumentDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth && !Utility.IsNullEmptyOrWhiteSpace(user.OrgCode)) {
                    if (model.DocumentId == 0) {
                        var documentIsExist = await _documentRepository.GetDocumentByEmployeeIdAsync(model.EmployeeId, model.DocumentName, user);
                        if (documentIsExist != null && documentIsExist.DocumentId > 0) {
                            return Ok(new ExecutionStatus() {
                                Status = false,
                                Msg = "You are trying to upload duplicate file. " + model.DocumentName + " is already exist."
                            });
                        }
                    }
                    const long maxSize = 300 * 1024;
                    if (model.File == null || model.File.Length <= maxSize) {
                        var data = await _employeeDocumentBusiness.SaveEmployeeDocumentAsync(model, user);
                        return Ok(data);
                    }
                    else {
                        return Ok(new ExecutionStatus() {
                            Status = false,
                            Msg = "File size is greater than 300 KB"
                        });
                    }

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeDocumentController", "SaveEmployeeDocumentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeDocuments")]
        public async Task<IActionResult> GetEmployeeDocumentsAsync([FromQuery] EmployeeDocument_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.CompanyId > 0 && user.OrganizationId > 0) {
                    var data = await _employeeDocumentBusiness.GetEmployeeDocumentsAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeDocumentController", "GetEmployeeDocumentsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeDocumentById")]
        public async Task<IActionResult> GetEmployeeDocumentByIdAsync(long id, long employeeId)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = (await _employeeDocumentBusiness.GetEmployeeDocumentsAsync(new EmployeeDocument_Filter() { DocumentId = id.ToString(), EmployeeId = employeeId.ToString() }, user)).FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeDocumentController", "GetEmployeeDocumentById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeDocumentFile")]
        public async Task<IActionResult> GetEmployeeDocumentFileAsync(long id, long employeeId)
        {
            var user = AppUser();
            try {
                if (id > 0 && employeeId > 0 && user.HasBoth) {
                    var data = (await _employeeDocumentBusiness.GetEmployeeDocumentsAsync(new EmployeeDocument_Filter() { DocumentId = id.ToString(), EmployeeId = employeeId.ToString() }, user)).FirstOrDefault();
                    if (data != null) {
                        var filebytes = Utility.GetFileBytes(string.Format(@"{0}/{1}/{2}", Utility.PhysicalDriver, data.FilePath, data.FileName));
                        if (filebytes != null) {
                            return File(filebytes, Utility.GetFileMimetype(data.FileFormat ?? ""), data.ActualFileName);
                        }
                        return NotFound();
                    }
                    else {
                        return NotFound();
                    }
                }
                return NotFound();
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeDocumentController", "GetEmployeeDocumentFile", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
