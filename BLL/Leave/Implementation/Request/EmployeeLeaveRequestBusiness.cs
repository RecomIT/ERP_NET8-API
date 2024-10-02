using Dapper;
using System;
using System.Net;
using System.Data;
using System.Linq;
using System.Net.Mail;
using Shared.Services;
using BLL.Base.Interface;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using Shared.OtherModels.Response;
using Shared.OtherModels.Pagination;
using Shared.OtherModels.DataService;
using Shared.Helpers;
using DAL.Repository.Control_Panel;
using DAL.UnitOfWork.Control_Panel.Interface;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using DAL.Repository.Leave.Interface;
using BLL.Leave.Interface.Request;
using Shared.Employee.ViewModel.Info;
using Shared.Employee.Filter.Info;
using Shared.Leave.DTO.Request;
using Shared.Leave.Filter.Request;
using Shared.Leave.ViewModel.Request;
using Shared.Leave.ViewModel.Setup;

namespace BLL.Leave.Implementation.Request
{
    public class EmployeeLeaveRequestBusiness : IEmployeeLeaveRequestBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly IEmployeeHierarchyRepository _employeeHierarchyRepository;
        private readonly IEmailSendingConfigRepository _emailSendingConfigRepository;
        private readonly EmailSettingReposiitory _emailSettingReposiitory;
        private readonly IControlPanelUnitOfWork _controlPanelDbContext;
        private readonly ILeaveTypeRepository _leaveTypeRepository;

        public EmployeeLeaveRequestBusiness(ISysLogger sysLogger, IDapperData dapper, ILeaveRequestRepository leaveRequestRepository, IEmployeeRepository employeeRepository, IEmployeeHierarchyRepository employeeHierarchyRepository, IEmailSendingConfigRepository emailSendingConfigRepository, IControlPanelUnitOfWork controlPanelDbContext, ILeaveTypeRepository leaveTypeRepository, ILeaveBalanceRepository leaveBalanceRepository)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _leaveRequestRepository = leaveRequestRepository;
            _employeeRepository = employeeRepository;
            _employeeHierarchyRepository = employeeHierarchyRepository;
            _emailSendingConfigRepository = emailSendingConfigRepository;
            _controlPanelDbContext = controlPanelDbContext;
            _emailSettingReposiitory = new EmailSettingReposiitory(_controlPanelDbContext);
            _leaveTypeRepository = leaveTypeRepository;
            _leaveBalanceRepository = leaveBalanceRepository;
        }
        public async Task<ExecutionStatus> DeleteEmployeeLeaveRequestAsync(DeleteEmployeeLeaveRequestDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                //var sp_name = "sp_HR_EmployeeLeaveRequest_2";
                //var parameters = DapperParam.AddParams(model, user);
                //parameters.Add("ExecutionFlag", Data.Delete);
                //executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                executionStatus = await _leaveRequestRepository.DeleteEmployeeLeaveRequestAsync(model, user);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "DeleteEmployeeLeaveRequestAsync", user);
            }
            return executionStatus;
        }
        public async Task<string> GetEmployeeLeaveCodeAsync(AppUser user)
        {
            string leaveCode = "";
            try
            {
                var query = $@"(Select Case When MAX(EmployeeLeaveCode) IS NULL Then 'ELRC-0000000001'  
					ELSE ('ELRC-' +RIGHT('0000000000'+ Convert(NVarchar(50),MAX(Convert(int,SUBSTRING(EmployeeLeaveCode,6,20)))+1),10)) END 
					From HR_EmployeeLeaveRequest Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId)";
                var parameters = new { user.CompanyId, user.OrganizationId };
                leaveCode = await _dapper.SqlQueryFirstAsync<string>(user.Database, query, parameters);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return leaveCode;
        }
        public async Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveHistoryAsync(LeaveHistory_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeLeaveRequestViewModel> list = new List<EmployeeLeaveRequestViewModel>();
            try
            {
                var sp_name = "sp_HR_EmployeeLeaveRequest_2";
                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", "History");
                list = await _dapper.SqlQueryListAsync<EmployeeLeaveRequestViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetEmployeeLeaveRequestsAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveHistoryAsync(LeaveRequest_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeLeaveRequestViewModel> list = new List<EmployeeLeaveRequestViewModel>();
            try
            {
                var query = $@"Select lt.Title 'LeaveTypeName',lr.DayLeaveType,lh.LeaveDate,lr.LeavePurpose,lr.EmergencyPhoneNo,lr.AddressDuringLeave,lr.StateStatus 
				From [HR_EmployeeLeaveRequest] lr
				Inner Join HR_LeaveTypes LT on lr.LeaveTypeId =LT.Id
			    Inner Join HR_EmployeeLeaveHistory lh On lh.EmployeeLeaveRequestId = lr.EmployeeLeaveRequestId And lr.EmployeeId = lh.EmployeeId
				Where 1=1
                AND (@EmployeeLeaveRequestId= 0 OR @EmployeeLeaveRequestId IS NULL OR lr.EmployeeLeaveRequestId=@EmployeeLeaveRequestId)
				AND (@EmployeeId IS NULL OR @EmployeeId=0 OR lr.EmployeeId=@EmployeeId)
				AND (@LeaveTypeId IS NULL OR @LeaveTypeId=0 OR lr.LeaveTypeId=@LeaveTypeId)
				AND (@StateStatus IS NULL OR @StateStatus ='' OR lr.StateStatus=@StateStatus)
				AND (
                    (@AppliedFromDate IS NOT NULL OR @AppliedToDate IS NOT NULL)
                    OR
				    (lh.LeaveDate between  Convert(Date,@AppliedFromDate)  AND  Convert(date,@AppliedToDate)) 
					OR
					(ISNULL(@AppliedFromDate,'') <> '' AND AppliedFromDate = Convert(date,@AppliedFromDate))
					OR
					(ISNULL(@AppliedToDate,'') <> '' AND AppliedToDate =CAST(@AppliedToDate AS date))
					OR
					(ISNULL(@AppliedFromDate,'') ='' AND ISNULL(@AppliedToDate,'') = '')
				)
				AND lr.CompanyId=@CompanyId
				AND lr.OrganizationId =@OrganizationId
				Order by lt.Title,lh.LeaveDate desc";

                var parameters = DapperParam.AddParams(filter, user);
                list = await _dapper.SqlQueryListAsync<EmployeeLeaveRequestViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetEmployeeLeaveHistoryAsync", user);
            }
            return list;
        }
        public async Task<DBResponse<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveRequestsAsync(LeaveRequest_Filter filter, AppUser user)
        {
            DBResponse<EmployeeLeaveRequestViewModel> data = new DBResponse<EmployeeLeaveRequestViewModel>();
            DBResponse response = new DBResponse();
            try
            {
                var query = $@"BEGIN 
WITH Data_CTE AS(Select lr.EmployeeLeaveRequestId, lr.EmployeeId, lr.DesignationId, lr.DepartmentId, lr.SectionId, lr.UnitId, lr.LeaveTypeId,lr.DayLeaveType, lr.AppliedFromDate, lr.AppliedToDate, lr.AppliedTotalDays, lr.LeavePurpose, lr.EmergencyPhoneNo, lr.AddressDuringLeave, lr.ApprovedFromDate, lr.ApprovedToDate, lr.TotalApprovalDays, lr.Remarks, lr.StateStatus, lr.IsApproved, lr.CreatedBy, lr.CreatedDate, lr.UpdatedBy, lr.UpdatedDate, lr.OrganizationId, lr.CompanyId, lr.ApprovedBy, lr.ApprovedDate, lr.ApprovalRemarks, lr.CancelledBy, lr.CancelledDate, lr.CancelRemarks, lr.BranchId, lr.CheckedBy, lr.CheckedDate, lr.CheckRemarks, lr.GradeId, lr.SubSectionId, lr.AttachmentFileNames, lr.AttachmentFileTypes, lr.AttachmentFiles, lr.HalfDayType, lr.ReliverDesignationId, lr.ReliverId, lr.EmployeeLeaveCode, lr.EmployeeLeaveBalanceId, lr.RejectedBy, lr.RejectedDate, lr.RejectedRemarks,
lt.Title 'LeaveTypeName',emp.FullName 'EmployeeName', emp.GradeName ,emp.DesignationName, emp.DepartmentName,emp.SectionName, emp.SubSectionName,
              
			CreaterInfo =(Case When lr.CreatedBy IS NOT NULL AND lr.CreatedBy !='' AND TRY_CONVERT(bigint,lr.CreatedBy) > 0 THEN 
			(SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.CreatedBy))
			END),
		UpdaterInfo =(Case When lr.UpdatedBy IS NOT NULL AND lr.UpdatedBy !='' AND TRY_CONVERT(bigint,lr.UpdatedBy) > 0 THEN 
			(SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.UpdatedBy))
			END),
		ApproverInfo=(Case When lr.ApprovedBy IS NOT NULL AND lr.ApprovedBy !='' AND TRY_CONVERT(bigint,lr.ApprovedBy) > 0 THEN 
			(SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.ApprovedBy))
			END),
		RejecterInfo=(Case When lr.RejectedBy IS NOT NULL AND lr.RejectedBy !='' AND TRY_CONVERT(bigint,lr.RejectedBy) > 0 THEN 
			(SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.RejectedBy))
			END),
		CheckerInfo=(Case When lr.CheckedBy IS NOT NULL AND lr.CheckedBy !='' AND TRY_CONVERT(bigint,lr.CheckedBy) > 0 THEN 
			(SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.CheckedBy))
			END),
		CancellerInfo=(Case When lr.CancelledBy IS NOT NULL AND lr.CancelledBy !='' AND TRY_CONVERT(bigint,lr.CancelledBy) > 0 THEN 
			(SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId = TRY_CONVERT(bigint,lr.CancelledBy))
			END),
         SupervisorId=(CASE WHEN lr.StateStatus='Pending' THEN ISNULL(eh.SupervisorId,0) ELSE ISNULL(lr.SupervisorId,0) END),
         SupervisorName=(CASE WHEN lr.StateStatus='Pending' THEN 
            (SELECT FullName+' ('+EmployeeCode+')' FROM HR_EmployeeInformation WHERE EmployeeId=ISNULL(eh.SupervisorId,0))
            ELSE NULL END),
        HODId=(CASE WHEN lr.StateStatus='Recommended' THEN ISNULL(eh.HeadOfDepartmentId,0) ELSE ISNULL(lr.HODId,0) END),
        HODName=(CASE WHEN lr.StateStatus='Recommended' THEN 
            (SELECT EmployeeName=(FullName+' ('+EmployeeCode+')') FROM HR_EmployeeInformation WHERE EmployeeId=ISNULL(eh.HeadOfDepartmentId,0))
            ELSE NULL END),lr.FilePath,lr.FileName,lr.EstimatedDeliveryDate,lr.ActualFileName

	From [HR_EmployeeLeaveRequest] lr
	Inner Join HR_LeaveTypes LT on lr.LeaveTypeId =LT.Id
	Inner Join vw_HR_EmployeeList emp on lr.EmployeeId = emp.EmployeeId AND lr.CompanyId=emp.CompanyId AND lr.OrganizationId=emp.OrganizationId
	Left Join (SELECT DISTINCT EmployeeId,SupervisorId,HeadOfDepartmentId FROM HR_EmployeeHierarchy Where IsActive=1) eh On eh.EmployeeId = emp.EmployeeId
	Where 1=1
				AND (@EmployeeLeaveRequestId IS NULL OR @EmployeeLeaveRequestId= 0 OR lr.EmployeeLeaveRequestId=@EmployeeLeaveRequestId)
				AND (@EmployeeId IS NULL OR @EmployeeId = 0 OR lr.EmployeeId=@EmployeeId)
				AND (@DepartmentId IS NULL OR @DepartmentId = 0 OR lr.DepartmentId=@DepartmentId)
				AND (@DesignationId IS NULL OR @DesignationId=0 OR lr.DesignationId=@DesignationId)
				AND (@SectionId IS NULL OR @SectionId=0 OR lr.SectionId=@SectionId)
				AND (@SubSectionId IS NULL OR @SubSectionId=0 OR lr.SubSectionId =@SubSectionId)
				AND (@LeaveTypeId IS NULL OR @LeaveTypeId=0 OR lr.LeaveTypeId=@LeaveTypeId)
				AND (@DayLeaveType IS NULL OR @DayLeaveType ='' OR lr.DayLeaveType=@DayLeaveType)
				AND (@StateStatus IS NULL OR @StateStatus ='' OR lr.StateStatus=@StateStatus)
				AND (@SupervisorId IS NULL OR @SupervisorId =0 OR eh.SupervisorId=@SupervisorId)
				AND (
					((ISNULL(@AppliedFromDate,'') <> '' AND ISNULL(@AppliedToDate,'') <> '') 
					AND AppliedFromDate >= Convert(Date,@AppliedFromDate)  AND AppliedToDate<= Convert(date,@AppliedToDate)) 
					OR
					(ISNULL(@AppliedFromDate,'') <> '' AND AppliedFromDate = Convert(date,@AppliedFromDate))
					OR
					(ISNULL(@AppliedToDate,'') <> '' AND AppliedToDate =CAST(@AppliedToDate AS date))
					OR
					(ISNULL(@AppliedFromDate,'') ='' AND ISNULL(@AppliedToDate,'') = '')
				)
				AND lr.CompanyId=@CompanyId
				AND lr.OrganizationId =@OrganizationId),
	Count_CTE AS (
	SELECT COUNT(*) AS [TotalRows]
	FROM Data_CTE)

	SELECT JSONData=(Select * From (SELECT *
	FROM Data_CTE
	ORDER BY 
	CASE WHEN ISNULL(@SortingCol,'') = '' THEN Data_CTE.EmployeeLeaveRequestId END,
	CASE WHEN @SortingCol = 'EmployeeLeaveCode' AND @SortType ='ASC' THEN Data_CTE.EmployeeLeaveCode END ASC
	OFFSET (@PageNumber-1)*@PageSize ROWS
	FETCH NEXT CAST(@PageSize AS INT) ROWS ONLY) tbl FOR JSON AUTO),TotalRows=(Select TotalRows From Count_CTE),PageSize=@PageSize,PageNumber=@PageNumber END";
                var parameters = DapperParam.AddParams(filter, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, query, parameters, CommandType.Text);
                data.ListOfObject = Utility.JsonToObject<IEnumerable<EmployeeLeaveRequestViewModel>>(response.JSONData) ?? new List<EmployeeLeaveRequestViewModel>();
                data.Pageparam = new Pageparam() { PageNumber = response.PageNumber, PageSize = response.PageSize, TotalPages = response.TotalPages, TotalRows = response.TotalRows };
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetEmployeeLeaveRequestsAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetEmployeeLeaveRequestsForSupervisorApprovalAsync(LeaveRequest_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeLeaveRequestViewModel> list = new List<EmployeeLeaveRequestViewModel>();
            try
            {
                var sp_name = "sp_HR_EmployeeLeaveRequest_2";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                parameters.Add("SupervisorId", filter.EmployeeId);
                parameters.Add("ExecutionFlag", Data.Read);
                list = await _dapper.SqlQueryListAsync<EmployeeLeaveRequestViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetEmployeeLeaveRequestsForSupervisorApprovalAsync", user);
            }
            return list;
        }
        //public async Task<IEnumerable<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(LeaveRequest_Filter filter, AppUser user)
        //{
        //    IEnumerable<EmployeeLeaveRequestViewModel> list = new List<EmployeeLeaveRequestViewModel>();
        //    try {
        //        list = await _leaveRequestRepository.GetSubordinatesEmployeeLeaveRequestAsync(filter, user);
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetSubordinatesEmployeeLeaveRequestAsync", user);
        //    }
        //    return list;
        //}
        public async Task<bool> IsLeavePendingAsync(long EmployeeLeaveRequestId, AppUser user)
        {
            bool status = false;
            try
            {
                var query = $@"(Select Count(*) From HR_EmployeeLeaveRequest 
				Where EmployeeLeaveRequestId=@EmployeeLeaveRequestId AND StateStatus IN('Pending','Recheck') AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId)";
                var parameters = new { EmployeeLeaveRequestId, user.CompanyId, user.OrganizationId };
                var count = await _dapper.SqlQueryFirstAsync<int>(user.Database, query, parameters);
                if (count > 0)
                {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "IsLeavePendingAsync", user);
            }
            return status;
        }
        public async Task<ExecutionStatus> LeaveRequestApprovalAsync(EmployeeLeaveRequestStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _leaveRequestRepository.LeaveRequestApprovalAsync(model, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "LeaveRequestApprovalAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<LeaveRequestEmailSendViewModel>> LeaveRequestEmailSendAsync(LeaveRequestEmail_Filter filter, AppUser user)
        {
            IEnumerable<LeaveRequestEmailSendViewModel> emailDtls = new List<LeaveRequestEmailSendViewModel>();
            try
            {
                var sp_name = "sp_HR_EmployeeLeaveRequest_2";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", filter.EmployeeId);
                parameters.Add("LeaveTypeId", filter.LeaveTypeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("ExecutionFlag", "Email");

                emailDtls = await _dapper.SqlQueryListAsync<LeaveRequestEmailSendViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);

                if (emailDtls != null)
                {
                    var emailSetting = Utility.JsonToObject<IEnumerable<EmailSettingDataWithIdViewModel>>(emailDtls.First().Json).FirstOrDefault();
                    var email = Utility.JsonToObject<IEnumerable<LeaveRequestEmailSendViewModel>>(emailDtls.First().JsonEmailCCBCC).FirstOrDefault();
                    if (emailSetting != null)
                    {
                        await EmailSend(emailDtls.First().EmailTo, emailDtls.First().EmpDtls, emailDtls.First().LeaveTypeName, email, emailSetting, emailDtls.First().UserName, emailDtls.First().UserEmail, filter.Remarks, filter.Status, filter.EmailType);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "LeaveRequestEmailSendAsync", user);
            }
            return emailDtls;
        }




        public async Task<ExecutionStatus> SaveEmployeeLeaveRequest2Async(LeaveRequestDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                if (model.File != null && model.File.Length > 0)
                {

                    var fileDetail = FileProcessor.Process(model.LeaveRequest.EmployeeLeaveRequestId, model.File, model.FilePath, user);
                    if (fileDetail != null)
                    {
                        model.FilePath = fileDetail.FilePath;
                        model.FileName = fileDetail.FileName;
                        model.FileSize = fileDetail.FileSize;
                        model.ActualFileName = fileDetail.ActualFileName;
                        model.FileType = fileDetail.Extenstion;
                    }
                }
                executionStatus = await _leaveRequestRepository.SaveAsync(model, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "SaveEmployeeLeaveRequest2Async", user);
            }
            return executionStatus;
        }




        public async Task<ExecutionStatus> SaveEmployeeLeaveRequestAsync(LeaveRequestDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeLeaveRequest_2";
                var jsonData = Utility.JsonData(model.LeaveDays);
                var parameters = Utility.DappperParams(model.LeaveRequest, user);
                parameters.Add("LeaveDaysJSON", jsonData);
                parameters.Add("ExecutionFlag", model.LeaveRequest.EmployeeLeaveRequestId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "SaveEmployeeLeaveRequestAsync", user);
            }
            return executionStatus;
        }
        public async Task<ExecutionStatus> SaveEmployeeLeaveRequestStatusAsync(EmployeeLeaveRequestStatusDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_HR_EmployeeLeaveRequest_2";
                var parameters = DapperParam.AddParams(model, user);
                parameters.Add("ExecutionFlag", Data.Checking);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                executionStatus.ItemId = model.EmployeeLeaveRequestId;
                executionStatus.Action = model.StateStatus;
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "SaveEmployeeLeaveRequestAsync", user);
            }
            return executionStatus;
        }
        public async Task<bool> SendLeaveEmailAsync(ExecutionStatus execution, AppUser user)
        {
            try
            {
                var presentValues = JsonReverseConverter.JsonToObject2<EmployeeLeaveRequestViewModel>(execution.PresentValue);
                EmployeeLeaveRequestViewModel previousValue = null;
                if (execution.PreviousValue != null)
                {
                    previousValue = JsonReverseConverter.JsonToObject2<EmployeeLeaveRequestViewModel>(execution.PreviousValue);
                }
                if (presentValues != null && presentValues.EmployeeLeaveRequestId > 0)
                {
                    var leaveType = await _leaveTypeRepository.GetByIdAsync(presentValues.LeaveTypeId, user);
                    if (leaveType != null)
                    {
                        presentValues.LeaveTypeName = leaveType.Title;
                    }

                    var leaveBalanceInfo = await _leaveBalanceRepository.GetEmployeeLeaveBalanceOfLeaveTypeAsync(presentValues.EmployeeId, presentValues.LeaveTypeId, presentValues.AppliedFromDate.Value.ToString("yyyy-MM-dd"), presentValues.AppliedToDate.Value.ToString("yyyy-MM-dd"), user);

                    if (leaveBalanceInfo != null)
                    {
                        presentValues.LeaveBalance = leaveBalanceInfo.TotalLeave - (leaveBalanceInfo.LeaveApplied ?? 0);
                        if (presentValues.LeaveBalance == 0)
                        {
                            presentValues.LeaveBalance = presentValues.AppliedTotalDays;
                        }
                        else
                        {
                            presentValues.LeaveBalance = presentValues.LeaveBalance + presentValues.AppliedTotalDays;
                        }
                    }
                    else
                    {
                        presentValues.LeaveBalance = presentValues.AppliedTotalDays;
                    }

                    var employeeInfo = (await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter
                    {
                        EmployeeId = presentValues.EmployeeId.ToString()
                    }, user)).FirstOrDefault();

                    if (employeeInfo != null)
                    {
                        var hierarchy = await _employeeHierarchyRepository.GetEmployeeActiveHierarchy(employeeInfo.EmployeeId, user);

                        EmployeeServiceDataViewModel supervisorInfo = null;
                        if (hierarchy.SupervisorId.HasValue && hierarchy.SupervisorId.Value > 0)
                        {
                            supervisorInfo = (await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter
                            {
                                EmployeeId = hierarchy.SupervisorId.ToString()
                            }, user)).FirstOrDefault();
                        }

                        EmployeeServiceDataViewModel lineManagerInfo = null;
                        if (hierarchy.LineManagerId.HasValue && hierarchy.LineManagerId.Value > 0)
                        {
                            lineManagerInfo = (await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter
                            {
                                EmployeeId = hierarchy.LineManagerId.ToString()
                            }, user)).FirstOrDefault();
                        }

                        EmployeeServiceDataViewModel managerInfo = null;
                        if (hierarchy.ManagerId.HasValue && hierarchy.ManagerId.Value > 0)
                        {
                            managerInfo = (await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter
                            {
                                EmployeeId = hierarchy.ManagerId.ToString()
                            }, user)).FirstOrDefault();
                        }

                        EmployeeServiceDataViewModel headOfDepartmentInfo = null;
                        if (hierarchy.HeadOfDepartmentId.HasValue && hierarchy.HeadOfDepartmentId.Value > 0)
                        {
                            headOfDepartmentInfo = (await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter
                            {
                                EmployeeId = hierarchy.HeadOfDepartmentId.ToString()
                            }, user)).FirstOrDefault();
                        }

                        EmployeeServiceDataViewModel hrAuthoriry = null;
                        if (hierarchy.HRAuthorityId.HasValue && hierarchy.HRAuthorityId.Value > 0)
                        {
                            hrAuthoriry = (await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter
                            {
                                EmployeeId = hierarchy.HRAuthorityId.ToString()
                            }, user)).FirstOrDefault();
                        }

                        var emailConfig = await _emailSendingConfigRepository.GetEmailSendingConfiguration("Leave", "Leave Request", user);
                        LeaveRequestEmailSendViewModel email = new LeaveRequestEmailSendViewModel();

                        if (emailConfig != null)
                        {
                            #region Email-To
                            if (emailConfig.EmailTo == "Level 1")
                            {
                                if (supervisorInfo != null)
                                {
                                    email.EmailTo = supervisorInfo.OfficeEmail.Default();
                                    email.EmailToName = supervisorInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailTo == "Level 2")
                            {
                                if (lineManagerInfo != null)
                                {
                                    email.EmailTo = lineManagerInfo.OfficeEmail.Default();
                                    email.EmailToName = lineManagerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailTo == "Level 3")
                            {
                                if (managerInfo != null)
                                {
                                    email.EmailTo = managerInfo.OfficeEmail.Default();
                                    email.EmailToName = managerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailTo == "Level 4")
                            {
                                if (headOfDepartmentInfo != null)
                                {
                                    email.EmailTo = headOfDepartmentInfo.OfficeEmail.Default();
                                    email.EmailToName = headOfDepartmentInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailTo == "Level 5")
                            {
                                if (hrAuthoriry != null)
                                {
                                    email.EmailTo = hrAuthoriry.OfficeEmail.Default();
                                    email.EmailToName = hrAuthoriry.EmployeeName;
                                }
                            }
                            #endregion

                            #region Email-CC1
                            if (emailConfig.EmailCC1 == "Level 1")
                            {
                                if (supervisorInfo != null)
                                {
                                    email.EmailCC1 = supervisorInfo.OfficeEmail.Default();
                                    email.EmailCC1Name = supervisorInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC1 == "Level 2")
                            {
                                if (lineManagerInfo != null)
                                {
                                    email.EmailCC1 = lineManagerInfo.OfficeEmail.Default();
                                    email.EmailCC1Name = lineManagerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC1 == "Level 3")
                            {
                                if (managerInfo != null)
                                {
                                    email.EmailCC1 = managerInfo.OfficeEmail.Default();
                                    email.EmailCC1Name = managerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC1 == "Level 4")
                            {
                                if (headOfDepartmentInfo != null)
                                {
                                    email.EmailCC1 = headOfDepartmentInfo.OfficeEmail.Default();
                                    email.EmailCC1Name = headOfDepartmentInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC1 == "Level 5")
                            {
                                if (hrAuthoriry != null)
                                {
                                    email.EmailCC1 = hrAuthoriry.OfficeEmail.Default();
                                    email.EmailCC1Name = hrAuthoriry.EmployeeName;
                                }
                            }
                            #endregion

                            #region Email-CC2
                            if (emailConfig.EmailCC2 == "Level 1")
                            {
                                if (supervisorInfo != null)
                                {
                                    email.EmailCC2 = supervisorInfo.OfficeEmail.Default();
                                    email.EmailCC2Name = supervisorInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC2 == "Level 2")
                            {
                                if (lineManagerInfo != null)
                                {
                                    email.EmailCC2 = lineManagerInfo.OfficeEmail.Default();
                                    email.EmailCC2Name = lineManagerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC2 == "Level 3")
                            {
                                if (managerInfo != null)
                                {
                                    email.EmailCC2 = managerInfo.OfficeEmail.Default();
                                    email.EmailCC2Name = managerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC2 == "Level 4")
                            {
                                if (headOfDepartmentInfo != null)
                                {
                                    email.EmailCC2 = headOfDepartmentInfo.OfficeEmail.Default();
                                    email.EmailCC2Name = headOfDepartmentInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC2 == "Level 5")
                            {
                                if (hrAuthoriry != null)
                                {
                                    email.EmailCC2 = hrAuthoriry.OfficeEmail.Default();
                                    email.EmailCC2Name = hrAuthoriry.EmployeeName;
                                }
                            }
                            #endregion

                            #region Email-CC3
                            if (emailConfig.EmailCC3 == "Level 1")
                            {
                                if (supervisorInfo != null)
                                {
                                    email.EmailCC3 = supervisorInfo.OfficeEmail.Default();
                                    email.EmailCC3Name = supervisorInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC3 == "Level 2")
                            {
                                if (lineManagerInfo != null)
                                {
                                    email.EmailCC3 = lineManagerInfo.OfficeEmail.Default();
                                    email.EmailCC3Name = lineManagerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC3 == "Level 3")
                            {
                                if (managerInfo != null)
                                {
                                    email.EmailCC3 = managerInfo.OfficeEmail.Default();
                                    email.EmailCC3Name = managerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC3 == "Level 4")
                            {
                                if (headOfDepartmentInfo != null)
                                {
                                    email.EmailCC3 = headOfDepartmentInfo.OfficeEmail.Default();
                                    email.EmailCC3Name = headOfDepartmentInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailCC3 == "Level 5")
                            {
                                if (hrAuthoriry != null)
                                {
                                    email.EmailCC3 = hrAuthoriry.OfficeEmail.Default();
                                    email.EmailCC3Name = hrAuthoriry.EmployeeName;
                                }
                            }
                            #endregion

                            #region Email-BCC1
                            if (emailConfig.EmailBCC1 == "Level 1")
                            {
                                if (supervisorInfo != null)
                                {
                                    email.EmailBCC1 = supervisorInfo.OfficeEmail.Default();
                                    email.EmailBCC1Name = supervisorInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailBCC1 == "Level 2")
                            {
                                if (lineManagerInfo != null)
                                {
                                    email.EmailBCC1 = lineManagerInfo.OfficeEmail.Default();
                                    email.EmailBCC1Name = lineManagerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailBCC1 == "Level 3")
                            {
                                if (managerInfo != null)
                                {
                                    email.EmailBCC1 = managerInfo.OfficeEmail.Default();
                                    email.EmailBCC1Name = managerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailBCC1 == "Level 4")
                            {
                                if (headOfDepartmentInfo != null)
                                {
                                    email.EmailBCC1 = headOfDepartmentInfo.OfficeEmail.Default();
                                    email.EmailBCC1Name = headOfDepartmentInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailBCC1 == "Level 5")
                            {
                                if (hrAuthoriry != null)
                                {
                                    email.EmailBCC1 = hrAuthoriry.OfficeEmail.Default();
                                    email.EmailBCC1Name = hrAuthoriry.EmployeeName;
                                }
                            }
                            #endregion

                            #region Email-BCC2
                            if (emailConfig.EmailBCC2 == "Level 1")
                            {
                                if (supervisorInfo != null)
                                {
                                    email.EmailBCC2 = supervisorInfo.OfficeEmail.Default();
                                    email.EmailBCC2Name = supervisorInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailBCC2 == "Level 2")
                            {
                                if (lineManagerInfo != null)
                                {
                                    email.EmailBCC2 = lineManagerInfo.OfficeEmail.Default();
                                    email.EmailBCC2Name = lineManagerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailBCC2 == "Level 3")
                            {
                                if (managerInfo != null)
                                {
                                    email.EmailBCC2 = managerInfo.OfficeEmail.Default();
                                    email.EmailBCC2Name = managerInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailBCC2 == "Level 4")
                            {
                                if (headOfDepartmentInfo != null)
                                {
                                    email.EmailBCC2 = headOfDepartmentInfo.OfficeEmail.Default();
                                    email.EmailBCC2Name = headOfDepartmentInfo.EmployeeName;
                                }
                            }
                            else if (emailConfig.EmailBCC2 == "Level 5")
                            {
                                if (hrAuthoriry != null)
                                {
                                    email.EmailBCC2 = hrAuthoriry.OfficeEmail.Default();
                                    email.EmailBCC2Name = hrAuthoriry.EmployeeName;
                                }
                            }
                            #endregion

                            #region Email Send

                            var emailSetting = await _emailSettingReposiitory.GetSingleAsync(item => item.EmailFor == "Send");
                            MailMessage message = null;



                            if (presentValues.StateStatus == StateStatus.Recommended)
                            {
                                EmployeeServiceDataViewModel recommendedBy = new EmployeeServiceDataViewModel();
                                if (presentValues.CheckedBy.IsStringNumber())
                                {
                                    long checkerId = Utility.TryParseLong(presentValues.CheckedBy);
                                    recommendedBy.EmployeeId = checkerId;
                                    recommendedBy.EmployeeName = supervisorInfo.EmployeeId == recommendedBy.EmployeeId ? supervisorInfo.EmployeeName : headOfDepartmentInfo.EmployeeName;
                                    recommendedBy.EmployeeCode = supervisorInfo.EmployeeId == recommendedBy.EmployeeId ? supervisorInfo.EmployeeCode : headOfDepartmentInfo.EmployeeCode;
                                }

                                message = new MailMessage(new MailAddress(emailSetting.EmailAddress, "Leave Notification"), new MailAddress(headOfDepartmentInfo.OfficeEmail.Default(), headOfDepartmentInfo.EmployeeName));
                                message.Subject = "Leave Approval Request (Recommended)";
                                message.Body = EmailTemplate.GetLeaveEmailTemplate("Recommended", headOfDepartmentInfo.EmployeeName, presentValues, previousValue, employeeInfo, supervisorInfo, headOfDepartmentInfo, recommendedBy);
                                message.CC.Add(new MailAddress(employeeInfo.OfficeEmail.Default(), employeeInfo.EmployeeName.Default()));
                            }

                            else if (presentValues.StateStatus == StateStatus.Approved)
                            {
                                EmployeeServiceDataViewModel approvedBy = new EmployeeServiceDataViewModel();
                                if (presentValues.ApprovedBy.IsStringNumber())
                                {
                                    long approvalId = Utility.TryParseLong(presentValues.ApprovedBy);
                                    approvedBy.EmployeeId = approvalId;
                                    approvedBy.EmployeeName = headOfDepartmentInfo.EmployeeId == approvedBy.EmployeeId ? headOfDepartmentInfo.EmployeeName : supervisorInfo.EmployeeName;
                                    approvedBy.EmployeeCode = headOfDepartmentInfo.EmployeeId == approvedBy.EmployeeId ? headOfDepartmentInfo.EmployeeCode : supervisorInfo.EmployeeCode;
                                }

                                message = new MailMessage(new MailAddress(emailSetting.EmailAddress, "Leave Notification"), new MailAddress(employeeInfo.OfficeEmail.Default(), employeeInfo.EmployeeName));
                                message.Subject = "Leave Approved";
                                message.Body = EmailTemplate.GetLeaveEmailTemplate("Approved", employeeInfo.EmployeeName, presentValues, previousValue, employeeInfo, supervisorInfo, headOfDepartmentInfo, approvedBy);

                                if (supervisorInfo.EmployeeId != headOfDepartmentInfo.EmployeeId)
                                {
                                    message.CC.Add(new MailAddress(supervisorInfo.OfficeEmail.Default(), supervisorInfo.EmployeeName));
                                }

                            }

                            else if (presentValues.StateStatus == StateStatus.Pending)
                            {
                                // TO: Supervisor
                                EmployeeServiceDataViewModel editedBy = new EmployeeServiceDataViewModel();
                                message = new MailMessage(new MailAddress(emailSetting.EmailAddress, "Leave Notification"), new MailAddress(email.EmailTo, email.EmailToName));
                                if (previousValue == null)
                                {
                                    message.Subject = "Leave Approval Request";
                                    message.Body = EmailTemplate.GetLeaveEmailTemplate("Request", email.EmailToName, presentValues, previousValue, employeeInfo, supervisorInfo, headOfDepartmentInfo, editedBy);
                                }
                                else
                                {
                                    message.Subject = "Leave Approval Request (Modified)";
                                    message.Body = EmailTemplate.GetLeaveEmailTemplate("Modified", email.EmailToName, presentValues, previousValue, employeeInfo, supervisorInfo, headOfDepartmentInfo, editedBy);
                                }
                            }

                            else if (presentValues.StateStatus == StateStatus.Rejected)
                            {
                                EmployeeServiceDataViewModel rejected = new EmployeeServiceDataViewModel();
                                if (presentValues.RejectedBy.IsStringNumber())
                                {
                                    long rejectedId = Utility.TryParseLong(presentValues.RejectedBy);
                                    rejected.EmployeeId = rejectedId;
                                    rejected.EmployeeName = headOfDepartmentInfo.EmployeeId == rejected.EmployeeId ? headOfDepartmentInfo.EmployeeName : supervisorInfo.EmployeeName;
                                    rejected.EmployeeCode = headOfDepartmentInfo.EmployeeId == rejected.EmployeeId ? headOfDepartmentInfo.EmployeeCode : supervisorInfo.EmployeeCode;
                                }

                                message = new MailMessage(new MailAddress(emailSetting.EmailAddress, "Leave Notification"), new MailAddress(employeeInfo.OfficeEmail.Default(), employeeInfo.EmployeeName));
                                message.Subject = "Leave Rejected";
                                message.Body = EmailTemplate.GetLeaveEmailTemplate("Rejected", employeeInfo.EmployeeName, presentValues, previousValue, employeeInfo, supervisorInfo, headOfDepartmentInfo, rejected);

                                if (supervisorInfo.EmployeeId != headOfDepartmentInfo.EmployeeId)
                                {
                                    message.CC.Add(new MailAddress(supervisorInfo.OfficeEmail.Default(), supervisorInfo.EmployeeName));
                                }

                            }

                            else if (presentValues.StateStatus == StateStatus.Cancelled)
                            {
                                EmployeeServiceDataViewModel cancelledBy = new EmployeeServiceDataViewModel();
                                if (presentValues.CancelledBy.IsStringNumber())
                                {
                                    long cancelledId = Utility.TryParseLong(presentValues.CancelledBy);
                                    cancelledBy.EmployeeId = cancelledId;
                                    cancelledBy.EmployeeName = employeeInfo.EmployeeId == cancelledBy.EmployeeId ? employeeInfo.EmployeeName : employeeInfo.EmployeeName;
                                    cancelledBy.EmployeeCode = employeeInfo.EmployeeId == cancelledBy.EmployeeId ? employeeInfo.EmployeeCode : employeeInfo.EmployeeCode;
                                }

                                message = new MailMessage(new MailAddress(emailSetting.EmailAddress, "Leave Notification"), new MailAddress(email.EmailTo, email.EmailToName));
                                message.Subject = "Leave Cancelled";
                                message.Body = EmailTemplate.GetLeaveEmailTemplate("Cancelled", email.EmailToName, presentValues, previousValue, employeeInfo, supervisorInfo, headOfDepartmentInfo, cancelledBy);
                            }

                            else if (presentValues.StateStatus == "Cancel Approved Leave")
                            {
                                long approvedBy = Utility.TryParseLong(presentValues.ApprovedBy);
                                long checkedBy = Utility.TryParseLong(presentValues.CheckedBy);

                                var approvalPerson = (await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter
                                {
                                    EmployeeId = approvedBy.ToString()
                                }, user)).FirstOrDefault();

                                if (approvedBy == checkedBy)
                                {
                                    if (approvalPerson != null && approvalPerson.EmployeeId > 0)
                                    {
                                        presentValues.ApprovedBy = approvalPerson.EmployeeName + " [" + approvalPerson.EmployeeCode + "]";
                                        presentValues.CheckedBy = presentValues.ApprovedBy;
                                    }
                                }
                                else
                                {
                                    var checkerPerson = (await _employeeRepository.GetEmployeeServiceDataAsync(new EmployeeService_Filter
                                    {
                                        EmployeeId = checkedBy.ToString()
                                    }, user)).FirstOrDefault();

                                    if (checkerPerson != null && checkerPerson.EmployeeId > 0)
                                    {
                                        presentValues.CheckedBy = approvalPerson.EmployeeName + " [" + approvalPerson.EmployeeCode + "]";
                                    }
                                }

                                EmployeeServiceDataViewModel cancelledBy = new EmployeeServiceDataViewModel();
                                if (presentValues.CancelledBy.IsStringNumber())
                                {
                                    long cancelledId = Utility.TryParseLong(presentValues.CancelledBy);
                                    cancelledBy.EmployeeId = cancelledId;
                                    cancelledBy.EmployeeName = employeeInfo.EmployeeId == cancelledBy.EmployeeId ? employeeInfo.EmployeeName : employeeInfo.EmployeeName;
                                    cancelledBy.EmployeeCode = employeeInfo.EmployeeId == cancelledBy.EmployeeId ? employeeInfo.EmployeeCode : employeeInfo.EmployeeCode;
                                }
                                message = new MailMessage(new MailAddress(emailSetting.EmailAddress, "Leave Notification"), new MailAddress(email.EmailTo, email.EmailToName));
                                message.Subject = "Approved leave has been cancelled" + " [" + presentValues.EmployeeLeaveCode + "]";
                                message.Body = EmailTemplate.GetLeaveEmailTemplate("ApprovedLeaveCancelled", email.EmailToName, presentValues, previousValue, employeeInfo, supervisorInfo, headOfDepartmentInfo, cancelledBy);

                                message.CC.Add(new MailAddress(approvalPerson.OfficeEmail.Default(), approvalPerson.EmployeeName.Default()));
                            }

                            message.IsBodyHtml = emailSetting.IsBodyHtml;

                            if (!string.IsNullOrEmpty(email.EmailCC1))
                            {
                                message.CC.Add(new MailAddress(email.EmailCC1, email.EmailCC1Name));
                            }
                            if (!string.IsNullOrEmpty(email.EmailCC2))
                            {
                                message.CC.Add(new MailAddress(email.EmailCC2, email.EmailCC2Name));
                            }
                            if (!string.IsNullOrEmpty(email.EmailCC3))
                            {
                                message.CC.Add(new MailAddress(email.EmailCC3, email.EmailCC3Name));
                            }

                            if (!string.IsNullOrEmpty(email.EmailBCC1))
                            {
                                message.Bcc.Add(new MailAddress(email.EmailBCC1, email.EmailBCC1Name));
                            }
                            if (!string.IsNullOrEmpty(email.EmailBCC2))
                            {
                                message.Bcc.Add(new MailAddress(email.EmailBCC2, email.EmailBCC2Name));
                            }



                            SmtpClient smtp = new SmtpClient();
                            smtp.EnableSsl = emailSetting.EnableSsl;
                            smtp.UseDefaultCredentials = emailSetting.UseDefaultCredentials;
                            smtp.Port = Convert.ToInt32(emailSetting.Port);
                            smtp.Host = emailSetting.Host;
                            smtp.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.EmailPassword);
                            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                            if (AppSettings.EmailService)
                            {
                                await smtp.SendMailAsync(message);
                            }
                            return true;

                            #endregion
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "", "", user);
            }
            return false;
        }
        public async Task<ExecutionStatus> ValidatorEmployeeLeaveRequestAsync(EmployeeLeaveRequestDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var isLeaveExist = await _dapper.SqlQueryFirstAsync<EmployeeLeaveRequestViewModel>(user.Database, string.Format(@"sp_HR_EmployeeLeaveRequest_2"), new { EmployeeLeaveRequestIdNot = model.EmployeeLeaveRequestId, model.EmployeeId, model.LeaveTypeId, user.CompanyId, user.OrganizationId, model.AppliedFromDate, model.AppliedToDate, ExecutionFlag = Data.Read }, CommandType.StoredProcedure) != null;

                var leaveBalance = await _dapper.SqlQueryFirstAsync<Select2Dropdown>(user.Database, string.Format(@"sp_HR_EmployeeLeaveBalance"), new { model.LeaveTypeId, user.CompanyId, user.OrganizationId, ExecutionFlag = Data.Extension }, commandType: CommandType.StoredProcedure);

                var exceedLeaveReq = Convert.ToDecimal(leaveBalance.Max) < model.AppliedTotalDays;
                var exceedAvailablity = Convert.ToDecimal(leaveBalance.Count) < model.AppliedTotalDays;

                if (isLeaveExist || exceedLeaveReq || exceedAvailablity)
                {
                    executionStatus = new ExecutionStatus();
                    executionStatus.Status = false;
                    executionStatus.Msg = "Validation Error";
                    executionStatus.Errors = new Dictionary<string, string>();

                    if (isLeaveExist)
                    {
                        executionStatus.Errors.Add("isLeaveExist", "Leave exist within this date");
                    }
                    if (exceedLeaveReq)
                    {
                        executionStatus.Errors.Add("exceedLeaveReq", "Exceed Max Leave Request");
                    }
                    if (exceedAvailablity)
                    {
                        executionStatus.Errors.Add("exceedAvailablity", "Exceed Leave Availablity");
                    }
                }
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "ValidatorEmployeeLeaveRequestAsync", user);
            }
            return executionStatus;
        }
        private async Task EmailSend(string toEmail, string empDtls, string leaveTypeName, LeaveRequestEmailSendViewModel email, EmailSettingDataWithIdViewModel emailSetting, string userName, string userEmail, string remarks, string status, string flag)
        {
            var Subject = "";
            try
            {

                MailMessage message = new MailMessage();
                message.From = new MailAddress(emailSetting.EmailAddress, "Leave Notification");

                if (!string.IsNullOrEmpty(email.EmailCC1))
                {
                    message.CC.Add(new MailAddress(email.EmailCC1));
                }
                if (!string.IsNullOrEmpty(email.EmailCC2))
                {
                    message.CC.Add(new MailAddress(email.EmailCC2));
                }
                if (!string.IsNullOrEmpty(email.EmailCC3))
                {
                    message.CC.Add(new MailAddress(email.EmailCC3));
                }

                if (!string.IsNullOrEmpty(email.EmailBCC1))
                {
                    message.Bcc.Add(new MailAddress(email.EmailBCC1));
                }
                if (!string.IsNullOrEmpty(email.EmailBCC2))
                {
                    message.Bcc.Add(new MailAddress(email.EmailBCC2));
                }

                if (flag == "Request")
                {
                    Subject = "Leave Approval Request";
                    message.To.Add(new MailAddress(toEmail));
                }
                else if (flag == "Modified")
                {
                    Subject = "Leave Approval Request (Modified)";
                    message.To.Add(new MailAddress(toEmail, "Yeasin Ahmed"));
                }
                else if (flag == "Cancelled")
                {
                    Subject = "Requested Leave has been Cancelled";
                    message.To.Add(new MailAddress(toEmail));
                }
                else if (flag == "Approved")
                {
                    message.To.Add(new MailAddress(toEmail));
                    if (status == "Approved")
                    {
                        Subject = "Leave Request Approved";
                    }
                    else
                    if (status == "Recheck")
                    {
                        Subject = "Leave Request Recheck";
                    }
                    else
                    if (status == "Cancelled")
                    {
                        Subject = "Leave Request Cancelled";
                    }
                }

                message.Subject = Subject;
                message.IsBodyHtml = emailSetting.IsBodyHtml;
                message.Body = EmailTemplate.GetEmailTemplate(flag, empDtls, leaveTypeName, userName, remarks, status);

                SmtpClient smtp = new SmtpClient();
                smtp.EnableSsl = emailSetting.EnableSsl;
                smtp.UseDefaultCredentials = emailSetting.UseDefaultCredentials;
                smtp.Port = Convert.ToInt32(emailSetting.Port);
                smtp.Host = emailSetting.Host;
                smtp.Credentials = new NetworkCredential(emailSetting.EmailAddress, emailSetting.EmailPassword);
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                await smtp.SendMailAsync(message);
            }
            catch (Exception)
            {

            }
        }
        public async Task<DBResponse<EmployeeLeaveRequestViewModel>> GetSubordinatesEmployeeLeaveRequestAsync(LeaveRequest_Filter filter, AppUser user)
        {
            DBResponse<EmployeeLeaveRequestViewModel> data = new DBResponse<EmployeeLeaveRequestViewModel>();
            try
            {
                data = await _leaveRequestRepository.GetSubordinatesEmployeeLeaveRequestAsync(filter, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetSubordinatesEmployeeLeaveRequestAsync", user);
            }
            return data;
        }

        public async Task<ExecutionStatus> ApprovedLeaveCancellationAsync(ApprovedLeaveCancellationDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                executionStatus = await _leaveRequestRepository.ApprovedLeaveCancellationAsync(model, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetSubordinatesEmployeeLeaveRequestAsync", user);
            }
            return executionStatus;
        }

        public async Task<EmployeeLeaveRequestInfoAndDetailViewModel> GetEmployeeLeaveRequestInfoAndDetailById(long id, AppUser user)
        {
            EmployeeLeaveRequestInfoAndDetailViewModel data = new EmployeeLeaveRequestInfoAndDetailViewModel();
            try
            {
                data = await _leaveRequestRepository.GetEmployeeLeaveRequestInfoAndDetailById(id, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveRequestBusiness", "GetEmployeeLeaveRequestInfoAndDetailById", user);
            }
            return data;
        }
    }
}
