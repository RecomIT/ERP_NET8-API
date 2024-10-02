using Dapper;
using System.Data;
using Shared.Services;
using DAL.DapperObject;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using Shared.Employee.DTO.Stage;
using Shared.Employee.ViewModel.Stage;
using Shared.Employee.Filter.Stage;
using BLL.Employee.Interface.Stage;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Stage
{
    public class EmployeeTransferBusiness : IEmployeeTransferBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public EmployeeTransferBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<DBResponse<EmployeeTransferProposalViewModel>> GetEmployeeTransferProposalsAsync(EmployeeTransfer_Filter query, AppUser user)
        {
            DBResponse<EmployeeTransferProposalViewModel> data = new DBResponse<EmployeeTransferProposalViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_HR_EmployeeTransfer_List";
                var parameters = DapperParam.AddParams(query, user, addBaseProperty: true, addBranch: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeTransferProposalViewModel>>(response.JSONData) ?? new List<EmployeeTransferProposalViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeTransferBusiness", "GetEmployeeTransferProposalsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveTransferProposalAsync(EmployeeTransferProposalDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_TransferProposal_Insert_Update";
                var parameters = DapperParam.AddParams(model, user, addBranch: true);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.ServerResponsedWithError);
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeBusiness", "SaveTransferProposalAsync", user);
            }
            return executionStatus;
        }

        //Added by Monzur 16-SEP-2023
        public async Task<ExecutionStatus> UploadTransferProposalAsync(List<TransferProposalReadExcelDTO> readExcelDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = string.Format(@"sp_HR_UploadEmployeeTransferProposal");
                List<TransferProposalInsertExcelDTO> insertExcelDTOs = new List<TransferProposalInsertExcelDTO>();
                foreach (var item in readExcelDTOs)
                {
                    var proposal = new TransferProposalInsertExcelDTO();
                    proposal.EmployeeCode = item.EmployeeCode;
                    proposal.PrevDepartmentId = await GetEmployeeComponentId(item.EmployeeCode, "", user, "PrevDepartmentId");
                    proposal.PrevDepartmentName = await GetEmployeeComponentText(item.EmployeeCode, proposal.PrevDepartmentId, user, "PrevDepartmentName");
                    proposal.DepartmentId = await GetEmployeeComponentId(item.ProposalText, "", user, "Department");

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
                paramaters.Add("Flag", "Upload_TransferProposal");
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
                await _sysLogger.SaveSystemException(ex, Database.ControlPanel, "EmployeeTransferBusiness", "UploadTransferProposalAsync", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return executionStatus;
        }
        public async Task<long> GetEmployeeComponentId(string name, string subName, AppUser user, string componentName)
        {
            long id = 0;
            try
            {
                var sp_name = string.Format(@"sp_HR_UploadEmployeeTransferProposal");
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
                var sp_name = string.Format(@"sp_HR_UploadEmployeeTransferProposal");
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
    }
}
