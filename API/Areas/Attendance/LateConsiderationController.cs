
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Shared.Helpers;
using DAL.DapperObject;
using BLL.Base.Interface;
using System.Collections.Generic;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.Attendance;
using API.Base;
using Shared.Attendance.ViewModel.Attendance.EarlyDeparture;
using Shared.Attendance.ViewModel.Attendance.LateConsideration;

namespace API.Areas.Attendance
{
    [ApiController, Area("HRMS"), Route("api/[area]/[controller]"), Authorize]
    public class LateConsiderationController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly ILateConsiderationBusiness _lateConsiderationBusiness;
        private readonly ISysLogger _sysLogger;
        public LateConsiderationController(IMapper mapper, ILateConsiderationBusiness lateConsiderationBusiness, IClientDatabase _clientDatabase, ISysLogger sysLogger) : base(_clientDatabase)
        {
            _mapper = mapper;
            _lateConsiderationBusiness = lateConsiderationBusiness;
            _sysLogger = sysLogger;

        }
        [HttpGet, Route("GetLateTransactionDate")]
        public async Task<IActionResult> GetLateTransactionDateAsync(string allowanceType)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _lateConsiderationBusiness.GetLateTransactionDateAsync(appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetLateReasons")]
        public async Task<IActionResult> GetLateReasonsAsync(long lateReasonId, string allowanceType)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _lateConsiderationBusiness.GetLateReasonsAsync(lateReasonId, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetSupervisor")]
        public async Task<IActionResult> GetSupervisorAsync(string allowanceType)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _lateConsiderationBusiness.GetSupervisorAsync(appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }


        [HttpPost, Route("SaveLateRequest")]
        public async Task<IActionResult> SaveApprovalHierarchyGroupsAsync([FromBody] List<LateRequestViewModel> groupAssignmentDTOs)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _lateConsiderationBusiness.SaveLateRequestAsync(groupAssignmentDTOs, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }


        [HttpGet, Route("GetLateConsiderationMaster")]
        public async Task<IActionResult> GetLateConsiderationMasterAsync([FromQuery] LateRequestFilter query)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.CompanyId > 0 && appUser.OrganizationId > 0)
                {
                    var data = await _lateConsiderationBusiness.GetLateConsiderationMasterAsync(query, appUser);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LateConsideration", "GetLateConsiderationMaster", appUser.Username, appUser.OrganizationId, appUser.CompanyId, appUser.BranchId);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetLateConsiderationDetail")]
        public async Task<IActionResult> GetLateConsiderationDetailAsync([FromQuery] long lateRequestsId)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.HasBoth && lateRequestsId > 0)
                {
                    var data = await _lateConsiderationBusiness.GetLateConsiderationDetailAsync(lateRequestsId, appUser);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LateConsideration", "GetLateConsiderationDetail", appUser.Username, appUser.OrganizationId, appUser.CompanyId, appUser.BranchId);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }


        [HttpPost, Route("UpdateStatusLateRequestDetaile")]
        public async Task<IActionResult> UpdateStatusLateRequestDetaileAsync(long lateRequestsDetailId, string comment, string flag, long attendanceId, long lateRequestsId)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _lateConsiderationBusiness.UpdateStatusLateRequestDetaileAsync(lateRequestsDetailId, comment, flag, attendanceId, lateRequestsId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
        [HttpGet, Route("GetLateConsiderationMasterById")]
        public async Task<IActionResult> GetLateConsiderationMasterByIdAsync([FromQuery] LateRequestFilter query)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.CompanyId > 0 && appUser.OrganizationId > 0)
                {
                    var data = await _lateConsiderationBusiness.GetLateConsiderationMasterByIdAsync(query, appUser);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LateConsideration", "GetLateConsiderationMasterById", appUser.Username, appUser.OrganizationId, appUser.CompanyId, appUser.BranchId);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("feedbackEmailLateRequest")]
        public async Task<IActionResult> feedbackEmailLateRequestAsync([FromBody] List<feedbackdata> dataList)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _lateConsiderationBusiness.feedbackEmailLateRequestAsync(dataList, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost("SaveEarlyDeparture")]
        public async Task<IActionResult> SaveEarlyDepartureAsync(EarlyDepartureViewModel model)
        {
            var appUser = AppUser();
            try
            {
                if (ModelState.IsValid && appUser.CompanyId > 0 && appUser.OrganizationId > 0)
                {
                    var data = await _lateConsiderationBusiness.SaveEarlyDepartureAsync(model, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception)
            {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetEarlyDepartureMaster")]
        public async Task<IActionResult> GetEarlyDepartureMaster([FromQuery] LateRequestFilter query)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.CompanyId > 0 && appUser.OrganizationId > 0)
                {
                    var data = await _lateConsiderationBusiness.GetEarlyMasterAsync(query, appUser);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LateConsideration", "GetLateConsiderationMaster", appUser.Username, appUser.OrganizationId, appUser.CompanyId, appUser.BranchId);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetEarlyDepartureById")]
        public async Task<IActionResult> GetEarlyDepartureByIdAsync([FromQuery] long earlyDepartureId)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.CompanyId > 0 && appUser.OrganizationId > 0)
                {
                    var data = await _lateConsiderationBusiness.GetEarlyDepartureByIdAsync(earlyDepartureId, appUser);

                    // Check if there's any data returned
                    if (data is not null)
                    {
                        // Since you expect only one record, you can return it directly
                        return Ok(data);
                    }

                    return NotFound(); // Or another appropriate status code if no record is found
                }

                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LateConsideration", "GetLateConsiderationMaster", appUser.Username, appUser.OrganizationId, appUser.CompanyId, appUser.BranchId);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }


        [HttpPost("UpdateEarlyDeparture")]
        public async Task<IActionResult> UpdateEarlyDepartureAsync([FromQuery] long earlyDepartureId, string comment, string flag)
        {
            var appUser = AppUser();
            try
            {
                if (ModelState.IsValid && appUser.CompanyId > 0 && appUser.OrganizationId > 0)
                {
                    var data = await _lateConsiderationBusiness.UpdateEarlyDepartureAsync(earlyDepartureId, comment, flag, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception)
            {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }


        [HttpPost, Route("feedbackEmailEarlyDeparture")]
        public async Task<IActionResult> feedbackEmailEarlyDepartureAsync([FromBody] List<EarlyDepartureFeedbackdata> dataList)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _lateConsiderationBusiness.feedbackEmailEarlyDepartureAsync(dataList, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);

            }
        }

        [HttpGet, Route("GetEarlyDepartureByIdMaster")]
        public async Task<IActionResult> GetEarlyDepartureByIdMaster([FromQuery] LateRequestFilter query)
        {
            var appUser = AppUser();
            try
            {
                if (appUser.CompanyId > 0 && appUser.OrganizationId > 0)
                {
                    var data = await _lateConsiderationBusiness.GetEarlyMasterByIdAsync(query, appUser);
                    Response.AddPagination(data.Pageparam.PageNumber, data.Pageparam.PageSize, data.Pageparam.TotalRows, data.Pageparam.TotalPages);
                    return Ok(data.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "LateConsideration", "GetLateConsiderationMaster", appUser.Username, appUser.OrganizationId, appUser.CompanyId, appUser.BranchId);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }



    }
}

