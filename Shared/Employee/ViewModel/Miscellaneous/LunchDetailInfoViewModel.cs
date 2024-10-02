using Shared.BaseModels.For_ViewModel;

namespace Shared.Employee.ViewModel.Miscellaneous
{
    public class LunchDetailInfoViewModel : BaseViewModel2
    {
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public short? GuestCount { get; set; }
    }
}
