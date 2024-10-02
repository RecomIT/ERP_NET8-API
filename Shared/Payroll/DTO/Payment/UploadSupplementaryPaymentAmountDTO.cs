using Microsoft.AspNetCore.Http;

namespace Shared.Payroll.DTO.Payment
{
    public class UploadSupplementaryPaymentAmountDTO
    {
        public long AllowanceId { get; set; }
        public short PaymentMonth { get; set; }
        public short PaymentYear { get; set; }    
        public string PaymentMode { get; set; }
        public bool WithCOC { get; set; }
        public IFormFile File { get; set; }
    }
}
