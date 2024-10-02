using Dapper;
using System;
using System.Data;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.Services;
using BLL.Base.Interface;
using DAL.DapperObject.Interface;
using BLL.Attendance.Interface.WorkShift;
using BLL.Employee.Interface.Info;
using Shared.Attendance.ViewModel.Workshift;
using Shared.Attendance.DTO.Workshift;
using Shared.Attendance.Filter.Shift;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;

namespace BLL.Attendance.Implementation.WorkShift
{
    public class EmployeeWorkShiftBusiness : IEmployeeWorkShiftBusiness
    {

        private readonly IDapperData _dapperData;
        private readonly ISysLogger _sysLogger;
        private readonly IInfoBusiness _employeeInfoBusiness;

        public EmployeeWorkShiftBusiness(IDapperData dapperData, ISysLogger sysLogger, IInfoBusiness employeeInfoBusiness)
        {
            _dapperData = dapperData;
            _sysLogger = sysLogger;
            _employeeInfoBusiness = employeeInfoBusiness;
        }

        public async Task<ExecutionStatus> EmployeeWorkShiftValidatorAsync(List<EmployeeWorkShiftDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {

            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeWorkShiftBusiness", "EmployeeWorkShiftValidatorAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeesForShiftAssignAsync(EmployeeService_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeServiceDataViewModel> list = new List<EmployeeServiceDataViewModel>();
            try
            {

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeWorkShiftBusiness", "GetEmployeesForShiftAssignAsync", user);
            }
            return list;
        }

        public async Task<EmployeeShiftViewModel> GetEmployeeShiftByIdAysnc(EmployeeShift_Filter filter, AppUser user)
        {
            EmployeeShiftViewModel data = new EmployeeShiftViewModel();
            try
            {
                var sp_name = "sp_HR_EmployeeWorkShift_List";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                parameters.Add("@ExecutionFlag", Data.Read);
                data = await _dapperData.SqlQueryFirstAsync<EmployeeShiftViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeWorkShiftBusiness", "GetEmployeesForShiftAssignAsync", user);
            }
            return data;
        }

        public async Task<IEnumerable<EmployeeWorkShiftViewModel>> GetEmployeeWorkShiftsAsync(EmployeeShift_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeWorkShiftViewModel> data = new List<EmployeeWorkShiftViewModel>();
            try
            {
                var sp_name = "sp_HR_EmployeeWorkShift";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                parameters.Add("Flag", Data.Read);
                data = await _dapperData.SqlQueryListAsync<EmployeeWorkShiftViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeWorkShiftBusiness", "GetEmployeeWorkShiftsAsync", user);
            }
            return data;
        }

        public async Task<ExecutionStatus> SaveEmployeesWorkShiftAsync(List<EmployeeWorkShiftDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeWorkShift";
                var parameter = new DynamicParameters();
                parameter.Add("JsonData", JsonReverseConverter.JsonData(model));
                parameter.Add("CompanyId", user.CompanyId);
                parameter.Add("OrganizationId", user.OrganizationId);
                parameter.Add("UserId", user.ActionUserId);
                parameter.Add("@Flag", "JsonInsert");
                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameter, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeWorkShiftBusiness", "GetEmployeeWorkShiftsAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveEmployeesWorkShiftCheckingAsync(List<EmployeeWorkShiftStatusDTO> model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeWorkShift";
                var JsonData = Utility.JsonData(model);
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user.UserId);
                parameters.Add("@CompanyId", user.CompanyId);
                parameters.Add("@OrganizationId", user.OrganizationId);
                parameters.Add("@Flag", "JsonUpdate");
                var parameter = DapperParam.AddParams(model, user);
                executionStatus = await _dapperData.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameter, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeWorkShiftBusiness", "SaveEmployeesWorkShiftCheckingAsync", user);
            }
            return executionStatus;
        }


    }
}
