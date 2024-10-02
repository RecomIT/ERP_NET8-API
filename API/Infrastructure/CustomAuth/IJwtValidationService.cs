using System.Threading.Tasks;

namespace API.Infrastructure.CustomAuth
{
    public interface IJwtValidationService
    {
        Task<JwtValidationResult> ValidateTokenAsync(string token);
    }
}
