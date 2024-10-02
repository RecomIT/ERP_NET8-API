using API.Base;
using AutoMapper;
using API.Services;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Payroll.DTO.Salary;
using DAL.DapperObject.Interface;
using BLL.Salary.Salary.Interface;
using Shared.Payroll.Filter.Salary;
using Shared.Payroll.ViewModel.Salary;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Salary.Process
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class SalaryAllowanceArrearAdjustmentController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        private readonly ISalaryAllowanceArrearAdjustmentBusiness _allowanceArrearAdjBusiness;
        public SalaryAllowanceArrearAdjustmentController(ISysLogger sysLogger, ISalaryAllowanceArrearAdjustmentBusiness allowanceArrearAdjBusiness,
           IMapper mapper, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _mapper = mapper;
            _sysLogger = sysLogger;
            _allowanceArrearAdjBusiness = allowanceArrearAdjBusiness;
        }

        [HttpGet, Route("DownloadSalaryAllowanceArrearAdj")]
        public async Task<IActionResult> DownloadSalaryAllowanceArrearAdjAsync(string fileName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel", fileName);
            var provider = new FileExtensionContentTypeProvider();
            string contenttype = "";
            if (System.IO.File.Exists(filepath))
            {
                contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, fileName);
        }

        [HttpPost, Route("UploadSalaryAllowanceArrearAdj")]
        public async Task<IActionResult> UploadSalaryAllowanceArrearAdjAsync([FromForm] UploadSalaryAllowanceArrearAdjustmentDTO uploadedFile)
        {
            var appUser = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedFile.ExcelFile?.Length > 0)
                    {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<SalaryAllowanceArrearAdjustmentDTO> arrearAdjustmentDTOs = new List<SalaryAllowanceArrearAdjustmentDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var amt = worksheet.Cells[row, 2].Value.ToString();
                                if (amt != null)
                                {
                                    SalaryAllowanceArrearAdjustmentDTO arrearAdjustmentDTO = new SalaryAllowanceArrearAdjustmentDTO();

                                    var employeeCode = worksheet.Cells[row, 1].Value.ToString();
                                    var amount = Convert.ToDecimal(worksheet.Cells[row, 2].Value.ToString());

                                    arrearAdjustmentDTO.EmployeeCode = employeeCode;
                                    arrearAdjustmentDTO.Amount = amount;
                                    arrearAdjustmentDTO.AllowanceNameId = uploadedFile.AllowanceNameId;
                                    arrearAdjustmentDTO.SalaryYear = uploadedFile.SalaryYear;
                                    arrearAdjustmentDTO.SalaryMonth = uploadedFile.SalaryMonth;

                                    if (amount < 0)
                                    {
                                        arrearAdjustmentDTO.Flag = "Adjustment";
                                    }
                                    else
                                    {
                                        arrearAdjustmentDTO.Flag = "Arrear";
                                    }

                                    arrearAdjustmentDTOs.Add(arrearAdjustmentDTO);
                                }
                                else
                                {
                                    continue;
                                }

                            }

                        }
                        var data = await _allowanceArrearAdjBusiness.UploadSalaryAllowanceArrearAdjAsync(arrearAdjustmentDTOs, appUser);
                        return Ok(data);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, appUser.Database, "SalaryReviewController", "UploadSalaryReviewExcelAsync", appUser);

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet("GetSalaryAllowanceArrearAdjustmentList")]
        public async Task<IActionResult> GetSalaryAllowanceArrearAdjustmentListAsync([FromQuery] SalaryAllowanceArrearAdjustment_Filter arrearAdjustment_Filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data_list = await _allowanceArrearAdjBusiness.GetSalaryAllowanceArrearAdjustmentListAsync(arrearAdjustment_Filter, user);
                    Response.AddPagination(data_list.Pageparam.PageNumber, data_list.Pageparam.PageSize, data_list.Pageparam.TotalRows, data_list.Pageparam.TotalPages);
                    return Ok(data_list.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentController", "GetMonthlyIncentiveListAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetSalaryAllowanceArrearAdjustmentById")]
        public async Task<IActionResult> GetSalaryAllowanceArrearAdjustmentByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0)
                {
                    var data = await _allowanceArrearAdjBusiness.GetSalaryAllowanceArrearAdjustmentByIdAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentController", "GetSalaryAllowanceArrearAdjustmentByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPut, Route("UpdateSalaryAllowanceArrearAdjustment")]
        public async Task<IActionResult> UpdateSalaryAllowanceArrearAdjustmentAsync(SalaryAllowanceArrearAdjustmentViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && model.Id > 0 && user.HasBoth)
                {
                    var data = await _allowanceArrearAdjBusiness.UpdateSalaryAllowanceArrearAdjustmentAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentController", "UpdateSalaryAllowanceArrearAdjustmentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("DeleteSalaryAllowanceArrearAdjustment")]
        public async Task<IActionResult> DeleteSalaryAllowanceArrearAdjustmentAsync(SalaryAllowanceArrearAdjustmentViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && model.Id > 0 && user.HasBoth)
                {
                    var data = await _allowanceArrearAdjBusiness.DeleteSalaryAllowanceArrearAdjustmentAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentController", "DeleteSalaryAllowanceArrearAdjustmentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("Save")]
        public async Task<IActionResult> SaveAsync(SalaryAllowanceArrearAdjustmentMasterDTO info)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _allowanceArrearAdjBusiness.SaveAsync(info, user);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentController", "SaveAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetPendingSalaryAllowanceArrearAdjustment")]
        public async Task<IActionResult> GetPendingSalaryAllowanceArrearAdjustmentAsync([FromQuery] SalaryAllowanceArrearAdjustment_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _allowanceArrearAdjBusiness.GetPendingSalaryAllowanceArrearAdjustmentAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentController", "GetPendingSalaryAllowanceArrearAdjustmentAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("ArrearAdjustmentApproval")]
        public async Task<IActionResult> ArrearAdjustmentApprovalAsync(ArrearAdjustmentApprovalDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var status = await _allowanceArrearAdjBusiness.ArrearAdjustmentApprovalAsync(model, user);
                    if (status.Status)
                    {
                        return Ok(status);
                    }
                    return BadRequest(status);
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceArrearAdjustmentController", "ArrearAdjustmentApprovalAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
