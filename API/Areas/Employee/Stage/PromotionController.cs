using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Stage;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Stage;
using Shared.Employee.DTO.Stage;

namespace API.Areas.Employee.Stage
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class PromotionController : ApiBaseController
    {
        private readonly IEmployeePromotionBusiness _employeePromotionBusiness;
        private readonly ISysLogger _sysLogger;
        public PromotionController(ISysLogger sysLogger, IEmployeePromotionBusiness employeePromotionBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger; 
            _employeePromotionBusiness = employeePromotionBusiness;
        }

        [HttpGet, Route("GetEmployeePromotionProposals")]
        public async Task<IActionResult> GetEmployeePromotionProposalsAsync([FromQuery] EmployeePromotion_Filter query)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeePromotionBusiness.GetEmployeePromotionProposalsAsync(query, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PromotionController", "GetEmployeePromotionProposalsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SavePromotionProposal")]
        public async Task<IActionResult> SavePromotionProposalAsync(EmployeePromotionProposalDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.CompanyId > 0 && user.OrganizationId > 0) {
                    var hasPendingItem = await _employeePromotionBusiness.SingleEmployeePendingProposalAsync(model.PromotionProposalId,model.EmployeeId,user);
                    if(hasPendingItem != null && hasPendingItem.PromotionProposalId > 0) {
                        return BadRequest("Employee already has a pending proposal");
                    }
                    else {
                        var data = await _employeePromotionBusiness.SavePromotionProposalAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PromotionController", "SavePromotionProposal", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeePromotionProposalById")]
        public async Task<IActionResult> GetEmployeePromotionProposalByIdAsync(long id)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && id > 0) {
                    var data = (await _employeePromotionBusiness.GetEmployeePromotionProposalsAsync(new EmployeePromotion_Filter() { PromotionProposalId = id }, user)).ListOfObject.FirstOrDefault();
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PromotionController", "GetEmployeePromotionProposalById", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost,Route("DeleteEmployeePendingProposal")]
        public async Task<IActionResult> DeleteEmployeePendingProposalAsync(PromotionProposalCancellationDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var status = await _employeePromotionBusiness.DeleteEmployeePendingProposalAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PromotionController", "DeleteEmployeePendingProposal", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        /// <summary>
        /// Added by Monzur 11-Sep-2023
        /// </summary>
        /// <param name="uploadFile"></param>
        /// <returns></returns>

        [HttpPost, Route("UploadPromotionProposal")]
        public async Task<IActionResult> UploadPromotionProposalAsync([FromForm] UploadEmployeePromotionProposal uploadFile)
        {
            try {
                var appUser = AppUser();

                if (ModelState.IsValid) {
                    if (uploadFile.ExcelFile?.Length > 0) {
                        var stream = uploadFile.ExcelFile.OpenReadStream();
                        List<PromotionProposalReadExcelDTO> readExcelDTOs = new List<PromotionProposalReadExcelDTO>();
                        using (var package = new ExcelPackage(stream)) {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++) {
                                var code = worksheet.Cells[row, 1].Value?.ToString();
                                if (code != null) {
                                    PromotionProposalReadExcelDTO dTO = new PromotionProposalReadExcelDTO();
                                    var empCode = worksheet.Cells[row, 1].Value?.ToString();
                                    var gradeName = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                                    var proposalText = worksheet.Cells[row, 3].Value?.ToString();
                                    var effectiveDate = DateTime.FromOADate((Convert.ToDouble(worksheet.Cells[row, 4].Value))).Date;
                                    dTO.EmployeeCode = empCode;
                                    dTO.GradeName = gradeName;
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
                        var data = await _employeePromotionBusiness.UploadPromotionProposalAsync(readExcelDTOs, appUser);
                        return Ok(data);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("DownloadPromotionProposalExcelFile")]
        public async Task<IActionResult> DownloadPromotionProposalExcelFile(string fileName)
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

        [HttpPost, Route("ApprovalProposal")]
        public async Task<IActionResult> ApprovalProposalAsync(ApprovalProposalDTO model)
        {
            var user = AppUser();
            try {
                if(model.Id > 0 && model.EmployeeId > 0) {
                    var status = await _employeePromotionBusiness.ApprovalProposalAsync(model.Id, model.EmployeeId, user);
                    if (status.Status) {
                        return Ok(status);
                    }
                    else {
                        return BadRequest(status);
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
