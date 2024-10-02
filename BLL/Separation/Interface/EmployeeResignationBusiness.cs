using BLL.Base.Interface;
using BLL.Separation.Implementation;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Separation.Models.DTO.Resignation;
using Shared.Separation.Models.Filter.Resignation;
using Shared.Separation.Models.ViewModel.Resignation;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BLL.Separation.Interface
{
    public class EmployeeResignationRequestBusiness : IEmployeeResignationRequestBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public EmployeeResignationRequestBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<DBResponse<EmployeeResignationRequestViewModel>> GetEmployeeResignationsAsync(ResignationRequest_Filter filter, AppUser user)
        {
            DBResponse<EmployeeResignationRequestViewModel> data = new DBResponse<EmployeeResignationRequestViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_HR_EmployeeResignations_List";
                var parameters = DapperParam.AddParams(filter, user);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeResignationRequestViewModel>>(response.JSONData) ?? new List<EmployeeResignationRequestViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "GetEmployeeResignationsAsync", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveResignationRequestAsync(EmployeeResignationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeResignations_Insert_Update_Delete";
                var parameters = DapperParam.AddParams(model, user, new string[] { "GradeId", "DesignationId", "DepartmentId", "IsInterviewDone", "IsApproved", "IsPullBack", "StateStatus" });
                parameters.Add("ExecutionFlag", model.ResignationId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "SaveResignationRequestAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> ValidateResignationAsync(EmployeeResignationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeResignations_Insert_Update_Delete";
                var parameters = Utility.DappperParams(model, user, new string[] { "GradeId", "DesignationId", "DepartmentId", "IsInterviewDone", "IsApproved", "IsPullBack", "StateStatus" });
                parameters.Add("ExecutionFlag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "ValidateResignationAsync", user);
            }
            return executionStatus;
        }
    }
}
