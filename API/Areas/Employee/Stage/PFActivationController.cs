using API.Base;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Shared.Employee.DTO.Info;
using Shared.Employee.DTO.Stage;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Info;
using BLL.Employee.Interface.Stage;
using Shared.Employee.Filter.Stage;
using Shared.Employee.ViewModel.Stage;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Employee.Stage
{

    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class PFActivationController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEmployeePFActivationBusiness _employeePFActivationBusiness;
        public PFActivationController(ISysLogger sysLogger, IEmployeePFActivationBusiness employeePFActivationBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employeePFActivationBusiness = employeePFActivationBusiness;
        }

        [HttpGet, Route("GetConfirmedEmployeesToAssignPF")]
        public async Task<IActionResult> GetConfirmedEmployeesToAssignPFAsync([FromQuery] ConfirmedEmployeesToAssignPF_Filter model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _employeePFActivationBusiness.GetConfirmedEmployeesToAssignPFAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PFActivationController", "GetConfirmedEmployeesToAssignPF", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("SavePFActivation")]
        public async Task<IActionResult> SavePFActivationAsync(EmployeePFActivationDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _employeePFActivationBusiness.SavePFActivationAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PFActivationController", "SavePFActivationAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetEmployeePFActivationList")]
        public async Task<IActionResult> GetEmployeePFActivationListAsync([FromQuery] PFActivation_Filter filter, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _employeePFActivationBusiness.GetEmployeePFActivationListAsync(filter, user);
                    var data = PagedList<EmployeePFActivationViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PFActivationController", "GetEmployeePFActivationListAsync", user);
            }
            return BadRequest(ResponseMessage.ServerResponsedWithError);
        }

        [HttpPost, Route("GetPFBasedAmountDropdown")]
        public async Task<IActionResult> GetPFBasedAmountDropdownAsync(string baseAmount)
        {
            var user = AppUser();
            try {
                var list = await _employeePFActivationBusiness.GetPFBasedAmountDropdownAsync(baseAmount, user);
                return Ok(list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PFActivationController", "GetPFBasedAmountDropdownAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("SavePFActivationApproval")]
        public async Task<IActionResult> SavePFActivationApprovalAsync(EmployeePFActivationApprovalDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var data = await _employeePFActivationBusiness.SavePFActivationApprovalAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PFActivationController", "SavePFActivationApprovalAsync", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetEmployeePFActivationById")]
        public async Task<IActionResult> GetEmployeePFActivationByIdAsync(long pfActivationId)
        {
            var user = AppUser();
            try {
                var data = (await _employeePFActivationBusiness.GetEmployeePFActivationListAsync(new PFActivation_Filter() {
                    PFActivationId = pfActivationId.ToString()
                }, user)).FirstOrDefault();
                if (data != null) {
                    return Ok(data);
                }
                return NotFound();
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "PFActivationController", "GetEmployeePFActivationById", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

    }
}
