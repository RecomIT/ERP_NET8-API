using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Shared.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Incentive.QuarterlyIncentive.ViewModel
{
    public class QuarterlyIncentivProcessDetailsViewModel : BaseViewModel2
    {
        [StringLength(50)]
        public string EmployeeCode { get; set; }
        public long? EmployeeId { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal KpiScore { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal DivisionalAndIndividualScore { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal EligibleQuarterlyIncentive { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal IncomeTax { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BankTransferAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WalletTransferAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal COCInWalletTransfer { get; set; }
        [StringLength(100)]
        public string WalletNumber { get; set; }
        [StringLength(50)]
        public string WalletAgent { get; set; }
    }
}
