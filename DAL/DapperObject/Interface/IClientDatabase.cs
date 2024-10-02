using Shared.Control_Panel.ViewModels;

namespace DAL.DapperObject.Interface
{
    public interface IClientDatabase
    {
        void ClietDbs();
        string GetDatabaseName(long organizationId);
        ClientDB GetClientObj(long organizationId);
        string GetDatabaseName(string username);
        ClientDB GetClientObj(string username);
    }
}
