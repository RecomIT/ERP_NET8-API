
using DAL.Context.Payroll;
using DAL.UnitOfWork.Payroll.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.UnitOfWork.Payroll.Implementation
{
    public class PayrollUnitOfWork : IPayrollUnitOfWork
    {
        private readonly PayrollDbContext _dbcontext;
        public PayrollUnitOfWork(PayrollDbContext dbContext)
        {
            _dbcontext = dbContext;
        }
        public DbContext Db { get { return _dbcontext; } }
        public void Dispose()
        {
        }
    }
}
