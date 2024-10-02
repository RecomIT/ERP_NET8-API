using API.Base;
using API.Services;
using AutoMapper;
using BLL.Base.Interface;
using BLL.Salary.Deduction.Interface;
using DAL.DapperObject.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Helpers;
using Shared.Payroll.Domain.Deduction;
using Shared.Payroll.ViewModel.Deduction;
using Shared.Services;

namespace API.Areas.Salary.Deduction
{
    [ApiController, Area("Payroll"), Route("api/[area]/Salary/[controller]"), Authorize]
    public class DeductionConfigController : ApiBaseController
    {
        private readonly IDeductionConfigBusiness _deductionConfigBusiness;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        public DeductionConfigController(IMapper mapper, IDeductionConfigBusiness deductionConfigBusiness, IClientDatabase clientDatabase, ISysLogger sysLogger) : base(clientDatabase)
        {
            _deductionConfigBusiness = deductionConfigBusiness;
            _mapper = mapper;
            _sysLogger = sysLogger;
        }

        [HttpPost("SaveDeductionConfig")]
        public async Task<IActionResult> SaveDeductionConfigAsync(DeductionConfigurationViewModel deduction)
        {
            var user = AppUser();
            try
            {
                if (ModelState.IsValid && user.HasBoth)
                {
                    var data = await _deductionConfigBusiness.SaveDeductionConfigAsync(_mapper.Map<DeductionConfiguration>(deduction), user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionConfigController", "SaveDeductionConfigAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetDeductionConfigurations")]
        public async Task<IActionResult> GetAllownaceConfigurationsAsync(long? deductionNameId, string status, string activationDateFrom, string activationDateTo, string deactivationDateFrom, string deactivationDateTo, long? branchId, long companyId, long organizationId, int pageNumber = 1, int pageSize = 15)
        {
            var user = AppUser();
            try
            {
                if (companyId > 0 && organizationId > 0)
                {
                    pageNumber = Utility.PageNumber(pageNumber);
                    pageSize = Utility.PageSize(pageSize);
                    var allData = await _deductionConfigBusiness.GetDeductionConfigurationsAsync(deductionNameId ?? 0, status, activationDateFrom, activationDateTo, deactivationDateFrom, deactivationDateTo, user);
                    var data = PagedList<DeductionConfigurationViewModel>.ToPagedList(allData, pageNumber, pageSize);
                    Response.AddPagination(data.CurrentPage, data.PageSize, data.TotalCount, data.TotalPages);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionConfigController", "GetAllownaceConfigurationsAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpGet("GetDeductionConfiguration")]
        public async Task<IActionResult> GetAllownaceConfigurationAsync(long configId, long deductionNameId)
        {
            var user = AppUser();
            try
            {
                if (user.HasBoth)
                {
                    var data = await _deductionConfigBusiness.GetDeductionConfigurationAsync(configId, deductionNameId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionConfigController", "GetAllownaceConfigurationAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }

        [HttpPost("SaveDeductionConfigurationStatus")]
        public async Task<IActionResult> SaveDeductionConfigurationStatusAsync(string status, string remarks, long configId, long deductionNameId)
        {
            var user = AppUser();
            try
            {
                if (configId > 0
                    && !Utility.IsNullEmptyOrWhiteSpace(user.UserId)
                    && Utility.StatusChecking(status, new string[] { "Approved", "Recheck", "Cancelled" })
                    && user.HasBoth && deductionNameId > 0)
                {
                    var data = await _deductionConfigBusiness.SaveDeductionConfigurationStatusAsync(status, remarks, configId, deductionNameId, user);
                    return Ok(data);
                }
                return BadRequest(ResponseMessage.InvalidParameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DeductionConfigController", "SaveDeductionConfigurationStatusAsync", user);
                return BadRequest(WebEnvironmentHelper.ErrorMessage(ex));
            }
        }
    }
}
