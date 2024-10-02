using Microsoft.AspNetCore.Http;
using Shared.Helpers.ValidationFilters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Filter.Incentive.QuarterlyIncentive
{
    public class UploadExcel
    {
        [Required]
        public short IncentiveYear { get; set; }
        [Required]
        public long IncentiveQuarterNoId { get; set; }
        [Required]
        public string BatchNo { get; set; }
        [Required, AllowedExtensions(new string[] { ".xlsx" })]
        public IFormFile ExcelFile { get; set; }
    }
}
