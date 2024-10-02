using API.Services;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using Shared.Services;
using System.Threading.Tasks;
using System;
using Shared.Models.Dashboard.CommonDashboard.QueryParam.Leave;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using BLL.Leave.Interface.Request;
using BLL.Dashboard.Admin.Interface;
using API.Base;
using Shared.Leave.DTO.Request;

namespace API.Areas.Leave.Email_Approval
{
    [ApiController, Area("hrms"), Route("api/[area]/Leave/[controller]")]
    public class EmailApprovalController : ApiBaseController
    {

        private readonly IEmployeeLeaveBusiness _employeeLeaveBusiness;
        private readonly IEmployeeLeaveRequestBusiness _employeeLeaveRequestBusiness;
        public EmailApprovalController(IClientDatabase clientDatabase, IEmployeeLeaveBusiness employeeLeaveBusiness, IEmployeeLeaveRequestBusiness employeeLeaveRequestBusiness) : base(clientDatabase)
        {
            _employeeLeaveBusiness = employeeLeaveBusiness;
            _employeeLeaveRequestBusiness = employeeLeaveRequestBusiness;
        }

        [HttpGet, Route("GetEmployeesLeaveApproval")]
        public async Task<IActionResult> GetEmployeesLeaveApproval([FromQuery] EmailApprove_Filter filter)
        {
            var user = AppUser();
            user.Database = "HRMSDEMO";
            user.CompanyId = 3;
            user.OrganizationId = 5;
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var allData = await _employeeLeaveBusiness.GetEmployeesLeaveApprovalAsync(filter, user);

                    var data = PagedList<object>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);

                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("LeaveRequestApproval")]
        public async Task<IActionResult> LeaveRequestApprovalAsync(EmployeeLeaveRequestStatusDTO model)
        {
            var user = AppUser();
            user.Database = "HRMSDEMO";
            user.CompanyId = 3;
            user.OrganizationId = 5;
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
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
            return BadRequest(ResponseMessage.InvalidParameters);

        }

        [HttpPost, Route("SendLeaveEmail")]
        public async Task<IActionResult> SendLeaveEmailAsync(ExecutionStatus status)
        {
            var user = AppUser();
            user.Database = "HRMSDEMO";
            user.CompanyId = 3;
            user.OrganizationId = 5;
            if (user.HasBoth)
            {
                var data = await _employeeLeaveRequestBusiness.SendLeaveEmailAsync(status, user);
                return Ok(data);
            }
            return BadRequest(ResponseMessage.InvalidParameters);
        }
    }
}
