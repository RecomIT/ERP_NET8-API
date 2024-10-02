using Shared.OtherModels.Pagination;


namespace Shared.Payroll.Filter.WalletPayment
{
    public class WalletPaymentConfiguration_Filter : Sortparam
    {
        public string InternalDesignationId { get; set; }
        public string IsActive { get; set; }
        public string BaseOfPayment { get; set; }
        //public string ActivationDate { get; set; }
        //public string DeactivationDate { get; set; }
        public string StateStatus { get; set; }
    }
}
