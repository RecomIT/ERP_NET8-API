using Shared.OtherModels.User;
using Shared.OtherModels.Response;
using Shared.Employee.ViewModel.Locational;
using Shared.Employee.Filter.Locational;
using Shared.Employee.DTO.Locational;

namespace BLL.Employee.Interface.Locational
{
    public interface ICountryBusiness
    {
        Task<IEnumerable<CountryViewModel>> GetCountriesAsync(Country_Filter filter, AppUser user);
        Task<ExecutionStatus> SaveCountryAsync(CountryDTO model, AppUser user);
        Task<ExecutionStatus> ValidateCountryAsync(CountryDTO model, AppUser user);
    }
}
