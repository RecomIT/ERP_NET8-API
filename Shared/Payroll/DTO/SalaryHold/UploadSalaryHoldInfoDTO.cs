using Microsoft.AspNetCore.Http;

namespace Shared.Payroll.DTO.SalaryHold
{
    public class UploadSalaryHoldInfoDTO
    {
        public short SalaryMonth { get; set; }
        public short SalaryYear { get; set; }
        public IFormFile File { get; set; }
    }
}
