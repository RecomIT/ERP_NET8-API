using System;
using System.Threading.Tasks;
using Shared.OtherModels.User;
using Shared.Models.Dashboard.CommonDashboard.ViewModels.EmployeeInfo;
using System.Data;
using Shared.Services;
using Shared.Models.Dashboard.CommonDashboard.CompanyEvents;
using DAL.DapperObject.Interface;
using BLL.Dashboard.CommonDashboard.Interface;
using BLL.Dashboard.DataService.Interface;

namespace BLL.Dashboard.CommonDashboard.Implementation
{
    public class CommonDashboardBusiness : ICommonDashboardBusiness
    {
        private readonly IDataGetService _dataGetService;
        private readonly IDapperData _dapper;
        public CommonDashboardBusiness(
            IDataGetService dataGetService,
            IDapperData dapper
            )
        {
            _dataGetService = dataGetService;
            _dapper = dapper;
        }

        public async Task<object> GetCompanyHolidayAndEventsAsync(AppUser user)
        {

            try
            {

                var sp_name = "sp_GetCompanyHolidayAndEvents";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<CompnayHolidayOrEventViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }




        // ----------------------- >>> GetEmployeeBloodGroupsAsync
        public async Task<object> GetEmployeeBloodGroupsAsync(AppUser user)
        {

            try
            {
                var sp_name = "sp_GetBloodGroups";
                var parameters = DapperParam.AddParams(user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }




        // ----------------------- >>> GetEmployeeContactAsync
        public async Task<object> GetEmployeeContactAsync(dynamic filter, AppUser user)
        {

            try
            {
                var sp_name = "sp_GetEmployeeContactInfo";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<EmployeeContactViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }

        public async Task<object> SaveCompanyEventsAsync(dynamic filter, AppUser user)
        {
            try
            {
                var sp_name = "sp_SaveCompanyEvents";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);

                var list = await _dapper.SqlQueryListAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return list;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
    }
}
