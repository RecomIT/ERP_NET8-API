using DAL.DapperObject.Interface;
using DAL.Logger.Interface;
using DAL.Payroll.Repository.Interface;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Payment;

namespace DAL.Payroll.Repository.Implementation
{
    public class SupplementaryPaymentAmountRepository : ISupplementaryPaymentAmountRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public SupplementaryPaymentAmountRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SupplementaryPaymentAmount>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<SupplementaryPaymentAmount>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<SupplementaryPaymentAmount> GetByIdAsync(long id, AppUser user)
        {
            SupplementaryPaymentAmount model = new SupplementaryPaymentAmount();
            try {

            }
            catch (Exception ex) {

            }
            return model;
        }
    }
}
