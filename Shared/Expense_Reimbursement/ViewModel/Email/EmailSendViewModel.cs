using Shared.Models;

namespace Shared.Expense_Reimbursement.ViewModel.Email
{
    public class EmailSendViewModel : BaseViewModel2
    {
        public string EmailCC1 { get; set; }
        public string EmailCC1Name { get; set; }
        public string EmailCC2 { get; set; }
        public string EmailCC2Name { get; set; }
        public string EmailCC3 { get; set; }
        public string EmailCC3Name { get; set; }
        public string EmailBCC1 { get; set; }
        public string EmailBCC1Name { get; set; }
        public string EmailBCC2 { get; set; }
        public string EmailBCC2Name { get; set; }
        public string EmailTo { get; set; }
        public string EmailToName { get; set; }
        public string EmployeeId { get; set; }
        public string JsonEmailCCBCC { get; set; }
        public string Json { get; set; }
        public string EmpDtls { get; set; }
        public string UserEmail { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public string Msg { get; set; }

    }
}
