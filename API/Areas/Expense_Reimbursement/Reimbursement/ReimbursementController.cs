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
using Dapper;
using Shared.OtherModels.User;
using BLL.Expense_Reimbursement.Interface.ReqReimbursementuest;
using BLL.Expense_Reimbursement.Implementation.Reimbursement;

namespace API.Expense_Reimbursement.Reimbursement
{
    [ApiController, Area("ExpenseReimbursement"), Route("api/[area]/Reimbursement/[controller]"), Authorize]
    public class ReimbursementController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IReimbursementBusiness _reimbursementBusiness;

        public ReimbursementController(
           ISysLogger sysLogger,
           IReimbursementBusiness reimbursementBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _reimbursementBusiness = reimbursementBusiness;            
        }


        [HttpGet, Route("GetRequestCountReimbursement")]
        public async Task<IActionResult> GetRequestCountReimbursementAsync(long authorityId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _reimbursementBusiness.GetRequestCountReimbursementAsync(authorityId, user);
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

        [HttpGet, Route("GetRequestCountEmployeeWise")]
        public async Task<IActionResult> GetRequestCountEmployeeWiseAsync(long employeeId, string transactionType)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _reimbursementBusiness.GetRequestCountEmployeeWiseAsync(employeeId, transactionType, user);
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

        [HttpGet, Route("GetRequestDataReimbursement")]
        public async Task<IActionResult> GetRequestDataReimbursementAsync([FromQuery] RequestFilter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _reimbursementBusiness.GetRequestDataReimbursementAsync(filter, user);
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

        [HttpGet, Route("GetRequestData")]
        public async Task<IActionResult> GetRequestDataAsync([FromQuery] RequestFilter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _reimbursementBusiness.GetRequestDataAsync(filter, user);            
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

        [HttpGet, Route("GetRequestDataList")]
        public async Task<IActionResult> GetRequestDataListAsync([FromQuery] RequestFilter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _reimbursementBusiness.GetRequestDataListAsync(filter, user);
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

        [HttpGet, Route("GetRequestDataByReimbursementId")]
        public async Task<IActionResult> GetRequestDataReimbursementAsync(long requestId, long employeeId, string transactionType)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _reimbursementBusiness.GetRequestDataReimbursementAsync(new RequestFilter { RequestId = requestId, EmployeeId = employeeId, TransactionType = transactionType }, user);
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


        [HttpGet, Route("GetRequestDataById")]
        public async Task<IActionResult> GetAssetIdAsync(long requestId, long employeeId, string transactionType)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _reimbursementBusiness.GetRequestDataAsync(new RequestFilter { RequestId = requestId, EmployeeId = employeeId, TransactionType = transactionType }, user);
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

        [HttpPost, Route("DeleteRequest")]
        public async Task<IActionResult> DeleteRequestAsync(RequestFilter model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _reimbursementBusiness.DeleteRequestAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "DeleteRequestAsync", user);
                return BadRequest(ResponseMessage.Invalid());
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
                    var data = await _reimbursementBusiness.EmailSendAsync(request, user);
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

        [HttpGet, Route("GetRequestID")]
        public async Task<IActionResult> GetRequestIDAsync([FromQuery] RequestFilter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _reimbursementBusiness.GetRequestIDAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetRequestIDAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        #region  Travels

        [HttpGet, Route("GetLocation")]
        public async Task<IActionResult> GetLocationAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data_list = await _reimbursementBusiness.GetLocationAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetLocationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveTravel")]
        public async Task<IActionResult> SaveTravelAsync([FromBody] TravelDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _reimbursementBusiness.ValidatorTravelAsync(model, user);
                    if (validator != null && validator.Status == true)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _reimbursementBusiness.SaveTravelAsync(model, user);
                        return Ok(data);
                    }

                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveTravelAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

        #region  Conveyance

        [HttpPost, Route("SaveConveyance")]
        public async Task<IActionResult> SaveConveyanceAsync(List<ConveyanceDTO> model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _reimbursementBusiness.ValidationConveyanceAsync(model, user);
                    if (validator != null && validator.Status == true)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _reimbursementBusiness.SaveConveyanceAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveConveyanceAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

        #region  Expat

        [HttpGet, Route("GetCompanyName")]
        public async Task<IActionResult> GetCompanyNameAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data_list = await _reimbursementBusiness.GetCompanyNameAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetCompanyNameAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBillType")]
        public async Task<IActionResult> GetBillTypeAsync()
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data_list = await _reimbursementBusiness.GetBillTypeAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "GetBillTypeAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveExpat")]
        public async Task<IActionResult> SaveExpatAsync(List<ExpatDTO> model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _reimbursementBusiness.ValidationExpatAsync(model, user);
                    if (validator != null && validator.Status == true)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _reimbursementBusiness.SaveExpatAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveExpatAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

        #region  Entertainment

        [HttpPost, Route("SaveEntertainment")]
        public async Task<IActionResult> SaveEntertainmentAsync(List<EntertainmentDTO> model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _reimbursementBusiness.ValidationEntertainmentAsync(model, user);
                    if (validator != null && validator.Status == true)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _reimbursementBusiness.SaveEntertainmentAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveEntertainmentAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpPost, Route("SaveEntertainmentUploadFile")]
        public async Task<IActionResult> SaveEntertainmentUploadFileAsync([FromForm] EntertainmentDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _reimbursementBusiness.SaveEntertainmentUploadFileAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(new { message = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SavePayrollException(ex, user.Database, "RequestController", "SaveEntertainmentUploadFileAsync", user);
                return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

        #region  Training

        [HttpPost, Route("SaveTraining")]
        public async Task<IActionResult> SaveTrainingAsync([FromBody] TrainingDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _reimbursementBusiness.ValidatorTrainingAsync(model, user);
                    if (validator != null && validator.Status == true)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _reimbursementBusiness.SaveTrainingAsync(model, user);
                        return Ok(data);
                    }

                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "RequestController", "SaveTrainingAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        #endregion

    }
}
