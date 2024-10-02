using System.Security.Claims;

namespace API.Infrastructure.CustomAuth
{
    public class CustomClaimsPrincipal : ClaimsPrincipal
    {
        public CustomClaimsPrincipal(ClaimsIdentity identity) : base(identity)
        {
        }

        // Custom properties
        public string UserId { get; set; }
        public string Username { get; set; }
        public string UserSession { get; set; }
        public long EmployeeId { get; set; }
        public long BranchId { get; set; }
        public long DivisionId { get; set; }
        public long CompanyId { get; set; }
        public long OrganizationId { get; set; }
        public long DepartmentId { get; set; }
        public long DesignationId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string AccessToken { get; set; }
        public string Database { get; set; }
        public string OrgCode { get; set; }

        // Additional custom methods
        public bool HasUserId => !string.IsNullOrEmpty(UserId) && !string.IsNullOrWhiteSpace(UserId);
        public bool HasCompanyId => CompanyId > 0;
        public bool HasOrganizationId => OrganizationId > 0;
        public bool HasEmployeeId => EmployeeId > 0;
        public string ActionUserId => EmployeeId > 0 ? EmployeeId.ToString() : UserId;
        public bool HasBoth => CompanyId > 0 && OrganizationId > 0;
    }
}
