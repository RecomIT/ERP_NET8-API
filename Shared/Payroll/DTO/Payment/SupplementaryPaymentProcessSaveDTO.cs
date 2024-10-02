using Shared.Payroll.Domain.Payment;

namespace Shared.Payroll.DTO.Payment
{
    public class SupplementaryPaymentProcessSaveDTO
    {
        public SupplementaryPaymentProcessSaveDTO()
        {
            AmountInfo = new SupplementaryPaymentAmount();
            TaxInfo = new SupplementaryPaymentTaxInfo();
            TaxDetails = new List<SupplementaryPaymentTaxDetail>();
            TaxSlabs = new List<SupplementaryPaymentTaxSlab>();
        }
        public SupplementaryPaymentAmount AmountInfo { get; set; }
        public SupplementaryPaymentTaxInfo TaxInfo { get; set; }
        public List<SupplementaryPaymentTaxDetail> TaxDetails { get; set; }
        public List<SupplementaryPaymentTaxSlab> TaxSlabs { get; set; }
    }
}
