using API.Base;
using API.Services;
using BLL.Base.Interface;
using BLL.Employee.Interface.Account;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using Shared.Employee.DTO.Account;
using Shared.Employee.Filter.Account;
using Shared.Employee.ViewModel.Account;
using Shared.Helpers;
using Shared.Services;

namespace API.Areas.Employee.Account
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class EmployeeAccountInfoController : ApiBaseController
    {
        private readonly IAccountInfoBusiness _accountInfoBusiness;
        private readonly ISysLogger _sysLogger;
        public EmployeeAccountInfoController(IAccountInfoBusiness accountInfoBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _accountInfoBusiness = accountInfoBusiness;
            _sysLogger = sysLogger;
        }

        [HttpPost, Route("SaveEmployeeAccountInfo")]
        public async Task<IActionResult> SaveEmployeeAccountInfoAsync(EmployeeAccountInfoDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _accountInfoBusiness.EmployeeAccountInfoValidatorAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var status = await _accountInfoBusiness.SaveEmployeeAccountInfoAsync(model, user);
                        return Ok(status);
                    }

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeAccountInfoController", "SaveEmployeeAccountInfoAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeAccountInfos")]
        public async Task<IActionResult> GetEmployeeAccountInfosAsync([FromQuery] EmployeeAccount_Filter filter)
        {
            var user = AppUser();
            try {
                var data = await _accountInfoBusiness.GetEmployeeAccountInfosAsync(filter, user);
                Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                return Ok(data.ListOfObject);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeAccountInfoController", "GetEmployeeAccountInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeAccountInfoById")]
        public async Task<IActionResult> GetEmployeeAccountInfoByIdAsync([FromQuery] EmployeeAccount_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _accountInfoBusiness.GetEmployeeAccountInfosAsync(filter, user);
                    return Ok(data.ListOfObject.FirstOrDefault());
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeAccountInfoController", "GetEmployeeAccountInfosAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeeAccountStatus")]
        public async Task<IActionResult> SaveEmployeeAccountStatusAsync(AccountInfoStatusDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _accountInfoBusiness.SaveApprovalOfEmployeeAccountAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeAccountInfoController", "SaveEmployeeAccountStatusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }


        //Added by Monzur 29-Nov-2023
        //Account Info Upload
        [HttpGet, Route("DownloadAccountInfoExcelFile")]
        public async Task<IActionResult> DownloadAccountInfoExcelFileAsync(string fileName)
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


        [HttpPost, Route("UploadAccountInfoExcel")]
        public async Task<IActionResult> UploadAccountInfoExcelAsync([FromForm] UploadAccountInfoViewModel uploadedFile)
        {
            try {
                var appUser = AppUser();

                if (ModelState.IsValid) {
                    if (uploadedFile.ExcelFile?.Length > 0) {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<EmployeeAccountInfoViewModel> models = new List<EmployeeAccountInfoViewModel>();

                        using (var package = new ExcelPackage(stream)) {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++) {
                                var empCode = worksheet.Cells[row, 1].Value.ToString();
                                if (empCode != null) {
                                    EmployeeAccountInfoViewModel model = new EmployeeAccountInfoViewModel();
                                    var employeeCode = worksheet.Cells[row, 1].Value?.ToString();
                                    var paymentMode = worksheet.Cells[row, 2].Value?.ToString();
                                    var bankName = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                                    var bankBrName = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                                    var accNo = worksheet.Cells[row, 5].Value?.ToString() ?? "";
                                    var agentName = worksheet.Cells[row, 6].Value?.ToString() ?? "";
                                    var activateReason = worksheet.Cells[row, 7].Value?.ToString() ?? "";
                                    var remarks = worksheet.Cells[row, 8].Value?.ToString() ?? "";

                                    model.EmployeeCode = employeeCode;
                                    model.PaymentMode = paymentMode;
                                    model.BankName = bankName;
                                    model.BankBranchName = bankBrName;
                                    model.AccountNo = accNo;
                                    model.AgentName = agentName;
                                    model.ActivationReason = activateReason;
                                    model.Remarks = remarks;
                                    models.Add(model);
                                }
                                else {
                                    continue;
                                }

                            }

                        }
                        var data = await _accountInfoBusiness.UploadAccountInfoAsync(models, appUser);
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
