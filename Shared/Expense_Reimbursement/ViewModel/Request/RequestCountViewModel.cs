using Microsoft.AspNetCore.Http;
using Shared.Expense_Reimbursement.DTO.Request;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Expense_Reimbursement.ViewModel.Request
{
    public class RequestCountViewModel
    {
        public long Request { get; set; }
        public string Type { get; set; }    


    }
}
