using BLL.Base.Interface;
using BLL.Employee.Interface.Organizational;
using DAL.DapperObject.Interface;
using DAL.Repository.Employee.Interface;
using Shared.OtherModels.DataService;
using Shared.OtherModels.User;

namespace BLL.Employee.Implementation.Organizational
{
    public class JobCategoryBusiness : IJobCategoryBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly IJobCategoryRepository _jobCategoryRepository;
        public JobCategoryBusiness(IDapperData dapper, ISysLogger sysLogger, IJobCategoryRepository jobCategoryRepository)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _jobCategoryRepository = jobCategoryRepository;
        }
        public async Task<IEnumerable<Dropdown>> GetJobCategoryDropdownAsync(AppUser user)
        {
            List<Dropdown> list = new List<Dropdown>();
            try
            {
                var data = await _jobCategoryRepository.GetAllAsync(user);
                if (data.Any())
                {
                    foreach (var item in data)
                    {
                        Dropdown dropdown = new Dropdown();
                        dropdown.Value = item.JobCategoryId.ToString();
                        dropdown.Id = item.JobCategoryId;
                        dropdown.Text = item.JobCategoryName;
                        list.Add(dropdown);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "JobCategoryBusiness", "GetJobCategoryDropdownAsync", user);
            }
            return list;
        }
    }
}
