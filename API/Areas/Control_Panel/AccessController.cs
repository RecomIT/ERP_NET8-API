using System.Text;
using Shared.Helpers;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Shared.Control_Panel.ViewModels;
using Shared.Control_Panel.Domain;
using BLL.Administration.Interface;

namespace API.Areas.Control_Panel
{
    [ApiController, Area("ControlPanel"), Route("api/[area]/[controller]")]
    public class AccessController : Controller
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private ILoginManager _loginManager;
        private IUserConfigBusiness _userConfigBusiness;
        private ISysLogger _sysLogger;
        public AccessController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, ILoginManager loginManager, IUserConfigBusiness userConfigBusiness, ISysLogger sysLogger)
        {
            signInManager = _signInManager;
            userManager = _userManager;
            _loginManager = loginManager;
            _userConfigBusiness = userConfigBusiness;
            _sysLogger = sysLogger;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] loginViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Username = Decryptor.DecryptStringAES(user.Username);
                    user.Password = Decryptor.DecryptStringAES(user.Password);
                    var signInResult = await signInManager.PasswordSignInAsync(user.Username, user.Password, false, true);
                    if (signInResult.Succeeded)
                    {
                        var userData = await _loginManager.GetAppUserLoggedInfo2Async(user.Username);
                        if (userData != null && userData.UserId.IsNullEmptyOrWhiteSpace() == false)
                        {
                            if (userData.IsActive)
                            {
                                if (userData.TerminationDate.HasValue)
                                {
                                    if (DateTime.Now > userData.TerminationDate.Value)
                                    {
                                        return Unauthorized("Employee has been discontinued");
                                    }
                                }
                                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SymmetricSecurityKey));
                                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                                var userInfoencrypt = Encryptor.EncryptStringAES(Shared.Services.Utility.JsonData(userData));
                                var tokenOptions = new JwtSecurityToken(
                                    issuer: AppSettings.ApiValidIssuer,
                                    audience: AppSettings.ApiValidAudience,
                                    claims: new List<Claim>() {
                                    new Claim("userinfo", userInfoencrypt),
                                    },
                                    expires: DateTime.Now.AddMinutes(120),
                                    signingCredentials: signingCredentials
                                );

                                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                                var userMenu = await _userConfigBusiness.GetAppUserMenusAsync(user.Username);
                                var userMenuInJsonString = Shared.Services.Utility.JsonData(userMenu);
                                var encrypt = Encryptor.EncryptStringAES(userMenuInJsonString);

                                var passObj = new
                                {
                                    userData.IsDefaultPassword,
                                    userData.DefaultCode,
                                    IsPasswordExpired = userData.RemainExpireDays == 0,
                                    userData.RemainExpireDays
                                };

                                var passObjJson = Shared.Services.Utility.JsonData(passObj);
                                var encryptPass = Encryptor.EncryptStringAES(passObjJson);

                                return Ok(new { token = tokenString, encrypt, passObj = encryptPass });

                            }
                            else
                            {
                                return Unauthorized("Inactive User");
                            }
                        }
                        else
                        {
                            return Unauthorized("Invalid Username");
                        }
                    }
                    if (signInResult.IsLockedOut)
                    {
                        return StatusCode(423, "Your account is locked. Please try again after 5 minutes later or contact to the admin");
                    }
                    return Unauthorized("Invalid Username/Password");
                }
                else
                {
                    return BadRequest(ResponseMessage.InvalidForm);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveControlPanelException(ex, Database.ControlPanel, "AccessController", "Login", user.Username, 0, 0, 0);
                return StatusCode(501, "Msg: " + ex.Message + " Inner Msg: " + ex.Message);
            }
        }

        [Authorize, HttpGet, Route("Userprivileges")]
        public async Task<IActionResult> Userprivileges(string username)
        {
            var userMenuwithcomponent = await _userConfigBusiness.GetAppUsermenuWithComponentAsync(username);
            var userMenuInJsonString = Shared.Services.Utility.JsonData(userMenuwithcomponent.AppUserMenus);
            var encrypt = Encryptor.EncryptStringAES(userMenuInJsonString);

            var passInfo = await _loginManager.GetAppUserLoggedInfoAsync(username);
            var passObj = new
            {
                passInfo.IsDefaultPassword,
                passInfo.DefaultCode,
                IsPasswordExpired = passInfo.RemainExpireDays == 0,
                passInfo.RemainExpireDays
            };
            var passObjJson = Shared.Services.Utility.JsonData(passObj);
            var encryptPass = Encryptor.EncryptStringAES(passObjJson);

            return Ok(new { encrypt, passObj = encryptPass });
        }

        [HttpPost, Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordAsync([FromBody] OTPRequestsViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _loginManager.UserForgetPasswordOTPResquestAsync(model);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "AccessController", "ForgetPasswordAsync", "", 0, 0, 0);
                return BadRequest(ResponseMessage.SomthingWentWrong);
            }
        }

        [HttpPost, Route("ForgetPasswordVerification")]
        public async Task<IActionResult> ForgetPasswordVerificationAsync([FromBody] OTPVerificationViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _loginManager.UserForgetPasswordOTPVerificationAsync(model);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "AccessController", "ForgetPasswordAsync", "", 0, 0, 0);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [Authorize, HttpGet, Route("CheckUserprivilege")]
        public async Task<IActionResult> CheckUserprivilegeAsync(string userId, string component, long companyId, long organizationId)
        {
            try
            {
                if (!Shared.Services.Utility.IsNullEmptyOrWhiteSpace(userId) && !Shared.Services.Utility.IsNullEmptyOrWhiteSpace(component) && companyId > 0 && organizationId > 0)
                {
                    Usercomponentprivilege usercomponentprivilege = new Usercomponentprivilege()
                    {
                        UserId = userId,
                        Component = component,
                        CompanyId = companyId,
                        OrganizationId = organizationId
                    };
                    var obj = await _userConfigBusiness.CheckUserprivilegeAsync(usercomponentprivilege);
                    return Ok(obj);
                }
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "AccessController", "CheckUserprivilegeAsync", "", 0, 0, 0);
                return BadRequest(ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpPost, Route("LoginByMobile")]
        public async Task<IActionResult> LoginByMobile([FromBody] loginViewModel user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var signInResult = await signInManager.PasswordSignInAsync(user.Username, user.Password, false, false);
                    if (signInResult.Succeeded)
                    {
                        var userData = await _loginManager.GetAppUserLoggedInfoAsync(user.Username);
                        if (userData.IsActive)
                        {
                            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AppSettings.SymmetricSecurityKey)); // SecurityKey must be 16 character
                            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                            var userInfoJson = Shared.Services.Utility.JsonData(userData);
                            var tokenOptions = new JwtSecurityToken(
                                issuer: AppSettings.ApiValidIssuer,//appsettings.ApiValidIssuer,
                                audience: AppSettings.ApiValidAudience,//appsettings.ApiValidAudience,
                                claims: new List<Claim>() {
                                    new Claim("userinfo",userInfoJson)
                                },
                                expires: DateTime.Now.AddMinutes(360),
                                signingCredentials: signingCredentials
                            );

                            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

                            var userMenu = await _userConfigBusiness.GetAppUserMenusAsync(user.Username); // Null ref exception occured...

                            var passObj = new
                            {
                                userData.IsDefaultPassword,
                                userData.DefaultCode,
                                IsPasswordExpired = userData.RemainExpireDays == 0,
                                userData.RemainExpireDays
                            };
                            return Ok(new { token = tokenString, privilege = userMenu, passObj });
                        }
                        else
                        {
                            return Unauthorized(new { messages = new string[] { "User is Inactive" } });
                        }
                    }
                    return Unauthorized(new { messages = new string[] { "Invalid Username/Password" } });
                }
                return BadRequest(new { message = ResponseMessage.InvalidClient });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "AccessController", "CheckUserprivilegeAsync", "", 0, 0, 0);
                return StatusCode(501, ResponseMessage.ServerResponsedWithError);
            }
        }

        [HttpGet, Route("LoginChecking/{id:long}")]
        public async Task<IActionResult> LoginChecking(long id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IEnumerable<loginViewModel> users = await _loginManager.GetLoginInfosAsync(id);
                    if (users != null && users.Any())
                    {
                        List<string> messages = new List<string>();
                        int successfulCount = 0;
                        foreach (var user in users)
                        {
                            string message = user + " ";
                            try
                            {
                                var signInResult = await signInManager.PasswordSignInAsync(user.Username, user.Password, false, true);
                                if (signInResult.Succeeded)
                                {
                                    var userData = await _loginManager.GetAppUserLoggedInfo2Async(user.Username);
                                    if (userData != null && userData.UserId.IsNullEmptyOrWhiteSpace() == false)
                                    {
                                        successfulCount = successfulCount + 1;
                                    }
                                    else
                                    {
                                        message = message + "Invalid User";
                                        messages.Add(message);
                                    }
                                }
                                else
                                {
                                    message = message + "Unsuccessful login";
                                    messages.Add(message);
                                }
                            }
                            catch (Exception ex)
                            {
                                message = message + " " + ex.Message;
                            }
                        }
                        return Ok(successfulCount);
                    }
                    else
                    {
                        return NotFound("User not found");
                    }
                }
                else
                {
                    return BadRequest(ResponseMessage.InvalidForm);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "AccessController", "Login", null, 0, 0, 0);
                return StatusCode(501, "Msg: " + ex.Message + " Inner Msg: " + ex.Message);
            }
        }
    }
}
