using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using DAL.DapperObject.Interface;
using API.Base;

namespace RcmApi.Areas.WebServices.Controllers
{
    //[Authorize]
    [ApiController, Area("WS"), Route("api/[area]/[controller]"), Authorize]
    public class HRServiceController : ApiBaseController
    {
        private readonly IMapper _mapper;
        public HRServiceController(IMapper mapper,IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _mapper = mapper;
        }

        //[HttpGet, Route("GetDivisionWithCountry")]
        //public async Task<IActionResult> GetDivisionWithCountryAsync(string DivisionName)
        //{
        //    var appUser = AppUser();
        //    try {
        //        var data = await _locationalBusiness.DivisionExtension(DivisionName, appUser, "DivisionWithCountry");
        //        return Ok(data);
        //    }
        //    catch (Exception) {

        //        return Ok(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetDistrictsWithDivisionAndCountry")]
        //public async Task<IActionResult> GetDistrictsWithDivisionAndCountryAsync(string DistrictName)
        //{
        //    var appUser = AppUser();
        //    try {
        //        var data = await _locationalBusiness.DistrictExtension(DistrictName, appUser, "DistrictWithDivisionAndCountry");
        //        return Ok(data);
        //    }
        //    catch (Exception ex) {
        //        return Ok(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetSubsectionWithSectionDivisionAndZone")]
        //public async Task<IActionResult> GetSubsectionWithSectionDivisionAndZoneAsync(string subSectionName)
        //{
        //    var appUser = AppUser();
        //    try {
        //        var data = await _organizationalBusiness.SubsectionExtension(subSectionName, appUser, "SubsectionWithSectionZoneAndDepartment");
        //        return Ok(data);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        // Police-Station
        //[HttpGet, Route("GetPolicaStationExtenstion")]
        //public async Task<IActionResult> GetPolicaStationExtenstionAsync(string flag, long OrgId)
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.HasBoth) {
        //            return Ok(await _locationalBusiness.PoliceStationExtension(flag.Trim(), appUser));
        //        }
        //    }
        //    catch (Exception ex) {

        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //    return BadRequest(ResponseMessage.InvalidParameters);
        //}

        //[HttpGet, Route("GetGradesExtension")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetGradesExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.HasBoth) {
        //            return Ok(await _organizationalBusiness.GetGradesExtensionAsync(appUser));
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {

        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetDesignationExtension")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetDesignationExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _organizationalBusiness.GetDesignationExtensionAsync(appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest("Invalid Parameters");
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetDepartmentExtension")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetDepartmentExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _organizationalBusiness.GetDepartmentExtensionAsync(appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest("Invalid Parameters");
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetSectionExtension")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetSectionExtensionAsync(int? departmentId)
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _organizationalBusiness.GetSectionExtensionAsync(departmentId ?? 0, appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest("Invalid Parameters");
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetSubsectionExtension")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetSubsectionExtensionAsync(int? sectionId)
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _organizationalBusiness.GetSubsectionExtensionAsync(sectionId ?? 0, appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest("Invalid Parameters");
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetEmployeeExtension")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetEmployeeExtensionAsync(long? notEmployee, long? designationId, long? departmentId, long? sectionId, long? subsectionId)
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _employeeBusiness.GetEmployeeExtensionAsync(notEmployee ?? 0, designationId ?? 0, departmentId ?? 0, sectionId ?? 0, subsectionId ?? 0, appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetEmployeeExtensionOne")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetEmployeeExtensionOneAsync(long? employeeId, string employeeIds, long? notEmployee, long? designationId, long? departmentId, long? sectionId, long? subsectionId)
        //{
        //    var user = AppUser();
        //    try {
        //        if (user.CompanyId > 0 && user.OrganizationId > 0) {
        //            var data = await _employeeBusiness.GetEmployeeExtensionAsync(employeeId ?? 0, employeeIds ?? "", notEmployee ?? 0, designationId ?? 0, departmentId ?? 0, sectionId ?? 0, subsectionId ?? 0, user, "Extension_1");
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetProbationaryEmployees")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetProbationaryEmployeesAsync()
        //{
        //    var user = AppUser();
        //    try {
        //        if (user.HasBoth) {
        //            var data = await _employeeBusiness.GetProbationaryEmployeesAsync(user);
        //            return Ok(data);
        //        }
        //        return BadRequest(new { message = ResponseMessage.InvalidParameters });
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(new { message = ResponseMessage.ServerResponsedWithError });
        //    }
        //}

        //[HttpGet, Route("GetBankBranchesExtension")]
        //public async Task<IActionResult> GetBankBranchesExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        var data = await _miscellaneousBusiness.GetBankBranchesExtensionAsync(appUser);
        //        return Ok(data);
        //    }
        //    catch (Exception ex) {

        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetBankBranchesExtensionWithBank")]
        //public async Task<IActionResult> GetBankBranchesExtensionWithBankAsync(long bankId)
        //{
        //    var appUser = AppUser();
        //    try {
        //        var data = await _miscellaneousBusiness.GetBankBranchesExtensionAsync(bankId, appUser);
        //        return Ok(data);
        //    }
        //    catch (Exception ex) {

        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetBanksExtension")]
        //public async Task<IActionResult> GetBanksExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        var data = await _miscellaneousBusiness.GetBanksExtensionAsync(appUser);
        //        return Ok(data);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetWorkShiftExtension")]
        //public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetWorkShiftExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _workShiftBusiness.GetWorkShiftExtensionAsync(appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest("Invalid Parameters");
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetLeaveTypesExtension")]
        //public async Task<IActionResult> GetLeaveTypesExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _organizationalBusiness.GetLeaveTypesExtensionAsync(appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetEmployeeLeaveBalancesExtension")]
        //public async Task<IActionResult> GetEmployeeLeaveBalancesExtensionAsync(long employeeId)
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (employeeId > 0 && appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _employeeLeaveBusiness.GetEmployeeLeaveBalancesExtensionAsync(employeeId, appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetDesignationExtension2")]
        //public async Task<IActionResult> GetDesignationExtension2Async()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _organizationalBusiness.GetDesignationExtension2Async(appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetJobTypesExtension")]
        //public async Task<IActionResult> GetJobTypesExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.OrganizationId > 0) {
        //            var data = await _miscellaneousBusiness.GetJobTypesExtensionAsync(appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetUnitExtension")]
        //public async Task<IActionResult> GetUnitExtensionAsync(long? subsectionId)
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.BranchId > 0 && appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _organizationalBusiness.GetUnitExtensionAsync(subsectionId ?? 0, appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetLinesExtension")]
        //public async Task<IActionResult> GetLinesExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.CompanyId > 0 && appUser.OrganizationId > 0) {
        //            var data = await _organizationalBusiness.GetLinesExtensionAsync(appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetReligionExtension")]
        //public async Task<IActionResult> GetReligionExtensionAsync()
        //{
        //    var appUser = AppUser();
        //    try {
        //        if (appUser.OrganizationId > 0) {
        //            var data = await _miscellaneousBusiness.GetReligionsExtension(appUser);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return BadRequest(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetEducationLevels")]
        //public async Task<IActionResult> GetEducationLevelsAsync()
        //{
        //    var user = AppUser();
        //    try {
        //        if (user.OrganizationId > 0) {
        //            var data = await _miscellaneousBusiness.GetLevelOfEducations(0, string.Empty, user);
        //            return Ok(data);
        //        }
        //        return Ok(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return Ok(ResponseMessage.ServerResponsedWithError);
        //    }
        //}

        //[HttpGet, Route("GetEducationDegrees")]
        //public async Task<IActionResult> GetEducationDegreesAsync(int levelOfEducationId)
        //{
        //    var user = AppUser();
        //    try {
        //        if (user.OrganizationId > 0) {
        //            var data = await _miscellaneousBusiness.GetEducationalDegrees(0, string.Empty, levelOfEducationId, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return StatusCode(501, ResponseMessage.ServerResponsedWithError);
        //    }
        //}
        //[HttpGet, Route("GetEmployeeShift")]
        //public async Task<IActionResult> GetEmployeeShiftAsync([FromQuery] EmployeeShift_Filter query)
        //{
        //    var user = AppUser();
        //    try {
        //        if (!Utility.IsNullEmptyOrWhiteSpace(query.Date) && Utility.TryParseInt(query.EmployeeId) > 0) {
        //            var data = await _workShiftBusiness.GetEmployeeShiftAysnc(query, user);
        //            return Ok(data);
        //        }
        //        return BadRequest(ResponseMessage.InvalidParameters);
        //    }
        //    catch (Exception ex) {
        //        return StatusCode(501, ResponseMessage.ServerResponsedWithError);
        //    }
        //}
    }
}
