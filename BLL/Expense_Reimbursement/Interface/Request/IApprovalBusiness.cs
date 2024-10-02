
using Shared.Expense_Reimbursement.DTO.Request;
using Shared.Expense_Reimbursement.Filter.Request;
using Shared.Expense_Reimbursement.ViewModel.Email;
using Shared.Expense_Reimbursement.ViewModel.Request;
using Shared.OtherModels.Response;
using Shared.OtherModels.User;


namespace BLL.Expense_Reimbursement.Interface.Request
{
    public interface IApprovalBusiness
    {
        Task<IEnumerable<RequestCountViewModel>> GetRequestCountAdvanceAsync(long authorityId, AppUser user);
        Task<IEnumerable<RequestCountViewModel>> GetRequestCountAsync(long authorityId, AppUser user);

        Task<DBResponse<RequestViewModel>> GetRequestDataAdvanceAsync(RequestFilter filter, AppUser user);
        Task<DBResponse<RequestViewModel>> GetRequestDataAsync(RequestFilter filter, AppUser user);
        Task<IEnumerable<RequestViewModel>> GetRequestDetailsDataAsync(RequestFilter filter, AppUser user);
        Task<ExecutionStatus> ApprovedRequestAsync(ApprovedDTO model, AppUser user);

        Task<IEnumerable<EmailSendViewModel>> EmailSendAsync(EmailDataViewModel filter, AppUser user);

        //Task<ExecutionStatus> DeleteRequestAsync(RequestFilter model, AppUser user);

        //#region Travels

        //Task<IEnumerable<TravelViewModel>> GetLocationAsync(AppUser user);
        //Task<ExecutionStatus> ValidatorTravelAsync(TravelDTO model, AppUser user);
        //Task<ExecutionStatus> SaveTravelAsync(TravelDTO model, AppUser user);

        //#endregion

        //#region  Entertainment

        //Task<ExecutionStatus> SaveEntertainmentUploadFileAsync(EntertainmentDTO model, AppUser user);
        //Task<ExecutionStatus> ValidationEntertainmentAsync(List<EntertainmentDTO> model, AppUser user);
        //Task<ExecutionStatus> SaveEntertainmentAsync(List<EntertainmentDTO> model, AppUser user);

        //#endregion

        //#region  Conveyance

        //Task<ExecutionStatus> ValidationConveyanceAsync(List<ConveyanceDTO> model, AppUser user);
        //Task<ExecutionStatus> SaveConveyanceAsync(List<ConveyanceDTO> model, AppUser user);

        //#endregion

        //#region  Expat

        //Task<IEnumerable<ExpatViewModel>> GetCompanyNameAsync(AppUser user);
        //Task<IEnumerable<ExpatViewModel>> GetBillTypeAsync(AppUser user);
        //Task<ExecutionStatus> ValidationExpatAsync(List<ExpatDTO> model, AppUser user);
        //Task<ExecutionStatus> SaveExpatAsync(List<ExpatDTO> model, AppUser user);

        //#endregion

        //#region Training

        //Task<ExecutionStatus> ValidatorTrainingAsync(TrainingDTO model, AppUser user);
        //Task<ExecutionStatus> SaveTrainingAsync(TrainingDTO model, AppUser user);

        //#endregion

    }
}
