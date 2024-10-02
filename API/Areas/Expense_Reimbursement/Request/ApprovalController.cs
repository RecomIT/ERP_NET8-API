using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using Shared.Helpers;
using API.Base;
using DAL.DapperObject.Interface;
using BLL.Expense_Reimbursement.Interface.Request;
using Shared.Expense_Reimbursement.DTO.Request;
using Shared.Expense_Reimbursement.Filter.Request;
using Shared.Expense_Reimbursement.ViewModel.Email;
using Shared.Expense_Reimbursement.ViewModel.Request;

namespace API.Expense_Reimbursement.Request
{
    [ApiController, Area("ExpenseReimbursement"), Route("api/[area]/Request/[controller]"), Authorize]
    public class ApprovalController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IApprovalBusiness _approvalBusiness;

        public ApprovalController(
           ISysLogger sysLogger,
           IApprovalBusiness approvalBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _approvalBusiness = approvalBusiness;            
        }

        [HttpGet, Route("GetRequestCount")]
        public async Task<IActionResult> GetRequestCountAsync(long authorityId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _approvalBusiness.GetRequestCountAsync(authorityId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetRequestCountEmployeeWiseAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetRequestCountAdvance")]
        public async Task<IActionResult> GetRequestCountAdvanceAsync(long authorityId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _approvalBusiness.GetRequestCountAdvanceAsync(authorityId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetRequestCountEmployeeWiseAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetRequestDataAdvance")]
        public async Task<IActionResult> GetRequestDataAdvanceAsync([FromQuery] RequestFilter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _approvalBusiness.GetRequestDataAdvanceAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet, Route("GetRequestDataByAdvanceId")]
        public async Task<IActionResult> GetRequestDataAdvanceAsync(long requestId, long employeeId, string transactionType, string stateStatus)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _approvalBusiness.GetRequestDataAdvanceAsync(new RequestFilter { RequestId = requestId, EmployeeId = employeeId, TransactionType = transactionType, StateStatus = stateStatus }, user);
                    if (data.ListOfObject.Count() > 0)
                    {
                        return Ok(data.ListOfObject.FirstOrDefault());
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetAssetIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }



        [HttpGet, Route("GetRequestData")]
        public async Task<IActionResult> GetRequestDataAsync([FromQuery] RequestFilter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _approvalBusiness.GetRequestDataAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

        [HttpGet, Route("GetRequestDetailsData")]
        public async Task<IActionResult> GetRequestDetailsDataAsync([FromQuery] RequestFilter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _approvalBusiness.GetRequestDetailsDataAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetRequestDataListAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetRequestDataById")]
        public async Task<IActionResult> GetRequestDataAsync(long requestId, long employeeId, string transactionType, string stateStatus)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _approvalBusiness.GetRequestDataAsync(new RequestFilter { RequestId = requestId, EmployeeId = employeeId, TransactionType = transactionType, StateStatus = stateStatus }, user);
                    if (data.ListOfObject.Count() > 0)
                    {
                        return Ok(data.ListOfObject.FirstOrDefault());
                    }
                    else
                    {
                        return NoContent();
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AssetController", "GetAssetIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("Approved")]
        public async Task<IActionResult> ApprovedRequestAsync([FromBody] ApprovedDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _approvalBusiness.ApprovedRequestAsync(model, user);
                    return Ok(data);                   
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ApprovalController", "ApprovedRequestAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("DownloadFile")]
        public async Task<IActionResult> DownloadFile(string path)
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

                await _sysLogger.SaveHRMSException(ex, user.Database, "ApprovalController", "DownloadFile", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("SendEmail")]
        public async Task<IActionResult> EmailSendAsync([FromQuery] EmailDataViewModel request)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _approvalBusiness.EmailSendAsync(request, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "EmailSendAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }

        //[HttpPost, Route("DeleteRequest")]
        //public async Task<IActionResult> DeleteRequestAsync(RequestFilter model)
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (ModelState.IsValid && user.HasBoth)
        //        {
        //            var data = await _requestBusiness.DeleteRequestAsync(model, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "DeleteRequestAsync", user);
        //        return BadRequest(ResponseMessage.Invalid());
        //    }
        //}




        //#region  Travels

        //[HttpGet, Route("GetLocation")]
        //public async Task<IActionResult> GetLocationAsync()
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (user.HasBoth)
        //        {
        //            var data_list = await _requestBusiness.GetLocationAsync(user);
        //            return Ok(data_list);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetLocationAsync", user);
        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}

        //[HttpPost, Route("SaveTravel")]
        //public async Task<IActionResult> SaveTravelAsync([FromBody] TravelDTO model)
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var validator = await _requestBusiness.ValidatorTravelAsync(model, user);
        //            if (validator != null && validator.Status == true)
        //            {
        //                return Ok(validator);
        //            }
        //            else
        //            {
        //                var data = await _requestBusiness.SaveTravelAsync(model, user);
        //                return Ok(data);
        //            }

        //        }
        //        return BadRequest(new { msg = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveTravelAsync", user);
        //        return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //#endregion

        //#region  Conveyance

        //[HttpPost, Route("SaveConveyance")]
        //public async Task<IActionResult> SaveConveyanceAsync(List<ConveyanceDTO> model)
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var validator = await _requestBusiness.ValidationConveyanceAsync(model, user);
        //            if (validator != null && validator.Status == true)
        //            {
        //                return Ok(validator);
        //            }
        //            else
        //            {
        //                var data = await _requestBusiness.SaveConveyanceAsync(model, user);
        //                return Ok(data);
        //            }
        //        }
        //        return BadRequest(new { msg = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveConveyanceAsync", user);
        //        return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //#endregion

        //#region  Expat

        //[HttpGet, Route("GetCompanyName")]
        //public async Task<IActionResult> GetCompanyNameAsync()
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (user.HasBoth)
        //        {
        //            var data_list = await _requestBusiness.GetCompanyNameAsync(user);
        //            return Ok(data_list);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetCompanyNameAsync", user);
        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}

        //[HttpGet, Route("GetBillType")]
        //public async Task<IActionResult> GetBillTypeAsync()
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (user.HasBoth)
        //        {
        //            var data_list = await _requestBusiness.GetBillTypeAsync(user);
        //            return Ok(data_list);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetBillTypeAsync", user);
        //        return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
        //    }
        //}

        //[HttpPost, Route("SaveExpat")]
        //public async Task<IActionResult> SaveExpatAsync(List<ExpatDTO> model)
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var validator = await _requestBusiness.ValidationExpatAsync(model, user);
        //            if (validator != null && validator.Status == true)
        //            {
        //                return Ok(validator);
        //            }
        //            else
        //            {
        //                var data = await _requestBusiness.SaveExpatAsync(model, user);
        //                return Ok(data);
        //            }
        //        }
        //        return BadRequest(new { msg = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveExpatAsync", user);
        //        return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //#endregion

        //#region  Entertainment

        //[HttpPost, Route("SaveEntertainment")]
        //public async Task<IActionResult> SaveEntertainmentAsync(List<EntertainmentDTO> model)
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var validator = await _requestBusiness.ValidationEntertainmentAsync(model, user);
        //            if (validator != null && validator.Status == true)
        //            {
        //                return Ok(validator);
        //            }
        //            else
        //            {
        //                var data = await _requestBusiness.SaveEntertainmentAsync(model, user);
        //                return Ok(data);
        //            }
        //        }
        //        return BadRequest(new { msg = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveEntertainmentAsync", user);
        //        return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //[HttpPost, Route("SaveEntertainmentUploadFile")]
        //public async Task<IActionResult> SaveEntertainmentUploadFileAsync([FromForm] EntertainmentDTO model)
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (ModelState.IsValid && user.HasBoth)
        //        {
        //            var data = await _requestBusiness.SaveEntertainmentUploadFileAsync(model, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(new { message = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SavePayrollException(ex, user.Database, "RequestController", "SaveEntertainmentUploadFileAsync", user);
        //        return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //#endregion

        //#region  Training

        //[HttpPost, Route("SaveTraining")]
        //public async Task<IActionResult> SaveTrainingAsync([FromBody] TrainingDTO model)
        //{
        //    var user = AppUser();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var validator = await _requestBusiness.ValidatorTrainingAsync(model, user);
        //            if (validator != null && validator.Status == true)
        //            {
        //                return Ok(validator);
        //            }
        //            else
        //            {
        //                var data = await _requestBusiness.SaveTrainingAsync(model, user);
        //                return Ok(data);
        //            }

        //        }
        //        return BadRequest(new { msg = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex)
        //    {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveTrainingAsync", user);
        //        return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //#endregion

    }
}
