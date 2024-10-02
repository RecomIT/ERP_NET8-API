using API.Services;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using BLL.Leave.Interface.Request;
using API.Base;
using Shared.Leave.DTO.Request;
using Shared.Leave.Filter.Request;
using Shared.Leave.ViewModel.Request;

namespace API.Areas.Leave.Request
{

    [ApiController, Area("HRMS"), Route("api/[area]/Leave/[controller]"), Authorize]
    public class LeaveRequestController : ApiBaseController
    {
        private readonly IEmployeeLeaveRequestBusiness _employeeLeaveRequestBusiness;
        private readonly ISysLogger _sysLogger;
        public LeaveRequestController(ISysLogger sysLogger, IEmployeeLeaveRequestBusiness employeeLeaveRequestBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employeeLeaveRequestBusiness = employeeLeaveRequestBusiness;
        }

        [HttpGet, Route("GetEmployeeLeaveRequests")]
        public async Task<IActionResult> GetEmployeeLeaveRequestsAsync([FromQuery] LeaveRequest_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeLeaveRequestBusiness.GetEmployeeLeaveRequestsAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "GetEmployeeLeaveRequestsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetEmployeeLeaveRequestById")]
        public async Task<IActionResult> GetEmployeeLeaveRequestByIdAsync([FromQuery] LeaveRequest_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeLeaveRequestBusiness.GetEmployeeLeaveRequestsAsync(filter, user);

                    if (data.ListOfObject.FirstOrDefault() != null)
                    {
                        return Ok(data.ListOfObject.FirstOrDefault());
                    }
                    return NoContent();
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "GetEmployeeLeaveRequestByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("SaveEmployeeLeaveRequest")]
        public async Task<IActionResult> SaveEmployeeLeaveRequestAsync(LeaveRequestDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    model.LeaveRequest.AppliedToDate = model.LeaveRequest.DayLeaveType == "Half-Day" ? model.LeaveRequest.AppliedFromDate : model.LeaveRequest.AppliedToDate;
                    var validator = await _employeeLeaveRequestBusiness.ValidatorEmployeeLeaveRequestAsync(model.LeaveRequest, user);
                    if (validator != null && validator.Status == false)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _employeeLeaveRequestBusiness.SaveEmployeeLeaveRequestAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "SaveEmployeeLeaveRequest", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveEmployeeLeaveRequest2")]
        public async Task<IActionResult> SaveEmployeeLeaveRequest2Async(LeaveRequestDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    model.LeaveRequest.AppliedToDate = model.LeaveRequest.DayLeaveType == "Half-Day" ? model.LeaveRequest.AppliedFromDate : model.LeaveRequest.AppliedToDate;
                    model.LeaveRequest.AppliedTotalDays = model.LeaveRequest.DayLeaveType == "Half-Day" ? Convert.ToDecimal(.50) : model.LeaveRequest.AppliedTotalDays;
                    var validator = await _employeeLeaveRequestBusiness.ValidatorEmployeeLeaveRequestAsync(model.LeaveRequest, user);
                    if (validator != null && validator.Status == true)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _employeeLeaveRequestBusiness.SaveEmployeeLeaveRequest2Async(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "SaveEmployeeLeaveRequest", user);
                return BadRequest(ResponseMessage.Invalid());
            }
        }

        [HttpPost, Route("SaveEmployeeLeaveRequest3")]
        public async Task<IActionResult> SaveEmployeeLeaveRequest3Async([FromForm] EmployeeLeaveRequestDTO requestData)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    LeaveRequestDTO model = new LeaveRequestDTO();
                    model.LeaveRequest = requestData;

                    if (!string.IsNullOrEmpty(requestData.LeaveDaysJson) && !string.IsNullOrWhiteSpace(requestData.LeaveDaysJson))
                    {
                        model.LeaveDays = JsonReverseConverter.JsonToObject<List<EmployeeLeaveDaysDTO>>(requestData.LeaveDaysJson);
                        model.File = requestData.File;
                    }
                    model.FilePath = requestData.FilePath;

                    model.LeaveRequest.AppliedToDate = model.LeaveRequest.DayLeaveType == "Half-Day" ? model.LeaveRequest.AppliedFromDate : model.LeaveRequest.AppliedToDate;
                    model.LeaveRequest.AppliedTotalDays = model.LeaveRequest.DayLeaveType == "Half-Day" ? Convert.ToDecimal(.50) : model.LeaveRequest.AppliedTotalDays;
                    var validator = await _employeeLeaveRequestBusiness.ValidatorEmployeeLeaveRequestAsync(model.LeaveRequest, user);
                    if (validator != null && validator.Status == true)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var data = await _employeeLeaveRequestBusiness.SaveEmployeeLeaveRequest2Async(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "SaveEmployeeLeaveRequest", user);
                return BadRequest(ResponseMessage.Invalid());
            }
        }

        [HttpPost, Route("SaveEmployeeLeaveRequestStatus")]
        public async Task<IActionResult> SaveEmployeeLeaveRequestStatusAsync(EmployeeLeaveRequestStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var status = await _employeeLeaveRequestBusiness.SaveEmployeeLeaveRequestStatusAsync(model, user);
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "SaveEmployeeLeaveRequest", user);
                return BadRequest(ResponseMessage.Invalid());
            }
        }

        [HttpPost, Route("DeleteEmployeeLeaveRequest")]
        public async Task<IActionResult> DeleteEmployeeLeaveRequestAsync(DeleteEmployeeLeaveRequestDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _employeeLeaveRequestBusiness.DeleteEmployeeLeaveRequestAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "DeleteEmployeeLeaveRequest", user);
                return BadRequest(ResponseMessage.Invalid());
            }
        }

        [HttpPost, Route("ApprovedLeaveCancellation")]
        public async Task<IActionResult> ApprovedLeaveCancellationAsync(ApprovedLeaveCancellationDTO model)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _employeeLeaveRequestBusiness.ApprovedLeaveCancellationAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "ApprovedLeaveCancellation", user);
                return BadRequest(ResponseMessage.Invalid());
            }
        }

        [HttpGet, Route("LeaveRequestEmailSend")]
        public async Task<IActionResult> LeaveRequestEmailSendAsync([FromQuery] LeaveRequestEmail_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _employeeLeaveRequestBusiness.LeaveRequestEmailSendAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "LeaveRequestEmailSend", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }

        [HttpPost, Route("SendLeaveEmail")]
        public async Task<IActionResult> SendLeaveEmailAsync(ExecutionStatus status)
        {
            var user = AppUser();
            if (user.HasBoth)
            {
                var data = await _employeeLeaveRequestBusiness.SendLeaveEmailAsync(status, user);
                return Ok("Ok");
            }
            return BadRequest(ResponseMessage.InvalidParameters);
        }

        [HttpGet, Route("GetEmployeeLeaveHistory")]
        public async Task<IActionResult> GetEmployeeLeaveHistoryAsync([FromQuery] LeaveRequest_Filter filter, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    pageNumber = Utility.PageNumber(pageNumber); pageSize = Utility.PageSize(pageSize);
                    var allData = await _employeeLeaveRequestBusiness.GetEmployeeLeaveHistoryAsync(filter, user);
                    var data = PagedList<EmployeeLeaveRequestViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "GetEmployeeLeaveHistoryAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            return BadRequest(ResponseMessage.InvalidParameters);
        }

        [HttpGet, Route("GetSubordinatesLeaveRequests")]
        public async Task<IActionResult> GetSubordinatesLeaveRequestsAsync([FromQuery] LeaveRequest_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeLeaveRequestBusiness.GetSubordinatesEmployeeLeaveRequestAsync(filter, user);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "GetSubordinatesLeaveRequestsAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            return BadRequest(ResponseMessage.InvalidParameters);

        }

        [HttpPost, Route("LeaveRequestApproval")]
        public async Task<IActionResult> LeaveRequestApprovalAsync(EmployeeLeaveRequestStatusDTO model)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth && ModelState.IsValid)
                {
                    var data = await _employeeLeaveRequestBusiness.LeaveRequestApprovalAsync(model, user);
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "GetSubordinatesLeaveRequestsAsync", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            return BadRequest(ResponseMessage.InvalidParameters);

        }

        [HttpGet, Route("GetEmployeeLeaveRequestInfoAndDetailById")]
        public async Task<IActionResult> GetEmployeeLeaveRequestInfoAndDetailById(long id)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _employeeLeaveRequestBusiness.GetEmployeeLeaveRequestInfoAndDetailById(id, user);
                    return Ok(data);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveRequestController", "GetEmployeeLeaveRequestInfoAndDetailById", user);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            return BadRequest(ResponseMessage.InvalidParameters);
        }
    }

}
