using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Expense_Reimbursement.DTO.Request
{
    public class DeleteRequestDTO
    {
        public long RequestId { get; set; }    
        public long EmployeeId { get; set; }
        public string TransactionType { get; set; }

    }
}
