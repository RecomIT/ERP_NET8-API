
using API.Base;
using Shared.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;
using Shared.Employee.ViewModel.Setup;

namespace API.Areas.Employee.Info
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class EmailSendingConfigurationController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEmailSendingConfigurationBusiness _emailSendingConfigurationBusiness;
        public EmailSendingConfigurationController(ISysLogger sysLogger, IEmailSendingConfigurationBusiness emailSendingConfigurationBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _emailSendingConfigurationBusiness = emailSendingConfigurationBusiness;
        }

        [HttpPost, Route("SaveEmailSendingConfiguration")]
        public async Task<IActionResult> SaveEmailSendingConfigurationAsync(EmailSendingConfigurationViewModel model)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var validator = await _emailSendingConfigurationBusiness.EmailSendingConfigurationValidatorAsync(model, user);
                    if (validator != null && !validator.Status) {
                        return Ok(validator);
                    }
                    model.CreatedBy = model.UserId;
                    var data = await _emailSendingConfigurationBusiness.SaveEmailSendingConfigurationAsync(model, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmailSendingConfiguration", "SaveEmailSendingConfiguration", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("GetEmailSendingConfiguration")]
        public async Task<IActionResult> GetEmailSendingConfigurationAsync(int Id)
        {
            var user = AppUser();
            try {
                if (user.CompanyId > 0 && user.OrganizationId > 0) {
                    var data = await _emailSendingConfigurationBusiness.GetEmailSendingConfigurationAsync(Id, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmailSendingConfiguration", "GetEmailSendingConfiguration", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("LoadModuleName")]
        public async Task<IActionResult> LoadModuleNameAsync()
        {
            var user = AppUser();
            try {
                if (user.OrganizationId > 0) {
                    var data = await _emailSendingConfigurationBusiness.LoadModuleNameAsync(user);
                    return Ok(data);
                }
                return Ok(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmailSendingConfiguration", "LoadModuleName", user);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }
    }
}
