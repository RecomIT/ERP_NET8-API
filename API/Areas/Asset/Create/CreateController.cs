using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Helpers;
using Microsoft.AspNetCore.StaticFiles;
using OfficeOpenXml;
using BLL.Asset.Interface.Create;
using Shared.Asset.Filter.Create;
using Shared.Asset.DTO.Create;
using API.Base;
using DAL.DapperObject.Interface;

namespace API.Asset_Module.Create
{
    [ApiController, Area("Asset"), Route("api/[area]/Create/[controller]"), Authorize]
    public class CreateController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ICreateBusiness _createBusiness;

        public CreateController(
           ISysLogger sysLogger,
           ICreateBusiness createBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _createBusiness = createBusiness;            
        }

        [HttpGet, Route("GetAsset")]
        public async Task<IActionResult> GetAssetAsync([FromQuery] Create_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _createBusiness.GetAssetAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet, Route("GetAssetById")]
        public async Task<IActionResult> GetAssetIdAsync(long assetId)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _createBusiness.GetAssetAsync(new Create_Filter { AssetId = assetId }, user);
                    if (data.ListOfObject.Count() > 0) {
                        return Ok(data.ListOfObject.FirstOrDefault());
                    }
                    else {
                        return NoContent();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetAssetIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveAsset")]
        public async Task<IActionResult> SaveAssetAsync(Create_DTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _createBusiness.ValidatorAssetAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {  
                        var data = await _createBusiness.SaveAssetAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "SaveAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("DownloadExcelFile")]
        public async Task<IActionResult> DownloadExcelAsync(string fileName)
        {
            var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files\\Excel\\Asset", fileName);
            var provider = new FileExtensionContentTypeProvider();
            string contenttype = "";
            if (System.IO.File.Exists(filepath)) {
                contenttype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            }
            var bytes = await System.IO.File.ReadAllBytesAsync(filepath);
            return File(bytes, contenttype, fileName);
        }

        [HttpPost, Route("UploadExcelFile")]
        public IActionResult UploadExcelFileAsync([FromForm] UploadFile_Filter uploadedFile)
        {
            try {
                var appUser = AppUser();

                if (ModelState.IsValid) {
                    if (uploadedFile.File?.Length > 0) {
                        var stream = uploadedFile.File.OpenReadStream();
                        List<UploadFile_DTO> readExcelDTOs = new List<UploadFile_DTO>();
                        using (var package = new ExcelPackage(stream)) {
                            var worksheet = package.Workbook.Worksheets.First();
                            var rowCount = worksheet.Dimension.Rows;
                            for (var row = 2; row <= rowCount; row++) {
                                var productId = worksheet.Cells[row, 1].Value?.ToString() ?? "";

                                if (uploadedFile.Format == "Serial") {    
                                    var number = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                                    UploadFile_DTO readDTO = new UploadFile_DTO();
                                    readDTO.ProductId = productId;
                                    readDTO.Number = number;
                                    readExcelDTOs.Add(readDTO);
                                }
                                if (uploadedFile.Format == "SIM") {     
                                    var number = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                                    var pin = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                                    var puk = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                                    UploadFile_DTO readDTO = new UploadFile_DTO();
                                    readDTO.ProductId = productId;
                                    readDTO.Number = number;
                                    readDTO.PIN = pin;
                                    readDTO.PUK = puk;
                                    readExcelDTOs.Add(readDTO);
                                }
                                if (uploadedFile.Format == "Phone") {
                                    var number = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                                    var imeI1 = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                                    var imeI2 = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                                    UploadFile_DTO readDTO = new UploadFile_DTO();
                                    readDTO.ProductId = productId;
                                    readDTO.Number = number;
                                    readDTO.IMEI1 = imeI1;
                                    readDTO.IMEI2 = imeI2;
                                    readExcelDTOs.Add(readDTO);
                                }
                                if (uploadedFile.Format == "Product ID") {
                                    UploadFile_DTO readDTO = new UploadFile_DTO();
                                    readDTO.ProductId = productId;
                                    readExcelDTOs.Add(readDTO);
                                }
                            }
                        }

                        return Ok(readExcelDTOs);

                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("CheckProductValidation")]
        public async Task<IActionResult> CheckProductValidationAsync(List<UploadFile_DTO> upload)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _createBusiness.ValidatorUploadExcelAsync(upload, user);
                    return Ok(validator);
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "CheckProductValidation", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }


        [HttpPost, Route("SaveExcelFile")]
        public async Task<IActionResult> UploadExcelAsync( List<UploadFile_DTO> upload)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _createBusiness.UploadExcelAsync( upload, user);
                    return Ok(data);
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "UploadExcelAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetProduct")]
        public async Task<IActionResult> GetProductAsync([FromQuery] Product_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _createBusiness.GetProductAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetProductAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
      
        }

        [HttpGet, Route("GetAssetDetails")]
        public async Task<IActionResult> GetAssetDetailsAsync([FromQuery] Create_Filter filter)
        {
            var user = AppUser();
            try {
                if (user.HasBoth && ModelState.IsValid) {
                    var data = await _createBusiness.GetAssetDetailsAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetAssetDetailsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }


        }

        [HttpGet, Route("SendEmail")]
        public async Task<IActionResult> EmailSendAsync()
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _createBusiness.EmailSendAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CreateController", "EmailSendAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }



        [HttpGet, Route("GetAssetDropdown")]
        public async Task<IActionResult> GetAssetDropdownAsync([FromQuery] Create_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _createBusiness.GetAssetDropdownAsync(filter, user));
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ExcelGenerator", "GetAssetDropdownAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    



        //[HttpPost, Route("ApprovedAsset")]
        //public async Task<IActionResult> ApprovedAssetAsync(int assetId,int assigningId, string activeTab)
        //{
        //    var user = AppUser();
        //    try {
        //        if (ModelState.IsValid) {
        //            var data = await _createBusiness.ApprovedAssetAsync(assetId, assigningId, activeTab, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "SaveAssetAsync", user);
        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}
    }
}
