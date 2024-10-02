using BLL.Leave.Interface.Encashment;
using DAL.Context.Leave;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Shared.Leave.ViewModel.Encashment;
using Shared.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Leave.Implementation.Encashment
{
    public class LeaveEncashmentBusiness : ILeaveEncashmentBusiness
    {
        private readonly LeaveModuleDbContext _leaveModuleContext;

        public LeaveEncashmentBusiness(LeaveModuleDbContext leaveModuleContext)
        {
            _leaveModuleContext = leaveModuleContext;
        }

        public async Task<EncashmentBalanceViewModel> GetTotalLeaveBalanceAsync(dynamic filter)
        {
            var httpContextAccessor = new HttpContextAccessor();
            var httpContext = httpContextAccessor.HttpContext;

            var user = UserObjects.UserData(httpContext?.Request);

            long leaveTypeId = filter.LeaveTypeId;
            long employeeId = user.EmployeeId;
            short currentYear = (short)DateTime.Now.Year;

            // Check if the LeaveType exists
            var leaveTypeExists = await _leaveModuleContext.HR_LeaveTypes.AnyAsync(lt => lt.Id == leaveTypeId);
            if (!leaveTypeExists)
            {
                throw new ArgumentException($"LeaveType with Id {leaveTypeId} does not exist.");
            }

            // Fetch total leave balance
            var totalLeave = await _leaveModuleContext.HR_EmployeeLeaveBalance
                .Where(b => b.LeaveTypeId == leaveTypeId && b.EmployeeId == employeeId && b.LeaveYear == currentYear)
                .SumAsync(b => b.TotalLeave);

            // Fetch leave setting
            var leaveSetting = await _leaveModuleContext.HR_LeaveSettings
                .Where(ls => ls.LeaveTypeId == leaveTypeId)
                .FirstOrDefaultAsync();

            var balanceViewModel = new EncashmentBalanceViewModel
            {
                LeaveTypeId = leaveTypeId,
                TotalLeaveBalance = totalLeave ?? 0,
                LeaveSetting = leaveSetting
            };

            return balanceViewModel;
        }
    }
}
