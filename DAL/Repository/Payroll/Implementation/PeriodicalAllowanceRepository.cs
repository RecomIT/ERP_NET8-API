using AutoMapper;
using DAL.Logger.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.Payroll.Domain.Variable;
using DAL.Payroll.Repository.Interface;

namespace DAL.Payroll.Repository.Implementation
{
    public class PeriodicalAllowanceRepository : IPeriodicalAllowanceRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly IMapper _mapper;
        public PeriodicalAllowanceRepository(IDALSysLogger sysLogger, IDapperData dapper, IMapper mapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _mapper = mapper;
        }

        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<PeriodicallyVariableAllowanceInfo>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<IEnumerable<PeriodicallyVariableAllowanceInfo>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }
        public Task<PeriodicallyVariableAllowanceInfo> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }
    }
}
