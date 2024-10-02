using Shared.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblCompanyAuthorization")]
    public class CompanyAuthorization : BaseModel
    {
        [Key]
        public long ComAuthId { get; set; }
        public long ApplicationId { get; set; }
        public long MainmenuId { get; set; }
        [StringLength(100)]
        public string MainMenuName { get; set; }
        public long ModuleId { get; set; }
        [StringLength(100)]
        public string ModuleName { get; set; }
    }
}
