namespace Shared.Payroll.DTO.Salary
{
    public class SalaryReviewSheetInfoDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public long SalaryReviewInfoId { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
        public string JoiningDate { get; set; }
        public string DesignationName { get; set; }
        public string EffectiveFrom { get; set; }
        public string ActivationDate { get; set; }
        public string ArrearDate { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ApprovedDate { get; set; }
        public decimal NewGross { get; set; }
        public decimal OldGross { get; set; }
        public long AllowanceNameId { get; set; }
        public string Allowance { get; set; }
        public decimal Amount { get; set; }
    }
}
