using API.Base;
using Shared.Helpers;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.WalletPayment;
using BLL.Salary.WalletPayment.Interface;
using Microsoft.AspNetCore.Authorization;
using Shared.Payroll.Filter.WalletPayment;

namespace API.Areas.Salary.WalletPayment
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class WalletPaymentController : ApiBaseController
    {
        private readonly IWalletPaymentBusiness _walletPaymentBusiness;
        private readonly ISysLogger _sysLogger;
        public WalletPaymentController(
            IWalletPaymentBusiness walletPaymentBusiness,
            ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _walletPaymentBusiness = walletPaymentBusiness;
            _sysLogger = sysLogger;
        }

        [HttpGet, Route("GetInternalDesignationExtension")]
        public async Task<IActionResult> GetInternalDesignationExtensionAsync(long? internalDesignationId)
        {
            try
            {
                var appUser = AppUser();
                if (appUser.HasBoth)
                {
                    var data = await _walletPaymentBusiness.GetInternalDesignationExtensionAsync(internalDesignationId, appUser);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost("SaveWalletPaymentConfigurations")]
        public async Task<IActionResult> SaveWalletPaymentConfigurationsAsync(List<WalletPaymentConfigurationDTO> configurationDTOs)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var validator = await _walletPaymentBusiness.ValidateWalletPaymentAsync(configurationDTOs, user);
                    if (validator != null && validator.Status == false)
                    {
                        return Ok(validator);
                    }
                    else
                    {
                        var dbResponse = await _walletPaymentBusiness.SaveWalletPaymentConfigurationsAsync(configurationDTOs, user);
                        return Ok(dbResponse);
                    }
                }
                return BadRequest(new { msg = ResponseMessage.InvalidParameters });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WalletPaymentController", "SaveWalletPaymentConfigurationsAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet("GetWalletPaymentConfigurations")]
        public async Task<IActionResult> GetWalletPaymentConfigurationsAsync([FromQuery] WalletPaymentConfiguration_Filter filter)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid)
                {
                    var list = await _walletPaymentBusiness.GetWalletPaymentConfigurationsAsync(filter, user);
                    Response.AddPagination(list.Pageparam.PageNumber, list.Pageparam.PageSize, list.Pageparam.TotalRows, list.Pageparam.TotalPages);
                    return Ok(list.ListOfObject);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "WalletPaymentController", "SaveWalletPaymentConfigurationsAsync", user);
                return BadRequest(new { msg = ResponseMessage.ServerResponsedWithError });
            }
        }

        [HttpGet, Route("GetWalletPaymentConfigById")]
        public async Task<IActionResult> GetWalletPaymentConfigByIdAsync(long walletConfigId)
        {
            var appUser = AppUser();
            try
            {
                var data = await _walletPaymentBusiness.GetWalletPaymentConfigByIdAsync(walletConfigId, appUser);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return Ok("Serve responed with error");
            }
        }

        [HttpPut, Route("UpdateWalletPaymentConfigurations")]
        public async Task<IActionResult> UpdateWalletPaymentConfigurationsAsync(WalletPaymentConfigurationDTO dTO)
        {
            try
            {
                var appUser = AppUser();
                if (ModelState.IsValid && dTO.WalletConfigId > 0 && appUser.HasBoth)
                {
                    var data = await _walletPaymentBusiness.UpdateWalletPaymentConfigurationsAsync(dTO, appUser);
                    return Ok(data);

                }
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
            catch (Exception ex)
            {

                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

    }
}
