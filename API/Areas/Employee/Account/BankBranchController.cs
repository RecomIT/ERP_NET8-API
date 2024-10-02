using API.Base;
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using BLL.Employee.Interface.Account;
using Microsoft.AspNetCore.Authorization;
using Shared.Employee.Filter.Account;
using Shared.Employee.DTO.Account;


namespace API.Areas.Employee.Account
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class BankBranchController : ApiBaseController
    {
        private readonly IBankBranchBusiness _bankBranchBusiness;
        private readonly ISysLogger _sysLogger;

        public BankBranchController(
            IBankBranchBusiness bankBranchBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _bankBranchBusiness = bankBranchBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetBankBranches")]
        public async Task<IActionResult> GetBankBranchesAsync([FromQuery]BankBranch_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _bankBranchBusiness.GetBankBranchesAsync(filter, user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchController", "GetBankBranchesAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveBankBranch")]
        public async Task<IActionResult> SaveBankBranchAsync(BankBranchDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _bankBranchBusiness.ValidatorBankBranchAsync(model, user);
                    if (validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var data = await _bankBranchBusiness.SaveBankBranchAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchController", "SaveBankBranchAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBankBranchById")]
        public async Task<IActionResult> GetBankBranchByIdAsync([FromQuery] BankBranch_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _bankBranchBusiness.GetBankBranchesAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchController", "GetBankBranchByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBankBranchDropdown")]
        public async Task<IActionResult> GetBankDropdownIdAsync([FromQuery]BankBranch_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _bankBranchBusiness.GetBankBranchDropdownAsync(filter, user));
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankBranchController", "GetBankBranchByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
