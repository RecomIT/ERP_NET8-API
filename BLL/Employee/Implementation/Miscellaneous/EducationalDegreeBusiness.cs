using BLL.Base.Interface;
using BLL.Employee.Interface.Miscellaneous;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using Shared.OtherModels.DataService;
using Shared.OtherModels.User;

namespace BLL.Employee.Implementation.Miscellaneous
{
    public class EducationalDegreeBusiness : IEducationalDegreeBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IEducationDegreeRepository _educationDegreeRepository;

        public EducationalDegreeBusiness(ISysLogger sysLogger, IDapperData dapper, IEducationDegreeRepository educationDegreeRepository)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _educationDegreeRepository = educationDegreeRepository;
        }
        public Task<IEnumerable<Dropdown>> GetEducationDegreeDropdownAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Dropdown>> GetEducationDegreeDropdownAsync(long leaveOfEductionId, AppUser user)
        {
            IEnumerable<Dropdown> list = new List<Dropdown>();
            try
            {
                list = await _educationDegreeRepository.GetEducationDegreeByEductionLevelAsync(leaveOfEductionId, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EducationalDegreeBusiness", "GetEducationDegreeDropdownAsync", user);
            }
            return list;
        }
    }
}
