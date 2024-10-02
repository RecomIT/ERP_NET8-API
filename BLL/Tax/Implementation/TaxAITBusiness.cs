using System;
using Dapper;
using AutoMapper;
using System.Data;
using Shared.Models;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.Payroll.Repository.Interface;
using Shared.OtherModels.DataService;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using BLL.Tax.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.Filter.Tax;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.Domain.AIT;
using DAL.Context.Payroll;
using DAL.Context.Employee;
using Microsoft.EntityFrameworkCore;

namespace BLL.Tax.Implementation
{
    public class TaxAITBusiness : ITaxAITBusiness
    {
        private readonly IMapper _mapper;
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly EmployeeModuleDbContext _employeeDbContext;
        private readonly ITaxDocumentSubmissionRepository _taxDocumentSubmissionRepository;
        public TaxAITBusiness(
            IDapperData dapper,
            ISysLogger sysLogger,
            PayrollDbContext payrollDbContext,
            EmployeeModuleDbContext employeeDbContext,
            ITaxDocumentSubmissionRepository taxDocumentSubmissionRepository,
            IMapper mapper)
        {
            _dapper = dapper;
            _mapper = mapper;
            _payrollDbContext = payrollDbContext;
            _employeeDbContext = employeeDbContext;
            _sysLogger = sysLogger;
            _taxDocumentSubmissionRepository = taxDocumentSubmissionRepository;
        }
        public async Task<ExecutionStatus> SaveAITAsync(TaxDocumentSubmissionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            string file;
            string filePath = "";
            string fileName = "";
            string extenstion = "";
            string fileSize = "";
            string actualFileName = "";
            try
            {
                if (model.SubmissionId > 0 && model.File == null)
                {
                    // Persist Existing 
                }
                else if (model.SubmissionId > 0 && model.File != null)
                {
                    Utility.DeleteFile(string.Format(@"{0}/{1}", Utility.PhysicalDriver, model.FilePath));
                    file = await Utility.SaveFileAsync(model.File, string.Format(@"{0}", user.OrgCode));
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(model.File.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = model.File.FileName;
                }
                else if (model.SubmissionId == 0 && model.File != null)
                {
                    file = await Utility.SaveFileAsync(model.File, string.Format(@"{0}", user.OrgCode));
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(model.File.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = model.File.FileName;
                }

                if (model.SubmissionId > 0)
                {
                    var itemInDb = _payrollDbContext.Payroll_TaxDocumentSubmission.FirstOrDefault(item =>
                     item.SubmissionId == model.SubmissionId
                     && item.CompanyId == user.CompanyId
                     && item.OrganizationId == user.OrganizationId);
                    if (itemInDb != null)
                    {
                        itemInDb.Amount = model.Amount;
                        itemInDb.UpdatedBy = user.ActionUserId;
                        itemInDb.UpdatedDate = DateTime.Now;
                        itemInDb.CompanyId = user.CompanyId;
                        itemInDb.OrganizationId = user.OrganizationId;
                        if (model.File != null)
                        {
                            itemInDb.FileName = fileName;
                            itemInDb.FilePath = filePath;
                            itemInDb.FileSize = fileSize;
                            itemInDb.FileFormat = extenstion;
                        }

                        _payrollDbContext.Update(itemInDb);
                        var rowCount = await _payrollDbContext.SaveChangesAsync();
                        if (rowCount > 0)
                        {
                            executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Updated);
                        }
                        else
                        {
                            executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                        }
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, "Item not found");
                    }
                }
                else
                {
                    var domain = _mapper.Map<TaxDocumentSubmission>(model);
                    domain.FileName = fileName;
                    domain.FilePath = filePath;
                    domain.FileSize = fileSize;
                    domain.FileFormat = extenstion;
                    domain.CompanyId = user.CompanyId;
                    domain.OrganizationId = user.OrganizationId;
                    domain.CreatedBy = user.ActionUserId;
                    domain.CreatedDate = DateTime.Now;

                    await _payrollDbContext.AddAsync(domain);
                    var rowCount = await _payrollDbContext.SaveChangesAsync();
                    if (rowCount > 0)
                    {
                        executionStatus = ResponseMessage.Message(true, rowCount.ToString() + " " + ResponseMessage.Updated);
                    }
                    else
                    {
                        executionStatus = ResponseMessage.Message(false, ResponseMessage.Unsuccessful);
                    }
                }

                //executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "SaveEmployeeTaxDocumentAsync", user);
            }
            return executionStatus;
        }
        public async Task<DBResponse<TaxDocumentSubmissionViewModel>> GetEmployeeAITDocumentsAsync(TaxDocumentQuery query, AppUser user)
        {
            DBResponse<TaxDocumentSubmissionViewModel> data = new DBResponse<TaxDocumentSubmissionViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                query.CertificateType = "AIT";
                var sp_name = "sp_Payroll_TaxDocumentSubmission_List";
                var parameters = Utility.DappperParams(query, user, addBaseProperty: true);
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
        public async Task<IEnumerable<Select2Dropdown>> GetAITFiscalYearsExtensionAsync(string fiscalYearRange, AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = "sp_Payroll_TaxDocumentSubmission_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("FiscalYearRange", fiscalYearRange);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", Data.Extension);
                data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "GetAITFiscalYearsExtensionAsync", user);
            }
            return data;
        }
        public async Task<decimal> GetCarAITAmountInTaxProcessAsync(long employeeId, long fiscalYearId, AppUser user)
        {
            decimal total = 0;
            try
            {
                var query = $@"SELECT ISNULL((Select SUM(ISNULL(Amount,0)) FROM Payroll_TaxDocumentSubmission 
			     Where FiscalYearId=@FiscalYearId AND CertificateType='AIT' AND EmployeeId =@EmployeeId
			     AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),0.00)";

                total = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new { FiscalYearId = fiscalYearId, EmployeeId = employeeId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "GetCarAITAmountInTaxProcessAsync", user);
            }
            return total;
        }
        public async Task<decimal> GetTaxRefundAmountInTaxProcessAsync(long employeeId, long fiscalYearId, AppUser user)
        {
            decimal total = 0;
            try
            {
                var query = $@"SELECT ISNULL((Select SUM(ISNULL(Amount,0)) FROM Payroll_TaxDocumentSubmission 
			     Where FiscalYearId=@FiscalYearId AND CertificateType='CET' AND EmployeeId =@EmployeeId
			     AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId),0.00)";

                total = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, new { FiscalYearId = fiscalYearId, EmployeeId = employeeId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "GetTaxRefundAmountInTaxProcessAsync", user);
            }
            return total;
        }
        public async Task<ExecutionStatus> SaveAsync(AITSubmissionDTO model, AppUser user)
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
                executionStatus = await _taxDocumentSubmissionRepository.SaveAITAsync(item, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "SaveAsync", user);
            }
            return executionStatus;
        }
        public Task<ExecutionStatus> CheckingAsync(string model, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<ExecutionStatus> BlukCheckingAsync(List<CheckingModel> models, AppUser user)
        {
            throw new NotImplementedException();
        }
        public async Task<DBResponse<TaxDocumentSubmissionViewModel>> GetAllAsync(TaxDocumentQuery filter, AppUser user)
        {
            DBResponse response = new DBResponse();
            DBResponse<TaxDocumentSubmissionViewModel> data = new DBResponse<TaxDocumentSubmissionViewModel>();
            try
            {

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxRefundBusiness", "GetAllAsync", user);
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
                Where TAX.CertificateType='AIT' AND TAX.EmployeeId=@EmployeeId 
                AND (@FiscalYearId IS NULL OR @FiscalYearId =0 OR TAX.FiscalYearId=@FiscalYearId) 
                AND TAX.CompanyId=@CompanyId AND TAX.OrganizationId=@OrganizationId";
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
                Where TAX.CertificateType='AIT' AND TAX.SubmissionId=@SubmissionId AND TAX.CompanyId=@CompanyId AND TAX.OrganizationId=@OrganizationId";
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
        public async Task<ExecutionStatus> DeleteAsync(long id, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var aitInDb = await _payrollDbContext.Payroll_TaxDocumentSubmission.FirstOrDefaultAsync(item =>
                    item.SubmissionId == id
                    && item.CertificateType == "AIT"
                    && item.CompanyId == user.CompanyId
                    && item.OrganizationId == user.OrganizationId
                );

                if (aitInDb != null && aitInDb.SubmissionId > 0)
                {
                    _payrollDbContext.Remove(aitInDb);
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
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxAITBusiness", "DeleteAsync", user);
            }
            return executionStatus;
        }
    }
}
