
using System.Data;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using BLL.Administration.Interface;
using BLL.Employee.Interface.Report;
using Shared.Employee.ViewModel.Report;

namespace BLL.Employee.Implementation.Report
{
    public class HRLetterBusiness : IHRLetterBusiness
    {
        private readonly ISysLogger logger;
        private readonly IDapperData dapper;
        private readonly IBranchInfoBusiness branchInfoBusiness;
        public HRLetterBusiness(ISysLogger _logger, IDapperData _dapper, IBranchInfoBusiness _branchInfoBusiness)
        {
            logger = _logger;
            dapper = _dapper;
            branchInfoBusiness = _branchInfoBusiness;
        }
        public async Task<HRLetterEmployeeInfoViewModel> GetEmployeeInfoAsync(long id, AppUser user)
        {
            HRLetterEmployeeInfoViewModel info = null;
            try
            {
                string query = $@"SELECT EMP.EmployeeId,EMP.BranchId,EmployeeName=EMP.FullName,EMP.EmployeeCode,EMP.DesignationId,Designation=DEG.DesignationName,EMP.DepartmentId,
                Department=DEP.DepartmentName,EMP.SectionId,SEC.SectionName,SUB.SubSectionId,SUB.SubSectionName,EMP.OfficeEmail,EMP.OfficeMobile,
                DateOfJoining=CONVERT(VARCHAR(20),EMP.DateOfJoining,106),[IsActive]=(
	            CASE 
                WHEN EMP.TerminationDate IS NOT NULL AND EMP.TerminationStatus ='Approved' AND CAST(EMP.TerminationDate AS DATE) < CAST(GETDATE() AS DATE) THEN 'False'
                WHEN EMP.IsActive IS NULL THEN 'False' 
	            ELSE EMP.IsActive  END),
                EMP.Jobtype,
                EMP.TerminationDate,EMP.TerminationStatus,DTL.LegalName,DTL.FatherName,DTL.MotherName,DTL.SpouseName,
                DateOfBirth=(CASE WHEN DTL.DateOfBirth IS NULL THEN '' ELSE CONVERT(VARCHAR(20),DTL.DateOfBirth,106) END),DTL.Gender,
                NID=ISNULL((SELECT DocumentNumber FROM HR_EmployeeDocument Where EmployeeId=EMP.EmployeeId AND DocumentName='NID'),''),
                TIN=ISNULL((SELECT DocumentNumber FROM HR_EmployeeDocument Where EmployeeId=EMP.EmployeeId AND DocumentName='TIN'),''),
                BirthCertificate=ISNULL((SELECT DocumentNumber FROM HR_EmployeeDocument Where EmployeeId=EMP.EmployeeId AND DocumentName='Birth Certificate'),''),
                ProfilePicPath=(CASE WHEN DTL.PhotoPath IS NULL THEN '' ELSE DTL.PhotoPath+'/'+DTL.Photo END),
                HeadOfDepartmentName=(SELECT FullName FROM HR_EmployeeInformation Where EmployeeId=EHC.HeadOfDepartmentId),
                HeadOfDepartmentDesignation=
                (
	                SELECT HDEG.DesignationName FROM HR_EmployeeInformation HEMP
	                INNER JOIN HR_Designations HDEG ON HEMP.DesignationId = HDEG.DesignationId
	                Where EmployeeId=EHC.HeadOfDepartmentId
                ),
                SupervisorName=(SELECT FullName FROM HR_EmployeeInformation Where EmployeeId=EHC.SupervisorId),
                SupervisorDesignation=
                (
	                SELECT SDEG.DesignationName FROM HR_EmployeeInformation SEMP
	                INNER JOIN HR_Designations SDEG ON SEMP.DesignationId = SDEG.DesignationId
	                Where EmployeeId=EHC.SupervisorId
                )
                FROM HR_EmployeeInformation EMP
                LEFT JOIN HR_EmployeeDetail DTL ON EMP.EmployeeId= DTL.EmployeeId
                LEFT JOIN (SELECT * FROM HR_EmployeeHierarchy Where IsActive=1) EHC ON EMP.EmployeeId=EHC.EmployeeId
                LEFT JOIN HR_Designations DEG ON EMP.DesignationId = DEG.DesignationId
                LEFT JOIN HR_Departments DEP ON EMP.DepartmentId = DEP.DepartmentId
                LEFT JOIN HR_Sections SEC ON EMP.SectionId = SEC.SectionId
                LEFT JOIN HR_SubSections SUB ON EMP.SubSectionId = SUB.SubSectionId
                WHERE 1=1 AND EMP.EmployeeId=@EmployeeId AND EMP.CompanyId=@CompanyId AND EMP.OrganizationId=@OrganizationId";
                info = await dapper.SqlQueryFirstAsync<HRLetterEmployeeInfoViewModel>(user.Database, query, new
                {
                    EmployeeId = id,
                    user.CompanyId,
                    user.OrganizationId
                });
                if (info != null)
                {
                    var branch = await branchInfoBusiness.GetBranchByIdAsync(info.BranchId, user);
                    if (branch != null)
                    {
                        info.BranchName = branch.BranchName;
                    }
                }
            }
            catch (Exception ex)
            {
                await logger.SaveHRMSException(ex, user.Database, "HRLetterBusiness", "GetEmployeeInfoAsync", user);
            }
            return info;
        }
        public async Task<DataTable> SalaryBreakdownAsync(long id, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = $"sp_HR_SalaryCertificate";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", id.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await logger.SaveHRMSException(ex, user.Database, "HRLetterBusiness", "GetEmployeeInfoAsync", user);
            }
            return dataTable;
        }
        public async Task<DataTable> SalaryHeadsAysnc(long id, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = $"sp_HR_SalaryCertificate";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("EmployeeId", id.ToString());
                parameters.Add("CompanyId", user.CompanyId.ToString());
                parameters.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await dapper.SqlDataTable(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await logger.SaveHRMSException(ex, user.Database, "HRLetterBusiness", "SalaryHeadsAysnc", user);
            }
            return dataTable;
        }
    }
}
