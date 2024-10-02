using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shared.Overtime.ViewModel
{
    public class OvertimeProcessViewModel
    {

        [Required]
        public int Year { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public DateTime ProcessDate { get; set; }

    }
}
