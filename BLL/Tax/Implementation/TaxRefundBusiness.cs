using AutoMapper;
using System.Data;
using Shared.Models;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using DAL.Context.Payroll;
using DAL.Context.Employee;
using Shared.Payroll.DTO.Tax;
using Shared.OtherModels.User;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.Domain.AIT;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.Payroll.ViewModel.Tax;
using Shared.OtherModels.Pagination;
using Microsoft.EntityFrameworkCore;
using DAL.Payroll.Repository.Interface;

namespace BLL.Tax.Implementation
{
    public class TaxRefundBusiness : ITaxRefundBusiness
    {
        private readonly IMapper _mapper;
        private readonly IDapperData _dapper;
        private readonly EmployeeModuleDbContext _employeeDbContext;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly ISysLogger _sysLogger;
        private readonly ITaxDocumentSubmissionRepository _taxDocumentSubmissionRepository;

        public TaxRefundBusiness(IMapper mapper, IDapperData dapper, ISysLogger sysLogger,
            ITaxDocumentSubmissionRepository taxDocumentSubmissionRepository,
            PayrollDbContext payrollDbContext, EmployeeModuleDbContext employeeDbContext)
        {
            _mapper = mapper;
            _dapper = dapper;
            _payrollDbContext = payrollDbContext;
            _employeeDbContext = employeeDbContext;
            _sysLogger = sysLogger;
            _taxDocumentSubmissionRepository = taxDocumentSubmissionRepository;
        }

        public Task<ExecutionStatus> BlukCheckingAsync(List<CheckingModel> models, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<ExecutionStatus> CheckingAsync(string model, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<ExecutionStatus> DeleteAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var refundInDb = await _payrollDbContext.Payroll_TaxDocumentSubmission.FirstOrDefaultAsync(item =>
                    item.SubmissionId == id
                    && item.CertificateType == "CET"
                    && item.CompanyId == user.CompanyId
                    && item.OrganizationId == user.OrganizationId
                );

                if (refundInDb != null && refundInDb.SubmissionId > 0)
                {
                    _payrollDbContext.Remove(refundInDb);
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
                    executionStatus = ResponseMessage.Message(false, "Item not found by this id");
                }
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundBusiness", "DeleteAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<TaxDocumentSubmissionViewModel>> GetAllAsync(TaxDocumentQuery filter, AppUser user)
        {
            DBResponse<TaxDocumentSubmissionViewModel> data = new DBResponse<TaxDocumentSubmissionViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                filter.CertificateType = "CET";
                var sp_name = "sp_Payroll_TaxDocumentSubmission_List";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<TaxDocumentSubmissionViewModel>>(response.JSONData) ?? new List<TaxDocumentSubmissionViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "GetEmployeeTaxDocumentsAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<TaxDocumentSubmissionViewModel>> GetIndividualEmployeeAsync(TaxDocumentQuery filter, AppUser user)
        {
            IEnumerable<TaxDocumentSubmissionViewModel> list = new List<TaxDocumentSubmissionViewModel>();
            try
            {
                var query = $@"SELECT TAX.SubmissionId,TAX.EmployeeId,EMP.EmployeeCode,EmployeeName=EMP.FullName,IY.FiscalYearId,IY.FiscalYearRange,
                TAX.Amount, FilePath=(ISNULL(TAX.FilePath,'') +'/'+ ISNULL(TAX.[FileName],'')),TAX.StateStatus
                FROM Payroll_TaxDocumentSubmission TAX
                INNER JOIN HR_EmployeeInformation EMP ON TAX.EmployeeId= EMP.EmployeeId
                INNER JOIN Payroll_FiscalYear IY ON TAX.FiscalYearId = IY.FiscalYearId
                Where TAX.CertificateType='CET'
                 AND (@FiscalYearId IS NULL OR @FiscalYearId =0 OR TAX.FiscalYearId=@FiscalYearId) 
                AND TAX.EmployeeId=@EmployeeId AND TAX.CompanyId=@CompanyId AND TAX.OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<TaxDocumentSubmissionViewModel>(user.Database, query, new
                {
                    filter.EmployeeId,
                    filter.FiscalYearId,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundBusiness", "GetIndividualEmployeeAsync", user);
            }
            return list;
        }
        public async Task<TaxDocumentSubmissionViewModel> GetSingleAsync(TaxDocumentQuery filter, AppUser user)
        {
            TaxDocumentSubmissionViewModel data = new TaxDocumentSubmissionViewModel();
            try
            {
                var query = $@"SELECT TAX.SubmissionId,TAX.EmployeeId,EMP.EmployeeCode,EmployeeName=EMP.FullName,IY.FiscalYearId,IY.FiscalYearRange,
                TAX.Amount, FilePath=(ISNULL(TAX.FilePath,'') +'/'+ ISNULL(TAX.[FileName],'')),TAX.StateStatus
                FROM Payroll_TaxDocumentSubmission TAX
                INNER JOIN HR_EmployeeInformation EMP ON TAX.EmployeeId= EMP.EmployeeId
                INNER JOIN Payroll_FiscalYear IY ON TAX.FiscalYearId = IY.FiscalYearId
                Where TAX.CertificateType='CET' AND TAX.SubmissionId=@SubmissionId AND TAX.CompanyId=@CompanyId AND TAX.OrganizationId=@OrganizationId";
                data = await _dapper.SqlQueryFirstAsync<TaxDocumentSubmissionViewModel>(user.Database, query, new
                {
                    filter.SubmissionId,
                    user.CompanyId,
                    user.OrganizationId
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundBusiness", "GetSingleAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveAsync(TaxRefundSubmissionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var item = _mapper.Map<TaxDocumentSubmission>(model);
                if (model.File != null)
                {
                    var fileDetail = FileProcessor.Process(model.SubmissionId, model.File, model.FilePath, user);
                    item.FilePath = fileDetail.FilePath.Replace("//", "/");
                    item.FileSize = fileDetail.FileSize;
                    item.FileFormat = fileDetail.Extenstion;
                    item.ActualFileName = model.File.FileName;
                    item.FileName = fileDetail.FileName;
                }
                else
                {
                    item.FilePath = null;
                }
                executionStatus = await _taxDocumentSubmissionRepository.SaveTaxRefundAsync(item, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundBusiness", "SaveAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> UploadAsync(List<TaxDocumentSubmissionDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var employeeCodes = model.Select(x => x.EmployeeCode).ToArray();

                var employees = (from emp in _employeeDbContext.HR_EmployeeInformation
                                 where employeeCodes.Contains(emp.EmployeeCode)
                                 select new { EmployeeId = emp.EmployeeId, EmployeeCode = emp.EmployeeCode }).ToList();

                if (employees.Any())
                {
                    var items = (from m in model
                                 join emp in employees on m.EmployeeCode equals emp.EmployeeCode
                                 select new TaxDocumentSubmission()
                                 {
                                     EmployeeId = emp.EmployeeId,
                                     FiscalYearId = m.FiscalYearId,
                                     CertificateType = m.CertificateType,
                                     Amount = m.Amount,
                                     StateStatus = StateStatus.Approved,
                                     IsApproved = true,
                                     CreatedBy = user.ActionUserId,
                                     CreatedDate = DateTime.Now,
                                     CompanyId = user.CompanyId,
                                     OrganizationId = user.OrganizationId
                                 }).ToList();

                    if (items.Any())
                    {
                        await _payrollDbContext.Payroll_TaxDocumentSubmission.AddRangeAsync(items);
                        var rowCount = await _payrollDbContext.SaveChangesAsync();
                        if (rowCount > 0)
                        {
                            executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Saved);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                        }
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, "There is not any eligible employee to save");
                    }
                }
                else
                {
                    executionStatus = ResponseMessage.Message(false, "There is one or more invalid employee id(s)");
                }

            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.SomthingWentWrong);
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "SaveAsync", user);
            }
            return executionStatus;
        }
    }
}
