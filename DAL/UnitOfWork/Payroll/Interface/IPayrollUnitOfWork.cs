using Microsoft.EntityFrameworkCore;
using System;

namespace DAL.UnitOfWork.Payroll.Interface
{
    public interface IPayrollUnitOfWork : IDisposable
    {
        DbContext Db { get; }
    }
}
