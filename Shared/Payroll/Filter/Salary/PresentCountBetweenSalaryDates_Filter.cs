using System;

namespace Shared.Payroll.Filter.Salary
{
    public class PresentCountBetweenSalaryDates_Filter
    {
        public long EmployeeId { get; set; }
        public bool IsActualDays { get; set; }
        public DateTime? SalaryDate { get; set; }
        public DateTime? FirstDate { get; set; }
        public DateTime? SecondDate { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? TerminationDate { get; set; }

    }
}
