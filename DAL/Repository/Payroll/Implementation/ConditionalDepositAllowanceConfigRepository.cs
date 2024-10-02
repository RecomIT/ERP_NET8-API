using AutoMapper;
using DAL.DapperObject.Interface;
using DAL.Logger.Interface;
using DAL.Payroll.Repository.Interface;
using Dapper;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Allowance;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.Filter.Payment;
using Shared.Payroll.ViewModel.Payment;
using Shared.Services;

namespace DAL.Payroll.Repository.Implementation
{
    public class ConditionalDepositAllowanceConfigRepository : IConditionalDepositAllowanceConfigRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly IAllowanceNameRepository _allowanceNameRepository;

        public ConditionalDepositAllowanceConfigRepository(IDALSysLogger sysLogger, IDapperData dapper, IMapper mapper, IAllowanceNameRepository allowanceNameRepository)
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
            _allowanceNameRepository = allowanceNameRepository;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<ConditionalDepositAllowanceConfig>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<ConditionalDepositAllowanceConfig>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<ConditionalDepositAllowanceConfig> GetByIdAsync(long id, AppUser user)
        {
            ConditionalDepositAllowanceConfig data = null;
            try {
                var query = $@"SELECT Config.* FROM Payroll_ConditionalDepositAllowanceConfig Config
                INNER JOIN Payroll_AllowanceName AL ON Config.AllowanceNameId = AL.AllowanceNameId
                Where Config.Id = @Id AND Config.CompanyId =@CompanyId AND Config.OrganizationId=@OrganizationId ";
                data = await _dapper.SqlQueryFirstAsync<ConditionalDepositAllowanceConfig>(user.Database, query, new { Id = id, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigRepository", "GetByIdAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetEligibleEmployeesByConfigIdAsync(long id, AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try {
                var config = await this.GetByIdAsync(id, user);
                var query = $@"SELECT 
                [Code]=Info.EmployeeCode,
                [Id]=CAST(Info.EmployeeId AS NVARCHAR(50)),
                [Value]=CAST(Info.EmployeeId AS NVARCHAR(50)),
                [Text]=(Info.FullName+ ' ['+Info.EmployeeCode+']') 
                FROM HR_EmployeeInformation Info
                LEFT JOIN HR_EmployeeDetail Detail On Info.EmployeeId=Detail.EmployeeId
                LEFT JOIN HR_Designations DESIG On Info.DesignationId=DESIG.DesignationId
                Where 1=1
                AND (@JobType IS NULL OR @JobType = '' OR @JobType = 'N/A' OR Info.JobType=@JobType)
                AND (@Gender IS NULL OR @Gender = '' OR @Gender = 'N/A' OR Detail.Gender=@Gender)
                AND (@Religion IS NULL OR @Religion = '' OR @Religion ='N/A' OR Detail.Religion=@Religion)
                AND (@MaritalStatus IS NULL OR @MaritalStatus = '' OR @MaritalStatus = 'N/A' OR Detail.Religion=@MaritalStatus)

                AND (@Citizen IS NULL OR @Citizen = '' OR @Citizen = 'N/A' OR (CASE 
                WHEN Detail.IsResidential IS NULL THEN 'NO' 
                WHEN Detail.IsResidential =1 THEN 'Yes' 
                ELSE 'NO' END)=@Citizen)

                AND (@PhysicalCondition IS NULL OR @PhysicalCondition = '' OR @PhysicalCondition = 'N/A' OR (CASE 
                WHEN Detail.IsPhysicallyDisabled IS NULL THEN 'NO' 
                WHEN Detail.IsPhysicallyDisabled =1 THEN 'Yes' 
                ELSE 'NO' END)=@PhysicalCondition )

                AND (Info.IsActive=1)
                AND Info.CompanyId=@CompanyId
                AND Info.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, new {
                    JobType = config.JobType,
                    Gender = config.Gender,
                    Religion = config.Religion,
                    MaritalStatus = config.MaritalStatus,
                    Citizen = config.Citizen,
                    PhysicalCondition = config.PhysicalCondition,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigRepository", "GetEligibleEmployeesByConfigIdAsync", user);
            }
            return list;
        }
        public async Task<EmployeeDepositPaymentViewModel> GetEmployeeAllowanceAccuredAmountInTaxProcessAsync(long employeeId, long allowanceNameId, short year, short month, long fiscalYearId, AppUser user)
        {
            EmployeeDepositPaymentViewModel data = null;
            try {
                var query = $@"SELECT 
                [TillPaidAmount]=ISNULL((SELECT SUM(ISNULL(DisbursedAmount,0)) FROM Payroll_DepositAllowancePaymentHistory Where AllowanceNameId=@AllowanceNameId AND EmployeeId=@EmployeeId AND FiscalYearId=@FiscalYearId AND IsDisbursed=1 AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),0),
                [ThisMonthPaidAmount]=ISNULL((SELECT SUM(ISNULL(DisbursedAmount,0)) FROM Payroll_DepositAllowancePaymentHistory Where AllowanceNameId=@AllowanceNameId AND EmployeeId=@EmployeeId AND FiscalYearId=@FiscalYearId AND PaymentMonth= @Month AND PaymentYear=@Year),0),
                 TillAccuredAmount= ISNULL((SELECT SUM(ISNULL(Amount,0)) FROM Payroll_DepositAllowanceHistory Where EmployeeId=@EmployeeId AND AllowanceNameId=@AllowanceNameId AND FiscalYearId=@FiscalYearId AND DepositMonth <> @Month),0),
                [ThisMonthAccuredAmount]=ISNULL((SELECT Amount FROM Payroll_DepositAllowanceHistory Where EmployeeId=@EmployeeId AND  AllowanceNameId=@AllowanceNameId AND FiscalYearId=@FiscalYearId AND DepositMonth=@Month AND DepositYear=@Year),0),
                [ThisMonthAccuredArrear]=ISNULL((SELECT Arrear FROM Payroll_DepositAllowanceHistory Where EmployeeId=@EmployeeId AND  AllowanceNameId=@AllowanceNameId AND FiscalYearId=@FiscalYearId AND DepositMonth=@Month AND DepositYear=@Year),0),
                [RemainAmount]=
                (
                ISNULL((SELECT SUM(ISNULL(Amount,0))+SUM(ISNULL(Arrear,0)) FROM Payroll_DepositAllowanceHistory Where EmployeeId=@EmployeeId AND AllowanceNameId=@AllowanceNameId AND FiscalYearId=@FiscalYearId),0)
                -
                ISNULL((SELECT SUM(ISNULL(DisbursedAmount,0)) FROM Payroll_DepositAllowancePaymentHistory Where EmployeeId=@EmployeeId AND AllowanceNameId=@AllowanceNameId AND FiscalYearId=@FiscalYearId),0))";

                data = await _dapper.SqlQueryFirstAsync<EmployeeDepositPaymentViewModel>(user.Database, query, new {
                    AllowanceNameId = allowanceNameId,
                    EmployeeId = employeeId,
                    FiscalYearId = fiscalYearId,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId,
                    Month = month,
                    Year = year
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigRepository", "GetEmployeeAllowanceAccuredAmountInTaxProcessAsync", user);
            }
            return data;
        }

        public async Task<IEnumerable<EmployeeDepositPaymentViewModel>> GetEmployeeEligibleDepositPaymentAsync(EmployeeEligibleDepositPayment_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeDepositPaymentViewModel> list = new List<EmployeeDepositPaymentViewModel>();
            try {

                ConditionalDepositAllowanceConfig config = await this.GetByIdAsync(Utility.TryParseLong(filter.ConfigId), user);
                AllowanceName allowanceName = await _allowanceNameRepository.GetByIdAsync(config.AllowanceNameId, user);

                if (config != null && config.Id > 0) {
                    var query = $@"SELECT *,PayableAmount= (Total-ISNULL(Paid,0)),DisbursedAmount=(Total-ISNULL(Paid,0)),AllowanceNameId FROM (SELECT EMP.EmployeeId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,EMP.DesignationName,
                        DEPOSIT.Total,
                        [Paid]=ISNULL((SELECT SUM(ISNULL(DisbursedAmount,0)) FROM Payroll_DepositAllowancePaymentHistory Where ConditionalDepositAllowanceConfigId= 2 AND EmployeeId=EMP.EmployeeId),0),
                        AllowanceNameId=@AllowanceNameId
                        FROM (SELECT Info.*,DESIG.DesignationName FROM HR_EmployeeInformation Info
                        LEFT JOIN HR_EmployeeDetail Detail On Info.EmployeeId=Detail.EmployeeId
                        LEFT JOIN HR_Designations DESIG On Info.DesignationId=DESIG.DesignationId
                        Where 1=1
                        AND (@JobType IS NULL OR @JobType = '' OR @JobType = 'N/A' OR Info.JobType=@JobType)
                        AND (@Gender IS NULL OR @Gender = '' OR @Gender = 'N/A' OR Detail.Gender=@Gender)
                        AND (@Religion IS NULL OR @Religion = '' OR @Religion ='N/A' OR Detail.Religion=@Religion)
                        AND (@MaritalStatus IS NULL OR @MaritalStatus = '' OR @MaritalStatus = 'N/A' OR Detail.Religion=@MaritalStatus)

                        AND (@Citizen IS NULL OR @Citizen = '' OR @Citizen = 'N/A' OR (CASE 
                        WHEN Detail.IsResidential IS NULL THEN 'NO' 
                        WHEN Detail.IsResidential =1 THEN 'Yes' 
                        ELSE 'NO' END)=@Citizen )

                        AND (@PhysicalCondition IS NULL OR @PhysicalCondition = '' OR @PhysicalCondition = 'N/A' OR (CASE 
                        WHEN Detail.IsPhysicallyDisabled IS NULL THEN 'NO' 
                        WHEN Detail.IsPhysicallyDisabled =1 THEN 'Yes' 
                        ELSE 'NO' END)=@PhysicalCondition )

                        AND (@BranchId IS NULL OR @BranchId=0 OR Info.BranchId=0)
                        AND (@EmployeeId IS NULL OR @EmployeeId=0 OR Info.EmployeeId=@EmployeeId)
                        AND (Info.IsActive=1)
                        AND Info.CompanyId=@CompanyId
                        AND Info.OrganizationId=@OrganizationId) EMP

                        LEFT JOIN (
	                        SELECT EmployeeId,AllowanceNameId,[Total]=ISNULL(SUM([Total]),0) FROM (
						
	                        SELECT EmployeeId, [Total]=ISNULL(DAH.Amount,0)+ISNULL(Arrear,0),ALW.AllowanceNameId FROM Payroll_DepositAllowanceHistory DAH	
	                        INNER JOIN Payroll_ConditionalDepositAllowanceConfig CONFIG ON CONFIG.StateStatus='Approved' AND DAH.ConditionalDepositAllowanceConfigId= CONFIG.Id
	                        INNER JOIN Payroll_AllowanceName ALW ON CONFIG.AllowanceNameId = ALW.AllowanceNameId

	                        Where CONFIG.Id=@Id AND DAH.OrganizationId=@OrganizationId AND DAH.CompanyId=@CompanyId 
	                        AND (@EmployeeId=0 OR @EmployeeId IS NULL OR DAH.EmployeeId=@EmployeeId)
	                        ) TBL
	                        GROUP BY EmployeeId,AllowanceNameId
                        ) DEPOSIT ON DEPOSIT.EmployeeId = EMP.EmployeeId) TBL
                        WHERE 1=1

                        AND (@EmployeeId=0 OR @EmployeeId IS NULL OR EmployeeId=@EmployeeId)
                        AND (@HasPayableAmount IS NULL OR @HasPayableAmount ='NO' OR (Total-ISNULL(Paid,0)) > 0)";

                    list = await _dapper.SqlQueryListAsync<EmployeeDepositPaymentViewModel>(user.Database, query, new {
                        Id = config.Id,
                        AllowanceNameId = allowanceName.AllowanceNameId,
                        JobType = config.JobType,
                        Gender = config.Gender,
                        Religion = config.Religion,
                        MaritalStatus = config.MaritalStatus,
                        Citizen = config.Citizen,
                        PhysicalCondition = config.PhysicalCondition,
                        BranchId = filter.BranchId,
                        EmployeeId = filter.EmployeeId,
                        HasPayableAmount = filter.HasPayableAmount,
                        CompanyId = user.CompanyId,
                        OrganizationId = user.OrganizationId,
                    });
                }

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigRepository", "SaveAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveAsync(ConditionalDepositAllowanceConfigDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try {
                if (model.Id == 0) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction()) {
                                try {
                                    var domain = _mapper.Map<ConditionalDepositAllowanceConfig>(model);
                                    domain.CreatedDate = DateTime.Now;
                                    domain.CompanyId = user.CompanyId;
                                    domain.CreatedBy = user.ActionUserId;
                                    domain.StateStatus = StateStatus.Pending;
                                    domain.OrganizationId = user.OrganizationId;

                                    var parameters = Utility.DappperParamsInKeyValuePairs(
                                        domain,
                                        user,
                                        addBaseProperty: true,
                                        addUserId: false,
                                        addCompany: false,
                                        addOrganization: false);
                                    parameters.Remove("Id");

                                    string query = Utility.GenerateInsertQuery(tableName: "Payroll_ConditionalDepositAllowanceConfig", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                    var itemInDb = await connection.QueryFirstOrDefaultAsync<ConditionalDepositAllowanceConfig>(query, parameters, transaction);

                                    if (itemInDb != null && itemInDb.Id > 0) {
                                        transaction.Commit();
                                        executionStatus = ResponseMessage.Message(true, "Data has been saved successfully");
                                    }
                                    else {
                                        transaction.Rollback();
                                        executionStatus = ResponseMessage.Invalid("Data has been failed to save");
                                    }
                                }
                                catch (Exception ex) {
                                    transaction.Rollback();
                                    executionStatus = ResponseMessage.Invalid("Something went wrong while saving data");
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigRepository", "SaveAsync", user);
                                }
                                finally {
                                    connection.Close();
                                }
                            }
                        }
                    }
                }
                else if (model.Id > 0) {
                    ConditionalDepositAllowanceConfig dataInDb = await this.GetByIdAsync(model.Id, user);
                    if (dataInDb != null && dataInDb.Id > 0 && dataInDb.StateStatus == StateStatus.Pending) {
                        using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                            connection.Open();
                            {
                                using (var transaction = connection.BeginTransaction()) {
                                    try {
                                        dataInDb.AllowanceNameId = model.AllowanceNameId;
                                        dataInDb.ServiceLength = model.ServiceLength;
                                        dataInDb.ServiceLengthUnit = model.ServiceLengthUnit;

                                        dataInDb.Religion = model.Religion;
                                        dataInDb.MaritalStatus = model.MaritalStatus;
                                        dataInDb.Citizen = model.Citizen;
                                        dataInDb.Gender = model.Gender;
                                        dataInDb.PhysicalCondition = model.PhysicalCondition;
                                        dataInDb.DepositType = model.DepositType;
                                        dataInDb.BaseOfPayment = model.BaseOfPayment;
                                        dataInDb.Amount = model.Amount;
                                        dataInDb.Percentage = model.Percentage;
                                        dataInDb.ActivationFrom = model.ActivationFrom;
                                        dataInDb.ActivationTo = model.ActivationTo;

                                        dataInDb.UpdatedBy = user.ActionUserId;
                                        dataInDb.UpdatedDate = DateTime.Now;

                                        var parameters = Utility.DappperParamsInKeyValuePairs(dataInDb, user, addBaseProperty: true, addUserId: false, addCompany: false, addOrganization: false);
                                        parameters.Remove("Id");
                                        string query = Utility.GenerateUpdateQuery(tableName: "Payroll_ConditionalDepositAllowanceConfig", paramkeys: parameters.Select(x => x.Key).ToList());
                                        parameters.Add("Id", model.Id);
                                        query += $@"Where Id=@Id";

                                        var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);
                                        if (rawAffected > 0) {
                                            transaction.Commit();
                                            executionStatus = ResponseMessage.Message(true, "Data has been updated successfully");
                                        }
                                        else {
                                            transaction.Rollback();
                                            executionStatus = ResponseMessage.Invalid("Data has been failed to save");
                                        }
                                    }
                                    catch (Exception ex) {
                                        transaction.Rollback();
                                        executionStatus = ResponseMessage.Invalid("Something went wrong while updating data");
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigRepository", "SaveAsync", user);
                                    }
                                    finally {
                                        connection.Close();
                                    }
                                }
                            }
                        }
                    }
                    else {
                        executionStatus = ResponseMessage.Invalid("Item not found");
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ConditionalDepositAllowanceConfigRepository", "SaveAsync", user);
            }
            return executionStatus;
        }
    }
}
