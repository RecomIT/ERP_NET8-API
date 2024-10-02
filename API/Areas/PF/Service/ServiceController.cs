using API.Base;
using API.Services;
using BLL.Base.Interface;
using BLL.PF.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.OtherModels.DataService;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Areas.PF.Service
{
    [ApiController, Area("Fund"), Route("api/[area]/PF/[controller]"), Authorize]
    public class ServiceController : ApiBaseController
    {
        private IPFServiceBusiness _pFServiceBusiness;
        private readonly ISysLogger _sysLogger;

        public ServiceController(ISysLogger sysLogger, IPFServiceBusiness pFServiceBusiness, IClientDatabase clientDatabase) : base(clientDatabase)
        {
            _sysLogger = sysLogger;
            _pFServiceBusiness = pFServiceBusiness;
        }


        [HttpGet, Route("GetPFEmployees")]
        public async Task<ActionResult<IEnumerable<Select2Dropdown>>> GetPFEmployeesAsync(long? notEmployee, long? designationId, long? departmentId, long? sectionId, long? subsectionId)
        {
            var user = AppUser();
            try
            {
                if (user.CompanyId > 0 && user.OrganizationId > 0)
                {
                    var data = await _pFServiceBusiness.GetPFEmployeesAsync(notEmployee ?? 0, designationId ?? 0, departmentId ?? 0, sectionId ?? 0, subsectionId ?? 0, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ServiceController", "GetPFEmployees", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

    }
}
