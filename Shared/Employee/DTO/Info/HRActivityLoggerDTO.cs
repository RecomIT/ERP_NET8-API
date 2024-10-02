using Shared.Employee.Domain.Logger;
using System;

namespace Shared.Employee.DTO.Info
{
    public class HRActivityLoggerDTO : HRActivityLogger
    {
        public DateTime? ApprovedDate { get; set; }
        public string ApprovedBy { get; set; }
        public string UserEmployeeId { get; set; }
        public string UserEmployeeCode { get; set; }
        public string Username { get; set; }
        public string Useremail { get; set; }
        public string UserDesignation { get; set; }
    }
}
