using BLL.Base.Interface;
using BLL.Separation.Interface;
using DAL.DapperObject.Interface;
using Shared.OtherModels.User;
using Shared.Separation.ViewModels.NoticePeriod;
using Shared.Services;
using System.Data;

namespace BLL.Separation.Implementation
{
    public class ResignationCategoryBusiness : IResignationCategoryBusiness
    {

        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;


        public ResignationCategoryBusiness(
            IDapperData dapper,
            ISysLogger sysLogger
            )
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }



        public async Task<object> GetResignationCategoryAsync(AppUser user)
        {
            try
            {
                var sp_name = "sp_HR_GetResignationCategory";
                var parameters = DapperParam.AddParams(user, addEmployee: false);
                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                return data;

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationCategoryBusiness", "GetResignationCategoryAsync", user);

                return null;
            }
        }


        public async Task<object> GetResignationSubCategoryAsync(dynamic filter, AppUser user)
        {
            try
            {
                var sp_name = "sp_HR_GetResignationSubCategory";

                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);
                var data = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                return data;
            }

            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationCategoryBusiness", "GetResignationSubCategoryAsync", user);

                return null;
            }
        }



        public async Task<object> GetResignationNoticePeriodAsync(dynamic filter, AppUser user)
        {
            try
            {
                var sp_name = "sp_HR_GetResignationNoticePeriod";

                var parameters = DapperParam.AddParams(filter, user);
                var data = await _dapper.SqlQueryFirstAsync<NoticePeriodViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
                return data;

            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "ResignationCategoryBusiness", "GetResignationNoticePeriodAsync", user);

                return null;
            }
        }


    }
}
