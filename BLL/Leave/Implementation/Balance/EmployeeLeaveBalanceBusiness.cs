using Dapper;
using System.Data;
using Shared.Services;
using BLL.Base.Interface;
using Shared.OtherModels.User;
using Shared.OtherModels.DataService;
using DAL.DapperObject.Interface;
using DAL.Repository.Leave.Interface;
using BLL.Leave.Interface.Balance;
using Shared.Leave.Filter.Report;
using Shared.Leave.Domain.Balance;
using Shared.Leave.ViewModel.Balance;
using Microsoft.EntityFrameworkCore;
using DAL.Context.Leave;
using DAL.Context.Employee;
using Shared.Leave.DTO.Balance;


namespace BLL.Leave.Implementation.Balance
{
    public class EmployeeLeaveBalanceBusiness : IEmployeeLeaveBalanceBusiness
    {
        private readonly ISysLogger _sysLogger;
        private readonly IDapperData _dapper;
        private readonly ILeaveBalanceRepository _leaveBalanceRepository;
        private readonly ILeaveRequestRepository _leaveRequestRepository;

        private readonly LeaveModuleDbContext _context;
        private readonly EmployeeModuleDbContext _employeeModuleDbContext;



        public EmployeeLeaveBalanceBusiness(
            ISysLogger sysLogger, IDapperData dapper, ILeaveBalanceRepository leaveBalanceRepository, ILeaveRequestRepository leaveRequestRepository, LeaveModuleDbContext context, EmployeeModuleDbContext employeeModuleDbContext)
        {
            _sysLogger = sysLogger;
            _dapper = dapper;
            _leaveBalanceRepository = leaveBalanceRepository;
            _leaveRequestRepository = leaveRequestRepository;
            _context = context;
            _employeeModuleDbContext = employeeModuleDbContext;
        }

        public async Task<IEnumerable<LeaveBalanceViewModel>> GetLeaveBalanceAsync(long employeeId, AppUser user)
        {
            IEnumerable<LeaveBalanceViewModel> list = new List<LeaveBalanceViewModel>();
            try
            {
                list = await _leaveBalanceRepository.GetLeaveBalanceAsync(employeeId, user);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetEmployeeBalanceAsync", user);
            }
            return list;
        }

        public async Task<EmployeeLeaveBalance> GetEmployeeLeaveBalanceOfLeaveTypeAsync(long employeeId, long leaveTypeId, string appliedFromDate, string appliedToDate, AppUser user)
        {
            EmployeeLeaveBalance employeeLeaveBalance = new EmployeeLeaveBalance();
            try
            {
                var query = $@"Select * from HR_EmployeeLeaveBalance						
						Where EmployeeId=@EmployeeId AND LeaveTypeId=@LeaveTypeId 
						AND LeavePeriodStart <= @AppliedFromDate and LeavePeriodEnd >= @AppliedToDate  
						AND CompanyId=@CompanyId AND OrganizationId=@OrganizationId";

                employeeLeaveBalance = await _dapper.SqlQueryFirstAsync<EmployeeLeaveBalance>(user.Database, query, new { EmployeeId = employeeId, LeaveTypeId = leaveTypeId, AppliedFromDate = appliedFromDate, AppliedToDate = appliedToDate, user.CompanyId, user.OrganizationId });
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetEmployeeLeaveBalanceOfLeaveType", user);
            }
            return employeeLeaveBalance;
        }




        //public async 
        public async Task<IEnumerable<EmployeeLeaveBalanceViewModel>> GetEmployeeLeaveBalancesAsync(LeaveBalance_Filter filter, AppUser user)
        {
            IEnumerable<EmployeeLeaveBalanceViewModel> list = new List<EmployeeLeaveBalanceViewModel>();
            try
            {
                var query = $@"Select elb.*,lt.Title 'LeaveTypeName',emp.FullName 'EmployeeName' From HR_EmployeeLeaveBalance elb
			INNER JOIN HR_LeaveTypes lt on elb.LeaveTypeId=lt.Id
			INNER JOIN HR_EmployeeInformation emp on elb.EmployeeId=emp.EmployeeId
			Where 1=1 
			AND (@EmployeeId IS NULL OR @EmployeeId=0 OR elb.EmployeeId =@EmployeeId)
			AND (@LeaveTypeId IS NULL OR @LeaveTypeId=0 OR lt.Id =@LeaveTypeId)
			AND (@LeaveYear IS NULL OR @LeaveYear='' OR elb.LeaveYear =@LeaveYear)
			AND (elb.CompanyId=@CompanyId)
			AND (elb.OrganizationId=@OrganizationId) Order By LT.SerialNo";


                var parameters = DapperParam.AddParams(filter, user);
                parameters.Add("ExecutionFlag", Data.Read);
                list = await _dapper.SqlQueryListAsync<EmployeeLeaveBalanceViewModel>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetEmployeeLeaveBalancesAsync", user);
            }
            return list;
        }




        //Added By Mahbur
        //public async Task<IEnumerable<EmployeeLeaveBalanceViewModel>> GetEmployeeLeaveBalancesAsync(LeaveBalance_Filter filter, AppUser user)
        //{
        //    IEnumerable<EmployeeLeaveBalanceViewModel> list = new List<EmployeeLeaveBalanceViewModel>();
        //    try {
        //        var query = @"

        //                    SELECT 
        //                        elb.EmployeeId,
        //                        emp.FullName AS EmployeeName,
        //                     lt.Id,
        //                        lt.Title AS LeaveTypeName,
        //                        elb.TotalLeave,
        //                     elb.LeavePeriodStart,
        //                     elb.LeavePeriodEnd
        //                    FROM 
        //                        HR_EmployeeLeaveBalance elb
        //                        INNER JOIN HR_LeaveTypes lt ON elb.LeaveTypeId = lt.Id
        //                        INNER JOIN HR_EmployeeInformation emp ON elb.EmployeeId = emp.EmployeeId
        //                    WHERE 
        //                        (elb.LeaveYear = 2024)
        //                        AND elb.CompanyId = 21
        //                        AND elb.OrganizationId = 14
        //                    GROUP BY 
        //                        elb.EmployeeId, emp.FullName, lt.Title, elb.TotalLeave, lt.Id, lt.SerialNo, elb.LeavePeriodStart, elb.LeavePeriodEnd
        //                    ORDER BY 
        //                        elb.EmployeeId ASC, 
        //                        lt.SerialNo;

        //                        ";

        //        var parameters = DapperParam.AddParams(filter, user);
        //        parameters.Add("ExecutionFlag", Data.Read);

        //        list = await _dapper.SqlQueryListAsync<EmployeeLeaveBalanceViewModel>(user.Database, query, parameters, CommandType.Text);
        //    }
        //    catch (Exception ex) {
        //        await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetEmployeeLeaveBalancesAsync", user);
        //    }
        //    return list;
        //}



        public async Task<IEnumerable<Select2Dropdown>> GetEmployeeLeaveBalancesDropdownAsync(long employeeId, AppUser user)
        {
            IEnumerable<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT Id, [Text], [Value]=Id,[Count], [Max]=(CASE WHEN [Max] = 0 THEN [Count] WHEN  [Count] < [Max] THEN [Count] ELSE [Max] END) 
			FROM (Select elb.LeaveTypeId AS 'Id',lt.Title AS 'Text', 
			(elb.TotalLeave-ISNULL(elb.LeaveApplied,0)) as 'Count', ls.MaxDaysLeaveAtATime 'Max' From HR_EmployeeLeaveBalance elb
			INNER JOIN HR_LeaveTypes lt on elb.LeaveTypeId=lt.Id
			INNER JOIN HR_LeaveSetting ls on lt.Id =ls.LeaveTypeId 
			INNER JOIN HR_EmployeeInformation emp on elb.EmployeeId=emp.EmployeeId
			Where 1=1
			AND ( CAST(GETDATE() AS DATE) BETWEEN elb.LeavePeriodStart AND elb.LeavePeriodEnd)
			AND (elb.EmployeeId =@EmployeeId)
			AND (elb.CompanyId=@CompanyId)
			AND (elb.OrganizationId=@OrganizationId)) tbl";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                list = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetEmployeeLeaveBalancesExtensionAsync", user);
            }
            return list;
        }



        public async Task<IEnumerable<Select2Dropdown>> GetEmployeeLeaveBalancesDropdownInEditAsync(long employeeLeaveRequestId, long employeeId, AppUser user)
        {

            var leaveRequestInDb = await _leaveRequestRepository.GetByIdAsync(employeeLeaveRequestId, user);

            List<Select2Dropdown> list = new List<Select2Dropdown>();
            try
            {
                var query = $@"SELECT Id, [Text], [Value]=Id,[Count], [Max]=(CASE WHEN [Max] = 0 THEN [Count]  WHEN  [Count] < [Max] THEN [Count] ELSE [Max] END) 
			FROM (Select elb.LeaveTypeId AS 'Id',lt.Title AS 'Text', 
			(elb.TotalLeave-elb.LeaveApplied) as 'Count', ls.MaxDaysLeaveAtATime 'Max' From HR_EmployeeLeaveBalance elb
			INNER JOIN HR_LeaveTypes lt on elb.LeaveTypeId=lt.Id
			INNER JOIN HR_LeaveSetting ls on lt.Id =ls.LeaveTypeId 
			INNER JOIN HR_EmployeeInformation emp on elb.EmployeeId=emp.EmployeeId
			Where 1=1
			AND ( CAST(GETDATE() AS DATE) BETWEEN elb.LeavePeriodStart AND elb.LeavePeriodEnd)
			AND (elb.EmployeeId =@EmployeeId)
			AND (elb.CompanyId=@CompanyId)
			AND (elb.OrganizationId=@OrganizationId)) tbl";
                var parameters = new DynamicParameters();
                parameters.Add("EmployeeId", employeeId);
                parameters.Add("CompanyId", user.CompanyId);
                parameters.Add("OrganizationId", user.OrganizationId);
                var dataInDb = await _dapper.SqlQueryListAsync<Select2Dropdown>(user.Database, query, parameters, CommandType.Text);

                foreach (var item in dataInDb)
                {
                    if (leaveRequestInDb.LeaveTypeId == Utility.TryParseLong(item.Id))
                    {
                        item.Count = (Utility.TryParseDecimal(item.Count) + leaveRequestInDb.AppliedTotalDays).ToString();
                        item.Max = item.Count;

                        //Utility.TryParseDecimal(item.Count) < Utility.TryParseDecimal(item.Max.ToString()) ? Utility.TryParseDecimal(item.Count.ToString()).ToString() : Utility.TryParseDecimal(item.Max.ToString()).ToString();
                    }
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetEmployeeLeaveBalancesExtensionAsync", user);
            }
            return list;
        }




        public async Task<object> SaveLeaveBalanceAsync(dynamic filter, AppUser user)
        {
            try
            {
                var leaveBalancesToUpdate = new List<EmployeeLeaveBalance>();
                var leaveBalancesToInsert = new List<EmployeeLeaveBalance>();
                var leaveBalancesToDelete = new List<EmployeeLeaveBalance>();

                long employeeId = filter.EmployeeId;
                DateTime leavePeriodStart = filter.LeavePeriodStart;
                DateTime leavePeriodEnd = filter.LeavePeriodEnd;
                short leaveYear = (short)DateTime.Now.Year;

                var existingLeaveBalances = await _context.HR_EmployeeLeaveBalance
                    .Where(lb => lb.EmployeeId == employeeId && lb.LeaveYear == leaveYear)
                    .ToListAsync();

                var newLeaveTypeIds = new HashSet<long>();

                foreach (var leaveBalance in filter.LeaveBalances)
                {
                    long leaveTypeId = leaveBalance.LeaveTypeId;
                    decimal totalLeave = leaveBalance.TotalLeave;

                    newLeaveTypeIds.Add(leaveTypeId);

                    var leaveType = await _context.HR_LeaveTypes.FirstOrDefaultAsync(lt => lt.Id == leaveTypeId);
                    var leaveTypeName = leaveType?.Title;

                    var leaveSetting = await _context.HR_LeaveSettings
                        .FirstOrDefaultAsync(ls => ls.LeaveTypeId == leaveTypeId);

                    if (leaveSetting == null)
                    {
                        throw new Exception($"Leave setting not found for leave type ID: {leaveTypeId}");
                    }

                    var existingLeaveBalance = existingLeaveBalances
                        .FirstOrDefault(lb => lb.LeaveTypeId == leaveTypeId);

                    if (existingLeaveBalance != null)
                    {
                        existingLeaveBalance.LeaveApplied = 0;
                        existingLeaveBalance.LeaveAvailed = 0;
                        existingLeaveBalance.BranchId = user.BranchId;
                        existingLeaveBalance.TotalLeave = totalLeave;
                        existingLeaveBalance.LeavePeriodStart = leavePeriodStart;
                        existingLeaveBalance.LeavePeriodEnd = leavePeriodEnd;
                        existingLeaveBalance.UpdatedBy = user.ActionUserId;
                        existingLeaveBalance.UpdatedDate = DateTime.Now;

                        leaveBalancesToUpdate.Add(existingLeaveBalance);
                    }
                    else
                    {
                        var newLeaveBalance = new EmployeeLeaveBalance
                        {
                            LeaveTypeId = leaveTypeId,
                            LeaveTypeName = leaveTypeName,
                            LeaveSettingId = leaveSetting.LeaveSettingId,
                            TotalLeave = totalLeave,
                            LeaveApplied = 0,
                            LeaveAvailed = 0,
                            EmployeeId = employeeId,
                            LeaveYear = leaveYear,
                            StateStatus = "Approved",
                            IsApproved = true,
                            LeavePeriodStart = leavePeriodStart,
                            LeavePeriodEnd = leavePeriodEnd,
                            YearStatus = "Active",
                            BranchId = user.BranchId,
                            CreatedBy = user.ActionUserId,
                            CreatedDate = DateTime.Now,
                            CompanyId = user.CompanyId,
                            OrganizationId = user.OrganizationId,
                        };

                        leaveBalancesToInsert.Add(newLeaveBalance);
                    }
                }


                var leaveBalancesToDeleteIds = existingLeaveBalances
                    .Where(lb => !newLeaveTypeIds.Contains(lb.LeaveTypeId))
                    .Select(lb => lb.LeaveTypeId)
                    .ToList();


                if (leaveBalancesToInsert.Any())
                {
                    _context.HR_EmployeeLeaveBalance.AddRange(leaveBalancesToInsert);
                }

                if (leaveBalancesToUpdate.Any())
                {
                    _context.HR_EmployeeLeaveBalance.UpdateRange(leaveBalancesToUpdate);
                }

                if (leaveBalancesToDeleteIds.Any())
                {
                    foreach (var leaveTypeId in leaveBalancesToDeleteIds)
                    {
                        var leaveBalanceToDelete = existingLeaveBalances.FirstOrDefault(lb => lb.LeaveTypeId == leaveTypeId);
                        if (leaveBalanceToDelete != null)
                        {
                            _context.HR_EmployeeLeaveBalance.Remove(leaveBalanceToDelete);
                        }
                    }
                }

                await _context.SaveChangesAsync();


                if (leaveBalancesToInsert.Any())
                {
                    return new { Success = true, Message = "Leave balances inserted successfully." };
                }
                else if (leaveBalancesToUpdate.Any())
                {
                    return new { Success = true, Message = "Leave balances updated successfully." };
                }
                else
                {
                    return new { Success = false, Message = "No operation was performed." };
                }
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeResignationBusiness", "SaveLeaveBalanceAsync", user);
                return new { Success = false, Message = "An error occurred while saving leave balances." };
            }
        }

        public async Task<PaginatedList<EmployeeLeaveBalanceDto>> GetEmployeeLeaveBalances(dynamic filter, AppUser user)
        {
            try
            {
                int pageNumber = filter.PageNumber;
                int pageSize = filter.PageSize;

                IQueryable<EmployeeLeaveBalance> query = _context.HR_EmployeeLeaveBalance;

                if (filter != null)
                {
                    foreach (var property in filter.GetType().GetProperties())
                    {
                        var propertyName = property.Name;
                        var propertyValue = property.GetValue(filter, null);

                        if (propertyValue != null && propertyName != "PageNumber" && propertyName != "PageSize")
                        {
                            switch (propertyName)
                            {
                                case "EmployeeId":
                                    long employeeId = (long)propertyValue;
                                    query = query.Where(b => b.EmployeeId == employeeId);
                                    break;
                                case "LeaveTypeId":
                                    long leaveTypeId = (long)propertyValue;
                                    query = query.Where(b => b.LeaveTypeId == leaveTypeId);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                var employeeIds = await query
                    .Where(b => b.YearStatus == "Active")
                    .Select(b => b.EmployeeId)
                    .Distinct()
                    .ToListAsync();

                var employeeNames = await _employeeModuleDbContext.HR_EmployeeInformation
                    .Where(e => employeeIds.Contains(e.EmployeeId))
                    .ToDictionaryAsync(e => e.EmployeeId, e => e.FullName);


                var result = await query
                    .Join(
                        _context.HR_LeaveTypes,
                        ebl => ebl.LeaveTypeId,
                        lt => lt.Id,
                        (ebl, lt) => new { ebl, lt }
                    )
                    .Where(b => b.ebl.YearStatus == "Active")  // Apply the filter on the joined results
                     .GroupBy(b => b.ebl.EmployeeId)
                    .Select(group => new EmployeeLeaveBalanceDto
                    {
                        EmployeeId = group.Key,
                        EmployeeName = employeeNames.ContainsKey(group.Key) ? employeeNames[group.Key] : "",
                        LeaveBalances = group.Select(b => new LeaveBalanceDto
                        {
                            LeaveTypeId = b.ebl.LeaveTypeId,
                            LeaveTypeName = b.lt.Title,
                            Balance = b.ebl.TotalLeave,
                            LeaveYear = b.ebl.LeaveYear,
                            Applied = b.ebl.LeaveApplied,
                            LeavePeriodStart = b.ebl.LeavePeriodStart,
                            LeavePeriodEnd = b.ebl.LeavePeriodEnd
                        }).OrderBy(lb => lb.LeaveTypeId).ToList()
                    }).ToListAsync();


                var paginatedResult = PaginatedList<EmployeeLeaveBalanceDto>.Create(result, pageNumber, pageSize);

                return paginatedResult;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetEmployeeLeaveBalancesAsync", user);
                throw new ApplicationException("An error occurred while fetching employee leave balances.", ex);
            }
        }

        public async Task<List<EmployeeLeaveBalanceDtoForExcel>> GetAllEmployeeLeaveBalances(AppUser user)
        {
            try
            {
                IQueryable<EmployeeLeaveBalance> query = _context.HR_EmployeeLeaveBalance;

                var employeeIds = await query
                    .Where(b => b.YearStatus == "Active")
                    .Select(b => b.EmployeeId)
                    .Distinct()
                    .ToListAsync();

                var employeeNames = await _employeeModuleDbContext.HR_EmployeeInformation
                    .Where(e => employeeIds.Contains(e.EmployeeId))
                    .ToDictionaryAsync(e => e.EmployeeId, e => e.FullName);

                var result = await query
                    .Where(b => b.YearStatus == "Active")
                    .GroupBy(b => b.EmployeeId)
                    .Select(group => new EmployeeLeaveBalanceDtoForExcel
                    {
                        EmployeeCode = group.Key.ToString(),
                        EmployeeName = employeeNames.ContainsKey(group.Key) ? employeeNames[group.Key] : "",
                        LeaveBalances = group.Select(b => new LeaveBalanceDtoForExcel
                        {
                            Type = b.LeaveTypeName,
                            Balance = b.TotalLeave
                        }).OrderBy(lb => lb.Type).ToList()
                    }).ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetAllEmployeeLeaveBalances", user);
                throw new ApplicationException("An error occurred while fetching employee leave balances.", ex);
            }
        }

        public async Task<List<Dictionary<string, object>>> GetAllEmployeeLeaveBalance(AppUser user)
        {
            try
            {
                IQueryable<EmployeeLeaveBalance> query = _context.HR_EmployeeLeaveBalance;

                // Get all distinct active employee IDs
                var employeeIds = await query
                    .Where(b => b.YearStatus == "Active")
                    .Select(b => b.EmployeeId)
                    .Distinct()
                    .ToListAsync();

                // Get employee names and codes by their IDs
                var employeesInfo = await _employeeModuleDbContext.HR_EmployeeInformation
                    .Where(e => employeeIds.Contains(e.EmployeeId))
                    .Select(e => new { e.EmployeeId, e.FullName, e.EmployeeCode })
                    .ToListAsync();

                // Create a dictionary for quick lookup
                var employeeInfoLookup = employeesInfo.ToDictionary(e => e.EmployeeId);

                // Get all distinct leave types
                var leaveTypeIds = await query.Select(e => e.LeaveTypeId).Distinct().ToArrayAsync();

                var leaveTypes =  await (from lv in _context.HR_LeaveTypes
                                 where leaveTypeIds.Contains(lv.Id)
                                 select lv.Title).ToListAsync();

                // Initialize the result list
                var result = new List<Dictionary<string, object>>();

                // Fetch and group the leave balances by employee ID
                var groupedLeaveBalances = await query
                    .Where(b => b.YearStatus == "Active")
                    .GroupBy(b => b.EmployeeId)
                    .ToListAsync();


                foreach (var group in groupedLeaveBalances)
                {
                    if (employeeInfoLookup.ContainsKey(group.Key))
                    {
                        var employeeData = new Dictionary<string, object>()
                        {
                            { "EmployeeCode", employeeInfoLookup[group.Key]?.EmployeeCode ?? group.Key.ToString() },
                            { "EmployeeName", employeeInfoLookup.ContainsKey(group.Key) ? employeeInfoLookup[group.Key].FullName : "" }
                        };
                        // Add leave balances
                        foreach (var leaveType in leaveTypes)
                        {
                            var balance = group.FirstOrDefault(b => b.LeaveTypeName == leaveType)?.TotalLeave ?? 0;
                            employeeData[leaveType] = balance;
                        }

                        result.Add(employeeData);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "EmployeeLeaveBalanceBusiness", "GetAllEmployeeLeaveBalances", user);
                throw new ApplicationException("An error occurred while fetching employee leave balances.", ex);
            }
        }



    }
}
