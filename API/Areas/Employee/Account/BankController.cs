
using API.Services;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Base;
using BLL.Employee.Interface.Account;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Account;
using Shared.Employee.DTO.Account;

namespace API.Areas.Employee.Account
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class BankController : ApiBaseController
    {
        private readonly IBankBusiness _bankBusiness;
        private readonly ISysLogger _sysLogger;

        public BankController(
            IBankBusiness bankBusiness,
            ISysLogger sysLogger,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _bankBusiness = bankBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet,Route("GetBanks")]
        public async Task<IActionResult> GetBanksAsync([FromQuery] Bank_Filter filter)
        {
            var user = AppUser();
            try {
                var data_list = await _bankBusiness.GetBanksAsync(filter,user);
                return Ok(data_list);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankController", "GetBanksAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBankById")]
        public async Task<IActionResult> GetBankByIdAsync([FromQuery] Bank_Filter filter)
        {
            var user = AppUser();
            try {
                var item = (await _bankBusiness.GetBanksAsync(filter, user)).FirstOrDefault();
                return Ok(item);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankController", "GetBankByIdAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SaveBank")]
        public async Task<IActionResult> SaveBankAsync(BankDTO model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid) {
                    var validator = await _bankBusiness.ValidatorBankAsync(model,user);
                    if(validator != null && validator.Status == false) {
                        return Ok(validator);
                    }
                    else {
                        var data = await _bankBusiness.SaveBankAsync(model, user);
                        return Ok(data);
                    }
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankController", "SaveBankAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetBankDropdown")]
        public async Task<IActionResult> GetBankDropdownAsync()
        {
            var user = AppUser();
            try {
                if (user != null && user.HasBoth) {
                    var data_list = await _bankBusiness.GetBankDropdownAsync(user);
                    return Ok(data_list);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "BankController", "GetBankDropdown", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
