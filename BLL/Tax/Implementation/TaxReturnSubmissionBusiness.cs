using Dapper;
using System.Data;
using Shared.Services;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Tax;
using Shared.Payroll.ViewModel.Tax;
using Shared.Payroll.Filter.Tax;

namespace BLL.Tax.Implementation
{
    public class TaxReturnSubmissionBusiness : ITaxReturnSubmissionBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public TaxReturnSubmissionBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<ExecutionStatus> SaveEmployeeTaxReturnSubmissionAsync(EmployeeTaxReturnSubmissionDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxReturnSubmission_Insert_Update_Delete";
                var param = DapperParam.AddParams(model, user, new string[] { "File", "FileName", "FilePath" });

                if (model.File != null)
                {
                    Utility.DeleteFile(string.Format(@"{0}/{1}", Utility.PhysicalDriver, model.FilePath ?? ""));
                    var fileDetail = FileProcessor.Process(model.TaxSubmissionId, model.File, model.FilePath + "/" + model.FileName, user);
                    param.Add("FilePath", fileDetail.FilePath);
                    param.Add("FileName", fileDetail.FileName);
                }
                param.Add("ExecutionFlag", model.TaxSubmissionId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReturnSubmissionBusiness", "SaveEmployeeTaxReturnSubmissionAsync", user);
            }
            return executionStatus;
        }

        public async Task<IEnumerable<EmployeeTaxReturnSubmissionViewModel>> GetEmployeeTaxReturnSubmissionAsync(EmployeeTaxReturnSubmission_Filter model, AppUser user)
        {
            IEnumerable<EmployeeTaxReturnSubmissionViewModel> list = new List<EmployeeTaxReturnSubmissionViewModel>();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxReturnSubmission_List";
                var param = DapperParam.AddParams(model, user);
                list = await _dapper.SqlQueryListAsync<EmployeeTaxReturnSubmissionViewModel>(user.Database, sp_name, param, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxReturnSubmissionBusiness", "GetEmployeeTaxReturnSubmissionAsync", user);
            }
            return list;
        }

        public async Task<EmployeeTaxReturnSubmissionViewModel> GetEmployeeTaxReturnSubmissionByIdAsync(long id, AppUser user)
        {
            EmployeeTaxReturnSubmissionViewModel data = new EmployeeTaxReturnSubmissionViewModel();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxReturnSubmission_List";
                var parameters = new DynamicParameters();
                parameters.Add("TaxSubmissionId", id.ToString());
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapper.SqlQueryFirstAsync<EmployeeTaxReturnSubmissionViewModel>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "TaxZoneBusiness", "GetEmployeeTaxZoneByIdAsync", user);
            }
            return data;
        }

    }
}

