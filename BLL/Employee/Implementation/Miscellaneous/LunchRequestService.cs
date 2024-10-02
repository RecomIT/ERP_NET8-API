using BLL.Administration.Interface;
using BLL.Base.Interface;
using BLL.Employee.Interface.Miscellaneous;
using DAL.Context.Employee;
using DAL.DapperObject.Interface;
using Shared.Employee.Domain.Miscellaneous;
using Shared.Employee.DTO.Lunch;
using Shared.Employee.Filter.Miscellaneous;
using Shared.Employee.ViewModel.Miscellaneous;
using Shared.OtherModels.User;
using System.Data;



namespace BLL.Employee.Implementation.Miscellaneous
{
    public class LunchRequestservice : ILunchRequestService
    {
        private readonly EmployeeModuleDbContext _context;
        private readonly IDapperData _dapper;
        private readonly IBranchInfoBusiness _branchInfoBusiness;
        private readonly ISysLogger _sysLogger;

        public LunchRequestservice(EmployeeModuleDbContext context, IDapperData dapper, IBranchInfoBusiness branchInfoBusiness, ISysLogger sysLogger)
        {
            _context = context;
            _dapper = dapper;
            _branchInfoBusiness = branchInfoBusiness;
            _sysLogger = sysLogger;
        }

        public bool CreateLunchRequest(LunchRequestDTO requestDto, AppUser user)
        {
            //var existingRequest = _context.HR_LunchRequest
            //    .FirstOrDefault(r => r.EmployeeId == requestDto.EmployeeID && r.RequestDate == requestDto.LunchDate && r.IsCanceled == false);

            //if (existingRequest != null)
            //{
            //    return false;
            //}
            var rate = GetLunchRateForDate(requestDto.LunchDate);
            var newRequest = new LunchRequest
            {
                EmployeeId = user.EmployeeId,
                RequestDate = requestDto.LunchDate,
                IsLunch = true,
                RequestedOn = DateTime.Now,
                GuestCount = 0,
                IsCanceled = false,
                LunchRateId = rate.LunchRateId,
                CreatedBy = user.ActionUserId,
                CreatedDate = DateTime.Now,
                CompanyId = user.CompanyId,
                OrganizationId = user.OrganizationId,
                BranchId = user.BranchId
            };

            _context.HR_LunchRequest.Add(newRequest);
            _context.SaveChanges();
            return true;
        }

        public bool CancelLunchRequest(long lunchRequestId)
        {
            var request = _context.HR_LunchRequest.Find(lunchRequestId);
            if (request == null || request.RequestDate.Date <= DateTime.Now.Date)
            {
                return false;
            }

            request.IsCanceled = true;
            _context.SaveChanges();
            return true;
        }

        public List<LunchRequestDTO> GetLunchRequestsForDate(DateTime date)
        {
            return _context.HR_LunchRequest
                .Where(r => r.RequestDate.Date == date.Date && r.IsLunch == true && r.IsCanceled == false)
                .Select(r => new LunchRequestDTO
                {
                    LunchRequestId = r.LunchRequestId,
                    EmployeeID = r.EmployeeId,
                    LunchDate = r.RequestDate,
                    IsLunch = r.IsLunch ?? false,
                    GuestCount = r.GuestCount ?? 0,
                    IsCanceled = r.IsCanceled ?? false,
                    RequestedOn = r.RequestedOn
                })
                .ToList();
        }

        public List<LunchRequestDTO> GetLastFiveRequestsForEmployee(long employeeId)
        {
            return _context.HR_LunchRequest
                .Where(r => r.EmployeeId == employeeId)
                .OrderByDescending(r => r.RequestDate)
                .Take(5)
                .Select(r => new LunchRequestDTO
                {
                    LunchRequestId = r.LunchRequestId,
                    EmployeeID = r.EmployeeId,
                    LunchDate = r.RequestDate,
                    IsLunch = r.IsLunch ?? false,
                    GuestCount = r.GuestCount ?? 0,
                    IsCanceled = r.IsCanceled ?? false,
                    RequestedOn = r.RequestedOn
                })
                .ToList();
        }

        //public (int YesCount, int NoCount) GetMonthlyRequestCounts(long employeeId, DateTime month)
        //{
        //    var startDate = new DateTime(month.Year, month.Month, 1);
        //    var endDate = startDate.AddMonths(1).AddDays(-1);

        //    var requests = _context.HR_LunchRequest
        //        .Where(r => r.EmployeeId == employeeId && r.RequestDate >= startDate && r.RequestDate <= endDate && r.IsCanceled == false)
        //        .ToList();

        //    return (
        //        YesCount: requests.Count(r => r.IsLunch ?? true),
        //        NoCount: requests.Count(r => !r.IsLunch ?? false)
        //    );
        //}

        //public decimal GetMonthlyLunchCost(long employeeId, DateTime month)
        //{
        //    // Start and end date of the month
        //    var startDate = new DateTime(month.Year, month.Month, 1);
        //    var endDate = startDate.AddMonths(1).AddDays(-1);

        //    // Retrieve all lunch requests for the employee in the given month
        //    var lunchRequests = _context.HR_LunchRequest
        //        .Where(r => r.EmployeeId == employeeId
        //                    && r.RequestDate >= startDate
        //                    && r.RequestDate <= endDate
        //                    && r.IsLunch
        //                    && !r.IsCanceled)
        //        .ToList();

        //    decimal totalCost = 0;

        //    foreach (var request in lunchRequests)
        //    {
        //        // Retrieve the lunch rate for the request date
        //        var rate = GetLunchRateForDate(request.RequestDate);

        //        // Calculate the cost for the request (including guests)
        //        totalCost += rate.Rate.GetValueOrDefault(0) * (1 + request.GuestCount);
        //    }

        //    return totalCost;
        //}


        public LunchRateDTO GetLunchRateForDate(DateTime date)
        {
            var rates = _context.HR_LunchRate
                .Where(r => r.ValidFrom <= date && (r.ValidTo == null || r.ValidTo >= date))
                .OrderByDescending(r => r.ValidFrom)
                .Select(r => new LunchRateDTO
                {
                    LunchRateId = r.LunchRateId,
                    Rate = r.Rate,
                    ValidFrom = r.ValidFrom,
                    ValidTo = r.ValidTo
                })
                .FirstOrDefault() ?? new LunchRateDTO { Rate = 0m };  // Default rate if none found

            return rates;
        }


        public bool AddOrUpdateLunchRate(LunchRateDTO rateDto)
        {
            var existingRate = _context.HR_LunchRate
                .FirstOrDefault(r => r.ValidFrom <= DateTime.Now && (r.ValidTo == null || r.ValidTo >= DateTime.Now));

            if (existingRate != null)
            {
                // Update existing rate
                existingRate.Rate = rateDto.Rate;
                existingRate.ValidFrom = rateDto.ValidFrom;
                existingRate.ValidTo = rateDto.ValidTo;
                _context.HR_LunchRate.Update(existingRate);
            }
            else
            {
                // Add new rate
                var newRate = new LunchRate
                {
                    Rate = rateDto.Rate,
                    ValidFrom = rateDto.ValidFrom,
                    ValidTo = rateDto.ValidTo
                };
                _context.HR_LunchRate.Add(newRate);
            }

            _context.SaveChanges();
            return true;
        }

        public int GetTotalLunchRequestsForDate(DateTime date)
        {
            // Normalize the input date to the same date part without time component
            var normalizedDate = date.Date;

            return _context.HR_LunchRequest
                .Where(r => r.RequestDate.Date == normalizedDate
                             && r.IsLunch == true // Ensure IsLunch is true
                             && r.IsCanceled != true) // Ensure IsCanceled is false
                .Sum(r => 1 + (r.GuestCount ?? 0)); // 1 for the employee + number of guests (default to 0 if null)
        }



        //private bool IsHolidayOrWeekend(DateTime date)
        //{
        //    var holiday = _context.Holidays
        //         .Where(r => r.StartDate == employeeId && r.RequestDate >= startDate && r.RequestDate <= endDate && !r.IsCanceled)
        //         .ToList();

        //    if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
        //    {
        //        return true;
        //    }
        //    return false;
        //}


        private bool IsBackdatedRequest(DateTime requestDate)
        {
            return requestDate.Date < DateTime.Now.Date || (requestDate.Date == DateTime.Now.Date && DateTime.Now.TimeOfDay > new TimeSpan(11, 0, 0));
        }

        public bool IsLunchExist(DateTime LunchDate, AppUser user)
        {
            var existingRequest = _context.HR_LunchRequest
                .FirstOrDefault(r => r.EmployeeId == user.EmployeeId && r.RequestDate == LunchDate && r.IsCanceled == false);

            return existingRequest != null;
        }
        public async Task<IEnumerable<LunchDetailInfoViewModel>> GetLunchDetailsAsync(string date, AppUser user)
        {
            IEnumerable<LunchDetailInfoViewModel> list = new List<LunchDetailInfoViewModel>();
            try
            {
                var query = $@"SELECT EMP.EmployeeCode,EMP.FullName,EMP.BranchId,Branch='',LREQ.RequestDate,LREQ.GuestCount,LREQ.CreatedDate 
FROM HR_LunchRequests LREQ
INNER JOIN HR_LunchRate LR ON LREQ.LunchRateId= LR.LunchRateId
INNER JOIN HR_EmployeeInformation EMP ON LREQ.EmployeeId = EMP.EmployeeId
Where 1=1
AND CAST(LREQ.RequestDate AS DATE) = CAST(@Date As Date)
AND LREQ.CompanyId=@CompanyId
AND LREQ.OrganizationId=@OrganizationId";

                list = await _dapper.SqlQueryListAsync<LunchDetailInfoViewModel>(user.Database, query, new
                {
                    Date = date,
                    CompanyId = user.CompanyId,
                    OrganizationId = user.OrganizationId
                });

                if (list.Any() && list != null)
                {
                    var branches = await _branchInfoBusiness.GetBranchsAsync("", user);
                    if (branches != null && branches.Any())
                    {
                        foreach (var item in list) {
                            var branch = branches.FirstOrDefault(i => i.BranchId == item.BranchId);
                            if(branch != null)
                            {
                                item.BranchName = branch.BranchName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return list;
        }

        public async Task<DataTable> LunchRequestReport(LunchRequestSheet_Filter filter, AppUser user)
        {
            DataTable dataTable = new DataTable();
            try
            {
                var sp_name = "sp_HR_GetLunchRequestReport";
                Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
                keyValuePairs.Add("FromDate", filter.FromDate);
                keyValuePairs.Add("ToDate", filter.ToDate);
                keyValuePairs.Add("BranchId", user.BranchId.ToString());
                keyValuePairs.Add("CompanyId", user.CompanyId.ToString());
                keyValuePairs.Add("OrganizationId", user.OrganizationId.ToString());
                dataTable = await _dapper.SqlDataTable(user.Database, sp_name, keyValuePairs, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                await _sysLogger.SaveHRMSException(ex, user.Database, "LunchRequestservice", "LunchRequestReport", user);
            }
            return dataTable;
        }
    }
}
