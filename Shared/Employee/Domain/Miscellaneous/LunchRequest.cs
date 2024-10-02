using Microsoft.EntityFrameworkCore;
using Shared.BaseModels.For_DomainModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Miscellaneous
{
    [Table("HR_LunchRequests"), Index("LunchRequestId", "EmployeeId", "LunchRateId", "RequestDate", "IsLunch", "IsCanceled", "RequestedOn", "RequestedByAdminId", "CompanyId", "OrganizationId", "BranchId", IsUnique = false, Name = "IX_HR_LunchRequests_NonClusteredIndex")]
    public class LunchRequest : BaseModel2
    {
        [Key]
        public long LunchRequestId { get; set; }
        public long EmployeeId { get; set; }
        [Column(TypeName = "date")]
        public DateTime RequestDate { get; set; }
        public bool? IsLunch { get; set; }
        [Column(TypeName = "date")]
        public DateTime RequestedOn { get; set; }
        public int? GuestCount { get; set; }
        public bool? IsCanceled { get; set; }
        public long? RequestedByAdminId { get; set; }
        [ForeignKey("LunchRate")]
        public long LunchRateId { get; set; }
        public LunchRate LunchRate { get; set; }
    }
}
