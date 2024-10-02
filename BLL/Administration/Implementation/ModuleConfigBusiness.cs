using Dapper;
using AutoMapper;
using System.Data;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using Shared.Control_Panel.ViewModels;
using BLL.Administration.Interface;

namespace BLL.Administration.Implementation
{
    public class ModuleConfigBusiness : IModuleConfigBusiness
    {
        private string sqlQuery = null;
        private IDapperData _dapperData;
        private IMapper _mapper;
        private readonly ISysLogger _sysLogger;

        public ModuleConfigBusiness(IDapperData dapperData, IMapper mapper, ISysLogger sysLogger)
        {
            _dapperData = dapperData;
            _mapper = mapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<HRModuleConfigViewModel>> GetHRModuleConfigsAsync(long companyId, long organizationId)
        {
            IEnumerable<HRModuleConfigViewModel> data = new List<HRModuleConfigViewModel>();
            try
            {

                sqlQuery = string.Format(@"sp_HRModuleConfig");
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", companyId);
                parameters.Add("@OrganizationId", organizationId);
                parameters.Add("@ExecutionFlag", Data.Read);

                data = await _dapperData.SqlQueryListAsync<HRModuleConfigViewModel>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ModuleConfigBusiness", "GetHRModuleConfigs", string.Empty, organizationId, companyId, 0);
            }
            return data;
        }
        public async Task<IEnumerable<PayrollModuleConfigViewModel>> GetPayrollModuleConfigsAsync(long companyId, long organizationId)
        {
            IEnumerable<PayrollModuleConfigViewModel> data = new List<PayrollModuleConfigViewModel>();
            try
            {

                sqlQuery = string.Format(@"sp_PayrollModuleConfig");
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", companyId);
                parameters.Add("@OrganizationId", organizationId);
                parameters.Add("@ExecutionFlag", Data.Read);

                data = await _dapperData.SqlQueryListAsync<PayrollModuleConfigViewModel>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ModuleConfigBusiness", "GetPayrollModuleConfigs", string.Empty, organizationId, companyId, 0);
            }
            return data;
        }
        public async Task<IEnumerable<PFModuleConfigViewModel>> GetPFModuleConfigsAsync(long companyId, long organizationId)
        {
            IEnumerable<PFModuleConfigViewModel> data = new List<PFModuleConfigViewModel>();
            try
            {

                sqlQuery = string.Format(@"sp_PFModuleConfig");
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", companyId);
                parameters.Add("@OrganizationId", organizationId);
                parameters.Add("@ExecutionFlag", Data.Read);

                data = await _dapperData.SqlQueryListAsync<PFModuleConfigViewModel>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ModuleConfigBusiness", "GetPFModuleConfigs", string.Empty, organizationId, companyId, 0);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveHRModuleConfigsAsync(HRModuleConfigViewModel model)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_HRModuleConfig");
                var parameters = new DynamicParameters();
                parameters.Add("ApplicationId", model.ApplicationId);
                parameters.Add("ModuleId", model.ModuleId);
                parameters.Add("MainmenuId", model.MainmenuId);

                parameters.Add("EnableMaxLateWarning", model.EnableMaxLateWarning ?? false);
                parameters.Add("MaxLateInMonth", model.MaxLateInMonth ?? 0);
                parameters.Add("EnableSequenceLateWarning", model.EnableSequenceLateWarning ?? false);
                parameters.Add("SequenceLateInMonth", model.SequenceLateInMonth ?? 0);

                parameters.Add("CompanyId", model.CompanyId);
                parameters.Add("OrganizationId", model.OrganizationId);
                parameters.Add("UserId", model.UserId);

                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ModuleConfigBusiness", "SaveHRModuleConfigsAsync", string.Empty, model.OrganizationId, model.CompanyId, 0);
                executionStatus = Utility.Invalid();
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> SavePayrollModuleConfigsAsync(PayrollModuleConfigViewModel model)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_PayrollModuleConfig");
                var parameters = new DynamicParameters();
                parameters.Add("ApplicationId", model.ApplicationId);

                parameters.Add("ModuleId", model.ModuleId);

                parameters.Add("MainmenuId", model.MainmenuId);

                parameters.Add("CompanyId", model.CompanyId);

                parameters.Add("OrganizationId", model.OrganizationId);

                parameters.Add("ConsiderationForMonth", model.WhatDoesConsiderationForMonth);

                parameters.Add("IsPFActive", model.IsProvidentFundactivated ?? false);

                parameters.Add("PercentageOfBasicForProvidentFund", model.PercentageOfBasicForProvidentFund ?? "0");

                parameters.Add("PercentageOfActualCalculatedTaxForMonthlyDeduction", model.PercentageOfActualCalculatedTaxForMonthlyDeduction ?? "0");

                parameters.Add("IsOnceOffTaxAvailable", model.IsOnceOffTaxAvailable ?? false);

                parameters.Add("WhenOnceOffTaxCutDown", model.WhenDoesOnceOffTaxCutDown);

                parameters.Add("IsNonResidentTaxApplied", model.IsNonResidentTaxApplied);

                parameters.Add("IsFestivalBonusDisbursedbasedonReligion", model.IsFestivalBonusDisbursedbasedonReligion ?? false);

                parameters.Add("DiscontinuedEmployeesLastMonthPaymentProcess", model.DiscontinuedEmployeesLastMonthPaymentProcess ?? "");
                parameters.Add("UserId", model.UserId);
                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ModuleConfigBusiness", "SavePayrollModuleConfigsAsync", string.Empty, model.OrganizationId, model.CompanyId, 0);
                executionStatus = Utility.Invalid();
            }
            return executionStatus;
        }

        public async Task<ExecutionStatus> SavePFModuleConfigsAsync(PFModuleConfigViewModel model)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                sqlQuery = string.Format(@"sp_PFModuleConfig");
                var parameters = new DynamicParameters();
                parameters.Add("ApplicationId", model.ApplicationId);
                parameters.Add("ModuleId", model.ModuleId);
                parameters.Add("MainmenuId", model.MainmenuId);
                parameters.Add("UserId", model.UserId);
                parameters.Add("CompanyId", model.CompanyId);
                parameters.Add("OrganizationId", model.OrganizationId);

                parameters.Add("CalculateByJoiningDate", model.CalculateByJoiningDate ?? false);
                parameters.Add("CashFlow", model.CashFlow ?? false);
                parameters.Add("Subsidiary", model.Subsidiary ?? false);
                parameters.Add("OnlyEmployeePartLoan", model.OnlyEmployeePartLoan ?? false);
                parameters.Add("IsIslamic", model.IsIslamic ?? false);
                parameters.Add("MonthWiseIntrument", model.MonthWiseIntrument ?? false);
                parameters.Add("PendingContribution", model.PendingContribution ?? false);
                parameters.Add("GenerateAmortization", model.GenerateAmortization ?? false);
                parameters.Add("LoanPaidandAmortization", model.LoanPaidandAmortization ?? false);

                parameters.Add("ReceivePaymentReport", model.ReceivePaymentReport ?? false);
                parameters.Add("ContributionFromPayroll", model.ContributionFromPayroll ?? false);

                parameters.Add("InstrumentAccruedProcess", model.InstrumentAccruedProcess ?? false);

                parameters.Add("Forfeiture", model.Forfeiture ?? false);
                parameters.Add("Monthlyprofit", model.Monthlyprofit ?? false);
                parameters.Add("Chequeue", model.Chequeue ?? false);

                parameters.Add("ExecutionFlag", Data.Insert);

                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(Database.ControlPanel, sqlQuery, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {

                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "ModuleConfigBusiness", "SavePFModuleConfigsAsync", string.Empty, model.OrganizationId, model.CompanyId, 0);
                executionStatus = Utility.Invalid();
            }
            return executionStatus;
        }
    }
}
