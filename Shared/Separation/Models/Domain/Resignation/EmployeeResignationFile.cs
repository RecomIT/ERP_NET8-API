using Shared.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Separation.Models.Domain.Resignation
{
    [Table("HR_EmployeeResignationFiles")]
    public class EmployeeResignationFile : BaseModel1
    {
        [Key]
        public long ResignationFilesId { get; set; }
        public long? ResignationRequestId { get; set; }
        public long? AdminResignationRequestApprovalId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string ActualFileName { get; set; }
    }
}
