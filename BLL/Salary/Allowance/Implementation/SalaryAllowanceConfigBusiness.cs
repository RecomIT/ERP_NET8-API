using Dapper;
using AutoMapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using DAL.Context.Payroll;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Microsoft.EntityFrameworkCore;
using BLL.Salary.Allowance.Interface;
using Shared.Payroll.Filter.Allowance;
using Shared.Payroll.Domain.Allowance;
using Shared.Payroll.DTO.Configuration;
using Shared.Payroll.ViewModel.Configuration;
using Shared.OtherModels.DataService;
using DAL.Context.Employee;
using Shared.Payroll.DTO.Allowance;

namespace BLL.Salary.Allowance.Implementation
{
    public class SalaryAllowanceConfigBusiness : ISalaryAllowanceConfigBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IMapper _mapper;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        public SalaryAllowanceConfigBusiness(IDapperData dapper, PayrollDbContext payrollDbContext, EmployeeModuleDbContext employeeModuleDbContext, IMapper mapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _mapper = mapper;
            _payrollDbContext = payrollDbContext;
            _employeeModuleDbContext = employeeModuleDbContext;
            _sysLogger = sysLogger;
        }
        public async Task<ExecutionStatus> SaveSalaryAllowanceConfigAsync(SalaryAllowanceConfigurationInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalaryAllowanceConfig_Insert_Update";
                var jsonData = JsonReverseConverter.JsonData(model.SalaryAllowanceConfigurationDetails);
                var parameters = DapperParam.AddParams(model, user, new string[] { "SalaryAllowanceConfigurationDetails", "HeadCount", "HeadDetails" });
                parameters.Add("JsonData", jsonData);
                parameters.Add("ExecutionFlag", Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "SaveSalaryAllowanceConfigAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<SalaryAllowanceConfigurationInfoViewModel>> GetSalaryAllowanceConfigurationInfosAsync(SalaryAllowanceConfig_Filter filter, AppUser user)
        {
            IEnumerable<SalaryAllowanceConfigurationInfoViewModel> list = new List<SalaryAllowanceConfigurationInfoViewModel>();
            try
            {
                var query = $@"Select *, 
			(Case 
			When info.ConfigCategory='Employee Wise' 
			Then (Select Count(Distinct EmployeeId) From Payroll_SalaryAllowanceConfigurationDetails 
			Where SalaryAllowanceConfigId=info.SalaryAllowanceConfigId)

			When info.ConfigCategory='All' Then '0'
			When info.ConfigCategory='Grade' Then  (Select Count(Distinct GradeId) From Payroll_SalaryAllowanceConfigurationDetails 
			Where SalaryAllowanceConfigId=info.SalaryAllowanceConfigId)

			When info.ConfigCategory='Designation' Then  (Select Count(Distinct DesignationId) From Payroll_SalaryAllowanceConfigurationDetails 
			Where SalaryAllowanceConfigId=info.SalaryAllowanceConfigId)

			Else '0' End)'HeadCount',
			(Case
				When info.ConfigCategory='Employee Wise' 
				Then(Select STRING_AGG(EmployeeCode+' '+FullName,',') From HR_EmployeeInformation 
				Where EmployeeId 
				IN (Select Distinct EmployeeId From Payroll_SalaryAllowanceConfigurationDetails Where SalaryAllowanceConfigId=info.SalaryAllowanceConfigId))

				When info.ConfigCategory='Grade' 
				Then(Select STRING_AGG(GradeName,',') From HR_Grades 
				Where GradeId IN (Select Distinct GradeId From Payroll_SalaryAllowanceConfigurationDetails Where SalaryAllowanceConfigId=info.SalaryAllowanceConfigId))

				When info.ConfigCategory='Designation' 
				Then(Select STRING_AGG(DesignationName,',') From HR_Designations 
				Where DesignationId 
				IN (Select Distinct DesignationId From Payroll_SalaryAllowanceConfigurationDetails Where SalaryAllowanceConfigId=info.SalaryAllowanceConfigId))

				Else '' End) 'HeadDetails'

			From Payroll_SalaryAllowanceConfigurationInfo info
			Where 1=1
			And (CompanyId = @CompanyId)
			And (OrganizationId = @OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<SalaryAllowanceConfigurationInfoViewModel>(user.Database, query, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "GetSalaryAllowanceConfigurationInfosAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<SalaryAllowanceConfigurationDetailViewModel>> GetSalaryAllowanceConfigurationDetailsAsync(long salaryAllowanceConfigId, AppUser user)
        {
            IEnumerable<SalaryAllowanceConfigurationDetailViewModel> list = new List<SalaryAllowanceConfigurationDetailViewModel>();
            try
            {
                var sp_name = $@"Select Distinct scd.AllowanceBase,an.[Name] 'AllowanceName',an.IsFixed,scd.[Percentage],
	scd.Amount,scd.IsPeriodically,scd.EffectiveFrom,scd.EffectiveTo,scd.MaxAmount,scd.MinAmount
	From Payroll_SalaryAllowanceConfigurationDetails scd
	Inner Join Payroll_AllowanceName an on an.AllowanceNameId = scd.AllowanceNameId
	Inner Join Payroll_AllowanceHead ah on an.AllowanceHeadId = ah.AllowanceHeadId
	Where 1=1
	AND (@SalaryAllowanceConfigInfoId IS NULL OR @SalaryAllowanceConfigInfoId =0 OR scd.SalaryAllowanceConfigId = @SalaryAllowanceConfigInfoId)
	AND (scd.CompanyId=@CompanyId)
	AND (scd.OrganizationId=@OrganizationId)";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryAllowanceConfigInfoId", salaryAllowanceConfigId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<SalaryAllowanceConfigurationDetailViewModel>(user.Database, sp_name, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "GetSalaryAllowanceConfigurationDetailsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveSalaryAllowanceConfigStatusAsync(SalaryAllowanceConfigurationStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_SalaryAllowanceConfigurationInfo";
                var parameters = new DynamicParameters();
                parameters.Add("SalaryAllowanceConfigId", model.SalaryAllowanceConfigId);
                parameters.Add("StateStatus", model.StateStatus);
                parameters.Add("Remarks", model.Remarks);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", "Checking");
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "SaveSalaryAllowanceConfigStatusAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveAsync(SalaryAllowanceConfigurationInfoDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                if (model.SalaryAllowanceConfigId == 0)
                {
                    // Insert
                    var domain = _mapper.Map<SalaryAllowanceConfigurationInfo>(model);
                    domain.JobType = model.SalaryAllowanceConfigurationDetails.First().JobType;
                    domain.IsActive = false;
                    domain.CreatedBy = user.ActionUserId;
                    domain.CreatedDate = DateTime.Now;
                    domain.CompanyId = user.CompanyId;
                    domain.OrganizationId = user.OrganizationId;

                    domain.SalaryAllowanceConfigurationDetails.ToList().ForEach(item =>
                     {
                         item.IsActive = false;
                         item.CreatedBy = user.ActionUserId;
                         item.CreatedDate = DateTime.Now;
                         item.CompanyId = user.CompanyId;
                         item.OrganizationId = user.OrganizationId;
                     });

                    await _payrollDbContext.AddAsync(domain);
                    var rowCount = await _payrollDbContext.SaveChangesAsync();

                    if (rowCount > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Successfull);
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }
                else
                {
                    // Update
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "SaveAsync", user);
                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<SalaryAllowanceConfigurationInfoViewModel>> GetAllAsync(SalaryAllowanceConfig_Filter filter, AppUser user)
        {
            IEnumerable<SalaryAllowanceConfigurationInfoViewModel> list = new List<SalaryAllowanceConfigurationInfoViewModel>();
            try
            {
                var configs = await (from config in _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo
                                     where config.CompanyId == user.CompanyId && config.OrganizationId == user.OrganizationId
                                     select new SalaryAllowanceConfigurationInfoViewModel()
                                     {
                                         SalaryAllowanceConfigId = config.SalaryAllowanceConfigId,
                                         ConfigCategory = config.ConfigCategory,
                                         HeadCount = 0,
                                         StateStatus = config.StateStatus,
                                         IsActive = config.IsActive,
                                         ApprovedDate = config.ApprovedDate,
                                         BaseType = config.BaseType

                                     }).ToListAsync();


                foreach (var config in configs)
                {
                    config.SalaryAllowanceConfigurationDetails = new List<SalaryAllowanceConfigurationDetailViewModel>();
                    if (config.ConfigCategory == "Employee Wise")
                    {
                        var data = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                    where detail.SalaryAllowanceConfigId == config.SalaryAllowanceConfigId
                                    && detail.CompanyId == user.CompanyId && detail.OrganizationId == user.OrganizationId
                                    select detail.EmployeeId
                                            ).Distinct();
                        config.HeadCount = await data.CountAsync();
                        config.SelectedItems = await data.ToArrayAsync();
                    }

                    if (config.ConfigCategory == "Designation")
                    {
                        var data = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                    where detail.SalaryAllowanceConfigId == config.SalaryAllowanceConfigId
                                    && detail.CompanyId == user.CompanyId && detail.OrganizationId == user.OrganizationId
                                    select detail.DesignationId
                                            ).Distinct();

                        config.HeadCount = await data.CountAsync();
                        config.SelectedItems = await data.ToArrayAsync();
                    }

                    if (config.ConfigCategory == "Grade")
                    {
                        var data = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                    where detail.SalaryAllowanceConfigId == config.SalaryAllowanceConfigId
                                    && detail.CompanyId == user.CompanyId && detail.OrganizationId == user.OrganizationId
                                    select detail.GradeId
                                            ).Distinct();

                        config.HeadCount = await data.CountAsync();
                        config.SelectedItems = await data.ToArrayAsync();
                    }

                    if (config.ConfigCategory == "Job Type")
                    {
                        config.HeadCount = 0;
                    }

                    var details = await (
                        from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                        join allowance in _payrollDbContext.Paryroll_AllownaceNames on detail.AllowanceNameId equals allowance.AllowanceNameId
                        where detail.SalaryAllowanceConfigId == config.SalaryAllowanceConfigId
                        && detail.CompanyId == user.CompanyId && detail.OrganizationId == user.OrganizationId
                        select new
                        {
                            detail.AllowanceNameId,
                            detail.AllowanceBase,
                            allowance.Name,
                            detail.Percentage,
                            detail.Amount,
                            detail.MaxAmount,
                            detail.MinAmount,
                            detail.AdditionalAmount,
                            detail.JobType,
                        }
                    ).ToListAsync();

                    foreach (var detail in details)
                    {
                        SalaryAllowanceConfigurationDetailViewModel item = new SalaryAllowanceConfigurationDetailViewModel();
                        item.AllowanceNameId = detail.AllowanceNameId;
                        item.AllowanceName = detail.Name;
                        item.AllowanceBase = detail.AllowanceBase; ;
                        item.Percentage = detail.Percentage; ;
                        item.Amount = detail.Amount; ;
                        item.MaxAmount = detail.MaxAmount; ;
                        item.MinAmount = detail.MinAmount; ;
                        item.AdditionalAmount = detail.AdditionalAmount;
                        config.JobType = detail.JobType;
                        config.SalaryAllowanceConfigurationDetails.Add(item);
                    }

                    if (Utility.IsNullEmptyOrWhiteSpace(config.BaseType))
                    {
                        config.BaseType = details[0].AllowanceBase;
                    }
                }

                list = configs;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "GetAllAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<KeyValue>> GetHeadsInfoAsync(Breakhead_Filter filter, AppUser user)
        {
            IEnumerable<KeyValue> list = new List<KeyValue>();
            try
            {
                if (filter != null)
                {
                    if (filter.ConfigCategory == "Employee Wise")
                    {
                        list = (from emp in _employeeModuleDbContext.HR_EmployeeInformation
                                where filter.Id.Contains(emp.EmployeeId) && emp.CompanyId == user.CompanyId && emp.OrganizationId == user.OrganizationId
                                select new KeyValue
                                {
                                    Key = emp.EmployeeId.ToString(),
                                    Value = emp.FullName + " [" + emp.EmployeeCode + "]",
                                });
                    }
                    else if (filter.ConfigCategory == "Designation")
                    {
                        list = (from desig in _employeeModuleDbContext.HR_Designations
                                where filter.Id.Contains(desig.DesignationId) && desig.CompanyId == user.CompanyId && desig.OrganizationId == user.OrganizationId
                                select new KeyValue
                                {
                                    Key = desig.DesignationId.ToString(),
                                    Value = desig.DesignationName,
                                });
                    }
                    else if (filter.ConfigCategory == "Grade")
                    {
                        list = (from grd in _employeeModuleDbContext.HR_Grades
                                where filter.Id.Contains(grd.GradeId) && grd.CompanyId == user.CompanyId && grd.OrganizationId == user.OrganizationId
                                select new KeyValue
                                {
                                    Key = grd.GradeId.ToString(),
                                    Value = grd.GradeName,
                                });
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "GetHeadsInfo", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> DeletePendingConfigAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var data = await (from info in _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo
                                  where info.SalaryAllowanceConfigId == id && info.CompanyId == user.CompanyId && info.OrganizationId == user.OrganizationId
                                  select info
                ).FirstOrDefaultAsync();

                if (data != null && data.SalaryAllowanceConfigId > 0 && data.StateStatus == StateStatus.Pending)
                {
                    _payrollDbContext.Remove(data);
                    var rowCount = await _payrollDbContext.SaveChangesAsync();

                    if (rowCount > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Deleted);
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "Item not found or status has changed");
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "DeletePendingConfigAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ApprovedPendingConfigAsync(ApprovedPendingSalaryAllowanceConfigDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var itemInDb = await _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo.FirstOrDefaultAsync(i => i.SalaryAllowanceConfigId == model.SalaryAllowanceConfigId && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId);
                if (itemInDb != null && itemInDb.StateStatus == StateStatus.Pending)
                {
                    if (model.ConfigCategory != "All" && model.ConfigCategory != "Job Type")
                    {
                        if (model.ConfigCategory == "Employee Wise")
                        {
                            var details = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                           join info in _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo
                                           on detail.SalaryAllowanceConfigId equals info.SalaryAllowanceConfigId
                                           where
                                           info.ConfigCategory == "Employee Wise"
                                           && info.StateStatus == StateStatus.Approved
                                           && (detail.EmployeeId ?? 0) > 0
                                           && detail.IsActive == true && model.Id.Contains(detail.EmployeeId ?? 0)
                                           select detail);

                            if (details.Any())
                            {
                                await details.ForEachAsync(item =>
                                {
                                    item.IsActive = false;
                                    item.EffectiveTo = DateTime.Now.AddDays(-1);
                                });

                                _payrollDbContext.UpdateRange(details);
                            }

                        }
                        if (model.ConfigCategory == "Designation")
                        {
                            var details = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                           join info in _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo
                                           on detail.SalaryAllowanceConfigId equals info.SalaryAllowanceConfigId
                                           where
                                           info.ConfigCategory == "Designation"
                                           && info.StateStatus == StateStatus.Approved
                                           && (detail.DesignationId ?? 0) > 0
                                           && detail.IsActive == true && model.Id.Contains(detail.DesignationId ?? 0)
                                           select detail);

                            if (details.Any())
                            {
                                await details.ForEachAsync(item =>
                                {
                                    item.IsActive = false;
                                    item.EffectiveTo = DateTime.Now.AddDays(-1);
                                });

                                _payrollDbContext.UpdateRange(details);
                            }

                        }
                        if (model.ConfigCategory == "Grade")
                        {
                            var details = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                           join info in _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo
                                           on detail.SalaryAllowanceConfigId equals info.SalaryAllowanceConfigId
                                           where
                                           info.ConfigCategory == "Grade"
                                           && info.StateStatus == StateStatus.Approved
                                           && (detail.GradeId ?? 0) > 0
                                           && detail.IsActive == true && model.Id.Contains(detail.GradeId ?? 0)
                                           select detail);

                            if (details.Any())
                            {
                                await details.ForEachAsync(item =>
                                {
                                    item.IsActive = false;
                                    item.EffectiveTo = DateTime.Now.AddDays(-1);
                                });

                                _payrollDbContext.UpdateRange(details);
                            }

                        }
                    }
                    else
                    {
                        var previousItem = await _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo.SingleOrDefaultAsync(
                            item => item.ConfigCategory == model.ConfigCategory
                            && item.StateStatus == StateStatus.Approved
                            && (item.ConfigCategory == "Job Type" ? item.JobType == itemInDb.JobType : item.JobType == item.JobType)
                            && item.IsActive == true);

                        if (model.ConfigCategory == "All")
                        {
                            var details = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                           join info in _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo
                                           on detail.SalaryAllowanceConfigId equals info.SalaryAllowanceConfigId
                                           where
                                           info.ConfigCategory == "All"
                                           && info.StateStatus == StateStatus.Approved
                                           && detail.IsActive == true
                                           select detail);

                            if (details.Any())
                            {
                                await details.ForEachAsync(item =>
                                {
                                    item.IsActive = false;
                                    item.EffectiveTo = DateTime.Now.AddDays(-1);
                                });

                                _payrollDbContext.UpdateRange(details);
                            }
                        }

                        else if (model.ConfigCategory == "Job Type")
                        {
                            var details = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                           join info in _payrollDbContext.Payroll_SalaryAllowanceConfigurationInfo
                                           on detail.SalaryAllowanceConfigId equals info.SalaryAllowanceConfigId
                                           where
                                           info.ConfigCategory == "Job Type"
                                           && info.StateStatus == StateStatus.Approved
                                           && detail.IsActive == true
                                           && detail.JobType == model.JobType
                                           select detail);

                            if (details.Any())
                            {
                                await details.ForEachAsync(item =>
                                {
                                    item.IsActive = false;
                                    item.EffectiveTo = DateTime.Now.AddDays(-1);
                                });

                                _payrollDbContext.UpdateRange(details);
                            }
                        }

                        if(previousItem != null)
                        {
                            previousItem.IsActive = false;
                            _payrollDbContext.Update(previousItem);
                        }
                    }

                    var detailsInDb = (from detail in _payrollDbContext.Payroll_SalaryAllowanceConfigurationDetail
                                       where detail.SalaryAllowanceConfigId == itemInDb.SalaryAllowanceConfigId
                                       select detail);

                    if (detailsInDb.Any())
                    {
                        await detailsInDb.ForEachAsync(item =>
                        {
                            item.IsActive = true;
                            item.ApprovedBy = user.ActionUserId;
                            item.ApprovedDate = DateTime.Now;
                            item.EffectiveFrom = DateTime.Now;
                        });

                        _payrollDbContext.UpdateRange(detailsInDb);

                        itemInDb.StateStatus = StateStatus.Approved;
                        itemInDb.IsActive = true;
                        itemInDb.ApprovedBy = user.ActionUserId;
                        itemInDb.ApprovedDate = DateTime.Now;

                        _payrollDbContext.UpdateRange(itemInDb);

                        var rowCount = await _payrollDbContext.SaveChangesAsync();

                        if (rowCount > 0)
                        {
                            executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Deleted);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "SalaryAllowanceConfigBusiness", "ApprovedPendingConfigAsync", user);
            }
            return executionStatus;
        }
    }
}
