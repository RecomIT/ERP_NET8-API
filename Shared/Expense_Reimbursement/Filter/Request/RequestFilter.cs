using Shared.OtherModels.Pagination;


namespace Shared.Expense_Reimbursement.Filter.Request
{
    public class RequestFilter : Sortparam
    {
        public long RequestId { get; set; }
        public long EmployeeId { get; set; }
        public string TransactionType { get; set; }
        public string Status { get; set; }
        public Nullable<DateTime> TransactionDate { get; set; }
        public Nullable<DateTime> RequestDate { get; set; }
        public string ReferenceNumber { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public long UserId { get; set; }
        public long StatusChange { get; set; }
        public string StateStatus { get; set; }
        public string CancelRemarks { get; set; }


    }
}
