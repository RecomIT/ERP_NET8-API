using System;


namespace Shared.Employee.ViewModel.Training
{
    public class HR_Training_ViewModel
    {
        public int TrainingID { get; set; }
        public string TrainingName { get; set; }
        public string Venue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int CalendarYear { get; set; }
        public string Institute { get; set; }
        public string TrainingType { get; set; }
        public string Remarks { get; set; }
        public string Objective { get; set; }
        public bool IsEnrolled { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRejected { get; set; }
    }
}
