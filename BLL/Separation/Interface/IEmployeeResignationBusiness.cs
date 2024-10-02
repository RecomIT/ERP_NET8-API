using Shared.OtherModels.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Separation.Interface
{
    public interface IEmployeeResignationBusiness
    {







        Task<object> GetEmployeesAsync(AppUser user);
        Task<object> GetEmployeeDetailsAsync(dynamic filter, AppUser user);
        Task<object> GetEmployeesDetailsAsync(dynamic filter, AppUser user);



        Task<object> SaveEmployeeResignationAsync(dynamic filter, AppUser user);
        Task<object> CancelEmployeeResignationAsync(dynamic filter, AppUser user);
        Task<object> GetEmployeeResignationListAsync(dynamic filter, AppUser user);
        Task<string> DownloadResignationLetterAsync(dynamic filter, AppUser user);



        Task<object> GetUserResignationListAsync(AppUser user);


        // -------------------------------- Supervisor
        Task<object> GetEmployeeResignationListForSupervisorAsync(dynamic filter, AppUser user);





        // --------------------------------------
        Task<object> ApproveEmployeeResignationAsync(dynamic filter, AppUser user);


    }
}
