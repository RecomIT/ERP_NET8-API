using AutoMapper;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using DAL.Context.Payroll;
using DAL.Context.Employee;
using Shared.Payroll.DTO.Tax;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Tax;
using Shared.OtherModels.Response;

namespace BLL.Tax.Implementation
{
    public class TaxChallanBusiness : ITaxChallanBusiness
    {
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        private readonly PayrollDbContext _context;
        private readonly IMapper _mapper;
        private readonly ISysLogger _sysLogger;
        public TaxChallanBusiness(EmployeeModuleDbContext employeeModuleDbContext, PayrollDbContext context, ISysLogger sysLogger, IMapper mapper)
        {
            _employeeModuleDbContext = employeeModuleDbContext;
            _mapper = mapper;
            _context = context;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> BulkSaveAsync(AllEmployeesTaxChallanInsertDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var items = (from tax in _context.Payroll_EmployeeTaxProcess
                             where tax.SalaryMonth == model.TaxMonth && tax.SalaryYear == model.TaxYear && tax.FiscalYearId == model.FiscalYearId
                             && tax.CompanyId == user.CompanyId && tax.OrganizationId == user.OrganizationId && tax.ActualTaxDeductionAmount > 0
                             && (model.SalaryProcessId == 0 ? 1 == 1 : tax.SalaryProcessId == model.SalaryProcessId)
                             select new TaxChallan()
                             {
                                 EmployeeId = tax.EmployeeId,
                                 Month = model.TaxMonth,
                                 TaxMonth = Utility.GetMonthName(model.TaxMonth),
                                 TaxYear = model.TaxYear,
                                 TaxDate = new DateTime(model.TaxYear, model.TaxMonth, 1),
                                 FiscalYearId = model.FiscalYearId,
                                 ChallanNumber = model.ChallanNumber,
                                 ChallanDate = model.ChallanDate,
                                 DepositeBank = model.DepositeBank,
                                 DepositeBranch = model.DepositeBranch,
                                 Amount = model.Amount,
                                 BranchId = tax.BranchId,
                                 CompanyId = tax.CompanyId,
                                 OrganizationId = tax.OrganizationId,
                                 CreatedBy = user.ActionUserId,
                                 CreatedDate = DateTime.Now,
                                 
                             }).ToList();

                if (items.Any())
                {
                    await _context.AddRangeAsync(items);
                    var rowCount = await _context.SaveChangesAsync();
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
                    executionStatus = ResponseMessage.Message(false, "There isn't any employee in tax process");
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxChallanBusiness", "BulkSaveAsync", user);
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }

        public Task<ExecutionStatus> TaxChallanDTO(TaxChallanDTO model, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<ExecutionStatus> UploadTaxChallanAsync(List<TaxChallanDTO> taxChallanDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                if (taxChallanDTOs != null && taxChallanDTOs.Any())
                {
                    var firstItem = taxChallanDTOs.First();
                    var employeeCodes = taxChallanDTOs.Select(i => i.EmployeeCode).ToArray();
                    var challans = (from emp in _employeeModuleDbContext.HR_EmployeeInformation
                                    where employeeCodes.Contains(emp.EmployeeCode)
                                    select new TaxChallanDTO()
                                    {
                                        EmployeeId = emp.EmployeeId,
                                        ChallanNumber = firstItem.ChallanNumber,
                                        ChallanDate = firstItem.ChallanDate,
                                        DepositeBank = firstItem.DepositeBank,
                                        DepositeBranch = firstItem.DepositeBranch,
                                        FiscalYearId = firstItem.FiscalYearId,
                                        TaxMonth = firstItem.TaxMonth,
                                        TaxYear = firstItem.TaxYear,
                                        Amount = firstItem.Amount,
                                        Month = firstItem.TaxMonth,
                                    });

                    var models = _mapper.Map<IEnumerable<TaxChallan>>(challans);
                    foreach (var item in models)
                    {
                        item.TaxMonth = DateTimeExtension.GetMonthName(item.Month);
                        item.CreatedBy = user.ActionUserId;
                        item.CreatedDate = DateTime.Now;
                        item.CompanyId = user.CompanyId;
                        item.OrganizationId = user.OrganizationId;
                    }

                    await _context.AddRangeAsync(models);
                    var rowCount = await _context.SaveChangesAsync();
                    if (rowCount > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, ResponseMessage.Successfull);
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxChallanBusiness", "UploadTaxChallanAsync", user);
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
    }
}
