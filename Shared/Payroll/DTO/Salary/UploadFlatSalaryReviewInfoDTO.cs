
namespace Shared.Payroll.DTO.Salary
{
    public class UploadFlatSalaryReviewInfoDTO
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public Nullable<DateTime> EffectiveDate { get; set; }
        public Nullable<DateTime> ActivationDate { get; set; }
        public Nullable<DateTime> ArrearDate { get; set; }
        public decimal? YearlyCTC { get; set; }
        public decimal? MonthlyFB { get; set; }
        public decimal? MonthlyPF { get; set; }
        public string IncermentReason { get; set; }
        public List<UploadFlatSalaryReviewDetailDTO> SalaryReviewDetails { get; set; }
    }
}
