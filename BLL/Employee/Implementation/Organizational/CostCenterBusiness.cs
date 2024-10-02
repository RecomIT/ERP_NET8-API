using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Organizational;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.ViewModel.Organizational;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;
using Shared.Helpers;
using DAL.Context.Employee;
using Shared.Employee.Domain.Organizational;


namespace BLL.Employee.Implementation.Organizational
{
    public class CostCenterBusiness : ICostCenterBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        public CostCenterBusiness(IDapperData dapper, ISysLogger sysLogger, EmployeeModuleDbContext employeeModuleDbContext)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _employeeModuleDbContext = employeeModuleDbContext;
        }
        public async Task<IEnumerable<Dropdown>> GetCostCenterDropdownAsync(AppUser user)
        {
            List<Dropdown> dropdowns = new List<Dropdown>();
            try
            {
                var list = await GetCostCentersAsync(new CostCenter_Filter() { }, user);
                foreach (var item in list)
                {
                    dropdowns.Add(new Dropdown()
                    {
                        Id = item.CostCenterId,
                        Value = item.CostCenterId.ToString(),
                        Text = item.CostCenterName.ToString(),
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterBusiness", "GetCostCenterDropdownAsync", user);
            }
            return dropdowns;
        }
        public async Task<IEnumerable<CostCenterViewModel>> GetCostCentersAsync(CostCenter_Filter filter, AppUser user)
        {
            IEnumerable<CostCenterViewModel> list = new List<CostCenterViewModel>();
            try
            {
                var sp_name = "sp_HR_CostCenter_List";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<CostCenterViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterBusiness", "GetCostCentersAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveAsync(CostCenterDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                if (model.CostCenterId > 0)
                {
                    var costCenterInDb = _employeeModuleDbContext.HR_Costcenter.FirstOrDefault(i => i.CostCenterId == model.CostCenterId);
                    if (costCenterInDb != null && costCenterInDb.CostCenterId > 0)
                    {
                        costCenterInDb.CostCenterCode = model.CostCenterCode.IsNullEmptyOrWhiteSpace() == false ? model.CostCenterCode : costCenterInDb.CostCenterCode;
                        costCenterInDb.CostCenterName = model.CostCenterName.IsNullEmptyOrWhiteSpace() == false ? model.CostCenterName : costCenterInDb.CostCenterName;
                        costCenterInDb.UpdatedBy = user.ActionUserId;
                        costCenterInDb.UpdatedDate = DateTime.Now;
                        _employeeModuleDbContext.Update(costCenterInDb);
                        if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                        {
                            executionStatus = ResponseMessage.Message(true, ResponseMessage.Updated);
                            executionStatus.ItemId = costCenterInDb.CostCenterId;
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                        }
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.NoDataFound);
                    }
                }
                else
                {
                    CostCenter costCenter = new CostCenter();
                    costCenter.CostCenterCode = model.CostCenterCode;
                    costCenter.CostCenterName = model.CostCenterName;
                    costCenter.CompanyId = user.CompanyId;
                    costCenter.OrganizationId = user.OrganizationId;
                    costCenter.CreatedBy = user.ActionUserId;
                    costCenter.CreatedDate = DateTime.Now;
                    await _employeeModuleDbContext.AddAsync(costCenter);
                    if (await _employeeModuleDbContext.SaveChangesAsync() > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Updated);
                        executionStatus.ItemId = costCenter.CostCenterId;
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterBusiness", "SaveAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveCostCenterAsync(CostCenterDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_CostCenter_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", model.CostCenterId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterBusiness", "SaveCostCenterAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateCostCenterAsync(CostCenterDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_CostCenter_Insert_Update_Delete";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "CostCenterBusiness", "SaveCostCenterAsync", user);
            }
            return executionStatus;
        }
    }
}
