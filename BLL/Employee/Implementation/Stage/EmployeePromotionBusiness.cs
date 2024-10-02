using System.Data;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using Shared.Employee.DTO.Stage;
using Shared.Employee.Filter.Stage;
using Shared.Employee.ViewModel.Stage;
using Dapper;
using AutoMapper;
using BLL.Employee.Interface.Stage;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using Shared.Employee.Domain.Stage;

namespace BLL.Employee.Implementation.Stage
{
    public class EmployeePromotionBusiness : IEmployeePromotionBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        private readonly IEmployeePromotionProposalRepository _employeePromotionProposalRepository;

        public EmployeePromotionBusiness(ISysLogger sysLogger, IDapperData dapper, IEmployeePromotionProposalRepository employeePromotionProposalRepository)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _employeePromotionProposalRepository = employeePromotionProposalRepository;
        }
        public async Task<DBResponse<EmployeePromotionProposalViewModel>> GetEmployeePromotionProposalsAsync(EmployeePromotion_Filter query, AppUser user)
        {
            DBResponse<EmployeePromotionProposalViewModel> data = new DBResponse<EmployeePromotionProposalViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_HR_EmployeePromotion_List";
                var parameters = DapperParam.AddParams(query, user, addBaseProperty: true, addBranch: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeePromotionProposalViewModel>>(response.JSONData) ?? new List<EmployeePromotionProposalViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePromotionBusiness", "GetEmployeePromotionProposalsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SavePromotionProposalAsync(EmployeePromotionProposalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_PromotionProposal_Insert_Update";
                var parameters = DapperParam.AddParams(model, user, addBranch: true);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeePromotionBusiness", "SavePromotionProposalAsync", user);
            }
            return executionStatus;
        }
        /// <summary>
        /// /Added by Monzur 11-Sep-23
        /// </summary>
        /// <param name="readExcelDTOs"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<ExecutionStatus> UploadPromotionProposalAsync(List<PromotionProposalReadExcelDTO> readExcelDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = string.Format(@"sp_HR_UploadEmployeePromotionProposal");
                List<PromotionProposalInsertExcelDTO> insertExcelDTOs = new List<PromotionProposalInsertExcelDTO>();
                foreach (var item in readExcelDTOs)
                {
                    var proposal = new PromotionProposalInsertExcelDTO();
                    proposal.EmployeeCode = item.EmployeeCode;
                    proposal.PrevDesignationId = await GetEmployeeComponentId(item.EmployeeCode, "", user, "PrevDesignationId");
                    proposal.PrevDesignationName = await GetEmployeeComponentText(item.EmployeeCode, proposal.PrevDesignationId, user, "PrevDesignationName");
                    proposal.GrdId = await GetEmployeeComponentId(item.GradeName, "", user, "Grade");
                    proposal.DesigId = await GetEmployeeComponentId(item.ProposalText, item.GradeName, user, "Designation");
                    proposal.Head = item.Head;
                    proposal.EffectiveDate = item.EffectiveDate;
                    proposal.ProposalText = item.ProposalText;
                    insertExcelDTOs.Add(proposal);
                }
                var jsonData = Utility.JsonData(insertExcelDTOs);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.UserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("UserBranchId", user.BranchId);
                paramaters.Add("Flag", "Upload_PromotionProposal");
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "EmployeePromotionBusiness", "UploadPromotionProposalAsync", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return executionStatus;
        }
        public async Task<long> GetEmployeeComponentId(string name, string subName, AppUser user, string componentName)
        {
            long id = 0;
            try
            {
                var sp_name = string.Format(@"sp_HR_UploadEmployeePromotionProposal");
                var parameters = new DynamicParameters();
                parameters.Add("Name", name ?? "");
                parameters.Add("SubName", subName ?? "");
                parameters.Add("UserBranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", componentName);

                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    id = await _dapper.SqlQueryFirstAsync<long>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
                    return id;
                }
            }
            catch (Exception ex)
            {
            }
            return id;
        }
        public async Task<string> GetEmployeeComponentText(string name, long? subId, AppUser user, string componentName)
        {
            var text = "";
            try
            {
                var sp_name = string.Format(@"sp_HR_UploadEmployeePromotionProposal");
                var parameters = new DynamicParameters();
                parameters.Add("Name", name ?? "");
                parameters.Add("SubId", subId);
                parameters.Add("UserBranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("Flag", componentName);

                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    text = await _dapper.SqlQueryFirstAsync<string>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
                    return text;
                }
            }
            catch (Exception ex)
            {
            }
            return text;
        }
        public async Task<ExecutionStatus> DeleteEmployeePendingProposalAsync(PromotionProposalCancellationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _employeePromotionProposalRepository.DeletePendingProposalAsync(model, user);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, "Something went wrong");
            }
            return executionStatus;

        }
        public async Task<ExecutionStatus> ApprovalProposalAsync(long id, long employeeId, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _employeePromotionProposalRepository.ApprovalProposalAsync(id, employeeId, user);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Message(false, ResponseMessage.ServerResponsedWithError);
            }
            return executionStatus;
        }

        public async Task<EmployeePromotionProposal> SingleEmployeePendingProposalAsync(long id, long employeeId, AppUser user)
        {
            EmployeePromotionProposal employeePromotionProposal = null;
            try
            {
                employeePromotionProposal = await _employeePromotionProposalRepository.SingleEmployeePendingProposalAsync(id, employeeId, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return employeePromotionProposal;
        }
    }
}
