using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{
    [Table("tblUserAuthTab")]
    public class UserAuthTab : BaseModel1
    {
        [Key]
        public long UATId { get; set; }
        [ForeignKey("UserAuthorization")]
        public long? TaskId { get; set; }
        public UserAuthorization UserAuthorization { get; set; }
        public long SubmenuId { get; set; }
        public long TabId { get; set; }
        [StringLength(100)]
        public string TabName { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Detail { get; set; }
        public bool Delete { get; set; }
        public bool Approval { get; set; }
        public bool Check { get; set; }
        public bool Accept { get; set; }
        public bool Report { get; set; }
        public bool Upload { get; set; }
        public string UserId { get; set; }
        //public long BranchId { get; set; }
    }
}
