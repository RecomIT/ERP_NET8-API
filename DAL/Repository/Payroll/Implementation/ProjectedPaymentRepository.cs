using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.Payroll.Repository.Interface;
using Shared.OtherModels.Response;
using Shared.Services;
using AutoMapper;
using Dapper;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using Shared.Payroll.Domain.Payment;
using Shared.Payroll.DTO.Payment;
using Shared.Payroll.ViewModel.Payment;


namespace DAL.Payroll.Repository.Implementation
{
    public class ProjectedPaymentRepository : IProjectedPaymentRepository
    {
        private readonly IDapperData _dapper;
        private readonly IDALSysLogger _sysLogger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IFiscalYearRepository _fiscalYearRepository;
        private readonly IMapper _mapper;

        public ProjectedPaymentRepository(
            IDapperData dapper,
            IEmployeeRepository employeeRepository,
            IDALSysLogger sysLogger,
            IFiscalYearRepository fiscalYearRepository,
            IMapper mapper)
        {
            _dapper = dapper;
            _mapper = mapper;
            _sysLogger = sysLogger;
            _employeeRepository = employeeRepository;
            _fiscalYearRepository = fiscalYearRepository;

        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmployeeProjectedPayment>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<EmployeeProjectedPayment>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<EmployeeProjectedPayment> GetByIdAsync(long id, AppUser user)
        {
            EmployeeProjectedPayment data = null;
            try {
                string query = $@"SELECT * FROM Payroll_EmployeeProjectedAllowance 
                Where ProjectedAllowanceId=@Id And CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                data = await _dapper.SqlQueryFirstAsync<EmployeeProjectedPayment>(user.Database, query, new {
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });
            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentRepository", "GetByIdAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveAysnc(List<EmployeeProjectedPaymentDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            var currentFiscalYear = await _fiscalYearRepository.GetFiscalYearByYearAsync((model.FirstOrDefault().PayableYear ?? 0), user);
            try {
                if (currentFiscalYear != null && currentFiscalYear.FiscalYearId > 0) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction()) {
                                try {
                                    int count = 0;
                                    foreach (var employee in model) {
                                        var employeeIdInDb = await _employeeRepository.GetByCodeAsync(employee.EmployeeCode, user);
                                        if (employeeIdInDb != null) {
                                            employee.EmployeeId = employeeIdInDb.EmployeeId;
                                            employee.EmployeeCode = employeeIdInDb.EmployeeCode;
                                            employee.FiscalYearId = currentFiscalYear.FiscalYearId;
                                            var domain = _mapper.Map<EmployeeProjectedPayment>(employee);
                                            domain.StateStatus = StateStatus.Pending;
                                            domain.CompanyId = user.CompanyId;
                                            domain.OrganizationId = user.OrganizationId;
                                            domain.CreatedBy = user.ActionUserId;
                                            domain.CreatedDate = DateTime.Now;

                                            var parameters = new Dictionary<string, object>();
                                            parameters = Utility.DappperParamsInKeyValuePairs(domain, user,
                                                addBaseProperty: true,
                                                addUserId: false,
                                                addCompany: false,
                                                addOrganization: false);
                                            parameters.Remove("ProjectedAllowanceId");
                                            parameters.Remove("EmployeeProjectedAllowanceProcessInfo");
                                            var query = Utility.GenerateInsertQuery(
                                                tableName: "Payroll_EmployeeProjectedAllowance",
                                                paramkeys: parameters.Select(x => x.Key).ToList(),
                                                output: "OUTPUT INSERTED.*");

                                            var execute = await connection.ExecuteAsync(query, parameters, transaction);

                                            if (execute > 0) {
                                                count++;
                                            }
                                            else {
                                                throw new Exception("Employee with this ID " + employee.EmployeeCode + " can not be inserted");
                                            }
                                        }
                                        else {
                                            throw new Exception("Employee with this ID " + employee.EmployeeCode + " is not found");
                                        }
                                    }
                                    if (count == model.Count) {
                                        transaction.Commit();
                                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                    }
                                    else {
                                        transaction.Rollback();
                                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                                    }
                                }
                                catch (Exception ex) {
                                    if (ex.InnerException != null) {
                                        executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentRepository", "SaveAysnc", user);
                                    }
                                    else {
                                        executionStatus = ResponseMessage.Message(false, ex.Message);
                                    }
                                }
                            }
                        }
                    }
                }
                else {
                    throw new Exception("No Income year found in this day");
                }
            }
            catch (Exception ex) {
                if (ex.InnerException != null) {
                    executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                    await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentRepository", "SaveAysnc", user);
                }
                else {
                    executionStatus = ResponseMessage.Message(false, ex.Message);
                }
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeletePendingAllowanceByIdAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            var itemInDb = await GetByIdAsync(id, user);
            try {
                if (itemInDb != null && itemInDb.StateStatus == StateStatus.Pending) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction()) {
                                try {
                                    var query = $@"DELETE FROM Payroll_EmployeeProjectedAllowance 
                                    Where ProjectedAllowanceId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                    var itemCount = await connection.ExecuteAsync(query, new {
                                        Id = id,
                                        CompanyId = user.CompanyId,
                                        OrganizationId = user.OrganizationId
                                    }, transaction);

                                    if (itemCount > 0) {
                                        transaction.Commit();
                                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                    }
                                    else {
                                        throw new Exception("Something went wrong while deleting the item.");
                                    }
                                }
                                catch (Exception ex) {
                                    transaction.Rollback();
                                    if (ex.InnerException != null) {
                                        executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentRepository", "DeletePendingAllowanceByIdAsync", user);
                                    }
                                    else {
                                        executionStatus = ResponseMessage.Message(false, ex.Message);
                                    }

                                }
                            }
                        }
                    }
                }
                else {
                    executionStatus = ResponseMessage.Message(false, "Item is not found/status has been changed");
                }
            }
            catch (Exception ex) {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentRepository", "DeletePendingAllowanceByIdAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> DeleteApprovedAllowanceByIdAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            var itemInDb = await GetByIdAsync(id, user);
            try {
                if (itemInDb != null && itemInDb.StateStatus == StateStatus.Approved) {
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database)) {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction()) {
                                try {
                                    var query = $@"DELETE FROM Payroll_EmployeeProjectedAllowance 
                                    Where ProjectedAllowanceId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                    var itemCount = await connection.ExecuteAsync(query, new {
                                        Id = id,
                                        CompanyId = user.CompanyId,
                                        OrganizationId = user.OrganizationId
                                    }, transaction);

                                    if (itemCount > 0) {
                                        transaction.Commit();
                                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                                    }
                                    else {
                                        throw new Exception("Something went wrong while deleting the item.");
                                    }
                                }
                                catch (Exception ex) {
                                    transaction.Rollback();
                                    if (ex.InnerException != null) {
                                        executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                                        await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentRepository", "DeletePendingAllowanceByIdAsync", user);
                                    }
                                    else {
                                        executionStatus = ResponseMessage.Message(false, ex.Message);
                                    }

                                }
                            }
                        }
                    }
                }
                else {
                    executionStatus = ResponseMessage.Message(false, "Item is not found/status has been changed");
                }
            }
            catch (Exception ex) {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentRepository", "DeletePendingAllowanceByIdAsync", user);
            }
            return executionStatus;
        }
        public async Task<EmployeeProjectedPaymentViewModel> GetProjectedAllowanceByIdAsync(long id, AppUser user)
        {
            EmployeeProjectedPaymentViewModel model = new EmployeeProjectedPaymentViewModel();
            try {
                var query = $@"SELECT EPA.ProjectedAllowanceId,ProjectedAllowanceCode=('ID-' + FORMAT(ProjectedAllowanceId,'0000000')),
    EPA.AllowanceReason,EPA.PayableYear,EPA.PaymentYear,EPA.PaymentMonth,
	PaymentMonthName=dbo.fnGetMonthName(EPA.PaymentMonth),EPA.AllowanceNameId,EI.EmployeeId,EI.EmployeeCode,
	EmployeeName=EI.FullName,[AllowanceName]=ALW.[Name],EPA.BaseOfPayment,EPA.[Percentage],EPA.Amount,
	EPA.PayableAmount,EPA.DisbursedAmount,EPA.TaxAmount,EPA.StateStatus,EPA.IsDisbursed,EPA.FiscalYearId,FY.FiscalYearRange
	FROM Payroll_EmployeeProjectedAllowance EPA
	INNER JOIN HR_EmployeeInformation EI ON EPA.EmployeeId=EI.EmployeeId
	LEFT JOIN HR_EmployeeDetail ED ON EI.EmployeeId=ED.EmployeeId
	INNER JOIN Payroll_AllowanceName ALW ON EPA.AllowanceNameId= ALW.AllowanceNameId
	INNER JOIN Payroll_FiscalYear FY ON EPA.FiscalYearId = FY.FiscalYearId
	Where 1=1 AND EPA.ProjectedAllowanceId=@Id AND EPA.CompanyId=@CompanyId AND EPA.OrganizationId=@OrganizationId";
                model = await _dapper.SqlQueryFirstAsync<EmployeeProjectedPaymentViewModel>(user.Database, query, new {
                    Id = id,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });

            }
            catch (Exception ex) {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ProjectedPaymentRepository", "GetProjectedAllowanceByIdAsync", user);
            }
            return model;
        }
    }
}
