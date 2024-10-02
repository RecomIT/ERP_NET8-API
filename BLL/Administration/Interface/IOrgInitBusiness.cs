using Shared.Control_Panel.Domain;
using Shared.Control_Panel.ViewModels;
using Shared.OtherModels.DataService;

namespace BLL.Administration.Interface
{
    public interface IOrgInitBusiness
    {
        // Organization
        Task<IEnumerable<OrganizationViewModel>> GetOrganizationsAsync(string OrgName, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveOrganizationAsync(OrganizationViewModel organization, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteOrganizationAsync(long OrganizationId, long OrgId, long ComId, long BranchId, string UserId);
        Task<IEnumerable<Select2Dropdown>> GetOrganizationExtensionAsync();

        // Company
        Task<IEnumerable<CompanyViewModel>> GetCompanyAsync(string CompanyName, long ComOrgId, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveCompanyAsync(CompanyViewModel company, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteCompanyAsync(long CompanyId, long OrgId, long ComId, long BranchId, string UserId);
        Task<IEnumerable<Select2Dropdown>> GetCompanyExtensionAsync(long orgId);

        // Division
        Task<IEnumerable<DivisionViewModel>> GetDivisionsAsync(string DivisionName, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveDivisionAsync(Division division, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteDivisionAsync(long DivisionId, long OrgId, long ComId, long BranchId, string UserId);

        // District
        Task<IEnumerable<DistrictViewModel>> GetDistrictsAsync(string DistrictName, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveDistrictAsync(District district, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteDistrictAsync(long DistrictId, long OrgId, long ComId, long BranchId, string UserId);

        // Zone
        Task<IEnumerable<ZoneViewModel>> GetZonesAsync(string ZoneName, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveZoneAsync(Zone zone, long OrgId, long ComId, long BranchId, string UserId);
        Task<bool> DeleteZoneAsync(long ZoneId, long OrgId, long ComId, long BranchId, string UserId);

        // Branch
        Task<IEnumerable<BranchViewModel>> GetBranchesAsync(string BranchName, bool? IsActive, long OrgId, long ComId, long BranchId);
        Task<bool> SaveBranchAsync(Branch branch, string UserId);
        Task<bool> DeleteBranchAsync(long Id, long OrgId, long ComId, long BranchId, string UserId);
        Task<IEnumerable<Dropdown>> BranchExtension(string flag, long ComId, long OrgId);
        Task<Branch> GetBranchById(long id, long companyId, long organizationId);
    }
}
