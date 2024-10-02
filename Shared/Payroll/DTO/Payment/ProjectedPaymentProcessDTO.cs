using System.Collections.Generic;

namespace Shared.Payroll.DTO.Payment
{
    public class ProjectedPaymentProcessDTO
    {
        public EmployeeProjectedAllowanceProcessInfoDTO info { get; set; }
        public List<EmployeeProjectedPaymentDTO> payments { get; set; }
        public List<ProjectedPaymentTransactionMode> projectedPaymentsMode { get; set; }
    }
}
