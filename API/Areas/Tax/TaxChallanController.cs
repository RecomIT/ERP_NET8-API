using API.Base;
using OfficeOpenXml;
using Shared.Helpers;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using Shared.Payroll.DTO.Tax;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxChallanController : ApiBaseController
    {
        private readonly ITaxChallanBusiness _taxChallanBusiness;
        private readonly ISysLogger _sysLogger;
        public TaxChallanController(ITaxChallanBusiness taxChallanBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _taxChallanBusiness = taxChallanBusiness;
        }

        [HttpGet, Route("DownloadIncomeTaxChallanFormat")]
        public async Task<IActionResult> DownloadIncomeTaxChallanFormatAsync()
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel\\TaxChallanUploadFormat.xlsx");
            string contentType = "";
            if (System.IO.File.Exists(filepath))
            {
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contentType, "Tax-Challan-Uploader.xlsx");
        }

        [HttpPost, Route("UploadIncomeTaxChallan")]
        public async Task<IActionResult> UploadIncomeTaxChallanAync([FromForm] TaxChallanUploadDTO model)
        {
            var user = AppUser();
            try
            {
                var appUser = AppUser();
                if (ModelState.IsValid)
                {
                    if (model.File?.Length > 0)
                    {
                        var stream = model.File.OpenReadStream();
                        List<TaxChallanDTO> readExcelDTOs = new List<TaxChallanDTO>();
                        using (var package = new ExcelPackage(stream))
                        {
                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                            var rowCount = worksheet!.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++)
                            {
                                var amt = worksheet.Cells[row, 3].Value?.ToString();
                                if (amt != null)
                                {
                                    TaxChallanDTO readDTO = new TaxChallanDTO();

                                    var employeeCode = worksheet.Cells[row, 1].Value.ToString();
                                    var challanNo = worksheet.Cells[row, 2].Value.ToString();
                                    var amount = Convert.ToDecimal(worksheet.Cells[row, 3].Value?.ToString());

                                    var challanDate = worksheet.Cells[row, 4].Value?.ToString();
                                    if (challanDate.IsStringNumber() && challanDate.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        readDTO.ChallanDate = DateTime.FromOADate(Convert.ToDouble(challanDate.RemoveWhitespace()));
                                    }
                                    else
                                    {
                                        readDTO.ChallanDate = challanDate.IsNullEmptyOrWhiteSpace() == false ? Convert.ToDateTime(challanDate.RemoveWhitespace()) : null;
                                    }
                                    var challanBank = worksheet.Cells[row, 5].Value?.ToString() ?? "";
                                    var challanBankBranch = worksheet.Cells[row, 6].Value?.ToString() ?? "";

                                    readDTO.EmployeeCode = employeeCode;
                                    readDTO.ChallanNumber = challanNo;
                                    readDTO.Amount = amount;
                                    readDTO.DepositeBank = challanBank;
                                    readDTO.DepositeBranch = challanBankBranch;

                                    readDTO.TaxMonth = model.TaxMonth;
                                    readDTO.TaxYear = model.TaxYear;
                                    readDTO.FiscalYearId = model.FiscalYearId;
                                    readExcelDTOs.Add(readDTO);

                                }
                                else
                                {
                                    continue;
                                }
                            }

                        }
                        var data = await _taxChallanBusiness.UploadTaxChallanAsync(readExcelDTOs, appUser);
                        if (data.Status == true)
                        {
                            return Ok(data);
                        }
                        else
                        {
                            return BadRequest(data);
                        }
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxChallanController", "UploadIncomeTaxChallanAync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("BulkSave")]
        public async Task<IActionResult> BulkSaveAsync(AllEmployeesTaxChallanInsertDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var status = await _taxChallanBusiness.BulkSaveAsync(model, user);
                    if(status.Status == true)
                    {
                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest(status);
                    }
                }
                return BadRequest(ResponseMessage.InvalidForm);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxChallanController", "UploadIncomeTaxChallanAync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
