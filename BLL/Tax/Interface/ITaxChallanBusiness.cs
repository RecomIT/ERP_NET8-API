using Shared.OtherModels.Response;
using Shared.OtherModels.User;
using Shared.Payroll.DTO.Tax;

namespace BLL.Tax.Interface
{
    public interface ITaxChallanBusiness
    {
        Task<ExecutionStatus> UploadTaxChallanAsync(List<TaxChallanDTO> taxChallanDTOs, AppUser user);
        Task<ExecutionStatus> TaxChallanDTO(TaxChallanDTO model, AppUser user);
        Task<ExecutionStatus> BulkSaveAsync(AllEmployeesTaxChallanInsertDTO model, AppUser user);
    }
}
