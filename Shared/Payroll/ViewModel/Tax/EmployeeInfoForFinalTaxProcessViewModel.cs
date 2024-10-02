namespace Shared.Payroll.ViewModel.Tax
{
    public class EmployeeInfoForFinalTaxProcessViewModel
    {
        public long EmployeeId { get; set; }
        public string FullName { get; set; }
        public string EmployeeCode { get; set; }
        public string Branch { get; set; }
        public string Designation { get; set; }
        public string Department { get; set; }
        public string Section { get; set; }
        public string SubSection { get; set; }
        public string Gender { get; set; }
        public string Religion { get; set; }
        public bool IsResidential { get; set; }=false;
        public Nullable<DateTime> LastMonth { get; set; }
        public Nullable<DateTime> TerminationDate { get; set; }
        public bool HasTaxCard { get; set; } = false;
    }
}
