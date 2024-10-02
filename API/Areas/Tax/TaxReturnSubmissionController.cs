using System;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using BLL.Tax.Interface;
using API.Base;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.DTO.Tax;

namespace API.Areas.Tax
{
    [ApiController, Area("Payroll"), Route("api/[area]/Tax/[controller]"), Authorize]
    public class TaxReturnSubmissionController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly ITaxReturnSubmissionBusiness _taxReturnSubmissionBusiness;
        public TaxReturnSubmissionController(ITaxReturnSubmissionBusiness taxReturnSubmissionBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _taxReturnSubmissionBusiness = taxReturnSubmissionBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetEmployeeTaxReturnSubmission")]
        public async Task<IActionResult> GetEmployeeTaxReturnSubmissionAsync([FromQuery] EmployeeTaxReturnSubmission_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _taxReturnSubmissionBusiness.GetEmployeeTaxReturnSubmissionAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReturnSubmissionController", "GetEmployeeTaxReturnSubmissionAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SaveEmployeeTaxReturnSubmission")]
        public async Task<IActionResult> SaveEmployeeTaxReturnSubmissionAsync([FromForm] EmployeeTaxReturnSubmissionDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _taxReturnSubmissionBusiness.SaveEmployeeTaxReturnSubmissionAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReturnSubmissionController", "SaveEmployeeTaxReturnSubmissionAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("DownloadEmployeeTaxReturnFile")]
        public async Task<IActionResult> DownloadEmployeeTaxReturnFile(string path)
        {
            var user = AppUser();
            try
            {
                if (!Utility.IsNullEmptyOrWhiteSpace(path) && user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var format = Utility.GetFileExtension(path);
                    var fileName = Utility.GetFileName(path);
                    var filebytes = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, path));
                    var mimeType = Utility.GetFileMimetype(format ?? "");
                    return File(filebytes, mimeType, fileName);
                }
                return NotFound();
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReturnSubmissionController", "DownloadEmployeeTaxReturnFile", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeTaxReturnSubmissionById")]
        public async Task<IActionResult> GetEmployeeTaxReturnSubmissionByIdAsync(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _taxReturnSubmissionBusiness.GetEmployeeTaxReturnSubmissionByIdAsync(id, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "TaxReturnSubmissionController", "GetEmployeeTaxReturnSubmissionAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        //[HttpGet, Route("GetEmployeeTaxReturnSubmission")]
        //public async Task<IActionResult> GetEmployeeTaxReturnSubmissionAsync([FromQuery] EmployeeTaxReturnSubmission_Filter filter)
        //{
        //    var user = AppUser();
        //    try {
        //        if (user.HasBoth) {
        //            var data = await _taxReturnSubmissionBusiness.GetEmployeeTaxReturnSubmissionAsync(filter, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(new { message = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SavePayrollException(ex, user.Database, "TaxReturnSubmissionController", "GetEmployeeTaxReturnSubmissionAsync", user);
        //        return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //[HttpPost, Route("SaveEmployeeTaxReturnSubmission")]
        //public async Task<IActionResult> SaveEmployeeTaxReturnSubmissionAsync([FromForm] EmployeeTaxReturnSubmissionDTO model)
        //{
        //    var user = AppUser();
        //    try {
        //        if (ModelState.IsValid && user.HasBoth) {
        //            var data = await _taxReturnSubmissionBusiness.SaveEmployeeTaxReturnSubmissionAsync(model, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(new { message = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SavePayrollException(ex, user.Database, "TaxReturnSubmissionController", "SaveEmployeeTaxReturnSubmissionAsync", user);
        //        return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //[HttpGet, Route("DownloadEmployeeTaxReturnFile")]
        //public async Task<IActionResult> DownloadEmployeeTaxReturnFile(string path)
        //{
        //    var user = AppUser();
        //    try {
        //        if (!Utility.IsNullEmptyOrWhiteSpace(path) && user.CompanyId > 0 && user.OrganizationId > 0) {
        //            var format = Utility.GetFileExtension(path);
        //            var fileName = Utility.GetFileName(path);
        //            var filebytes = Utility.GetFileBytes(string.Format(@"{0}/{1}", Utility.PhysicalDriver, path));
        //            var mimeType = Utility.GetFileMimetype(format ?? "");
        //            return File(filebytes, mimeType, fileName);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex) {

        //        await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReturnSubmissionController", "DownloadEmployeeTaxReturnFile", user);
        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}
    }
}
