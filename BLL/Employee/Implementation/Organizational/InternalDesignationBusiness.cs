using BLL.Base.Interface;
using Dapper;
using Shared.OtherModels.Pagination;
using System.Data;
using Shared.Services;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.Filter.Organizational;
using Shared.Employee.DTO.Organizational.InternalDesignation;
using Shared.Employee.ViewModel.Organizational.InternalDesignation;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Organizational
{
    public class InternalDesignationBusiness : IInternalDesignationBusiness
    {
        private readonly IDapperData _dapperData;
        private readonly ISysLogger _sysLogger;
        public InternalDesignationBusiness(IDapperData dapperData, ISysLogger sysLogger)
        {
            _dapperData = dapperData;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> SaveInternalDesignationAsync(InternalDesignationDTO designationDTO, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_InternalDesignations";
                var parameters = Utility.DappperParams(designationDTO, user);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("ExecutionFlag", designationDTO.InternalDesignationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "OrganizationalBusiness", "SaveInternalDesignationAsync", user);
            }
            return executionStatus;
        }

        public async Task<DBResponse<InternalDesignationViewModel>> GetInternalDesignationsAsync(InternalDesignation_Filter filter, AppUser user)
        {
            DBResponse<InternalDesignationViewModel> data = new DBResponse<InternalDesignationViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_HR_InternalDesignation_List";
                var parameters = Utility.DappperParams(filter, user, addBaseProperty: true);
                response = await _dapperData.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<InternalDesignationViewModel>>(response.JSONData) ?? new List<InternalDesignationViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "OrganizationalBusiness", "GetInternalDesignationsAsync", user);
            }
            return data;
        }

        public async Task<IEnumerable<InternalDesignationViewModel>> GetInternalDesignationByIdAync(long internalDesignationId, AppUser user)
        {
            IEnumerable<InternalDesignationViewModel> data = new List<InternalDesignationViewModel>();
            try
            {
                var sp_name = "sp_HR_InternalDesignations";
                var parameters = new DynamicParameters();
                parameters.Add("InternalDesignationId", internalDesignationId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("UserId", user.UserId);
                parameters.Add("ExecutionFlag", Data.Read);

                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    data = await _dapperData.SqlQueryListAsync<InternalDesignationViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
                }
            }
            catch (Exception ex)
            {
            }
            return data;
        }
        public async Task<ExecutionStatus> UploadInternalDesignationExcelAsync(List<InternalDesignationDTO> internalDesignationDTOs, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var sp_name = "sp_HR_InternalDesignations";
                var jsonData = Utility.JsonData(internalDesignationDTOs);
                var paramaters = new DynamicParameters();
                paramaters.Add("JsonData", jsonData);
                paramaters.Add("UserId", user.UserId);
                paramaters.Add("CompanyId", user.CompanyId);
                paramaters.Add("OrganizationId", user.OrganizationId);
                paramaters.Add("BranchId", user.BranchId);
                paramaters.Add("ExecutionFlag", "InternalDesignations_Upload");
                if (!Utility.IsNullEmptyOrWhiteSpace(sp_name))
                {
                    executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, paramaters, commandType: CommandType.StoredProcedure);
                    return executionStatus;
                }
                executionStatus = Utility.Invalid(ResponseMessage.InvalidExecution);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<InternalDesignationViewModel>> GetInternalDesignationListAsync(InternalDesignation_Filter filter, AppUser user)
        {
            IEnumerable<InternalDesignationViewModel> list = new List<InternalDesignationViewModel>();
            try
            {
                var sp_name = "sp_HR_Internal_Designations";
                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapperData.SqlQueryListAsync<InternalDesignationViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "OrganizationalBusiness", "GetInternalDesignationListAsync", user);
            }
            return list;
        }
    }
}
