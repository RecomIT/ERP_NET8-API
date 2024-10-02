using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Payroll.Domain.AIT;
using DAL.Repository.Base.Interface;

namespace DAL.Payroll.Repository.Interface
{
    public interface ITaxDocumentSubmissionRepository : IDapperBaseRepository<TaxDocumentSubmission>
    {
        #region AIT
        Task<TaxDocumentSubmission> GetAITByIdAsync(long id, AppUser user);
        Task<bool> IsAITApprovedAsync(long id, long employeeId, AppUser user);
        Task<bool> IsAITExistAsync(long fiscalYearId, long employeeId, AppUser user);
        Task<ExecutionStatus> SaveAITAsync(TaxDocumentSubmission model, AppUser user);
        #endregion

        #region Tax-Refund
        Task<TaxDocumentSubmission> GetTaxRefundByIdAsync(long id, AppUser user);
        Task<bool> IsTaxRefundApprovedAsync(long id, long employeeId, AppUser user);
        Task<bool> IsTaxRefundExistAsync(long fiscalYearId, long employeeId, AppUser user);
        Task<ExecutionStatus> SaveTaxRefundAsync(TaxDocumentSubmission model, AppUser user);
        #endregion


    }
}
