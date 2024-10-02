using DAL.DapperObject.Interface;
using DAL.Logger.Interface;
using DAL.Repository.Leave.Interface;
using Shared.Leave.Domain.Setup;
using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repository.Leave.Implementation
{
    public class LeaveTypeRepository : ILeaveTypeRepository
    {
        private readonly IDapperData _dapper;
        private readonly IDALSysLogger _sysLogger;
        public LeaveTypeRepository(IDapperData dapper, IDALSysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LeaveType>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LeaveType>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<LeaveType> GetByIdAsync(long id, AppUser user)
        {
            LeaveType leaveType = null;
            try
            {
                var query = $@"SELECT * FROM HR_LeaveTypes Where Id=@Id AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                leaveType = await _dapper.SqlQueryFirstAsync<LeaveType>(user.Database, query, new { Id = id, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LeaveTypeBusiness", "GetLeaveTypeById", user);
            }
            return leaveType;
        }
    }
}
