using BLL.Base.Interface;
using BLL.Employee.Interface.Info;
using DAL.DapperObject.Interface;
using Dapper;
using Shared.Employee.DTO.Info;
using Shared.Employee.Filter.Info;
using Shared.Employee.ViewModel.Info;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Services;
using System.Data;

namespace BLL.Employee.Implementation.Info
{
    public class DocumentBusiness : IDocumentBusiness
    {
        private readonly IDapperData _dapper;
        private readonly ISysLogger _sysLogger;
        public DocumentBusiness(IDapperData dapper, ISysLogger sysLogger)
        {
            _dapper = dapper;
            _sysLogger = sysLogger;
        }

        public async Task<IEnumerable<EmployeeDocumentViewModel>> GetEmployeeDocumentsAsync(EmployeeDocument_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeDocumentViewModel> list = new List<EmployeeDocumentViewModel>();
            try
            {
                var sp_name = "sp_HR_EmployeeDocument_List";
                var parameters = Utility.DappperParams(filter, user);
                list = await _dapper.SqlQueryListAsync<EmployeeDocumentViewModel>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeDocumentBusiness", "GetEmployeeDocumentsAsync", user);
            }
            return list;
        }
        public async Task<ExecutionStatus> SaveEmployeeDocumentAsync(EmployeeDocumentDTO model, AppUser user)
        {
            ExecutionStatus executionStatus = new ExecutionStatus();
            string file = null;
            string filePath = null;
            string fileName = null;
            string extenstion = null;
            string fileSize = null;
            string actualFileName = null;
            try
            {
                if (model.DocumentId > 0 && model.File == null)
                {
                    // Persist Existing 
                }
                else if (model.DocumentId > 0 && model.File != null)
                {
                    if (File.Exists(string.Format(@"{0}/{1}", Utility.PhysicalDriver, model.FilePath)))
                    {
                        Utility.DeleteFile(string.Format(@"{0}/{1}", Utility.PhysicalDriver, model.FilePath));
                    }
                    file = await Utility.SaveFileAsync(model.File, string.Format(@"{0}", user.OrgCode));
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(model.File.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = model.File.FileName;
                }
                else
                {
                    file = await Utility.SaveFileAsync(model.File, string.Format(@"{0}", user.OrgCode));
                    filePath = file.Substring(0, file.LastIndexOf("/"));
                    fileName = file.Substring(file.LastIndexOf("/") + 1);
                    extenstion = fileName.Substring(fileName.LastIndexOf(".") + 1);
                    fileSize = Math.Round(Convert.ToDecimal(model.File.Length / 1024), 0, MidpointRounding.AwayFromZero).ToString() + "KB";
                    actualFileName = model.File.FileName;
                }


                var sp_name = "sp_HR_EmployeeDocument_Insert_Update";
                var parameters = new DynamicParameters();
                parameters.Add("DocumentId", model.DocumentId);
                parameters.Add("EmployeeId", model.EmployeeId);
                parameters.Add("DocumentName", model.DocumentName);
                parameters.Add("DocumentNumber", model.DocumentNumber);
                parameters.Add("FileName", fileName ?? "");
                parameters.Add("ActualFileName", actualFileName ?? "");
                parameters.Add("FileSize", fileSize ?? "");
                parameters.Add("FilePath", filePath ?? "");
                parameters.Add("FileFormat", extenstion ?? "");
                parameters.Add("UserId", user.ActionUserId);
                parameters.Add("BranchId", user.BranchId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                parameters.Add("@ExecutionFlag", model.DocumentId > 0 ? Data.Update : Data.Insert);

                executionStatus = await _dapper.SqlQueryFirstAsync<ExecutionStatus>(user.Database, sp_name, parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                executionStatus = ResponseMessage.Invalid("File has been failed to save");
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeDocumentBusiness", "SaveEmployeeDocumentAsync", user);
            }
            return executionStatus;
        }
    }
}
