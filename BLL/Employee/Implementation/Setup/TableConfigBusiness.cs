using Shared.OtherModels.User;
using BLL.Employee.Interface.Setup;
using DAL.Context.Employee;
using BLL.Base.Interface;
using Shared.Employee.ViewModel.Setup;
using Microsoft.EntityFrameworkCore;

namespace BLL.Employee.Implementation.Setup
{
    public class TableConfigBusiness : ITableConfigBusiness
    {
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;
        private readonly ISysLogger _sysLogger;
        public TableConfigBusiness(EmployeeModuleDbContext employeeModuleDbContext, ISysLogger sysLogger)
        {
            _employeeModuleDbContext = employeeModuleDbContext;
            _sysLogger = sysLogger;
        }
        public async Task<IEnumerable<TableConfigViewModel>> GetColumnsAsync(string table, string purpose, AppUser user)
        {
            IEnumerable<TableConfigViewModel> list = new List<TableConfigViewModel>();
            try
            {
                list = await _employeeModuleDbContext.HR_TableConfig.Where(i => i.IsVisible == true && i.CompanyId == user.CompanyId && i.OrganizationId == user.OrganizationId).OrderBy(i => i.Serial).Select(i => new TableConfigViewModel
                {
                    Id = i.Id,
                    Column = i.Column,
                    IsVisible = i.IsVisible,
                    IsMandatory = i.IsMandatory,
                    IsDisabled = i.IsDisabled,
                    DataType = i.DataType,
                    DefaultValue = i.DefaultValue,
                    MaxLength = i.MaxLength,
                    Label = i.Label,
                    HelpText = i.HelpText,
                    ParentId = i.ParentId,
                    Serial = i.Serial,
                    Parent = "",
                    IsConstant = i.IsConstant,
                    IsUnique = i.IsUnique,
                    IsNewEntry = i.IsNewEntry,
                    Group = i.Group
                }).ToListAsync();

                foreach (var item in list)
                {
                    if(item.ParentId > 0)
                    {
                        var parentItem = list.FirstOrDefault(i=> i.Id == item.ParentId);
                        if(parentItem != null)
                        {
                            item.Parent = "Depands on "+ parentItem.Column;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeInfoBusiness", "GetEmployeeServiceDataAsync", user);
            }
            return list;
        }
    }
}
