namespace Shared.Tools.DTO
{
    public class EasyTaxDTO
    {
        public string EmployeeId { get; set; } = string.Empty;
        public string EmployeeName { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Gender { get; set; } = string.Empty;
        public string TIN { get; set; } = string.Empty;
        public string Region { get; set; } = string.Empty;

        public bool OverAged { get; set; } = false;
        public bool PhysicallyChallenged { get; set; } = false;
        public bool FreedomFighters { get; set; } = false;

        public decimal GrossIncome { get; set; } = 0;

        public SalaryComponent Basic { get; set; } = new();
        public SalaryComponent HouseRent { get; set; } = new();
        public SalaryComponent Medical { get; set; } = new();
        public SalaryComponent Conveyance { get; set; } = new();
        public SalaryComponent Bonus { get; set; } = new();
        public SalaryComponent PF { get; set; } = new();
        public SalaryComponent GF { get; set; } = new();
        public SalaryComponent OtherAllowances { get; set; } = new();

        public decimal ActualInvestmentAmount { get; set; }= 0;
        public decimal AITAmount { get; set; } = 0;
        public decimal RefundAmount { get; set; } = 0;
    }

    public class SalaryComponent

    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public decimal Amount { get; set; } = 0;
        public string Type { get; set; } = string.Empty;     

    }

}
