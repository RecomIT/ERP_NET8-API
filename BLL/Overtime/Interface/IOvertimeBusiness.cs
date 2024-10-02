using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Overtime.Domain;
using Shared.Overtime.DTO;
using Shared.Overtime.ViewModel;

namespace BLL.Overtime.Interface
{
    public interface IOvertimeBusiness
    {
        //Employee Details
        Task<EmployeeDTO> GetEmployeeDetailsById(long empId, AppUser user);
        Task<EmployeeDTO> GetEmployeeDetailsByCode(string employeeCode, AppUser user);
        // Overtime Policy
        Task<ExecutionStatus> SaveOvertimePolicy(OvertimePolicy overtimePolicy, AppUser user);
        Task<ExecutionStatus> UpdateOvertimePolicy(OvertimePolicy overtimePolicy, AppUser user);
        Task<ExecutionStatus> DeleteOvertimePolicy(long overtimeId, AppUser user);
        Task<IEnumerable<OvertimePolicy>> GetAllOvertimePolicy(AppUser user);
        Task<OvertimePolicy> GetOvertimePolicyById(long overtimeId, AppUser user);
        Task<IEnumerable<OvertimePolicy>> GetOvertimePolicyBySpecification(string column, dynamic value, AppUser user);


        // Overtime Approval Level

        Task<IEnumerable<OvertimeApprovalLevel>> GetAllOvertimeApprovalLevel(AppUser user);
        Task<OvertimeApprovalLevel> GetOvertimeApprovalLevelById(long overtimeApprovalLevelId, AppUser user);
        Task<ExecutionStatus> SaveOvertimeApprovalLevel(OvertimeApprovalLevel overtimeApprovalLevel, AppUser user);
        Task<ExecutionStatus> UpdateOvertimeApprovalLevel(OvertimeApprovalLevel overtimeApprovalLevel, AppUser user);
        Task<ExecutionStatus> DeleteOvertimeApprovalLevel(long overtimeApprovalLevelId, AppUser user);


        // Overtime Approver Assignment
        Task<IEnumerable<OvertimeApproverDTO>> GetEmployeesForOvertimeApprover(AppUser user);
        Task<IEnumerable<OvertimeApproverDTO>> GetAllOvertimeApprover(AppUser user);
        Task<OvertimeApproverDTO> GetOvertimeApproverByApproverId(long overtimeApproverId, AppUser user);
        Task<IEnumerable<OvertimeApproverDTO>> GetOvertimeApproversByApproverIds(List<long> overtimeApproverIds, AppUser user);
        Task<OvertimeApproverDTO> GetOvertimeApproverByEmployeeId(long employeeId, AppUser user);
        Task<ExecutionStatus> SaveOvertimeApprover(List<OvertimeApprover> overtimeApproverList, AppUser user);
        Task<ExecutionStatus> UpdateOvertimeApprover(OvertimeApprover overtimeApprover, AppUser user);
        Task<ExecutionStatus> DeleteOvertimeApprover(long overtimeApproverId, AppUser user);


        // Overtime Team Approval Mapping
        Task<IEnumerable<OvertimeEmployeeDTO>> GetEmployeesForOvertimeTeamApprovalMapping(AppUser user);
        Task<IEnumerable<OvertimeEmployeeDTO>> GetAllOvertimeTeamMembersApprovalMapping(AppUser user);
        Task<OvertimeEmployeeDTO> GetOvertimeTeamMemberApprovalMappingById(long overtimeTeamApprovalMappingId, AppUser user);
        Task<IEnumerable<OvertimeEmployeeDTO>> GetOvertimeTeamMembersApprovalMappingByTeamMemberId(long teamMemberId, AppUser user);
        Task<IEnumerable<OvertimeEmployeeDTO>> GetOvertimeTeamMembersByApproverId(long overtimeApproverId, AppUser user);
        Task<ExecutionStatus> SaveOvertimeTeamMembersToApprover(List<OvertimeTeamApprovalMapping> overtimeTeamMemberList, AppUser user);
        Task<OvertimeEmployeeDTO> GetOvertimeTeamMemberByApprovalMappingId(long overtimeTeamApprovalMappingId, AppUser user);
        Task<ExecutionStatus> DeleteOvertimeTeamMemberByApprovalMappingId(long overtimeTeamApprovalMappingId, AppUser user);
        Task<ExecutionStatus> DeleteOvertimeTeamMemberByApproverId(long overtimeApproverId, AppUser user);
        Task<ExecutionStatus> UpdateOvertimeTeamApprovalMappingLevel(long OvertimeTeamApprovalMappingId, int level, AppUser user);


        // Overtime Request

        Task<IEnumerable<OvertimeRequest>> GetAllOvertimeRequest(string status, AppUser user);
        Task<OvertimeRequest> GetOvertimeRequestById(long overtimeRequestId, AppUser user);
        Task<IEnumerable<OvertimeRequest>> GetAllOvertimeRequestForEmployee(long employeeId, string status, AppUser user);
        Task<IEnumerable<OvertimeRequest>> GetAllOvertimeRequestForApprover(long overtimeApproverId, string status, AppUser user);
        Task<ExecutionStatus> SaveOvertimeRequest(OvertimeRequest overtimeRequest, AppUser user);
        Task<ExecutionStatus> UpdateOvertimeRequest(OvertimeRequest overtimeRequest, AppUser user);
        Task<OvertimeRequest> CheckOvertimeRequestAlreadyExist(SaveOvertimeRequestDTO overtimeRequest, AppUser user);
        Task<ExecutionStatus> OvertimeRequestApprovalAction(OvertimeRequest overtimeRequest, AppUser user);
        Task<ExecutionStatus> DeleteOvertimeRequesById(long overtimeRequestId, AppUser user);


        // Overtime Process

        Task<IEnumerable<OvertimeProcess>> GetAllOvertimeProcess(AppUser user);
        Task<OvertimeProcess> GetOvertimeProcessById(long overtimeProcessId, AppUser user);
        Task<ExecutionStatus> OvertimeProcess(OvertimeProcessViewModel model, AppUser user);
        Task<ExecutionStatus> RollBackOvertimeProcess(long overtimeProcessId, AppUser user);
        Task<ExecutionStatus> DisburseOvertimeProcess(OvertimeProcess overtimeProcess, AppUser user);
        Task<ExecutionStatus> UploadOvertimeTimeCard(List<UploadOvertimeAllowances> uploadOTAllowanceList, AppUser user);
        Task<ExecutionStatus> RollBackUploadedTimeCard(RollBackTimeCardDTO model, AppUser user);


    }
}
