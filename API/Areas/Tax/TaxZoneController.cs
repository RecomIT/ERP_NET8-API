using API.Base;
using API.Services;
using BLL.Base.Interface;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Payroll.ViewModel.Tax;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxZoneController : ApiBaseController
    {
        private readonly ITaxZoneBusiness _taxZoneBusiness;
        private readonly ISysLogger _sysLogger;
        public TaxZoneController(ITaxZoneBusiness taxZoneBusiness, IClientDatabase clientDatabase, ISysLogger sysLogger) : base(clientDatabase)
        {
            _taxZoneBusiness = taxZoneBusiness;
            _sysLogger = sysLogger;
        }

        [HttpPost, Route("SaveEmployeeTaxZone")]
        public async Task<IActionResult> SaveEmployeeTaxZoneAsync(List<EmployeeTaxZoneViewModel> taxzones)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _taxZoneBusiness.SaveEmployeeTaxZoneAsync(taxzones, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneController", "SaveEmployeeTaxZone", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeTaxZoneById")]
        public async Task<IActionResult> GetEmployeeTaxZoneByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (id > 0)
                {
                    var data = await _taxZoneBusiness.GetEmployeeTaxZoneByIdAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneController", "GetEmployeeTaxZoneByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPut, Route("UpdateTaxZone")]
        public async Task<IActionResult> UpdateTaxZoneAsync(EmployeeTaxZoneViewModel model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && model.EmployeeTaxZoneId > 0 && user.HasBoth)
                {
                    var validator = await _taxZoneBusiness.TaxZoneValidatorAsync(new List<EmployeeTaxZoneViewModel>() { model }, user);
                    if (validator != null)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _taxZoneBusiness.UpdateTaxZoneAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneController", "GetEmployeeTaxZoneByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeTaxZones")]
        public async Task<IActionResult> GetEmployeeTaxZonesAsync(long? employeeId, string taxZone, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                pageNumber = Utility.PageNumber(pageNumber);
                pageSize = Utility.PageSize(pageSize);
                var allData = await _taxZoneBusiness.GetEmployeeTaxZonesAsync(employeeId ?? 0, taxZone ?? "", user);
                var data = PagedList<EmployeeTaxZoneViewModel>.ToPagedList(allData, pageNumber, pageSize);
                Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                return Ok(data);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneController", "GetEmployeeTaxZoneByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("UploadEmployeeTaxZoneExcel")]
        public async Task<IActionResult> UploadEmployeeTaxZoneExcelAsync([FromForm] EmployeeTaxZoneUpload uploadedFile)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    if (uploadedFile.ExcelFile?.Length > 0)
                    {
                        var stream = uploadedFile.ExcelFile.OpenReadStream();
                        List<EmployeeTaxZoneViewModel> taxZones = new List<EmployeeTaxZoneViewModel>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var amount = worksheet.Cells[row, 3].Value?.ToString();
                                if (amount != null)
                                {
                                    EmployeeTaxZoneViewModel taxZone = new EmployeeTaxZoneViewModel();
                                    var EmployeeCode = worksheet.Cells[row, 1].Value.ToString();
                                    var TaxZone = worksheet.Cells[row, 2].Value.ToString();
                                    var MinimumTaxAmount = worksheet.Cells[row, 3].Value.ToString();
                                    var effectiveDate = Convert.ToDateTime(worksheet.Cells[row, 4].Value).Date;

                                    taxZone.EmployeeCode = EmployeeCode;
                                    taxZone.TaxZone = TaxZone;
                                    taxZone.MinimumTaxAmount = Convert.ToDecimal(MinimumTaxAmount);
                                    taxZone.EffectiveDate = effectiveDate;
                                    taxZones.Add(taxZone);
                                }
                                else
                                {
                                    continue;
                                }
                            }
                        }
                        var data = await _taxZoneBusiness.UploadTaxZoneAsync(taxZones, user);
                        return Ok(data);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneController", "UploadEmployeeTaxZoneExcelAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadEmployeeTaxZoneExcel")]
        public async Task<IActionResult> DownloadEmployeeTaxZoneExcelAsync(string fileName)
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
    }
}
