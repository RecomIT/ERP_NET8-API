using AutoMapper;
using DAL.DapperObject.Interface;
using DAL.Logger.Interface;
using DAL.Payroll.Repository.Interface;
using Dapper;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.DTO.Payment;
using Shared.Services;
using System.Data;

namespace DAL.Payroll.Repository.Implementation
{
    public class DepositAllowancePaymentHistoryRepository : IDepositAllowancePaymentHistoryRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly IFiscalYearRepository _fiscalYearRepository;

        public DepositAllowancePaymentHistoryRepository(IDALSysLogger sysLogger, IDapperData dapper, IMapper mapper, IFiscalYearRepository fiscalYearRepository)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _mapper = mapper;
            _fiscalYearRepository = fiscalYearRepository;
        }

        public async Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            try {
                var query = $@"DELETE FROM Payroll_DepositAllowancePaymentHistory Where Id=@Id AND CompanyId= @CompanyId AND OrganizationId=@OrganizationId";
                var execute = await _dapper.SqlExecuteNonQuery(user.Database, query, new { Id = id, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId }, CommandType.Text);
                return execute;
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryRepository", "DeleteByIdAsync", user);
            }
            return 0;
        }
        public Task<IEnumerable<DepositAllowancePaymentHistory>> GetAllAsync(AppUser user)
        {
            throw new System.NotImplementedException();
        }
        public Task<IEnumerable<DepositAllowancePaymentHistory>> GetAllAsync(object filter, AppUser user)
        {
            throw new System.NotImplementedException();
        }
        public async Task<DepositAllowancePaymentHistory> GetByIdAsync(long id, AppUser user)
        {
            DepositAllowancePaymentHistory domain = null;
            try {
                var query = $@"SELECT * FROM Payroll_DepositAllowancePaymentHistory Where Id=@Id AND CompanyId= @CompanyId AND OrganizationId=@OrganizationId";
                domain = await _dapper.SqlQueryFirstAsync<DepositAllowancePaymentHistory>(user.Database, query, new { Id = id, CompanyId = user.CompanyId, OrganizationId = user.OrganizationId });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryRepository", "DeleteByIdAsync", user);
            }
            return domain;
        }
        public async Task<ExecutionStatus> SavePaymentOfDepositAmountAsync(List<PaymentOfDepositAmountByConfigDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try {
                var fiscalYearInfo = await _fiscalYearRepository.GetCurrectFiscalYearAsync(user);
                if (fiscalYearInfo != null) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        int count = 0;
                        using (var transaction = connection.BeginTransaction()) {
                            foreach (var item in model) {
                                var domain = _mapper.Map<DepositAllowancePaymentHistory>(item);
                                domain.IsVisibleInPayslip = true;
                                domain.IsVisibleInSalarySheet = true;
                                domain.IncomingFlag = (domain.ConditionalDepositAllowanceConfigId ?? 0) > 0 ? "Conditional" : "Individual";
                                domain.CreatedBy = user.ActionUserId;
                                domain.StateStatus = StateStatus.Pending;
                                domain.CreatedDate = DateTime.Now;
                                domain.CompanyId = user.CompanyId;
                                domain.OrganizationId = user.OrganizationId;

                                var parameters = Utility.DappperParamsInKeyValuePairs(
                                           domain,
                                           user,
                                           addBaseProperty: true,
                                           addUserId: false,
                                           addCompany: false,
                                           addOrganization: false);
                                parameters.Remove("Id");

                                string query = Utility.GenerateInsertQuery(tableName: "Payroll_DepositAllowancePaymentHistory", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");
                                var itemInDb = await connection.QueryFirstOrDefaultAsync<DepositAllowancePaymentHistory>(query, parameters, transaction);

                                if (itemInDb != null && itemInDb.Id > 0) {
                                    count++;
                                }
                            }
                            if (count == model.Count) {
                                transaction.Commit();
                                executionStatus = ResponseMessage.Message(true, "Data has been saved successfully");
                            }
                            else {
                                transaction.Rollback();
                                executionStatus = ResponseMessage.Invalid("Data has been failed to save");
                            }
                        }
                    }
                }
                else {
                    executionStatus = ResponseMessage.Invalid("Fiscal Year Information is not found");
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryRepository", "SavePaymentOfDepositAmountAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UpdatePaymentOfDepositAmountAsync(PaymentOfDepositAmountByConfigDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try {

                var domain = await GetByIdAsync((model.ConditionalDepositAllowanceConfigId ?? 0), user);
                if (domain != null && domain.Id > 0 && domain.StateStatus == StateStatus.Pending) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction()) {
                                domain.PaymentApproach = model.PaymentApproach;
                                domain.PayableAmount = model.PayableAmount;
                                domain.ProposalAmount = model.ProposalAmount;
                                domain.UpdatedBy = user.ActionUserId;
                                domain.UpdatedDate = DateTime.Now;

                                var parameters = Utility.DappperParamsInKeyValuePairs(
                                          domain,
                                          user,
                                          addBaseProperty: true,
                                          addUserId: false,
                                          addCompany: false,
                                          addOrganization: false);
                                parameters.Remove("Id");

                                string query = Utility.GenerateUpdateQuery(tableName: "Payroll_DepositAllowancePaymentHistory", paramkeys: parameters.Select(x => x.Key).ToList());
                                parameters.Add("Id", model.Id);
                                query += $@"Where Id=@Id";


                                var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);
                                if (rawAffected > 0) {
                                    executionStatus = new ExecutionStatus();
                                    executionStatus.Status = true;
                                    executionStatus.Msg = "Data has been updated successfully";
                                    transaction.Commit();
                                }
                                else {
                                    transaction.Rollback();
                                    executionStatus = new ExecutionStatus();
                                    executionStatus.Status = false;
                                    executionStatus.Msg = "Data has been failed to update";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DepositAllowancePaymentHistoryRepository", "UpdatePaymentOfDepositAmountAsync", user);
            }
            return executionStatus;
        }
    }
}
