using Microsoft.AspNetCore.Http;
using Shared.Expense_Reimbursement.DTO.Request;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Expense_Reimbursement.ViewModel.Request
{
    public class ApprovedDTO
    {
        public long RequestId { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public long EmployeeId { get; set; }
        public string ReferenceNumber { get; set; }
        [StringLength(100)]
        public string StateStatus { get; set; }
        public bool IsApproved { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> RequestDate { get; set; }
        [StringLength(250)]
        public string SpendMode { get; set; }
        [StringLength(100)]
        public string AccountStatus { get; set; }
        [StringLength(250)]
        public string Purpose { get; set; }
        [StringLength(350)]
        public string Description { get; set; }
        public string CompanyName { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Cost { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AdvanceAmount { get; set; }
        [StringLength(500)]
        public string CommentsUser { get; set; }
        [StringLength(500)]
        public string CommentsAccount { get; set; }

        public string Flag { get; set; }
        public string FileName { get; set; }
        [StringLength(200)]
        public string ActualFileName { get; set; }
        [StringLength(50)]
        public string FileSize { get; set; }
        [StringLength(300)]
        public string FilePath { get; set; }
        [StringLength(100)]
        public string FileFormat { get; set; }
        public IFormFile File { get; set; }



        #region Conveyance

        public long ConveyanceDetailId { get; set; }
        public string Destination { get; set; }
        public string Mode { get; set; }

        #endregion

        #region Travel

        [Column(TypeName = "date")]
        public Nullable<DateTime> FromDate { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> ToDate { get; set; }
        [StringLength(200)]
        public string Location { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? AccommodationCosts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SubsistenceCosts { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? OtherCosts { get; set; }
        public string Transportation { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TransportationCosts { get; set; }

        #endregion

        #region Entertainment
        public long EntertainmentDetailId { get; set; }
        public string Item { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Quantity { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Price { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }
        public string Entertainments { get; set; }

        #endregion

        #region Expat
        public long ExpatDetailId { get; set; }
        [StringLength(300)]
        public string BillType { get; set; }
        [StringLength(500)]
        public string Expat { get; set; }

        #endregion

        #region Training

        public string InstitutionName { get; set; }
        public string Course { get; set; }
        [Column(TypeName = "date")]
        public Nullable<DateTime> AdmissionDate { get; set; }
        public string Duration { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal? TrainingCosts { get; set; }

        #endregion


        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DepartmentName { get; set; }
        public string DesignationName { get; set; }
    }
}
