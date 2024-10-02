using DAL.Context.Control_Panel;
using DAL.UnitOfWork.Control_Panel.Interface;
using Microsoft.EntityFrameworkCore;

namespace DAL.UnitOfWork.Control_Panel.Implementation
{
    public class ControlPanelUnitOfWork : IControlPanelUnitOfWork
    {
        private readonly ControlPanelDbContext _dbcontext;
        public ControlPanelUnitOfWork(ControlPanelDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public DbContext Db { get { return _dbcontext; } }
        public void Dispose()
        {

        }
    }
}
