using API.Base;
using API.Services;
using BLL.Base.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BLL.Employee.Interface.Miscellaneous;
using DAL.DapperObject.Interface;

namespace API.Areas.Employee.Miscellaneous
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee/[controller]"), Authorize]
    public class DataLabelController : ApiBaseController
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDataLabelBusiness _dataLabelBusiness;

        public DataLabelController(IDapperData dapper, IDataLabelBusiness dataLabelBusiness, ISysLogger sysLogger, IClientDatabase clientDatabase) : base(clientDatabase)
        {   
            _sysLogger = sysLogger;
            _dataLabelBusiness = dataLabelBusiness;
        }

        [HttpGet,Route("GetDataByLabel")]
        public async Task<IActionResult> GetDataByLabelAsyncAsync(string label)
        {
            var user = AppUser();
            try {
                var data = await _dataLabelBusiness.GetDataByLabelAsync(label, user);
                if(data != null) {
                    return Ok(data);
                }
                else {
                    return NotFound("No data found");
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterController", "GetCostCentersAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
