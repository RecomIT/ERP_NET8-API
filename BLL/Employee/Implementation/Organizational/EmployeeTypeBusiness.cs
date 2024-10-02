using Dapper;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using DAL.DapperObject.Interface;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using Shared.Employee.DTO.Organizational;
using BLL.Employee.Interface.Organizational;
using Shared.Employee.Domain.Organizational;
using Shared.Employee.Filter.Organizational;

namespace BLL.Employee.Implementation.Organizational
{
    public class EmployeeTypeBusiness : IEmployeeTypeBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        public EmployeeTypeBusiness(ISysLogger sysLogger, IDapperData dapper)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
        }

        public async Task<IEnumerable<Dropdown>> GetEmployeeTypeDropdownAsync(AppUser user)
        {
            List<Dropdown> list = new List<Dropdown>();
            try
            {
                var data = await GetEmployeeTypesAsync(new EmployeeType_Filter(), user);
                foreach (var item in data)
                {
                    list.Add(new Dropdown()
                    {
                        Id = item.EmployeeTypeId,
                        Value = item.EmployeeTypeId.ToString(),
                        Text = item.EmployeeTypeName
                    });
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeTypeBusiness", "GetEmployeeTypeDropdownAsync", user);
            }
            return list;
        }
        public async Task<IEnumerable<EmployeeType>> GetEmployeeTypesAsync(EmployeeType_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeType> list = new List<EmployeeType>();
            try
            {
                var query = $@"Select * From HR_EmployeeType 
                Where 
                1=1
                AND (@Id IS NULL OR @Id = 0 OR EmployeeTypeId=@Id)
                AND (@EmployeeTypeName IS NULL OR @EmployeeTypeName = '' OR EmployeeTypeName=@EmployeeTypeName)
                AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                list = await _dapper.SqlQueryListAsync<EmployeeType>(user.Database, query.Trim(), new { filter.Id, filter.EmployeeTypeName, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeTypeBusiness", "GetEmployeeTypesAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveEmployeeTypeAsync(EmployeeTypeDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                if (model.EmployeeTypeId > 0)
                {
                    // Update
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var query = $@"Update HR_EmployeeType SET EmployeeTypeName = @EmployeeTypeName, Remarks=@Remarks, UpdatedBy=@UserId, UpdatedDate=GETDATE()";
                                    query += $@" Where EmployeeTypeId=@EmployeeTypeId AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";
                                    var parameters = new
                                    {
                                        model.EmployeeTypeId,
                                        model.EmployeeTypeName,
                                        model.Remarks,
                                        UserId = user.ActionUserId,
                                        user.CompanyId,
                                        user.OrganizationId
                                    };
                                    var rawAffected = await connection.ExecuteAsync(query, parameters, transaction);
                                    if (rawAffected > 0)
                                    {
                                        transaction.Commit();
                                        executionStatus.Msg = "Data has been updated successfully";
                                        executionStatus.Status = true;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    executionStatus.Msg = "Someting went wrong in update process";
                                    executionStatus.Status = false;
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeTypeBusiness", "SaveEmployeeTypeAsync", user);
                                }
                                finally { connection.Close(); }
                            }
                        }
                    }
                }
                else
                {
                    // Insert
                    using (var connection = await _dapper.SqlConnectionForTransactionalAsync(user.Database))
                    {
                        connection.Open();
                        {
                            using (var transaction = connection.BeginTransaction())
                            {
                                try
                                {
                                    var query = $@"INSERT INTO [dbo].[HR_EmployeeType] ([EmployeeTypeName],[Remarks],[CreatedBy],[CreatedDate],[OrganizationId],[CompanyId]) OUTPUT INSERTED.EmployeeTypeId";
                                    query += $@" VALUES(@EmployeeTypeName,@Remarks,@UserId,GETDATE(),@OrganizationId,@CompanyId)";
                                    var parameters = new
                                    {
                                        model.EmployeeTypeName,
                                        model.Remarks,
                                        UserId = user.ActionUserId,
                                        user.CompanyId,
                                        user.OrganizationId
                                    };
                                    var id = await connection.QueryFirstOrDefaultAsync<int>(query, parameters, transaction);
                                    if (id > 0)
                                    {
                                        transaction.Commit();
                                        executionStatus.Msg = "Data has been save successfully";
                                        executionStatus.Status = true;
                                        executionStatus.ItemId = id;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    executionStatus.Msg = "Someting went wrong in save process";
                                    executionStatus.Status = false;
                                    await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeTypeBusiness", "SaveEmployeeTypeAsync", user);
                                }
                                finally { connection.Close(); }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeTypeBusiness", "SaveEmployeeTypeAsync", user);
            }
            return executionStatus;
        }
    }
}
