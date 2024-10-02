using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using BLL.Tax.Interface;
using Shared.Payroll.ViewModel.Tax;
using DAL.DapperObject.Interface;

namespace BLL.Tax.Implementation
{
    public class IncomeTaxSlabBusiness : IIncomeTaxSlabBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public IncomeTaxSlabBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<IncomeTaxSlabViewModel>> GetIncomeTaxSlabsAsync(long? IncomeTaxSlabId, string ImpliedCondition, long? FiscalYearId, AppUser user)
        {
            IEnumerable<IncomeTaxSlabViewModel> list = new List<IncomeTaxSlabViewModel>();
            try
            {
                var sp_name = $@"Select slab.*,fiscal.FiscalYearRange,fiscal.AssesmentYear From Payroll_IncomeTaxSlab slab
			Inner Join Payroll_FiscalYear fiscal on slab.FiscalYearId = fiscal.FiscalYearId
			Where 1=1
			AND (@FiscalYearId=0 OR slab.FiscalYearId=@FiscalYearId)
			AND (@ImpliedCondition='' OR slab.ImpliedCondition=@ImpliedCondition)
			AND slab.CompanyId=@CompanyId
			AND slab.OrganizationId=@OrganizationId
			Order By slab.FiscalYearId, slab.ImpliedCondition";
                var parameters = new DynamicParameters();
                parameters.Add("IncomeTaxSlabId", IncomeTaxSlabId ?? 0);
                parameters.Add("ImpliedCondition", ImpliedCondition ?? "");
                parameters.Add("FiscalYearId", FiscalYearId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);

                list = await _dapper.SqlQueryListAsync<IncomeTaxSlabViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabBusiness", "GetIncomeTaxSlabsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveIncomeTaxSlabAsync(TaxSlabInfo model, AppUser user)
        {
            ExecutionStatus execution = null;
            try
            {
                var json = Utility.JsonData(model.TaxSlabDetails);
                var sp_name = "sp_Payroll_IncomeTaxSlab";
                var parameters = new DynamicParameters();
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("IncomeTaxSlabJson", json);
                parameters.Add("UserId", user.UserId);
                parameters.Add("ExecutionFlag", Data.Insert);

                execution = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabBusiness", "SaveIncomeTaxSlabAsync", user);
            }
            return execution;
        }
        public async Task<ExecutionStatus> ValidateIncomeTaxSlabAsync(TaxSlabInfo model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {

            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabBusiness", "ValidateIncomeTaxSlabAsync", user);
            }
            return executionStatus;
        }
        public Task<ExecutionStatus> ValidateIncomeTaxSlabAsync(IncomeTaxSlabViewModel model, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<TaxSlabData>> GetIncomeTaxSlabsDataAsync(long? IncomeTaxSlabId, string ImpliedCondition, long? FiscalYearId, AppUser user)
        {
            List<TaxSlabData> data = new List<TaxSlabData>();
            try
            {

                IEnumerable<IncomeTaxSlabViewModel> list = new List<IncomeTaxSlabViewModel>();
                var sp_name = "sp_Payroll_IncomeTaxSlab";
                var parameters = new DynamicParameters();
                parameters.Add("IncomeTaxSlabId", IncomeTaxSlabId ?? 0);
                parameters.Add("ImpliedCondition", ImpliedCondition ?? "");
                parameters.Add("FiscalYearId", FiscalYearId ?? 0);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Read);

                list = await _dapper.SqlQueryListAsync<IncomeTaxSlabViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                if (list.Count() > 0)
                {
                    var fiscalYears = list.Select(x => x.FiscalYearId).Distinct();
                    var impliedConditions = list.Select(x => x.ImpliedCondition).Distinct();

                    foreach (var item in fiscalYears)
                    {
                        foreach (var condition in impliedConditions)
                        {
                            var info = list.FirstOrDefault(x => x.FiscalYearId == item && x.ImpliedCondition == condition);
                            if (info != null)
                            {
                                TaxSlabData taxSlabData = new TaxSlabData();
                                taxSlabData.FiscalYearId = item;
                                taxSlabData.FiscalYearRange = info.FiscalYearRange;
                                taxSlabData.AssesmentYear = info.AssesmentYear;
                                taxSlabData.ImpliedCondition = info.ImpliedCondition;

                                taxSlabData.TaxSlabAmounts =
                                    list.Where(x => x.FiscalYearId == item && x.ImpliedCondition == taxSlabData.ImpliedCondition && x.ImpliedCondition == condition)
                                    .Select(i => new TaxSlabAmount()
                                    {
                                        IncomeTaxSlabId = i.IncomeTaxSlabId,
                                        SlabMaximumAmount = i.SlabMaximumAmount,
                                        SlabMininumAmount = i.SlabMininumAmount,
                                        SlabPercentage = i.SlabPercentage
                                    }).ToList();

                                data.Add(taxSlabData);
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabBusiness", "UpdateIncomeTaxSlabAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> UpdateIncomeTaxSlabAsync(TaxSlabUpdate model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_IncomeTaxSlab";
                var parameters = new DynamicParameters();
                parameters.Add("IncomeTaxSlabId", model.IncomeTaxSlabId);
                parameters.Add("SlabMininumAmount", model.SlabMininumAmount);
                parameters.Add("SlabMaximumAmount", model.SlabMaximumAmount);
                parameters.Add("SlabPercentage", model.SlabPercentage);
                parameters.Add("SlabPercentage", model.SlabPercentage);
                parameters.Add("UserId", user.UserId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Update);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabBusiness", "UpdateIncomeTaxSlabAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<IncomeTaxSlabViewModel>> GetIncomeTaxSlabsByImpliedConditionAsync(string ImpliedCondition, long FiscalYearId, AppUser user)
        {
            IEnumerable<IncomeTaxSlabViewModel> list = new List<IncomeTaxSlabViewModel>();
            try
            {
                var query = $@"Select slab.*,fiscal.FiscalYearRange,fiscal.AssesmentYear From Payroll_IncomeTaxSlab slab
			Inner Join Payroll_FiscalYear fiscal on slab.FiscalYearId = fiscal.FiscalYearId
			Where 1=1
			AND (slab.FiscalYearId=@FiscalYearId)
			AND (slab.ImpliedCondition=@ImpliedCondition)
			AND slab.CompanyId=@CompanyId
			AND slab.OrganizationId=@OrganizationId
			Order By slab.FiscalYearId, slab.ImpliedCondition";

                list = await _dapper.SqlQueryListAsync<IncomeTaxSlabViewModel>(user.Database, query, new { ImpliedCondition, FiscalYearId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "IncomeTaxSlabBusiness", "GetIncomeTaxSlabsAsync", user);
            }
            return list;
        }
    }
}
