using Shared.OtherModels.User;
using Shared.OtherModels.DataService;
using BLL.Base.Interface;
using BLL.Employee.Interface.Miscellaneous;
using DAL.DapperObject.Interface;

namespace BLL.Employee.Implementation.Miscellaneous
{
    public class DataLabelBusiness : IDataLabelBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;

        public DataLabelBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<Dropdown>> GetDataByLabelAsync(string label, AppUser user)
        {
            IEnumerable<Dropdown> list = new List<Dropdown>();
            try
            {
                var sqlQuery = $@"SELECT [Id]=Id,[Text]=DisplayName,[Value]=Alias,[Type]=[Label], [Employee Name]=''
                FROM HR_DataLabel
                Where CompanyId=@CompanyId AND OrganizationId=@OrganizationId
                AND [Label]=@Label
                Order By Serial";

                list = await _dapper.SqlQueryListAsync<Dropdown>(user.Database, sqlQuery, new
                {
                    user.CompanyId,
                    user.OrganizationId,
                    Label = label
                });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "DataLabelBusiness", "GetDataByLabel", user);
            }
            return list;
        }
    }
}
