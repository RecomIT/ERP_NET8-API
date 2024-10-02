namespace Shared.Payroll.Process.Allowance
{
    public class AllowanceInfo
    {
        public long BasicAllowance { get; set; } = 0;
        public string BasicAllowanceName { get; set; } = "";
        public long HouseRentAllowance { get; set; } = 0;
        public string HouseRentAllowanceName { get; set; } = "";
        public long ConveyanceAllowance { get; set; }= 0;
        public string ConveyanceAllowanceName { get; set; } = "";
        public long MedicalAllowance { get; set; } = 0;
        public string MedicalAllowanceName { get; set; } = "";
        public long LFAAllowance { get; set; } = 0;
        public string LFAAllowanceName { get; set; } = "";
        public long PFAllowance { get; set; } = 0;
        public string PFAllowanceName { get; set; } = "";
    }
}
