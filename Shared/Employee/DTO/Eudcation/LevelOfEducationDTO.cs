using System.ComponentModel.DataAnnotations;

namespace Shared.Employee.DTO.Eudcation
{
    public class LevelOfEducationDTO
    {
        public int LevelOfEducationId { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(150)]
        public string NameInBengali { get; set; }
    }
}
