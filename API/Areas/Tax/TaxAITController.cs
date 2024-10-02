using API.Base;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using Shared.Payroll.DTO.Tax;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.Filter.Tax;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using OfficeOpenXml;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxAITController : ApiBaseController
    {
        private readonly ITaxAITBusiness _taxAITBusiness;
        private readonly ISysLogger _sysLogger;
        public TaxAITController(ISysLogger sysLogger, ITaxAITBusiness taxAITBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _taxAITBusiness = taxAITBusiness;
        }

        [HttpGet, Route("GetEmployeeAITDocuments")]
        public async Task<IActionResult> GetEmployeeAITDocumentsAsync([FromQuery] TaxDocumentQuery query)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _taxAITBusiness.GetEmployeeAITDocumentsAsync(query, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITController", "SaveEmployeeTaxDocument", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveAIT")]
        public async Task<IActionResult> SaveAITAsync([FromForm] TaxDocumentSubmissionDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.CompanyId > 0 && user.OrganizationId > 0 && !Utility.IsNullEmptyOrWhiteSpace(user.OrgCode))
                {
                    var data = await _taxAITBusiness.SaveAITAsync(model, user);
                    if (data.Status)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return BadRequest(data);
                    }

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITController", "SaveEmployeeTaxDocument", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeAITDocumentById")]
        public async Task<IActionResult> GetEmployeeAITDocumentByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = (await _taxAITBusiness.GetEmployeeAITDocumentsAsync(new TaxDocumentQuery() { SubmissionId = id.ToString() }, user)).ListOfObject.FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITController", "GetEmployeeTaxDocumentById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeTaxDocumentFile")]
        public async Task<IActionResult> GetEmployeeTaxDocumentFileAsync(string path)
        {
            var user = AppUser();
            try
            {
                if (!Utility.IsNullEmptyOrWhiteSpace(path) && user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var format = Utility.GetFileExtension(path);
                    var fileName = Utility.GetFileName(path);
                    byte[] filebytes = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, path));
                    var mimeType = Utility.GetFileMimetype(format ?? "");
                    if (filebytes != null)
                    {
                        return File(filebytes, mimeType, fileName);
                    }
                    return File(filebytes, null, null);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITController", "GetEmployeeTaxDocumentFile", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UploadAIT")]
        public async Task<IActionResult> UploadAsync([FromForm] UploadTaxDocumentDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    if (model.File?.Length > 0)
                    {
                        var stream = model.File.OpenReadStream();
                        List<TaxDocumentSubmissionDTO> list = new List<TaxDocumentSubmissionDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            var rowCount = worksheet!.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {

                                var employeeCode = worksheet.Cells[row, 1].Value.ToString();
                                var amount = Utility.TryParseDecimal(worksheet.Cells[row, 2].Value?.ToString());

                                if (Utility.IsNullEmptyOrWhiteSpace(employeeCode) == false && amount > 0)
                                {
                                    TaxDocumentSubmissionDTO item = new TaxDocumentSubmissionDTO()
                                    {
                                        EmployeeCode = employeeCode,
                                        FiscalYearId = model.FiscalYearId,
                                        CertificateType = model.CertificateType,
                                        Amount = amount
                                    };
                                    list.Add(item);
                                }
                            }
                        }
                        if (list.Any())
                        {
                            var status = await _taxAITBusiness.UploadAsync(list, user);
                            if (status.Status)
                            {
                                return Ok(status);
                            }
                            else
                            {
                                return BadRequest(status);
                            }
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITController", "UploadAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("DeleteAIT")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0 && user.HasBoth)
                {
                    var data = await _taxAITBusiness.DeleteAsync(id, user);
                    if (data.Status)
                    {
                        return Ok(data);
                    }
                    else
                    {
                        return BadRequest(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITController", "DeleteAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
