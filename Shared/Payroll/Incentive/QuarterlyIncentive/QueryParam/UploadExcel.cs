using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Payroll.Incentive.QuarterlyIncentive.QueryParam
{
    public class UploadExcel
    {
        public IFormFile ExcelFile { get; set; }
    }
}
