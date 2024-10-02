using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Employee.DTO.Info
{
    public class DownloadEmployeeInfoCheckedItemsDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Value { get; set; }
        public long BranchId { get; set; }
        public string BranchName { get; set; }
    }
}
