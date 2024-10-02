namespace Shared.Overtime.Queries
{
    public static class OvertimeQueries
    {
        public static string EmployeeDetails()
        {
            return
                $@"SELECT e.EmployeeId, e.EmployeeCode, e.FullName AS 'Name', ISNULL(desig.DesignationName,'-') AS 'Designation', ISNULL(dpt.DepartmentName,'-') AS 'Department', 
                ISNULL(dv.DivisionName ,'-') AS 'Division', ISNULL(br.BranchName  ,'-')  AS 'Branch'
                FROM  dbo.HR_EmployeeInformation e
                LEFT JOIN dbo.HR_Designations desig
                ON e.DepartmentId = desig.DesignationId
                LEFT JOIN dbo.HR_Departments dpt
                ON e.DepartmentId = dpt.DepartmentId
                LEFT JOIN dbo.HR_Divisions dv
                ON e.DivisionId = dv.DivisionId
                LEFT JOIN [ControlPanel].dbo.tblBranches br 
                ON  br.CompanyId = e.CompanyId AND e.BranchId = br.BranchId ";

        }

        public static string EmployeeDetailsForOvertimeApprover()
        {
            return
                $@"SELECT e.EmployeeId, e.EmployeeCode, e.FullName AS 'Name', ISNULL(desig.DesignationName,'-') AS 'Designation', ISNULL(dpt.DepartmentName,'-') AS 'Department', 
                ISNULL(dv.DivisionName ,'-') AS 'Division', ISNULL(br.BranchName  ,'-')  AS 'Branch',  ISNULL(ot.OvertimeApproverId,0) AS 'OvertimeApproverId',
                ISNULL(ot.IsActive ,0) AS 'IsActive',ISNULL(ot.ProxyEnabled ,0) AS 'ProxyEnabled',ISNULL(ot.ProxyApproverId ,0) AS 'ProxyApproverId', ot.CreatedDate, ot.UpdatedDate
                FROM  dbo.HR_EmployeeInformation e
                LEFT JOIN dbo.HR_Designations desig
                ON e.DepartmentId = desig.DesignationId
                LEFT JOIN dbo.HR_Departments dpt
                ON e.DepartmentId = dpt.DepartmentId
                LEFT JOIN dbo.HR_Divisions dv
                ON e.DivisionId = dv.DivisionId
                LEFT JOIN [ControlPanel].dbo.tblBranches br 
                ON  br.CompanyId = e.CompanyId AND e.BranchId = br.BranchId
                LEFT JOIN dbo.Payroll_OvertimeApprover ot
                ON ot.EmployeeId = e.EmployeeId ";

        }

        public static string EmployeeDetailsForOvertimeTeamApprovalMapping(string columns = "")
        {
            return
                $@"SELECT e.EmployeeId, e.EmployeeCode, e.FullName AS 'Name', ISNULL(desig.DesignationName,'-') AS 'Designation', ISNULL(dpt.DepartmentName,'-') AS 'Department', 
                ISNULL(dv.DivisionName ,'-') AS 'Division', ISNULL(br.BranchName  ,'-')  AS 'Branch' {columns}
                FROM  dbo.HR_EmployeeInformation e
                LEFT JOIN dbo.HR_Designations desig
                ON e.DepartmentId = desig.DesignationId
                LEFT JOIN dbo.HR_Departments dpt
                ON e.DepartmentId = dpt.DepartmentId
                LEFT JOIN dbo.HR_Divisions dv
                ON e.DivisionId = dv.DivisionId
                LEFT JOIN [ControlPanel].dbo.tblBranches br 
                ON  br.CompanyId = e.CompanyId AND e.BranchId = br.BranchId ";
        }

    }
}
