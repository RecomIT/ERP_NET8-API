using Shared.Models;

namespace Shared.Employee.ViewModel.Info
{
    public class EmployeeInit : BaseViewModel1
    {
        public EmployeeOfficeInfo ProfessionalInfo { get; set; }
        public EmployeePersonalInfo PersonalInfo { get; set; }
        public EmployeePaymentInfo PaymentInfo { get; set; }
    }
}
