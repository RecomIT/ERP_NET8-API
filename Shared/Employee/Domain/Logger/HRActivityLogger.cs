using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Shared.Employee.Domain.Logger
{
    [Table("HR_ActivityLogger"), Index("UserId", "EmployeeId", "CompanyId", "OrganizationId", IsUnique = false, Name = "IX_HR_ActivityLogger_NonClusteredIndex")]
    public class HRActivityLogger
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string EMPCode { get; set; }
        [StringLength(50)]
        public string EmployeeId { get; set; }
        [StringLength(200)]
        public string Controller { get; set; }
        public string ActionMethod { get; set; }
        // Log In, Log Out, Insert, Update, Delete
        public string ActionName { get; set; }
        public string ActionDetails { get; set; }
        public string ImpactTables { get; set; }
        public string PreviousValue { get; set; }
        public string PresentValue { get; set; }
        public string PK { get; set; }
        public string UpdatedValue { get; set; }
        [StringLength(100)]
        public string LogInIP { get; set; }
        [StringLength(100)]
        public string PCName { get; set; }
        [StringLength(100)]
        public string MACID { get; set; }
        [StringLength(100)]
        public string DeviceType { get; set; }
        [StringLength(100)]
        public string DeviceModel { get; set; }
        public long OrganizationId { get; set; }
        public long CompanyId { get; set; }
        public long BranchId { get; set; }
        [StringLength(100)]
        public string UserId { get; set; }
        [StringLength(100)]
        public string SessionId { get; set; }
        public DateTime? CreatedDate { get; set; }

    }
}
