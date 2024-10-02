using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Employee.Domain.Training
{
    public class HR_TrainingRequest : BaseModel3
    {
        [Key]
        public int TrainingRequestId { get; set; }
        public int TrainingID { get; set; }

        [ForeignKey("TrainingID")] // Define foreign key relationship
        public HR_Training Training { get; set; } // Navigation property


        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime RequestDate { get; set; }
        public string RequestRemarks { get; set; }

        public long SupervisorId { get; set; }
        public string SupervisorStatus { get; set; }
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
    }
}
