using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Employee.ViewModel.Stage
{
    public class EmploymentStageVM
    {
        public EmploymentStageInfoVM EmploymentStageInfo { get; set; }
        public List<EmploymentStageDetailsVM> EmploymentStageDetails { get; set; }
    }
}
