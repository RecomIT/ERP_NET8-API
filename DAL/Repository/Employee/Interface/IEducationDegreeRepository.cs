using DAL.Repository.Base.Interface;
using Shared.Employee.Domain.Education;
using Shared.OtherModels.DataService;
using Shared.OtherModels.User;

namespace DAL.Repository.Employee.Interface
{
    public interface IEducationDegreeRepository : IDapperBaseRepository<EducatioalDegree>
    {
        Task<IEnumerable<Dropdown>> GetEducationDegreeByEductionLevelAsync(long levelOfEductionId, AppUser user);
    }
}
