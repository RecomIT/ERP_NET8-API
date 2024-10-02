using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Separation.Models.Domain.Resignation
{

    [Table("HR_ResignationInterviewQuestion")]
    public class ResignationInterviewQuestion : BaseModel
    {
        [Key]
        public long QuestionId { get; set; }
        public string QuestionText { get; set; }
        public bool? IsActive { get; set; }

    }
}
