using AutoMapper;
using BLL.Base.Interface;
using BLL.Overtime.Interface;
using DAL.DapperObject.Interface;
using Dapper;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Overtime;
using Shared.Overtime.Domain;
using Shared.Overtime.DTO;
using Shared.Overtime.Queries;
using Shared.Overtime.ViewModel;
using Shared.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Overtime.Implementation
{
    public class OvertimeBusiness : IOvertimeBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public OvertimeBusiness(IDapperData dapper, IMapper mapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        // Employee Detail
        public async Task<EmployeeDTO> GetEmployeeDetailsById(long empId, AppUser user)
        {
            EmployeeDTO data = new EmployeeDTO();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetails();
                sqlQuery += $@"WHERE e.EmployeeId = @EmployeeId";
                data = await _dapper.SqlQueryFirstAsync<EmployeeDTO>(user.Database, sqlQuery, new { EmployeeId = empId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetEmployeeDetailsById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<EmployeeDTO> GetEmployeeDetailsByCode(string employeeCode, AppUser user)
        {
            EmployeeDTO data = new EmployeeDTO();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetails();
                sqlQuery += $@"WHERE e.EmployeeCode = @employeeCode";
                data = await _dapper.SqlQueryFirstAsync<EmployeeDTO>(user.Database, sqlQuery, new { employeeCode }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetEmployeeDetailsByCode", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }


        // Overtime Approval Level

        public async Task<IEnumerable<OvertimeApprovalLevel>> GetAllOvertimeApprovalLevel(AppUser user)
        {
            IEnumerable<OvertimeApprovalLevel> data = new List<OvertimeApprovalLevel>();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeApprovalLevel");
                data = await _dapper.SqlQueryListAsync<OvertimeApprovalLevel>(user.Database, sqlQuery, new { }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetAllOvertimeApprovalLevel", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<OvertimeApprovalLevel> GetOvertimeApprovalLevelById(long overtimeApprovalLevelId, AppUser user)
        {
            OvertimeApprovalLevel data = new OvertimeApprovalLevel();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeApprovalLevel");
                sqlQuery += $"WHERE OvertimeApprovalLevelId = @overtimeApprovalLevelId";

                data = await _dapper.SqlQueryFirstAsync<OvertimeApprovalLevel>(user.Database, sqlQuery, new { overtimeApprovalLevelId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeApprovalLevelById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveOvertimeApprovalLevel(OvertimeApprovalLevel overtimeApprovalLevel, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            try
            {
                var parameters = Utility.DappperParamsInKeyValuePairs(overtimeApprovalLevel, user, addBranch: true, addUserId: false);
                parameters.Remove("OvertimeApprovalLevelId");
                parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                parameters.Add("CreatedDate", DateTime.Now);

                sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeApprovalLevel", paramkeys: parameters.Select(x => x.Key).ToList());

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameters, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "SaveOvertimeApprovalLevel", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> UpdateOvertimeApprovalLevel(OvertimeApprovalLevel overtimeApprovalLevel, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            try
            {
                var parameters = Utility.DappperParamsInKeyValuePairs(overtimeApprovalLevel, user, addBranch: true, addUserId: false);
                parameters.Remove("OvertimeApprovalLevelId");
                parameters.Add("UpdatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                parameters.Add("UpdatedDate", DateTime.Now);

                sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimeApprovalLevel", paramkeys: parameters.Select(x => x.Key).ToList());
                sqlQuery += $"WHERE OvertimeApprovalLevelId = @OvertimeApprovalLevelId";
                parameters.Add("OvertimeApprovalLevelId", overtimeApprovalLevel.OvertimeApprovalLevelId);

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameters, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "UpdateOvertimeApprovalLevel", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> DeleteOvertimeApprovalLevel(long overtimeApprovalLevelId, AppUser user)
        {
            ExecutionStatus execution = new();

            try
            {
                string sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeApprovalLevel");
                sqlQuery += $"WHERE OvertimeApprovalLevelId = @overtimeApprovalLevelId";

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, new { overtimeApprovalLevelId }, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been Deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "DeleteOvertimeApprovalLevel", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }


        // Overtime Approver Assignment

        public async Task<IEnumerable<OvertimeApproverDTO>> GetEmployeesForOvertimeApprover(AppUser user)
        {
            IEnumerable<OvertimeApproverDTO> data = new List<OvertimeApproverDTO>();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeApprover();
                sqlQuery += $@"WHERE e.IsActive = @IsActive AND  ot.EmployeeId IS NULL";

                data = await _dapper.SqlQueryListAsync<OvertimeApproverDTO>(user.Database, sqlQuery, new { IsActive = 1 }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetEmployeesForOvertimeApprover", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<IEnumerable<OvertimeApproverDTO>> GetAllOvertimeApprover(AppUser user)
        {
            IEnumerable<OvertimeApproverDTO> data = new List<OvertimeApproverDTO>();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeApprover();
                sqlQuery += $@"WHERE e.IsActive = @IsActive AND ot.EmployeeId IS NOT NULL"; // AND ot.IsActive = @IsActiveApprover
                data = await _dapper.SqlQueryListAsync<OvertimeApproverDTO>(user.Database, sqlQuery, new { IsActive = 1 }, CommandType.Text); //, IsActiveApprover = 1
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetAllOvertimeApprover", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<OvertimeApproverDTO> GetOvertimeApproverByApproverId(long overtimeApproverId, AppUser user)
        {
            OvertimeApproverDTO data = new OvertimeApproverDTO();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeApprover();
                sqlQuery += $@"WHERE e.IsActive = @IsActive AND ot.OvertimeApproverId = @overtimeApproverId"; //AND  ot.IsActive = @IsActiveApprover
                data = await _dapper.SqlQueryFirstAsync<OvertimeApproverDTO>(user.Database, sqlQuery, new { IsActive = 1, overtimeApproverId }, CommandType.Text);//IsActiveApprover = 1,
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeApproverById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<IEnumerable<OvertimeApproverDTO>> GetOvertimeApproversByApproverIds(List<long> overtimeApproverIds, AppUser user)
        {
            IEnumerable<OvertimeApproverDTO> data = new List<OvertimeApproverDTO>();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeApprover();
                sqlQuery += $@"WHERE e.IsActive = @IsActive AND  ot.OvertimeApproverId IN @ids"; //ot.IsActive = @IsActiveApprover AND
                data = await _dapper.SqlQueryListAsync<OvertimeApproverDTO>(user.Database, sqlQuery, new { IsActive = 1, ids = overtimeApproverIds }, CommandType.Text); //IsActiveApprover = 1,
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeApproversByApproverIds", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<OvertimeApproverDTO> GetOvertimeApproverByEmployeeId(long employeeId, AppUser user)
        {
            OvertimeApproverDTO data = new OvertimeApproverDTO();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeApprover();
                sqlQuery += $@"WHERE e.IsActive = @IsActive AND  ot.EmployeeId IS NOT NULL AND e.EmployeeId = @employeeId"; //ot.IsActive = @IsActiveApprover AND
                data = await _dapper.SqlQueryFirstAsync<OvertimeApproverDTO>(user.Database, sqlQuery, new { IsActive = 1, employeeId }, CommandType.Text); //IsActiveApprover = 1,
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeApproverByEmployeeId", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveOvertimeApprover(List<OvertimeApprover> overtimeApproverList, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            List<Dictionary<string, dynamic>> parameterList = new();

            try
            {

                foreach (var overtimeApprover in overtimeApproverList)
                {
                    var parameters = Utility.DappperParamsInKeyValuePairs(overtimeApprover, user, addBranch: true, addUserId: false);
                    parameters.Remove("OvertimeApproverId");
                    parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                    parameters.Add("CreatedDate", DateTime.Now);
                    parameterList.Add(parameters);
                }

                sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeApprover", paramkeys: parameterList.First().Select(x => x.Key).ToList());

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameterList, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "SaveOvertimeApprover", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> UpdateOvertimeApprover(OvertimeApprover overtimeApprover, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            try
            {
                var parameters = Utility.DappperParamsInKeyValuePairs(overtimeApprover, user, addBranch: true, addUserId: false);
                parameters.Remove("OvertimeApproverId");
                parameters.Add("UpdatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                parameters.Add("UpdatedDate", DateTime.Now);

                sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimeApprover", paramkeys: parameters.Select(x => x.Key).ToList());
                sqlQuery += $"WHERE OvertimeApproverId = @OvertimeApproverId";
                parameters.Add("OvertimeApproverId", overtimeApprover.OvertimeApproverId);

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameters, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "UpdateOvertimeApprover", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> DeleteOvertimeApprover(long overtimeApproverId, AppUser user)
        {
            ExecutionStatus execution = new();

            try
            {
                string sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeApprover");
                sqlQuery += $"WHERE OvertimeApproverId = @overtimeApproverId";

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, new { overtimeApproverId }, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been Deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "DeleteOvertimeApprover", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }


        // Overtime Team Approval Mapping

        // Get All Active Employess from Assigning to a Approver (Team Approver);
        public async Task<IEnumerable<OvertimeEmployeeDTO>> GetEmployeesForOvertimeTeamApprovalMapping(AppUser user)
        {
            IEnumerable<OvertimeEmployeeDTO> data = new List<OvertimeEmployeeDTO>();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeTeamApprovalMapping();
                sqlQuery += $@"WHERE e.IsActive = @IsActive ";

                data = await _dapper.SqlQueryListAsync<OvertimeEmployeeDTO>(user.Database, sqlQuery, new { IsActive = 1 }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetEmployeesForOvertimeTeamApprovalMapping", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;

        }

        // Get All Active Employess from Assigning to a Approver(Team Approver);
        public async Task<IEnumerable<OvertimeEmployeeDTO>> GetAllOvertimeTeamMembersApprovalMapping(AppUser user)
        {
            IEnumerable<OvertimeEmployeeDTO> data = new List<OvertimeEmployeeDTO>();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeTeamApprovalMapping(columns: @", ISNULL(ot.OvertimeApproverId,0) AS 'OvertimeApproverId' ,
                                                                                                             ISNULL(ot.OvertimeTeamApprovalMappingId,0) AS 'OvertimeTeamApprovalMappingId',
                                                                                                             ISNULL(ot.ApprovalLevel, 0) AS 'ApprovalLevel' , ot.CreatedDate, ot.UpdatedDate");
                sqlQuery += $@"LEFT JOIN dbo.Payroll_OvertimeTeamApprovalMapping ot
                               ON ot.EmployeeId = e.EmployeeId
                               WHERE e.IsActive = @IsActive AND ot.OvertimeApproverId IS NOT NULL";

                data = await _dapper.SqlQueryListAsync<OvertimeEmployeeDTO>(user.Database, sqlQuery, new { IsActive = 1 }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetAllOvertimeTeamMembersApprovalMapping", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;

        }
        public async Task<OvertimeEmployeeDTO> GetOvertimeTeamMemberApprovalMappingById(long overtimeTeamApprovalMappingId, AppUser user)
        {
            OvertimeEmployeeDTO data = new();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeTeamApprovalMapping(columns: @", ISNULL(ot.OvertimeApproverId,0) AS 'OvertimeApproverId' ,
                                                                                                             ISNULL(ot.OvertimeTeamApprovalMappingId,0) AS 'OvertimeTeamApprovalMappingId',
                                                                                                             ISNULL(ot.ApprovalLevel, 0) AS 'ApprovalLevel' , ot.CreatedDate, ot.UpdatedDate");
                sqlQuery += $@"LEFT JOIN dbo.Payroll_OvertimeTeamApprovalMapping ot
                               ON ot.EmployeeId = e.EmployeeId
                               WHERE e.IsActive = @IsActive AND ot.OvertimeApproverId IS NOT NULL AND ot.OvertimeTeamApprovalMappingId = @overtimeTeamApprovalMappingId";

                data = await _dapper.SqlQueryFirstAsync<OvertimeEmployeeDTO>(user.Database, sqlQuery, new { IsActive = 1, overtimeTeamApprovalMappingId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeTeamMembersApprovalMappingById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;

        }
        public async Task<IEnumerable<OvertimeEmployeeDTO>> GetOvertimeTeamMembersApprovalMappingByTeamMemberId(long teamMemberId, AppUser user)
        {
            IEnumerable<OvertimeEmployeeDTO> data = new List<OvertimeEmployeeDTO>();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeTeamApprovalMapping(columns: @", ISNULL(ot.OvertimeApproverId,0) AS 'OvertimeApproverId' ,
                                                                                                             ISNULL(ot.OvertimeTeamApprovalMappingId,0) AS 'OvertimeTeamApprovalMappingId' ,
                                                                                                             ISNULL(ot.ApprovalLevel, 0) AS 'ApprovalLevel' , ot.CreatedDate, ot.UpdatedDate");
                sqlQuery += $@"LEFT JOIN dbo.Payroll_OvertimeTeamApprovalMapping ot
                               ON ot.EmployeeId = e.EmployeeId
                               WHERE e.IsActive = @IsActive AND ot.OvertimeApproverId IS NOT NULL AND e.EmployeeId = @teamMemberId";

                data = await _dapper.SqlQueryListAsync<OvertimeEmployeeDTO>(user.Database, sqlQuery, new { IsActive = 1, teamMemberId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeApproverByTeamMemberId", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;

        }
        public async Task<IEnumerable<OvertimeEmployeeDTO>> GetOvertimeTeamMembersByApproverId(long overtimeApproverId, AppUser user)
        {
            IEnumerable<OvertimeEmployeeDTO> data = new List<OvertimeEmployeeDTO>();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeTeamApprovalMapping(columns: @", ISNULL(ot.OvertimeApproverId,0) AS 'OvertimeApproverId' ,
                                                                                                             ISNULL(ot.OvertimeTeamApprovalMappingId,0) AS 'OvertimeTeamApprovalMappingId' ,
                                                                                                             ISNULL(ot.ApprovalLevel, 0) AS 'ApprovalLevel' , ot.CreatedDate, ot.UpdatedDate");

                sqlQuery += $@"LEFT JOIN dbo.Payroll_OvertimeTeamApprovalMapping ot
                               ON ot.EmployeeId = e.EmployeeId
                               WHERE e.IsActive = @IsActive AND ot.OvertimeApproverId = @overtimeApproverId";

                data = await _dapper.SqlQueryListAsync<OvertimeEmployeeDTO>(user.Database, sqlQuery, new { IsActive = 1, overtimeApproverId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeTeamMembersByApproverBy", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;

        }
        public async Task<OvertimeEmployeeDTO> GetOvertimeTeamMemberByApprovalMappingId(long overtimeTeamApprovalMappingId, AppUser user)
        {
            OvertimeEmployeeDTO data = new OvertimeEmployeeDTO();

            try
            {
                string sqlQuery = OvertimeQueries.EmployeeDetailsForOvertimeTeamApprovalMapping(columns: @", ISNULL(ot.OvertimeApproverId,0) AS 'OvertimeApproverId' ,
                                                                                                             ISNULL(ot.OvertimeTeamApprovalMappingId,0) AS 'OvertimeTeamApprovalMappingId' ,
                                                                                                             ISNULL(ot.ApprovalLevel, 0) AS 'ApprovalLevel' , ot.CreatedDate, ot.UpdatedDate");
                sqlQuery += $@"LEFT JOIN dbo.Payroll_OvertimeTeamApprovalMapping ot
                               ON ot.EmployeeId = e.EmployeeId
                               WHERE  ot.OvertimeTeamApprovalMappingId = @overtimeTeamApprovalMappingId";

                data = await _dapper.SqlQueryFirstAsync<OvertimeEmployeeDTO>(user.Database, sqlQuery, new { overtimeTeamApprovalMappingId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeTeamMemberByApprovalMappingId", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;

        }
        public async Task<ExecutionStatus> SaveOvertimeTeamMembersToApprover(List<OvertimeTeamApprovalMapping> overtimeTeamMemberList, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            List<Dictionary<string, dynamic>> parameterList = new();

            try
            {

                foreach (var teamMember in overtimeTeamMemberList)
                {
                    var parameters = Utility.DappperParamsInKeyValuePairs(teamMember, user, addBranch: true, addUserId: false);
                    parameters.Remove("OvertimeTeamApprovalMappingId");
                    parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                    parameters.Add("CreatedDate", DateTime.Now);
                    parameterList.Add(parameters);
                }

                sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeTeamApprovalMapping", paramkeys: parameterList.First().Select(x => x.Key).ToList());

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameterList, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "SaveOvertimeTeamMembersToApprover", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> DeleteOvertimeTeamMemberByApprovalMappingId(long overtimeTeamApprovalMappingId, AppUser user)
        {
            ExecutionStatus execution = new();

            try
            {
                string sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeTeamApprovalMapping");
                sqlQuery += $"WHERE OvertimeTeamApprovalMappingId = @overtimeTeamApprovalMappingId";

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, new { overtimeTeamApprovalMappingId }, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been Deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "DeleteOvertimeTeamMemberApprovalMapping", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> DeleteOvertimeTeamMemberByApproverId(long overtimeApproverId, AppUser user)
        {
            ExecutionStatus execution = new();

            try
            {
                string sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeTeamApprovalMapping");
                sqlQuery += $"WHERE OvertimeApproverId = @overtimeApproverId";

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, new { overtimeApproverId }, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been Deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "DeleteOvertimeTeamMemberByApproverId", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> UpdateOvertimeTeamApprovalMappingLevel(long overtimeTeamApprovalMappingId, int level, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            try
            {
                var parameters = new Dictionary<string, dynamic> {
                    { "ApprovalLevel", level },
                    { "UpdatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId },
                    { "UpdatedDate", DateTime.Now }
                };

                //sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimeTeamApprovalMapping", paramkeys: parameters.Select(x => x.Key).ToList());
                //sqlQuery += $"WHERE OvertimeApprovalLevelId = @OvertimeApprovalLevelId";
                //parameters.Add("OvertimeApprovalLevelId", overtimeApprovalLevel.OvertimeApprovalLevelId);

                sqlQuery = sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimeTeamApprovalMapping", paramkeys: parameters.Select(x => x.Key).ToList());
                sqlQuery += $"WHERE OvertimeTeamApprovalMappingId = @OvertimeTeamApprovalMappingId";
                parameters.Add("OvertimeTeamApprovalMappingId", overtimeTeamApprovalMappingId);

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameters, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "UpdateOvertimeTeamApprovalMappingLevel", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }


        // Overtime Policy

        public async Task<IEnumerable<OvertimePolicy>> GetAllOvertimePolicy(AppUser user)
        {
            IEnumerable<OvertimePolicy> data = new List<OvertimePolicy>();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimePolicy");
                data = await _dapper.SqlQueryListAsync<OvertimePolicy>(user.Database, sqlQuery, new { }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetAllOvertimePolicy", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<OvertimePolicy> GetOvertimePolicyById(long overtimeId, AppUser user)
        {
            OvertimePolicy data = new OvertimePolicy();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimePolicy");
                sqlQuery += $"WHERE OvertimeId = @overtimeId";
                data = await _dapper.SqlQueryFirstAsync<OvertimePolicy>(user.Database, sqlQuery, new { overtimeId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimePolicyById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveOvertimePolicy(OvertimePolicy overtimePolicy, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            try
            {
                var parameters = Utility.DappperParamsInKeyValuePairs(overtimePolicy, user, addBranch: true, addUserId: false);
                parameters.Remove("OvertimeId");
                parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                parameters.Add("CreatedDate", DateTime.Now);

                sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimePolicy", paramkeys: parameters.Select(x => x.Key).ToList());

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameters, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "SaveOvertimePolicy", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> UpdateOvertimePolicy(OvertimePolicy overtimePolicy, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            try
            {
                var parameters = Utility.DappperParamsInKeyValuePairs(overtimePolicy, user, addBranch: true, addUserId: false);
                parameters.Remove("OvertimeId");
                parameters.Add("UpdatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                parameters.Add("UpdatedDate", DateTime.Now);

                sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimePolicy", paramkeys: parameters.Select(x => x.Key).ToList());
                sqlQuery += $"WHERE OvertimeId = @OvertimeId";
                parameters.Add("OvertimeId", overtimePolicy.OvertimeId);

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameters, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "UpdateOvertimePolicy", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> DeleteOvertimePolicy(long overtimeId, AppUser user)
        {
            ExecutionStatus execution = new();

            try
            {
                string sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimePolicy");
                sqlQuery += $"WHERE OvertimeId = @overtimeId";

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, new { overtimeId }, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been Deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "DeleteOvertimePolicy", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<IEnumerable<OvertimePolicy>> GetOvertimePolicyBySpecification(string column, dynamic value, AppUser user)
        {
            IEnumerable<OvertimePolicy> data = new List<OvertimePolicy>();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimePolicy");
                sqlQuery += $"WHERE {column} = @value";
                data = await _dapper.SqlQueryListAsync<OvertimePolicy>(user.Database, sqlQuery, new { value }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimePolicyBySpecification", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }


        // Overtime Request 

        public async Task<IEnumerable<OvertimeRequest>> GetAllOvertimeRequest(string status, AppUser user)
        {

            var overtimeRequests = new List<OvertimeRequest>();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequest");

                if (!string.IsNullOrWhiteSpace(status))
                {
                    sqlQuery += " WHERE Status = @status;";
                }

                sqlQuery += Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequestDetails");

                using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
                {

                    connection.Open();
                    using (var resultSets = await connection.QueryMultipleAsync(sqlQuery, new { status }))
                    {
                        var requests = await resultSets.ReadAsync<OvertimeRequest>();
                        var requestDetails = await resultSets.ReadAsync<OvertimeRequestDetails>();

                        if (requests.Any())
                        {
                            foreach (var req in requests)
                            {
                                req.OvertimeRequestDetails = requestDetails.Where(x => x.OvertimeRequestId == req.OvertimeRequestId).ToList();
                                overtimeRequests.Add(req);
                            }
                        }
                    }
                }//con

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetAllOvertimeRequest", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return overtimeRequests;
        }
        public async Task<IEnumerable<OvertimeRequest>> GetAllOvertimeRequestForEmployee(long employeeId, string status, AppUser user)
        {

            IEnumerable<OvertimeRequest> overtimeRequests = new List<OvertimeRequest>();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequest");
                sqlQuery += $"WHERE EmployeeId = @EmployeeId ";

                if (!string.IsNullOrWhiteSpace(status))
                {
                    sqlQuery += " AND Status = @status ;";
                }

                overtimeRequests = await _dapper.SqlQueryListAsync<OvertimeRequest>(user.Database, sqlQuery, new { EmployeeId = employeeId, status }, CommandType.Text);

                if (overtimeRequests.Any())
                {

                    foreach (var req in overtimeRequests)
                    {

                        sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequestDetails");
                        sqlQuery += $"WHERE OvertimeRequestID = @OvertimeRequestID;";

                        var reqDetails = await _dapper.SqlQueryListAsync<OvertimeRequestDetails>(user.Database, sqlQuery, new { OvertimeRequestID = req.OvertimeRequestId }, CommandType.Text);
                        if (reqDetails.Any())
                        {
                            req.OvertimeRequestDetails = reqDetails.ToList();
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetAllOvertimeRequest", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return overtimeRequests;
        }
        public async Task<IEnumerable<OvertimeRequest>> GetAllOvertimeRequestForApprover(long overtimeApproverId, string status, AppUser user)
        {

            var overtimeRequests = new List<OvertimeRequest>();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequestDetails");
                sqlQuery += $"WHERE OvertimeApproverId = @OvertimeApproverId AND ActionRequired = @ActionRequired ";

                if (!string.IsNullOrWhiteSpace(status))
                {
                    sqlQuery += " AND Status = @status ;";
                }

                var reqDetailList = await _dapper.SqlQueryListAsync<OvertimeRequestDetails>(user.Database, sqlQuery, new { OvertimeApproverId = overtimeApproverId, status, ActionRequired = 1 }, CommandType.Text);

                if (reqDetailList.Any())
                {

                    foreach (var reqDetails in reqDetailList)
                    {

                        sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequest");
                        sqlQuery += $"WHERE OvertimeRequestId = @OvertimeRequestId;";
                        sqlQuery += Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequestDetails");
                        sqlQuery += $"WHERE OvertimeRequestId = @OvertimeRequestId;";

                        using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
                        {

                            connection.Open();
                            using (var resultSets = await connection.QueryMultipleAsync(sqlQuery, new { reqDetails.OvertimeRequestId }))
                            {
                                var request = await resultSets.ReadFirstOrDefaultAsync<OvertimeRequest>();
                                var requestDetails = await resultSets.ReadAsync<OvertimeRequestDetails>();

                                if (request != null)
                                {
                                    request.OvertimeRequestDetails = requestDetails.ToList();
                                    overtimeRequests.Add(request);
                                }
                            }
                        }//con

                        //sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequest");
                        //sqlQuery += $"WHERE OvertimeRequestId = @OvertimeRequestId;";

                        //var request = await _dapper.SqlQueryFirstAsync<OvertimeRequest>(user.Database, sqlQuery, new { reqDetails.OvertimeRequestId }, CommandType.Text);

                        //if (request != null) {

                        //    sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequestDetails");
                        //    sqlQuery += $"WHERE OvertimeRequestID = @OvertimeRequestID;";

                        //    var requestDetails = await _dapper.SqlQueryListAsync<OvertimeRequestDetails>(user.Database, sqlQuery, new { OvertimeRequestID = request.OvertimeRequestId }, CommandType.Text);

                        //    if (requestDetails.Any()) {
                        //        request.OvertimeRequestDetails = requestDetails.ToList();
                        //    }

                        //}

                        //overtimeRequests.Add(request);
                    }

                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetAllOvertimeRequestForApprover", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return overtimeRequests;
        }
        public async Task<OvertimeRequest> GetOvertimeRequestById(long overtimeRequestId, AppUser user)
        {
            OvertimeRequest overtimeRequest = new();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequest");
                sqlQuery += $"WHERE OvertimeRequestID = @OvertimeRequestID; ";
                sqlQuery += Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequestDetails");
                sqlQuery += $"WHERE OvertimeRequestID = @OvertimeRequestID; ";

                using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
                {

                    connection.Open();
                    using (var resultSets = await connection.QueryMultipleAsync(sqlQuery, new { OvertimeRequestID = overtimeRequestId }))
                    {
                        var req = await resultSets.ReadFirstOrDefaultAsync<OvertimeRequest>();
                        var reqDetails = await resultSets.ReadAsync<OvertimeRequestDetails>();

                        if (req != null)
                        {
                            overtimeRequest = req;
                            overtimeRequest.OvertimeRequestDetails = reqDetails.ToList();
                        }
                        else
                        {
                            overtimeRequest = null;
                        }
                    }

                }//con

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeRequestById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return overtimeRequest;
        }
        public async Task<ExecutionStatus> SaveOvertimeRequest(OvertimeRequest overtimeRequest, AppUser user)
        {
            ExecutionStatus execution = new();

            using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
            {

                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {

                    string sqlQuery = string.Empty;

                    try
                    {
                        var parameters = Utility.DappperParamsInKeyValuePairs(overtimeRequest, user, addBranch: true, addUserId: false);
                        parameters.Remove("OvertimeRequestId");
                        parameters.Remove("OvertimeRequestDetails");
                        parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                        parameters.Add("CreatedDate", DateTime.Now);

                        sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeRequest", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");//.OvertimeRequestId

                        var insertedRequest = await connection.QueryFirstOrDefaultAsync<OvertimeRequest>(sqlQuery, parameters, transaction);

                        if (insertedRequest != null)
                        {

                            List<Dictionary<string, dynamic>> parameterList = new();
                            parameters.Clear();

                            foreach (var overtimeRequestDetail in overtimeRequest.OvertimeRequestDetails)
                            {
                                overtimeRequestDetail.OvertimeRequestId = insertedRequest.OvertimeRequestId;
                                parameters = Utility.DappperParamsInKeyValuePairs(overtimeRequestDetail, appUser: null);
                                parameters.Remove("OvertimeRequestDetailsId");
                                parameterList.Add(parameters);
                            }

                            sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeRequestDetails", paramkeys: parameters.Select(x => x.Key).ToList());

                            int rawAffected = await connection.ExecuteAsync(sqlQuery, parameterList, transaction);

                            if (rawAffected > 0)
                            {
                                execution.Status = true;
                                execution.Msg = "Data has been saved successfully.";
                                transaction.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                        await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "SaveOvertimeRequest", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
                    }
                    finally { connection.Close(); }
                }

                return execution;
            }
        }
        public async Task<ExecutionStatus> UpdateOvertimeRequest(OvertimeRequest overtimeRequest, AppUser user)
        {
            ExecutionStatus execution = new();

            using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
            {

                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {

                    string sqlQuery = string.Empty;

                    try
                    {
                        var parameters = Utility.DappperParamsInKeyValuePairs(overtimeRequest, user, addBranch: true, addUserId: false);
                        parameters.Remove("OvertimeRequestId");
                        parameters.Remove("OvertimeRequestDetails");
                        parameters.Add("UpdatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                        parameters.Add("UpdatedDate", DateTime.Now);

                        sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimeRequest", paramkeys: parameters.Select(x => x.Key).ToList());

                        sqlQuery += $"WHERE OvertimeRequestId = @OvertimeRequestId";
                        parameters.Add("OvertimeRequestId", overtimeRequest.OvertimeRequestId);

                        int rawAffected = await connection.ExecuteAsync(sqlQuery, parameters, transaction);

                        if (rawAffected > 0)
                        {

                            parameters.Clear();

                            List<Dictionary<string, dynamic>> parameterList = new();
                            parameters.Clear();

                            foreach (var overtimeRequestDetail in overtimeRequest.OvertimeRequestDetails.Where(x => x.IsReverted == false).ToList())
                            {

                                overtimeRequestDetail.OvertimeRequestId = overtimeRequest.OvertimeRequestId;
                                parameters = Utility.DappperParamsInKeyValuePairs(overtimeRequestDetail, appUser: null);
                                parameters.Remove("OvertimeRequestDetailsId");
                                parameterList.Add(parameters);
                            }

                            sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeRequestDetails", paramkeys: parameters.Select(x => x.Key).ToList());

                            int rawAffectedforDetails = await connection.ExecuteAsync(sqlQuery, parameterList, transaction);

                            if (rawAffectedforDetails > 0)
                            {
                                execution.Status = true;
                                execution.Msg = "Data has been Updated successfully.";

                            }

                            transaction.Commit();
                        }
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                        await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "UpdateOvertimeRequest", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
                    }
                    finally { connection.Close(); }
                }

                return execution;
            }
        }
        public async Task<OvertimeRequest> CheckOvertimeRequestAlreadyExist(SaveOvertimeRequestDTO overtimeRequest, AppUser user)
        {

            OvertimeRequest data = new();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequest");
                sqlQuery += $@" WHERE EmployeeId = @EmployeeId AND OvertimeId = @OvertimeId AND RequestDate = @RequestDate";

                data = await _dapper.SqlQueryFirstAsync<OvertimeRequest>(user.Database, sqlQuery, new { overtimeRequest.EmployeeId, overtimeRequest.OvertimeId, overtimeRequest.RequestDate }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "CheckOvertimeRequestAlreadyExist", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<ExecutionStatus> OvertimeRequestApprovalAction(OvertimeRequest overtimeRequest, AppUser user)
        {
            ExecutionStatus execution = new();

            using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
            {

                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {

                    string sqlQuery = string.Empty;

                    try
                    {
                        var parameters = Utility.DappperParamsInKeyValuePairs(overtimeRequest, user, addBranch: true, addUserId: false);
                        parameters.Remove("OvertimeRequestId");
                        parameters.Remove("OvertimeRequestDetails");
                        //parameters.Add("UpdatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                        //parameters.Add("UpdatedDate", DateTime.Now);

                        sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimeRequest", paramkeys: parameters.Select(x => x.Key).ToList());

                        sqlQuery += $"WHERE OvertimeRequestId = @OvertimeRequestId";
                        parameters.Add("OvertimeRequestId", overtimeRequest.OvertimeRequestId);

                        int rawAffected = await connection.ExecuteAsync(sqlQuery, parameters, transaction);
                        int rawAffectedforDetails = 0;

                        if (rawAffected > 0)
                        {

                            parameters.Clear();

                            foreach (var overtimeRequestDetail in overtimeRequest.OvertimeRequestDetails)
                            {

                                parameters = Utility.DappperParamsInKeyValuePairs(overtimeRequestDetail, appUser: null);
                                parameters.Remove("OvertimeRequestDetailsId");

                                sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimeRequestDetails", paramkeys: parameters.Select(x => x.Key).ToList());
                                sqlQuery += $"WHERE OvertimeRequestDetailsId = @OvertimeRequestDetailsId";
                                parameters.Add("OvertimeRequestDetailsId", overtimeRequestDetail.OvertimeRequestDetailsId);

                                rawAffectedforDetails += await connection.ExecuteAsync(sqlQuery, parameters, transaction);
                            }

                            if (rawAffectedforDetails == overtimeRequest.OvertimeRequestDetails.Count)
                            {
                                execution.Status = true;
                                execution.Msg = "Data has been Updated successfully.";

                            }

                            transaction.Commit();
                        }

                    }

                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                        await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "SaveOvertimeRequest", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
                    }

                    finally { connection.Close(); }
                }

                return execution;
            }
        }
        public async Task<ExecutionStatus> DeleteOvertimeRequesById(long overtimeRequestId, AppUser user)
        {

            ExecutionStatus execution = new();

            using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
            {

                connection.Open();

                using (var transaction = connection.BeginTransaction())
                {

                    string sqlQuery = string.Empty;

                    try
                    {

                        sqlQuery += Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeRequestDetails");
                        sqlQuery += $"WHERE OvertimeRequestID = @OvertimeRequestID; ";

                        int rawAffectedForDetails = await connection.ExecuteAsync(sqlQuery, new { OvertimeRequestID = overtimeRequestId }, transaction);

                        if (rawAffectedForDetails > 0)
                        {

                            sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeRequest");
                            sqlQuery += $"WHERE OvertimeRequestID = @OvertimeRequestID";

                            int rawAffected = await connection.ExecuteAsync(sqlQuery, new { OvertimeRequestID = overtimeRequestId }, transaction);

                            if (rawAffected > 0)
                            {
                                execution.Status = true;
                                execution.Msg = "Data has been Deleted successfully.";
                                transaction.Commit();
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                        await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "DeleteOvertimeRequesById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
                    }
                    finally { connection.Close(); }
                }

                return execution;
            }
        }


        // Overtime Process

        public async Task<IEnumerable<OvertimeProcess>> GetAllOvertimeProcess(AppUser user)
        {
            IEnumerable<OvertimeProcess> data = new List<OvertimeProcess>();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeProcess");
                data = await _dapper.SqlQueryListAsync<OvertimeProcess>(user.Database, sqlQuery, new { }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetAllOvertimeProcess", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<OvertimeProcess> GetOvertimeProcessById(long overtimeProcessId, AppUser user)
        {

            OvertimeProcess data = new OvertimeProcess();

            try
            {
                string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeProcess");
                sqlQuery += $"WHERE Id = @overtimeProcessId";

                data = await _dapper.SqlQueryFirstAsync<OvertimeProcess>(user.Database, sqlQuery, new { overtimeProcessId }, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "GetOvertimeProcessById", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return data;
        }
        public async Task<ExecutionStatus> OvertimeProcess(OvertimeProcessViewModel model, AppUser user)
        {
            ExecutionStatus execution = new();

            var proStartDate = new DateTime(model.Year, model.Month, 1);
            var daysInMonth = DateTime.DaysInMonth(model.Year, model.Month);
            DateTime proEndDate = new DateTime(model.Year, model.Month, daysInMonth);
            DateTime salaryMonth = proEndDate;

            var overtimeProcess = new OvertimeProcess();
            List<OvertimeAllowances> overtimeAllowanceList = new();

            using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
            {

                connection.Open();

                try
                {

                    string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeProcess");
                    sqlQuery += $"WHERE YEAR(SalaryMonth) = @Year AND MONTH(SalaryMonth) = @Month;";

                    overtimeProcess = await connection.QueryFirstOrDefaultAsync<OvertimeProcess>(sqlQuery, new { model.Year, model.Month });

                    /// 1. Checking for  Overtime Process Existance

                    if (overtimeProcess != null)
                    {
                        execution.Status = false;
                        execution.Msg = $"Overtime is already Processed for the Salary Month {salaryMonth.ToString("dd MMM yyyy")}";
                        connection.Close();
                        return execution;
                    }

                    /// 2. Get Overtime Requests and Uploads to Calculate Amount
                    sqlQuery = string.Empty;
                    sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimeRequest");
                    sqlQuery += " WHERE Status = @status AND  RequestDate BETWEEN @startDate AND @endDate;";

                    var allIndividualRequest = await connection.QueryAsync<OvertimeRequest>(sqlQuery, new { status = "Approved", startDate = proStartDate, endDate = proEndDate }); //Approved


                    sqlQuery = string.Empty;
                    sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_UploadOvertimeAllowances");
                    sqlQuery += " WHERE SalaryMonth BETWEEN @startDate AND @endDate;";
                    var allUploadRequest = await connection.QueryAsync<UploadOvertimeAllowances>(sqlQuery, new { startDate = proStartDate, endDate = proEndDate });

                    if (!allIndividualRequest.Any() && !allUploadRequest.Any())
                    {

                        execution.Status = false;
                        execution.Msg = $"Overtime Process Unsuccessfull. No Individual Request or Upload Overtime Allowance Found for the Salary Month {salaryMonth.ToString("dd MMM yyyy")}";
                        connection.Close();
                        return execution;
                    }

                    using (var transaction = connection.BeginTransaction())
                    {

                        try
                        {

                            /// Create Overtime Process Object

                            overtimeProcess = new OvertimeProcess();
                            overtimeProcess.ProcessDate = model.ProcessDate.Date;
                            overtimeProcess.SalaryMonth = proEndDate;
                            overtimeProcess.IsDisbursed = false;

                            var parameters = Utility.DappperParamsInKeyValuePairs(overtimeProcess, user, addBranch: true, addUserId: false);
                            parameters.Remove("Id");
                            parameters.Remove("OvertimeProcessDetails");
                            parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                            parameters.Add("CreatedDate", DateTime.Now);

                            sqlQuery = string.Empty;
                            sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeProcess", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");

                            var insertedOvertimeProcess = await connection.QueryFirstOrDefaultAsync<OvertimeProcess>(sqlQuery, parameters, transaction);

                            if (insertedOvertimeProcess == null)
                            {
                                execution.Status = false;
                                execution.Msg = $"Overtime Processed Unsucessfully for the Salary Month {salaryMonth.ToString("dd MMM yyyy")}";
                                transaction.Rollback();
                                connection.Close();
                                return execution;
                            }

                            /// Process All Individual Request

                            foreach (var request in allIndividualRequest)
                            {

                                OvertimePolicy overtimePolicy = new();
                                decimal finalCalculativeAmount = 0;
                                decimal currentdailyBasic = 0;
                                decimal currentHourlyBasic = 0;

                                sqlQuery = string.Empty;
                                sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimePolicy");
                                sqlQuery += $"WHERE OvertimeId = @OvertimeId";
                                overtimePolicy = await connection.QueryFirstOrDefaultAsync<OvertimePolicy>(sqlQuery, new { request.OvertimeId }, transaction);

                                if (overtimePolicy == null)
                                {
                                    execution.Status = false;
                                    execution.Msg = $"Overtime Process Unsuccessfull.Overtime Policy Data Not Found";
                                    transaction.Rollback();
                                    connection.Close();
                                    return execution;
                                }

                                // Percentage of Basic Amount Type
                                if (overtimePolicy.IsPercentageAmountType)
                                {

                                    sqlQuery = string.Empty;
                                    sqlQuery = "SELECT Basic From TABLE WHERE EmployeeId = @EmployeeId"; //Need to Update
                                    //thisMonthBasicAmount = await connection.QueryFirstOrDefaultAsync<OvertimePolicy>(sqlQuery, new { EmployeeId = 0 });
                                    currentdailyBasic = 1200;
                                    currentHourlyBasic = 150;
                                }

                                finalCalculativeAmount = OvertimeCalculationHelper.CalculateIndividualOvertimeAmount(overtimePolicy, request, currentdailyBasic, currentHourlyBasic);
                                if (finalCalculativeAmount > 0)
                                {

                                    OvertimeAllowances overtimeAllowance = new();
                                    overtimeAllowance.OvertimeProcessId = insertedOvertimeProcess.Id;
                                    overtimeAllowance.EmployeeId = request.EmployeeId;
                                    overtimeAllowance.OvertimeId = overtimePolicy.OvertimeId;
                                    overtimeAllowance.OvertimeName = overtimePolicy.OvertimeName;
                                    overtimeAllowance.SalaryMonth = salaryMonth;
                                    overtimeAllowance.Amount = finalCalculativeAmount;
                                    overtimeAllowance.ArrearAmount = 0;
                                    overtimeAllowance.Remarks = request.Remarks ?? "-";

                                    parameters = new Dictionary<string, dynamic>();

                                    parameters = Utility.DappperParamsInKeyValuePairs(overtimeAllowance, user, addBranch: true, addUserId: false);
                                    parameters.Remove("Id");
                                    parameters.Remove("OvertimeProcess");
                                    parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                                    parameters.Add("CreatedDate", DateTime.Now);

                                    sqlQuery = string.Empty;
                                    sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeAllowances", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");

                                    var insertedAllowance = await connection.QueryFirstOrDefaultAsync<OvertimeAllowances>(sqlQuery, parameters, transaction);

                                    if (insertedAllowance != null)
                                    {
                                        overtimeAllowanceList.Add(insertedAllowance);
                                    }
                                }


                            }

                            /// Process All Upload Request

                            foreach (var request in allUploadRequest)
                            {

                                OvertimePolicy overtimePolicy = new();
                                decimal finalCalculativeAmount = 0;
                                decimal currentdailyBasic = 0;
                                decimal currentHourlyBasic = 0;

                                sqlQuery = string.Empty;
                                sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_OvertimePolicy");
                                sqlQuery += $"WHERE OvertimeId = @OvertimeId";
                                overtimePolicy = await connection.QueryFirstOrDefaultAsync<OvertimePolicy>(sqlQuery, new { request.OvertimeId }, transaction);

                                if (overtimePolicy == null)
                                {
                                    execution.Status = false;
                                    execution.Msg = $"Overtime Process Unsuccessfull.Overtime Policy Data Not Found";
                                    transaction.Rollback();
                                    connection.Close();
                                    return execution;
                                }

                                // Percentage of Basic Amount Type
                                if (overtimePolicy.IsPercentageAmountType)
                                {

                                    sqlQuery = string.Empty;
                                    sqlQuery = "SELECT Basic From TABLE WHERE EmployeeId = @EmployeeId"; //Need to Update
                                    //thisMonthBasicAmount = await connection.QueryFirstOrDefaultAsync<OvertimePolicy>(sqlQuery, new { EmployeeId = 0 });
                                    currentdailyBasic = 1200;
                                    currentHourlyBasic = 150;
                                }

                                if (request.IsUnitUpload)
                                {
                                    finalCalculativeAmount = OvertimeCalculationHelper.CalculateUploadedOvertimeAmount(overtimePolicy, request, currentdailyBasic, currentHourlyBasic);
                                }
                                else
                                {
                                    finalCalculativeAmount = request.Amount;
                                }

                                if (finalCalculativeAmount > 0)
                                {

                                    OvertimeAllowances overtimeAllowance = new();
                                    overtimeAllowance.OvertimeProcessId = insertedOvertimeProcess.Id;
                                    overtimeAllowance.EmployeeId = request.EmployeeId;
                                    overtimeAllowance.OvertimeId = overtimePolicy.OvertimeId;
                                    overtimeAllowance.OvertimeName = overtimePolicy.OvertimeName;
                                    overtimeAllowance.SalaryMonth = salaryMonth;
                                    overtimeAllowance.Amount = finalCalculativeAmount;
                                    overtimeAllowance.ArrearAmount = request.ArrearAmount;
                                    overtimeAllowance.Remarks = request.Remarks ?? "-";

                                    parameters = new Dictionary<string, dynamic>();

                                    parameters = Utility.DappperParamsInKeyValuePairs(overtimeAllowance, user, addBranch: true, addUserId: false);
                                    parameters.Remove("Id");
                                    parameters.Remove("OvertimeProcess");
                                    parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                                    parameters.Add("CreatedDate", DateTime.Now);

                                    sqlQuery = string.Empty;
                                    sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeAllowances", paramkeys: parameters.Select(x => x.Key).ToList(), output: "OUTPUT INSERTED.*");

                                    var insertedAllowance = await connection.QueryFirstOrDefaultAsync<OvertimeAllowances>(sqlQuery, parameters, transaction);

                                    if (insertedAllowance != null)
                                    {
                                        overtimeAllowanceList.Add(insertedAllowance);
                                    }
                                }


                            }

                            /// Create Overtime Process Details Object From overtimeAllowanceList

                            if (overtimeAllowanceList.Any())
                            {

                                foreach (var item in overtimeAllowanceList.GroupBy(g => g.EmployeeId))
                                {

                                    OvertimeProcessDetails overtimeProcessDetails = new();
                                    overtimeProcessDetails.OvertimeProcessId = insertedOvertimeProcess.Id;
                                    overtimeProcessDetails.SalaryMonth = salaryMonth;
                                    overtimeProcessDetails.EmployeeId = item.Key;
                                    overtimeProcessDetails.TotalAmount = item.ToList().Sum(s => s.Amount);
                                    overtimeProcessDetails.TotalArrearAmount = item.ToList().Sum(s => s.ArrearAmount);
                                    overtimeProcessDetails.NetPay = item.ToList().Sum(s => s.Amount + s.ArrearAmount);

                                    parameters = new Dictionary<string, dynamic>();

                                    parameters = Utility.DappperParamsInKeyValuePairs(overtimeProcessDetails, user, addBranch: true, addUserId: false);
                                    parameters.Remove("Id");
                                    parameters.Remove("OvertimeProcess");
                                    parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                                    parameters.Add("CreatedDate", DateTime.Now);

                                    sqlQuery = string.Empty;
                                    sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_OvertimeProcessDetails", paramkeys: parameters.Select(x => x.Key).ToList());

                                    var affectedRow = await connection.ExecuteAsync(sqlQuery, parameters, transaction);
                                }
                            }



                            execution.Status = true;
                            execution.Msg = $"Overtime Processed Sucessfully for the Salary Month {salaryMonth.ToString("dd MMM yyyy")}";
                            transaction.Commit();
                            connection.Close();
                            return execution;
                        }

                        catch (Exception ex)
                        {

                            transaction.Rollback();
                            execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                            await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "OvertimeProcess", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
                        }
                    }
                }

                catch (Exception ex)
                {
                    execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                    await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "OvertimeProcess", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
                }

                finally { connection.Close(); }

                return execution;
            }

        }
        public async Task<ExecutionStatus> RollBackOvertimeProcess(long overtimeProcessId, AppUser user)
        {
            ExecutionStatus execution = new();
            using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
            {

                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        string sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeAllowances");
                        sqlQuery += $"WHERE OvertimeProcessId = @overtimeProcessId";

                        await connection.ExecuteAsync(sqlQuery, new { overtimeProcessId }, transaction);

                        sqlQuery = string.Empty;
                        sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeProcessDetails");
                        sqlQuery += $"WHERE OvertimeProcessId = @overtimeProcessId";

                        await connection.ExecuteAsync(sqlQuery, new { overtimeProcessId }, transaction);

                        sqlQuery = string.Empty;
                        sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_OvertimeProcess");
                        sqlQuery += $"WHERE Id = @overtimeProcessId";

                        await connection.ExecuteAsync(sqlQuery, new { overtimeProcessId }, transaction);

                        execution.Status = true;
                        execution.Msg = "Data has been Roll-Back successfully.";

                        transaction.Commit();
                        connection.Close();
                        return execution;
                    }

                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                        await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "RollBackOvertimeProcess", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
                    }

                    finally { connection.Close(); }

                }

                return execution;

            }
        }
        public async Task<ExecutionStatus> DisburseOvertimeProcess(OvertimeProcess overtimeProcess, AppUser user)
        {

            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            try
            {
                var parameters = new Dictionary<string, dynamic> {
                    { "IsDisbursed", overtimeProcess.IsDisbursed},
                    { "UpdatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId },
                    { "UpdatedDate", DateTime.Now }
                };

                sqlQuery = Utility.GenerateUpdateQuery(tableName: "Payroll_OvertimeProcess", paramkeys: parameters.Select(x => x.Key).ToList());
                sqlQuery += $"WHERE Id = @Id";
                parameters.Add("Id", overtimeProcess.Id);

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameters, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Overtime Process Disbursed successfully.";
                }
            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "DisburseOvertimeProcess", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;

        }
        public async Task<ExecutionStatus> UploadOvertimeTimeCard(List<UploadOvertimeAllowances> uploadOTAllowanceList, AppUser user)
        {
            ExecutionStatus execution = new();
            string sqlQuery = string.Empty;
            List<Dictionary<string, dynamic>> parameterList = new();

            try
            {

                foreach (var uploadOTAlw in uploadOTAllowanceList)
                {

                    var parameters = Utility.DappperParamsInKeyValuePairs(uploadOTAlw, user, addBranch: true, addUserId: false);
                    parameters.Remove("Id");
                    parameters.Add("CreatedBy", user.EmployeeId > 0 ? user.EmployeeId : user.UserId);
                    parameters.Add("CreatedDate", DateTime.Now);
                    parameterList.Add(parameters);
                }

                sqlQuery = Utility.GenerateInsertQuery(tableName: "Payroll_UploadOvertimeAllowances", paramkeys: parameterList.First().Select(x => x.Key).ToList());

                int rawAffected = await _dapper.SqlExecuteNonQuery(user.Database, sqlQuery, parameterList, CommandType.Text);

                if (rawAffected > 0)
                {
                    execution.Status = true;
                    execution.Msg = "Data has been saved successfully.";
                }

            }
            catch (Exception ex)
            {
                execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "UploadOvertimeTimeCard", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
            }
            return execution;
        }
        public async Task<ExecutionStatus> RollBackUploadedTimeCard(RollBackTimeCardDTO model, AppUser user)
        {

            ExecutionStatus execution = new();
            using (var connection = _dapper.SqlConnectionForTransactionalNonQuery(user.Database))
            {

                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {

                        string sqlQuery = Utility.GenerateSelectQuery(tableName: "Payroll_UploadOvertimeAllowances");
                        sqlQuery += $"WHERE OvertimeId = @OvertimeId AND YEAR(SalaryMonth) = @Year AND MONTH(SalaryMonth) = @Month;";

                        var uploadAllowances = await connection.QueryAsync<UploadOvertimeAllowances>(sqlQuery, new { model.OvertimeId, model.Year, model.Month }, transaction);

                        if (!uploadAllowances.Any())
                        {
                            execution.Status = false;
                            execution.Msg = $"No Uploaded Data Found for the Salary Month {new DateTime(model.Year, model.Month, 1).ToString("MMM yyyy")}";
                            transaction.Rollback();
                            connection.Close();
                            return execution;
                        }

                        sqlQuery = string.Empty;
                        sqlQuery = Utility.GenerateDeleteQuery(tableName: "Payroll_UploadOvertimeAllowances");
                        sqlQuery += $"WHERE OvertimeId = @OvertimeId AND YEAR(SalaryMonth) = @Year AND MONTH(SalaryMonth) = @Month;";

                        int rowCount = await connection.ExecuteAsync(sqlQuery, new { model.OvertimeId, model.Year, model.Month }, transaction);

                        if (rowCount > 0)
                        {
                            execution.Status = true;
                            execution.Msg = "Data has been Roll-Back successfully.";
                            transaction.Commit();
                        }
                    }

                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        execution = Utility.Invalid(ex.InnerException != null ? ex.InnerException.Message : ex.Message);
                        await _sysLogger.SaveSystemException(ex, user.Database, "OvertimeBusiness", "RollBackUploadedTimeCard", user.Username, user.OrganizationId, user.CompanyId, user.BranchId);
                    }

                    finally { connection.Close(); }

                }

                return execution;

            }

        }


    }
}
