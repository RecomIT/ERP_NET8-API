using API.Base;
using API.Services;
using Shared.Helpers;
using Shared.Services;
using Microsoft.AspNetCore.Mvc;
using DAL.DapperObject.Interface;
using Shared.Employee.DTO.Training;
using BLL.Employee.Interface.Training;
using Shared.Employee.Filter.Training;
using Microsoft.AspNetCore.Authorization;

namespace API.Areas.Separation.Training
{
    [ApiController, Area("HRMS"), Route("api/[area]/Employee_Module/[controller]"), Authorize]
    public class TrainingRequestController : ApiBaseController
    {

        private readonly ITrainingBusiness _trainingBusiness;
        public TrainingRequestController(IClientDatabase clientDatabase, ITrainingBusiness trainingBusiness) : base(clientDatabase)
        {
            _trainingBusiness = trainingBusiness;
        }
        [HttpGet, Route("GetAllTraining")]
        public async Task<IActionResult> GetAllTraining([FromQuery] Training_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _trainingBusiness.GetAllTrainingAsync(filter, user);

                    var data = PagedList<object>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost, Route("SubmitTrainingRequest")]
        public async Task<IActionResult> SubmitTrainingRequest(SubmitTrainingRequestDTO filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var data = await _trainingBusiness.SaveTrainingRequestAsync(filter, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet, Route("GetTrainingRequests")]
        public async Task<IActionResult> GetTrainingRequests([FromQuery] TrainingRequest_Filter filter)
        {
            var user = AppUser();
            try {
                if (ModelState.IsValid && user.HasBoth) {
                    var allData = await _trainingBusiness.GetTrainingRequestsAsync(filter, user);

                    var data = PagedList<object>.ToPagedList(allData, filter.PageNumber, filter.PageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex) {

                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
