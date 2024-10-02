using System;
using Microsoft.EntityFrameworkCore;

namespace DAL.UnitOfWork.Control_Panel.Interface
{
    public interface IControlPanelUnitOfWork : IDisposable
    {
        DbContext Db { get; }
    }

}
