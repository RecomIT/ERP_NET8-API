using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Shared.OtherModels.User;

namespace BLL.PF.Interface
{
    public interface IWundermanPFServiceBusiness
    {
        Task<decimal> GetEmployeeLoanDeductionAmountAsync(string employeeCode, int salaryYear, int salaryMonth, AppUser user);
    }
}
