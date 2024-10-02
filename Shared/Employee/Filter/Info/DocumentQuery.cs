using Shared.OtherModels.Pagination;
using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.Filter.Info
{
    public class DocumentQuery : Sortparam
    {
        public string DocumentId { get; set; }
        public string EmployeeId { get; set; }
        [StringLength(50)]
        public string EmployeeCode { get; set; }
    }
}
