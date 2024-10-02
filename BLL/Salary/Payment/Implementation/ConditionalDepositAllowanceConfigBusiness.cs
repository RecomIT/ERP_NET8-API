using Dapper;
using AutoMapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using BLL.Salary.Payment.Interface;
using Shared.OtherModels.DataService;
using DAL.Payroll.Repository.Interface;
using Shared.Payroll.ViewModel.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.DTO.Payment;

namespace BLL.Salary.Payment.Implementation
{
    public class ConditionalDepositAllowanceConfigBusiness : IConditionalDepositAllowanceConfigBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly IConditionalDepositAllowanceConfigRepository _conditionalDepositAllowanceConfigRepository;

        public ConditionalDepositAllowanceConfigBusiness(ISysLogger sysLogger, IDapperData dapper, IMapper mapper, IConditionalDepositAllowanceConfigRepository conditionalDepositAllowanceConfigRepository)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _mapper = mapper;
            _conditionalDepositAllowanceConfigRepository = conditionalDepositAllowanceConfigRepository;
        }

        public async Task<IEnumerable<ConditionalDepositAllowanceConfigViewModel>> GetConditionalDepositAllowanceConfigsAsync(ConditionalDepositAllowanceConfig_Filter filter, AppUser user)
        {
            IEnumerable<ConditionalDepositAllowanceConfigViewModel> list = new List<ConditionalDepositAllowanceConfigViewModel>();
            try
            {
                var query = $@"SELECT
                CDA.Id,
                CDA.Code,
               ServiceLength=(Case When CDA.ServiceLength =0 OR CDA.ServiceLength IS NULL THEN NULL ELSE CAST(CDA.ServiceLength AS NVARCHAR(50)) END)+' ' + ISNULL((Case When CDA.ServiceLengthUnit ='' OR CDA.ServiceLengthUnit IS NULL THEN 'N/A' ELSE CDA.ServiceLengthUnit END),''),
                JobType=(Case When CDA.JobType ='' OR CDA.JobType IS NULL THEN 'N/A' ELSE CDA.JobType END),
                Religion=(Case When CDA.Religion ='' OR CDA.Religion IS NULL THEN 'N/A' ELSE CDA.Religion END),
                MaritalStatus=(Case When CDA.MaritalStatus ='' OR CDA.MaritalStatus IS NULL THEN 'N/A' ELSE CDA.MaritalStatus END),
                Citizen=(Case When CDA.Citizen ='' OR CDA.Citizen IS NULL THEN 'N/A' ELSE CDA.Citizen END),
                Gender=(Case When CDA.Gender ='' OR CDA.Gender IS NULL THEN 'N/A' ELSE CDA.Gender END),
                PhysicalCondition=(Case When CDA.PhysicalCondition ='' OR CDA.PhysicalCondition IS NULL THEN 'N/A' ELSE CDA.PhysicalCondition END),
                IsVisibleInPayslip=ISNULL(CDA.IsVisibleInPayslip,0),
                IsVisibleInSalarySheet=ISNULL(CDA.IsVisibleInSalarySheet,0),
                DepositType=(Case When CDA.DepositType ='' AND CDA.DepositType IS NULL THEN 'N/A' ELSE CDA.DepositType END),
                BaseOfPayment,
                [Percentage]=ISNULL(CDA.[Percentage],0),
                [Amount]=ISNULL(CDA.Amount,0),
                CDA.StateStatus,
                CDA.ActivationFrom,
                CDA.ActivationTo,
                A.Name AS AllowanceName
                From [dbo].[Payroll_ConditionalDepositAllowanceConfig] CDA
                INNER JOIN [dbo].[Payroll_AllowanceName] A ON CDA.AllowanceNameId = A.AllowanceNameId
                Where 1=1
                AND CDA.CompanyId=@CompanyId AND CDA.OrganizationId=@OrganizationId";
                var parameters = new DynamicParameters();
                parameters.Add("@CompanyId", user.CompanyId);
                parameters.Add("@OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<ConditionalDepositAllowanceConfigViewModel>(user.Database, query, parameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigBusiness", "GetConditionalDepositAllowanceConfigsAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetEligibleEmployeesByConfigIdAsync(long configId, AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                list = await _conditionalDepositAllowanceConfigRepository.GetEligibleEmployeesByConfigIdAsync(configId, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigBusiness", "GetEligibleEmployeesByConfigIdAsync", user);

            }
            return list;
        }
        public Task<EmployeeDepositPaymentViewModel> GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(long employeeId, short year, short month, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<ConditionalDepositAllowanceConfigViewModel> GetEmployeeConditionalDepositAllowanceConfigById(long id, AppUser user)
        {
            ConditionalDepositAllowanceConfigViewModel model = new ConditionalDepositAllowanceConfigViewModel();
            try
            {
                var dataInDb = await _conditionalDepositAllowanceConfigRepository.GetByIdAsync(id, user);
                _mapper.Map(dataInDb, model);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigBusiness", "GetEmployeeConditionalDepositAllowanceConfigById", user);
            }
            return model;
        }
        public async Task<IEnumerable<ConditionalDepositAllowanceConfigViewModel>> GetEmployeeConditionalDepositAllowanceConfigsAsync(EligibilityInConditionalDeposit_Filter filter, AppUser user)
        {
            IEnumerable<ConditionalDepositAllowanceConfigViewModel> list = new List<ConditionalDepositAllowanceConfigViewModel>();
            try
            {
                var query = $@"SELECT Config.*,[AllowanceName]=ALW.[Name] FROM Payroll_ConditionalDepositAllowanceConfig Config
INNER JOIN Payroll_AllowanceName ALW ON Config.AllowanceNameId = ALW.AllowanceNameId
WHERE 1=1
AND (Config.Gender IS NULL OR Config.Gender ='' OR Config.Gender=@Gender)
AND (Config.JobType IS NULL OR Config.JobType='' OR Config.JobType=@JobType)
AND (Config.Religion IS NULL OR Config.Religion ='' OR Config.Religion=@Religion)
AND (Config.MaritalStatus IS NULL OR Config.MaritalStatus='' OR Config.MaritalStatus=@MaritalStatus)
AND (Config.PhysicalCondition IS NULL OR Config.PhysicalCondition='' OR Config.PhysicalCondition=@PhysicalCondition)
AND (@SalaryDate >= Config.ActivationFrom AND (Config.ActivationTo IS NULL OR @SalaryDate BETWEEN Config.ActivationFrom AND Config.ActivationTo))
AND (Config.StateStatus IN('Approved'))
AND (Config.CompanyId=@CompanyId)
AND (Config.OrganizationId=@OrganizationId)";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<ConditionalDepositAllowanceConfigViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigBusiness", "GetEmployeeConditionalDepositAllowanceConfigsAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<EmployeeDepositPaymentViewModel>> GetEmployeeEligibleDepositPaymentAsync(EmployeeEligibleDepositPayment_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeDepositPaymentViewModel> list = null;
            try
            {
                list = await _conditionalDepositAllowanceConfigRepository.GetEmployeeEligibleDepositPaymentAsync(filter, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigBusiness", "GetEmployeeEligibleDepositPaymentAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveAsync(ConditionalDepositAllowanceConfigDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _conditionalDepositAllowanceConfigRepository.SaveAsync(model, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigBusiness", "GetEmployeeConditionalDepositAllowanceConfigsAsync", user);
            }
            return executionStatus;
        }
    }
}
