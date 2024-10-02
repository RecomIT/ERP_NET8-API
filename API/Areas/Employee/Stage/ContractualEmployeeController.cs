using API.Base;
using Shared.Models;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Employee.DTO.Stage;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Stage;
using Shared.Employee.Filter.Stage;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee_Module.Stage
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class ContractualEmployeeController : ApiBaseController
    {
        private readonly IContractualEmploymentBusiness _contractualEmploymentBusiness;
        private readonly ISysLogger _sysLogger;
        public ContractualEmployeeController(ISysLogger sysLogger, IContractualEmploymentBusiness contractualEmploymentBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _contractualEmploymentBusiness = contractualEmploymentBusiness;
        }

        [HttpPost,Route("SaveRenewContract")]
        public async Task<IActionResult> SaveRenewContractAysnc(ContractualEmploymentDTO model)
        {
            var user = AppUser();
            try {
                if(ModelState.IsValid && user.HasBoth) {
                    var status = await _contractualEmploymentBusiness.SaveRenewContractAysnc(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmployeeController", "SaveRenewContractAysnc", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetContractEmployees")]
        public async Task<IActionResult> GetContractEmployeesAsync([FromQuery]ContractualEmployment_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var response = await _contractualEmploymentBusiness.GetContractualEmploymentsInfoAsync(filter, user);
                    Response.AddPagination(response.Pageparam.PageNumber, response.Pageparam.PageSize, response.Pageparam.TotalRows, response.Pageparam.TotalPages);
                    return Ok(response.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmployeeController", "GetContractEmployees", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("DownloadUploadFormat")]
        public async Task<IActionResult> DownloadUploadFormatAsync()
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel\\Contractual_Employee_Uploader.xlsx");
            string contentType = "";
            if (System.IO.File.Exists(filepath)) {
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contentType, "contract-info-uploader.xlsx");
        }

        [HttpGet, Route("GetContractEmployeeById")]
        public async Task<IActionResult> GetContractEmployeeByIdAsync([FromQuery] ContractualEmployment_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var response = await _contractualEmploymentBusiness.GetContractualEmploymentsInfoAsync(filter, user);
                    var data = response.ListOfObject.FirstOrDefault();
                    if (data != null) {
                        return Ok(data);
                    }
                    return NoContent();
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmployeeController", "GetContractEmployees", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("UploadContractEmployee")]
        public async Task<IActionResult> UploadContractEmployeeAsync([FromForm] ExcelFileUploaderDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    if (model.File?.Length > 0) {
                        var stream = model.File.OpenReadStream();
                        List<ContractualEmploymentDTO> list = new List<ContractualEmploymentDTO>();
                        using (var package = new ExcelPackage(stream)) {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            
                            for (var row = 2; row <= rowCount; row++) {
                                ContractualEmploymentDTO employee = new ContractualEmploymentDTO();
                                employee.EmployeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                employee.EmployeeCode = employee.EmployeeCode != null ? employee.EmployeeCode.RemoveWhitespace() : employee.EmployeeCode;

                                if (!employee.EmployeeCode.IsNullEmptyOrWhiteSpace()) {
                                    var lastContractEndDate = worksheet.Cells[row, 2].Value?.ToString();

                                    if (lastContractEndDate.IsStringNumber() && !lastContractEndDate.IsNullEmptyOrWhiteSpace()) {
                                        employee.LastContractEndDate = DateTime.FromOADate(Convert.ToDouble(lastContractEndDate.RemoveWhitespace()));
                                    }
                                    else {
                                        employee.LastContractEndDate = lastContractEndDate.IsNullEmptyOrWhiteSpace() == false ? 
                                            Convert.ToDateTime(lastContractEndDate.RemoveWhitespace()) : null;
                                    }

                                    var contractStartDate = worksheet.Cells[row, 3].Value?.ToString();
                                    if (contractStartDate.IsStringNumber() && !contractStartDate.IsNullEmptyOrWhiteSpace()) {
                                        employee.ContractStartDate = DateTime.FromOADate(Convert.ToDouble(contractStartDate.RemoveWhitespace()));
                                    }
                                    else {
                                        employee.ContractStartDate = contractStartDate.IsNullEmptyOrWhiteSpace() == false ?
                                            Convert.ToDateTime(contractStartDate.RemoveWhitespace()) : null;
                                    }

                                    var contractEndDate = worksheet.Cells[row, 4].Value?.ToString();
                                    if (contractEndDate.IsStringNumber() && !contractEndDate.IsNullEmptyOrWhiteSpace()) {
                                        employee.ContractEndDate = DateTime.FromOADate(Convert.ToDouble(contractEndDate.RemoveWhitespace()));
                                    }
                                    else {
                                        employee.ContractEndDate = contractEndDate.IsNullEmptyOrWhiteSpace() == false ?
                                            Convert.ToDateTime(contractEndDate.RemoveWhitespace()) : null;
                                    }
                                }
                                list.Add(employee);
                            }
                            var data = await _contractualEmploymentBusiness.UploadEmployeeContractAsync(list,user);
                            return Ok(data);
                        }
                    }
                    return BadRequest("Empty file");
                }
                return Ok(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmployeeController", "UploadContractEmployeeAsync", user);
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            
        }

        [HttpPost, Route("SaveEmployeeContractApproval")]
        public async Task<IActionResult> SaveEmployeeContractApprovalAsync(ContractualEmploymentApprovalDTO model)
        {
            var user = AppUser();
            try {
                if(ModelState.IsValid && user.HasBoth) {
                    var status = await _contractualEmploymentBusiness.SaveEmployeeContractApprovalAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ContractualEmployeeController", "SaveEmployeeContractApproval", user);
                return BadRequest(ResponseMessage.InvalidParameters);
            }
        }
    }
}
