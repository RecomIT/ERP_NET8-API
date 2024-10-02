
using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.OtherModels.DataService;
using BLL.Salary.Allowance.Interface;
using DAL.DapperObject.Interface;
using Shared.Payroll.DTO.Allowance;
using Shared.Payroll.ViewModel.Allowance;
using Shared.Payroll.Filter.Allowance;
using DAL.Context.Payroll;
using Shared.Payroll.Process.Allowance;

namespace BLL.Salary.Allowance.Implementation
{
    public class AllowanceNameBusiness : IAllowanceNameBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        private readonly PayrollDbContext _dbContext;
        public AllowanceNameBusiness(IDapperData dapper, ISysLogger sysLogger, PayrollDbContext dbContext)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
            _dbContext = dbContext;
        }
        public async Task<ExecutionStatus> SaveAllowanceNameAsync(AllowanceNameDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_AllowanceName";
                var parameters = DapperParam.AddParams(model, user, new string[] { "AllowanceHeadName", "IsTaxble", "TaxableType" });
                parameters.Add("Flag", model.AllowanceNameId > 0 ? Data.Update : Data.Insert);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, commandType: CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameBusiness", "SaveAllowanceNameAsync", user);
                executionStatus = ResponseMessage.Invalid(ResponseMessage.SomthingWentWrong);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<AllowanceNameViewModel>> GetAllowanceNamesAsync(AllowanceName_Filter filter, AppUser user)
        {
            IEnumerable<AllowanceNameViewModel> data = new List<AllowanceNameViewModel>();
            try
            {
                var sp_name = $@"Select an.*,ah.AllowanceHeadName From Payroll_AllowanceName an
			Inner Join Payroll_AllowanceHead ah on an.AllowanceHeadId = ah.AllowanceHeadId
			Where 1= 1
			AND (@AllowanceNameId IS NULL OR @AllowanceNameId = 0 OR an.AllowanceNameId = @AllowanceNameId)
			AND (@Name IS NULL OR @Name = '' OR an.[Name]=@Name)
			AND (@AllowanceFlag ='' OR @AllowanceFlag IS NULL OR Flag=@AllowanceFlag)
			AND (an.CompanyId=@CompanyId)
			AND (an.OrganizationId=@OrganizationId)
			Order By an.[Name]";
                var parameters = DapperParam.AddParams(filter, user);
                data = await _dapper.SqlQueryListAsync<AllowanceNameViewModel>(user.Database, sp_name, parameters, commandType: CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameBusiness", "GetAllowanceNamesAsync", user);
            }
            return data;
        }
        public async Task<IEnumerable<Select2Dropdown>> GetAllowanceNameExtensionAsync(string allowanceType, long allowanceHeadId, AppUser user)
        {
            IEnumerable<Select2Dropdown> data = new List<Select2Dropdown>();
            try
            {
                var sp_name = "sp_Payroll_AllowanceName";
                var parameters = new DynamicParameters();
                parameters.Add("AllowanceType", allowanceType ?? "");
                parameters.Add("AllowanceHeadId", allowanceHeadId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("Flag", Data.Extension);
                data = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameBusiness", "GetAllowanceNameExtensionAsync", user);

            }
            return data;
        }
        public async Task<ExecutionStatus> AllowanceNameValidatorAsync(AllowanceNameDTO allowanceName, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_AllowanceName";
                var parameters = DapperParam.AddParams(allowanceName, user, new string[] { "AllowanceHeadName", "IsTaxble", "TaxableType" });
                parameters.Add("Flag", Data.Validate);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameBusiness", "AllowanceNameValidatorAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<EmployeeTaxableAllowance>> GetEmployeeTaxableAllowances(long employeeId, long SalaryReviewInfoId, AppUser user)
        {
            IEnumerable<EmployeeTaxableAllowance> data = new List<EmployeeTaxableAllowance>();
            try
            {
                var sp_name = "sp_Payroll_EmployeeTaxableAllowanceHead";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("SalaryReviewInfoId", SalaryReviewInfoId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                data = await _dapper.SqlQueryListAsync<EmployeeTaxableAllowance>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameBusiness", "GetEmployeeTaxableAllowances", user);
            }
            return data;
        }
        public async Task<ExecutionStatus> SaveAllowanceNameWithConfigAsync(AllowanceNameDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            try
            {
                var sp_name = "sp_Payroll_AllowanceName_Insert_Update_Delete";
                var parameters = Utility.DappperParams(model, user);
                parameters.Add("ExecutionFlag", model.AllowanceNameId > 0 ? Data.Update : Data.Read);
                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = Utility.Invalid();
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameBusiness", "SaveAllowanceNameAsync", user);
            }
            return executionStatus;
        }
        public async Task<IEnumerable<Dropdown>> GetAllowanceNameDropdownAsync(AppUser user)
        {
            List<Dropdown> list = new List<Dropdown>();
            try
            {
                var data = await GetAllowanceNamesAsync(new AllowanceName_Filter(), user);
                if (data != null && data.AsList().Count > 0)
                {
                    foreach (var item in data)
                    {
                        Dropdown dropdown = new Dropdown()
                        {
                            Id = item.AllowanceNameId,
                            Text = item.Name + " [" + item.AllowanceHeadName + "-" + item.AllowanceType + "]",
                            Value = item.AllowanceNameId.ToString()
                        };
                        list.Add(dropdown);
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameBusiness", "GetAllowanceNameDropdownAsync", user);
            }
            return list;
        }
        public async Task<AllowanceNameViewModel> GetAllowanceNameByIdAsync(long allowanceNameId, AppUser user)
        {
            AllowanceNameViewModel allowance = new AllowanceNameViewModel();
            try
            {
                var query = $@"Select an.*,ah.AllowanceHeadName From Payroll_AllowanceName an
			Inner Join Payroll_AllowanceHead ah on an.AllowanceHeadId = ah.AllowanceHeadId
			Where 1= 1
			AND (an.AllowanceNameId = @AllowanceNameId)
			AND (an.CompanyId=@CompanyId)
			AND (an.OrganizationId=@OrganizationId)
			Order By an.[Name]";
                allowance = await _dapper.SqlQueryFirstAsync<AllowanceNameViewModel>(user.Database, query, new { AllowanceNameId = allowanceNameId, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "AllowanceNameBusiness", "GetAllowanceNameByIdAsync", user);
            }
            return allowance;
        }

        public async Task<AllowanceInfo> GetAllowanceInfos(AppUser user)
        {
            AllowanceInfo allowanceInfo = new AllowanceInfo();
            try
            {
                var basicAllowance = (await GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "BASIC"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel basicAllowanceInfo = null;
                if (basicAllowance != null)
                {
                    basicAllowanceInfo = basicAllowance;
                }

                var houseAllowance = (await GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "HR"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel houseAllowanceInfo = null;
                if (houseAllowance != null)
                {
                    houseAllowanceInfo = houseAllowance;
                }

                var conveyanceAllowance = (await GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "CONVEYANCE"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel conveyanceAllowanceInfo = null;
                if (conveyanceAllowance != null)
                {
                    conveyanceAllowanceInfo = conveyanceAllowance;
                }

                var medicalAllowance = (await GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "MEDICAL"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel medicalAllowanceInfo = null;
                if (medicalAllowance != null)
                {
                    medicalAllowanceInfo = medicalAllowance;
                }

                var LFAAllowance = (await GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "LFA"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel LFAAllowanceInfo = null;
                if (LFAAllowance != null)
                {
                    LFAAllowanceInfo = LFAAllowance;
                }

                var PFAllowance = (await GetAllowanceNamesAsync(new AllowanceName_Filter()
                {
                    AllowanceFlag = "PF"
                }, user)).FirstOrDefault();
                AllowanceNameViewModel PFAllowanceInfo = null;
                if (PFAllowance != null)
                {
                    PFAllowanceInfo = PFAllowance;
                }

                allowanceInfo.BasicAllowance = basicAllowanceInfo != null ? basicAllowanceInfo.AllowanceNameId : 0;
                allowanceInfo.BasicAllowanceName = basicAllowanceInfo != null ? basicAllowanceInfo.Name : "";
                allowanceInfo.HouseRentAllowance = houseAllowanceInfo != null ? houseAllowanceInfo.AllowanceNameId : 0;
                allowanceInfo.HouseRentAllowanceName = houseAllowanceInfo != null ? houseAllowanceInfo.Name : "";
                allowanceInfo.ConveyanceAllowance = conveyanceAllowanceInfo != null ? conveyanceAllowanceInfo.AllowanceNameId : 0;
                allowanceInfo.ConveyanceAllowanceName = conveyanceAllowanceInfo != null ? conveyanceAllowanceInfo.Name : "";
                allowanceInfo.MedicalAllowance = medicalAllowanceInfo != null ? medicalAllowanceInfo.AllowanceNameId : 0;
                allowanceInfo.MedicalAllowanceName = medicalAllowanceInfo != null ? medicalAllowanceInfo.Name : "";
                allowanceInfo.LFAAllowance = LFAAllowanceInfo != null ? LFAAllowanceInfo.AllowanceNameId : 0;
                allowanceInfo.LFAAllowanceName = LFAAllowanceInfo != null ? LFAAllowanceInfo.Name : "";
                allowanceInfo.PFAllowance = PFAllowanceInfo != null ? PFAllowanceInfo.AllowanceNameId : 0;
                allowanceInfo.PFAllowanceName = PFAllowanceInfo != null ? PFAllowanceInfo.Name : "";
            }
            catch (Exception)
            {
            }
            return allowanceInfo;
        }
    }
}
