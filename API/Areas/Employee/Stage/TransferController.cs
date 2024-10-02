using API.Base;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Employee.DTO.Stage;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Stage;
using Shared.Employee.Filter.Stage;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Stage
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class TransferController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEmployeeTransferBusiness _employeeTransferBusiness;
        public TransferController(IEmployeeTransferBusiness employeeTransferBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _employeeTransferBusiness = employeeTransferBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet,Route("GetEmployeeTransferProposals")]
        public async Task<IActionResult> GetEmployeeTransferProposalsAsync([FromQuery]EmployeeTransfer_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeTransferBusiness.GetEmployeeTransferProposalsAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "GetEmployeeTransferProposalsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeTransferProposalById")]
        public async Task<IActionResult> GetEmployeeTransferProposalsAsync(long transferProposalId)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeTransferBusiness.GetEmployeeTransferProposalsAsync(new EmployeeTransfer_Filter() {
                        TransferProposalId = transferProposalId.ToString()
                    }, user);
                    var item = data.ListOfObject.FirstOrDefault();
                    if (item != null) {
                        return Ok(item);
                    }
                    return NoContent();
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "GetEmployeeTransferProposalsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveTransferProposal")]
        public async Task<IActionResult> SaveTransferProposalAsync(EmployeeTransferProposalDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _employeeTransferBusiness.SaveTransferProposalAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConfirmationController", "SaveTransferProposalAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UploadTransferProposal")]
        public async Task<IActionResult> UploadTransferProposalAsync([FromForm] UploadTransferProposal uploadFile)
        {
            try {
                var appUser = AppUser();
                if (ModelState.IsValid) {
                    if (uploadFile.ExcelFile?.Length > 0) {
                        var stream = uploadFile.ExcelFile.OpenReadStream();
                        List<TransferProposalReadExcelDTO> readExcelDTOs = new List<TransferProposalReadExcelDTO>();
                        using (var package = new ExcelPackage(stream)) {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++) {
                                var code = worksheet.Cells[row, 1].Value?.ToString();
                                if (code != null) {
                                    TransferProposalReadExcelDTO dTO = new TransferProposalReadExcelDTO();
                                    var empCode = worksheet.Cells[row, 1].Value?.ToString();
                                    var proposalText = worksheet.Cells[row, 2].Value?.ToString();
                                    var effectiveDate = DateTime.FromOADate((Convert.ToDouble(worksheet.Cells[row, 3].Value))).Date;
                                    dTO.EmployeeCode = empCode;
                                    dTO.ProposalText = proposalText;
                                    dTO.EffectiveDate = effectiveDate;
                                    dTO.Head = uploadFile.Head;
                                    readExcelDTOs.Add(dTO);
                                }
                                else {
                                    continue;
                                }
                            }
                        }
                        var data = await _employeeTransferBusiness.UploadTransferProposalAsync(readExcelDTOs, appUser);
                        return Ok(data);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("DownloadTransferProposalExcelFile")]
        public async Task<IActionResult> DownloadTransferProposalExcelFile(string fileName)
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
    }
}
