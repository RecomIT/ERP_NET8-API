using API.Base;
using BLL.Administration.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Control_Panel.Domain;
using Shared.Services;

namespace API.Areas.ControlPanel.Controllers
{
    [ApiController, Area("ControlPanel"), Route("api/[area]/[controller]")]
    public class GoogleAuthenticatorController : ApiBaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILoginManager _loginManager;
        private readonly IUserConfigBusiness _userConfigBusiness;
        private readonly IGoogleAuthenticatorBusiness _googleAuthenticatorBusiness;

        public GoogleAuthenticatorController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILoginManager loginManager,
            IUserConfigBusiness userConfigBusiness,
            IGoogleAuthenticatorBusiness googleAuthenticatorBusiness,
            IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _loginManager = loginManager;
            _userConfigBusiness = userConfigBusiness;
            _googleAuthenticatorBusiness = googleAuthenticatorBusiness;
        }

        [HttpGet, Route("GenerateQRcode")]
        public async Task<IActionResult> GenerateQRcodeAsync([FromQuery]string sendToEmail)
        {
            try {
             
                var user = AppUser();
                if (user.HasBoth) {

                    var obj = await _googleAuthenticatorBusiness.GenerateQRcodeAsync(sendToEmail,user);
                   // var obj = 0;
                    return Ok(obj);
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex) {
                return BadRequest(ResponseMessage.ServerResponsedWithError +ex);
            }
        }

        [HttpPost, Route("TwoFactorAuthenticate")]
        public async Task<IActionResult> TwoFactorAuthenticateAsync([FromHeader]string codeDigit, [FromHeader] bool sendToEmail)
        {
            try {
               
                var user = AppUser();
                if (user.HasBoth) {

                    var status = await _googleAuthenticatorBusiness.TwoFactorAuthenticate(codeDigit, sendToEmail, user);
                    
                    return Ok(status);
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex) {
                return BadRequest(ResponseMessage.ServerResponsedWithError + ex);
            }
        }


    }
}
