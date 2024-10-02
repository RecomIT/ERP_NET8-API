using BLL.Base.Interface;
using BLL.Employee.Interface.Training;
using DAL.DapperObject;
using DAL.DapperObject.Interface;
using Shared.Employee.ViewModel.Training;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;

namespace BLL.Employee.Implementation.Training
{
    public class TrainingBusiness : ITrainingBusiness
    {

        private readonly IDapperData _dapper;
        private ISysLogger _sysLogger;

        public TrainingBusiness(
            IDapperData dapper,
            ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<object>> GetAllTrainingAsync(dynamic filter, AppUser user)
        {
            IEnumerable<HR_Training_ViewModel> data = new List<HR_Training_ViewModel>();
            try
            {
                var sp_name = "sp_HR_GetAllTraining";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);
                data = await _dapper.SqlQueryListAsync<HR_Training_ViewModel>(user.Database, sp_name, parameters,
               CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel,
               "TrainingBusiness", "GetAllTrainingAsync", user);
            }
            return data;
        }



        public async Task<IEnumerable<object>> GetTrainingRequestsAsync(dynamic filter, AppUser user)
        {
            IEnumerable<HR_TrainingRequest_ViewModel> data = new List<HR_TrainingRequest_ViewModel>();
            try
            {
                var sp_name = "sp_HR_GetTrainingRequests";
                var parameters = DapperParam.AddParams(filter, user, addEmployee: false);
                data = await _dapper.SqlQueryListAsync<HR_TrainingRequest_ViewModel>(user.Database, sp_name, parameters,
               CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, Database.ControlPanel,
               "TrainingBusiness", "GetTrainingRequestsAsync", user);
            }
            return data;
        }




        public async Task<object> SaveTrainingRequestAsync(dynamic filter, AppUser user)
        {
            try
            {

                var sp_name = "sp_HR_SaveTrainingRequest";

                var parameters = DapperParam.AddParams(filter, user);
                //parameters.Add("ExecutionFlag", Data.Insert);

                var data = await _dapper.SqlQueryFirstAsync<object>(user.Database, sp_name, parameters, CommandType.StoredProcedure);

                return data;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TrainingBusiness", "SaveTrainingRequestAsync", user);

                return null;
            }
        }
    }
}
