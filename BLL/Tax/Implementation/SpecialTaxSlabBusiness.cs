using AutoMapper;
using BLL.Tax.Interface;
using BLL.Base.Interface;
using DAL.Context.Payroll;
using Shared.OtherModels.User;
using Shared.Payroll.Domain.Tax;
using Microsoft.EntityFrameworkCore;
using Shared.Payroll.ViewModel.Tax;
using DAL.Context.Payroll.Migrations;

namespace BLL.Tax.Implementation
{
    public class SpecialTaxSlabBusiness : ISpecialTaxSlabBusiness
    {
        private readonly IMapper _mapper;
        private readonly PayrollDbContext _payrollDbContext;
        private readonly ISysLogger _sysLogger;
        public SpecialTaxSlabBusiness(
            IMapper mapper,
            PayrollDbContext payrollDbContext,
            ISysLogger sysLogger)
        {
            _mapper = mapper;
            _sysLogger = sysLogger;
            _payrollDbContext = payrollDbContext;
        }
        public async Task<IEnumerable<IncomeTaxSlabViewModel>> GetByEmployeeId(long employeeId, long fiscalYearId, AppUser user)
        {
            IEnumerable<IncomeTaxSlabViewModel> list = new List<IncomeTaxSlabViewModel>();
            try
            {
                var slabs = await _payrollDbContext.Payroll_SpecialTaxSlab.Where(
                    item =>
                    item.EmployeeId == employeeId &&
                    item.FiscalYearId == fiscalYearId &&
                    item.CompanyId == user.CompanyId &&
                    item.OrganizationId == user.OrganizationId).ToListAsync();
                if (slabs.Any())
                {
                    list = _mapper.Map<IEnumerable<IncomeTaxSlabViewModel>>(slabs);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "SpecialTaxSlabBusiness", "GetByEmployeeId", user);
            }
            return list;
        }
    }
}
