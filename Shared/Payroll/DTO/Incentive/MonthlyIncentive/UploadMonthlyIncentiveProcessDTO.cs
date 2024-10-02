using Microsoft.AspNetCore.Http;

namespace Shared.Payroll.DTO.Incentive.MonthlyIncentive
{
    public class UploadMonthlyIncentiveProcessDTO
    {
        public IFormFile ExcelFile { get; set; }
        public short IncentiveMonth { get; set; }
        public short IncentiveYear { get; set; }
        public string BatchNo { get; set; }
    }
}
