using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Separation.ViewModels.User
{
    public class EmployeeInfoViewModel
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string FullName { get; set; }
        public string DesignationName { get; set; }
        public string DepartmentName { get; set; }
        public string GradeName { get; set; }
        public string BranchName { get; set; }
        public string JobType { get; set; }
        public string PhotoPath { get; set; }
        public string Photo { get; set; }
        public string OfficeEmail { get; set; }
        public string PersonalEmailAddress { get; set; }
        public string OfficeMobile { get; set; }
        public string PersonalMobileNo { get; set; }




        [DisplayFormat(DataFormatString = "{0:dd MMM, yyyy}")]
        public DateTime DateOfJoining { get; set; }

        public bool IsActive { get; set; }

        public long SupervisorId { get; set; }
        public string SupervisorName { get; set; }


    }
}
