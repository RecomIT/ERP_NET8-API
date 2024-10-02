
using API.Services;
using BLL.Base.Interface;
using DAL.DapperObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Services;
using System.Threading.Tasks;
using System;
using BLL.Asset.Interface.Dashboard;
using API.Base;
using DAL.DapperObject.Interface;

namespace API.Asset.Dashboard
{
    [ApiController, Area("Asset"), Route("api/[area]/Dashboard/[controller]"), Authorize]
    public class EmployeeController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IEmployeeBusiness _employeeBusiness;

        public EmployeeController(
           ISysLogger sysLogger,
           IEmployeeBusiness employeeBusiness,
           IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _employeeBusiness = employeeBusiness;            
        }

        [HttpGet, Route("GetEmployeeAssetList")]
        public async Task<IActionResult> GetAssetAsync()
        {
            var user = AppUser();
            try {
                if (user.HasBoth) {
                    var data = await _employeeBusiness.GetAssetAsync(user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeController", "GetAssetAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }

        }

    }
}
