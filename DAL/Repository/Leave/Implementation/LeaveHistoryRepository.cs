using System;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using System.Collections.Generic;
using DAL.Logger.Interface;
using DAL.DapperObject.Interface;
using DAL.Repository.Leave.Interface;
using Shared.Leave.Domain.History;
using Shared.Leave.ViewModel.History;

namespace DAL.Repository.Leave.Implementation
{
    public class LeaveHistoryRepository : ILeaveHistoryRepository
    {
        private readonly IDALSysLogger _sysLogger;
        private readonly IDapperData _dapper;

        public LeaveHistoryRepository(IDALSysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }
        public Task<int> DeleteByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeLeaveHistory>> GetAllAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<EmployeeLeaveHistory>> GetAllAsync(object filter, AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeLeaveHistory> GetByIdAsync(long id, AppUser user)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EmployeeLeaveHistoryInfoViewModel>> GetLeaveHistoryByIdAsync(long requestId, AppUser user)
        {
            IEnumerable<EmployeeLeaveHistoryInfoViewModel> list = new List<EmployeeLeaveHistoryInfoViewModel>();
            try
            {
                var query = $@"SELECT [Count],[Status],[LeaveDate],[DayName]=DATENAME(DW,LeaveDate),ReplacementDate  FROM HR_EmployeeLeaveHistory
                Where 1=1
                AND EmployeeLeaveRequestId=@Id
                AND CompanyId=@CompanyId
                AND OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<EmployeeLeaveHistoryInfoViewModel>(user.Database, query, new { Id = requestId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
            }
            return list;
        }
    }
}
