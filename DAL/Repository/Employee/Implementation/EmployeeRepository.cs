using Dapper;
using System.Data;
using Shared.Services;
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using Shared.Employee.Domain.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using Shared.OtherModels.Response;
using Shared.Employee.DTO.Info;
using Shared.Helpers;
using DAL.Repository.Employee.Interface;
using DAL.DapperObject.Interface;

namespace DAL.Repository.Employee.Implementation
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IDiscontinuedEmployeeRepository _discontinuedEmployeeRepository;

        public EmployeeRepository(IDALSysLogger sysLogger, IDapperData dapper, IDiscontinuedEmployeeRepository discontinuedEmployeeRepository)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _discontinuedEmployeeRepository = discontinuedEmployeeRepository;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeInformation>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }



        public Task<IEnumerable<EmployeeInformation>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<EmployeeInformation> GetByCodeAsync(string code, AppUser user)
        {
            EmployeeInformation employeeInformation = null;
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeInformation Where EmployeeCode=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeeInformation = await _dapper.SqlQueryFirstAsync<EmployeeInformation>(user.Database, query, new { Id = code, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetByIdAsync", user);
            }
            return employeeInformation;
        }

        public async Task<EmployeeInformation> GetByIdAsync(long id, AppUser user)
        {
            EmployeeInformation employeeInformation = null;
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeInformation Where EmployeeId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeeInformation = await _dapper.SqlQueryFirstAsync<EmployeeInformation>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetByIdAsync", user);
            }
            return employeeInformation;
        }

        public async Task<EmployeeDetail> GetEmployeeDetailByIdAsync(long id, AppUser user)
        {
            EmployeeDetail employeeDetail = null;
            try
            {
                var query = $@"SELECT * FROM HR_EmployeeDetail Where EmployeeId=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                employeeDetail = await _dapper.SqlQueryFirstAsync<EmployeeDetail>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeDetailByIdAsync", user);
            }
            return employeeDetail;
        }

        public async Task<IEnumerable<EmployeeServiceDataViewModel>> GetEmployeeServiceDataAsync(EmployeeService_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeServiceDataViewModel> list = new List<EmployeeServiceDataViewModel>();
            try
            {
                var sp_name = @"SELECT EMP.EmployeeId,EMP.EmployeeCode,[EmployeeName]=EMP.FullName,EMP.BranchId,'' AS BranchName,GRD.GradeId,GRD.GradeName,EMP.DesignationId,DEG.DesignationName,
	EMP.DepartmentId,DPT.DepartmentName,SEC.SectionName,EMP.SectionId,SUB.SubSectionName,EMP.SubSectionId,EMP.IsActive,EMP.TerminationDate,EMP.TerminationStatus,EMP.OfficeEmail, EMP.OfficeMobile, DTL.PresentAddress,
	DTL.MaritalStatus,PreviousReviewId=ISNULL((Select TOP 1 SalaryReviewInfoId From Payroll_SalaryReviewInfo Where EmployeeId=emp.EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND IsApproved=1 Order By SalaryReviewInfoId desc),0)
	FROM HR_EmployeeInformation EMP
	LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId = DTL.EmployeeId
	LEFT JOIN HR_Designations DEG ON EMP.DesignationId = DEG.DesignationId
	LEFT JOIN HR_Grades GRD ON DEG.GradeId = GRD.GradeId
	LEFT JOIN HR_Departments DPT ON EMP.DepartmentId = DPT.DepartmentId
	LEFT JOIN HR_Sections SEC ON EMP.SectionId = SEC.SectionId
	LEFT JOIN HR_SubSections SUB ON EMP.SubSectionId = SUB.SubSectionId
	Where 1=1
	 AND (@EmployeeId IS NULL OR @EmployeeId=0 OR EMP.EmployeeId=@EmployeeId)
	 AND (@EmployeeCode IS NULL OR @EmployeeCode='' OR EMP.EmployeeCode=@EmployeeCode)
	 AND (@EmployeeName IS NULL OR @EmployeeName='' OR EMP.FullName=@EmployeeName)
	 AND (@Gender IS NULL OR @Gender='' OR DTL.Gender=@Gender)
	 AND (@BranchId IS NULL OR @BranchId=0 OR EMP.BranchId=@BranchId)
	 AND (@GradeId IS NULL OR @GradeId=0 OR EMP.GradeId=@GradeId)
	 AND (@DesignationId IS NULL OR @DesignationId=0 OR EMP.DesignationId=@DesignationId)
	 AND (@ZoneId IS NULL OR @ZoneId=0)
	 AND (@UnitId IS NULL OR @UnitId=0)
     AND (@JobType IS NULL OR @JobType ='' OR EMP.JobType=@JobType)
	 AND (@DepartmentId IS NULL OR @DepartmentId=0 OR EMP.DepartmentId=@DepartmentId)
	 AND (@SectionId IS NULL OR @SectionId=0 OR EMP.SectionId=@SectionId)
	 AND (@SubSectionId IS NULL OR @SubSectionId=0 OR EMP.SubSectionId=@SubSectionId)
	 AND (@IsActive IS NULL OR @IsActive ='' OR EMP.IsActive=@IsActive)
	 AND (@IncludedEmployeeCode IS NULL OR @IncludedEmployeeCode ='' OR EMP.EmployeeCode IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@IncludedEmployeeCode,',')))
	 AND (@IncludedEmployeeId IS NULL OR @IncludedEmployeeId ='' OR EMP.EmployeeId IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@IncludedEmployeeId,',')))
	 AND (@ExcludedEmployeeCode IS NULL OR @ExcludedEmployeeCode ='' OR ((@ExcludedEmployeeCode ='' OR EMP.EmployeeCode NOT IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@ExcludedEmployeeCode,',')))))
	 AND (@ExcludedEmployeeId IS NULL OR @ExcludedEmployeeId ='' OR ((@ExcludedEmployeeId ='' OR EMP.EmployeeId NOT IN (Select Convert( bigint,[Value]) from fn_split_string_to_column(@ExcludedEmployeeId,',')))))
	 AND (@MartialStatus IS NULL OR @MartialStatus ='' OR DTL.MaritalStatus = @MartialStatus)
	 AND (@TerminationStatus IS NULL OR @TerminationStatus ='' OR EMP.TerminationStatus = @TerminationStatus)
	AND EMP.CompanyId=@CompanyId
	AND EMP.OrganizationId=@OrganizationId
	Order By EMP.EmployeeId";
                var parameters = DapperParam.AddParams(filter, user, addUserId: false);
                list = await _dapper.SqlQueryListAsync<EmployeeServiceDataViewModel>(user.Database, sp_name.Trim(), parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeServiceDataAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> UpdateByUploaderAsync(EmployeeUploadInformation employee, AppUser user)
        {
            ExecutionStatus executionStatus = null;
            try
            {
                var employeeInfoInDb = await GetByIdAsync(Utility.TryParseLong(employee.EmployeeId), user);
                var employeeDetailInDb = await GetEmployeeDetailByIdAsync(Utility.TryParseLong(employee.EmployeeId), user);
                using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                {
                    connection.Open();
                    {
                        bool isSuccessful = false;
                        using (var transaction = connection.BeginTransaction())
                        {
                            try
                            {
                                if (employeeInfoInDb != null && employeeInfoInDb.EmployeeId > 0)
                                {
                                    if ((employeeInfoInDb.IsApproved ?? false) == true)
                                    {

                                        #region Change Branch
                                        var branchIdExistingValue = employeeInfoInDb.BranchId.HasValue ? employeeInfoInDb.BranchId.Value : 0;
                                        var newBranch = Utility.TryParseLong(employee.BranchId);

                                        if (newBranch > 0 && branchIdExistingValue != newBranch)
                                        {

                                            var query = $@"Update HR_EmployeeTransferProposal
					                        SET InActiveBy = @UserId,InActiveDate=GETDATE(),IsActive=0
					                        Where EmployeeId=@EmployeeId AND Head='Branch' AND CompanyId=@CompanyId 
					                        AND OrganizationId=@OrganizationId AND IsActive=1";

                                            var rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            query = $@"INSERT INTO HR_EmployeeTransferProposal(EmployeeId, Head, Flag, ExistingValue, ExistingText, ProposalValue, ProposalText, IsActive, StateStatus, EffectiveDate,ActiveDate, ActiveBy, CreatedBy, CreatedDate,OrganizationId, CompanyId, BranchId, ApprovedBy, ApprovedDate, ApprovalRemarks)";
                                            query += $@"VALUES(@EmployeeId,'Branch','Transfer',@BranchIdExistingValue,'',@BranchId,'',1,'Approved',GETDATE(),GETDATE(),@UserId,@UserId,GETDATE(),@OrganizationId,@CompanyId,NULL,@UserId,GETDATE(),NULL)";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                BranchIdExistingValue = branchIdExistingValue,
                                                BranchId = newBranch,
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);
                                        }
                                        #endregion

                                        #region Change Department

                                        var departmentIdExistingValue = employeeInfoInDb.DepartmentId.HasValue ? employeeInfoInDb.DepartmentId.Value : 0;
                                        var newDepartment = Utility.TryParseLong(employee.DepartmentId);

                                        if (newDepartment > 0 && departmentIdExistingValue != newDepartment)
                                        {
                                            var query = $@"Update HR_EmployeeTransferProposal
					                        SET InActiveBy = @UserId,InActiveDate=GETDATE(),IsActive=0
					                        Where EmployeeId=@EmployeeId AND Head='Department' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId 
                                            AND IsActive=1";

                                            var rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            query = $@"INSERT INTO HR_EmployeeTransferProposal(EmployeeId, Head, Flag, ExistingValue, ExistingText, ProposalValue, ProposalText, IsActive, StateStatus, EffectiveDate,ActiveDate, ActiveBy, CreatedBy, CreatedDate,OrganizationId, CompanyId, BranchId, ApprovedBy, ApprovedDate, ApprovalRemarks)";
                                            query += $@"VALUES(@EmployeeId,'Department','Transfer',@DepartmentIdExistingValue,'',@DepartmentId,'',1,'Approved',GETDATE(),GETDATE(),@UserId,@UserId,GETDATE(),@OrganizationId,@CompanyId,NULL,@UserId,GETDATE(),NULL)";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                DepartmentIdExistingValue = departmentIdExistingValue,
                                                DepartmentId = newDepartment,
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                        }

                                        #endregion

                                        #region Change Section
                                        var sectionIdExistingValue = employeeInfoInDb.SectionId.HasValue ? employeeInfoInDb.SectionId.Value : 0;
                                        var newSection = Utility.TryParseLong(employee.SectionId);

                                        if (newSection > 0 && sectionIdExistingValue != newSection)
                                        {
                                            var query = $@"Update HR_EmployeeTransferProposal
					                        SET InActiveBy = @UserId,InActiveDate=GETDATE(),IsActive=0
					                        Where EmployeeId=@EmployeeId AND Head='Section' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId 
                                            AND IsActive=1";

                                            var rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            query = $@"INSERT INTO HR_EmployeeTransferProposal(EmployeeId, Head, Flag, ExistingValue, ExistingText, ProposalValue, ProposalText, IsActive, StateStatus, EffectiveDate,ActiveDate, ActiveBy, CreatedBy, CreatedDate,OrganizationId, CompanyId, BranchId, ApprovedBy, ApprovedDate, ApprovalRemarks)";
                                            query += $@"VALUES(@EmployeeId,'Section','Transfer',@SectionIdExistingValue,'',@SectionId,'',1,'Approved',GETDATE(),GETDATE(),@UserId,@UserId,GETDATE(),@OrganizationId,@CompanyId,NULL,@UserId,GETDATE(),NULL)";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                SectionIdExistingValue = departmentIdExistingValue,
                                                SectionId = newSection,
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);
                                        }
                                        #endregion

                                        #region Change Sub Section
                                        var subSectionIdExistingValue = employeeInfoInDb.SubSectionId.HasValue ? employeeInfoInDb.SubSectionId.Value : 0;
                                        var newSubSection = Utility.TryParseLong(employee.SubSectionId);
                                        if (newSubSection > 0 && subSectionIdExistingValue != newSubSection)
                                        {

                                            var query = $@"Update HR_EmployeeTransferProposal
					                        SET InActiveBy = @UserId,InActiveDate=GETDATE(),IsActive=0
					                        Where EmployeeId=@EmployeeId AND Head='SubSection' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId 
                                            AND IsActive=1";

                                            var rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            query = $@"INSERT INTO HR_EmployeeTransferProposal(EmployeeId, Head, Flag, ExistingValue, ExistingText, ProposalValue, ProposalText, IsActive, StateStatus, EffectiveDate,ActiveDate, ActiveBy, CreatedBy, CreatedDate,OrganizationId, CompanyId, BranchId, ApprovedBy, ApprovedDate, ApprovalRemarks)";
                                            query += $@"VALUES(@EmployeeId,'SubSection','Transfer',@SubSectionIdExistingValue,'',@SubSectionId,'',1,'Approved',GETDATE(),GETDATE(),@UserId,@UserId,GETDATE(),@OrganizationId,@CompanyId,NULL,@UserId,GETDATE(),NULL)";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                SubSectionIdExistingValue = departmentIdExistingValue,
                                                SubSectionId = newSubSection,
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);
                                        }

                                        #endregion

                                        #region Change Cost Center
                                        var costCenterIdExistingValue = employeeInfoInDb.CostCenterId.HasValue ? employeeInfoInDb.CostCenterId.Value : 0;
                                        var newCostCenter = Utility.TryParseLong(employee.CostCenterId);

                                        if (newCostCenter > 0 && costCenterIdExistingValue != newCostCenter)
                                        {
                                            var query = $@"Update HR_EmployeeTransferProposal
					                        SET InActiveBy = @UserId,InActiveDate=GETDATE(),IsActive=0
					                        Where EmployeeId=@EmployeeId AND Head='CostCenter' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId 
                                            AND IsActive=1";

                                            var rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            query = $@"INSERT INTO HR_EmployeeTransferProposal(EmployeeId, Head, Flag, ExistingValue, ExistingText, ProposalValue, ProposalText, IsActive, StateStatus, EffectiveDate,ActiveDate, ActiveBy, CreatedBy, CreatedDate,OrganizationId, CompanyId, BranchId, ApprovedBy, ApprovedDate, ApprovalRemarks)";
                                            query += $@"VALUES(@EmployeeId,'CostCenter','Transfer',@CostCenterIdExistingValue,'',@CostCenterId,'',1,'Approved',GETDATE(),GETDATE(),@UserId,@UserId,GETDATE(),@OrganizationId,@CompanyId,NULL,@UserId,GETDATE(),NULL)";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                CostCenterIdExistingValue = costCenterIdExistingValue,
                                                CostCenterId = newCostCenter,
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);
                                        }
                                        #endregion

                                        #region Change Grade
                                        var gradeIdExistingValue = employeeInfoInDb.GradeId.HasValue ? employeeInfoInDb.GradeId.Value : 0;
                                        var newGrade = Utility.TryParseLong(employee.GradeId);

                                        if (newGrade > 0 && gradeIdExistingValue != newGrade)
                                        {
                                            var query = $@"Update HR_EmployeePromotionProposal
					                        SET InActiveBy = @UserId,InActiveDate=GETDATE(),IsActive=0
					                        Where EmployeeId=@EmployeeId AND Head='Grade' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId 
                                            AND IsActive=1";

                                            var rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            query = $@"INSERT INTO HR_EmployeePromotionProposal(EmployeeId, Head, Flag, ExistingValue, ExistingText, ProposalValue, ProposalText, IsActive, StateStatus, EffectiveDate,ActiveDate, ActiveBy, CreatedBy, CreatedDate,OrganizationId, CompanyId, BranchId, ApprovedBy, ApprovedDate, ApprovalRemarks)";
                                            query += $@"VALUES(@EmployeeId,'Grade','Promotion',@GradeIdExistingValue,'',@GradeId,'',1,'Approved',GETDATE(),GETDATE(),@UserId,@UserId,GETDATE(),@OrganizationId,@CompanyId,NULL,@UserId,GETDATE(),NULL)";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                GradeIdExistingValue = gradeIdExistingValue,
                                                GradeId = newGrade,
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);
                                        }
                                        #endregion

                                        #region Change Designation
                                        var desiganationIdExistingValue = employeeInfoInDb.DesignationId.HasValue ? employeeInfoInDb.DesignationId.Value : 0;
                                        var newDesiganation = Utility.TryParseLong(employee.DesignationId);

                                        if (newDesiganation > 0 && desiganationIdExistingValue != newDesiganation)
                                        {
                                            var query = $@"Update HR_EmployeePromotionProposal
					                        SET InActiveBy = @UserId,InActiveDate=GETDATE(),IsActive=0
					                        Where EmployeeId=@EmployeeId AND Head='Desgination' AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND IsActive=1";

                                            var rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            query = $@"INSERT INTO HR_EmployeePromotionProposal(EmployeeId, Head, Flag, ExistingValue, ExistingText, ProposalValue, ProposalText, IsActive, StateStatus, EffectiveDate,ActiveDate, ActiveBy, CreatedBy, CreatedDate,OrganizationId, CompanyId, BranchId, ApprovedBy, ApprovedDate, ApprovalRemarks)";
                                            query += $@"VALUES(@EmployeeId,'Desgination','Promotion',@DesignationIdExistingValue,'',@DesignationId,'',1,'Approved',GETDATE(),GETDATE(),@UserId,@UserId,GETDATE(),@OrganizationId,@CompanyId,NULL,@UserId,GETDATE(),NULL)";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                DesignationIdExistingValue = desiganationIdExistingValue,
                                                DesignationId = newDesiganation,
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);
                                        }
                                        #endregion

                                        #region Change Confirmation Date
                                        var newConfirmationDate = DateTimeExtension.TryParseDate(employee.ConfirmationDate);
                                        if (employeeInfoInDb.DateOfConfirmation.HasValue == false && newConfirmationDate.HasValue == true)
                                        {
                                            var query = $@"Update HR_EmployeeInformation
					                        SET DateOfConfirmation = @ConfirmationDate, 
					                        IsConfirmed = (CASE WHEN CAST(@ConfirmationDate AS DATE) <= CAST(GETDATE() AS DATE) THEN 1 ELSE 0 END), 
					                        UpdatedBy=@UserId, UpdatedDate=GETDATE()
					                        Where EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                                            var rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                ConfirmationDate = newConfirmationDate,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);

                                            query = $@"INSERT INTO [dbo].[HR_EmployeeConfirmationProposal](EmployeeId, ConfirmationDate, TotalRatingScore, AppraiserId, AppraiserComment, EffectiveDate, StateStatus, CreatedBy, CreatedDate,OrganizationId, CompanyId, BranchId, ApprovedBy, ApprovedDate, ApprovalRemarks)";
                                            query += $@"VALUES(@EmployeeId,@ConfirmationDate,0,@UserId,0,@ConfirmationDate,'Approved',@UserId, GETDATE(),@OrganizationId,@CompanyId,0,@UserId,GETDATE(),NULL)";

                                            rawAffected = await connection.ExecuteAsync(query, new
                                            {
                                                employeeInfoInDb.EmployeeId,
                                                ConfirmationDate = newConfirmationDate,
                                                UserId = user.ActionUserId,
                                                user.CompanyId,
                                                user.OrganizationId
                                            }, transaction);
                                        }
                                        #endregion

                                        #region Change PF Activation

                                        #endregion
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.Salutation) == false)
                                    {
                                        if (employee.Salutation.ReplaceWhitespace() != "")
                                        {
                                            employeeInfoInDb.Salutation = employee.Salutation.ReplaceWhitespace();
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.FirstName) == false)
                                    {
                                        if (employee.FirstName.ReplaceWhitespace() != "")
                                        {
                                            employeeInfoInDb.FirstName = employee.FirstName.ReplaceWhitespace();
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.MiddleName) == false)
                                    {
                                        if (employee.MiddleName.ReplaceWhitespace() != "")
                                        {
                                            employeeInfoInDb.MiddleName = employee.MiddleName.ReplaceWhitespace();
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.LastName) == false)
                                    {
                                        if (employee.LastName.ReplaceWhitespace() != "")
                                        {
                                            employeeInfoInDb.LastName = employee.LastName.ReplaceWhitespace();
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.NickName) == false)
                                    {
                                        if (employee.NickName.ReplaceWhitespace() != "")
                                        {
                                            employeeInfoInDb.NickName = employee.NickName.ReplaceWhitespace();
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.BranchId) == false)
                                    {
                                        if (Utility.TryParseLong(employee.BranchId) > 0)
                                        {
                                            employeeInfoInDb.BranchId = Utility.TryParseLong(employee.BranchId);
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.GradeId) == false)
                                    {
                                        if (Utility.TryParseLong(employee.GradeId) > 0)
                                        {
                                            employeeInfoInDb.GradeId = Utility.TryParseInt(employee.GradeId);
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.DesignationId) == false)
                                    {
                                        if (Utility.TryParseLong(employee.DesignationId) > 0)
                                        {
                                            employeeInfoInDb.DesignationId = Utility.TryParseInt(employee.DesignationId);
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.DepartmentId) == false)
                                    {
                                        if (Utility.TryParseLong(employee.DepartmentId) > 0)
                                        {
                                            employeeInfoInDb.DepartmentId = Utility.TryParseInt(employee.DepartmentId);
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.SectionId) == false)
                                    {
                                        if (Utility.TryParseLong(employee.SectionId) > 0)
                                        {
                                            employeeInfoInDb.SectionId = Utility.TryParseInt(employee.SectionId);
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.SubSectionId) == false)
                                    {
                                        if (Utility.TryParseLong(employee.SubSectionId) > 0)
                                        {
                                            employeeInfoInDb.SubSectionId = Utility.TryParseInt(employee.SubSectionId);
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.CostCenterId) == false)
                                    {
                                        if (Utility.TryParseLong(employee.CostCenterId) > 0)
                                        {
                                            employeeInfoInDb.CostCenterId = Utility.TryParseInt(employee.CostCenterId);
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.WorkShiftId) == false)
                                    {
                                        if (Utility.TryParseLong(employee.WorkShiftId) > 0)
                                        {
                                            employeeInfoInDb.WorkShiftId = Utility.TryParseInt(employee.WorkShiftId);
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.JoiningDate) == false)
                                    {
                                        var joiningDate = DateTimeExtension.TryParseDate(employee.JoiningDate);
                                        if (DateTimeExtension.TryParseDate(employee.JoiningDate) != null)
                                        {
                                            employeeInfoInDb.DateOfJoining = joiningDate;
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.ConfirmationDate) == false)
                                    {
                                        var confirmationDate = DateTimeExtension.TryParseDate(employee.ConfirmationDate);
                                        if (DateTimeExtension.TryParseDate(employee.ConfirmationDate) != null)
                                        {
                                            employeeInfoInDb.DateOfConfirmation = confirmationDate;
                                            employeeInfoInDb.IsConfirmed = confirmationDate.Value.IsThisDateExistInThisMonth();
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.AppointmentDate) == false)
                                    {
                                        var appointmentDate = DateTimeExtension.TryParseDate(employee.AppointmentDate);
                                        if (DateTimeExtension.TryParseDate(employee.AppointmentDate) != null)
                                        {
                                            employeeInfoInDb.AppointmentDate = appointmentDate;
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.LastWorkingDate) == false)
                                    {
                                        var terminationDate = DateTimeExtension.TryParseDate(employee.LastWorkingDate);
                                        if (DateTimeExtension.TryParseDate(employee.AppointmentDate) != null)
                                        {
                                            employeeInfoInDb.TerminationDate = terminationDate;
                                            employeeInfoInDb.TerminationStatus = StateStatus.Approved;
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.PFActivationDate) == false)
                                    {
                                        var pfActivationDate = DateTimeExtension.TryParseDate(employee.PFActivationDate);
                                        if (DateTimeExtension.TryParseDate(employee.AppointmentDate) != null)
                                        {
                                            employeeInfoInDb.PFActivationDate = pfActivationDate;
                                            employeeInfoInDb.IsPFMember = pfActivationDate.Value.IsThisDateExistInThisMonth();
                                        }
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.OfficeMobile) == false)
                                    {
                                        employeeInfoInDb.OfficeMobile = employee.OfficeMobile.ReplaceWhitespace();
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.OfficeEmail) == false)
                                    {
                                        employeeInfoInDb.OfficeEmail = employee.OfficeEmail.ReplaceWhitespace();
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.ReferenceId) == false)
                                    {
                                        employeeInfoInDb.ReferenceNo = employee.ReferenceId.ReplaceWhitespace();
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.FingureId) == false)
                                    {
                                        employeeInfoInDb.FingerID = employee.FingureId.ReplaceWhitespace();
                                    }
                                    if (Utility.IsNullEmptyOrWhiteSpace(employee.JobType) == false)
                                    {
                                        if (Jobtype.IsValid(employee.JobType) == true)
                                        {
                                            employeeInfoInDb.JobType = employee.JobType.ReplaceWhitespace();
                                        }
                                    }
                                    employeeInfoInDb.UpdatedBy = user.ActionUserId;
                                    employeeInfoInDb.UpdatedDate = DateTime.Now;

                                    var parameters = DapperParam.GetKeyValuePairsDynamic(employeeInfoInDb, new string[] { "EmployeeId", "EmployeeFamilyInfos", "EmployeeRelativesInfos", "EmployeeHierarchies", "EmployeeEducations", "EmployeeExperiences", "EmployeeSkills" }, addBaseProperty: true);

                                    var sqlQuery = Utility.GenerateUpdateQuery(tableName: "HR_EmployeeInformation", paramkeys: parameters.Select(x => x.Key).ToList());
                                    sqlQuery += $"WHERE EmployeeId = @Id";
                                    parameters.Add("Id", employeeInfoInDb.EmployeeId);

                                    int updateRaw = await connection.ExecuteAsync(sqlQuery, parameters, transaction);

                                    if (updateRaw > 0)
                                    {
                                        isSuccessful = true;
                                        transaction.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeRepository", "UpdateByUploaderAsync", user);
                            }
                        }

                        using (var transaction = connection.BeginTransaction())
                        {
                            if (employeeDetailInDb != null && employeeDetailInDb.EmployeeDetailId > 0)
                            {

                                employeeDetailInDb.FatherName = employee.FatherName != null && (employeeDetailInDb.FatherName ?? "") != (employee.FatherName ?? "") ? employee.FatherName : employeeDetailInDb.FatherName ?? "";
                                employeeDetailInDb.MotherName = employee.MotherName != null && (employeeDetailInDb.MotherName ?? "") != (employee.MotherName ?? "") ? employee.MotherName : employeeDetailInDb.MotherName ?? "";
                                employeeDetailInDb.SpouseName = employee.SpouseName != null && (employeeDetailInDb.SpouseName ?? "") != (employee.SpouseName ?? "") ? employee.SpouseName : employeeDetailInDb.SpouseName ?? "";
                                employeeDetailInDb.PersonalMobileNo = employee.PersonalMobileNumber != null && (employeeDetailInDb.PersonalMobileNo ?? "") != (employee.PersonalMobileNumber ?? "") ? employee.PersonalMobileNumber : employeeDetailInDb.PersonalMobileNo ?? "";
                                employeeDetailInDb.PersonalEmailAddress = employee.PersonalEmail != null && (employeeDetailInDb.PersonalEmailAddress ?? "") != (employee.PersonalEmail ?? "") ? employee.PersonalEmail : employeeDetailInDb.PersonalEmailAddress ?? "";
                                employeeDetailInDb.DateOfBirth = employee.DateOfBirth != null && employeeDetailInDb.DateOfBirth.Value != DateTimeExtension.TryParseDate(employee.DateOfBirth) ? DateTimeExtension.TryParseDate(employee.DateOfBirth) : employeeDetailInDb.DateOfBirth.Value;

                                employeeDetailInDb.Gender = employee.Gender != null && employeeDetailInDb.Gender != employee.Gender ? employee.Gender : employeeDetailInDb.Gender;
                                employeeDetailInDb.BloodGroup = employee.BloodGroup != null && employeeDetailInDb.BloodGroup != employee.BloodGroup ? employee.BloodGroup : employeeDetailInDb.BloodGroup;
                                employeeDetailInDb.MaritalStatus = employee.MaritalStatus != null && employeeDetailInDb.MaritalStatus != employee.MaritalStatus ? employee.MaritalStatus : employeeDetailInDb.MaritalStatus;
                                employeeDetailInDb.IsResidential = employee.IsResidential.IsBoolean() != null && (employeeDetailInDb.IsResidential ?? false) != employee.IsResidential.IsBoolean() ? employee.IsResidential.IsBoolean() : employeeDetailInDb.IsResidential;
                                employeeDetailInDb.PresentAddress = employee.PresentAddress.Default() != "" && employeeDetailInDb.PresentAddress != employee.PresentAddress.Default() ? employee.PresentAddress : employeeDetailInDb.PresentAddress;
                                employeeDetailInDb.NumberOfChild = employee.NumberOfChild.Default() != "" && employeeDetailInDb.NumberOfChild != employee.NumberOfChild.Default() ? employee.NumberOfChild : employeeDetailInDb.NumberOfChild;
                                employeeDetailInDb.PresentAddressCity = employee.PresentAddressCity.Default() != "" && employeeDetailInDb.PresentAddressCity != employee.PresentAddressCity.Default() ? employee.PresentAddressCity : employeeDetailInDb.PresentAddressCity;
                                employeeDetailInDb.PresentAddressContactNo = employee.PresentAddressContactNo.Default() != "" && employeeDetailInDb.PresentAddressContactNo != employee.PresentAddressContactNo.Default() ? employee.PresentAddressContactNo : employeeDetailInDb.PresentAddressContactNo;

                                employeeDetailInDb.PresentAddressZipCode = employee.PresentAddressZipCode.Default() != "" && employeeDetailInDb.PresentAddressZipCode != employee.PresentAddressZipCode.Default() ? employee.PresentAddressZipCode : employeeDetailInDb.PresentAddressZipCode;

                                employeeDetailInDb.PermanentAddress = employee.PermanentAddress.Default() != "" && employeeDetailInDb.PermanentAddress != employee.PermanentAddress.Default() ? employee.PermanentAddress : employeeDetailInDb.PermanentAddress;

                                employeeDetailInDb.PermanentAddressDistrict = employee.PermanentAddressDistrict.Default() != "" && employeeDetailInDb.PermanentAddressDistrict != employee.PermanentAddressDistrict.Default() ? employee.PermanentAddressDistrict : employeeDetailInDb.PermanentAddressDistrict;

                                employeeDetailInDb.PermanentAddressUpazila = employee.PermanentAddressUpazila.Default() != "" && employeeDetailInDb.PermanentAddressUpazila != employee.PermanentAddressUpazila.Default() ? employee.PermanentAddressUpazila : employeeDetailInDb.PermanentAddressUpazila;

                                employeeDetailInDb.PermanentAddressContactNumber = employee.PermanentAddressContactNumber.Default() != "" && employeeDetailInDb.PermanentAddressContactNumber != employee.PermanentAddressContactNumber.Default() ? employee.PermanentAddressContactNumber : employeeDetailInDb.PermanentAddressContactNumber;

                                employeeDetailInDb.PermanentAddressZipCode = employee.PermanentAddressZipCode.Default() != "" && employeeDetailInDb.PermanentAddressZipCode != employee.PermanentAddressZipCode.Default() ? employee.PermanentAddressZipCode : employeeDetailInDb.PermanentAddressZipCode;

                                employeeDetailInDb.EmergencyContactPerson = employee.EmergencyContactPerson1.Default() != "" && employeeDetailInDb.EmergencyContactPerson != employee.EmergencyContactPerson1.Default() ? employee.EmergencyContactPerson1 : employeeDetailInDb.EmergencyContactPerson;

                                employeeDetailInDb.RelationWithEmergencyContactPerson = employee.RelationWithEmergencyContactPerson1.Default() != "" && employeeDetailInDb.RelationWithEmergencyContactPerson != employee.RelationWithEmergencyContactPerson1.Default() ? employee.RelationWithEmergencyContactPerson1 : employeeDetailInDb.RelationWithEmergencyContactPerson;

                                employeeDetailInDb.EmergencyContactNo = employee.EmergencyContactNoPerson1.Default() != "" && employeeDetailInDb.EmergencyContactPerson != employee.EmergencyContactPerson1.Default() ? employee.EmergencyContactPerson1 : employeeDetailInDb.EmergencyContactPerson;

                                employeeDetailInDb.EmergencyContactAddress = employee.EmergencyContactAddressPerson1.Default() != "" && employeeDetailInDb.EmergencyContactAddress != employee.EmergencyContactAddressPerson1.Default() ? employee.EmergencyContactAddressPerson1 : employeeDetailInDb.EmergencyContactAddress;

                                employeeDetailInDb.EmergencyContactEmailAddress = employee.EmergencyContactEmailAddressPerson1.Default() != "" && employeeDetailInDb.EmergencyContactEmailAddress != employee.EmergencyContactEmailAddressPerson1.Default() ? employee.EmergencyContactEmailAddressPerson1 : employeeDetailInDb.EmergencyContactEmailAddress;

                                employeeDetailInDb.EmergencyContactPerson2 = employee.EmergencyContactPerson2.Default() != "" && employeeDetailInDb.EmergencyContactPerson2 != employee.EmergencyContactPerson2.Default() ? employee.EmergencyContactPerson2 : employeeDetailInDb.EmergencyContactPerson2;

                                employeeDetailInDb.EmergencyContactEmailAddress2 = employee.EmergencyContactEmailAddressPerson2.Default() != "" && employeeDetailInDb.EmergencyContactPerson2 != employee.EmergencyContactEmailAddressPerson2.Default() ? employee.EmergencyContactEmailAddressPerson2 : employeeDetailInDb.EmergencyContactEmailAddress2;

                                employeeDetailInDb.UpdatedBy = user.ActionUserId;
                                employeeDetailInDb.UpdatedDate = DateTime.Now;
                            }
                            else
                            {

                            }
                        }

                        using (var transaction = connection.BeginTransaction())
                        {
                            if (DateTimeExtension.TryParseDate(employee.LastWorkingDate) != null && DateTimeExtension.TryParseDate(employee.LastWorkingDate).HasValue)
                            {
                                var discontinuedEmployeeInDb = await _discontinuedEmployeeRepository.GetPendingOrApprovedDiscontinuedByEmployeeIdAsync(employeeInfoInDb.EmployeeId, user);
                                if (discontinuedEmployeeInDb != null)
                                {
                                    var query = $@"Update HR_DiscontinuedEmployee
					                SET LastWorkingDate = @LastWorkingDate, UpdatedBy=@UserId,UpdatedDate=GETDATE()
					                Where EmployeeId=@EmployeeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId AND StateStatus IN ('Pending','Approved')";

                                    int updateRaw = await connection.ExecuteAsync(query,
                                        new
                                        {
                                            LastWorkingDate = DateTimeExtension.TryParseDate(employee.LastWorkingDate).Value,
                                            employeeInfoInDb.EmployeeId,
                                            user.CompanyId,
                                            user.OrganizationId,
                                            UserId = user.ActionUserId,

                                        }, transaction);

                                    if (updateRaw > 0)
                                    {
                                        transaction.Commit();
                                        isSuccessful = true;
                                    }
                                }
                                else
                                {
                                    var query = $@"INSERT INTO HR_DiscontinuedEmployee([EmployeeId],[LastWorkingDate],[CalculateFestivalBonusTaxProratedBasis],[CalculateProjectionTaxProratedBasis],[StateStatus],[CompanyId],[OrganizationId],[CreatedBy],[CreatedDate],ApprovedBy,[ApprovedDate])";
                                    query += $@"VALUES(@EmployeeId,@LastWorkingDate,0,0,'Approved',@CompanyId,@OrganizationId,@UserId,GETDATE(),@UserId,GETDATE())";

                                    int insertedRaw = await connection.ExecuteAsync(query,
                                       new
                                       {
                                           employeeInfoInDb.EmployeeId,
                                           LastWorkingDate = DateTimeExtension.TryParseDate(employee.LastWorkingDate).Value,
                                           user.CompanyId,
                                           user.OrganizationId,
                                           UserId = user.ActionUserId,

                                       }, transaction);

                                    if (insertedRaw > 0)
                                    {
                                        transaction.Commit();
                                        isSuccessful = true;
                                    }
                                }
                            }

                        }

                        using (var transaction = connection.BeginTransaction())
                        {

                        }

                    }
                };


            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeRepository", "GetEmployeeServiceDataAsync", user);
            }
            return executionStatus;
        }
    }
}
