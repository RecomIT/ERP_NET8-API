using Shared.Employee.DTO.Lunch;
using Shared.Employee.Filter.Miscellaneous;
using Shared.Employee.ViewModel.Miscellaneous;
using Shared.OtherModels.User;
using System.Data;


namespace BLL.Employee.Interface.Miscellaneous
{
    public interface ILunchRequestService
    {
        bool CreateLunchRequest(LunchRequestDTO requestDto, AppUser user);
        bool CancelLunchRequest(long lunchRequestId);
        List<LunchRequestDTO> GetLunchRequestsForDate(DateTime date);
        List<LunchRequestDTO> GetLastFiveRequestsForEmployee(long employeeId);
        LunchRateDTO GetLunchRateForDate(DateTime date);
        bool AddOrUpdateLunchRate(LunchRateDTO rateDto);
        int GetTotalLunchRequestsForDate(DateTime date);
        bool IsLunchExist(DateTime LunchDate, AppUser user);
        Task<IEnumerable<LunchDetailInfoViewModel>> GetLunchDetailsAsync(string date, AppUser user);
        Task<DataTable> LunchRequestReport(LunchRequestSheet_Filter filter, AppUser user);
    }
}
