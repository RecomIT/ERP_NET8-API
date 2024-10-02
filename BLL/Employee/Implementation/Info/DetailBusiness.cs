using BLL.Base.Interface;
using BLL.Employee.Interface.Info;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;

namespace BLL.Employee.Implementation.Info
{
    public class DetailBusiness : IDetailBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public DetailBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<EmployeePersonalInfoList> GetEmployeePersonalInfoByIdAsync(EmployeePersonalInfoQuery query, AppUser user)
        {
            var data = new EmployeePersonalInfoList();
            DBResponse response = new DBResponse();
            try
            {
                var sp_name = "sp_HR_EmployeePersonalInfo_List";
                var parameters = Utility.DappperParams(query, user, addBaseProperty: true);
                response = await _dapper.SqlQueryFirstAsync<DBResponse>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                if (response.JSONData == null)
                {
                    data = new EmployeePersonalInfoList();
                }
                else
                {
                    data = JsonReverseConverter.JsonToObject<IEnumerable<EmployeePersonalInfoList>>(response.JSONData).FirstOrDefault() ?? new EmployeePersonalInfoList();
                }

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel, "EmployeeBusiness", "GetEmployeePersonalInfoByIdAsync", user);
            }
            return data;
        }
    }
}
