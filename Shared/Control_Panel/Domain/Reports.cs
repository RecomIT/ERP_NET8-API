using System;
using System.Collections.Generic;
using Shared.BaseModels.For_DomainModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Control_Panel.Domain
{

    [Table("tblReportCategory")]
    public class ReportCategory
    {
        [Key]
        public long Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ICollection<ReportSubCategory> ReportSubCategories { get; set; }

    }

    [Table("tblReportSubCategory")]
    public class ReportSubCategory
    {
        [Key]
        public long Id { get; set; }
        [StringLength(150)]
        public string Name { get; set; }
        [StringLength(100)]
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [StringLength(100)]
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        [ForeignKey("ReportCategory")]
        public long ReportCategoryId { get; set; }
        public ReportCategory ReportCategory { get; set; }
    }

    [Table("tblReportAuthorization")]
    public class ReportAuthorization : BaseModel
    {
        [Key]
        public long Id { get; set; }
        public long ReporCategorytId { get; set; }
        public long ReporSubCategorytId { get; set; }
        [StringLength(100)]
        public string UserId { get; set; }
        public long? EmployeeId { get; set; }
        public long? ModuleId { get; set; }
        public long? MainmenuId { get; set; }
        public long? SubmenuId { get; set; }
    }

    [Table("tblReportSignatories")]
    public class ReportSignatories : BaseModel1
    {
        public long Id { get; set; }
        public long FirstSignerId { get; set; } // employeeId
        public string FirstSignerSignaturePath { get; set; }
        public long SecondSignerId { get; set; } // employeeId
        public string SecondSignerSignaturePath { get; set; }
        public long ThirdSignerId { get; set; } // employeeId
        public string ThirdSignerSignaturePath { get; set; }
        public long ForthSignerId { get; set; } // employeeId
        public string ForthSignerSignaturePath { get; set; }
    }

}
